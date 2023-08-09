using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC_M4.Modules
{
    public class Rect
    {
        public int id { get; set; }
        public int action_id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int threshold { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

        public static void CreateTable()
        {
            string sql = string.Format("CREATE TABLE IF NOT EXISTS `rect` (" +
                "`id`\tINTEGER NOT NULL," +
                "`action_id`\tINTEGER NOT NULL," +
                "`x`\tINTEGER NOT NULL," +
                "`y`\tINTEGER NOT NULL," +
                "`width`\tINTEGER NOT NULL," +
                "`height`\tINTEGER NOT NULL," +
                "`threshold`\tINTEGER NOT NULL," +
                "`created_at`\tTEXT," +
                "`updated_at`\tTEXT," +
                " PRIMARY KEY(`id` AUTOINCREMENT)" +
                ");");
            SQliteDataAccess.Execute(sql, null);
        }
        private Dictionary<string, object> CreateParameters()
        {
            return new Dictionary<string, object>
                {
                    { "@id", id },
                    { "@action_id", action_id },
                    { "@x", x },
                    { "@y", y },
                    { "@width", width },
                    { "@height", height },
                    { "@threshold", threshold },
                    { "@created_at", SQliteDataAccess.GetDateTimeNow() },
                    { "@updated_at", SQliteDataAccess.GetDateTimeNow() }
                };
        }
        public void Save()
        {
            string sql = "insert into rect (action_id,x,y,width,height,threshold,created_at,updated_at) values (@action_id,@x,@y,@width,@height,@threshold,@created_at,@updated_at)";
            SQliteDataAccess.Execute(sql, CreateParameters());
        }

        public void Update()
        {
            string sql = "update rect set action_id = @action_id, x = @x, y = @y, width = @width, height = @height, threshold = @threshold, updated_at = @updated_at where id = @id";
            SQliteDataAccess.Execute(sql, CreateParameters());
        }

        public void Delete()
        {
            string sql = "delete from rect where id = @id";
            SQliteDataAccess.Execute(sql, CreateParameters());
        }

        public static List<Rect> Get()
        {
            string sql = "select * from rect order by id desc limit 1000";
            return SQliteDataAccess.Query<Rect>(sql, null);
        }
        public static Rect Get(int id)
        {
            string sql = "select * from rect where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@id", id } };
            return SQliteDataAccess.Query<Rect>(sql, parameters).FirstOrDefault();
        }
        public static List<Rect> GetByAction(int action_id)
        {
            string sql = "select * from rect where action_id = @action_id order by id desc limit 1000";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@action_id", action_id } };
            return SQliteDataAccess.Query<Rect>(sql, parameters);
        }
    }
}
