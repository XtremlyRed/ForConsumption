using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using ForConsumption.Common;

using Plugins.ToolKits;
using Plugins.ToolKits.MVVM;

namespace ForConsumption.ViewModels.StatisticsViewModels
{
    public class StatisticsCategotyViewModel : BaseViewModel<StatisticsCategotyViewModel>
    {
        public override string Title => GetValue<string>();

        public ObservableCollection<ItemsDisplay> ItemsDisplayList { get; } = new ObservableCollection<ItemsDisplay>();

        public void SetItem(ItemsDisplay item)
        {
            item.ThrowIfNull();

            SetValue(item.Header, nameof(Title));

            ItemsDisplayList.Clear();
            ItemsDisplay[] items = item
             .GroupBy(i => i.CreateTime.ToString("yyyy-MM-dd"))
             .ToDictionary(i => i.Key, i => i.ToList())
             .Select(i =>
             {
                 ItemsDisplay? item = new ItemsDisplay { Header = i.Key, };
                 item.AddItems(i.Value);
                 item.TotalMoney = item.Sum(iq => iq.Money);
                 //item.Percent = (double)(item.TotalMoney / TotalMoney) * 100;
                 return item;
             }).ToArray();

            ItemsDisplayList.AddItems(items);
            RaisePropertyChanged(nameof(ItemsDisplayList));
        }


        public ICommand OpenDetailsCommand => CommandBinder.TryBindExclusiveCommand<ConsumptionItem>(async locker =>
        {
            using IDisposable? @lock = locker.BeginLock();

            await System.Threading.Tasks.Task.CompletedTask;

            EditConsumptionViewModel.DisplayInstance.Current = locker.Parameter.Copy();

            Messenger.Default.Send(MessageKeys.NavigatToConsumptionDetaisViewToken, true);

        }, async e => await MessageShower.ShowAsync(e.Message));

    }
}
