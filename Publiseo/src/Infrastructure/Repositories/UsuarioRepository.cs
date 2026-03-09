using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public sealed class UsuarioRepository : IUsuarioRepository
{
    private readonly PubliseoDbContext _context;

    public UsuarioRepository(PubliseoDbContext context) => _context = context;

    public async Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Usuarios.FindAsync([id], cancellationToken);

    public async Task<Usuario?> ObterPorLoginAsync(string login, CancellationToken cancellationToken = default)
        => await _context.Usuarios.FirstOrDefaultAsync(u => u.Login == login, cancellationToken);

    public async Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task<Usuario> InserirAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync(cancellationToken);
        return usuario;
    }

    public async Task AtualizarAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
