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
    public class PaymentModeViewModel : BaseViewModel<PaymentModeViewModel>
    {
        private readonly PaymentModel paymentModel = new PaymentModel();


        public async Task InitializeAsyc()
        {
            Common.Common.JsonResult<PaymentMode[]>? result = await paymentModel.GetListAsync();
            if (result is null || result.Result == false || result.Data is null || result.Data.Length == 0)
            {
                return;
            }
            AllPaymentModes.Clear();
            AllPaymentModes.AddItems(result.Data.OrderByDescending(i => i.Counter).ToArray());

            await RaisePropertyListChangedAsync(nameof(PaymentModes), nameof(AllPaymentModes));

        }

        public static ObservableCollection<PaymentMode> AllPaymentModes { get; } = new ObservableCollection<PaymentMode>();

        public override string Title => "支付类型";
        public ObservableCollection<PaymentMode> PaymentModes => AllPaymentModes;

        public ICommand AddNewPaymentModeCommand => CommandBinder.BindExclusiveCommand(locker =>
        {
            using System.IDisposable? @lock = locker.BeginLock();

            EditPaymentModeViewModel.AddInstance.Current = new PaymentMode();

            Messenger.Default.Send(MessageKeys.EditPaymentModePageToken, false);

        });


        public ICommand UpdatePaymentModeCommand => CommandBinder.BindExclusiveCommand<PaymentMode>(locker =>
        {
            using System.IDisposable? @lock = locker.BeginLock();

            EditPaymentModeViewModel.UpdateInstance.Current = locker.Parameter.Copy();

            Messenger.Default.Send(MessageKeys.EditPaymentModePageToken, true);

        });
    }
    public class EditPaymentModeViewModel : BaseViewModel
    {
        private readonly PaymentModel paymentModel = new PaymentModel();

        public new string Title
        {
            get => GetValue<string>();
            set => SetValue(value);
        }


        public PaymentMode Current
        {
            get => GetValue(new PaymentMode());
            set => SetValue(value);
        }


        public ICommand CompleteCommand => CommandBinder.TryBindExclusiveCommand(async locker =>
        {
            using System.IDisposable? @lock = locker.BeginLock();

            try
            {
                ObservableCollection<PaymentMode>? list = PaymentModeViewModel.Instance.PaymentModes;
                PaymentMode forst = list.FirstOrDefault(i => i.ID == Current.ID);
                if (forst is null)
                {
                    forst = Current;
                }
                else
                {
                    forst.Mode = Current.Mode;
                    forst.CreateTime = Current.CreateTime;
                }

                Common.Common.JsonResult<int>? result = await paymentModel.EditAsync(forst);

                if (result.Result)
                {
                    PaymentModeViewModel.Instance.InitializeAsyc().NoAwaiter();
                    Messenger.Default.Send(MessageKeys.GoBackToken);
                    return;
                }

                await MessageShower.ShowAsync(result.Message);

            }
            finally
            {
                Current = new PaymentMode();
            }


        }, e => MessageShower.ShowAsync(e.Message));




        private EditPaymentModeViewModel()
        {

        }

        public static EditPaymentModeViewModel UpdateInstance { get; } = new EditPaymentModeViewModel() { Title = "修改支付类型" };
        public static EditPaymentModeViewModel AddInstance { get; } = new EditPaymentModeViewModel() { Title = "添加支付类型" };
    }
}
