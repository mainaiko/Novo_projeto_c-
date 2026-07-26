using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

// Entidade que representa uma transação financeira (receita ou despesa)
// vinculada a uma pessoa da residência.
public class Transacao
{
    // Identificador único gerado automaticamente pelo banco (auto-increment).
    [Key]
    public int Id { get; set; }

    // Descrição da transação (ex: "Conta de luz", "Salário"). Obrigatória.
    [Required]
    [MaxLength(500)]
    public string Descricao { get; set; } = string.Empty;

    // Valor monetário. Deve ser positivo; a direção é definida pelo Tipo.
    [Required]
    [Column(TypeName = "decimal(18,2)")] // Precisão decimal para armazenamento correto no banco.
    public decimal Valor { get; set; }

    // Tipo: Despesa (0) ou Receita (1).
    // Receitas são bloqueadas para menores de 18 anos (validado no serviço).
    [Required]
    public TipoTransacao Tipo { get; set; }

    // Chave estrangeira referenciando a pessoa responsável.
    // Usada pelo EF Core para o relacionamento e cascade delete.
    [Required]
    public int PessoaId { get; set; }

    // Propriedade de navegação para a pessoa associada à transação.
    [ForeignKey("PessoaId")]
    public Pessoa? Pessoa { get; set; }
}
