// Importação dos pacotes necessários para o controller
using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using Backend.Services;

// Namespace que agrupa os controllers da API
namespace Backend.Controllers;

// Habilita validação automática do ModelState e binding de parâmetros
[ApiController]

// Define a rota base como /api/pessoas
[Route("api/[controller]")]

// Controller responsável pelos endpoints de gerenciamento de pessoas.
// Herda de ControllerBase, que fornece métodos auxiliares como Ok(), NotFound(), etc.
public class PessoasController : ControllerBase
{
    private readonly IPessoaService _pessoaService;
    // Construtor com injeção de dependência do serviço de pessoas.
    // O container de DI resolve automaticamente a implementação de IPessoaService.
    public PessoasController(IPessoaService pessoaService)
    {
        _pessoaService = pessoaService;
    }

    // GET /api/pessoas — Retorna a lista de todas as pessoas cadastradas.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PessoaResponse>>> Listar()
    {
        return Ok(await _pessoaService.ListarAsync());
    }

    // POST /api/pessoas — Cadastra uma nova pessoa.
    // Recebe os dados no corpo da requisição e retorna HTTP 201 com a pessoa criada.
    [HttpPost]
    public async Task<ActionResult<PessoaResponse>> Criar([FromBody] CriarPessoaRequest request)
    {
        var pessoa = await _pessoaService.CriarAsync(request);
        return CreatedAtAction(nameof(Listar), new { id = pessoa.Id }, pessoa);
    }

    // DELETE /api/pessoas/{id} — Remove uma pessoa e suas transações (cascade).
    // Retorna HTTP 204 se deletada ou HTTP 404 se não encontrada.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(int id)
    {
        var deletada = await _pessoaService.DeletarAsync(id);
        if (!deletada)
        {
            return NotFound(new { erro = $"Pessoa com ID {id} não foi encontrada." });
        }

        return NoContent();
    }
}
