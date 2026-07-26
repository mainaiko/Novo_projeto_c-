namespace Backend.Exceptions;

// Exceção personalizada lançada quando uma regra de negócio é violada.
// Capturada pelo ExceptionMiddleware e convertida em resposta HTTP 422
// (Unprocessable Entity), retornando a mensagem de erro ao cliente.
public class BusinessException : Exception
{
    // Construtor que recebe a mensagem descritiva do erro de negócio
    // e a repassa para a classe base Exception.
    public BusinessException(string message) : base(message)
    {
    }
}
