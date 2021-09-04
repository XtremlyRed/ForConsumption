using ForConsumption.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForConsumption.Views.DetailsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListDetailsView : ContentPage
    {
        public ListDetailsView()
        {
            InitializeComponent();
            BindingContext = DetailsViewModel.Instance;
        }
    }
}