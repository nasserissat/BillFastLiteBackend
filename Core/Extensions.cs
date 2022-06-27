namespace Core;

public static class Extensions {
   public static string PascalCaseToUpperSnakeCase(this string text) =>
      string.Join(string.Empty, (text ?? string.Empty).Select(c => char.IsUpper(c) ? $"_{c}" : $"{c}")).TrimStart('_').ToUpper();

   public static string PascalCaseToTitleCase(this string text) =>
      string.Join(string.Empty, (text ?? string.Empty).Select(c => char.IsUpper(c) ? $" {c}" : $"{c}")).Trim()
         .Replace(" De ", " de ")
         .Replace(" Del ", " del ")
         .Replace(" El ", " el ")
         .Replace(" La ", " la ")
         .Replace(" Los ", " los ")
         .Replace(" Las ", " las ")
         .Replace(" En ", " en ");

   public static bool HasBeenSince(this DateTime date, double weeks = 0, double days = 0, double hours = 0, double minutes = 0, double seconds = 0, double milliseconds = 0, DateTime? from = null) {
      double target_milliseconds =
         weeks   * 7 * 24 * 60 * 60 * 1000 +
         days    * 24 * 60 * 60 * 1000 +
         hours   * 60 * 60 * 1000 +
         minutes * 60 * 1000 +
         seconds * 1000 +
         milliseconds;

      return (date - (from ?? DateTime.Now)).TotalMilliseconds >= target_milliseconds;
   }

   public static string? NullIfNothing(this string? text) => string.IsNullOrWhiteSpace(text) ? null : text;
}