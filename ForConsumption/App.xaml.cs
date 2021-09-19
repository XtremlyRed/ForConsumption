
using ForConsumption.Assists;

using Plugins.ToolKits;
using Plugins.ToolKits.MVVM;

using Xamarin.Forms;

namespace ForConsumption
{
    public partial class App : Application
    {

 
        public const string IpAddress ="192.168.3.4";
 


        public App()
        {
            InitializeComponent();

            XF.Material.Forms.Material.Init(this);

            Injecter.Register<IMessageDialogService, MessageAssist>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
