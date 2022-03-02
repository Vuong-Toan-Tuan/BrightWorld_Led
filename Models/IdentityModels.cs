using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BrightWorld_LED.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new ApplicationDbContextDataInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<BrandModel> Brands { get; set; }
        public DbSet<CartModel> Carts { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderDetailModel> OrderDetails { get; set; }
        
    }

    public class ApplicationDbContextDataInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);

            context.Roles.Add(new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = RoleString.ADMIN
            });
            context.Roles.Add(new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = RoleString.USER
            });

            context.Categories.AddRange(new List<CategoryModel>
            {
                new CategoryModel
                {
                    CategoryId = Guid.NewGuid(),
                    CategoryName = "Đèn Led",
                    CreatedOn = DateTime.Now
                },
                new CategoryModel
                {
                    CategoryId = Guid.NewGuid(),
                    CategoryName = "Đèn sợi đốt",
                    CreatedOn = DateTime.Now
                }
            });

            context.Brands.AddRange(new List<BrandModel>
            {
                new BrandModel
                {
                    BrandId = Guid.NewGuid(),
                    BrandName = "Brand 1",
                    BrandImage = "Image 1",
                    Description = "abc 1 ..."
                },
                new BrandModel
                {
                    BrandId = Guid.NewGuid(),
                    BrandName = "Đèn sợi đốt",
                    BrandImage = "Image 2",
                    Description = "abc 2 ..."
                }
            });
            

        }
    } 
}

namespace System
{
    public static class RoleString
    {
        public const string ADMIN = "Admin";
        public const string USER = "User";
    }
}