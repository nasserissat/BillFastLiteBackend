namespace Core.Errors;

public class Error: Exception {
   public string Code    { get; protected set; }
   public string Message { get; protected set; }

   public Error(string code, string message) { Code = code; Message = message; }
   public Error(string message) { Code = GetType().Name.PascalCaseToUpperSnakeCase(); Message = message; }
   public Error(Exception ex) : this("INTERNAL_ERROR", $"[{ex.GetType().Name}] {ex.Message}") { }
}