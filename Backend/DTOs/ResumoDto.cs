namespace Backend.DTOs;

// DTO com o resumo financeiro individual de uma pessoa.
// Os totais são calculados a partir das transações associadas.
public class ResumoPessoaDto
{
    // Identificador da pessoa.
    public int PessoaId { get; set; }

    // Nome da pessoa, utilizado para exibição no front-end.
    public string PessoaNome { get; set; } = string.Empty;

    // Total acumulado das receitas desta pessoa.
    public decimal TotalReceitas { get; set; }

    // Total acumulado das despesas desta pessoa.
    public decimal TotalDespesas { get; set; }

    // Saldo líquido (receitas - despesas).
    // Positivo = superávit, negativo = déficit.
    public decimal SaldoLiquido => TotalReceitas - TotalDespesas;
}

// DTO com o resumo financeiro consolidado de toda a residência.
// Agrega os totais individuais de cada pessoa.
public class ResumoGeralDto
{
    // Resumo financeiro individual de cada pessoa da residência.
    public List<ResumoPessoaDto> ResumosPorPessoa { get; set; } = new();

    // Total acumulado de receitas de todas as pessoas.
    public decimal TotalGeralReceitas { get; set; }

    // Total acumulado de despesas de todas as pessoas.
    public decimal TotalGeralDespesas { get; set; }

    // Saldo líquido geral da residência (receitas - despesas).
    // Positivo = superávit, negativo = déficit.
    public decimal SaldoLiquidoGeral => TotalGeralReceitas - TotalGeralDespesas;
}
