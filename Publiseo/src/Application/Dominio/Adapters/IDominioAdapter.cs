using Application.Dominio.Contracts;
using Application.Dominio.Contracts.GoDaddy;

namespace Application.Dominio.Adapters;

/// <summary>
/// Adapter unificado para operações de domínio (disponibilidade, sugestão, compra).
/// Uma única abstração para o provedor externo (ex.: GoDaddy).
/// </summary>
public interface IDominioAdapter
{
    /// <summary>Verifica se o domínio está disponível para registro.</summary>
    Task<DominioDisponibilidadeResponse> VerificarDisponibilidadeAsync(string dominio, CancellationToken cancellationToken = default);

    /// <summary>Sugere domínios alternativos (ex.: GET /v1/domains/suggest).</summary>
    Task<IReadOnlyList<SugestaoDominioItemResponse>> SugerirAsync(
        SugestaoDominioOptions options,
        string? shopperId,
        CancellationToken cancellationToken = default);

    /// <summary>Obtém os agreement keys exigidos para o TLD (ex.: GET /v1/domains/agreements).</summary>
    Task<IReadOnlyList<GoDaddyAgreement>> ObterAgreementsAsync(string tld, bool privacy, CancellationToken cancellationToken = default);

    /// <summary>Valida o payload de compra antes de enviar (POST /v1/domains/purchase/validate).</summary>
    Task ValidarCompraAsync(GoDaddyDomainPurchaseRequest request, CancellationToken cancellationToken = default);

    /// <summary>Efetua a compra do domínio (POST /v1/domains/purchase).</summary>
    Task<GoDaddyDomainPurchaseResponse> ComprarAsync(GoDaddyDomainPurchaseRequest request, CancellationToken cancellationToken = default);
}
