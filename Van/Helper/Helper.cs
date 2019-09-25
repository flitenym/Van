﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Van.ViewModel;

namespace Van.Helper
{
    public static class Helper
    {
        public static void LoadingAble()
        {
            MainWindowViewModel win = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            win.IsLoadingPanelVisible = true;
        }

        public static void LoadingDisable()
        {
            MainWindowViewModel win = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            win.IsLoadingPanelVisible = false;
        }
        public static void Message(string content)
        {
            MainWindowViewModel win = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            win.IsMessagePanelContent = content;
        } 
    }
}