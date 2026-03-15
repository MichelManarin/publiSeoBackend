using Application.Artigo.Contracts;
using MediatR;

namespace Application.Artigo.Commands;

/// <summary>
/// Edita título, descrição, conteúdo e capa do artigo. O tipo (IA/manual) não pode ser alterado: permanece o valor original.
/// </summary>
public record EditarArtigoCommand(
    Guid Id,
    Guid UsuarioId,
    string Titulo,
    string? MetaDescription,
    string Conteudo,
    string? ImagemCapaUrl = null,
    string? ImagemCapaUnsplashId = null,
    string? ImagemCapaAttribution = null) : IRequest<ArtigoResponse?>;
