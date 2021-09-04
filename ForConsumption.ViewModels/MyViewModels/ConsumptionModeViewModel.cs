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
    public class ConsumptionModeViewModel : BaseViewModel<ConsumptionModeViewModel>
    {
        private readonly CategoryModel categoryModel = new CategoryModel();


        public async Task InitializeAsyc()
        {
            Common.Common.JsonResult<CategoryMode[]>? result = await categoryModel.GetListAsync();
            if (result is null || result.Result == false || result.Data is null || result.Data.Length == 0)
            {
                return;
            }

            AllConsumptionModes.Clear();

            AllConsumptionModes.AddItems(result.Data.OrderByDescending(i => i.Counter).ToArray());

            await RaisePropertyListChangedAsync(nameof(ConsumptionModes), nameof(AllConsumptionModes));

        }

        public override string Title => "消费类型";
        public static ObservableCollection<CategoryMode> AllConsumptionModes { get; } = new ObservableCollection<CategoryMode>();

        public ObservableCollection<CategoryMode> ConsumptionModes => AllConsumptionModes;


        public ICommand AddNewConsumptionModeCommand => CommandBinder.BindExclusiveCommand(locker =>
        {
            using System.IDisposable? @lock = locker.BeginLock();

            EditConsumptionModeViewModel.AddInstance.Current = new CategoryMode();

            Messenger.Default.Send(MessageKeys.EditConsumptionModePageToken, false);

        });


        public ICommand UpdateConsumptionModeCommand => CommandBinder.BindExclusiveCommand<CategoryMode>(locker =>
        {
            using System.IDisposable? @lock = locker.BeginLock();

            EditConsumptionModeViewModel.UpdateInstance.Current = locker.Parameter.Copy();

            Messenger.Default.Send(MessageKeys.EditConsumptionModePageToken, true);

        });
    }

    public class EditConsumptionModeViewModel : BaseViewModel
    {
        private readonly CategoryModel categoryModel = new CategoryModel();


        public new string Title
        {
            get => GetValue<string>();
            set => SetValue(value);
        }


        public CategoryMode Current
        {
            get => GetValue(new CategoryMode());
            set => SetValue(value);
        }


        public ICommand CompleteCommand => CommandBinder.TryBindExclusiveCommand(async locker =>
        {
            using System.IDisposable? @lock = locker.BeginLock();

            try
            {
                ObservableCollection<CategoryMode>? list = ConsumptionModeViewModel.Instance.ConsumptionModes;
                CategoryMode? forst = list.FirstOrDefault(i => i.ID == Current.ID);
                if (forst is null)
                {
                    forst = Current;
                }
                else
                {
                    forst.Mode = Current.Mode;
                    forst.CreateTime = Current.CreateTime;
                }

                Common.Common.JsonResult<int>? result = await categoryModel.EditAsync(forst);

                if (result.Result == true)
                {
                    ConsumptionModeViewModel.Instance.InitializeAsyc().NoAwaiter();
                    Messenger.Default.Send(MessageKeys.GoBackToken);
                    return;
                }

                await MessageShower.ShowAsync(result.Message);

            }
            finally
            {
                Current = new CategoryMode();
            }


        }, e => MessageShower.ShowAsync(e.Message));




        private EditConsumptionModeViewModel()
        {

        }

        public static EditConsumptionModeViewModel UpdateInstance { get; } = new EditConsumptionModeViewModel() { Title = "修改消费类型" };
        public static EditConsumptionModeViewModel AddInstance { get; } = new EditConsumptionModeViewModel() { Title = "添加消费类型" };
    }
}
