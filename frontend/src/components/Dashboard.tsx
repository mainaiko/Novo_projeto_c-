import { useEffect, useState } from 'react';
import type { ResumoGeral } from '../types/types';
import { obterResumo } from '../services/api';

/**
 * Formata um valor numérico para o padrão de moeda brasileira (BRL).
 * Exemplo: 1234.56 → "R$ 1.234,56".
 */
function formatarMoeda(valor: number): string {
  return valor.toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL',
  });
}
// Componente de painel financeiro.
// Exibe cards com totais gerais e tabela de resumo por pessoa.
// Recarrega automaticamente quando refreshKey muda.

export default function Dashboard({ refreshKey }: { refreshKey: number }) {
  const [resumo, setResumo] = useState<ResumoGeral | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const carregar = async () => {
      setIsLoading(true);
      try {
        const data = await obterResumo();
        setResumo(data);
      } catch (error) {
        console.error('Erro ao carregar resumo:', error);
      } finally {
        setIsLoading(false);
      }
    };
    // Recarrega os dados sempre que refreshKey muda (ex: após criar/deletar dados).
    carregar();
  }, [refreshKey]);
  // Exibe spinner enquanto os dados são carregados.
  if (isLoading) {
    return (
      <div className="data-card">
        <div className="loading-spinner" />
        <p className="loading-text">Carregando resumo financeiro...</p>
      </div>
    );
  }
  // Exibe mensagem de erro caso o resumo não tenha sido carregado.
  if (!resumo) {
    return (
      <div className="data-card">
        <div className="empty-state">
          <span className="empty-state__icon">📉</span>
          <p>Erro ao carregar o resumo financeiro.</p>
        </div>
      </div>
    );
  }
  // Renderiza o painel financeiro completo.
  return (
    <div className="dashboard">
      {/* Cards de totais gerais */}
      <div className="dashboard__cards">
        <div className="summary-card summary-card--receitas">
          <div className="summary-card__icon">📈</div>
          <div className="summary-card__content">
            <span className="summary-card__label">Total de Receitas</span>
            <span className="summary-card__value">
              {formatarMoeda(resumo.totalGeralReceitas)}
            </span>
          </div>
        </div>

        <div className="summary-card summary-card--despesas">
          <div className="summary-card__icon">📉</div>
          <div className="summary-card__content">
            <span className="summary-card__label">Total de Despesas</span>
            <span className="summary-card__value">
              {formatarMoeda(resumo.totalGeralDespesas)}
            </span>
          </div>
        </div>

        <div className={`summary-card summary-card--saldo ${resumo.saldoLiquidoGeral >= 0 ? 'summary-card--positive' : 'summary-card--negative'}`}>
          <div className="summary-card__icon">
            {resumo.saldoLiquidoGeral >= 0 ? '✅' : '⚠️'}
          </div>
          <div className="summary-card__content">
            <span className="summary-card__label">Saldo Líquido</span>
            <span className="summary-card__value">
              {formatarMoeda(resumo.saldoLiquidoGeral)}
            </span>
          </div>
        </div>
      </div>

      {/* Tabela de resumo por pessoa */}
      <div className="data-card">
        <h3 className="data-card__title">
          <span className="data-card__icon">👥</span>
          Resumo por Pessoa
        </h3>

        {resumo.resumosPorPessoa.length === 0 ? (
          <div className="empty-state">
            <span className="empty-state__icon">📊</span>
            <p>Nenhuma pessoa cadastrada para exibir resumo.</p>
          </div>
        ) : (
          <div className="table-wrapper">
            <table className="data-table">
              <thead>
                <tr>
                  <th>Pessoa</th>
                  <th>Total Receitas</th>
                  <th>Total Despesas</th>
                  <th>Saldo Líquido</th>
                </tr>
              </thead>
              <tbody>
                {resumo.resumosPorPessoa.map((r) => (
                  <tr key={r.pessoaId}>
                    <td><strong>{r.pessoaNome}</strong></td>
                    <td className="data-table__valor data-table__valor--positive">
                      {formatarMoeda(r.totalReceitas)}
                    </td>
                    <td className="data-table__valor data-table__valor--negative">
                      {formatarMoeda(r.totalDespesas)}
                    </td>
                    <td className={`data-table__valor data-table__valor--${r.saldoLiquido >= 0 ? 'positive' : 'negative'}`}>
                      <strong>{formatarMoeda(r.saldoLiquido)}</strong>
                    </td>
                  </tr>
                ))}
              </tbody>
              {/* Rodapé com total geral da residência */}
              <tfoot>
                <tr className="data-table__footer">
                  <td><strong>🏠 Total Geral</strong></td>
                  <td className="data-table__valor data-table__valor--positive">
                    <strong>{formatarMoeda(resumo.totalGeralReceitas)}</strong>
                  </td>
                  <td className="data-table__valor data-table__valor--negative">
                    <strong>{formatarMoeda(resumo.totalGeralDespesas)}</strong>
                  </td>
                  <td className={`data-table__valor data-table__valor--${resumo.saldoLiquidoGeral >= 0 ? 'positive' : 'negative'}`}>
                    <strong>{formatarMoeda(resumo.saldoLiquidoGeral)}</strong>
                  </td>
                </tr>
              </tfoot>
            </table>
          </div>
        )}
      </div>
    </div>
  );
}
