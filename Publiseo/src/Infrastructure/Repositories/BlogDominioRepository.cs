using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public sealed class BlogDominioRepository : IBlogDominioRepository
{
    private readonly PubliseoDbContext _context;

    public BlogDominioRepository(PubliseoDbContext context) => _context = context;

    public async Task<BlogDominio?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.BlogDominios.FindAsync([id], cancellationToken);

    public async Task<BlogDominio?> ObterPorNomeDominioAsync(string nomeDominio, CancellationToken cancellationToken = default)
    {
        var normalizado = nomeDominio.Trim().ToLowerInvariant();
        return await _context.BlogDominios
            .Include(x => x.Blog)
            .FirstOrDefaultAsync(x => x.NomeDominio == normalizado, cancellationToken);
    }

    public async Task<BlogDominio?> ObterPorBlogENomeAsync(Guid blogId, string nomeDominio, CancellationToken cancellationToken = default)
        => await _context.BlogDominios
            .FirstOrDefaultAsync(x => x.BlogId == blogId && x.NomeDominio == nomeDominio, cancellationToken);

    public async Task<IReadOnlyList<BlogDominio>> ListarPorBlogAsync(Guid blogId, CancellationToken cancellationToken = default)
        => await _context.BlogDominios
            .Where(x => x.BlogId == blogId)
            .OrderBy(x => x.NomeDominio)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<BlogDominio>> ListarTodosAsync(CancellationToken cancellationToken = default)
        => await _context.BlogDominios.OrderBy(x => x.NomeDominio).ToListAsync(cancellationToken);

    public async Task<BlogDominio> InserirAsync(BlogDominio blogDominio, CancellationToken cancellationToken = default)
    {
        _context.BlogDominios.Add(blogDominio);
        await _context.SaveChangesAsync(cancellationToken);
        return blogDominio;
    }

    public async Task ExcluirAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.BlogDominios.FindAsync([id], cancellationToken);
        if (entity != null)
        {
            _context.BlogDominios.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
