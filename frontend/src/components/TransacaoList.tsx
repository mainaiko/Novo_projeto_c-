import type { Transacao } from '../types/types';

//interface - define as propriedades do componente TransacaoList
//transacoes: Transacao[] - lista de transações
//isLoading: boolean - indica se as transações estão sendo carregadas
interface TransacaoListProps {
  transacoes: Transacao[];
  isLoading: boolean;
}

//função formatarMoeda - formata o valor da transação em reais
function formatarMoeda(valor: number): string {
  return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}

//exporta o componente TransacaoList
export default function TransacaoList({ transacoes, isLoading }: TransacaoListProps) {
  if (isLoading) {
    return <div className="data-card"><p>Carregando transações...</p></div>;
  }

  //retorna o componente TransacaoList
  //Possui cinco colunas: ID, Descrição, Pessoa, Tipo e Valor
  //se não tiver transações exibe a mensagem de "Nenhuma transação registrada ainda."
  //se tiver transações exibe a tabela com as transações
  return (
    <div className="data-card">
      <h3 className="data-card__title">📊 Transações Registradas ({transacoes.length})</h3>

      {transacoes.length === 0 ? (
        <p className="empty-state">Nenhuma transação registrada ainda.</p>
      ) : (
        <table className="data-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Descrição</th>
              <th>Pessoa</th>
              <th>Tipo</th>
              <th>Valor</th>
            </tr>
          </thead>
          <tbody>
            {transacoes.map((t) => (
              <tr key={t.id}>
                <td>#{t.id}</td>
                <td>{t.descricao}</td>
                <td>{t.pessoaNome}</td>
                <td>
                  <span className={`badge badge--${t.tipo === 'Receita' ? 'success' : 'expense'}`}>
                    {t.tipo === 'Receita' ? '🟢 Receita' : '🔴 Despesa'}
                  </span>
                </td>
                <td className={`data-table__valor--${t.tipo === 'Receita' ? 'positive' : 'negative'}`}>
                  {t.tipo === 'Receita' ? '+' : '-'} {formatarMoeda(t.valor)}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
