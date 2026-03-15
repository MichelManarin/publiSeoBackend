namespace Application.Artigo.Contracts;

/// <summary>
/// DTO público de artigo (nome, descrição, data de publicação, conteúdo e autor).
/// </summary>
public record ArtigoPublicoResponse(
    string Nome,
    string? Descricao,
    DateTime DataPublicacao,
    string Conteudo,
    string Autor,
    string? ImagemCapaUrl = null,
    string? ImagemCapaAttribution = null);
