import { useState } from 'react';
import type { Pessoa } from '../types/types';
import { deletarPessoa, ApiRequestError } from '../services/api';

interface PessoaListProps {
  pessoas: Pessoa[];
  isLoading: boolean;
  onPessoaDeletada: () => void;
  onNotify: (message: string, type: 'success' | 'error') => void;
}

export default function PessoaList({ pessoas, isLoading, onPessoaDeletada, onNotify }: PessoaListProps) {
  const [deletingId, setDeletingId] = useState<number | null>(null);

  const handleDelete = async (pessoa: Pessoa) => {
    const confirmed = window.confirm(
      `Excluir "${pessoa.nome}"?\n\n⚠️ TODAS as transações desta pessoa serão apagadas do sistema.`
    );

    if (!confirmed) return;

    setDeletingId(pessoa.id);
    try {
      await deletarPessoa(pessoa.id);
      onNotify(`Pessoa "${pessoa.nome}" e suas transações foram excluídas.`, 'success');
      onPessoaDeletada();
    } catch (error) {
      if (error instanceof ApiRequestError) {
        onNotify(error.message, 'error');
      } else {
        onNotify('Erro ao excluir pessoa.', 'error');
      }
    } finally {
      setDeletingId(null);
    }
  };

  if (isLoading) {
    return <div className="data-card"><p>Carregando pessoas...</p></div>;
  }

  return (
    <div className="data-card">
      <h3 className="data-card__title">📋 Pessoas Cadastradas ({pessoas.length})</h3>

      {pessoas.length === 0 ? (
        <p className="empty-state">Nenhuma pessoa cadastrada ainda.</p>
      ) : (
        <table className="data-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Nome</th>
              <th>Idade</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {pessoas.map((p) => (
              <tr key={p.id}>
                <td>#{p.id}</td>
                <td>{p.nome}</td>
                <td>
                  {p.idade} anos{' '}
                  {p.idade < 18 && <span className="badge badge--warning">Menor</span>}
                </td>
                <td>
                  <button
                    className="btn btn--danger btn--small"
                    onClick={() => handleDelete(p)}
                    disabled={deletingId === p.id}
                  >
                    {deletingId === p.id ? '...' : '🗑️ Excluir'}
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
