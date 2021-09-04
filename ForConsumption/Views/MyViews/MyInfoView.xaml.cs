using ForConsumption.ViewModels;
using ForConsumption.Views.MyViews;
using ForConsumption.Views.MyViews.ConsumptionModeViews;
using ForConsumption.Views.MyViews.PaymentModeViews;
using ForConsumption.Views.MyViews.SystemConfigViews;

using Plugins.ToolKits.MVVM;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForConsumption.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyInfoView : ContentPage
    {

        public MyInfoView()
        {
            InitializeComponent();

            BindingContext = MyViewModel.Instance;

            Messenger.Default.Register(this);
        }


        [RecipientMethodToken(MessageKeys.OpenConsumptionModePageToken)]
        public void OpenConsumptionModePage()
        {
            Shell.Current.GoToAsync(nameof(ConsumptionModeView), true);
        }

        [RecipientMethodToken(MessageKeys.EditConsumptionModePageToken)]
        public void OpenEditConsumptionModePage(bool isUpdate)
        {
            string uri = $"{nameof(EditConsumptionModeView)}?{nameof(EditConsumptionModeView.IsUpdateMode)}={isUpdate}";
            Shell.Current.GoToAsync(uri, true);
        }




        [RecipientMethodToken(MessageKeys.OpenPaymentModePageToken)]
        public void OpenPaymentModePage()
        {
            Shell.Current.GoToAsync(nameof(PaymentModeView), true);
        }


        [RecipientMethodToken(MessageKeys.EditPaymentModePageToken)]
        public void OpenEditPaymentModePage(bool isUpdate)
        {
            string uri = $"{nameof(EditPaymentModeView)}?{nameof(EditPaymentModeView.IsUpdateMode)}={isUpdate}";
            Shell.Current.GoToAsync(uri, true);
        }


        [RecipientMethodToken(MessageKeys.OpenSystemConfigPageToken)]
        public void OpenSystemConfigPage()
        {
            Shell.Current.GoToAsync(nameof(SystemConfigView), true);
        }
    }
}