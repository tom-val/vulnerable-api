using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

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

app.Run();
