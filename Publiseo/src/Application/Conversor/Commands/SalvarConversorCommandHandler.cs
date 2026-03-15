using Application.Conversor.Contracts;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using ConversorEntity = Domain.Entities.Conversor;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Conversor.Commands;

[ExcludeFromCodeCoverage]
public sealed class SalvarConversorCommandHandler : IRequestHandler<SalvarConversorCommand, ConversorConfigResponse?>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IConversorRepository _conversorRepository;

    public SalvarConversorCommandHandler(
        IBlogRepository blogRepository,
        IConversorRepository conversorRepository)
    {
        _blogRepository = blogRepository;
        _conversorRepository = conversorRepository;
    }

    public async Task<ConversorConfigResponse?> Handle(SalvarConversorCommand request, CancellationToken cancellationToken)
    {
        var blogsDoUsuario = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        var blog = blogsDoUsuario.FirstOrDefault(b => b.Id == request.BlogId);
        if (blog == null)
            return null;

        var req = request.Request;
        var conversor = await _conversorRepository.ObterPorBlogIdComPerguntasAsync(request.BlogId, cancellationToken);
        var isNew = conversor == null;
        if (conversor == null)
        {
            conversor = new ConversorEntity
            {
                Id = Guid.NewGuid(),
                BlogId = request.BlogId,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };
        }

        conversor.Ativo = req.Ativo;
        conversor.TextoBotaoInicial = string.IsNullOrWhiteSpace(req.TextoBotaoInicial) ? null : req.TextoBotaoInicial.Trim();
        conversor.TipoFinalizacao = req.TipoFinalizacao;
        conversor.MensagemFinalizacao = string.IsNullOrWhiteSpace(req.MensagemFinalizacao) ? null : req.MensagemFinalizacao.Trim();
        conversor.WhatsAppNumero = string.IsNullOrWhiteSpace(req.WhatsAppNumero) ? null : req.WhatsAppNumero.Trim();
        conversor.WhatsAppTextoPreDefinido = string.IsNullOrWhiteSpace(req.WhatsAppTextoPreDefinido) ? null : req.WhatsAppTextoPreDefinido.Trim();

        var perguntasAtual = conversor.Perguntas.ToList();
        var idsMantidos = (req.Perguntas ?? Array.Empty<SalvarConversorPerguntaItemDto>())
            .Where(p => p.Id.HasValue)
            .Select(p => p.Id!.Value)
            .ToHashSet();
        foreach (var p in perguntasAtual.Where(p => !idsMantidos.Contains(p.Id)))
            conversor.Perguntas.Remove(p);

        var ordem = 0;
        foreach (var item in (req.Perguntas ?? Array.Empty<SalvarConversorPerguntaItemDto>()).OrderBy(p => p.Ordem))
        {
            var existente = item.Id.HasValue ? conversor.Perguntas.FirstOrDefault(p => p.Id == item.Id.Value) : null;
            if (existente != null)
            {
                existente.Ordem = ordem;
                existente.TextoPergunta = item.TextoPergunta.Trim();
                existente.TipoCampo = item.TipoCampo;
            }
            else
            {
                conversor.Perguntas.Add(new ConversorPergunta
                {
                    Id = Guid.NewGuid(),
                    ConversorId = conversor.Id,
                    Ordem = ordem,
                    TextoPergunta = item.TextoPergunta.Trim(),
                    TipoCampo = item.TipoCampo
                });
            }
            ordem++;
        }

        if (isNew)
            await _conversorRepository.InserirAsync(conversor, cancellationToken);
        else
            await _conversorRepository.AtualizarAsync(conversor, cancellationToken);

        var ordenadas = conversor.Perguntas.OrderBy(p => p.Ordem).ToList();
        return new ConversorConfigResponse(
            conversor.Id,
            conversor.BlogId,
            conversor.Ativo,
            conversor.TextoBotaoInicial,
            conversor.TipoFinalizacao,
            conversor.MensagemFinalizacao,
            conversor.WhatsAppNumero,
            conversor.WhatsAppTextoPreDefinido,
            ordenadas.Select(p => new ConversorPerguntaItemDto(p.Id, p.Ordem, p.TextoPergunta, p.TipoCampo)).ToList(),
            conversor.DataCriacao,
            conversor.DataAtualizacao);
    }
}
