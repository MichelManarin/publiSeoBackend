using Application.Blog.Contracts;
using MediatR;

namespace Application.Blog.Queries;

public record ListarBlogsPorUsuarioQuery(Guid UsuarioId) : IRequest<IReadOnlyList<BlogResponse>>;
