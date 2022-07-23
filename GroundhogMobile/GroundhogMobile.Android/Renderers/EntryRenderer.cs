using Android.Content;
using Android.Content.Res;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(GroundhogMobile.Droid.Renderers.EntryRenderer))]
namespace GroundhogMobile.Droid.Renderers
{
    public class EntryRenderer : Xamarin.Forms.Platform.Android.EntryRenderer
    {
        [Obsolete]
        public EntryRenderer(Context context)
        { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BackgroundTintList = ColorStateList.ValueOf(ColorConverter.ToAndroidColor((Color)App.Current.Resources["Additional text"]));
                Control.SetTextCursorDrawable(0);
            }
        }
    }
}