using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public sealed class BlogIntegracaoRepository : IBlogIntegracaoRepository
{
    private readonly PubliseoDbContext _context;

    public BlogIntegracaoRepository(PubliseoDbContext context) => _context = context;

    public async Task<BlogIntegracao?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.BlogIntegracoes
            .Include(x => x.Blog)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<BlogIntegracao>> ListarPorBlogIdAsync(Guid blogId, CancellationToken cancellationToken = default)
        => await _context.BlogIntegracoes
            .Where(x => x.BlogId == blogId)
            .OrderBy(x => x.Ordem)
            .ThenBy(x => x.DataCriacao)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<BlogIntegracao>> ListarPorBlogExternalIdAsync(Guid blogExternalId, CancellationToken cancellationToken = default)
        => await _context.BlogIntegracoes
            .Where(x => x.Blog.ExternalId == blogExternalId)
            .OrderBy(x => x.Ordem)
            .ThenBy(x => x.DataCriacao)
            .ToListAsync(cancellationToken);

    public async Task<BlogIntegracao> InserirAsync(BlogIntegracao integracao, CancellationToken cancellationToken = default)
    {
        _context.BlogIntegracoes.Add(integracao);
        await _context.SaveChangesAsync(cancellationToken);
        return integracao;
    }

    public async Task<BlogIntegracao> AtualizarAsync(BlogIntegracao integracao, CancellationToken cancellationToken = default)
    {
        integracao.DataAtualizacao = DateTime.UtcNow;
        _context.BlogIntegracoes.Update(integracao);
        await _context.SaveChangesAsync(cancellationToken);
        return integracao;
    }

    public async Task ExcluirAsync(BlogIntegracao integracao, CancellationToken cancellationToken = default)
    {
        _context.BlogIntegracoes.Remove(integracao);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
