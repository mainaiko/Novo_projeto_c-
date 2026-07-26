using Backend.DTOs;

namespace Backend.Services;

// Interface para o serviço de gerenciamento de pessoas.
public interface IPessoaService
{
    //Cria uma nova pessoa na residência.
    //CriarAsync recebe como parametro um CriarPessoaRequest e retorna uma PessoaResponse
    Task<PessoaResponse> CriarAsync(CriarPessoaRequest request);

    // Lista todas as pessoas cadastradas.
    Task<IEnumerable<PessoaResponse>> ListarAsync();

    // Deleta uma pessoa e todas as suas transações associadas (cascade delete).
    // Parametro id: Id da pessoa a ser deletada.
    // Retorna: True se a pessoa foi encontrada e deletada.
    Task<bool> DeletarAsync(int id);
}
