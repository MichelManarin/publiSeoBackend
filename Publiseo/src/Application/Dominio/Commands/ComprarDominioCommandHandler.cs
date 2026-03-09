using Application.Dominio.Adapters;
using Application.Dominio.Builders;
using Application.Dominio.Contracts;
using Application.Dominio.Contracts.GoDaddy;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Application.Dominio.Commands;

[ExcludeFromCodeCoverage]
public sealed class ComprarDominioCommandHandler : IRequestHandler<ComprarDominioCommand, ComprarDominioResponse?>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IDominioAdapter _dominioAdapter;
    private readonly IDominioRepository _dominioRepository;
    private readonly CompanyDomainSettings _companySettings;

    public ComprarDominioCommandHandler(
        IUsuarioRepository usuarioRepository,
        IDominioAdapter dominioAdapter,
        IDominioRepository dominioRepository,
        IOptions<CompanyDomainSettings> companySettings)
    {
        _usuarioRepository = usuarioRepository;
        _dominioAdapter = dominioAdapter;
        _dominioRepository = dominioRepository;
        _companySettings = companySettings?.Value ?? throw new ArgumentNullException(nameof(companySettings));
    }

    public async Task<ComprarDominioResponse?> Handle(ComprarDominioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(request.UsuarioId, cancellationToken);
        if (usuario == null)
            return null;

        var nomeDominio = request.Dominio.Trim().ToLowerInvariant();

        var customerData = MapUsuarioToCustomerData(usuario);
        var contacts = GoDaddyDomainContactsBuilder.Build(customerData, _companySettings);

        var disponibilidade = await _dominioAdapter.VerificarDisponibilidadeAsync(nomeDominio, cancellationToken);
        if (!disponibilidade.Disponivel)
            throw new BadRequestException($"O domínio {nomeDominio} não está disponível para registro.");

        var tld = ExtrairTld(nomeDominio);
        var agreements = await _dominioAdapter.ObterAgreementsAsync(tld, request.Privacy, cancellationToken);
        var agreementKeys = agreements
            .Where(a => !string.IsNullOrWhiteSpace(a.AgreementKey))
            .Select(a => a.AgreementKey!)
            .ToList();
        if (agreementKeys.Count == 0)
            throw new BadRequestException($"Não foi possível obter os termos (agreements) para o TLD {tld}.");

        var consent = new GoDaddyDomainConsent
        {
            AgreementKeys = agreementKeys,
            AgreedBy = request.AgreedBy ?? "0.0.0.0",
            AgreedAt = DateTime.UtcNow.ToString("O")
        };

        var purchaseRequest = new GoDaddyDomainPurchaseRequest
        {
            Domain = nomeDominio,
            Consent = consent,
            ContactAdmin = contacts.ContactAdmin,
            ContactBilling = contacts.ContactBilling,
            ContactRegistrant = contacts.ContactRegistrant,
            ContactTech = contacts.ContactTech,
            Period = Math.Clamp(request.Period, 1, 10),
            Privacy = request.Privacy,
            RenewAuto = request.RenewAuto
        };

        await _dominioAdapter.ValidarCompraAsync(purchaseRequest, cancellationToken);
        var purchaseResponse = await _dominioAdapter.ComprarAsync(purchaseRequest, cancellationToken);

        var dataCompra = DateTime.UtcNow;
        var periodoAnos = Math.Clamp(request.Period, 1, 10);

        var entidadeDominio = new Domain.Entities.Dominio
        {
            Id = Guid.NewGuid(),
            UsuarioId = request.UsuarioId,
            NomeDominio = nomeDominio,
            DataCompra = dataCompra,
            DataExpiracao = dataCompra.AddYears(periodoAnos),
            OrdemIdExterno = purchaseResponse.OrderId,
            Total = purchaseResponse.Total,
            Moeda = purchaseResponse.Currency,
            PeriodoAnos = periodoAnos,
            Privacy = request.Privacy,
            RenewAuto = request.RenewAuto
        };
        await _dominioRepository.InserirAsync(entidadeDominio, cancellationToken);

        return new ComprarDominioResponse
        {
            DominioId = entidadeDominio.Id,
            NomeDominio = entidadeDominio.NomeDominio,
            OrdemIdExterno = purchaseResponse.OrderId,
            DataCompra = entidadeDominio.DataCompra,
            DataExpiracao = entidadeDominio.DataExpiracao,
            Total = entidadeDominio.Total,
            Moeda = entidadeDominio.Moeda,
            PeriodoAnos = entidadeDominio.PeriodoAnos,
            Privacy = entidadeDominio.Privacy,
            RenewAuto = entidadeDominio.RenewAuto
        };
    }

    private static CustomerDomainContactData MapUsuarioToCustomerData(Usuario u)
    {
        return new CustomerDomainContactData
        {
            FirstName = u.Nome ?? string.Empty,
            LastName = u.Sobrenome ?? string.Empty,
            Email = u.Email ?? string.Empty,
            Phone = u.Telefone ?? string.Empty,
            Address1 = u.Endereco ?? string.Empty,
            Address2 = null,
            City = u.Cidade ?? string.Empty,
            State = u.Estado ?? string.Empty,
            PostalCode = u.CodigoPostal ?? string.Empty,
            Country = u.Pais ?? string.Empty
        };
    }

    private static string ExtrairTld(string dominio)
    {
        var parts = dominio.Split('.', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2 ? parts[^1].ToLowerInvariant() : "com";
    }
}
