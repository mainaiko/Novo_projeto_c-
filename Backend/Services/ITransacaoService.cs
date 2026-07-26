using Backend.DTOs;

namespace Backend.Services;

// Contrato do serviço de gerenciamento de transações financeiras.
// Define as operações de criação, listagem e cálculo de resumos.
public interface ITransacaoService
{
    // Cria uma nova transação financeira.
    // Aplica a regra: menores de 18 anos só podem cadastrar Despesas.
    // Recebe os dados da transação e retorna a transação criada.
    Task<TransacaoResponse> CriarAsync(CriarTransacaoRequest request);

    // Lista todas as transações com dados da pessoa associada.
    // Retorna a lista ordenada por Id descendente (mais recentes primeiro).
    Task<IEnumerable<TransacaoResponse>> ListarAsync();

    // Calcula o resumo financeiro consolidado, agrupado por pessoa
    // e com totais gerais da residência.
    Task<ResumoGeralDto> ObterResumoAsync();
}
