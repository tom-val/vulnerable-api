using Microsoft.EntityFrameworkCore;

namespace VulnerableAPI.Database;

public class CreatedDbContext
{
    public  readonly UserDbContext Context;
    public CreatedDbContext(UserDbContext context)
    {
        Context = context;
        //TODO create and migrate while creating new user?
        Context.Database.Migrate();
    }
}
