namespace Domain.Enums;

/// <summary>
/// Status da geração de conteúdo por IA do artigo.
/// </summary>
public enum StatusGeracaoArtigo
{
    /// <summary>Aguardando processamento pela fila/job.</summary>
    Pendente = 0,
    /// <summary>Em processamento (geração em andamento).</summary>
    EmProcessamento = 1,
    /// <summary>Conteúdo gerado com sucesso.</summary>
    Concluido = 2,
    /// <summary>Falha após o número máximo de tentativas.</summary>
    Falha = 3
}
