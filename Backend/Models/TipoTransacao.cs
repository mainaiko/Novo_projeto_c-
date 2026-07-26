namespace Backend.Models;

//Enum que define os tipos possíveis de transação financeira.
//Utiliza valores numéricos explícitos para garantir consistência
//na serialização/deserialização e no armazenamento no banco de dados.
//Como so existem 2 tipos possiveis de transacao nao e necessario criar uma classe para isso.
//so seria nescessario se houvessem mais de 2 tipos de transacao.

public enum TipoTransacao
{
    //Saída de dinheiro (gasto).
    Despesa = 0,

    //Entrada de dinheiro (ganho). Restrita para menores de 18 anos.
    Receita = 1
}
