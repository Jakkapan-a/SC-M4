using SC_M4.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC_M4.Modules
{
    public class MasterLB : IDataItem
    {
        public int id { get; set;}
        public int master_sw_id { get; set;}
        public string name { get; set;}
        public string color_name { get; set; }
        public string created_at { get; set;}
        public string updated_at { get; set;}

        public static void CreateTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS `master_lb` (`id` INTEGER NOT NULL, `master_sw_id` INTEGER, `name` TEXT, `color_name` TEXT, `created_at` TEXT, `updated_at` TEXT, PRIMARY KEY(`id` AUTOINCREMENT));";
            SQliteDataAccess.Execute(sql, null);
        }

        public void Save()
        {
            string sql = "insert into master_lb (master_sw_id,name,color_name,created_at,updated_at) values (@master_sw_id,@name,@color_name,@created_at,@updated_at)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@master_sw_id", master_sw_id);
            parameters.Add("@name", name);
            parameters.Add("@color_name", color_name);
            parameters.Add("@created_at", SQliteDataAccess.GetDateTimeNow());
            parameters.Add("@updated_at", SQliteDataAccess.GetDateTimeNow());
            SQliteDataAccess.Execute(sql, parameters);
        }

        public void Update()
        {
            string sql = "update master_lb set master_sw_id = @master_sw_id, name = @name, color_name=@color_name, updated_at = @updated_at where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            parameters.Add("@master_sw_id", master_sw_id);
            parameters.Add("@name", name);
            parameters.Add("@color_name", color_name);
            parameters.Add("@updated_at", SQliteDataAccess.GetDateTimeNow());
            SQliteDataAccess.Execute(sql, parameters);
        }
        public void Delete()
        {
            string sql = "delete from master_lb where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            SQliteDataAccess.Execute(sql, parameters);
        }
        public static List<MasterLB> Get() => SQliteDataAccess.GetRow<MasterLB>("select * from master_lb order by id desc");
 
        public static List<MasterLB> GetMasterLB() => SQliteDataAccess.GetRow<MasterLB>("select * from master_lb order by id desc");

        public static List<MasterLB> GetMasterLB(int id) => SQliteDataAccess.GetRow<MasterLB>("select * from master_lb where id = " + id);

        public static List<MasterLB> GetMasterLB(string name) => SQliteDataAccess.GetRow<MasterLB>("select * from master_lb where name = '" + name + "'");

        public static List<MasterLB> GetMasterLB(string name, int id) => SQliteDataAccess.GetRow<MasterLB>("select * from master_lb where name = '" + name + "' and id <> " + id);

        public static List<MasterLB> GetMasterLB(int master_sw_id, string name) => SQliteDataAccess.GetRow<MasterLB>("select * from master_lb where master_sw_id = " + master_sw_id + " and name = '" + name + "'");

        public static List<MasterLB> GetMasterLBBySW(int master_sw_id) => SQliteDataAccess.GetRow<MasterLB>("select * from master_lb where master_sw_id = " + master_sw_id + " ");

        public static int getTotalLB(int id, string name){
            string sql = "select count(*) as total from master_lb where not id = " + id + " and name = '" + name + "'";
            return SQliteDataAccess.GetRow<int>(sql).FirstOrDefault();
        }

        public static bool IsExist(int id, string name) => getTotalLB(id,name) > 0 ? true : false;
        public static bool IsExist(string name) => GetMasterLB(name).Count > 0 ? true : false;


        public static void Delete(int id_lb) => SQliteDataAccess.Execute("delete from master_lb where id = " + id_lb, null);
    }
}
