using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenMover.Models
{
    //TODO : Link this up with the .NET host configuration rather than a seperate file, maybe??
    public partial class ScreenConfiguration : ObservableObject
    {
        [ObservableProperty]
        private string _appName = "Application Name - From Titlebar";
        [ObservableProperty]
        private bool _moveApp = false;
        [ObservableProperty]
        private int _xPos  = 0;
        [ObservableProperty]
        private int _yPos = 0;
        [ObservableProperty]
        private bool _hideTitleBar = false;
        [ObservableProperty]
        private bool _resizeApp = false;
        [ObservableProperty]
        private int _xSize = 0;
        [ObservableProperty]
        private int _ySize = 0;


    }

    public partial class ScreenMoverApplicationConfiguration : ObservableObject
    {
        [ObservableProperty]
        private bool _autoStart = true;

        [ObservableProperty]
        private ObservableCollection<ScreenConfiguration> _screens = new();
    }
}
