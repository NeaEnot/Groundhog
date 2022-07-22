using Android.Content;
using Android.Content.Res;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Editor), typeof(GroundhogMobile.Droid.Renderers.EditorRenderer))]
namespace GroundhogMobile.Droid.Renderers
{
    class EditorRenderer : Xamarin.Forms.Platform.Android.EditorRenderer
    {
        [Obsolete]
        public EditorRenderer(Context context)
        { }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BackgroundTintList = ColorStateList.ValueOf(ColorConverter.ToAndroidColor((Color)App.Current.Resources["Additional text"]));
            }
        }
    }
}