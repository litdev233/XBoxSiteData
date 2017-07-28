using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Diagnostics;

namespace PickXboxData
{
    class Program
    {
        static int method = 0;
        static string processXboxSiteParseEXE = ConfigurationManager.AppSettings["XboxSiteParseEXE"];
        static void Main(string[] args)
        {
            if (args == null)
                return;

            try
            {
                method = int.Parse(args[0]);
            }
            catch { return; }

            switch (method)
            {
                case 1: //金会员价格
                    PollLiveGoldArea();
                    break;
                case 2: //金会员免费游戏
                    PollFreeGameWithGold();
                    break;
                case 3: //金会员优惠游戏
                    PollDealGameWithGold();
                    break;
                case 4: //EA会员免费游戏
                    GetEAAccessGames();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 轮询金会员区域获取价格
        /// </summary>
        static void PollLiveGoldArea()
        {
            DataBase.DB.XBoxLiveGold.ClearPriceTable();
            var area_list = DataBase.DB.XBoxLiveGold.GetAreaList();
            foreach (var area in area_list)
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = processXboxSiteParseEXE;
                info.Arguments = method.ToString() + " " + area.ID.ToString() + " " + area.Language;
                Process pro = Process.Start(info);
                pro.WaitForExit();
                Console.WriteLine("金会员价格："+area.Language+" 数据抓取完毕");
                System.Threading.Thread.Sleep(2000);
            }
            
            //完成后根据汇率生成app使用的数据
            var Exchangerate = Newtonsoft.Json.JsonConvert.DeserializeObject<Model.Exchangerate>(ToolHelper.GetExchangerateJson());
            if (Exchangerate == null) return;
            var exchangerate_currency = Exchangerate.value;
            if (exchangerate_currency == null) return;

            var area_price_list = DataBase.DB.XBoxLiveGold.GetAreaPriceList();
            List<Model.XboxGoldPriceToJson> json_list = new List<Model.XboxGoldPriceToJson>();
            var china_model = new DataBase.Model.XboxLiveGoldArea();
            china_model.CurrencyCode = "CNY";
            area_list.Add(china_model);

            foreach (var area in area_list)
            {
                var sel_month_group = area_price_list.GroupBy(p => p.Month);
                foreach (var sel_month in sel_month_group)
                {
                    Model.XboxGoldPriceToJson json_model = new Model.XboxGoldPriceToJson();
                    int month = sel_month.Key;
                    json_model.currency = area.CurrencyCode;
                    json_model.month = month;
                    List<Model.XboxGoldPriceAreaToJson> price_area_list = new List<Model.XboxGoldPriceAreaToJson>();
                    foreach (var item in sel_month.ToList())
                    {
                        Model.XboxGoldPriceAreaToJson temp_model = new Model.XboxGoldPriceAreaToJson();
                        temp_model.area_name = item.AreaName;
                        temp_model.o_currency = item.CurrencyCode;
                        temp_model.o_price = item.Price;
                        //转换汇率
                        //原来的货币
                        var exchangerate_now = exchangerate_currency.Where(p => p.name == item.CurrencyCode).FirstOrDefault();
                        if (exchangerate_now == null) continue;
                        //现在要转的货币
                        var exchangerate_old = exchangerate_currency.Where(p => p.name == area.CurrencyCode).FirstOrDefault();
                        if (exchangerate_old == null) continue;
                        decimal now_price_long = (item.Price / exchangerate_now.price) * exchangerate_old.price;
                        decimal now_price_short = decimal.Parse(now_price_long.ToString("#0.0000"));
                        temp_model.currency = area.CurrencyCode;
                        temp_model.price = now_price_short;
                        price_area_list.Add(temp_model);
                    }
                    price_area_list.Sort();
                    json_model.area = price_area_list;
                    json_list.Add(json_model);
                }
            }
            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(json_list);
            if (area_price_list.Count > 0)
            {
                ToolHelper.SaveGoldPriceToJsonFile(json_list);
                Console.WriteLine("金会员价格json生成完毕");
            }
        }
        
        /// <summary>
        /// 轮询获取各个区金会员免费游戏
        /// </summary>
        static void PollFreeGameWithGold()
        {
            var area_list = DataBase.DB.XBoxLiveGold.GetAreaList();
            foreach (var area in area_list)
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = processXboxSiteParseEXE;
                info.Arguments = method.ToString() + " "+ area.Language;
                Process pro = Process.Start(info);
                pro.WaitForExit();
                Console.WriteLine("Free With Gold Games Done ：" + area.Language);
                System.Threading.Thread.Sleep(20000);
            }
            DataBase.IOHelper.WriteLogs("金会员免费数据抓取完毕");
        }

        /// <summary>
        /// 轮询获取各个区金会员优惠游戏
        /// </summary>
        static void PollDealGameWithGold()
        {
            var area_list = DataBase.DB.XBoxLiveGold.GetAreaList();
            foreach (var area in area_list)
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = processXboxSiteParseEXE;
                info.Arguments = method.ToString() + " " + area.Language;
                Process pro = Process.Start(info);
                pro.WaitForExit();
                Console.WriteLine("金会员优惠游戏数据抓取完毕：" + area.Language);
            }
        }

        /// <summary>
        /// 获取EA会员免费游戏
        /// </summary>
        static void GetEAAccessGames()
        {
            //ProcessStartInfo info = new ProcessStartInfo();
            //info.FileName = processXboxSiteParseEXE;
            //info.Arguments = method.ToString();
            //Process pro = Process.Start(info);
            //pro.WaitForExit();
            //Console.WriteLine("EA会员游戏数据抓取完毕");
        }
        

        static string ReadHtml(string file_path)
        {
            if (!File.Exists(file_path)) return null;
            return File.ReadAllText(file_path);
        }
    }
}
