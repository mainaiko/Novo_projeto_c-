//todos os imports necessários
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

//type - define os tipos de abas disponíveis na navegação principal.
type Tab = 'pessoas' | 'transacoes' | 'dashboard';

//interface - define as propriedades do componente Notification
interface NotificationItem {
  id: number;
  message: string;
  type: 'success' | 'error';
}

//Componente principal da aplicação
//Gerencia a navegação por abas e o estado global de dados
//As abas são: Pessoas, Transações e Dashboard (Resumo)
export default function App() {
  const [activeTab, setActiveTab] = useState<Tab>('pessoas');
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  const [loadingPessoas, setLoadingPessoas] = useState(true);
  const [loadingTransacoes, setLoadingTransacoes] = useState(true);
  const [notifications, setNotifications] = useState<NotificationItem[]>([]);
  const [dashboardKey, setDashboardKey] = useState(0);

  //Carrega a lista de pessoas do backend.
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

  //Carrega a lista de transações do backend.
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

  //Adiciona uma notificação à fila de exibição.
  //Cada notificação recebe um ID único para controle de remoção.
  const addNotification = useCallback((message: string, type: 'success' | 'error') => {
    const id = Date.now();
    setNotifications((prev) => [...prev, { id, message, type }]);
  }, []);

  //Remove uma notificação da fila pelo ID.
  const removeNotification = useCallback((id: number) => {
    setNotifications((prev) => prev.filter((n) => n.id !== id));
  }, []);

  //Callback chamado quando dados mudam (criação/deleção).
  //Recarrega as listas e incrementa a chave do dashboard para forçar refresh.
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
