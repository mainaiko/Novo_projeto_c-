using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data;

// Contexto do banco de dados da aplicação.
// Herda de DbContext (Entity Framework Core) para gerenciar a conexão
// e o mapeamento objeto-relacional das entidades.
public class AppDbContext : DbContext
{
    // DbSet representa a tabela de cada entidade no banco de dados.
    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Transacao> Transacoes => Set<Transacao>();

    // Construtor que recebe as opções de configuração do contexto (string de conexão, provider, etc.)
    // e as repassa para a classe base DbContext.
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Configura o mapeamento das entidades para o banco de dados
    // usando a Fluent API do Entity Framework Core.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da entidade Pessoa: chave primária, restrições e cascade delete.
        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nome).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Idade).IsRequired();

            entity.HasMany(p => p.Transacoes)
                  .WithOne(t => t.Pessoa)
                  .HasForeignKey(t => t.PessoaId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuração da entidade Transação: chave primária, restrições e índice.
        modelBuilder.Entity<Transacao>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Descricao).IsRequired().HasMaxLength(500);
            entity.Property(t => t.Valor).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(t => t.Tipo).IsRequired();
            entity.HasIndex(t => t.PessoaId);
        });
    }
}
