using Core.Errors;
using Core.Models;
using Core.Services;
using Core.Views;

namespace Core;

public partial class Logic {
   public Logic(IDataService data, IAuthenticationService authentication, IEncryptionService encryption, IResourceService resources) {
      Data           = data;
      Authentication = authentication;
      Encryption     = encryption;
      Resources      = resources;
   }

   readonly IDataService           Data;
   readonly IAuthenticationService Authentication;
   readonly IEncryptionService     Encryption;
   readonly IResourceService       Resources;

   public async Task<(UserView user, string token)> Login(string username, string password) {
      var user = await Data.GetUserByCaseInsensitiveUsername(username);
      if (user is null)
         throw new NotAuthenticated("Credenciales inválidas.");
      if (!Authentication.VerifyPassword(user, password))
         throw new NotAuthenticated("Credenciales inválidas.");
      {
         user.DateLastLogin = DateTime.Now;
         await Data.Update(user);
      }
      var token = Authentication.GenerateToken(user);
      return (UserView.From(user), token);
   }

   public async Task<User> Authenticate(string? token) {
      if (string.IsNullOrEmpty(token))
         throw new NotAuthenticated("Debe iniciar sesión.");
      var (success, user_id, date_issued) = Authentication.Authenticate(token);
      if (!success)
         throw new NotAuthenticated("Debe iniciar sesión.");
      var user = await Data.GetUserById(user_id!.Value);
      if (user is null)
         throw new NotAuthenticated("Token inválido.");
      if (user.DateLastPasswordChange > date_issued)
         throw new NotAuthenticated("Sesión ha sido invalidada por un cambio de contraesña.");
      return user;
   }
}