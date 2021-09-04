using ForConsumption.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForConsumption.Views.MyViews.ConsumptionModeViews
{
    [QueryProperty(nameof(IsUpdateMode), nameof(IsUpdateMode))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditConsumptionModeView : ContentPage
    {
        public EditConsumptionModeView()
        {
            InitializeComponent();
        }



        public bool IsUpdateMode
        {
            set => BindingContext = value ? EditConsumptionModeViewModel.UpdateInstance : EditConsumptionModeViewModel.AddInstance;
        }
    }
}