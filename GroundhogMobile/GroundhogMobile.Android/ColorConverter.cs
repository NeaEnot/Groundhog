namespace GroundhogMobile.Droid
{
    internal static class ColorConverter
    {
        public static Android.Graphics.Color ToAndroidColor(Xamarin.Forms.Color from)
        {
            Android.Graphics.Color color = 
                new Android.Graphics.Color((byte)(from.R * byte.MaxValue),
                                           (byte)(from.G * byte.MaxValue),
                                           (byte)(from.B * byte.MaxValue));

            return color;
        }
    }
}