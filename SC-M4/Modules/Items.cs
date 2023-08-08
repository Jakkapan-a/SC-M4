using System.Collections.Generic;
using System.Linq;

namespace SC_M4.Modules
{
    public class Items
    {
        public int id { get; set; }
        public int model_id { get; set; }
        public string name { get; set; }
        public int index { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

        public static void CreateTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS `items` (" +
                "`id`\tINTEGER NOT NULL," +
                "`model_id`\tINTEGER NOT NULL," +
                "`name`\tTEXT," +
                "`index`\tINTEGER NOT NULL," +
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
                { "@index", index },
                { "@created_at", SQliteDataAccess.GetDateTimeNow() },
                { "@updated_at", SQliteDataAccess.GetDateTimeNow() }
            };
        }

        public void Save()
        {
            string sql = "insert into items (model_id,name,index,created_at,updated_at) values (@model_id,@name,@index,@created_at,@updated_at)";
            this.index = GetLastIndex() + 1;
            SQliteDataAccess.Execute(sql, CreateParameters());
        }

        public void Update()
        {
            string sql = "update items set model_id = @model_id, name = @name, index = @index, updated_at = @updated_at where id = @id";
            SQliteDataAccess.Execute(sql, CreateParameters());
        }

        public void Delete()
        {
            string sql = "delete from items where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            SQliteDataAccess.Execute(sql, parameters);
        }

        public static List<Items> Get()
        {
            string sql = "select * from items order by `index` asc";
            return SQliteDataAccess.Query<Items>(sql, null);
        }
        public static List<Items> GetItemsByModelId(int model_id)
        {
            string sql = "select * from items where model_id = @model_id order by `index` asc";
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

        public static int GetLastIndex()
        {
            string sql = "select max(`index`) as `index` from items";
            var result = SQliteDataAccess.Query<int>(sql, null).FirstOrDefault();
            return result == null ? 0 : result;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
        }
        public static Items Get(int id)
        {
            string sql = "select * from items where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            return SQliteDataAccess.Query<Items>(sql, parameters).FirstOrDefault();
        }

        public static Items GetUp(int id)
        {
            string sql = "select * from items where `index` < (select `index` from items where id = @id) and `model_id` = (select `model_id` from items where id = @id) order by `index` desc limit 1";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            return SQliteDataAccess.Query<Items>(sql, parameters).FirstOrDefault();
        }

        public static Items GetDown(int id)
        {
            string sql = "select * from items where `index` > (select `index` from items where id = @id) and `model_id` = (select `model_id` from items where id = @id) order by `index` asc limit 1";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            return SQliteDataAccess.Query<Items>(sql, parameters).FirstOrDefault();
        }

        public static bool SetUp(int id)
        {
            Items item = Items.Get(id);
            Items itemUp = Items.GetUp(id);
            if (itemUp != null)
            {
                int index = item.index;
                item.index = itemUp.index;
                itemUp.index = index;
                item.Update();
                itemUp.Update();
                return true;
            }
            return false;
        }

        public static bool SetDown(int id)
        {
            Items item = Items.Get(id);
            Items itemDown = Items.GetDown(id);
            if (itemDown != null)
            {
                int index = item.index;
                item.index = itemDown.index;
                itemDown.index = index;
                item.Update();
                itemDown.Update();
                return true;
            }
            return false;
        }


    }
}
