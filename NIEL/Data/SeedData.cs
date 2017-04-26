using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Linq;

namespace NIEL.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext  context)
        {
            context.Database.EnsureCreated();

            string[] roles = new string[] { "Organizer", "Team Player", "Contributor", "Observer" };

            foreach (string role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                if (!context.Roles.Any(r => r.Name == role))
                {
                    var result = roleStore.CreateAsync(new IdentityRole(role) {NormalizedName = role.ToUpper() }).Result;
                }
            }
            context.SaveChanges();
        }
    }
}
