using Application.Dominio.Contracts;
using MediatR;

namespace Application.Dominio.Commands;

/// <summary>
/// Comando para comprar um domínio em nome do usuário (titularidade do cliente).
/// Verifica disponibilidade, obtém agreements, monta contatos (cliente + empresa) e executa a compra na GoDaddy.
/// </summary>
/// <param name="AgreedBy">IP do cliente que aceita os termos (recomendado para consent); se omitido usa placeholder.</param>
public record ComprarDominioCommand(
    Guid UsuarioId,
    string Dominio,
    int Period = 1,
    bool Privacy = false,
    bool RenewAuto = true,
    string? AgreedBy = null) : IRequest<ComprarDominioResponse?>;
