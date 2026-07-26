# Sistema de Controle de Gastos Residenciais

Sistema completo para gerenciamento financeiro de uma residência, permitindo cadastrar pessoas, registrar transações (receitas e despesas) e visualizar um dashboard com resumo financeiro.

---

## Tecnologias Utilizadas

| Camada | Tecnologia | Versão |
|--------|-----------|--------|
| **Back-end** | C# / ASP.NET Core Web API | .NET 10 |
| **Front-end** | React + TypeScript | React 19 / TS 6 |
| **Bundler** | Vite | 8.x |
| **Banco de Dados** | SQLite (via Entity Framework Core) | EF Core 10 |

> O SQLite foi escolhido para que os dados persistam após fechar a aplicação **sem necessidade de instalar um servidor de banco de dados externo** (MySQL, PostgreSQL, etc). O arquivo `gastos.db` é criado automaticamente na primeira execução.

---

## Pré-requisitos

Antes de rodar o projeto, certifique-se de ter instalado no seu computador:

### 1. .NET SDK 10 (ou superior)
- **Download:** [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
- **Verificar instalação:**
  ```bash
  dotnet --version
  ```
  Deve retornar algo como `10.0.xxx`

### 2. Node.js 18+ e npm
- **Download:** [https://nodejs.org](https://nodejs.org) (versão LTS recomendada)
- **Verificar instalação:**
  ```bash
  node --version
  npm --version
  ```

### 3. Git (opcional, para clonar o repositório)
- **Download:** [https://git-scm.com](https://git-scm.com)

---

## Passo a Passo para Iniciar a Aplicação

### Passo 1 — Clonar ou copiar o projeto

```bash
# Se tiver o repositório no Git:
git clone <URL_DO_REPOSITORIO>
cd Novo_projeto

# Ou simplesmente copie a pasta do projeto para o seu computador
```

### Passo 2 — Iniciar o Back-end (API)

Abra um terminal e execute:

```bash
# Entrar na pasta do backend
cd Backend

# Restaurar as dependências do .NET (pacotes NuGet)
dotnet restore

# Executar a API
dotnet run
```

A API irá iniciar e exibirá uma mensagem similar a:
```
Now listening on: http://localhost:5202
```

> **Importante:** Deixe este terminal aberto enquanto usa a aplicação. O backend precisa estar rodando para o frontend funcionar.

### Passo 3 — Iniciar o Front-end (React)

Abra **outro terminal** (sem fechar o do backend) e execute:

```bash
# Entrar na pasta do frontend
cd frontend

# Instalar as dependências do Node.js
npm install

# Iniciar o servidor de desenvolvimento
npm run dev
```

O Vite irá iniciar e exibirá:
```
Local: http://localhost:5173/
```

### Passo 4 — Acessar a aplicação

Abra o navegador e acesse:

```
http://localhost:5173
```

A aplicação estará funcionando com as 3 abas: **Pessoas**, **Transações** e **Dashboard**.

---

## Como o Backend e o Frontend Funcionam Juntos

### Visão Geral da Arquitetura

```
┌──────────────────────────┐         ┌──────────────────────────────┐
│   FRONTEND (React)       │         │   BACKEND (ASP.NET Core)     │
│   http://localhost:5173   │  HTTP   │   http://localhost:5202      │
│                          │ ──────> │                              │
│  - Formulários           │  JSON   │  - Controllers (rotas)       │
│  - Tabelas               │ <────── │  - Services (regras)         │
│  - Dashboard             │         │  - DbContext (banco SQLite)  │
└──────────────────────────┘         └──────────────┬───────────────┘
                                                    │
                                              ┌─────┴─────┐
                                              │ gastos.db  │
                                              │  (SQLite)  │
                                              └───────────┘
```

### Fluxo de Comunicação

1. O **Frontend** (React) roda no navegador do usuário na porta `5173`.
2. Quando o usuário realiza uma ação (ex: cadastrar uma pessoa), o frontend envia uma **requisição HTTP** (GET, POST ou DELETE) para a API.
3. O **Vite Proxy** (`vite.config.ts`) intercepta qualquer requisição que comece com `/api` e redireciona automaticamente para `http://localhost:5202` (onde o backend roda). Isso evita problemas de CORS durante o desenvolvimento.
4. O **Backend** recebe a requisição, processa a lógica de negócio no **Service** e interage com o banco **SQLite** via **Entity Framework Core**.
5. O backend retorna uma resposta **JSON** que o frontend usa para atualizar a tela.

### Endpoints da API

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/pessoas` | Lista todas as pessoas cadastradas |
| `POST` | `/api/pessoas` | Cadastra uma nova pessoa |
| `DELETE` | `/api/pessoas/{id}` | Exclui uma pessoa (e todas as suas transações) |
| `GET` | `/api/transacoes` | Lista todas as transações |
| `POST` | `/api/transacoes` | Cadastra uma nova transação |
| `GET` | `/api/resumo` | Retorna o resumo financeiro (dashboard) |

---

## Regras de Negócio

### 1. Deleção em Cascata
Quando uma pessoa é excluída, **todas as transações associadas** a ela são automaticamente removidas do banco de dados.

### 2. Restrição de Menores de Idade
Pessoas com **menos de 18 anos** podem registrar apenas **Despesas**. O cadastro de **Receitas** é bloqueado.

### 3. Validações de Entrada
- Nome da pessoa não pode ser vazio.
- Idade não pode ser negativa.
- Valor da transação deve ser maior que zero.
- A pessoa associada à transação deve existir no banco.
- O tipo da transação deve ser válido (Despesa ou Receita).

---

## Solução de Problemas

### "A API não responde" ou "Erro de conexão"
- Certifique-se de que o backend está rodando (`dotnet run` no terminal).
- Verifique se a porta `5202` está livre.

### "npm run dev" dá erro
- Execute `npm install` antes para instalar as dependências.
- Verifique se o Node.js está instalado (`node --version`).

### "dotnet run" dá erro de pacotes
- Execute `dotnet restore` para restaurar os pacotes NuGet.

### O banco de dados não é criado
- O arquivo `gastos.db` é criado automaticamente na primeira execução do backend.
- Se precisar resetar os dados, basta deletar o arquivo `gastos.db` e reiniciar o backend.

---

