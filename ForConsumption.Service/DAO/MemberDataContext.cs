using ForConsumption.Common;

using FreeSql;

namespace ForConsumption.Service.DAO
{
    public class MemberDataContext : DataContext
    {
        public MemberDataContext() : base()
        {
        }
        public DbSet<Member> Members { get; set; }

    }
}
