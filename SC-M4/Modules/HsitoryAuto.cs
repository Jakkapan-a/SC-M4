using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC_M4.Modules
{
    public class HistoryAuto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string sw_ver { get; set; }
        public string lb_ver { get; set; }
        public string step { get; set; }
        public int time_s { get; set; }
        public string result { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

        public static void CreateTable(){
            string sql = "CREATE TABLE IF NOT EXISTS `history_auto` (`id` INTEGER NOT NULL, `name` TEXT, `sw_ver` TEXT, `lb_ver` TEXT, `step` TEXT, `time_s` INTEGER, `result` TEXT, `created_at` TEXT, `updated_at` TEXT, PRIMARY KEY(`id` AUTOINCREMENT));";
            SQliteDataAccess.Execute(sql, null);
        }

        private Dictionary<string, object> CreateParameters()
        {
            return new Dictionary<string, object>
                {
                    { "@id", id },
                    { "@name", name },
                    { "@sw_ver", sw_ver },
                    { "@lb_ver", lb_ver },
                    { "@step", step },
                    { "@time_s", time_s },
                    { "@result", result },
                    { "@created_at", SQliteDataAccess.GetDateTimeNow() },
                    { "@updated_at", SQliteDataAccess.GetDateTimeNow() }
                };
        }

        public void Save()
        {
            string sql = "insert into history_auto (name,sw_ver,lb_ver,step,time_s,result,created_at,updated_at) values (@name,@sw_ver,@lb_ver,@step,@time_s,@result,@created_at,@updated_at)";
            
            SQliteDataAccess.Execute(sql, CreateParameters());
        }

        public void Update()
        {
            string sql = "update history_auto set name = @name, sw_ver = @sw_ver, lb_ver = @lb_ver, step = @step, time_s = @time_s, result = @result, updated_at = @updated_at where id = @id";
            SQliteDataAccess.Execute(sql, CreateParameters());
        }

        public void Delete()
        {
            string sql = "delete from history_auto where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@id", id } };
            SQliteDataAccess.Execute(sql, parameters);
        }

        public static List<HistoryAuto> Get()
        {
            string sql = "select * from history_auto order by id desc limit 50";
            return SQliteDataAccess.Query<HistoryAuto>(sql, null);
        }
    }
}
