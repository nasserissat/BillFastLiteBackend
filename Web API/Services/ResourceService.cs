using Core.Services;
using Microsoft.Extensions.Options;

namespace WebAPI.Services;

public class ResourceService : IResourceService {
   public ResourceService(IConfiguration config) {
      ResourcesDirectory = config.GetSection("Directories").GetValue<string>("Resources");
   }

   readonly string ResourcesDirectory;

   void EnsureDirectory(string prefix) {
      Directory.CreateDirectory(Path.Combine(ResourcesDirectory, prefix));
   }

   bool ExistsResource(string prefix, string id) {
      EnsureDirectory(prefix);
      return Directory.GetFiles(Path.Combine(ResourcesDirectory, prefix), $"{id}.*").Any();
   }

   string ResourcePath(string prefix, Guid id, string mimetype) {
      return Path.Combine(ResourcesDirectory, prefix, $"{id}.{mimetype.Replace("/", "_")}");
   }

   string ExistingResourcePath(string prefix, string id) {
      var files = Directory.GetFiles(Path.Combine(ResourcesDirectory, prefix), $"{id}.*");
      return files.First();
   }

   public async Task<string> Save(string prefix, byte[] data, string mimetype) {
      EnsureDirectory(prefix);
      Guid id;
      do {
         id = Guid.NewGuid();
      } while (ExistsResource(prefix, id.ToString()));
      await File.WriteAllBytesAsync(ResourcePath(prefix, id, mimetype), data);
      return id.ToString();
   }

   public Task<string> Save(byte[] data, string mimetype) => Save(string.Empty, data, mimetype);

   public async Task<bool> Exists(string prefix, string id) {
      return ExistsResource(prefix, id);
   }

   public Task<bool> Exists(string id) => Exists(string.Empty, id);

   public async Task<(byte[] data, string mimetype)> Load(string prefix, string id) {
      if (!ExistsResource(prefix, id))
         throw new FileNotFoundException($"Resource '{id}' not found.");
      string resource_path = ExistingResourcePath(prefix, id);
      var data = await File.ReadAllBytesAsync(resource_path);
      string mimetype = Path.GetExtension(resource_path).Replace("_", "/");
      return (data, mimetype);
   }

   public Task<(byte[] data, string mimetype)> Load(string id) => Load(string.Empty, id);

   public async Task Remove(string prefix, string id) {
      File.Delete(ExistingResourcePath(prefix, id));
   }

   public Task Remove(string id) => Remove(string.Empty, id);

   public async Task<string> LoadAsDataUrl(string prefix, string id) {
      var (data, mimetype) = await Load(prefix, id);
      return $"data:{mimetype};base64,{Convert.ToBase64String(data)}";
   }

   public Task<string> LoadAsDataUrl(string id) => LoadAsDataUrl(string.Empty, id);
}
