using Microsoft.Win32;
using svchost.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace svchost
{
    public static class WindowsSettings
    {
        /// <summary>
        /// Get from the registry installed size of the icons
        /// </summary>
        /// <returns></returns>
        public static Size GetIconSize()
        {
            int size = int.Parse(Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\Shell\Bags\1\Desktop").GetValue("IconSize").ToString());

            return new Size(size, size);
        }

        /// <summary>
        /// Get the files at the desktop
        /// </summary>
        /// <returns>List of the files</returns>
        public static List<DesktopFileItemModel> GetDesktopFiles()
        {
            List<DesktopFileItemModel> files = new List<DesktopFileItemModel>();

            Size size = GetIconSize();

            foreach (string file in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
            {
                files.Add(new DesktopFileItemModel(Icon.ExtractAssociatedIcon(file), size, Path.GetFileNameWithoutExtension(file)));
            }

            return files;
        }

        /// <summary>
        /// Get from the registry the path of desktop wallpaper
        /// </summary>
        /// <returns>Path</returns>
        public static string GetWallpaperPath()
        {
            return Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop").GetValue("Wallpaper").ToString();
        }
    }
}
