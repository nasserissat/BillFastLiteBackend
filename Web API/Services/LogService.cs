using Microsoft.Extensions.Options;
using System.Reflection;

namespace WebAPI.Services;

public class LogService {
   static string nl = Environment.NewLine;
   static string ind = "\t";

   static string LogDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()!.Location)!, "errors");
   static string LogPath(string filename) =>
      Path.Combine(LogDirectory, filename);

   public static void LogException(Exception exception, string name) {
      static string ExceptionToString(Exception ex) =>
         string.Join(nl, new[] {
            $"Exception: {ex.GetType().Name}",
            $"Message: {ex.Message}",
            ex.InnerException is not null
               ? string.Join(nl, ExceptionToString(ex.InnerException).Split(nl).Select(l => $"{ind}{l}"))
               : null,
            $"Stack trace: {ex.StackTrace}"
         }.Where(l => l is not null));

      var date = DateTime.Now;

      Task.Run(() => {
         try {
            Directory.CreateDirectory(LogDirectory);
            File.WriteAllText(LogPath($"{date.Ticks}_{name}.error-log"), 
               string.Join(nl, new[] {
                  $"Time: {date:yyyy-MM-dd HH:mm:ss.fff}",
                  ExceptionToString(exception),
               }));
         } catch (Exception ex) { ; }
      });
   }

   public static void LogException(Exception exception) => LogException(exception, exception.GetType().Name);
}
