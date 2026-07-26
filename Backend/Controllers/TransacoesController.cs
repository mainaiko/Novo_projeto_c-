using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using Backend.Services;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]

//TransacoesController herda de ControllerBase
public class TransacoesController : ControllerBase
{
    //metodo construtor que recebe o service como parametro
    private readonly ITransacaoService _transacaoService;

    public TransacoesController(ITransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    //METODO GET QUE LISTA TODAS AS TRANSAÇÕES
    //HttpGet = define um método que será chamado quando uma requisição GET for feita para a rota /api/transacoes
    //IEnumerable = define que o método retorna uma coleção de transações
    //ActionResult = define o tipo de retorno do método
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransacaoResponse>>> Listar()
    {
        return Ok(await _transacaoService.ListarAsync());
    }

    //METODO POST QUE CRIA UMA NOVA TRANSAÇÃO
    //HttpPost = define um método que será chamado quando uma requisição POST for feita para a rota /api/transacoes
    //ActionResult = define o tipo de retorno do método
    //TransacaoResponse = define o tipo de retorno do método
    [HttpPost]
    public async Task<ActionResult<TransacaoResponse>> Criar([FromBody] CriarTransacaoRequest request)
    {
        var transacao = await _transacaoService.CriarAsync(request);
        return CreatedAtAction(nameof(Listar), new { id = transacao.Id }, transacao);
    }
}
