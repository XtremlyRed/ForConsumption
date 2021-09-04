using System;
using System.Windows.Input;

using Plugins.ToolKits.MVVM;

namespace ForConsumption.ViewModels
{
    public class MyViewModel : BaseViewModel<MyViewModel>
    {

        public override string Title => "我的";

        public string MyName
        {
            get => GetValue("你的名字");
            set => SetValue(value);
        }

        public ICommand OpenConsumptionModeCommand => CommandBinder.BindExclusiveCommand(locker =>
        {
            using IDisposable? @lock = locker.BeginLock();

            Messenger.Default.Send(MessageKeys.OpenConsumptionModePageToken);

        });

        public ICommand OpenPaymentModeCommand => CommandBinder.BindExclusiveCommand(locker =>
        {
            using IDisposable? @lock = locker.BeginLock();

            Messenger.Default.Send(MessageKeys.OpenPaymentModePageToken);

        });

        public ICommand OpenSystemConfigCommand => CommandBinder.BindExclusiveCommand(locker =>
        {
            using IDisposable? @lock = locker.BeginLock();

            Messenger.Default.Send(MessageKeys.OpenSystemConfigPageToken);

        });
    }
}
