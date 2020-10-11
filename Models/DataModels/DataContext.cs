using IFTurist.Models.TourModel;
using IFTurist.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace IFTurist.Models.DataModels
{
    public class DataContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<AboutTour> AboutTours { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "admin@gmail.com";
            string adminPassword = "admin123";
            string turistEmail = "user@gmail.com";
            string turistPassword = "user123";

            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };

            User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };
            User turistUser = new User { Id = 2, Email = turistEmail, Password = turistPassword, RoleId = userRole.Id };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser, turistUser });

            base.OnModelCreating(modelBuilder);
        }
    }
}
