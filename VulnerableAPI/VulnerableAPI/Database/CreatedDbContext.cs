using Microsoft.EntityFrameworkCore;

namespace VulnerableAPI.Database;

public class CreatedDbContext
{
    public  readonly DatabaseContext Context;
    public CreatedDbContext(DatabaseContext context)
    {
        Context = context;
        //TODO create and migrate while creating new user?
        Context.Database.Migrate();
    }
}
