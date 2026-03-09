using Application.Data;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Infrastructure.Data;

[ExcludeFromCodeCoverage]
public sealed class ComplexQueryExecutor : IComplexQueryExecutor
{
    private readonly PubliseoDbContext _context;
    private static readonly string ScriptsBasePath = Path.Combine(AppContext.BaseDirectory, "SqlScripts");

    public ComplexQueryExecutor(PubliseoDbContext context) => _context = context;

    public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, CancellationToken cancellationToken = default, params object[] parameters) where T : class
    {
        var query = _context.Database.SqlQueryRaw<T>(sql, parameters);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<int> ExecuteAsync(string sql, CancellationToken cancellationToken = default, params object[] parameters)
    {
        return await _context.Database.ExecuteSqlRawAsync(sql, cancellationToken, parameters);
    }

    public async Task<IReadOnlyList<T>> QueryFromScriptAsync<T>(string scriptName, CancellationToken cancellationToken = default, params object[] parameters) where T : class
    {
        var sql = await LoadScriptAsync(scriptName, cancellationToken);
        return await QueryAsync<T>(sql, cancellationToken, parameters);
    }

    public async Task<int> ExecuteFromScriptAsync(string scriptName, CancellationToken cancellationToken = default, params object[] parameters)
    {
        var sql = await LoadScriptAsync(scriptName, cancellationToken);
        return await ExecuteAsync(sql, cancellationToken, parameters);
    }

    private static async Task<string> LoadScriptAsync(string scriptName, CancellationToken cancellationToken)
    {
        if (!scriptName.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
            scriptName += ".sql";

        // 1) Pasta SqlScripts ao lado do executável (deploy)
        var filePath = Path.Combine(ScriptsBasePath, scriptName);
        if (File.Exists(filePath))
            return await File.ReadAllTextAsync(filePath, cancellationToken);

        // 2) Embedded resource no assembly da Infrastructure (padrão: Infrastructure.SqlScripts.NomeDoScript.sql)
        var assembly = typeof(ComplexQueryExecutor).Assembly;
        var resourceName = assembly.GetManifestResourceNames()
            .FirstOrDefault(n => n.EndsWith(scriptName.Replace(" ", "_"), StringComparison.OrdinalIgnoreCase));
        if (resourceName != null)
        {
            await using var stream = assembly.GetManifestResourceStream(resourceName)!;
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync(cancellationToken);
        }

        throw new FileNotFoundException($"Script SQL não encontrado: {scriptName}. Procure em '{filePath}' ou como resource em {assembly.FullName}.");
    }
}
