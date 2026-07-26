using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.DTOs;
using Backend.Exceptions;
using Backend.Models;

namespace Backend.Services;

//TransacaoService implementa a interface ITransacaoService
//TransacaoService e o servico que lida com as operacoes de CRUD (Create, Read, Update, Delete) das transacoes
public class TransacaoService : ITransacaoService
{
    private readonly AppDbContext _context;

    public TransacaoService(AppDbContext context)
    {
        _context = context;
    }

    //Cria uma nova transacao na residência.
    //CriaAsync recebe como parametro um CriarTransacaoRequest e retorna uma TransacaoResponse.
    public async Task<TransacaoResponse> CriarAsync(CriarTransacaoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Descricao))
            throw new BusinessException("A descrição da transação é obrigatória.");

        if (request.Valor <= 0)
            throw new BusinessException("O valor da transação deve ser maior que zero.");

        var pessoa = await _context.Pessoas.FindAsync(request.PessoaId);
        if (pessoa == null)
            throw new BusinessException($"Pessoa com ID {request.PessoaId} não foi encontrada.");

        // Regra de negócio explícita e legível:
        // Menores de 18 anos só podem registrar Despesas, nunca Receitas.
        if (pessoa.Idade < 18 && request.Tipo == TipoTransacao.Receita)
        {
            throw new BusinessException(
                $"A pessoa \"{pessoa.Nome}\" é menor de idade ({pessoa.Idade} anos) e só pode registrar Despesas."
            );
        }

        var transacao = new Transacao
        {
            Descricao = request.Descricao.Trim(),
            Valor = request.Valor,
            Tipo = request.Tipo,
            PessoaId = request.PessoaId
        };

        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();

        return MapToResponse(transacao, pessoa.Nome);
    }

    //Lista todas as transacoes cadastradas ordenadas por ID em ordem decrescente.
    public async Task<IEnumerable<TransacaoResponse>> ListarAsync()
    {
        var transacoes = await _context.Transacoes
            .Include(t => t.Pessoa)
            .OrderByDescending(t => t.Id)
            .AsNoTracking()
            .ToListAsync();

        return transacoes.Select(t => MapToResponse(t, t.Pessoa?.Nome ?? "Desconhecido"));
    }

    //Obtem um resumo geral das transacoes por pessoa.
    //Recebe um ResumoGeralDto contendo o resumo geral das transacoes por pessoa.
    public async Task<ResumoGeralDto> ObterResumoAsync()
    {
        var pessoas = await _context.Pessoas
            .Include(p => p.Transacoes)
            .OrderBy(p => p.Nome)
            .AsNoTracking()
            .ToListAsync();

        var resumosPorPessoa = pessoas.Select(pessoa => new ResumoPessoaDto
        {
            PessoaId = pessoa.Id,
            PessoaNome = pessoa.Nome,
            TotalReceitas = pessoa.Transacoes.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor),
            TotalDespesas = pessoa.Transacoes.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor)
        }).ToList();

        return new ResumoGeralDto
        {
            ResumosPorPessoa = resumosPorPessoa,
            TotalGeralReceitas = resumosPorPessoa.Sum(r => r.TotalReceitas),
            TotalGeralDespesas = resumosPorPessoa.Sum(r => r.TotalDespesas)
        };
    }

    //Metodo privado que mapeia uma Transacao para uma TransacaoResponse.
    private static TransacaoResponse MapToResponse(Transacao transacao, string pessoaNome) => new()
    {
        Id = transacao.Id,
        Descricao = transacao.Descricao,
        Valor = transacao.Valor,
        Tipo = transacao.Tipo.ToString(),
        PessoaId = transacao.PessoaId,
        PessoaNome = pessoaNome
    };
}
