namespace MobiGate.Application.Auth.Login;

public record LoginResult(string Token, DateTime ExpiresAt);
