using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

// Entidade que representa uma pessoa residente.
// Possui múltiplas transações financeiras associadas (relação 1:N).
// Ao deletar uma pessoa, todas as suas transações são removidas em cascata.
public class Pessoa
{
    // Identificador único gerado automaticamente pelo banco (auto-increment).
    [Key]
    public int Id { get; set; }

    // Nome completo da pessoa. Obrigatório, máximo de 200 caracteres.
    [Required]
    [MaxLength(200)]
    public string Nome { get; set; } = string.Empty;

    // Idade em anos. Utilizada na regra que impede menores de 18
    // de cadastrar transações do tipo Receita.
    [Required]
    [Range(0, 150)]
    public int Idade { get; set; }

    // Coleção de navegação para as transações desta pessoa (relação 1:N).
    // Utilizada pelo EF Core para cascade delete.
    public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}
