using Android.Content;
using Android.Content.Res;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Picker), typeof(GroundhogMobile.Droid.Renderers.PickerRenderer))]
namespace GroundhogMobile.Droid.Renderers
{
    public class PickerRenderer : Xamarin.Forms.Platform.Android.AppCompat.PickerRenderer
    {
        [Obsolete]
        public PickerRenderer(Context context)
        { }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BackgroundTintList = ColorStateList.ValueOf(ColorConverter.ToAndroidColor((Color)App.Current.Resources["Additional text"]));
            }
        }
    }
}