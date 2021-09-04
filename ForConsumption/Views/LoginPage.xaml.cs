
using ForConsumption.Assists;
using ForConsumption.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using XF.Material.Forms;

namespace ForConsumption.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            Material.PlatformConfiguration.ChangeStatusBarColor(Color.White);
             
            BindingContext = MemberViewModel.Instance;

            MemberAssist.ReadLoginInfo();


        }




    }
}