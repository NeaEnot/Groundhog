﻿using Core;
using System.Windows;
using StorageFile.Implements;
using YandexDisk;

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
