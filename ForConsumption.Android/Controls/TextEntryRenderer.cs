
using Android.Graphics.Drawables;

using ForConsumption.Controls;
using ForConsumption.Droid.Controls;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TextEntry), typeof(TextEntryRenderer))]
namespace ForConsumption.Droid.Controls
{
    public class TextEntryRenderer : EntryRenderer
    {
        private readonly GradientDrawable gd = new GradientDrawable();
        public TextEntryRenderer() : base(Android.App.Application.Context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control is null)
            {
                return;
            }
            if (!(e.NewElement is TextEntry entry))
            {
                return;

            }
            gd.SetCornerRadius((float)entry.CornerRadius);
            gd.SetColor(entry.TextAreaColor.ToAndroid());

            gd.SetStroke((int)entry.StrokeThickness, entry.Stroke.ToAndroid());

            Control.Background = gd;

        }


    }


}