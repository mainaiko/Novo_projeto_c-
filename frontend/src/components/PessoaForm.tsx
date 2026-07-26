import { useState } from 'react';
import { criarPessoa, ApiRequestError } from '../services/api';

// Props do componente PessoaForm.
// onPessoaCriada: callback após criação. onNotify: exibe notificações ao usuário.
interface PessoaFormProps {
  onPessoaCriada: () => void;
  onNotify: (message: string, type: 'success' | 'error') => void;
}
// Formulário para cadastro de uma nova pessoa (nome e idade).
export default function PessoaForm({ onPessoaCriada, onNotify }: PessoaFormProps) {
  const [nome, setNome] = useState('');
  const [idade, setIdade] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const nomeTrimmed = nome.trim();
    const idadeNum = parseInt(idade, 10);

    if (!nomeTrimmed || isNaN(idadeNum) || idadeNum < 0) {
      onNotify('Preencha um nome e idade válidos.', 'error');
      return;
    }

    setIsLoading(true);
    try {
      await criarPessoa({ nome: nomeTrimmed, idade: idadeNum });
      onNotify(`Pessoa "${nomeTrimmed}" cadastrada com sucesso!`, 'success');
      setNome('');
      setIdade('');
      onPessoaCriada();
    } catch (error) {
      if (error instanceof ApiRequestError) {
        onNotify(error.message, 'error');
      } else {
        onNotify('Erro ao cadastrar pessoa.', 'error');
      }
    } finally {
      setIsLoading(false);
    }
  };

  // Renderiza o formulário com campos de nome e idade.
  return (
    <form className="form-card" onSubmit={handleSubmit}>
      <h3 className="form-card__title">👤 Nova Pessoa</h3>

      <div className="form-card__field">
        <label htmlFor="pessoa-nome">Nome</label>
        <input
          id="pessoa-nome"
          type="text"
          placeholder="Nome completo"
          value={nome}
          onChange={(e) => setNome(e.target.value)}
          required
        />
      </div>

      <div className="form-card__field">
        <label htmlFor="pessoa-idade">Idade</label>
        <input
          id="pessoa-idade"
          type="number"
          placeholder="Ex: 25"
          value={idade}
          onChange={(e) => setIdade(e.target.value)}
          min={0}
          required
        />
      </div>

      <button type="submit" className="btn btn--primary" disabled={isLoading}>
        {isLoading ? 'Cadastrando...' : 'Cadastrar Pessoa'}
      </button>
    </form>
  );
}
