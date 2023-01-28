using Core;
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
                { DayOfWeek.Sunday, GroundhogContext.Language.DaysOfWeek.SundayAbbreviated },
                { DayOfWeek.Monday, GroundhogContext.Language.DaysOfWeek.MondayAbbreviated },
                { DayOfWeek.Tuesday, GroundhogContext.Language.DaysOfWeek.TuesdayAbbreviated },
                { DayOfWeek.Wednesday, GroundhogContext.Language.DaysOfWeek.WednesdayAbbreviated },
                { DayOfWeek.Thursday, GroundhogContext.Language.DaysOfWeek.ThursdayAbbreviated },
                { DayOfWeek.Friday, GroundhogContext.Language.DaysOfWeek.FridayAbbreviated },
                { DayOfWeek.Saturday, GroundhogContext.Language.DaysOfWeek.SaturdayAbbreviated }
            };

        public string Format(DayOfWeek dayOfWeek)
        {
            return days[dayOfWeek];
        }
    }
}
