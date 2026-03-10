using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public sealed class ArtigoRepository : IArtigoRepository
{
    private readonly PubliseoDbContext _context;

    public ArtigoRepository(PubliseoDbContext context) => _context = context;

    public async Task<Artigo?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Artigos
            .Include(a => a.Blog)
            .Include(a => a.UltimoUsuario)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Artigo>> ListarPorBlogAsync(Guid blogId, CancellationToken cancellationToken = default)
        => await _context.Artigos
            .Where(a => a.BlogId == blogId)
            .Include(a => a.UltimoUsuario)
            .OrderByDescending(a => a.DataAtualizacao)
            .ToListAsync(cancellationToken);

    public async Task<Artigo> InserirAsync(Artigo artigo, CancellationToken cancellationToken = default)
    {
        _context.Artigos.Add(artigo);
        await _context.SaveChangesAsync(cancellationToken);
        return artigo;
    }

    public async Task<Artigo> AtualizarAsync(Artigo artigo, CancellationToken cancellationToken = default)
    {
        _context.Artigos.Update(artigo);
        await _context.SaveChangesAsync(cancellationToken);
        return artigo;
    }
}
