using Backend.DTOs;

namespace Backend.Services;

// Interface para o serviço de gerenciamento de transações financeiras.
// Define o contrato para criação, listagem e cálculo de resumos.
public interface ITransacaoService
{
    // Cria uma nova transação financeira.
    // Aplica a regra de restrição de menores: pessoas com menos de 18 anos
    // só podem cadastrar Despesas (Receitas são bloqueadas).
    // Parametro request: Dados da transação a ser criada.
    // Retorna: Dados da transação criada.
    Task<TransacaoResponse> CriarAsync(CriarTransacaoRequest request);

    // Lista todas as transações cadastradas com dados da pessoa associada.
    // Retorna: Lista de transações ordenadas por Id descendente (mais recentes primeiro).
    Task<IEnumerable<TransacaoResponse>> ListarAsync();

    // Calcula o resumo financeiro consolidado, agrupando totais por pessoa
    // e calculando o total geral da residência.
    // Retorna: Resumo com totais por pessoa e total geral.
    Task<ResumoGeralDto> ObterResumoAsync();
}
