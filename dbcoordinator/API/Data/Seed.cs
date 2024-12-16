using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(DataContext context)
    {

        if (!context.Users.Any())
        {
            for (int index = 1; index <= 3; index++)
            {
                var user = new User();
                user.Nume = "Utilizator " + index;
                user.Email = "email" + index + "@distributedmachine.ro";
                user.Parola = "Parola" + index;
                context.Add(user);
            }

            await context.SaveChangesAsync();
        }

    }

}
