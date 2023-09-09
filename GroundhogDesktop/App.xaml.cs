using Core;
using System.Windows;
using StorageFile.Implements;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Globalization;
using YandexDisk.Language;
using YandexDisk.Storage;
using Core.Logic;

namespace WindowsDesktop
{
    public partial class App : Application
    {
        public App()
        {
            NetworkStorageLogic networkStorageLogic = new NetworkStorageLogic();

            GroundhogContext.TaskInstanceLogic = new TaskInstanceLogic();
            GroundhogContext.TaskLogic = new TaskLogic();
            GroundhogContext.PurposeLogic = new PurposeLogic();
            GroundhogContext.PurposeGroupLogic = new PurposeGroupLogic();
            GroundhogContext.NoteLogic = new NoteLogic();

            GroundhogContext.NetworkStorageLogic = networkStorageLogic;
            GroundhogContext.NetworkLanguageLogic = new NetworkLanguageLogic();

            GroundhogContext.LocalBackupLogic = new LocalStorageBackupLogic();
            GroundhogContext.CloudBackupLogic = networkStorageLogic;

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
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(GroundhogContext.Language.Culture);

            App.Current.Resources["Lang.ControlCommands.Create"] = GroundhogContext.Language.ControlCommands.Create;
            App.Current.Resources["Lang.ControlCommands.Save"] = GroundhogContext.Language.ControlCommands.Save;
            App.Current.Resources["Lang.ControlCommands.Duplicate"] = GroundhogContext.Language.ControlCommands.Duplicate;
            App.Current.Resources["Lang.ControlCommands.Edit"] = GroundhogContext.Language.ControlCommands.Edit;
            App.Current.Resources["Lang.ControlCommands.Delete"] = GroundhogContext.Language.ControlCommands.Delete;
            App.Current.Resources["Lang.ControlCommands.DeleteAllInstances"] = GroundhogContext.Language.ControlCommands.DeleteAllInstances;
            App.Current.Resources["Lang.ControlCommands.Copy"] = GroundhogContext.Language.ControlCommands.Copy;
            App.Current.Resources["Lang.ControlCommands.Cut"] = GroundhogContext.Language.ControlCommands.Cut;
            App.Current.Resources["Lang.ControlCommands.Paste"] = GroundhogContext.Language.ControlCommands.Paste;

            App.Current.Resources["Lang.Notes.Notes"] = GroundhogContext.Language.Notes.Notes;
            App.Current.Resources["Lang.Notes.Note"] = GroundhogContext.Language.Notes.Note;
            App.Current.Resources["Lang.Notes.NoteName"] = GroundhogContext.Language.Notes.NoteName;
            App.Current.Resources["Lang.Notes.Find"] = GroundhogContext.Language.Notes.Find;

            App.Current.Resources["Lang.Purposes.Purposes"] = GroundhogContext.Language.Purposes.Purposes;
            App.Current.Resources["Lang.Purposes.Purpose"] = GroundhogContext.Language.Purposes.Purpose;
            App.Current.Resources["Lang.Purposes.PurposesGroup"] = GroundhogContext.Language.Purposes.PurposesGroup;
            App.Current.Resources["Lang.Purposes.GroupName"] = GroundhogContext.Language.Purposes.GroupName;

            App.Current.Resources["Lang.Syncronization.Syncronization"] = GroundhogContext.Language.Syncronization.Syncronization;
            App.Current.Resources["Lang.Syncronization.Download"] = GroundhogContext.Language.Syncronization.Download;
            App.Current.Resources["Lang.Syncronization.Upload"] = GroundhogContext.Language.Syncronization.Upload;
            App.Current.Resources["Lang.Syncronization.EnterCode"] = GroundhogContext.Language.Syncronization.EnterCode;
            App.Current.Resources["Lang.Syncronization.Send"] = GroundhogContext.Language.Syncronization.Send;

            App.Current.Resources["Lang.Settings.Settings"] = GroundhogContext.Language.Settings.Settings;
            App.Current.Resources["Lang.Settings.PlanningAndOptimizationRange"] = GroundhogContext.Language.Settings.PlanningAndOptimizationRange;
            App.Current.Resources["Lang.Settings.ConnectionString"] = GroundhogContext.Language.Settings.ConnectionString;
            App.Current.Resources["Lang.Settings.SelectLanguage"] = GroundhogContext.Language.Settings.SelectLanguage;
            App.Current.Resources["Lang.Settings.ColorSchema"] = GroundhogContext.Language.Settings.ColorSchema;
            App.Current.Resources["Lang.Settings.StandartSchema"] = GroundhogContext.Language.Settings.StandartSchema;
            App.Current.Resources["Lang.Settings.MainColor"] = GroundhogContext.Language.Settings.MainColor;
            App.Current.Resources["Lang.Settings.AditionalColor"] = GroundhogContext.Language.Settings.AditionalColor;
            App.Current.Resources["Lang.Settings.MainText"] = GroundhogContext.Language.Settings.MainText;
            App.Current.Resources["Lang.Settings.AditionalText"] = GroundhogContext.Language.Settings.AditionalText;
            App.Current.Resources["Lang.Settings.SelectedItem"] = GroundhogContext.Language.Settings.SelectedItem;
            App.Current.Resources["Lang.Settings.SelectedItemInactive"] = GroundhogContext.Language.Settings.SelectedItemInactive;
            App.Current.Resources["Lang.Settings.ChosenItem"] = GroundhogContext.Language.Settings.ChosenItem;

            App.Current.Resources["Lang.PlanningAndOptimization.DaysPlanning"] = GroundhogContext.Language.PlanningAndOptimization.DaysPlanning;
            App.Current.Resources["Lang.PlanningAndOptimization.DaysOfWeekPlanning"] = GroundhogContext.Language.PlanningAndOptimization.DaysOfWeekPlanning;
            App.Current.Resources["Lang.PlanningAndOptimization.WatchesPlanning"] = GroundhogContext.Language.PlanningAndOptimization.WatchesPlanning;
            App.Current.Resources["Lang.PlanningAndOptimization.DaysOfMonthPlanning"] = GroundhogContext.Language.PlanningAndOptimization.DaysOfMonthPlanning;
            App.Current.Resources["Lang.PlanningAndOptimization.DaysOfYearPlanning"] = GroundhogContext.Language.PlanningAndOptimization.DaysOfYearPlanning;
            App.Current.Resources["Lang.PlanningAndOptimization.Optimization"] = GroundhogContext.Language.PlanningAndOptimization.Optimization;

            App.Current.Resources["Lang.Tasks.List"] = GroundhogContext.Language.Tasks.List;
            App.Current.Resources["Lang.Tasks.Calendar"] = GroundhogContext.Language.Tasks.Calendar;
            App.Current.Resources["Lang.Tasks.Tasks"] = GroundhogContext.Language.Tasks.Tasks;
            App.Current.Resources["Lang.Tasks.Task"] = GroundhogContext.Language.Tasks.Task;
            App.Current.Resources["Lang.Tasks.RepeatMode"] = GroundhogContext.Language.Tasks.RepeatMode;
            App.Current.Resources["Lang.Tasks.TransferTaskToNextDay"] = GroundhogContext.Language.Tasks.TransferTaskToNextDay;
            App.Current.Resources["Lang.Tasks.OffsetNextTasks"] = GroundhogContext.Language.Tasks.OffsetNextTasks;
            App.Current.Resources["Lang.Tasks.PlanningRange"] = GroundhogContext.Language.Tasks.PlanningRange;
            App.Current.Resources["Lang.Tasks.OptimizationRange"] = GroundhogContext.Language.Tasks.OptimizationRange;

            App.Current.Resources["Lang.Backup.Backup"] = GroundhogContext.Language.Backup.Backup;
            App.Current.Resources["Lang.Backup.Cloud"] = GroundhogContext.Language.Backup.Cloud;
            App.Current.Resources["Lang.Backup.Local"] = GroundhogContext.Language.Backup.Local;
            App.Current.Resources["Lang.Backup.Restore"] = GroundhogContext.Language.Backup.Restore;
            App.Current.Resources["Lang.Backup.BackupName"] = GroundhogContext.Language.Backup.BackupName;
            App.Current.Resources["Lang.Backup.AutoCloudBackup"] = GroundhogContext.Language.Backup.AutoCloudBackup;
            App.Current.Resources["Lang.Backup.AutoLocalBackup"] = GroundhogContext.Language.Backup.AutoLocalBackup;
        }
    }
}
