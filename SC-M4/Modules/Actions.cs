
using System.Collections.Generic;
using System.Linq;

namespace SC_M4.Modules
{
    public class Actions
    {
        public int id { get; set; }
        public int item_id { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public int action_io_id { get; set; }
        public int state { get; set; }
        public int auto_delay { get; set; }
        public int servo { get; set; }
        public string image_name { get; set; }
        public int type_image { get; set; }
        public int threshold { get; set; }
        public int delay { get; set; }
        public int index { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

        public static void CreateTable(){
            string sql = "CREATE TABLE IF NOT EXISTS `actions` (`id` INTEGER NOT NULL, `item_id` INTEGER NOT NULL, `name` TEXT, `type` INTEGER NOT NULL, `action_io_id` INTEGER, `state` INTEGER , `auto_delay` INTEGER , `servo` INTEGER, `image_name` TEXT, `type_image` INTEGER , `threshold` INTEGER, `delay` INTEGER,`index` INTEGER, `created_at` TEXT, `updated_at` TEXT, PRIMARY KEY(`id` AUTOINCREMENT));";
            SQliteDataAccess.Execute(sql, null);
        }

        private Dictionary<string, object> CreateParameters()
        {
            return new Dictionary<string, object>
                {
                    { "@id", id },
                    { "@item_id", item_id },
                    { "@name", name },
                    { "@type", type },
                    { "@action_io_id", action_io_id },
                    { "@state", state },
                    { "@auto_delay", auto_delay },
                    { "@image_name", image_name },
                    { "@type_image", type_image },
                    { "@threshold", threshold },
                    { "@delay", delay },
                    { "@index", index },
                    { "@created_at", SQliteDataAccess.GetDateTimeNow() },
                    { "@updated_at", SQliteDataAccess.GetDateTimeNow() }
                };
        }

        public void Save(){
            string sql = "insert into actions (item_id,name,type,action_io_id,state,auto_delay,image_name,type_image,threshold,delay,created_at,updated_at) values (@item_id,@name,@type,@action_io_id,@state,@auto_delay,@image_name,@type_image,@threshold,@delay,@created_at,@updated_at)";
            this.index = GetLastIndex() + 1;
            SQliteDataAccess.Execute(sql, CreateParameters());
        }

        public void Update(){
            string sql = "update actions set item_id = @item_id, name = @name, type = @type, action_io_id = @action_io_id, state = @state, auto_delay = @auto_delay, image_name = @image_name, type_image = @type_image, threshold = @threshold, delay = @delay, updated_at = @updated_at where id = @id";
            SQliteDataAccess.Execute(sql, CreateParameters());
        }

        public void Delete(){
            string sql = "delete from actions where id = @id";
            SQliteDataAccess.Execute(sql, CreateParameters());
        }  
         public void Delete(int id){
            string sql = "delete from actions where id = @id";
            SQliteDataAccess.Execute(sql, new Dictionary<string, object> { { "@id", id } });
            
        }

        public static List<Actions> GetList(){
            string sql = "select * from actions order by id desc";
            return SQliteDataAccess.Query<Actions>(sql, null);
        }

        public static Actions Get(int id){
            string sql = "select * from actions where id = @id";
            return SQliteDataAccess.Query<Actions>(sql, new Dictionary<string, object> { { "@id", id } }).FirstOrDefault();
        }

        public static List<Actions> GetListByItemId(int item_id){
            string sql = "select * from actions where item_id = @item_id order by id desc";
            return SQliteDataAccess.Query<Actions>(sql, new Dictionary<string, object> { { "@item_id", item_id } });
        }

        public static List<Actions> GetListByItemIdAndType(int item_id, int type){
            string sql = "select * from actions where item_id = @item_id and type = @type order by id desc";
            return SQliteDataAccess.Query<Actions>(sql, new Dictionary<string, object> { { "@item_id", item_id }, { "@type", type } });
        }

        public static int GetLastIndex(){
            try
            {
                string sql = "select max(`index`) as `index` from actions";
                return SQliteDataAccess.Query<int>(sql, null).FirstOrDefault();
            }
            catch
            {
                return 0;
            }

        }

        public static Actions GetUp(int id){
            string sql = "select * from actions where `index` < (select `index` from actions where id = @id) and item_id = (select item_id from actions where id = @id) order by `index` desc limit 1";
            return SQliteDataAccess.Query<Actions>(sql, new Dictionary<string, object> { { "@id", id } }).FirstOrDefault();
        }

        public static Actions GetDown(int id){
            string sql = "select * from actions where `index` > (select `index` from actions where id = @id) and item_id = (select item_id from actions where id = @id) order by `index` asc limit 1";
            return SQliteDataAccess.Query<Actions>(sql, new Dictionary<string, object> { { "@id", id } }).FirstOrDefault();
        }

        public static bool SetUp(int id){
            Actions action = Get(id);
            Actions action_up = GetUp(id);
            if(action_up != null){
                int index = action.index;
                action.index = action_up.index;
                action_up.index = index;
                action.Update();
                action_up.Update();

                return true;
            }
            return false;
        }

        public static bool SetDown(int id){
            Actions action = Get(id);
            Actions action_down = GetDown(id);
            if(action_down != null){
                int index = action.index;
                action.index = action_down.index;
                action_down.index = index;
                action.Update();
                action_down.Update();
                return true;
            }
            return false;
        }
    }
}
