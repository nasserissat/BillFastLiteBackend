namespace Core.Errors;

public class NotFound : Error {
   public NotFound(string? message = null) : base(message ?? "No encontrado.") { }
}
