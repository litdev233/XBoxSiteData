using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataBase.DB
{
    /// <summary>
    /// 金会员数据操作
    /// </summary>
    public class XBoxLiveGold
    {
        /// <summary>
        /// 获取区域列表,要循环的
        /// </summary>
        /// <returns></returns>
        public static List<Model.XboxLiveGoldArea> GetAreaList()
        {
            string strSql = "select * from XBoxGoldArea";
            DataTable dt_db = DBHelper.getDataTablebySQL(strSql);
            return ModelConvertHelper<Model.XboxLiveGoldArea>.ConvertToModel(dt_db);
        }

        /// <summary>
        /// 漏抓的区域列表
        /// </summary>
        /// <returns></returns>
        public static List<Model.XboxLiveGoldArea> GetAreaListLou()
        {
            string strSql = "select * from XBoxGoldArea where ID not in(SELECT XBoxGoldAreaID FROM [dbo].[XBoxGoldPrice] GROUP BY XBoxGoldAreaID)";
            DataTable dt_db = DBHelper.getDataTablebySQL(strSql);
            return ModelConvertHelper<Model.XboxLiveGoldArea>.ConvertToModel(dt_db);
        }

        /// <summary>
        /// 获取所有区域价格
        /// </summary>
        /// <param name="date">日期，默认当天</param>
        /// <returns></returns>
        public static List<Model.XboxLiveGoldAreaPrice> GetAreaPriceList(string date = null)
        {
            if (string.IsNullOrWhiteSpace(date))
                date = DateTime.Now.ToString("yyyy-MM-dd");

            string strSql = "SELECT A.ID as AreaID,A.Name as AreaName,A.[Language] as [Language],A.CurrencyCode as CurrencyCode,P.[Month] as [Month],P.Price as Price FROM [dbo].[XBoxGoldPrice] as P left JOIN XBoxGoldArea as A on P.XBoxGoldAreaID = A.ID where P.UpdateTime between '" + date + " 00:00:00' and '" + date + " 23:59:59';";
            DataTable dt_db = DBHelper.getDataTablebySQL(strSql);
            return ModelConvertHelper<Model.XboxLiveGoldAreaPrice>.ConvertToModel(dt_db);
        }

        /// <summary>
        /// 添加一条区域的价格
        /// </summary>
        /// <returns></returns>
        public static bool AddAreaPrice(int area_id, int month, decimal price, string remark, string remark2)
        {
            string strSqlTemplate = "select * from XBoxGoldPrice where XBoxGoldAreaID={0} and Month={1} and UpdateTime between '{2} 00:00:00' and '{2} 23:59:59'";
            string strSql = string.Format(strSqlTemplate, area_id, month, DateTime.Now.ToString("yyyy-MM-dd"));
            var exists_db = DBHelper.getDataTablebySQL(strSql);
            if (exists_db.Rows.Count == 0)
            {
                //添加
                string insertSqlTemplate = "insert into XBoxGoldPrice (XBoxGoldAreaID,Month,Price,Remark,Remark2,UpdateTime) values({0},{1},{2},'{3}','{4}',getdate())";
                string insertSql = string.Format(insertSqlTemplate, area_id, month, price, remark, remark2);
                return DBHelper.ExecuteSql(insertSql) > 0;

            }
            else
            {
                //修改
                string updateSqlTemplate = "update XBoxGoldPrice set price={0},UpdateTime=getdate() where ID ={1}";
                string updateSql = string.Format(updateSqlTemplate, price, area_id);
                return DBHelper.ExecuteSql(updateSql) > 0;
            }
        }

        /// <summary>
        /// 清空价格表
        /// </summary>
        /// <returns></returns>
        public static bool ClearPriceTable()
        {
            string strSql = "TRUNCATE table [dbo].[XBoxGoldPrice]";
            return DBHelper.ExecuteSql(strSql) > 0;
        }
    }
}
