using Core;
using System.Windows;
using StorageFile.Implements;
using YandexDisk;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace GroundhogWindows
{
    public partial class App : Application
    {
        public App()
        {
            GroundhogContext.TaskInstanceLogic = new TaskInstanceLogic();
            GroundhogContext.TaskLogic = new TaskLogic();
            GroundhogContext.PurposeLogic = new PurposeLogic();
            GroundhogContext.PurposeGroupLogic = new PurposeGroupLogic();
            GroundhogContext.NoteLogic = new NoteLogic();
            GroundhogContext.NetworkLogic = new NetworkLogic();

            Dictionary<string, string> colors = new Dictionary<string, string>()
            {
                { "Main color", "#FFFFFF" },
                { "Additional color", "#F0F0F0" },
                { "Main text", "#000000" },
                { "Additional text", "#818282" },
                { "Selected item", "#CBE8F6" },
                { "Selected item inactive", "#F6F6F6" },
                { "Select item", "#E5F3FB" }
            };

            bool isNeedSaveSettings = false;

            List<string> absentColors = GroundhogContext.Settings.ColorSchema.ColorSchemaAbsent(colors.Keys.ToList());

            if (absentColors.Count > 0)
            {
                foreach (string key in absentColors)
                    GroundhogContext.Settings.ColorSchema.Colors.Add(key, colors[key]);

                isNeedSaveSettings = true;
            }

            List<string> languages = GroundhogContext.Languages.ToList();
            if (languages.Contains(GroundhogContext.Settings.Language))
            {
                GroundhogContext.Language = GroundhogContext.LoadLanguage(GroundhogContext.Settings.Language);
            }
            else
            {
                GroundhogContext.Language = GroundhogContext.LoadLanguage(GroundhogContext.DefaultLanguage);
                isNeedSaveSettings = true;
            }

            if (isNeedSaveSettings)
                GroundhogContext.SaveSettings();
        }

        public static void ApplyColorSchema()
        {
            App.Current.Resources["Main color"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(GroundhogContext.Settings.ColorSchema.Colors["Main color"]));
            App.Current.Resources["Additional color"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(GroundhogContext.Settings.ColorSchema.Colors["Additional color"]));
            App.Current.Resources["Main text"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(GroundhogContext.Settings.ColorSchema.Colors["Main text"]));
            App.Current.Resources["Additional text"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(GroundhogContext.Settings.ColorSchema.Colors["Additional text"]));
            App.Current.Resources["Selected item"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(GroundhogContext.Settings.ColorSchema.Colors["Selected item"]));
            App.Current.Resources["Selected item inactive"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(GroundhogContext.Settings.ColorSchema.Colors["Selected item inactive"]));
            App.Current.Resources["Select item"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(GroundhogContext.Settings.ColorSchema.Colors["Select item"]));
        }

        public static void ApplyLanguage()
        {
            App.Current.Resources["Lang.ControlCommands.Create"] = GroundhogContext.Language.ControlCommands.Create;
            App.Current.Resources["Lang.ControlCommands.Save"] = GroundhogContext.Language.ControlCommands.Save;
            App.Current.Resources["Lang.ControlCommands.Duplicate"] = GroundhogContext.Language.ControlCommands.Duplicate;
            App.Current.Resources["Lang.ControlCommands.Edit"] = GroundhogContext.Language.ControlCommands.Edit;
            App.Current.Resources["Lang.ControlCommands.Delete"] = GroundhogContext.Language.ControlCommands.Delete;

            App.Current.Resources["Lang.Notes.Find"] = GroundhogContext.Language.Notes.Find;
            App.Current.Resources["Lang.Notes.Note"] = GroundhogContext.Language.Notes.Note;
            App.Current.Resources["Lang.Notes.NoteName"] = GroundhogContext.Language.Notes.NoteName;

            App.Current.Resources["Lang.Purposes.Purpose"] = GroundhogContext.Language.Purposes.Purpose;
            App.Current.Resources["Lang.Purposes.PurposesGroup"] = GroundhogContext.Language.Purposes.PurposesGroup;
            App.Current.Resources["Lang.Purposes.GroupName"] = GroundhogContext.Language.Purposes.GroupName;
        }
    }
}
