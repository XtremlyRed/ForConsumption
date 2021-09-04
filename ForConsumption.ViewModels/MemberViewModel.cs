using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using ForConsumption.Common;
using ForConsumption.Common.Common;
using ForConsumption.ViewModels.Models;

using Plugins.ToolKits;
using Plugins.ToolKits.MVVM;

namespace ForConsumption.ViewModels
{
    public class MemberViewModel : BaseViewModel<MemberViewModel>
    {
        private readonly MemberModel memberModel = new MemberModel();
        public static ObservableCollection<Member> AllMembers { get; } = new ObservableCollection<Member>();

        public override string Title => "登录界面";

        public string LoginName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Password
        {
            get => GetValue<string>();
            set => SetValue(value);
        }


        public Member? Member { get; private set; }


        public ICommand LoginCommand => CommandBinder.TryBindExclusiveCommand(async (locker) =>
        {
            using IDisposable? @lock = locker.BeginLock();

            if (LoginName.IsNullOrEmpty())
            {
                await MessageShower.ShowAsync("用户名不能为空");
                return;
            }

            if (Password.IsNullOrEmpty())
            {
                await MessageShower.ShowAsync("密码不能为空");
                return;
            }
            await Login();

        }, e => MessageShower.ShowAsync(e.Message));

        public async Task InitializeAsyc()
        {
            Common.Common.JsonResult<Member[]>? result = await memberModel.GetListAsync();
            if (result is null || result.Result == false || result.Data is null || result.Data.Length == 0)
            {
                return;
            }
            AllMembers.Clear();
            AllMembers.AddItems(result.Data);

        }

        public async Task<bool> Login()
        {
            JsonResult<string[]>? result = await memberModel.Login(LoginName, Password);

            if (result.Result == false)
            {
                await MessageShower.ShowAsync(result.Message);
                return false;
            }

            string[]? infos = result.Data;

            if (infos is null || infos.Length != 2)
            {
                await MessageShower.ShowAsync("登录失败");
                return false;
            }

            TokenParameter.CurrentToken = infos[0];

            Member = JsonMapper.Deserialize<Member>(infos[1]);



            Task? task1 = PaymentModeViewModel.Instance.InitializeAsyc();
            Task? task2 = ConsumptionModeViewModel.Instance.InitializeAsyc();
            Task? task3 = InitializeAsyc();

            await Task.WhenAll(task1, task2, task3);

            Messenger.Default.Send(MessageKeys.LoginSuccessToken);

            return true;
        }

    }
}
