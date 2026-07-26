namespace Backend.Models;

// Enum que define os tipos possíveis de transação financeira.
// Usa valores numéricos explícitos para garantir consistência
// na serialização JSON e no armazenamento no banco de dados.
// Como só existem 2 tipos, um enum é mais adequado que uma classe ou tabela.

public enum TipoTransacao
{
    // Saída de dinheiro (gasto).
    Despesa = 0,

    // Entrada de dinheiro (ganho). Bloqueada para menores de 18 anos.
    Receita = 1
}
