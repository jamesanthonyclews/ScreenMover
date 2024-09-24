using Microsoft.Win32;
using ScreenMover.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WindowScrape.Static;

namespace ScreenMover.Services
{
    public partial class ScreenMoverService : ObservableObject
    { 
        [DllImport("USER32.DLL")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("USER32.DLL")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        const int WS_BORDER = 8388608;
        const int WS_DLGFRAME = 4194304;
        const int WS_CAPTION = WS_BORDER | WS_DLGFRAME;
        const int WS_SYSMENU = 524288;
        const int WS_THICKFRAME = 262144;
        const int WS_MINIMIZE = 536870912;
        const int WS_MAXIMIZEBOX = 65536;
        const int GWL_STYLE = -16;
    
        [ObservableProperty]
        private ScreenMoverApplicationConfiguration _configuration = new();

        [ObservableProperty]
        private bool _Started = false;
        partial void OnStartedChanged(bool value)
        {
            Stopped = !value;
        }

        [ObservableProperty]
        private bool _Stopped = false;
        

        private string _configFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments)}\\ScreenMover\\config.json";

        public ScreenMoverService()
        {
            if(File.Exists(_configFilePath))
            {
                string jsonText = File.ReadAllText(_configFilePath);
                Configuration = JsonSerializer.Deserialize<ScreenMoverApplicationConfiguration>(jsonText);
            }
            else
            {
                Configuration = new();
                Configuration.Screens.Add(new ScreenConfiguration { AppName = "Application Name", MoveApp = false, ResizeApp = false, XPos = 0, YPos = 0, XSize = 0, YSize = 0, HideTitleBar = false });
                SaveConfiguration();

            }

            if(Configuration.AutoStart)
            {
                Start();
            }

            WindowHookNet.Instance.WindowCreated += OnWindowCreated;

        }

        public void LoadConfiguration(ScreenMoverApplicationConfiguration config)
        {
            Configuration = config;
            Initialise();
        }

        public void SaveConfiguration()
        {
            Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments)}\\ScreenMover");
            File.WriteAllText(_configFilePath, JsonSerializer.Serialize(Configuration));
        }

        public void SaveConfiguration(ScreenMoverApplicationConfiguration config)
        {
            Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments)}\\ScreenMover");
            File.WriteAllText(_configFilePath, JsonSerializer.Serialize(config));
        }

        public void Initialise()
        {
            if(Configuration.AutoStart && ! Started)
            {
                Start();
            }
        }

        public void Start()
        {
            
            Started = true;
            Refresh();
        }

        public void Stop()
        {
            //WindowHookNet.Instance.WindowCreated -= OnWindowCreated;
            //WindowHookNet.Instance.Shutdown();
            Started = false;
        }

        void OnWindowCreated(object sender, WindowHookEventArgs e)
        {
            if (Started)
            {
                if (Configuration.Screens.Any(x => x.AppName == e.WindowTitle))
                {
                    UpdateScreen(e.WindowTitle);
                }
            }
        }

        public void Refresh()
        {
            if(Started)
            {
                foreach (ScreenConfiguration screen in Configuration.Screens)
                {
                    UpdateScreen(screen.AppName);
                }
            }

        }

        private void UpdateScreen(string title)
        {
            if (Started)
            {
                List<IntPtr> windows = HwndInterface.EnumHwnds();

                foreach (ScreenConfiguration screen in Configuration.Screens.Where(x => x.AppName == title))
                {
                    foreach (IntPtr window in windows)
                    {
                        if (HwndInterface.GetHwndTitle(window) == title)
                        {
                            if (screen.MoveApp)
                            {
                                HwndInterface.SetHwndPos(window, screen.XPos, screen.YPos);
                            }

                            if (screen.ResizeApp)
                            {
                                HwndInterface.SetHwndSize(window, screen.XSize, screen.YSize);
                            }
                        }
                    }
                }
            }
        }
    }
}
