using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Context;

[ExcludeFromCodeCoverage]
public class PubliseoDbContext : DbContext
{
    public PubliseoDbContext(DbContextOptions<PubliseoDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Blog> Blogs => Set<Blog>();
    public DbSet<BlogMembro> BlogMembros => Set<BlogMembro>();
    public DbSet<Dominio> Dominios => Set<Dominio>();
    public DbSet<BlogDominio> BlogDominios => Set<BlogDominio>();
    public DbSet<Artigo> Artigos => Set<Artigo>();
    public DbSet<SearchConsoleMetrica> SearchConsoleMetricas => Set<SearchConsoleMetrica>();
    public DbSet<SearchConsoleOAuth> SearchConsoleOAuth => Set<SearchConsoleOAuth>();
    public DbSet<Conversor> Conversores => Set<Conversor>();
    public DbSet<ConversorPergunta> ConversorPerguntas => Set<ConversorPergunta>();
    public DbSet<ConversorLead> ConversorLeads => Set<ConversorLead>();
    public DbSet<BlogIntegracao> BlogIntegracoes => Set<BlogIntegracao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PubliseoDbContext).Assembly);
    }
}
