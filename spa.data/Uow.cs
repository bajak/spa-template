using System;
using System.Data.Entity;
using WebMatrix.WebData;
using spa.model;

namespace spa.data
{
    public class Uow : IUow, IDisposable
    {
        public IRepository<UserProfile> Users { get { return GetStandardRepo<UserProfile>(); } }

        public Uow(IRepositoryProvider repositoryProvider)
        {
            CreateDbContext();

            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }

        private void InitializeWebSecurity()
        {
            WebSecurity.InitializeDatabaseConnection(
                "DefaultConnection", "UserProfile", "UserId", "Username", true);
        }

        protected void CreateDbContext()
        {
            Database.SetInitializer(
                new DatabaseInitializer(
                    InitializeWebSecurity));
            DbContext = new DatabaseContext();
            DbContext.Database.Initialize(true);
            DbContext.Configuration.ProxyCreationEnabled = true;
            DbContext.Configuration.LazyLoadingEnabled = false;
            DbContext.Configuration.ValidateOnSaveEnabled = false;
            if (!WebSecurity.Initialized)
                InitializeWebSecurity();
        }

        protected IRepositoryProvider RepositoryProvider { get; set; }

        private IRepository<T> GetStandardRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }
        private T GetRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>();
        }

        private DatabaseContext DbContext { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (DbContext != null)
            {
                DbContext.Dispose();
            }
        }
    }
}