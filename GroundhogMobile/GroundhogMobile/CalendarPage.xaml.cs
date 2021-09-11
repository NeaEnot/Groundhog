using Xalendar.View.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        public CalendarPage()
        {
            InitializeComponent();
        }

        private void CalendarView_DaySelected(DaySelected obj)
        {
            Navigation.PushAsync(new TasksPage(obj.DateTime));
        }
    }
}