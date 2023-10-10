using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VulnerableAPI;
using VulnerableAPI.Database;
using VulnerableAPI.Options;
using VulnerableAPI.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.AddAWSProvider();
});

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v2",
        Title = "Ledgers API",
        Description = "Very secure ledgers API"
    });
    c.SchemaFilter<SwaggerSkipPropertyFilter>();
});

var validator = new JwtSecurityTokenHandler();
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
};
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = false,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        SignatureValidator = (token, parameters) =>
        {
            var jwt = new JwtSecurityToken(token);
            if (jwt.SignatureAlgorithm is SecurityAlgorithms.None)
            {
                return jwt;
            }

            if (jwt.SignatureAlgorithm is not SecurityAlgorithms.HmacSha256) return null;
            try
            {
                var claim = validator.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                return claim is not null ? validatedToken : null;
            }
            catch
            {
                return null;
            }
        }
    };
});
builder.Services.Configure<SqliteOptions>(builder.Configuration.GetSection(SqliteOptions.ConfigSection));
builder.Services.AddHttpContextAccessor();
builder.Services.AddEntityFrameworkSqlite()
    .AddDbContext<UserDbContext>()
    .AddDbContext<AdminDbContext>();
builder.Services.AddDirectoryBrowser();

var app = builder.Build();

var fileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "files"));
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = fileProvider, RequestPath = "/files"
});
app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = fileProvider,
    RequestPath = "/files"
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.InjectJavascript("/files/swag/custom.js");
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Use(async (context, next) =>
{
    var token = await context.GetTokenAsync("access_token");
    if (token is not null)
    {
        var jwt = new JwtSecurityToken(token);
        if (jwt.SignatureAlgorithm is SecurityAlgorithms.None)
        {
            context.Response.Headers.Add("BrokenAuthenticationFlag", builder.Configuration["Flags:BrokenAuthentication"]);
        }
    }
    await next.Invoke();
});

var requestsCounts = new ConcurrentDictionary<string, FixedInterval>();
app.Use(async (context, next) =>
{
    if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
    {
        var user = context.User.GetEmail();
        var fixedWindow = requestsCounts.GetOrAdd(user, (x) => new FixedInterval(TimeSpan.FromSeconds(5), 20));
        var requestConforms = fixedWindow.Conforms();

        if (!requestConforms)
        {
            context.Response.Headers.Add("UnrestrictedConsumptionNoRateLimiting", builder.Configuration["Flags:UnrestrictedConsumptionNoRateLimiting"]);
        }
    }

    await next.Invoke();
});

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AdminDbContext>();
context.Database.Migrate();

var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Starting application");

app.Run();
