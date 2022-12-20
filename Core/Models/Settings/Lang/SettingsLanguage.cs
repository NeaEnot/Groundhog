using System.Collections.Generic;

namespace Core.Models.Settings.Lang
{
    public class SettingsLanguage
    {
        public string Settings { get; set; }
        public string ConnectionString { get; set; }
        public string PlanningAndOptimizationRange { get; set; }
        // Color schema
        public string ColorSchema { get; set; }
        public string StandartSchema { get; set; }
        public string MainColor { get; set; }
        public string AditionalColor { get; set; }
        public string MainText { get; set; }
        public string AditionalText { get; set; }
        public string SelectedItem { get; set; }
        public string SelectedItemInactive { get; set; }
        public string ChosenItem { get; set; }

        internal static SettingsLanguage Parse(Dictionary<string, string> dict)
        {
            SettingsLanguage language = new SettingsLanguage
            {
                Settings = dict["Settings"],
                ConnectionString = dict["ConnectionString"],
                PlanningAndOptimizationRange = dict["PlanningAndOptimizationRange"],
                ColorSchema = dict["ColorSchema"],
                MainColor = dict["MainColor"],
                AditionalColor = dict["AditionalColor"],
                MainText = dict["MainText"],
                AditionalText = dict["AditionalText"],
                SelectedItem = dict["SelectedItem"],
                SelectedItemInactive = dict["SelectedItemInactive"],
                ChosenItem = dict["ChosenItem"],
                StandartSchema = dict["StandartSchema"],
            };

            return language;
        }

        internal string Serrialize()
        {
            string content = "";

            content += $"# Settings" + '\n';
            content += $"Settings={Settings}" + '\n';
            content += $"ConnectionString={ConnectionString}" + '\n';
            content += $"PlanningAndOptimizationRange={PlanningAndOptimizationRange}" + '\n';
            content += '\n';

            content += $"# Settings - Color schema" + '\n';
            content += $"ColorSchema={ColorSchema}" + '\n';
            content += $"StandartSchema={StandartSchema}" + '\n';
            content += $"MainColor={MainColor}" + '\n';
            content += $"AditionalColor={AditionalColor}" + '\n';
            content += $"MainText={MainText}" + '\n';
            content += $"AditionalText={AditionalText}" + '\n';
            content += $"SelectedItem={SelectedItem}" + '\n';
            content += $"SelectedItemInactive={SelectedItemInactive}" + '\n';
            content += $"ChosenItem={ChosenItem}" + '\n';
            content += '\n';

            return content;
        }
    }
}
