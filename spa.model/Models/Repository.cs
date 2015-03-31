using System;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace spa.model
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public DbContext DbContext { get; set; }
        public DbSet<T> DbSet { get; set; }

        public Repository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentException();
            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public virtual T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != System.Data.EntityState.Detached)
            {
                dbEntityEntry.State = System.Data.EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
        }

        public virtual void Update(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != System.Data.EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = System.Data.EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != System.Data.EntityState.Deleted)
            {
                dbEntityEntry.State = System.Data.EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
                Delete(entity);
        }
    }
}
