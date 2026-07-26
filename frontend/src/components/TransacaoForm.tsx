import { useState } from 'react';
import type { Pessoa, CriarTransacaoRequest, TipoTransacao } from '../types/types';
import { criarTransacao, ApiRequestError } from '../services/api';

//interface-define as propriedades do componente TransacaoForm
//pessoas: Pessoa[] - lista de pessoas
//onTransacaoCriada: () => void - função para criar transação
//onNotify: (message: string, type: 'success' | 'error') => void - função para notificar
interface TransacaoFormProps {
  pessoas: Pessoa[];
  onTransacaoCriada: () => void;
  onNotify: (message: string, type: 'success' | 'error') => void;
}

//exporta o componente TransacaoForm
export default function TransacaoForm({ pessoas, onTransacaoCriada, onNotify }: TransacaoFormProps) {
  const [descricao, setDescricao] = useState('');
  const [valor, setValor] = useState('');
  const [tipo, setTipo] = useState<TipoTransacao>('Despesa');
  const [pessoaId, setPessoaId] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const valorNum = parseFloat(valor);
    if (isNaN(valorNum) || valorNum <= 0) {
      onNotify('O valor deve ser maior que zero.', 'error');
      return;
    }

    if (!pessoaId) {
      onNotify('Selecione uma pessoa.', 'error');
      return;
    }

    setIsLoading(true);
    try {
      const request: CriarTransacaoRequest = {
        descricao: descricao.trim(),
        valor: valorNum,
        tipo,
        pessoaId: Number(pessoaId),
      };

      await criarTransacao(request);
      onNotify('Transação cadastrada com sucesso!', 'success');

      setDescricao('');
      setValor('');
      setTipo('Despesa');
      setPessoaId('');
      onTransacaoCriada();
    } catch (error) {
      if (error instanceof ApiRequestError) {
        onNotify(error.message, 'error');
      } else {
        onNotify('Erro ao cadastrar transação.', 'error');
      }
    } finally {
      setIsLoading(false);
    }
  };

  //renderiza o componente TransacaoForm
  //o formulário possui três campos: descrição, valor e tipo
  return (
    <form className="form-card" onSubmit={handleSubmit}>
      <h3 className="form-card__title">💰 Nova Transação</h3>

      <div className="form-card__field">
        <label htmlFor="transacao-pessoa">Pessoa</label>
        <select
          id="transacao-pessoa"
          value={pessoaId}
          onChange={(e) => setPessoaId(e.target.value)}
          required
        >
          <option value="">Selecione uma pessoa...</option>
          {pessoas.map((p) => (
            <option key={p.id} value={p.id}>
              {p.nome} ({p.idade} anos){p.idade < 18 ? ' ⚠️ Menor' : ''}
            </option>
          ))}
        </select>
      </div>

      <div className="form-card__row">
        <div className="form-card__field">
          <label htmlFor="transacao-tipo">Tipo</label>
          <select
            id="transacao-tipo"
            value={tipo}
            onChange={(e) => setTipo(e.target.value as TipoTransacao)}
          >
            <option value="Despesa">🔴 Despesa</option>
            <option value="Receita">🟢 Receita</option>
          </select>
        </div>

        <div className="form-card__field">
          <label htmlFor="transacao-valor">Valor (R$)</label>
          <input
            id="transacao-valor"
            type="number"
            placeholder="0,00"
            value={valor}
            onChange={(e) => setValor(e.target.value)}
            min="0.01"
            step="0.01"
            required
          />
        </div>
      </div>

      <div className="form-card__field">
        <label htmlFor="transacao-descricao">Descrição</label>
        <input
          id="transacao-descricao"
          type="text"
          placeholder="Ex: Conta de luz..."
          value={descricao}
          onChange={(e) => setDescricao(e.target.value)}
          required
        />
      </div>

      <button type="submit" className="btn btn--primary" disabled={isLoading || pessoas.length === 0}>
        {isLoading ? 'Cadastrando...' : 'Registrar Transação'}
      </button>

      {pessoas.length === 0 && (
        <p className="form-card__hint">
          ⚠️ Cadastre uma pessoa antes de registrar transações.
        </p>
      )}
    </form>
  );
}
