using Core.Interfaces;
using Core.Models;
using System.Windows;
using TelegramImplement.Implements;

namespace GroundhogWindows
{
    public partial class App : Application
    {
        public static IAccauntLogic AccauntLogic { get; private set; }
        public static ITaskInstanceLogic TaskInstanceLogic { get; private set; }
        public static ITaskLogic TaskLogic { get; private set; }

        public static Accaunt Accaunt { get; set; }

        public App()
        {
            AccauntLogic = new AccauntLogic();
            TaskInstanceLogic = new TaskInstanceLogic();
            TaskLogic = new TaskLogic();
        }
    }
}
