using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Auth;

[ExcludeFromCodeCoverage]
public class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = "Publiseo";
    public string Audience { get; set; } = "Publiseo";
    public int AccessTokenMinutes { get; set; } = 60;
}
