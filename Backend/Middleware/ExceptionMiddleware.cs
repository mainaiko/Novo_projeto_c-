using System.Net;
using System.Text.Json;
using Backend.Exceptions;

namespace Backend.Middleware;


// Middleware global de tratamento de exceções.
// Intercepta exceções não tratadas e retorna respostas HTTP padronizadas.
// concentra aqui o tratamento de erros, evita try-catch repetitivos nos controllers e garante um formato de resposta de erro consistente.

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // Executa o middleware, capturando exceções e convertendo-as em respostas HTTP apropriadas.
    // <see cref="BusinessException"/>: HTTP 422 com mensagem amigável.
    // Outras exceções: HTTP 500 com mensagem genérica (detalhes são logados, não expostos).
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning("Regra de negócio violada: {Message}", ex.Message);
            await WriteErrorResponse(context, HttpStatusCode.UnprocessableEntity, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno não esperado.");
            await WriteErrorResponse(
                context,
                HttpStatusCode.InternalServerError,
                "Ocorreu um erro interno no servidor. Tente novamente mais tarde."
            );
        }
    }

    // Escreve uma resposta de erro JSON padronizada no formato { "erro": "mensagem" }.
    private static async Task WriteErrorResponse(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = JsonSerializer.Serialize(new { erro = message });
        await context.Response.WriteAsync(response);
    }
}
