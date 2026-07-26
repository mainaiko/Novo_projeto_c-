using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using Backend.Services;

namespace Backend.Controllers;

// Habilita validação automática do ModelState e binding de parâmetros
[ApiController]

// Define a rota base como /api/resumo
[Route("api/[controller]")]

// Controller responsável pelo endpoint de resumo financeiro da residência.
public class ResumoController : ControllerBase
{
    // Serviço de transações injetado via DI, utilizado para calcular o resumo financeiro.
    private readonly ITransacaoService _transacaoService;

    // Construtor com injeção de dependência do serviço de transações.
    public ResumoController(ITransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    // GET /api/resumo — Retorna o resumo financeiro consolidado da residência.
    // Inclui totais por pessoa (receitas, despesas, saldo) e totais gerais.
    // Pessoas sem transações aparecem com valores zerados.

    [HttpGet]
    public async Task<ActionResult<ResumoGeralDto>> ObterResumo()
    {
        var resumo = await _transacaoService.ObterResumoAsync();
        return Ok(resumo);
    }
}
