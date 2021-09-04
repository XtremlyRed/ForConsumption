
using ForConsumption.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForConsumption.Views.ConsumptionViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConsumptionDisplayView : ContentPage
    {
        public ConsumptionDisplayView()
        {
            InitializeComponent();
            BindingContext = EditConsumptionViewModel.DisplayInstance;
        }
    }
}