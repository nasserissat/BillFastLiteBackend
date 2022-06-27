using Core.Errors;

namespace Core;

public static class Validation {
   static void Validate<T>(T data, string parameter_name, Func<T, bool> validator, string error_message) {
      if (!validator(data))
         throw new InvalidParameter($"Parámetro inválido: {parameter_name}\n - {error_message}");
   }
}