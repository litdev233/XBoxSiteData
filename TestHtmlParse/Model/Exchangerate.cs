using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickXboxData.Model
{
    /// <summary>
    /// 汇率json
    /// </summary>
    public class Exchangerate
    {
        public int code { get; set; }

        public string message { get; set; }

        public string redirect { get; set; }

        /// <summary>
        /// 各个货币单位兑美元价格
        /// </summary>
        public List<ExchangerateCurrency> value { get; set; }
    }

    public class ExchangerateCurrency
    {
        /// <summary>
        /// 货币代码
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 该货币兑美元汇率
        /// </summary>
        public decimal price { get; set; }

        public int ts { get; set; }

    }
}
