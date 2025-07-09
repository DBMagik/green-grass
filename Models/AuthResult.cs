using GreenGrass.Models;

public enum AuthStatus { Login, Registration, Failure }

public class AuthResult
{
    public Login? Login { get; set; }
    public AuthStatus Status { get; set; }
    public bool Succeeded => Status == AuthStatus.Login || Status == AuthStatus.Registration;
}