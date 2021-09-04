using ForConsumption.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForConsumption.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(IsUpdateMode), "IsUpdateMode")]
    public partial class AddConsumptionView : ContentPage
    {
        public AddConsumptionView()
        {
            InitializeComponent();
            BindingContext = EditConsumptionViewModel.AddInstance;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            AddressBox.Focused += (s, e) =>
            {
                AddressBox.CursorPosition = 0;
                AddressBox.SelectionLength = AddressBox.Text?.Length ?? 0;
            };
            DescriptionBox.Focused += (s, e) =>
            {
                DescriptionBox.SelectionLength = DescriptionBox.Text?.Length ?? 0;
            };
            MomeyBox.Focused += (s, e) =>
            {
                MomeyBox.SelectionLength = MomeyBox.Text?.Length ?? 0;
            };
        }

        public bool IsUpdateMode
        {
            set => BindingContext = value ? EditConsumptionViewModel.UpdateInstance : EditConsumptionViewModel.AddInstance;
        }

    }

    public class MyDatePicker : DatePicker
    {

    }
}