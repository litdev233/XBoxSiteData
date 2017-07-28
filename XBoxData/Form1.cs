using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;

namespace XBoxData
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public extern static bool SetForegroundWindow(int hWnd);

        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        
        static bool IsActivated = false;

        /// <summary>
        /// 保存的Html文件缓存路径
        /// </summary>
        static string XBoxHtmlCachePath = ConfigurationManager.AppSettings["XBoxHtmlCachePath"];

        /// <summary>
        /// 要打开保存的网页
        /// </summary>
        static string XboxSiteUrl = "";

        string[] args = null;
        int method = 0;
        int area_id = 0;
        string language = "";

        public Form1()
        {
            InitializeComponent();

            this.args = new string[] { "3", "zh-HK" };
            //Process.GetCurrentProcess().Kill();
            Hide();

        }

        public Form1(string[] args)
        {
            InitializeComponent();
            this.args = args;
            Hide();
        }
        

        //http://www.enhanceie.com/ua.aspx
        private void Form1_Activated(object sender, EventArgs e)
        {
            if (IsActivated) return;

            method = int.Parse(args[0]);
            switch (method)
            {
                case 1: //金会员价格
                    area_id = int.Parse(args[1]);
                    language = args[2];
                    XboxSiteUrl = string.Format(ConfigurationManager.AppSettings["XBoxLiveGoldPriceUrl"], language);
                    StartBrowserProcess(XboxSiteUrl);
                    break;
                case 2://金会员免费
                    language = args[1];
                    XboxSiteUrl = string.Format(ConfigurationManager.AppSettings["XboxFreeGameWithGold"], language);
                    break;
                case 3://金会员优惠
                    language = args[1];
                    XboxSiteUrl = string.Format(ConfigurationManager.AppSettings["XboxDealsGameWithGold"], language);
                    break;
                default:
                    break;
            }
            
            IsActivated = !IsActivated;            
        }



        /// <summary>
        /// 保存指定网站的网页
        /// </summary>
        /// <param name="url"></param>
        void StartBrowserProcess(string url)
        {
            string chrome_path = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";//@"C:\Program Files\Mozilla Firefox\firefox.exe";
            var process = Process.Start(chrome_path, url);
            string process_title = process.MainWindowTitle;

            System.Threading.Thread.Sleep(15000);
            Io_Api ia = new Io_Api();

            var window = FindWindow(null, process_title);
            if (window != IntPtr.Zero)
            {
                SwitchToThisWindow(window, true);
            }
            ia.mouse_click("L");
            //发送Ctrl+S组合键保存网页
            byte[] s = { ia.getKeys("LControl"), ia.getKeys("S") };
            ia.keybd(s);
            System.Threading.Thread.Sleep(1000);

            ia.keybd(ia.getKeys("A"));

            System.Threading.Thread.Sleep(1000);
            
            ia.keybd(ia.getKeys("Enter"));
            //等待下载完
            System.Threading.Thread.Sleep(10000);
            //解析DOM文档
            DoParse(XBoxHtmlCachePath + "\\a.html");
            //等待1s再关闭浏览器
            System.Threading.Thread.Sleep(1000);
            process.Kill();

            //清空缓存文件夹
            DataBase.IOHelper.ClearFolder(XBoxHtmlCachePath);

            //关掉当前程序
            Process.GetCurrentProcess().Kill();
        }
    
        void DoParse(string html_path)
        {
            if (!File.Exists(html_path)) return;
            var html = ReadHtml(html_path);
            var is_ok = false;
            switch (method)
            {
                case 1://金会员价格
                    is_ok = ParseHtml.XBoxLiveGoldPrice.Parse(html, area_id);
                    break;
                case 2://金会员免费游戏
                    is_ok = ParseHtml.XboxFreeGameWithGold.Parse(html, language);
                    break;
                case 3://金会员优惠游戏
                    is_ok = ParseHtml.XboxDealsGameWithGold.Parse(html, language);
                    break;
                case 4://EA会员免费游戏

                    break;
                default:
                    break;
            }
            if (is_ok)
            {
                //DataBase.IOHelper.WriteLogs("正常结束，干掉程序");
                //Process.GetCurrentProcess().Kill();
            }
        }

        static string ReadHtml(string file_path)
        {
            if (!File.Exists(file_path)) return null;
            return File.ReadAllText(file_path);
        }

        
    }
}
