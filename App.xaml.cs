﻿using System.Windows;

namespace NetworkingProgramming
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();

            ServerWindow serverWindow = new ServerWindow();
            serverWindow.Show();
        }
    }
}
