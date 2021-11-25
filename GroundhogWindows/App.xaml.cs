using Core;
using System.Windows;
using NetworkTelegram.Implements;

namespace GroundhogWindows
{
    public partial class App : Application
    {
        public App()
        {
            GroundhogContext.AccauntLogic = new AccauntLogic();
            GroundhogContext.TaskInstanceLogic = new TaskInstanceLogic();
            GroundhogContext.TaskLogic = new TaskLogic();
            GroundhogContext.NetworkLogic = new NetworkLogic();
        }
    }
}
