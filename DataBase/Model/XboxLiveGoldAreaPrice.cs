using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Model
{
    /// <summary>
    /// 金会员区域价格
    /// </summary>
    public class XboxLiveGoldAreaPrice
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        public int AreaID { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 区域价格
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 区域货币代码
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 当前区域的价格
        /// </summary>
        public decimal Price { get; set; }
        
    }
}
