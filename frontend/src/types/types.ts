//exporta a interface CriarPessoaRequest
export interface CriarPessoaRequest {
  nome: string;
  idade: number;
}

//exporta a interface Pessoa
export interface Pessoa {
  id: number;
  nome: string;
  idade: number;
}

//exporta o tipo Transacao
export type TipoTransacao = 'Despesa' | 'Receita';

//exporta a interface CriarTransacaoRequest
export interface CriarTransacaoRequest {
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  pessoaId: number;
}

//exporta a interface Transacao
export interface Transacao {
  id: number;
  descricao: string;
  valor: number;
  tipo: string;
  pessoaId: number;
  pessoaNome: string;
}

//exporta a interface ResumoPessoa
export interface ResumoPessoa {
  pessoaId: number;
  pessoaNome: string;
  totalReceitas: number;
  totalDespesas: number;
  saldoLiquido: number;
}

//exporta a interface ResumoGeral
export interface ResumoGeral {
  resumosPorPessoa: ResumoPessoa[];
  totalGeralReceitas: number;
  totalGeralDespesas: number;
  saldoLiquidoGeral: number;
}

//exporta a interface ApiError
export interface ApiError {
  erro: string;
}
