using System;
using System.Collections.Generic;
using Xalendar.Api.Interfaces;

namespace GroundhogMobile.Formatters
{
    public class DayOfWeek2CaractersFormatter : IDayOfWeekFormatter
    {
        private static Dictionary<DayOfWeek, string> days =
            new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Sunday, "Вс" },
                { DayOfWeek.Monday, "Пн" },
                { DayOfWeek.Tuesday, "Вт" },
                { DayOfWeek.Wednesday, "Ср" },
                { DayOfWeek.Thursday, "Чт" },
                { DayOfWeek.Friday, "Пт" },
                { DayOfWeek.Saturday, "Сб" }
            };

        public string Format(DayOfWeek dayOfWeek)
        {
            return days[dayOfWeek];
        }
    }
}
