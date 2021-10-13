﻿using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using LGSTrayCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LGSTrayGHUB;
using LGSTrayHID;

namespace LGSTrayGUI
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool? _autoStart = null;
        public bool AutoStart
        {
            get
            {
                if (_autoStart == null)
                {
                    RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    _autoStart = registryKey?.GetValue("LGSTrayGUI") != null;
                }

                return _autoStart ?? false;
            }
            set
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if (registryKey == null)
                {
                    return;
                }

                if (value)
                {
                    registryKey.SetValue("LGSTrayGUI", Assembly.GetEntryAssembly().Location);
                }
                else
                {
                    registryKey.DeleteValue("LGSTrayGUI", false);
                }

                _autoStart = value;
            }
        }

        private Thread httpThread;
        public Thread UpdateThread;
        private CancellationTokenSource _ctTimerSource;

        private ObservableCollection<ObservableCollection<LogiDevice>> _logiDevices = new ObservableCollection<ObservableCollection<LogiDevice>>();
        public ObservableCollection<ObservableCollection<LogiDevice>> LogiDevices { get { return this._logiDevices; } }

        private GHUBDeviceManager ghubDeviceManager;
        private HIDDeviceManager hidDeviceManager;

        public MainWindowViewModel()
        {
        }

        public async Task LoadViewModel()
        {
            //ObservableCollection<LogiDevice> ghubDevices = new ObservableCollection<LogiDevice>();
            //ghubDeviceManager = new GHUBDeviceManager(ghubDevices);
            //var t1 = ghubDeviceManager.LoadDevicesAsync();

            ObservableCollection<LogiDevice> hidDevices = new ObservableCollection<LogiDevice>();
            hidDeviceManager = new HIDDeviceManager(hidDevices);
            var t2 = hidDeviceManager.LoadDevicesAsync();

            //await Task.WhenAll(t1, t2);
            //LogiDevices.Add(ghubDevices);
            //LogiDevices.Add(hidDevices);

            HttpServer.LoadConfig();
            if (HttpServer.ServerEnabled)
            {
                httpThread = new Thread(() => HttpServer.ServeLoop(_logiDevices));
                httpThread.IsBackground = true;
                httpThread.Start();
            }
        }
    }
}