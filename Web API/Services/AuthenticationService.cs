using Core.Services;

namespace WebAPI.Services;

public class AuthenticationService: IAuthenticationService {
   public AuthenticationService(IEncryptionService encryption) {
      Encryption = encryption;
   }

   readonly IEncryptionService Encryption;
}