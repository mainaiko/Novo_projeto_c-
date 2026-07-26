using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.DTOs;


// DTO de request para criação de uma nova transação.
// Valida os campos obrigatórios antes de chegar à camada de serviço,
// onde regras de negócio adicionais (como restrição de menores) são aplicadas.
public class CriarTransacaoRequest
{
    // Descrição da transação (ex: "Conta de água").
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [MaxLength(500, ErrorMessage = "A descrição não pode ter mais de 500 caracteres.")]
    public string Descricao { get; set; } = string.Empty; // Método GET e SET para a descrição da transação

    // Valor da transação. Deve ser estritamente positivo.
    // Valores negativos não são aceitos pois o tipo (Despesa/Receita) já define a direção.
    [Required(ErrorMessage = "O valor é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Valor { get; set; } // Método GET e SET para o valor da transação

    // Tipo da transação: 0 = Despesa, 1 = Receita.
    // Receitas são bloqueadas para menores de 18 anos.
    [Required(ErrorMessage = "O tipo da transação é obrigatório.")]
    public TipoTransacao Tipo { get; set; } // Método GET e SET para o tipo da transação

    // Id da pessoa responsável. Deve existir no banco.
    [Required(ErrorMessage = "O ID da pessoa é obrigatório.")]
    public int PessoaId { get; set; }
}

// DTO de response para retornar dados de uma transação,
// incluindo o nome da pessoa associada para exibição no front-end.
public class TransacaoResponse
{
    public int Id { get; set; } // Método GET e SET para o ID da transação
    public string Descricao { get; set; } = string.Empty; // Método GET e SET para a descrição da transação
    public decimal Valor { get; set; } // Método GET e SET para o valor da transação
    public string Tipo { get; set; } = string.Empty; // Método GET e SET para o tipo da transação
    public int PessoaId { get; set; } // Método GET e SET para o ID da pessoa
    public string PessoaNome { get; set; } = string.Empty; // Método GET e SET para o nome da pessoa
}
