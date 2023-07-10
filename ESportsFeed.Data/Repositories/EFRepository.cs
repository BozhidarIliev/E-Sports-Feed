namespace ESportsFeed.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ESportsFeed.Data.Common.Repositories;
    using ESportsFeed.Data.Models;
    using ESportsFeed.Services.Messaging.Interfaces;

    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IActiveEntity
    {
        private readonly IMessageProducer messageProducer;

        public EfRepository(ApplicationDbContext context, IMessageProducer messageProducer)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.DbSet = this.Context.Set<TEntity>();
            this.messageProducer = messageProducer;
        }

        protected DbSet<TEntity> DbSet { get; set; }

        protected ApplicationDbContext Context { get; set; }

        public virtual IQueryable<TEntity> All() => this.DbSet.Where(x => x.IsActive);

        public virtual Task<List<TEntity>> GetAllAsync() => this.DbSet.ToListAsync();

        public virtual async Task<TEntity> GetByIdAsync(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await DbSet.FirstOrDefaultAsync(x => x.ID == id);
        }

        public virtual Task AddAsync(TEntity entity) => this.DbSet.AddAsync(entity).AsTask();

        public void AddRange(IEnumerable<TEntity> additions)
        {
            Context.AddRange(additions);
        }

        public virtual void Update(TEntity entity)
        {
            var entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity) => this.DbSet.Remove(entity);

        public virtual async Task<int> SaveChangesAsync()
        {
            return await this.Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Context?.Dispose();
            }
        }
    }
}