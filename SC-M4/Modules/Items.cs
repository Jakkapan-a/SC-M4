using System.Collections.Generic;

namespace SC_M4.Modules
{
    public class Items
    {
        public int id { get; set; }
        public int model_id { get; set; }
        public string name { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

        public static void CreateTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS `items` (" +
                "`id`\tINTEGER NOT NULL," +
                "`model_id`\tINTEGER NOT NULL," +
                "`name`\tTEXT," +
                "`created_at`\tTEXT," +
                "`updated_at`\tTEXT," +
                " PRIMARY KEY(`id` AUTOINCREMENT)" +
                ");";
            SQliteDataAccess.Execute(sql, null);
        }

        private Dictionary<string, object> CreateParameters()
        {
            return new Dictionary<string, object>
            {
                { "@id", id },
                { "@model_id", model_id },
                { "@name", name },
                { "@created_at", SQliteDataAccess.GetDateTimeNow() },
                { "@updated_at", SQliteDataAccess.GetDateTimeNow() }
            };
        }

        public void Save()
        {
            string sql = "insert into items (model_id,name,created_at,updated_at) values (@model_id,@name,@created_at,@updated_at)";
            SQliteDataAccess.Execute(sql, CreateParameters());
        }

        public void Update()
        {
            string sql = "update items set model_id = @model_id, name = @name, updated_at = @updated_at where id = @id";
            SQliteDataAccess.Execute(sql, CreateParameters());
        }

        public void Delete()
        {
            string sql = "delete from items where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            SQliteDataAccess.Execute(sql, parameters);
        }

        public static List<Items> GetItemsByModelId(int model_id)
        {
            string sql = "select * from items where model_id = @model_id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@model_id", model_id);
            return SQliteDataAccess.Query<Items>(sql, parameters);
        }

        public static List<Items> GetItemsByModelIdAndName(int model_id, string name)
        {
            string sql = "select * from items where model_id = @model_id and name = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@model_id", model_id);
            parameters.Add("@name", name);
            return SQliteDataAccess.Query<Items>(sql, parameters);
        }

        public static List<Items> GetItemsByModelIdAndNameAndId(string name)
        {
            string sql = "select * from items where name = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", name);
            return SQliteDataAccess.Query<Items>(sql, parameters);
        }
        public bool IsExist(string name)
        {
            string sql = "select * from items where name = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", name);
            List<Items> items = SQliteDataAccess.Query<Items>(sql, parameters);
            return items.Count > 0;
        }
    }
}
