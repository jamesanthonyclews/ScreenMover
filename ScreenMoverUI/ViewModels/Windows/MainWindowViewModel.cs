// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using ScreenMover.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows;
using Wpf.Ui.Controls;

namespace ScreenMover.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "ScreenMover";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };

        [ObservableProperty]
        private NotifyIconWrapper.NotifyRequestRecord? _notifyRequest;

        [ObservableProperty]
        private bool _showInTaskbar;

        private WindowState _thisWindowState;
        public WindowState ThisWindowState
        {
            get => _thisWindowState;
            set
            {
                ShowInTaskbar = true;
                SetProperty(ref _thisWindowState, value);
                ShowInTaskbar = value != WindowState.Minimized;
            }
        }

        [RelayCommand]
        private void Loaded()
        {
            ThisWindowState = WindowState.Minimized;
        }

        [RelayCommand]
        private void StateChanged()
        {
            ;
        }


        [RelayCommand]
        private void Closing(CancelEventArgs? e)
        {
            if (e == null)
                return;
            e.Cancel = true;
        ThisWindowState = WindowState.Minimized;
        }

        [RelayCommand]
        private void Notify(string message)
        {
            NotifyRequest = new NotifyIconWrapper.NotifyRequestRecord
            {
                Title = "Notify",
                Text = message,
                Duration = 1000
            };


        }

        [RelayCommand]
        private void NotifyIconOpen()
        {
        ThisWindowState = WindowState.Normal;
        }

        [RelayCommand]
        private void NotifyIconExit()
        {
            System.Windows.Application.Current.Shutdown();
        }

    }
}
