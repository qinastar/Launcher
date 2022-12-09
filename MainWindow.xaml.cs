﻿using Launcher.Model;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Launcher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }


        public MainWindow()
        {
            InitializeComponent();
            Instance = this;


            if (App.launcherConfig == null)
            {

                App.launcherConfig = LauncherConfig.Load("config.json");

                if (!string.IsNullOrEmpty(App.launcherConfig.Language))
                {
                    CultureInfo cultureOverride = new CultureInfo(App.launcherConfig.Language);
                    Thread.CurrentThread.CurrentCulture = cultureOverride;
                }
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lb = sender as ListBox;

            if (rootFrame == null)
            {
                return;
            }
            switch (lb.SelectedIndex)
            {
                case 0: rootFrame.Navigate(new Uri("View/Home.xaml", UriKind.Relative)); break;
                case 1: rootFrame.Navigate(new Uri("View/Setting.xaml", UriKind.Relative)); break;
                case 2: rootFrame.Navigate(new Uri("View/About.xaml", UriKind.Relative)); break;
                default:
                    break;
            }
        }
    }
}
