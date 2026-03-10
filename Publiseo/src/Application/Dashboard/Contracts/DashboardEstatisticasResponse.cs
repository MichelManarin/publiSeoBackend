namespace Application.Dashboard.Contracts;

/// <summary>
/// Estatísticas do dashboard: número de blogs e de artigos (dos blogs que o usuário tem acesso).
/// </summary>
public record DashboardEstatisticasResponse(int Blogs, int Artigos);
