using GreenGrass.Data;
using GreenGrass.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GreenGrass.Services
{
    public enum AuthStatus { Login, Registration, Failure }

    public class AuthResult
    {
        public Login? Login { get; set; }
        public AuthStatus Status { get; set; }
    }

    public interface IAuthService
    {
        Task<AuthResult> LoginOrCreateAsync(string email, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AuthResult> LoginOrCreateAsync(string email, string password)
        {
            try
            {
                var existingLogin = await _context.Logins
                    .FirstOrDefaultAsync(l => l.Email == email);

                if (existingLogin != null)
                {
                    if (VerifyPassword(password, existingLogin.PasswordHash, existingLogin.Salt))
                    {
                        return new AuthResult { Login = existingLogin, Status = AuthStatus.Login };
                    }
                    else
                    {
                        return new AuthResult { Status = AuthStatus.Failure };
                    }
                }
                else
                {
                    var salt = GenerateSalt();
                    var passwordHash = HashPassword(password, salt);

                    var newLogin = new Login
                    {
                        Id = Guid.NewGuid(),
                        Email = email,
                        PasswordHash = passwordHash,
                        Salt = salt
                    };

                    _context.Logins.Add(newLogin);
                    await _context.SaveChangesAsync();

                    return new AuthResult { Login = newLogin, Status = AuthStatus.Registration };
                }
            }
            catch (DbUpdateException)
            {
                return new AuthResult { Status = AuthStatus.Failure };
            }
            catch
            {
                return new AuthResult { Status = AuthStatus.Failure };
            }
        }

        private byte[] GenerateSalt()
        {
            return RandomNumberGenerator.GetBytes(32);
        }

        private byte[] HashPassword(string password, byte[] salt)
        {
            const int iterations = 10000;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(64);
            }
        }

        private bool VerifyPassword(string password, byte[] storedHash, byte[] salt)
        {
            var computedHash = HashPassword(password, salt);
            return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
        }
    }
}