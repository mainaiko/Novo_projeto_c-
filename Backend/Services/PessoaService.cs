using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.DTOs;
using Backend.Exceptions;
using Backend.Models;

namespace Backend.Services;

// Implementação do serviço de pessoas.
// Contém as operações de criação, listagem e exclusão com validações de negócio.
public class PessoaService : IPessoaService
{
    private readonly AppDbContext _context;

    public PessoaService(AppDbContext context)
    {
        _context = context;
    }

    // Cria uma nova pessoa após validar nome e idade.
    // Retorna os dados da pessoa criada.
    public async Task<PessoaResponse> CriarAsync(CriarPessoaRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
            throw new BusinessException("O nome da pessoa é obrigatório e não pode ser vazio.");

        if (request.Idade < 0)
            throw new BusinessException("A idade não pode ser negativa.");

        var pessoa = new Pessoa
        {
            Nome = request.Nome.Trim(),
            Idade = request.Idade
        };

        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        return new PessoaResponse { Id = pessoa.Id, Nome = pessoa.Nome, Idade = pessoa.Idade };
    }

    // Lista todas as pessoas cadastradas ordenadas por nome.
    public async Task<IEnumerable<PessoaResponse>> ListarAsync()
    {
        return await _context.Pessoas
            .OrderBy(p => p.Nome)
            .AsNoTracking()
            .Select(p => new PessoaResponse { Id = p.Id, Nome = p.Nome, Idade = p.Idade })
            .ToListAsync();
    }

    // Remove uma pessoa pelo Id (cascade delete remove transações associadas).
    // Retorna true se encontrada e removida, false se não encontrada.
    public async Task<bool> DeletarAsync(int id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);
        if (pessoa == null) return false;

        // O cascade delete configurado no DbContext remove as transações automaticamente.
        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();
        return true;
    }
}
