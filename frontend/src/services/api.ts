// Importação dos tipos utilizados nas requisições e respostas da API.
import type {
  Pessoa,
  CriarPessoaRequest,
  Transacao,
  CriarTransacaoRequest,
  ResumoGeral,
  ApiError,
} from '../types/types';

// URL base para todas as chamadas à API do backend.
const API_BASE = '/api';


// Erro personalizado para falhas de comunicação com a API, incluindo o status HTTP.
export class ApiRequestError extends Error {
  public statusCode: number;

  constructor(message: string, statusCode: number) {
    super(message);
    this.name = 'ApiRequestError';
    this.statusCode = statusCode;
  }
}

// Processa a resposta HTTP: extrai o JSON em caso de sucesso ou lança ApiRequestError.
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

  // HTTP 204 não possui corpo na resposta.
  if (response.status === 204) {
    return undefined as T;
  }

  return response.json();
}

// Busca todas as pessoas cadastradas (GET /api/pessoas).
export async function listarPessoas(): Promise<Pessoa[]> {
  const response = await fetch(`${API_BASE}/pessoas`);
  return handleResponse<Pessoa[]>(response);
}

// Cadastra uma nova pessoa (POST /api/pessoas).
export async function criarPessoa(data: CriarPessoaRequest): Promise<Pessoa> {
  const response = await fetch(`${API_BASE}/pessoas`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse<Pessoa>(response);
}

// Remove uma pessoa e suas transações (DELETE /api/pessoas/{id}).
export async function deletarPessoa(id: number): Promise<void> {
  const response = await fetch(`${API_BASE}/pessoas/${id}`, {
    method: 'DELETE',
  });
  return handleResponse<void>(response);
}

// Busca todas as transações cadastradas (GET /api/transacoes).
export async function listarTransacoes(): Promise<Transacao[]> {
  const response = await fetch(`${API_BASE}/transacoes`);
  return handleResponse<Transacao[]>(response);
}

// Cadastra uma nova transação financeira (POST /api/transacoes).
export async function criarTransacao(data: CriarTransacaoRequest): Promise<Transacao> {
  const response = await fetch(`${API_BASE}/transacoes`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  return handleResponse<Transacao>(response);
}

// Busca o resumo financeiro consolidado da residência (GET /api/resumo).
export async function obterResumo(): Promise<ResumoGeral> {
  const response = await fetch(`${API_BASE}/resumo`);
  return handleResponse<ResumoGeral>(response);
}
