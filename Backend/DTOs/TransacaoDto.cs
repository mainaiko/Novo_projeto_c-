using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.DTOs;


// DTO de entrada para criação de uma nova transação.
// Valida campos obrigatórios antes da camada de serviço,
// onde regras de negócio adicionais (restrição de menores) são aplicadas.
public class CriarTransacaoRequest
{
    // Descrição da transação (ex: "Conta de água").
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [MaxLength(500, ErrorMessage = "A descrição não pode ter mais de 500 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    // Valor da transação. Deve ser estritamente positivo.
    // Valores negativos não são aceitos pois o tipo (Despesa/Receita) já define a direção.
    [Required(ErrorMessage = "O valor é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Valor { get; set; }

    // Tipo da transação: 0 = Despesa, 1 = Receita.
    // Receitas são bloqueadas para menores de 18 anos.
    [Required(ErrorMessage = "O tipo da transação é obrigatório.")]
    public TipoTransacao Tipo { get; set; }

    // Id da pessoa responsável. Deve existir no banco.
    [Required(ErrorMessage = "O ID da pessoa é obrigatório.")]
    public int PessoaId { get; set; }
}

// DTO de saída com os dados de uma transação,
// incluindo o nome da pessoa associada para exibição no front-end.
public class TransacaoResponse
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public int PessoaId { get; set; }
    public string PessoaNome { get; set; } = string.Empty;
}
