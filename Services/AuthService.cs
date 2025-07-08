using GreenGrass.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GreenGrass.Services
{
    public interface IAuthService
    {
        Task<User?> AuthenticateAsync(string usernameOrEmail, string password);
        Task<User?> RegisterAsync(string username, string email, string password, string firstName, string lastName);
        Task<bool> UserExistsAsync(string usernameOrEmail);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateAsync(string usernameOrEmail, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);

            if (user == null || !VerifyPassword(password, user.PasswordHash))
                return null;

            // Update last login
            user.LastLogin = DateTime.Now;
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> RegisterAsync(string username, string email, string password, string firstName, string lastName)
        {
            // Check if user already exists
            if (await UserExistsAsync(username) || await UserExistsAsync(email))
                return null;

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                PasswordHash = HashPassword(password),
                FirstName = firstName,
                LastName = lastName,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExistsAsync(string usernameOrEmail)
        {
            return await _context.Users
                .AnyAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "GreenGrassSalt"));
            return Convert.ToBase64String(hashedBytes);
        }

        public bool VerifyPassword(string password, string hash)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hash;
        }
    }
}