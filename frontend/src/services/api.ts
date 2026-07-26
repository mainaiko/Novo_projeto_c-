//importação das interfaces
import type {
  Pessoa,
  CriarPessoaRequest,
  Transacao,
  CriarTransacaoRequest,
  ResumoGeral,
  ApiError,
} from '../types/types';

//cria a constante API_BASE que define a rota base da API
const API_BASE = '/api';


//cria a classe ApiRequestError que estende Error
export class ApiRequestError extends Error {
  public statusCode: number;

  constructor(message: string, statusCode: number) {
    super(message);
    this.name = 'ApiRequestError';
    this.statusCode = statusCode;
  }
}

//função handleResponse que lida com a resposta da API
async function handleResponse<T>(response: Response): Promise<T> {
  if (!response.ok) {
    let errorMessage = 'Erro desconhecido ao comunicar com o servidor.';

    try {
      const errorData: ApiError = await response.json();
      if (errorData.erro) {
        errorMessage = errorData.erro;
      }
    } catch {
      errorMessage = `Erro ${response.status}: ${response.statusText}`;
    }

    throw new ApiRequestError(errorMessage, response.status);
  }

  // 204 No Content não tem body
  if (response.status === 204) {
    return undefined as T;
  }

  return response.json();
}

//função listarPessoas que busca todas as pessoas cadastradas
export async function listarPessoas(): Promise<Pessoa[]> {
  const response = await fetch(`${API_BASE}/pessoas`);
  return handleResponse<Pessoa[]>(response);
}

//função criarPessoa que cria uma nova pessoa na residência
export async function criarPessoa(data: CriarPessoaRequest): Promise<Pessoa> {
  const response = await fetch(`${API_BASE}/pessoas`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse<Pessoa>(response);
}

//função deletarPessoa que deleta uma pessoa e todas as suas transações (cascade).
export async function deletarPessoa(id: number): Promise<void> {
  const response = await fetch(`${API_BASE}/pessoas/${id}`, {
    method: 'DELETE',
  });
  return handleResponse<void>(response);
}

//função listarTransacoes que busca todas as transações cadastradas
export async function listarTransacoes(): Promise<Transacao[]> {
  const response = await fetch(`${API_BASE}/transacoes`);
  return handleResponse<Transacao[]>(response);
}

//função criarTransacao que cria uma nova transação financeira
export async function criarTransacao(data: CriarTransacaoRequest): Promise<Transacao> {
  const response = await fetch(`${API_BASE}/transacoes`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse<Transacao>(response);
}

//função obterResumo que busca o resumo financeiro consolidado da residência
export async function obterResumo(): Promise<ResumoGeral> {
  const response = await fetch(`${API_BASE}/resumo`);
  return handleResponse<ResumoGeral>(response);
}
