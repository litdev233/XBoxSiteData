using Gecko;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WINTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Xpcom.Initialize("Firefox");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //GeckoMIMEInputStream postdata = new GeckoMIMEInputStream();
            var postdata = Gecko.IO.MimeInputStream.Create();
            //postdata.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            //postdata.AddHeader("Accept-Encoding", "gzip, deflate");
            //postdata.AddHeader("Accept-Language", "zh-CN,zh;q=0.8");
            postdata.AddHeader("Cache-Control", "no-cache");
            postdata.AddHeader("Pragma", "no-cache");
            postdata.AddHeader("Upgrade-Insecure-Requests","1");
            //postdata.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36");
            var browser = new GeckoWebBrowser { Dock = DockStyle.Fill };

            nsICookieManager CookieMan;
            CookieMan = Xpcom.GetService<nsICookieManager>("@mozilla.org/cookiemanager;1");
            CookieMan = Xpcom.QueryInterface<nsICookieManager>(CookieMan);
            CookieMan.RemoveAll();

            this.Controls.Add(browser);
            browser.Navigate("http://www.xbox.com/zh-HK/live/games-with-gold?xr=shellnav", GeckoLoadFlags.FirstLoad, "", null);
            browser.DocumentCompleted += Browser_DocumentCompleted;
        }

        private void Browser_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            var browser = (GeckoWebBrowser)sender;
            System.Diagnostics.Debug.WriteLine("完成....");
        }
    }
    }
