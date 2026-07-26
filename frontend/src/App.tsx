// Importações de hooks do React, tipos, serviços e componentes.
import { useState, useEffect, useCallback } from 'react';
import type { Pessoa, Transacao } from './types/types';
import { listarPessoas, listarTransacoes } from './services/api';
import PessoaForm from './components/PessoaForm';
import PessoaList from './components/PessoaList';
import TransacaoForm from './components/TransacaoForm';
import TransacaoList from './components/TransacaoList';
import Dashboard from './components/Dashboard';
import Notification from './components/Notification';
import './App.css';

// Tipos de abas disponíveis na navegação principal.
type Tab = 'pessoas' | 'transacoes' | 'dashboard';

// Estrutura de uma notificação toast exibida ao usuário.
interface NotificationItem {
  id: number;
  message: string;
  type: 'success' | 'error';
}

// Componente principal da aplicação.
// Gerencia navegação por abas (Pessoas, Transações, Dashboard) e estado global dos dados.
export default function App() {
  const [activeTab, setActiveTab] = useState<Tab>('pessoas');
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  const [loadingPessoas, setLoadingPessoas] = useState(true);
  const [loadingTransacoes, setLoadingTransacoes] = useState(true);
  const [notifications, setNotifications] = useState<NotificationItem[]>([]);
  const [dashboardKey, setDashboardKey] = useState(0);

  // Busca a lista de pessoas no backend.
  const carregarPessoas = useCallback(async () => {
    setLoadingPessoas(true);
    try {
      const data = await listarPessoas();
      setPessoas(data);
    } catch (error) {
      console.error('Erro ao carregar pessoas:', error);
    } finally {
      setLoadingPessoas(false);
    }
  }, []);

  // Busca a lista de transações no backend.
  const carregarTransacoes = useCallback(async () => {
    setLoadingTransacoes(true);
    try {
      const data = await listarTransacoes();
      setTransacoes(data);
    } catch (error) {
      console.error('Erro ao carregar transações:', error);
    } finally {
      setLoadingTransacoes(false);
    }
  }, []);

  // Carregamento inicial ao montar o componente
  useEffect(() => {
    carregarPessoas();
    carregarTransacoes();
  }, [carregarPessoas, carregarTransacoes]);

  // Adiciona uma notificação à fila com ID único para controle de remoção.
  const addNotification = useCallback((message: string, type: 'success' | 'error') => {
    const id = Date.now();
    setNotifications((prev) => [...prev, { id, message, type }]);
  }, []);

  // Remove uma notificação da fila pelo seu ID.
  const removeNotification = useCallback((id: number) => {
    setNotifications((prev) => prev.filter((n) => n.id !== id));
  }, []);

  // Callback acionado após criação/exclusão de dados.
  // Recarrega as listas e força atualização do dashboard.
  const handleDataChange = useCallback(() => {
    carregarPessoas();
    carregarTransacoes();
    setDashboardKey((prev) => prev + 1);
  }, [carregarPessoas, carregarTransacoes]);

  return (
    <div className="app">
      {/* Notificações toast */}
      <div className="notifications-container">
        {notifications.map((n) => (
          <Notification
            key={n.id}
            message={n.message}
            type={n.type}
            onClose={() => removeNotification(n.id)}
          />
        ))}
      </div>

      {/* Header */}
      <header className="app-header">
        <div className="app-header__content">
          <h1 className="app-header__title">
            <span className="app-header__logo">🏠</span>
            Controle de Gastos Residenciais
          </h1>
          <p className="app-header__subtitle">
            Gerencie as finanças da sua residência
          </p>
        </div>
      </header>

      {/* Navegação por abas */}
      <nav className="tab-nav">
        <button
          className={`tab-nav__item ${activeTab === 'pessoas' ? 'tab-nav__item--active' : ''}`}
          onClick={() => setActiveTab('pessoas')}
        >
          <span className="tab-nav__icon">👤</span>
          Pessoas
        </button>
        <button
          className={`tab-nav__item ${activeTab === 'transacoes' ? 'tab-nav__item--active' : ''}`}
          onClick={() => setActiveTab('transacoes')}
        >
          <span className="tab-nav__icon">💰</span>
          Transações
        </button>
        <button
          className={`tab-nav__item ${activeTab === 'dashboard' ? 'tab-nav__item--active' : ''}`}
          onClick={() => setActiveTab('dashboard')}
        >
          <span className="tab-nav__icon">📊</span>
          Dashboard
        </button>
      </nav>

      {/* Conteúdo da aba ativa */}
      <main className="app-main">
        {activeTab === 'pessoas' && (
          <div className="tab-content">
            <PessoaForm
              onPessoaCriada={handleDataChange}
              onNotify={addNotification}
            />
            <PessoaList
              pessoas={pessoas}
              isLoading={loadingPessoas}
              onPessoaDeletada={handleDataChange}
              onNotify={addNotification}
            />
          </div>
        )}

        {activeTab === 'transacoes' && (
          <div className="tab-content">
            <TransacaoForm
              pessoas={pessoas}
              onTransacaoCriada={handleDataChange}
              onNotify={addNotification}
            />
            <TransacaoList
              transacoes={transacoes}
              isLoading={loadingTransacoes}
            />
          </div>
        )}

        {activeTab === 'dashboard' && (
          <div className="tab-content">
            <Dashboard refreshKey={dashboardKey} />
          </div>
        )}
      </main>
    </div>
  );
}
