using ForConsumption.Common;

using FreeSql;

namespace ForConsumption.Service.DAO
{
    public class CategoryDataContext : DataContext
    {
        public CategoryDataContext() : base()
        {
        }
        public DbSet<CategoryMode> CategoryModes { get; set; }

    }

}
