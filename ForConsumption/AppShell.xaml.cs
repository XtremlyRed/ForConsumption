using System.Collections.Generic;

using ForConsumption.Assists;
using ForConsumption.ViewModels;
using ForConsumption.Views;
using ForConsumption.Views.ConsumptionViews;
using ForConsumption.Views.MyViews;
using ForConsumption.Views.MyViews.ConsumptionModeViews;
using ForConsumption.Views.MyViews.PaymentModeViews;
using ForConsumption.Views.MyViews.SystemConfigViews;
using ForConsumption.Views.StatisticsViews;

using Plugins.ToolKits.MVVM;

using Xamarin.Forms;

namespace ForConsumption
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddConsumptionView), typeof(AddConsumptionView));
            Routing.RegisterRoute(nameof(ConsumptionDisplayView), typeof(ConsumptionDisplayView));


            Routing.RegisterRoute(nameof(PaymentModeView), typeof(PaymentModeView));
            Routing.RegisterRoute(nameof(EditPaymentModeView), typeof(EditPaymentModeView));
            Routing.RegisterRoute(nameof(SystemConfigView), typeof(SystemConfigView));

            Routing.RegisterRoute(nameof(ConsumptionModeView), typeof(ConsumptionModeView));
            Routing.RegisterRoute(nameof(EditConsumptionModeView), typeof(EditConsumptionModeView));

            Routing.RegisterRoute(nameof(StatisticsCategotyView), typeof(StatisticsCategotyView));

            Messenger.Default.Register(this, MessageKeys.LoginSuccessToken, () =>
              {
                  IList<ToolbarItem> a = ToolbarItems;
                  LoginView.IsVisible = false;
                  MainView.IsVisible = true;
                  MemberAssist.SaveLoginInfo();
              });

            Messenger.Default.Register(this, MessageKeys.GoBackToken, () =>
            {
                Shell.Current.GoToAsync("..", true);
            });

            //var result= MemberAssist.AutoLoginTask.GetAwaiter().GetResult();

        }



    }
}
