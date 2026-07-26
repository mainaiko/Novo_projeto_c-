// using = importar bibliotecas externas que ajudam a criar a aplicação
using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using Backend.Services;

// namespace = serve para organizar o código em pastas
namespace Backend.Controllers;

//ApiController =  permite que o controller receba requisições HTTP
[ApiController]

//Route = Define a rota da API
[Route("api/[controller]")]

// classe pessoasController é uma classe que herda de ControllerBase
// ControllerBase fornece métodos e propriedades comuns a todos os controllers
// ControllerBase pode ser encontrado em microsoft.aspnetcore.mvc
public class PessoasController : ControllerBase
{
    private readonly IPessoaService _pessoaService;
    //PessoaService é uma classe que implementa a interface IPessoaService
    //IPessoaService é uma interface que define os métodos que o controller pode usar
    //_pessoaService é uma variável privada que recebe a instância de IPessoaService
    public PessoasController(IPessoaService pessoaService)
    {
        _pessoaService = pessoaService;
    }

    //HttpGet = define um método que será chamado quando uma requisição GET for feita para a rota /api/pessoas
    //Async = permite que o método seja executado de forma assíncrona
    //ActionResult = define o tipo de retorno do método
    //IEnumerable = define que o método retorna uma coleção de pessoas  
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PessoaResponse>>> Listar()
    {
        return Ok(await _pessoaService.ListarAsync());
    }

    //HttpPost = define um método que será chamado quando uma requisição POST for feita para a rota /api/pessoas
    //PessoaResponse = define o tipo de retorno do método
    //CriarPessoaRequest = define o tipo de retorno do método
    //FromBody = define que o método recebe o corpo da requisição
    //CreatedAtAction = define o código de status da resposta
    [HttpPost]
    public async Task<ActionResult<PessoaResponse>> Criar([FromBody] CriarPessoaRequest request)
    {
        var pessoa = await _pessoaService.CriarAsync(request);
        return CreatedAtAction(nameof(Listar), new { id = pessoa.Id }, pessoa);
    }

    //HttpDelete = define um método que será chamado quando uma requisição DELETE for feita para a rota /api/pessoas
    //IActionResult = define o tipo de retorno do método
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
