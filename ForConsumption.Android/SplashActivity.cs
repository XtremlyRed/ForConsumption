using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;

using AndroidX.AppCompat.App;

namespace ForConsumption.Droid
{
    [Activity(Label = "小记一下", Icon = "@drawable/ok2", Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState, persistentState);
        }

        protected override void OnResume()
        {
            base.OnResume();

            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(1200);
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }, TaskCreationOptions.LongRunning);
        }
    }
}