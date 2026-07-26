using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

// DTO de entrada para criação de uma nova pessoa.
// Isola a API da entidade interna e aplica validações específicas da requisição.
public class CriarPessoaRequest
{
    // Nome completo da pessoa. Campo obrigatório, máximo 200 caracteres.
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(200, ErrorMessage = "O nome não pode ter mais de 200 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    // Idade da pessoa em anos. Campo obrigatório, deve estar entre 0 e 150.
    [Required(ErrorMessage = "A idade é obrigatória.")]
    [Range(0, 150, ErrorMessage = "A idade deve ser entre 0 e 150 anos.")]
    public int Idade { get; set; }
}

// DTO de saída com os dados de uma pessoa.
// Inclui o Id gerado pelo banco, sem a coleção de transações
// (transações possuem endpoint próprio para consulta).
public class PessoaResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Idade { get; set; }
}
