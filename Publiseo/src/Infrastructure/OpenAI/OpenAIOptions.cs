using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.OpenAI;

[ExcludeFromCodeCoverage]
public class OpenAIOptions
{
    public const string SectionName = "OpenAI";

    /// <summary>Chave da API OpenAI (ex.: sk-...).</summary>
    public string ApiKey { get; set; } = string.Empty;
    /// <summary>Modelo a usar (ex.: gpt-4o, gpt-4-turbo).</summary>
    public string Model { get; set; } = "gpt-4o";
}
