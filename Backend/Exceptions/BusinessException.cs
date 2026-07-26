namespace Backend.Exceptions;

// Exceção customizada para violações de regras de negócio.
// É capturada pelo <see cref="Backend.Middleware.ExceptionMiddleware"/> e convertida
// em uma resposta HTTP 422 (Unprocessable Entity) com mensagem amigável.
// 
// Método GET e SET para a exceção
public class BusinessException : Exception
{
    // Método GET e SET
    // Cria uma nova exceção de regra de negócio com a mensagem de erro especificada.
    // Parametro message: Mensagem descritiva do erro de negócio.
    public BusinessException(string message) : base(message)
    {
    }
}
