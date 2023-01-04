using System.Collections.Generic;

namespace Core.Models.Settings.Lang
{
    public class ErrorsMessagesLanguage
    {
        public string Error { get; set; }
        public string FieldMustBeFilled { get; set; }
        public string FieldsMustBeFilled { get; set; }
        public string CodeWasNotEntered { get; set; }
        public string StringNotMatchColorHexFormat { get; set; }
        public string ConnectionStringNotMatchFormat { get; set; }
        public string CodeWasNotReceived { get; set; }
        public string EntityWithSameIdDontExist { get; set; }
        public string CorrectValue { get; set; }
        public string CorrectFormat { get; set; }
        public string IncorrectValue { get; set; }
        public string IncorrectNumberOfDays { get; set; }
        public string IncorrectFormatOfDayOfMonth { get; set; }
        public string IncorrectNumberOfMonth { get; set; }
        public string IncorrectNumberOfDay { get; set; }
        public string ThereAreFewerDaysInSpecifiedMonth { get; set; }
        public string IncorrectDayOfTheWeek { get; set; }
        public string IncorrectFormat { get; set; }
        public string IncorrectNumberOfArguments { get; set; }
        public string Or { get; set; }

        internal static ErrorsMessagesLanguage Parse(Dictionary<string, string> dict)
        {
            ErrorsMessagesLanguage language = new ErrorsMessagesLanguage
            {
                Error = dict["Error"],
                FieldMustBeFilled = dict["FieldMustBeFilled"],
                FieldsMustBeFilled = dict["FieldsMustBeFilled"],
                CodeWasNotEntered = dict["CodeWasNotEntered"],
                StringNotMatchColorHexFormat = dict["StringNotMatchColorHexFormat"],
                ConnectionStringNotMatchFormat = dict["ConnectionStringNotMatchFormat"],
                CodeWasNotReceived = dict["CodeWasNotReceived"],
                EntityWithSameIdDontExist = dict["EntityWithSameIdDontExist"],
                CorrectValue = dict["CorrectValue"],
                CorrectFormat = dict["CorrectFormat"],
                IncorrectValue = dict["IncorrectValue"],
                IncorrectNumberOfDays = dict["IncorrectNumberOfDays"],
                IncorrectFormatOfDayOfMonth = dict["IncorrectFormatOfDayOfMonth"],
                IncorrectNumberOfMonth = dict["IncorrectNumberOfMonth"],
                IncorrectNumberOfDay = dict["IncorrectNumberOfDay"],
                ThereAreFewerDaysInSpecifiedMonth = dict["ThereAreFewerDaysInSpecifiedMonth"],
                IncorrectDayOfTheWeek = dict["IncorrectDayOfTheWeek"],
                IncorrectFormat = dict["IncorrectFormat"],
                IncorrectNumberOfArguments = dict["IncorrectNumberOfArguments"],
                Or = dict["Or"],
            };

            return language;
        }

        internal string Serialize()
        {
            string content = "";

            content += $"# Errors messages" + '\n';
            content += $"Error={Error}" + '\n';
            content += $"FieldMustBeFilled={FieldMustBeFilled}" + '\n';
            content += $"FieldsMustBeFilled={FieldsMustBeFilled}" + '\n';
            content += $"CodeWasNotEntered={CodeWasNotEntered}" + '\n';
            content += $"StringNotMatchColorHexFormat={StringNotMatchColorHexFormat}" + '\n';
            content += $"ConnectionStringNotMatchFormat={ConnectionStringNotMatchFormat}" + '\n';
            content += $"CodeWasNotReceived={CodeWasNotReceived}" + '\n';
            content += $"EntityWithSameIdDontExist={EntityWithSameIdDontExist}" + '\n';
            content += $"CorrectValue={CorrectValue}" + '\n';
            content += $"CorrectFormat={CorrectFormat}" + '\n';
            content += $"IncorrectValue={IncorrectValue}" + '\n';
            content += $"IncorrectNumberOfDays={IncorrectNumberOfDays}" + '\n';
            content += $"IncorrectFormatOfDayOfMonth={IncorrectFormatOfDayOfMonth}" + '\n';
            content += $"IncorrectNumberOfMonth={IncorrectNumberOfMonth}" + '\n';
            content += $"IncorrectNumberOfDay={IncorrectNumberOfDay}" + '\n';
            content += $"ThereAreFewerDaysInSpecifiedMonth={ThereAreFewerDaysInSpecifiedMonth}" + '\n';
            content += $"IncorrectDayOfTheWeek={IncorrectDayOfTheWeek}" + '\n';
            content += $"IncorrectFormat={IncorrectFormat}" + '\n';
            content += $"IncorrectNumberOfArguments={IncorrectNumberOfArguments}" + '\n';
            content += $"Or={Or}" + '\n';
            content += '\n';

            return content;
        }
    }
}
