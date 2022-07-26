using Android.Content;
using Android.Content.Res;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Editor), typeof(GroundhogMobile.Droid.Renderers.EditorRenderer))]
namespace GroundhogMobile.Droid.Renderers
{
    public class EditorRenderer : Xamarin.Forms.Platform.Android.EditorRenderer
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
                Control.SetHighlightColor(ColorConverter.ToAndroidColor((Color)App.Current.Resources["Selected item"]));
                Control.TextSelectHandle.SetTintList(ColorStateList.ValueOf(ColorConverter.ToAndroidColor((Color)App.Current.Resources["Selected item"])));
                Control.TextSelectHandleLeft.SetTintList(ColorStateList.ValueOf(ColorConverter.ToAndroidColor((Color)App.Current.Resources["Selected item"])));
                Control.TextSelectHandleRight.SetTintList(ColorStateList.ValueOf(ColorConverter.ToAndroidColor((Color)App.Current.Resources["Selected item"])));
                Control.SetTextCursorDrawable(0);
            }
        }
    }
}