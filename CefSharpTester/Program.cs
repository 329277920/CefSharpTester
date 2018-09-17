using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CefSharpTester
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void Init()
        {
            var settings = new CefSettings();
            settings.LogFile = @"F:\Git\CefSharpTester\CefSharpTester\Log\log.log";
            settings.CachePath = @"F:\Git\CefSharpTester\CefSharpTester\Cache";
            Cef.Initialize(settings);
        }
    }
}
