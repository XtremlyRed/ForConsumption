using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using ForConsumption.Common;
using ForConsumption.Common.Common;
using ForConsumption.ViewModels.Models;

using Plugins.ToolKits;
using Plugins.ToolKits.MVVM;
namespace ForConsumption.ViewModels
{
    public class ConsumptionListViewModel : BaseViewModel<ConsumptionListViewModel>
    {
        public ConsumptionListViewModel()
        {
            ItemsSource = new ObservableCollection<ConsumptionItem>();
            InitializeAsync();
        }

        public DateTime Today
        {
            get => GetValue(DateTime.Now);
            set => SetValue(value);
        }


        public override string Title => "记账";
        public decimal TodayMoney
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public bool IsRefreshing
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }


        public ObservableCollection<ConsumptionItem> ItemsSource { get; }

        public Task InitializeAsync()
        {

            return Invoker.RunAsync(async () =>
            {

                JsonResult<ConsumptionItem[], int>? jsonResult = await ForConsumptionModel.Instance.LoadTodayInfosAsync();

                ItemsSource.Clear();
                TodayMoney = 0;
                if (jsonResult.Data is null || jsonResult.Data.Length == 0)
                {
                    return;
                }

                ICollection<ConsumptionItem>? array = jsonResult.Data.DataFill();
                if (array != null && array.Count > 0)
                {
                    ItemsSource.AddItems(array);
                    TodayMoney = ItemsSource.Sum(i => i.Money);
                } 
            });
        }

        public ICommand AddConsumptionCommand => CommandBinder.TryBindExclusiveCommand(async locker =>
        {
            using System.IDisposable? @lock = locker.BeginLock();

            await System.Threading.Tasks.Task.CompletedTask;

            EditConsumptionViewModel.AddInstance.Current = new ConsumptionItem();

            Messenger.Default.Send(MessageKeys.NavigatToAddConsumptionViewToken, false);

        }, e => MessageShower.ShowAsync(e.Message));


        public ICommand OpenDetailsCommand => CommandBinder.TryBindExclusiveCommand<ConsumptionItem>(async locker =>
        {
            using System.IDisposable? @lock = locker.BeginLock();

            await System.Threading.Tasks.Task.CompletedTask;

            EditConsumptionViewModel.DisplayInstance.Current = locker.Parameter.Copy();

            Messenger.Default.Send(MessageKeys.NavigatToConsumptionDetaisViewToken, true);


        }, e => MessageShower.ShowAsync(e.Message));


        public ICommand RefreshCommand => CommandBinder.TryBindExclusiveCommand(async locker =>
        {
            try
            {
                using System.IDisposable? @lock = locker.BeginLock();
                IsRefreshing = true;
                await InitializeAsync();
            }
            finally
            {
                IsRefreshing = false;
            }

        }, e => MessageShower.ShowAsync(e.Message));
    }
}
