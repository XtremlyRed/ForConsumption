using ForConsumption.ViewModels;
using ForConsumption.Views.ConsumptionViews;

using Plugins.ToolKits.MVVM;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForConsumption.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConsumptionListView : ContentPage
    {
        public ConsumptionListView()
        {
            InitializeComponent();
            BindingContext = ConsumptionListViewModel.Instance;


            Messenger.Default.Register<bool>(this, MessageKeys.NavigatToAddConsumptionViewToken, (isUpdateMode) =>
              {
                  string uri = $"{nameof(AddConsumptionView)}?{nameof(AddConsumptionView.IsUpdateMode)}={isUpdateMode}";
                  Shell.Current.GoToAsync(uri, true);
              });



            Messenger.Default.Register<bool>(this, MessageKeys.NavigatToConsumptionDetaisViewToken, (isUpdateMode) =>
            {
                string uri = $"{nameof(ConsumptionDisplayView)}";
                Shell.Current.GoToAsync(uri, true);
            });


        }
    }
}