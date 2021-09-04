
using ForConsumption.ViewModels.StatisticsViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForConsumption.Views.StatisticsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticsCategotyView : ContentPage
    {
        public StatisticsCategotyView()
        {
            InitializeComponent();
            BindingContext = StatisticsCategotyViewModel.Instance;
        }
    }
}