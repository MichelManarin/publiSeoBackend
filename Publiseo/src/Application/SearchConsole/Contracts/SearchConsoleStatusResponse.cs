namespace Application.SearchConsole.Contracts;

/// <summary>
/// Status da conexão OAuth do usuário com o Google Search Console.
/// </summary>
public record SearchConsoleStatusResponse(bool Conectado, string? EmailGoogle);
