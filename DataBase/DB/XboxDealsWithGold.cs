using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DataBase.DB
{
    /// <summary>
    /// 金会员优惠游戏
    /// </summary>
    public class XboxDealsWithGold
    {
        /// <summary>
        /// 添加一条
        /// </summary>
        /// <param name="type">1:360,2:one</param>
        /// <returns></returns>
        public static bool Add(string language,string game_name,string game_cover,string off,int type)
        {
            if (string.IsNullOrWhiteSpace(game_name)) return false;
            game_name = game_name.Replace("&amp;", "&");

            SqlParameter[] sql_params = new SqlParameter[] {
                    new SqlParameter("@language",language),
                    new SqlParameter("@name",game_name),
                    new SqlParameter("@cover",game_cover),
                    new SqlParameter("@off",off),
                    new SqlParameter("@type",type)
                };

            //添加
            string insertSql = "insert into XboxDealsGameWithGold (Language,GameName,GameCover,DiscountOff,Type) values(@language,@name,@cover,@off,@type)";
            return DBHelper.ExecuteCommand(insertSql,sql_params) > 0;
        }

        /// <summary>
        /// 漏抓的区域列表
        /// </summary>
        /// <returns></returns>
        public static List<Model.XboxLiveGoldArea> GetAreaListLou()
        {
            string strSql = "select * from XBoxGoldArea where [Language] not in(select [Language] from XboxDealsGameWithGold group BY [Language])";
            DataTable dt_db = DBHelper.getDataTablebySQL(strSql);
            return ModelConvertHelper<Model.XboxLiveGoldArea>.ConvertToModel(dt_db);
        }

        /// <summary>
        /// 删除某个语言区域下的数据
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static bool DelByLanguage(string language)
        {
            string delSql = "delete XboxDealsGameWithGold where Language='" + language + "'";
            return DBHelper.ExecuteSql(delSql) > 0;
        }
    }
}
