using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Infrastructure.EntityConfiguration;

internal sealed class PalavrasChaveJsonConverter : ValueConverter<List<string>, string>
{
    private static readonly JsonSerializerOptions Options = new();

    public PalavrasChaveJsonConverter()
        : base(
            v => JsonSerializer.Serialize(v, Options),
            v => JsonSerializer.Deserialize<List<string>>(v, Options) ?? new List<string>())
    {
    }
}
