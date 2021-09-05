using Android.Graphics.Drawables;
using ForConsumption.Controls;
using ForConsumption.Droid.Controls;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NullLineDatePicker), typeof(NullLineDatePickerRenderer))]

namespace ForConsumption.Droid.Controls
{
    public class NullLineDatePickerRenderer : DatePickerRenderer
    {
        private readonly GradientDrawable gd = new GradientDrawable();
        public NullLineDatePickerRenderer() : base(Android.App.Application.Context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control is null)
            {
                return;
            }
            if (!(e.NewElement is NullLineDatePicker entry))
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