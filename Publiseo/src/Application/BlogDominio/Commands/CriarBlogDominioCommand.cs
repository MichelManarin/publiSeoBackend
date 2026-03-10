using Application.BlogDominio.Contracts;
using MediatR;

namespace Application.BlogDominio.Commands;

public record CriarBlogDominioCommand(Guid BlogId, string NomeDominio) : IRequest<BlogDominioResponse?>;
