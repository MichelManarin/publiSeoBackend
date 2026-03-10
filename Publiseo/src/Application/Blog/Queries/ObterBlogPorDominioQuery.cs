using Application.Blog.Contracts;
using MediatR;

namespace Application.Blog.Queries;

/// <summary>
/// Obtém o ExternalId do blog pelo domínio de origem (nome em blog_dominios).
/// </summary>
public record ObterBlogPorDominioQuery(string Dominio) : IRequest<BlogPorDominioResponse?>;
