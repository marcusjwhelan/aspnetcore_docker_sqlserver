using System.Linq;
using Commander.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Commander.Models
{
    public static class PrepDB
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            // get the scope of our db context
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<CommanderContext>());
            }
        }

        public static void SeedData(CommanderContext context)
        {
            // Applies the database schema
            System.Console.WriteLine("Applying Migrations...");
            context.Database.Migrate();
            // add some if no data
            if (!context.Commands.Any())
            {
                System.Console.WriteLine("Seeding data...");
                context.Commands.AddRange(
                    new Command() {HowTo = "a",Line = "a",Platform = "a"},
                    new Command() {HowTo = "b",Line = "b",Platform = "b"},
                    new Command() {HowTo = "c",Line = "c",Platform = "c"},
                    new Command() {HowTo = "d",Line = "d",Platform = "d"}
                );
                context.SaveChanges();
            }
            else
            {
                System.Console.WriteLine("Already have data.");
            }
        }
    }
}