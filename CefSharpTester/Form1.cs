using CefSharp;
using CefSharp.WinForms;
using CefSharpTester.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CefSharpTester
{
    public partial class Form1 : Form
    {
        private CustomeWebBrowser _browser;

        public Form1()
        {
            InitializeComponent();
            this._browser = new CustomeWebBrowser();

            this._browser.OnBeforeResourceLoad += _browser_OnBeforeResourceLoad;
            this._browser.OnResourceLoadComplete += _browser_OnResourceLoadComplete;

            this._browser.Dock = DockStyle.Fill;
            this.Controls.Add(this._browser);

            this._browser.Load("https://toutiao.io/");
        }

        private int idx = 0;
        private void _browser_OnResourceLoadComplete(object sender, ResourceLoadCompleteEventArgs e)
        {
            if (e.Content != null)
            {
                var filePath = @"F:\Git\CefSharpTester\CefSharpTester\Images\" + $"{++idx}.jpg";
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    e.Content.CopyTo(fs);
                }             
            }
        }

     

        private void _browser_OnBeforeResourceLoad(object sender, BeforeResourceLoadEventArgs e)
        {
            if (e.ResourceType == ResourceType.Image)
            {
                e.CacheToQueue = true;
            }           
        }
    }
}
