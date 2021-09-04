
using System;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

using AndroidX.Core.Graphics;

using ForConsumption.Common.Common;
using ForConsumption.Common.Services;
using ForConsumption.ViewModels;

using Plugins.ToolKits;
using Plugins.ToolKits.EasyHttp;
using Plugins.ToolKits.MVVM;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using XF.Material.Droid;

using Messenger = Plugins.ToolKits.MVVM.Messenger;

namespace ForConsumption.Droid
{
    [Activity(Label = "小记一下", Icon = "@drawable/ok2", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Initialize();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            XF.Material.Droid.Material.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void AttachBaseContext(Context @base)
        {
            Android.Content.Res.Configuration config = @base.Resources.Configuration;
            config.FontScale = 1f;
            Context context = @base.CreateConfigurationContext(config);
            base.AttachBaseContext(context);
        }

        public override void OnBackPressed()
        {
            Material.HandleBackButton(base.OnBackPressed);
        }


        public void ChangeStatusBarColor(Xamarin.Forms.Color color)
        {
            Android.Graphics.Color newColor = ColorExtensions.ToAndroid(color);

            bool isColorDark = ColorUtils.CalculateLuminance(newColor) < 0.5;

            SetStatusBarColor(ColorExtensions.ToAndroid(color));

            //if (!isColorDark)
            //{
            //    this.Window.DecorView.SystemUiVisibility = 8192;
            //    return;
            //}
            //this.Window.DecorView.SystemUiVisibility = 0;
        }

        private void Initialize()
        {
            SetStatusBarColor(global::Android.Graphics.Color.White);

            Injecter.Register<IVersionManager, VersionManager>();
            Messenger.Default.Register<Action>(this, MessageKeys.RunOnUIThread, action => Device.BeginInvokeOnMainThread(action), RegistrationMode.Once);

            Injecter.RegisterInstance(RestConfig.Default()
              .UseBaseUrl($"http://{App.IpAddress}:5001")
              .UseDeserializer((@string, type) => JsonMapper.Deserialize(@string, type))
              .UseSerializer(o => JsonMapper.Serialize(o))
              .UseTimeout(100000)
              .UseEncoding(Encoding.UTF8)
              .Build()).As<IRestClient>();


            //  MemberAssist.AutoLogin();
        }
    }
}