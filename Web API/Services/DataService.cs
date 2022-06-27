using Core;
using Core.Models;
using Core.Services;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Services;

public class DataService : DbContext, IDataService {
   public DataService(DbContextOptions<DataService> options, IWebHostEnvironment env) : base(options) { this.env = env; }
   IWebHostEnvironment env;

   public async Task<T?> GetById<T>(params object[] keys) where T : class =>
      await Set<T>().FindAsync(keys);

   public async Task<T> Add<T>(T data) where T : class {
      var entity = (await Set<T>().AddAsync(data)).Entity;
      await SaveChangesAsync();
      return entity;
   }

   public async Task Update<T>(T data) where T : class {
      Set<T>().Update(data);
      await SaveChangesAsync();
   }

   public async Task Delete<T>(T data) where T : class {
      Set<T>().Remove(data);
      await SaveChangesAsync();
   }

   public async Task AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class {
      await Set<TEntity>().AddRangeAsync(entities);
      await SaveChangesAsync();
   }

   public async Task DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class {
      Set<TEntity>().RemoveRange(entities);
      await SaveChangesAsync();
   }

   public async Task Atomic(Func<Task> operation) {
      if (Database.IsInMemory()) {
         await operation();
      } else {
         using (var transaction = await Database.BeginTransactionAsync()) {
            try {
               await operation();
               await transaction.CommitAsync();
            } catch (Exception ex) {
               await transaction.RollbackAsync();
               throw new Exception("Exception in atomic data operation", ex);
            }
         }
      }
   }

   protected override void OnModelCreating(ModelBuilder m) {
      var entities = m.Model.GetEntityTypes().Select(et => et.ClrType).Where(t => !(t.Namespace?.StartsWith("System") ?? false));
      foreach (var (type, builder) in entities.Select(t => (t, m.Entity(t))).ToArray()) {
         if (type.GetProperties().Any(p => p.Name == "Id"))
            builder.HasKey("Id");
         else if (type.GetProperties().Any(p => p.Name.EndsWith("Id")))
            builder.HasKey(type.GetProperties()
               .Where(p => p.Name.EndsWith("Id"))
               .Select(p => p.Name)
               .ToArray());
         foreach (var property in type.GetProperties()) {
            if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
               builder.Property(property.Name).HasColumnType("decimal(18,6)");
         }
         foreach (var readonly_property in type.GetProperties().Where(p => !p.CanWrite))
            builder.Ignore(readonly_property.Name);
      }
      foreach (var (type, data) in Seeder.Seeds())
         m.Entity(type, e => e.HasData(data));
      if (env.IsDevelopment()) {
         foreach (var (type, data) in Seeder.DevelopmentSeeds())
            m.Entity(type, e => e.HasData(data));
      }
   }
}