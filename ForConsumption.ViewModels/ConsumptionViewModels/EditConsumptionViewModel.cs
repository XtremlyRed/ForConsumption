using System;
using System.Linq;
using System.Windows.Input;

using ForConsumption.Common;
using ForConsumption.ViewModels.Models;

using Plugins.ToolKits;
using Plugins.ToolKits.MVVM;

namespace ForConsumption.ViewModels
{
    public class EditConsumptionViewModel : BaseViewModel
    {
        private ConsumptionItem current;

        private EditConsumptionViewModel()
        {
        }

        public static EditConsumptionViewModel DisplayInstance { get; } = new EditConsumptionViewModel() { Title = "账单详情", DeleteVisible=true };

        public static EditConsumptionViewModel UpdateInstance { get; } = new EditConsumptionViewModel() { Title = "修改账单" };
        public static EditConsumptionViewModel AddInstance { get; } = new EditConsumptionViewModel() { Title = "添加账单" };

        public ConsumptionItem Current
        {
            get => current;
            set
            {
                current = value;

                RaisePropertyChanged(nameof(Current));


                MoneyString = value is null || value.Money < 0.01M ? string.Empty : value.Money.ToString("F2");

                PaymentMode? paymentMode = PaymentModeViewModel.AllPaymentModes.FirstOrDefault(i => i.ID == value?.PaymentId);
                if (paymentMode is null)
                {
                    paymentMode = PaymentModeViewModel.AllPaymentModes.FirstOrDefault();
                }
                PaymentMode = paymentMode;

                CategoryMode? categoryMode = ConsumptionModeViewModel.AllConsumptionModes.FirstOrDefault(i => i.ID == value?.CategoryId);
                if (categoryMode is null)
                {
                    categoryMode = ConsumptionModeViewModel.AllConsumptionModes.FirstOrDefault();
                }
                CategoryMode = categoryMode;
            }
        }

        public string MoneyString { get; set; }

        public CategoryMode CategoryMode
        {
            get => GetValue<CategoryMode>();
            set => SetValue(value);
        }

        public PaymentMode PaymentMode
        {
            get => GetValue<PaymentMode>();
            set => SetValue(value);
        }

        public new string Title
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public ICommand ModifyCommand => CommandBinder.TryBindExclusiveCommand(async locker =>
        {
            using IDisposable? @lock = locker.BeginLock();
            await System.Threading.Tasks.Task.CompletedTask;
            UpdateInstance.Current = Current;
            Messenger.Default.Send(MessageKeys.NavigatToAddConsumptionViewToken, true);


        }, e => MessageShower.ShowAsync(e.Message));

        public ICommand ComplateCommand => CommandBinder.TryBindExclusiveCommand(async locker =>
        {
            using IDisposable? @lock = locker.BeginLock();
            if (!decimal.TryParse(MoneyString, out decimal number) || number <= 0.01M)
            {
                await MessageShower.ShowAsync("请输入正确的金额");
                return;
            }

            Current.Money = number;

            Current.Category = CategoryMode.Mode;
            Current.Payment = PaymentMode.Mode;

            Current.CategoryId = ConsumptionModeViewModel.AllConsumptionModes.FirstOrDefault(i => i.Mode == Current.Category)?.ID ?? 0;
            Current.PaymentId = PaymentModeViewModel.AllPaymentModes.FirstOrDefault(i => i.Mode == Current.Payment)?.ID ?? 0;

            if (Current.ID <= 0)
            {
                Current.OwnerId = MemberViewModel.Instance.Member?.ID ?? 0;
            }

            Common.Common.JsonResult? jsonResult = await ForConsumptionModel.Instance.UpdateConsumptionItemAsync(Current);

            if (!jsonResult.Result)
            {
                await MessageShower.ShowAsync(jsonResult.Message);
                return;
            }

            ConsumptionListViewModel.Instance.InitializeAsync().NoAwaiter();

            Messenger.Default.Send(MessageKeys.GoBackToken);

        }, e => MessageShower.ShowAsync(e.Message));



        public  bool DeleteVisible
        {
            get => GetValue(false);
            set => SetValue(value);
        }

        public ICommand DeleteCommand => CommandBinder.TryBindExclusiveCommand(async locker=> 
        {
            using (locker.BeginLock()) ;

            if (DeleteVisible == false)
            {
                return;
            }

            var confirmResult =await MessageShower.ConfirmAsync("确认删除吗???");
            if (confirmResult == false)
            {
                return ;
            }

            Common.Common.JsonResult? jsonResult = await ForConsumptionModel.Instance.DeleteConsumptionItemAsync(Current);

            if (!jsonResult.Result)
            {
                await MessageShower.ShowAsync(jsonResult.Message);
                return;
            }


            ConsumptionListViewModel.Instance.InitializeAsync().NoAwaiter();

            Messenger.Default.Send(MessageKeys.GoBackToken);

        }, e => MessageShower.ShowAsync(e.Message));
    }
}
