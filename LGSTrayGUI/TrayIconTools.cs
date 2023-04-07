﻿using LGSTrayCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGSTrayGUI
{
    public static class TrayIconTools
    {
        private static Bitmap Mouse => CheckTheme.LightTheme ? Properties.Resources.Mouse : Properties.Resources.Mouse_dark;
        private static Bitmap Keyboard => CheckTheme.LightTheme ? Properties.Resources.Keyboard : Properties.Resources.Keyboard_dark;
        private static Bitmap Headset => CheckTheme.LightTheme ? Properties.Resources.Headset : Properties.Resources.Headset_dark;
        private static Bitmap Battery => CheckTheme.LightTheme ? Properties.Resources.Battery : Properties.Resources.Battery_dark;
        private static Bitmap Missing => CheckTheme.LightTheme ? Properties.Resources.Missing : Properties.Resources.Missing_dark;
        private static Bitmap Charging => Properties.Resources.Charging;

        private static Bitmap MixBitmap(Bitmap device, Bitmap battery, Bitmap indicator)
        {
            Bitmap bitmap = new Bitmap(device.Width, device.Height);
            Graphics canvas = Graphics.FromImage(bitmap);
            canvas.DrawImage(device, new Point(0, 0));
            canvas.DrawImage(battery, new Point(0, 0));
            canvas.DrawImage(indicator, new Point(0, 0));
            canvas.Save();

            return bitmap;
        }

        private static Bitmap MixBitmap(Bitmap device, Bitmap battery, Bitmap indicator, Bitmap charging)
        {
            Bitmap bitmap = new Bitmap(device.Width, device.Height);
            Graphics canvas = Graphics.FromImage(bitmap);
            canvas.DrawImage(device, new Point(0, 0));
            canvas.DrawImage(battery, new Point(0, 0));
            canvas.DrawImage(indicator, new Point(0, 0));
            canvas.DrawImage(charging, new Point(0, 0));
            canvas.Save();

            return bitmap;
        }

        private static Bitmap ErrorBitMap()
        {
            return MixBitmap(Mouse, Battery, Missing);
        }

        public static Icon ErrorIcon()
        {
            return Icon.FromHandle(ErrorBitMap().GetHicon());
        }
        public static Icon GenerateIcon(LogiDevice logiDevice)
        {
            Bitmap output;
            if (logiDevice == null)
            {
                output = ErrorBitMap();
            }
            else
            {
                Bitmap device;
                Bitmap indicator;

                switch (logiDevice.DeviceType)
                {
                    case DeviceType.Mouse:
                        device = Mouse;
                        break;
                    case DeviceType.Keyboard:
                        device = Keyboard;
                        break;
                    case DeviceType.Headset:
                        device = Headset;
                        break;
                    default:
                        device = Mouse;
                        break;
                }

                if (logiDevice.BatteryPercentage > 70)
                {
                    indicator = Properties.Resources.Indicator_100;
                }
                else if (logiDevice.BatteryPercentage > 40)
                {
                    indicator = Properties.Resources.Indicator_50;
                }
                else if (logiDevice.BatteryPercentage > 20)
                {
                    indicator = Properties.Resources.Indicator_30;
                }
                else if (logiDevice.BatteryPercentage > 0)
                {
                    indicator = Properties.Resources.Indicator_10;
                }
                else
                {
                    indicator = Missing;
                }

                output = MixBitmap(device, Battery, indicator);
            }

            return Icon.FromHandle(output.GetHicon());
        }

        public static Icon GenerateIcon(LGSTrayGHUB.LogiDeviceGHUB logiDevice)
        {
            Bitmap output;
            if (logiDevice == null)
            {
                output = ErrorBitMap();
            }
            else
            {
                Bitmap device;
                Bitmap indicator;

                switch (logiDevice.DeviceType)
                {
                    case DeviceType.Mouse:
                        device = Mouse;
                        break;
                    case DeviceType.Keyboard:
                        device = Keyboard;
                        break;
                    case DeviceType.Headset:
                        device = Headset;
                        break;
                    default:
                        device = Mouse;
                        break;
                }

                if (logiDevice.BatteryPercentage > 70)
                {
                    indicator = Properties.Resources.Indicator_100;
                }
                else if (logiDevice.BatteryPercentage > 40)
                {
                    indicator = Properties.Resources.Indicator_50;
                }
                else if (logiDevice.BatteryPercentage > 20)
                {
                    indicator = Properties.Resources.Indicator_30;
                }
                else if (logiDevice.BatteryPercentage > 0)
                {
                    indicator = Properties.Resources.Indicator_10;
                }
                else
                {
                    indicator = Missing;
                }

                if (logiDevice.Charging && logiDevice.BatteryPercentage > 0)
                {
                    output = MixBitmap(device, Battery, indicator, Charging);
                }
                else
                {
                    output = MixBitmap(device, Battery, indicator);
                }
            }

            return Icon.FromHandle(output.GetHicon());
        }
    }
}

