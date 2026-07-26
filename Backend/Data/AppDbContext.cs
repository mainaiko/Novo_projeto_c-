using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data;

//AppDbContext herda de DbContext
//DbContext e uma classe que fornece métodos e propriedades para interagir com o banco de dados
//Dbcontex vem do pacote Microsoft.EntityFrameworkCore
public class AppDbContext : DbContext
{
    //DbSet<T> e uma classe que representa uma tabela no banco de dados
    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Transacao> Transacoes => Set<Transacao>();

    //metodo construtor
    //options e uma propriedade que recebe as opções do contexto
    //base(options) chama o construtor da classe DbContext
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    //metodo que configura o contexto
    //base(options) chama o metodo construtor da classe DbContext
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da entidade Pessoa com Deleção em Cascata
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

        // Configuração da entidade Transação
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
