using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

// DTO de request para criação de uma nova pessoa.
// Separado da entidade para não expor detalhes internos do modelo
// e para permitir validações específicas da API.
public class CriarPessoaRequest
{
    // Nome da pessoa. Obrigatório.
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(200, ErrorMessage = "O nome não pode ter mais de 200 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    // Idade da pessoa em anos. Deve ser >= 0.
    [Required(ErrorMessage = "A idade é obrigatória.")]
    [Range(0, 150, ErrorMessage = "A idade deve ser entre 0 e 150 anos.")]
    public int Idade { get; set; }
}

// DTO de response para retornar dados de uma pessoa.
// Inclui o Id gerado pelo banco, mas exclui a coleção de transações
// para evitar carregamento desnecessário (consultas de transações têm endpoint próprio).
public class PessoaResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Idade { get; set; }
}
