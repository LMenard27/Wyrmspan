using Microsoft.EntityFrameworkCore;
using Wyrmspan.Models;

namespace Wyrmspan.Services {
    public class AuthService
    {
        private readonly AppDbContext _db;

        public AuthService(AppDbContext db)
        {
            _db = db;
        }

        public User Register(string username, string password, string pfp)
        {
            var user = new User
            {
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Pfp = pfp
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            return user;
        }

        public User ValidateUser(string username, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == username);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
                return user;

            return null;
        }
    }
}