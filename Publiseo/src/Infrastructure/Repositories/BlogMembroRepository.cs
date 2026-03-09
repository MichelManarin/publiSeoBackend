using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public sealed class BlogMembroRepository : IBlogMembroRepository
{
    private readonly PubliseoDbContext _context;

    public BlogMembroRepository(PubliseoDbContext context) => _context = context;

    public async Task InserirAsync(BlogMembro membro, CancellationToken cancellationToken = default)
    {
        _context.BlogMembros.Add(membro);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UsuarioEhMembroAsync(Guid blogId, Guid usuarioId, CancellationToken cancellationToken = default)
        => await _context.BlogMembros.AnyAsync(m => m.BlogId == blogId && m.UsuarioId == usuarioId, cancellationToken);
}
