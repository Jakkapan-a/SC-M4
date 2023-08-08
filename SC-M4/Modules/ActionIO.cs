using System.Collections.Generic;

namespace SC_M4.Modules
{
    public class ActionIO
    {
        public int id { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public string hex { get; set; }
        public int values { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public static void CreateTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS `action_io` (`id` INTEGER NOT NULL, `name` TEXT, `type` INTEGER NOT NULL, `hex` TEXT, `values` INTEGER, `created_at` TEXT, `updated_at` TEXT, PRIMARY KEY(`id` AUTOINCREMENT));";
            SQliteDataAccess.Execute(sql, null);
        }

        private Dictionary<string, object> CreateParameters()
        {
            return new Dictionary<string, object>
                {
                    { "@id", id },
                    { "@name", name },
                    { "@type", type },
                    { "@hex", hex },
                    { "@values", values },
                    { "@created_at", SQliteDataAccess.GetDateTimeNow() },
                    { "@updated_at", SQliteDataAccess.GetDateTimeNow() }
                };
        }

        public void Save()
        {
            string sql = "insert into action_io (name,type,hex,values,created_at,updated_at) values (@name,@type,@hex,@values,@created_at,@updated_at)";
            SQliteDataAccess.Execute(sql, CreateParameters());
        }

        public void Update()
        {
            string sql = "update action_io set name = @name, type = @type, hex = @hex, values = @values, updated_at = @updated_at where id = @id";
            SQliteDataAccess.Execute(sql, CreateParameters());
        }

        public void Delete()
        {
            string sql = "delete from action_io where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@id", id } };
            SQliteDataAccess.Execute(sql, parameters);
        }

        public static List<ActionIO> Get()
        {
            string sql = "select * from action_io order by id desc limit 1000";
            return SQliteDataAccess.Query<ActionIO>(sql, null);
        }

        public static List<ActionIO> GetByType(TypeAction type)
        {
            string sql = "select * from action_io where type = @type order by id desc limit 1000";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@type", type } };
            return SQliteDataAccess.Query<ActionIO>(sql, parameters);
        }

        public static bool IsExist(string name){
            string sql = "select * from action_io where name = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@name", name } };
            return SQliteDataAccess.Query<ActionIO>(sql, parameters).Count > 0;
        }
    }
    
    public enum TypeAction
    {
        io,
        servo,
    }
}
