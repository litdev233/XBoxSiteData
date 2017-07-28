using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Model
{
    /// <summary>
    /// 金会员区域
    /// </summary>
    public class XboxLiveGoldArea
    {
        public int ID { get; set; }

        /// <summary>
        /// 首字母
        /// </summary>
        public string Letter { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 语言简写，zh-CN
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 货币代码,CNY，转大写
        /// </summary>
        public string CurrencyCode { get; set; }
    }
}
