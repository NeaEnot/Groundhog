using Android.Content;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Slider), typeof(GroundhogMobile.Droid.Renderers.SliderRenderer))]
namespace GroundhogMobile.Droid.Renderers
{
    class SliderRenderer : Xamarin.Forms.Platform.Android.SliderRenderer
    {
        [Obsolete]
        public SliderRenderer(Context context)
        { }

        [Obsolete]
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.ProgressDrawable.SetColorFilter(ColorConverter.ToAndroidColor((Color)App.Current.Resources["Additional text"]), Android.Graphics.PorterDuff.Mode.SrcIn);
                Control.Thumb.SetColorFilter(ColorConverter.ToAndroidColor((Color)App.Current.Resources["Selected item"]), Android.Graphics.PorterDuff.Mode.SrcIn);
            }
        }
    }
}