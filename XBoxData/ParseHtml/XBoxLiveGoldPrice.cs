using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace XBoxData.ParseHtml
{
    /// <summary>
    /// 金会员价格
    /// </summary>
    public class XBoxLiveGoldPrice
    {
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="html"></param>
        public static bool Parse(string html,int area_id)
        {
            if (string.IsNullOrWhiteSpace(html)) return false;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var data_list = doc.DocumentNode.Descendants().Where(p => p.Name == "div" && p.Attributes["class"] != null && p.Attributes["class"].Value.Contains("cli_availability active")).ToList();
            List<string> temp = new List<string>();
            foreach (var data in data_list)
            {
                int mongth = GetGoldMonth(data.Attributes["data-belonging-sku-id"].Value);
                var meta_list = data.SelectNodes(".//meta").ToList();
                var price_str = meta_list[0].Attributes["content"].Value;
                if (price_str.Equals("0")) continue;
                var price_currency = meta_list[1].Attributes["content"].Value;
                if (price_str.Length > 1 && price_str.Length > 0)
                    temp.Add(price_currency + "-" + price_str);

                //DataBase.IOHelper.WriteLogs(mongth.ToString() + "---" + price_str + "---" + price_currency);
                DataBase.DB.XBoxLiveGold.AddAreaPrice(area_id, mongth, decimal.Parse(price_str), "", "");
            }

            return temp.Count>0;
        }


        /// <summary>
        /// 根据SKUid得到月份
        /// </summary>
        /// <param name="sku_id"></param>
        /// <returns></returns>
        static int GetGoldMonth(string sku_id)
        {
            if (sku_id.Equals("000C"))
                return 1;
            else if (sku_id.Equals("000D"))
                return 3;
            else if (sku_id.Equals("000G"))
                return 12;
            else
                return 0;
        }
    }
}
