namespace Core.Services;

public interface IDataService {
   Task<T?> GetById<T>(params object[] keys) where T : class;
   Task<T> Add<T>(T entity) where T : class;
   Task AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
   Task Update<T>(T entity) where T : class;
   Task Delete<T>(T entity) where T : class;
   Task DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
   Task Atomic(Func<Task> operation);
}