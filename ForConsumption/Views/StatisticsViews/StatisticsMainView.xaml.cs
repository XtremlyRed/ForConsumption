using ForConsumption.ViewModels;
using ForConsumption.ViewModels.StatisticsViewModels;

using Plugins.ToolKits;
using Plugins.ToolKits.MVVM;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForConsumption.Views.StatisticsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticsMainView : ContentPage
    {
        public StatisticsMainView()
        {
            InitializeComponent();
            BindingContext = StatisticsViewModel.Instance;

            Messenger.Default.Register(this, MessageKeys.StatisticsCategotyViewToken, () =>
            {
                Shell.Current.GoToAsync(nameof(StatisticsCategotyView), true);
            });

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            StatisticsViewModel.Instance.UpdateAsync().NoAwaiter();
        }
    }
}