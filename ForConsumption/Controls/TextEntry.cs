
using Xamarin.Forms;

namespace ForConsumption.Controls
{
    public class TextEntry : Entry
    {

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(TextEntry), 5d);

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValueCore(CornerRadiusProperty, value, Xamarin.Forms.Internals.SetValueFlags.RaiseOnEqual);
        }


        public static readonly BindableProperty StrokeProperty =
           BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(TextEntry), Color.Transparent);

        public Color Stroke
        {
            get => (Color)GetValue(StrokeProperty);
            set => SetValueCore(StrokeProperty, value, Xamarin.Forms.Internals.SetValueFlags.RaiseOnEqual);
        }

        public static readonly BindableProperty StrokeThicknessProperty =
           BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(TextEntry), 0d);

        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValueCore(StrokeThicknessProperty, value, Xamarin.Forms.Internals.SetValueFlags.RaiseOnEqual);
        }


        public static readonly BindableProperty TextAreaColorProperty =
          BindableProperty.Create(nameof(TextAreaColor), typeof(Color), typeof(TextEntry), Color.FromHex("#60d9d9f3"));

        public Color TextAreaColor
        {
            get => (Color)GetValue(TextAreaColorProperty);
            set => SetValueCore(TextAreaColorProperty, value, Xamarin.Forms.Internals.SetValueFlags.RaiseOnEqual);
        }
    }




}
