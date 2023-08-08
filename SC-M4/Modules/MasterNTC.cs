using SC_M4.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC_M4.Modules
{
    public class MasterNTC
    {
        public int id { get; set; }
        public string hex { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set;}
        public static void CreateTable()
        {
            string sql = string.Format("CREATE TABLE IF NOT EXISTS `master_ntc` (" +
                "`id`\tINTEGER NOT NULL," +
                "`hex`\tTEXT," +
                "\t`name`\tTEXT," +
                "`color`\tTEXT," +
                "`created_at`\tTEXT," +
                "`updated_at`\tTEXT," +
                " PRIMARY KEY(`id` AUTOINCREMENT)" +
                ");");
            SQliteDataAccess.Execute(sql,null);
        }

        public void Save(){
            string sql = "insert into master_ntc (hex,name,color,created_at,updated_at) values (@hex,@name,@color,@created_at,@updated_at)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@hex", hex);
            parameters.Add("@name", name);
            parameters.Add("@color", color);
            parameters.Add("@created_at", SQliteDataAccess.GetDateTimeNow());
            parameters.Add("@updated_at", SQliteDataAccess.GetDateTimeNow());
            SQliteDataAccess.Execute(sql, parameters);
        }

        public void Update(){
            string sql = "update master_ntc set hex = @hex, name = @name, color = @color, updated_at = @updated_at where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            parameters.Add("@hex", hex);
            parameters.Add("@name", name);
            parameters.Add("@color", color);
            parameters.Add("@updated_at", SQliteDataAccess.GetDateTimeNow());
            SQliteDataAccess.Execute(sql, parameters);
        }

        public void Delete(){
            string sql = "delete from master_ntc where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            SQliteDataAccess.Execute(sql, parameters);
        }

        public static void UpdateByHex(string hex,string color){
            string sql = "update master_ntc set color = @color, updated_at = @updated_at where hex = @hex";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@hex", hex);
            parameters.Add("@color", color);
            parameters.Add("@updated_at", SQliteDataAccess.GetDateTimeNow());
            SQliteDataAccess.Execute(sql, parameters);
        }
        public static List<MasterNTC> GetMasterNTC() => SQliteDataAccess.GetRow<MasterNTC>("select * from master_ntc order by id asc");
        public static List<MasterNTC> GetMasterNTC(int id) => SQliteDataAccess.GetRow<MasterNTC>("select * from master_ntc where id = " + id);
        public static List<MasterNTC> GetMasterNTC(int start, int end) => SQliteDataAccess.GetRow<MasterNTC>("select * from master_ntc where id between " + start + " and " + end);
        public static List<MasterNTC> GetMasterNTC(string name) => SQliteDataAccess.GetRow<MasterNTC>("select * from master_ntc where name = '" + name + "'");
        public static List<MasterNTC> GetMasterHex(string hex) => SQliteDataAccess.GetRow<MasterNTC>("select * from master_ntc where hex = '" + hex + "'");
        public static void UpdateColor(int id, string color){
            string sql = "update master_ntc set color = @color, updated_at = @updated_at where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            parameters.Add("@color", color);
            parameters.Add("@updated_at", SQliteDataAccess.GetDateTimeNow());
            SQliteDataAccess.Execute(sql, parameters);
        }
        public static bool isHexExist(string hex) => GetMasterHex(hex).Count > 0 ? true : false;

       
    }
    public class ExternNTC
    {
        public string hex { get; set; }
        public string name { get; set; }
        public string color { get; set; }
    }
}
