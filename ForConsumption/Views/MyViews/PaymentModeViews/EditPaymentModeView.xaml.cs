using ForConsumption.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForConsumption.Views.MyViews.PaymentModeViews
{
    [QueryProperty(nameof(IsUpdateMode), nameof(IsUpdateMode))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPaymentModeView : ContentPage
    {
        public EditPaymentModeView()
        {
            InitializeComponent();
        }
        public bool IsUpdateMode
        {
            set => BindingContext = value ? EditPaymentModeViewModel.UpdateInstance : EditPaymentModeViewModel.AddInstance;
        }
    }
}