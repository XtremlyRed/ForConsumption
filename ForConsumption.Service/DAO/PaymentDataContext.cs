using ForConsumption.Common;

using FreeSql;

namespace ForConsumption.Service.DAO
{
    public class PaymentDataContext : DataContext
    {
        public PaymentDataContext() : base()
        {
        }
        public DbSet<PaymentMode> PaymentModes { get; set; }

    }
}
