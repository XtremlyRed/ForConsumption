using Android.Graphics.Drawables;
using ForConsumption.Controls;
using ForConsumption.Droid.Controls;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(NullLinePicker), typeof(NullLinePickerRenderer))]
namespace ForConsumption.Droid.Controls
{
    public class NullLinePickerRenderer : PickerRenderer
    {
        private readonly GradientDrawable gd = new GradientDrawable();
        public NullLinePickerRenderer() : base(Android.App.Application.Context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control is null)
            {
                return;
            }
            if (!(e.NewElement is NullLinePicker entry))
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