using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using Backend.Services;

namespace Backend.Controllers;

//ApiController = permite que o controller receba requisições HTTP
[ApiController]

//Route = define a rota da API
[Route("api/[controller]")]

//ResumoController herda de ControllerBase
public class ResumoController : ControllerBase
{
    //readonly = somente leitura
    private readonly ITransacaoService _transacaoService; // _transacaoService e uma variavel privada que recebe a instancia de ITransacaoService
    //ITransacaoService e uma interface que define os métodos que o controller pode usar

    //construtor que recebe o service como parametro
    public ResumoController(ITransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    // Retorna o resumo financeiro consolidado da residência.
    // 
    // Inclui:
    // - Totais por pessoa (receitas, despesas, saldo líquido).
    // - Total geral (soma de todas receitas, despesas e saldo da residência).
    // 
    // Pessoas sem transações aparecem com valores zerados.

    [HttpGet]
    public async Task<ActionResult<ResumoGeralDto>> ObterResumo()
    {
        var resumo = await _transacaoService.ObterResumoAsync();
        return Ok(resumo);
    }
}
