using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

//Entidade que representa uma pessoa residente.
//Cada pessoa pode ter múltiplas transações financeiras associadas.
//A deleção de uma pessoa dispara exclusão em cascata de todas as suas transações
public class Pessoa
{
    //Identificador único, gerado automaticamente pelo banco (auto-increment)
    [Key]
    public int Id { get; set; }

    //Nome completo da pessoa. Obrigatório e não pode ser vazio.
    [Required]
    [MaxLength(200)]
    public string Nome { get; set; } = string.Empty;

    //Idade da pessoa em anos. Utilizada na regra de negócio que impede
    //menores de 18 anos de cadastrar transações do tipo "Receita".
    [Required]
    [Range(0, 150)]
    public int Idade { get; set; }

    //Coleção de navegação para as transações desta pessoa.
    //Utilizada pelo EF Core para o relacionamento 1:N e cascade delete.
    public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}
