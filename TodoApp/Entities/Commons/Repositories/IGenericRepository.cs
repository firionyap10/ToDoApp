namespace TodoApp.API.Entities.Commons.Repositories
{
    public interface IGenericRepository<T>
    {
        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<bool> SaveAll();
    }
}
