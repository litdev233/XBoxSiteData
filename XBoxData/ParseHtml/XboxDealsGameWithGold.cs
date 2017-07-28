using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace XBoxData.ParseHtml
{
    /// <summary>
    /// 金会员优惠游戏
    /// </summary>
    public class XboxDealsGameWithGold
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
            var games_div = doc.DocumentNode.Descendants().Where(p => p.Name == "div" && p.Attributes["class"] != null && p.Attributes["class"].Value.Contains("gameDivsWrapper")).ToList();
            if (games_div.Count > 0)
                DataBase.DB.XboxDealsWithGold.DelByLanguage(language);
            int i = 1;
            foreach (var game_platform in games_div)
            {
                var games_list = game_platform.Descendants().Where(p => p.Name == "section" && p.Attributes["class"] != null && p.Attributes["class"].Value.Contains("m-product-placement-item")).ToList();
                foreach (var game in games_list)
                {
                    string game_img_url = "";
                    string game_title = "";
                    string game_off = "";
                    int type = 0; //1:360,2:one
                    if (i == 1) type = 2;
                    else if (i == 2) type = 1;
                    
                    var pic_mo = game.Descendants().Where(p => p.Name == "img" && p.Attributes["class"] != null && p.Attributes["class"].Value.Contains("c-image")).FirstOrDefault();
                    //TODO 获取不到图片地址，这个arrt里是空的
                    if (pic_mo != null) game_img_url = pic_mo.Attributes["srcset"].Value;// + "&w=200&h=300&format=jpg";
                    var title_mo = game.Descendants().Where(p => p.Name == "h3" && p.Attributes["class"] != null && p.Attributes["class"].Value.Contains("c-heading")).FirstOrDefault();
                    if (title_mo != null) game_title = title_mo.InnerText;
                    //string remark = "";
                    //var avail_mo = game.Descendants().Where(p => p.Name == "span" && p.Attributes["class"] != null && p.Attributes["class"].Value.Contains("avail")).FirstOrDefault();
                    //if (avail_mo != null)
                    //{
                    //    remark = avail_mo.InnerText;
                    //}
                    var game_off_mo = game.Descendants().Where(p => p.Name == "span" && p.Attributes["class"] != null && p.Attributes["class"].Value.Contains("c-badge")).FirstOrDefault();
                    if (game_off_mo != null) game_off = game_off_mo.InnerText;


                    //排除的区域
                    var exclud_area = game.Attributes["data-region-exclude"].Value;
                    //if (string.IsNullOrWhiteSpace(exclud_area)) continue;
                    if (exclud_area.IndexOf(language) > -1) continue;
                    //包含的区域
                    var cloud_area = game.Attributes["data-region-include"].Value;
                    if (cloud_area.Length > 0)
                    {
                        if (exclud_area.IndexOf(language) == -1) continue;
                    }

                    
                    if (!string.IsNullOrWhiteSpace(game_title))
                    {
                        temp.Add(game_title);
                        DataBase.DB.XboxDealsWithGold.Add(language, game_title, game_img_url, game_off, type);

                    }
                }
                i++;
            }
            return temp.Count > 0;
        }
    }
}
