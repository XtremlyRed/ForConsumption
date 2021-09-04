using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using ForConsumption.Common;
using ForConsumption.Common.Common;
using ForConsumption.ViewModels.Models;

using Plugins.ToolKits;
using Plugins.ToolKits.MVVM;

namespace ForConsumption.ViewModels.StatisticsViewModels
{
    public class StatisticsViewModel : BaseViewModel<StatisticsViewModel>
    {
        public override string Title => "统计";

        public StatisticsViewModel()
        {

        }

        public Task InitializeAsync()
        {
            return Invoker.RunAsync(async () =>
            {
                JsonResult<ConsumptionItem[]>? jsonResult = await ForConsumptionModel.Instance.GetConsumptionInfosByMouth(CurrentMouth);

                if (jsonResult.Result == false)
                {
                    await MessageShower.ShowAsync(jsonResult.Message);
                    return;
                }

                ItemsDisplayList.Clear();

                ConsumptionItem[]? list = jsonResult.Data;

                if (list is null || list.Length == 0)
                {
                    TotalMoney = 0;
                    TotalCount = 0;
                    return;
                }

                list.DataFill();

                TotalMoney = list.Sum(i => i.Money);
                TotalCount = list.Count();

                ItemsDisplay[]? items = list
                .GroupBy(i => i.Category)
                .ToDictionary(i => i.Key, i => i.ToList())
                .Select(i =>
                {
                    ItemsDisplay? item = new ItemsDisplay { Header = i.Key, };
                    item.AddItems(i.Value);
                    item.TotalCount = i.Value.Count();
                    item.TotalMoney = item.Sum(iq => iq.Money);
                    item.Percent = (double)(item.TotalMoney / TotalMoney) * 100;
                    return item;
                }).ToArray();

                ItemsDisplayList.AddItems(items);

                RaisePropertyChanged(nameof(ItemsDisplayList));
            });
        }

        public Task UpdateAsync()
        {
            return InitializeAsync();
        }

        public ObservableCollection<ItemsDisplay> ItemsDisplayList { get; } = new ObservableCollection<ItemsDisplay>();

        public ICommand StatisticsCategotyCommand => CommandBinder.TryBindExclusiveCommand<ItemsDisplay>(async locker =>
        {
            using IDisposable? @lock = locker.BeginLock();

            StatisticsCategotyViewModel.Instance.SetItem(locker.Parameter);

            Messenger.Default.Send(MessageKeys.StatisticsCategotyViewToken);

            await Task.CompletedTask;

        }, async e => await MessageShower.ShowAsync(e.Message));


        public ICommand LastMonthCommand => CommandBinder.TryBindExclusiveCommand(async locker =>
        {
            using IDisposable? @lock = locker.BeginLock();

            DateTime target = CurrentMouth.AddMonths(-1);

            CurrentMouth = target;

            await InitializeAsync();


        }, async e => await MessageShower.ShowAsync(e.Message));

        public ICommand NextMonthCommand => CommandBinder.TryBindExclusiveCommand(async locker =>
        {
            using IDisposable? @lock = locker.BeginLock();
            DateTime target = CurrentMouth.AddMonths(1);

            if (target > DateTime.Now)
            {
                return;
            }

            CurrentMouth = target;

            await InitializeAsync();


        }, async e => await MessageShower.ShowAsync(e.Message));


        public decimal TotalMoney
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public int TotalCount
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public DateTime CurrentMouth
        {
            get => GetValue<DateTime>(DateTime.Now);
            set => SetValue(value);
        }
    }
}
