using System.Collections.Generic;
using System.Linq;

using ForConsumption.Common;

using Plugins.ToolKits;

namespace ForConsumption.ViewModels
{
    public static class ConsumptionItemExtension
    {

        public static ICollection<ConsumptionItem> DataFill(this ICollection<ConsumptionItem> items)
        {
            if (items == null)
            {
                return new List<ConsumptionItem>();
            }

            Dictionary<int, string>? consumptionDict = ConsumptionModeViewModel.AllConsumptionModes.ToDictionary(i => i.ID, i => i.Mode);
            Dictionary<int, string>? paymentDict = PaymentModeViewModel.AllPaymentModes.ToDictionary(i => i.ID, i => i.Mode);
            Dictionary<int, string>? member = MemberViewModel.AllMembers.ToDictionary(i => i.ID, i => i.LoginName);
            items.ForEach(i =>
            {
                i.Category = consumptionDict[i.CategoryId];
                i.Payment = paymentDict[i.PaymentId];
                i.Owner = member[i.OwnerId];
            });

            return items;
        }

    }
}
