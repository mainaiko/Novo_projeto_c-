using Backend.DTOs;

namespace Backend.Services;

// Contrato do serviço de gerenciamento de pessoas.
public interface IPessoaService
{
    // Cria uma nova pessoa na residência.
    // Recebe um CriarPessoaRequest e retorna os dados da pessoa criada.
    Task<PessoaResponse> CriarAsync(CriarPessoaRequest request);

    // Lista todas as pessoas cadastradas.
    Task<IEnumerable<PessoaResponse>> ListarAsync();

    // Remove uma pessoa e todas as suas transações (cascade delete).
    // Retorna true se encontrada e removida, false caso contrário.
    Task<bool> DeletarAsync(int id);
}
