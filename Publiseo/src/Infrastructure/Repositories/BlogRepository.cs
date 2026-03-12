using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public sealed class BlogRepository : IBlogRepository
{
    private readonly PubliseoDbContext _context;

    public BlogRepository(PubliseoDbContext context) => _context = context;

    public async Task<Blog?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Blogs.Include(b => b.Usuario).FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public async Task<Blog?> ObterPorExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default)
        => await _context.Blogs.FirstOrDefaultAsync(b => b.ExternalId == externalId, cancellationToken);

    public async Task<IReadOnlyList<Blog>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default)
        => await _context.Blogs
            .Where(b => b.UsuarioId == usuarioId || b.Membros.Any(m => m.UsuarioId == usuarioId))
            .OrderBy(b => b.Nome)
            .ToListAsync(cancellationToken);

    public async Task<Blog> InserirAsync(Blog blog, CancellationToken cancellationToken = default)
    {
        _context.Blogs.Add(blog);
        await _context.SaveChangesAsync(cancellationToken);
        return blog;
    }

    public async Task<Blog> AtualizarAsync(Blog blog, CancellationToken cancellationToken = default)
    {
        _context.Blogs.Update(blog);
        await _context.SaveChangesAsync(cancellationToken);
        return blog;
    }
}
