using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickXboxData.Model
{
    /// <summary>
    /// 金会员各区域价格转换json实体类
    /// </summary>
    public class XboxGoldPriceToJson
    {
        /// <summary>
        /// 当前货币代码
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int month { get; set; }

        /// <summary>
        /// 各个区域
        /// </summary>
        public List<XboxGoldPriceAreaToJson> area { get; set; }
    }

    /// <summary>
    /// 各个区域
    /// </summary>
    public class XboxGoldPriceAreaToJson: IComparable<XboxGoldPriceAreaToJson>
    {
        /// <summary>
        /// 区域名称
        /// </summary>
        public string area_name { get; set; }

        /// <summary>
        /// 原来的价格
        /// </summary>
        public decimal o_price { get; set; }

        /// <summary>
        /// 原来的价格货币代码
        /// </summary>
        public string o_currency { get; set; }

        /// <summary>
        /// 当前货币单位的价格
        /// </summary>
        public decimal price { get; set; }

        /// <summary>
        /// 当前区域
        /// </summary>
        public string currency { get; set; }

        public int CompareTo(XboxGoldPriceAreaToJson other)
        {
            if(null == other)
            {
                return 1;
            }
            return this.price.CompareTo(other.price);//升序
            //return other.price.CompareTo(this.price);//降序
        }
    }
}
