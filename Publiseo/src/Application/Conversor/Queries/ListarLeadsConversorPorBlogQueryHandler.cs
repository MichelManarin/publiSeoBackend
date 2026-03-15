using Application.Conversor.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Application.Conversor.Queries;

[ExcludeFromCodeCoverage]
public sealed class ListarLeadsConversorPorBlogQueryHandler : IRequestHandler<ListarLeadsConversorPorBlogQuery, IReadOnlyList<ConversorLeadItemResponse>>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IConversorLeadRepository _leadRepository;

    public ListarLeadsConversorPorBlogQueryHandler(
        IBlogRepository blogRepository,
        IConversorLeadRepository leadRepository)
    {
        _blogRepository = blogRepository;
        _leadRepository = leadRepository;
    }

    public async Task<IReadOnlyList<ConversorLeadItemResponse>> Handle(ListarLeadsConversorPorBlogQuery request, CancellationToken cancellationToken)
    {
        var blogsDoUsuario = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        if (blogsDoUsuario.All(b => b.Id != request.BlogId))
            return Array.Empty<ConversorLeadItemResponse>();

        var leads = await _leadRepository.ListarPorBlogIdAsync(request.BlogId, cancellationToken);
        return leads.Select(l =>
        {
            var respostas = Array.Empty<string>();
            if (!string.IsNullOrWhiteSpace(l.RespostasJson))
            {
                try
                {
                    respostas = JsonSerializer.Deserialize<string[]>(l.RespostasJson) ?? Array.Empty<string>();
                }
                catch
                {
                    // mantém array vazio se JSON inválido
                }
            }
            return new ConversorLeadItemResponse(
                l.Id,
                l.NomeCompleto,
                l.Telefone,
                respostas,
                l.ArtigoId,
                l.Ip,
                l.DataCriacao);
        }).ToList();
    }
}
