using Core;
using System.Windows;
using NetworkVk;
using StorageFile.Implements;

namespace GroundhogWindows
{
    public partial class App : Application
    {
        public App()
        {
            GroundhogContext.TaskInstanceLogic = new TaskInstanceLogic();
            GroundhogContext.TaskLogic = new TaskLogic();
            GroundhogContext.NetworkLogic = new NetworkLogic();
        }
    }
}
