import type { Transacao } from '../types/types';

// Props do componente TransacaoList.
// transacoes: lista a exibir. isLoading: estado de carregamento.
interface TransacaoListProps {
  transacoes: Transacao[];
  isLoading: boolean;
}

// Formata um valor numérico para moeda brasileira (R$).
function formatarMoeda(valor: number): string {
  return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}

// Tabela que exibe todas as transações cadastradas.
export default function TransacaoList({ transacoes, isLoading }: TransacaoListProps) {
  if (isLoading) {
    return <div className="data-card"><p>Carregando transações...</p></div>;
  }

  // Renderiza a tabela com colunas: ID, Descrição, Pessoa, Tipo e Valor.
  // Exibe estado vazio se não houver transações.
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
