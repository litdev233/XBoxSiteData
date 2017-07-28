using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using System.Collections;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string free_json = DataBase.IOHelper.GetHttp("http://www.xbox.com/en-US/live/games-with-gold/rowJS/gwg-globalContent.js");
            free_json = free_json.Substring(0, free_json.LastIndexOf("globalContentOld"));
            free_json = free_json.Replace("globalContentNew = ","");
            //json = json.Substring(0, json.LastIndexOf("}"));

            JsonData jd_root_free = JsonMapper.ToObject(free_json);
            if (jd_root_free.Count != 1) return;
            var dic_free = ((IDictionary)jd_root_free[0]);
            Console.WriteLine("金会员免费区域数量：" + dic_free.Count);
            foreach (DictionaryEntry dic_item in dic_free)
            {
                string area = dic_item.Key.ToString();
                var temp_area = area.Split('-');
                string language = temp_area[0] + "-" + temp_area[1].ToUpper();
                JsonData jd = (JsonData)dic_item.Value;
                var dic_jd = ((IDictionary)jd);
                DataBase.IOHelper.WriteLogs(language);
                #region 当前会免游戏
                for (int i = 1; i <= 6; i++)
                {
                    string key_name = "keyCopytitlenowgame" + i.ToString();
                    string key_store = "keyLinknowgame" + i.ToString();
                    string key_cover = "keyImagenowgame" + i.ToString();
                    string key_date = "keyCopydatesnowgame" + i.ToString();
                    if (dic_jd.Contains(key_name))
                    {
                        var name = jd[key_name].ToString();
                        if (string.IsNullOrWhiteSpace(name) || name == "####") continue;
                        var store = jd[key_store].ToString().Replace("/store", "/" + language + "/store");
                        var cover = jd[key_cover].ToString();
                        var date = jd[key_date].ToString();
                        Console.WriteLine(language+",会免游戏：" + name + ",时间：" + date + ",封面:");
                    }
                }
                #endregion

                #region 追加游戏
                //本月追加游戏，Available 7/16
                var available = jd["keyCopylateravailable"].ToString();
                if (string.IsNullOrWhiteSpace(available) || available == "####") continue;
                for (int i = 1; i <= 5; i++)
                {
                    string key_name = "keyCopytitlelatergame" + i.ToString();
                    //追加的游戏没有商城链接
                    //string key_store = "keyLinknowgame" + i.ToString();
                    string key_cover = "keyImagelatergame" + i.ToString();
                    string key_date = "keyCopydateslatergame" + i.ToString();
                    if (dic_jd.Contains(key_name))
                    {
                        var name = jd[key_name].ToString();
                        if (string.IsNullOrWhiteSpace(name) || name == "####") continue;
                        //var store = jd[key_store].ToString().Replace("/store", "/" + language + "/store");
                        var cover = jd[key_cover].ToString();
                        var date = jd[key_date].ToString();
                        Console.WriteLine(language + ",追加游戏：" + name + ",时间：" + date + ",封面:");
                    }
                } 
                #endregion

            }



            Console.ReadKey();
        }
    }
}
