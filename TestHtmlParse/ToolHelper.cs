using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Web;
using System.Net;

namespace PickXboxData
{
    public class ToolHelper
    {
        /// <summary>
        /// 下载汇率json文件
        /// </summary>
        /// <returns></returns>
        public static string GetExchangerateJson()
        {
            try
            {
                //网络json文件地址
                string json_url = ConfigurationManager.AppSettings["ExchangerateFileWebUrl"];// "http://pfile-dl.flyme.cn/exchangerate/exchangerate_flyme5.json";
                                                                                             //本地保存json文件地址
                string save_path = ConfigurationManager.AppSettings["ExchangerateFile"];
                //本地json文件完整路径
                string full_path = save_path + "\\exchangerate.json";

                if (!Directory.Exists(save_path)) Directory.CreateDirectory(save_path);
                if (File.Exists(full_path)) File.Delete(full_path);
                if (File.Exists(full_path)) File.Delete(full_path);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(json_url);
                Stream stream = request.GetResponse().GetResponseStream();
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string line = string.Empty;
                    while ((line = reader.ReadLine()) != null)
                    {
                        UTF8Encoding utf8 = new UTF8Encoding(false);
                        //写txt文件  
                        using (StreamWriter sw = new StreamWriter(full_path, true, utf8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }

                return File.ReadAllText(full_path);
            }
            catch (Exception ex)
            {
                DataBase.IOHelper.WriteLogs("下载汇率json文件出错：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 保存生成后的价格json到文件里
        /// </summary>
        /// <param name="list"></param>
        public static void SaveGoldPriceToJsonFile(List<Model.XboxGoldPriceToJson> list)
        {
            string json_save_path = ConfigurationManager.AppSettings["XboxLiveGoldPriceFolor"] + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
            if (Directory.Exists(json_save_path)) DataBase.IOHelper.ClearFolder(json_save_path);
            if (!Directory.Exists(json_save_path)) Directory.CreateDirectory(json_save_path);
            foreach (var item in list)
            {
                string file_name = item.currency + "-" + item.month.ToString() + ".json";
                string full_name = json_save_path + file_name;
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(item.area);
                UTF8Encoding utf8 = new UTF8Encoding(false);
                using (StreamWriter sw = new StreamWriter(full_name, true, utf8))
                {
                    sw.WriteLine(json);
                }
            }
        }

        
    }
}
