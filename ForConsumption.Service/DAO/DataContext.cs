using ForConsumption.Common;

using FreeSql;

using Plugins.ToolKits;

namespace ForConsumption.Service.DAO
{





    public abstract class DataContext : DbContext
    {

        public DbSet<ConsumptionItem> ConsumptionItems { get; set; }

        protected DataContext() : base(Injecter.Resolve<IFreeSql>(), new DbContextOptions())
        {

        }

        public DbSet<TEntity> Table<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

    }

}
