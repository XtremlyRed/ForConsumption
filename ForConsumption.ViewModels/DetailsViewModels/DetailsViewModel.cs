using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ForConsumption.Common;
using ForConsumption.ViewModels.Models;
using Plugins.ToolKits;
using Plugins.ToolKits.MVVM;

namespace ForConsumption.ViewModels
{
    public class DetailsViewModel : BaseViewModel<DetailsViewModel>
    {
        private readonly int pageSize = 10;
        private int currentPageIndex;
        private int totalCount;

        public DetailsViewModel()
        {
            InitializeAsync().NoAwaiter();
        }

        public override string Title => "账单";

        public ObservableCollection<ItemsDisplay> ConsumptionItems { get; } = new();


        public ICommand OpenDetailsCommand => CommandBinder.TryBindExclusiveCommand<ConsumptionItem>(async locker =>
        {
            using var @lock = locker.BeginLock();

            await Task.CompletedTask;

            EditConsumptionViewModel.DisplayInstance.Current = locker.Parameter.Copy();

            Messenger.Default.Send(MessageKeys.NavigatToConsumptionDetaisViewToken, true);
        }, async e => await MessageShower.ShowAsync(e.Message));


        public ICommand LoadMoreCommand => CommandBinder.TryBindExclusiveCommand(async locker =>
        {
            using var @lock = locker.BeginLock();


            if (ConsumptionItems.Count < 3) return;

            if (ConsumptionItems.Count >= totalCount && totalCount > 0) return;
            currentPageIndex += 1;
            var jsonResult = await ForConsumptionModel.Instance.GetPageInfosAsync(currentPageIndex);

            if (!jsonResult.Result)
            {
                await MessageShower.ShowAsync(jsonResult.Message);
                return;
            }

            if (jsonResult.Data is null || jsonResult.Data.Length == 0) return;

            totalCount = jsonResult.Value;

            var itemsDist = jsonResult.Data.DataFill()
                .GroupBy(i => i.CreateTime.ToString("yyyy-MM-dd"))
                .ToDictionary(i => i.Key, i => i.ToList());
            itemsDist.ForEach(i =>
            {
                var exist = ConsumptionItems.FirstOrDefault(iq => iq.Header == i.Key);
                if (exist is null)
                {
                    exist = new ItemsDisplay {Header = i.Key};
                    ConsumptionItems.Add(exist);
                }

                exist.AddItems(i.Value);

                exist.TotalMoney = exist.Sum(iq => iq.Money);
            });

            RaisePropertyChanged(nameof(ConsumptionItems));
        }, async e => await MessageShower.ShowAsync(e.Message));


        public Task InitializeAsync()
        {
            return Invoker.RunAsync(async () =>
            {
                currentPageIndex = 1;

                var jsonResult = await ForConsumptionModel.Instance.GetPageInfosAsync(currentPageIndex);

                if (!jsonResult.Result)
                {
                    await MessageShower.ShowAsync(jsonResult.Message);
                    return;
                }

                ConsumptionItems.Clear();

                if (jsonResult.Data is null || jsonResult.Data.Length == 0) return;

                var items = jsonResult.Data.DataFill()
                    .GroupBy(i => i.CreateTime.ToString("yyyy-MM-dd"))
                    .ToDictionary(i => i.Key, i => i.ToList())
                    .Select(i =>
                    {
                        var item = new ItemsDisplay {Header = i.Key};
                        item.AddItems(i.Value);
                        item.TotalMoney = item.Sum(iq => iq.Money);
                        return item;
                    }).ToArray();

                Messenger.Default.Send(MessageKeys.RunOnUIThread, new Action(() => ConsumptionItems.AddItems(items)));
            });
        }


        #region 刷新

        public bool IsRefreshing
        {
            get => GetValue(false);
            set => SetValue(value);
        }

        public ICommand RefreshCommand => CommandBinder.TryBindExclusiveCommand(async locker =>
        {
            using var @lock = locker.BeginLock();
            IsRefreshing = true;
            try
            {
                await InitializeAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }, async e => await MessageShower.ShowAsync(e.Message));

        #endregion
    }
}