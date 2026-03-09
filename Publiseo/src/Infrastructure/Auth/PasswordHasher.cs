using Application.Auth.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Auth;

[ExcludeFromCodeCoverage]
public sealed class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

    public bool Verify(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
}
