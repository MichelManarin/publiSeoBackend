using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Application.Conversor.Commands;

[ExcludeFromCodeCoverage]
public sealed class RegistrarConversorLeadCommandHandler : IRequestHandler<RegistrarConversorLeadCommand, bool>
{
    private readonly IConversorRepository _conversorRepository;
    private readonly IConversorLeadRepository _leadRepository;

    public RegistrarConversorLeadCommandHandler(
        IConversorRepository conversorRepository,
        IConversorLeadRepository leadRepository)
    {
        _conversorRepository = conversorRepository;
        _leadRepository = leadRepository;
    }

    public async Task<bool> Handle(RegistrarConversorLeadCommand request, CancellationToken cancellationToken)
    {
        var conversor = await _conversorRepository.ObterPorBlogExternalIdAsync(request.BlogExternalId, cancellationToken);
        if (conversor == null || !conversor.Ativo)
            return false;

        var respostasJson = JsonSerializer.Serialize(request.Respostas ?? Array.Empty<string>());
        var lead = new ConversorLead
        {
            Id = Guid.NewGuid(),
            ConversorId = conversor.Id,
            NomeCompleto = request.NomeCompleto.Trim(),
            Telefone = request.Telefone.Trim(),
            RespostasJson = respostasJson,
            ArtigoId = request.ArtigoId,
            Ip = string.IsNullOrWhiteSpace(request.Ip) ? null : request.Ip.Trim().Length > 45 ? request.Ip.Trim().Substring(0, 45) : request.Ip.Trim(),
            DataCriacao = DateTime.UtcNow
        };
        await _leadRepository.InserirAsync(lead, cancellationToken);
        return true;
    }
}
