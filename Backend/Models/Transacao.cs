using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

// Entidade que representa uma transação financeira (receita ou despesa)
// vinculada obrigatoriamente a uma pessoa da residência.
public class Transacao
{
    //Identificador único, gerado automaticamente pelo banco (auto-increment).
    [Key]
    public int Id { get; set; }

    //Descrição da transação (ex: "Conta de luz", "Salário").
    [Required]
    [MaxLength(500)]
    public string Descricao { get; set; } = string.Empty;

    //Valor monetário da transação. Deve ser estritamente positivo.
    [Required]
    [Column(TypeName = "decimal(18,2)")] //A precisão decimal (18,2) é definida aqui para garantir que o valor seja armazenado corretamente no banco de dados.
    public decimal Valor { get; set; }

    //Tipo da transação: Despesa (0) ou Receita (1).
    //Receitas são bloqueadas para menores de 18 anos (regra de negócio no serviço).
    [Required]
    public TipoTransacao Tipo { get; set; }

    //Chave estrangeira obrigatória referenciando a pessoa responsável pela transação.
    //O EF Core usa esta propriedade para configurar o relacionamento e o cascade delete.
    [Required]
    public int PessoaId { get; set; }

    //Propriedade de navegação para a pessoa associada.
    [ForeignKey("PessoaId")]
    public Pessoa? Pessoa { get; set; }
}
