using ForConsumption.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForConsumption.Views.MyViews.PaymentModeViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentModeView : ContentPage
    {
        public PaymentModeView()
        {
            InitializeComponent();

            BindingContext = PaymentModeViewModel.Instance;
        }
    }
}