using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.GoDaddy;

[ExcludeFromCodeCoverage]
public class GoDaddyOptions
{
    public const string SectionName = "GoDaddy";

    public string ApiKey { get; set; } = string.Empty;
    public string ApiSecret { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.ote-godaddy.com";
}
