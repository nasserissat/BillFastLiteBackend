namespace WebAPI.Routes;

public static class Commands {
   public static void MapCommands(this WebApplication app) {
      app.MapPost("/login", (LoginParameters data, Context context) => context.Execute(async core => {
         var (User, Token) = await core.Login(data.Username, data.Password);
         return new { User, Token };
      }));
   }

   internal class LoginParameters {
      public string Username { get; set; }
      public string Password { get; set; }
   }
}
