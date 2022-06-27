namespace Core.Services;

public interface IResourceService {
   Task<string> Save(byte[] data, string mimetype);
   Task<string> Save(string prefix, byte[] data, string mimetype);

   Task<(byte[] data, string mimetype)> Load(string id);
   Task<(byte[] data, string mimetype)> Load(string prefix, string id);

   Task Remove(string id);
   Task Remove(string prefix, string id);

   Task<string> LoadAsDataUrl(string id);
   Task<string> LoadAsDataUrl(string prefix, string id);

   Task<bool> Exists(string id);
   Task<bool> Exists(string prefix, string id);
}