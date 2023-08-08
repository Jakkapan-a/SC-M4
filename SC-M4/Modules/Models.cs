using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC_M4.Modules
{
    public class Models
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

        public static void CreateTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS `models` (`id` INTEGER NOT NULL, `name` TEXT, `description` TEXT, `created_at` TEXT, `updated_at` TEXT, PRIMARY KEY(`id` AUTOINCREMENT));";
            SQliteDataAccess.Execute(sql, null);
        }

        private Dictionary<string, object> CreateParameters()
        {
            return new Dictionary<string, object>
        {
            { "@id", id },
            { "@name", name },
            { "@description", description },
            { "@created_at", SQliteDataAccess.GetDateTimeNow() },
            { "@updated_at", SQliteDataAccess.GetDateTimeNow() }
        };
        }

        public void Save()
        {
            string sql = "insert into models (name,description,created_at,updated_at) values (@name,@description,@created_at,@updated_at)";
            SQliteDataAccess.Execute(sql, CreateParameters());
        }

        public void Update()
        {
            string sql = "update models set name = @name, description = @description, updated_at = @updated_at where id = @id";
            SQliteDataAccess.Execute(sql, CreateParameters());
        }

        public void Delete()
        {
            string sql = "delete from models where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@id", id } };
            SQliteDataAccess.Execute(sql, parameters);
        }

        public static List<Models> Get()
        {
            string sql = "select * from models order by id desc limit 1000";
            return SQliteDataAccess.Query<Models>(sql, null);
        }

        public static Models Get(int id)
        {
            string sql = "select * from models where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@id", id } };
            return SQliteDataAccess.Query<Models>(sql, parameters).FirstOrDefault();
        }


        public static Models Get(string name)
        {
            string sql = "select * from models where name = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", name);
            return SQliteDataAccess.Query<Models>(sql, parameters).FirstOrDefault();
        }

        public static List<Models> Get(string name, string description)
        {
            string sql = "select * from models where name = @name and description = @description";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", name);
            parameters.Add("@description", description);
            return SQliteDataAccess.Query<Models>(sql, parameters);
        }

        public static bool IsExist(string name)
        {
            string sql = "select * from models where name = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", name);
            return SQliteDataAccess.Query<Models>(sql, parameters).Count > 0;
        }
    }







    public enum TypeAction
    {
        io_manual,
        io_auto,
        image
    }
    public enum TypeImage
    {
        full,
        rect
    }
}
