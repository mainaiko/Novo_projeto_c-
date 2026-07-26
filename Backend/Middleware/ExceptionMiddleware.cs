using System.Net;
using System.Text.Json;
using Backend.Exceptions;

namespace Backend.Middleware;


// Middleware global de tratamento de exceções.
// Intercepta erros não tratados em todo o pipeline HTTP e retorna
// respostas JSON padronizadas, evitando try-catch repetitivos nos controllers.

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // Executa o próximo middleware do pipeline dentro de um try-catch.
    // BusinessException → HTTP 422 com mensagem do erro de negócio.
    // Outras exceções → HTTP 500 com mensagem genérica (detalhes são logados).
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

    // Monta e envia uma resposta JSON de erro no formato { "erro": "mensagem" }.
    private static async Task WriteErrorResponse(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = JsonSerializer.Serialize(new { erro = message });
        await context.Response.WriteAsync(response);
    }
}
