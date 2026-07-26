namespace Backend.DTOs;

// DTO que representa o resumo financeiro de uma pessoa individual.
// Calculado a partir de todas as transações associadas à pessoa.
public class ResumoPessoaDto
{
    // Id da pessoa.
    public int PessoaId { get; set; }

    // Nome da pessoa para exibição.
    public string PessoaNome { get; set; } = string.Empty;

    // Soma de todas as transações do tipo Receita desta pessoa.
    public decimal TotalReceitas { get; set; }

    // Soma de todas as transações do tipo Despesa desta pessoa.
    public decimal TotalDespesas { get; set; }

    // Saldo líquido = TotalReceitas - TotalDespesas.
    // Valor positivo indica superávit, negativo indica déficit.
    public decimal SaldoLiquido => TotalReceitas - TotalDespesas;
}

// DTO que representa o resumo financeiro consolidado da residência inteira.
// Agrega os dados de todas as pessoas para uma visão geral.
public class ResumoGeralDto
{
    // Lista com o resumo individual de cada pessoa.
    public List<ResumoPessoaDto> ResumosPorPessoa { get; set; } = new();

    // Soma de todas as receitas de todas as pessoas.
    public decimal TotalGeralReceitas { get; set; }

    // Soma de todas as despesas de todas as pessoas.
    public decimal TotalGeralDespesas { get; set; }

    // Saldo líquido geral da residência = TotalGeralReceitas - TotalGeralDespesas.
    // Valor positivo indica superávit, negativo indica déficit.
    public decimal SaldoLiquidoGeral => TotalGeralReceitas - TotalGeralDespesas;
}
