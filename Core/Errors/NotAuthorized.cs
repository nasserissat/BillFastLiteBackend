namespace Core.Errors;
public class NotAuthorized : Error {
   public NotAuthorized(string? message = null) : base(message ?? "No esta autorizado para realizar esta operación.") { }
}
