// Dados necessários para criar uma nova pessoa.
export interface CriarPessoaRequest {
  nome: string;
  idade: number;
}

// Representa uma pessoa retornada pela API.
export interface Pessoa {
  id: number;
  nome: string;
  idade: number;
}

// Tipos possíveis de transação financeira.
export type TipoTransacao = 'Despesa' | 'Receita';

// Dados necessários para criar uma nova transação.
export interface CriarTransacaoRequest {
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  pessoaId: number;
}

// Representa uma transação retornada pela API.
export interface Transacao {
  id: number;
  descricao: string;
  valor: number;
  tipo: string;
  pessoaId: number;
  pessoaNome: string;
}

// Resumo financeiro individual de uma pessoa.
export interface ResumoPessoa {
  pessoaId: number;
  pessoaNome: string;
  totalReceitas: number;
  totalDespesas: number;
  saldoLiquido: number;
}

// Resumo financeiro consolidado de toda a residência.
export interface ResumoGeral {
  resumosPorPessoa: ResumoPessoa[];
  totalGeralReceitas: number;
  totalGeralDespesas: number;
  saldoLiquidoGeral: number;
}

// Estrutura padrão de erro retornada pela API.
export interface ApiError {
  erro: string;
}
