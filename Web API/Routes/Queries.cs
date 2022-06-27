namespace WebAPI.Routes;

public static class Queries {
   public static void MapQueries(this WebApplication app) {
      // app.MapGet("/company/all", (Context ctx) => ctx.ExecuteAuthenticated(
      //    (user, core) => core.Companies(user)));

      // app.MapGet("/company/{id:guid}/promotion/all", (Guid id, Context ctx) => ctx.ExecuteAuthenticated(
      //    (user, core) => core.Promotions(user, id)));
   }
}
