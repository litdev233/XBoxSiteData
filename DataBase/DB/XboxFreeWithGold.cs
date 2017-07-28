using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataBase.DB
{
    /// <summary>
    /// 金会员免费游戏
    /// </summary>
    public class XboxFreeWithGold
    {
        /// <summary>
        /// 添加一条
        /// </summary>
        /// <returns></returns>
        public static bool AddOrUpdate(string language, string game_name, string game_cover, string date)
        {
            if (string.IsNullOrWhiteSpace(game_name))
                return false;
            game_name = game_name.Replace("&amp;", "&");
            string strSql = "select * from XboxFreeGameWithGold where Language=@language and GameName=@name";
            SqlParameter[] exists_params = new SqlParameter[]{
                    new SqlParameter("@language",language),
                    new SqlParameter("@name",game_name)
                };
            var exists_db = DBHelper.getDataTablebySQL(strSql, exists_params);


            SqlParameter[] sql_params = new SqlParameter[] {
                    new SqlParameter("@language",language),
                    new SqlParameter("@name",game_name),
                    new SqlParameter("@cover",game_cover),
                    new SqlParameter("@date",date)
                };
            if (exists_db.Rows.Count == 0)
            {
                //添加
                string insertSql = "insert into XboxFreeGameWithGold (Language,GameName,GameCover,AvailDate) values(@language,@name,@cover,@date)";
                return DBHelper.ExecuteCommand(insertSql, sql_params) > 0;

            }
            else
            {
                //修改
                string updateSql = "update XboxFreeGameWithGold set GameCover=@cover,AvailDate=@date where Language=@language and GameName=@name";
                return DBHelper.ExecuteCommand(updateSql, sql_params) > 0;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static bool DelByLanguage(string language)
        {
            string strSql = "delete XboxFreeGameWithGold where Language='" + language + "'";
            return DBHelper.ExecuteSql(strSql) > 0;
        }
    }
}
