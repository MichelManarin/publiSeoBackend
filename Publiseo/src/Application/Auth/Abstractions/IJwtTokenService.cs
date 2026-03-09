namespace Application.Auth.Abstractions;

public interface IJwtTokenService
{
    string GenerateAccessToken(Guid usuarioId, string login);
}
