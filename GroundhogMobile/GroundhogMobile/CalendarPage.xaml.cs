using GroundhogMobile.Formatters;
using System;
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

            CreateCalendar();
        }

        private void CalendarView_DaySelected(DaySelected obj)
        {
            Navigation.PushAsync(new TasksPage(obj.DateTime));

            CreateCalendar();
        }

        private void CreateCalendar()
        {
            CalendarView calendar = new CalendarView();
            calendar.Theme = Resources;
            calendar.DaysOfWeekFormatter = new DayOfWeek2CaractersFormatter();
            calendar.DaySelected += CalendarView_DaySelected;
            calendar.FirstDayOfWeek = DayOfWeek.Monday;

            Content = calendar;
        }
    }
}