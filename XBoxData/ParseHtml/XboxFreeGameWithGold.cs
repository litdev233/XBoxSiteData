using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace XBoxData.ParseHtml
{
    /// <summary>
    /// 金会员免费游戏
    /// </summary>
    public class XboxFreeGameWithGold
    {
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="html"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static bool Parse(string html, string language)
        {
            if (string.IsNullOrWhiteSpace(html)) return false;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            List<string> temp = new List<string>();
            var games_div = doc.DocumentNode.Descendants().Where(p => p.Name == "div" && p.Attributes["class"] != null && p.Attributes["class"].Value.Contains("gameDivsWrapper")).FirstOrDefault();
            if (games_div != null)
            {
                var games_list = games_div.Descendants().Where(p => p.Name == "section" && p.Attributes["class"] != null && p.Attributes["class"].Value.Contains("m-product-placement-item")).ToList();
                if(games_list.Count>2)
                {
                    DataBase.DB.XboxFreeWithGold.DelByLanguage(language);
                }
                foreach (var game in games_list)
                {
                    string game_img_url = "";
                    string game_title = "";
                    string game_time = "";

                    var pic_mo = game.Descendants().Where(p => p.Name == "img" && p.Attributes["class"] != null && p.Attributes["class"].Value.Contains("c-image")).FirstOrDefault();
                    if (pic_mo != null) game_img_url = pic_mo.Attributes["srcset"].Value + "&w=200&h=300&format=jpg";
                    var title_mo = game.Descendants().Where(p => p.Name == "h3" && p.Attributes["class"] != null && p.Attributes["class"].Value.Contains("c-heading")).FirstOrDefault();
                    if (title_mo != null) game_title = title_mo.InnerText;
                    //string remark = "";
                    //var avail_mo = game.Descendants().Where(p => p.Name == "span" && p.Attributes["class"] != null && p.Attributes["class"].Value.Contains("avail")).FirstOrDefault();
                    //if (avail_mo != null)
                    //{
                    //    remark = avail_mo.InnerText;
                    //}
                    var avail_date_mo = game.Descendants().Where(p => p.Name == "span" && p.Attributes["class"] != null && p.Attributes["class"].Value.Contains("availDate")).FirstOrDefault();
                    if (avail_date_mo != null) game_time = avail_date_mo.InnerText;
                    if (!string.IsNullOrWhiteSpace(game_title))
                    {
                        temp.Add(game_title);
                        DataBase.DB.XboxFreeWithGold.AddOrUpdate(language, game_title, game_img_url, game_time);
                    }
                }
            }
            return temp.Count > 2;
        }
    }
}
