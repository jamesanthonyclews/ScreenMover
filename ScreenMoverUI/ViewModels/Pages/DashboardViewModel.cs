// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using ScreenMover.Models;
using ScreenMover.Services;
using System.Runtime.CompilerServices;

namespace ScreenMover.ViewModels.Pages
{
    public partial class DashboardViewModel : ObservableObject
    {
        [ObservableProperty]
        private ScreenMoverService _screenMoverService;

        [ObservableProperty]
        private ScreenConfiguration _selectedScreenConfiguration;

        public DashboardViewModel(ScreenMoverService screenMoverService)
        {
            _screenMoverService = screenMoverService;
        }

        [RelayCommand]
        private void Refresh()
        {
            if(ScreenMoverService.Started)
            {
                ScreenMoverService.Refresh();
            }
            
        }

        [RelayCommand]
        private void Stop()
        {
            if(ScreenMoverService.Started)
            {
                ScreenMoverService.Stop();
            }
            
        }

        [RelayCommand]
        private void Start()
        {
            if(!ScreenMoverService.Started)
            {
                ScreenMoverService.Start();
            }
            
        }

        [RelayCommand]
        private void Delete()
        {
            if(SelectedScreenConfiguration is not null)
            {
                ScreenMoverService.Configuration.Screens.Remove(SelectedScreenConfiguration);
            }
        }

        [RelayCommand]
        private void Add()
        {
            ScreenMoverService.Configuration.Screens.Add(new ScreenConfiguration());
        }

        [RelayCommand]
        private void Save()
        {
            ScreenMoverService.SaveConfiguration();
        }

    }
}
