using DataAccessLayer.Data;
using DataAccessLayer.Models;

namespace MyWebAPI
{
    public class Seed
    {
        private readonly DataContext _context;

        public Seed(DataContext context)
        {
            _context = context;
        }

        public void SeedDataContext()
        {
            if (!_context.Roles.Any())
            {
                // Seed roles
                var roles = new List<Roles>
            {
                new Roles { Name = "Reader" },
                new Roles { Name = "Writer" },
                new Roles { Name = "Editor" }
            };

                _context.Roles.AddRange(roles);
                _context.SaveChanges();
            }

            if (!_context.Users.Any())
            {
                // Seed admin user
                var adminUser = new Users
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin") // You should hash the password
                };

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                // Assign roles to admin user
                var readerRole = _context.Roles.Single(r => r.Name == "Reader");
                var writerRole = _context.Roles.Single(r => r.Name == "Writer");
                var editorRole = _context.Roles.Single(r => r.Name == "Editor");

                var adminRoles = new List<UserRoles>
                {
                    new UserRoles { UserId = adminUser.Id, RoleId = readerRole.Id },
                    new UserRoles { UserId = adminUser.Id, RoleId = writerRole.Id },
                    new UserRoles { UserId = adminUser.Id, RoleId = editorRole.Id }
                };

                _context.UserRoles.AddRange(adminRoles);
                _context.SaveChanges();
            }
        }
    }

}
