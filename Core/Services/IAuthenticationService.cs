using Core.Models;

namespace Core.Services;

public interface IAuthenticationService {
   bool VerifyPassword(User user, string password);
   string GenerateToken(User user, DateTime? issued = null);
   (bool success, Guid? user_id, DateTime? date_issued) Authenticate(string token);
}