namespace Application.Data;

/// <summary>
/// Executor de SQL bruto para consultas e comandos complexos.
/// Use LINQ via repositórios/DbSet para o dia a dia; use este interface para relatórios e queries que não se encaixam bem em LINQ.
/// </summary>
public interface IComplexQueryExecutor
{
    /// <summary>
    /// Executa uma query SQL e mapeia o resultado para <typeparamref name="T"/> (por nome de coluna).
    /// Use parâmetros para evitar SQL injection, ex: "SELECT * FROM tabela WHERE id = {0}" passando o valor no array.
    /// </summary>
    Task<IReadOnlyList<T>> QueryAsync<T>(string sql, CancellationToken cancellationToken = default, params object[] parameters) where T : class;

    /// <summary>
    /// Executa uma query SQL (INSERT/UPDATE/DELETE ou DDL) e retorna a quantidade de linhas afetadas.
    /// </summary>
    Task<int> ExecuteAsync(string sql, CancellationToken cancellationToken = default, params object[] parameters);

    /// <summary>
    /// Executa um script SQL carregado do disco (pasta SqlScripts). Útil para queries complexas versionadas.
    /// O script pode conter parâmetros no formato {0}, {1}, etc., preenchidos por <paramref name="parameters"/>.
    /// </summary>
    Task<IReadOnlyList<T>> QueryFromScriptAsync<T>(string scriptName, CancellationToken cancellationToken = default, params object[] parameters) where T : class;

    /// <summary>
    /// Executa um script SQL (comando) e retorna linhas afetadas.
    /// </summary>
    Task<int> ExecuteFromScriptAsync(string scriptName, CancellationToken cancellationToken = default, params object[] parameters);
}
