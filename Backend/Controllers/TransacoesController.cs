using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using Backend.Services;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]

// Controller responsável pelos endpoints de gerenciamento de transações financeiras.
public class TransacoesController : ControllerBase
{
    // Serviço de transações injetado via DI para operações de CRUD.
    private readonly ITransacaoService _transacaoService;

    public TransacoesController(ITransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    // GET /api/transacoes — Retorna a lista de todas as transações cadastradas.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransacaoResponse>>> Listar()
    {
        return Ok(await _transacaoService.ListarAsync());
    }

    // POST /api/transacoes — Cria uma nova transação financeira.
    // Recebe os dados no corpo da requisição e retorna HTTP 201 com a transação criada.
    [HttpPost]
    public async Task<ActionResult<TransacaoResponse>> Criar([FromBody] CriarTransacaoRequest request)
    {
        var transacao = await _transacaoService.CriarAsync(request);
        return CreatedAtAction(nameof(Listar), new { id = transacao.Id }, transacao);
    }
}
