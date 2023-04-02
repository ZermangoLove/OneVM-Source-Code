using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace xVM.GUI
{
    internal static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            using (var mutex = new Mutex(true, "1191bb8624264f8d8222d6129c9f88ea"))
            {
                if (mutex.WaitOne(TimeSpan.Zero))
                {  
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Main());
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
