using ForConsumption.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForConsumption.Views.MyViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConsumptionModeView : ContentPage
    {
        public ConsumptionModeView()
        {
            InitializeComponent();
            BindingContext = ConsumptionModeViewModel.Instance;
        }
    }
}