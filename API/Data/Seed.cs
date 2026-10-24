using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public static class Seed
{
    /// <summary>Adds a few demo users to an empty database.</summary>
    public static async Task SeedUsersAsync(DataContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        context.Users.AddRange(
            new AppUser { UserName = "bob" },
            new AppUser { UserName = "tom" },
            new AppUser { UserName = "jane" });

        await context.SaveChangesAsync();
    }
}
