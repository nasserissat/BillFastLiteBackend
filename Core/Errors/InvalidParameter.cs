namespace Core.Errors;

public class InvalidParameter: Error {
   public InvalidParameter(string? message = null) : base(message ?? "Parámetro inválido.") { }
}