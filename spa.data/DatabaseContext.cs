using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using spa.model;

namespace spa.data
{
    internal class DatabaseContext : DbContext
    {
        public DbSet<UserProfile> Users { get; set; }

        public DatabaseContext() : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
