namespace Core.Services;

public interface IEncryptionService {
   string Encrypt(string data);
   string Decrypt(string cypher);

   string Encrypt<T>(T data);
   T Decrypt<T>(string cypher);
}