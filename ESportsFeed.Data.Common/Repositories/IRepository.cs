namespace ESportsFeed.Data.Common.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        IQueryable<TEntity> All();

        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(string id);

        Task AddAsync(TEntity entity);

        void AddRange(IEnumerable<TEntity> additions);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task<int> SaveChangesAsync();
    }

}
