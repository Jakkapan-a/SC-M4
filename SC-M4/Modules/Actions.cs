﻿
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
        public int threshold_percent { get; set; }
        public int delay { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

        public static void CreateTable(){
            string sql = "CREATE TABLE IF NOT EXISTS `actions` (`id` INTEGER NOT NULL, `item_id` INTEGER NOT NULL, `name` TEXT, `type` INTEGER NOT NULL, `action_io_id` INTEGER, `state` INTEGER , `auto_delay` INTEGER , `servo` INTEGER, `image_name` TEXT, `type_image` INTEGER , `threshold_percent` INTEGER, `delay` INTEGER, `created_at` TEXT, `updated_at` TEXT, PRIMARY KEY(`id` AUTOINCREMENT));";
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
                    { "@threshold_percent", threshold_percent },
                    { "@delay", delay },
                    { "@created_at", SQliteDataAccess.GetDateTimeNow() },
                    { "@updated_at", SQliteDataAccess.GetDateTimeNow() }
                };
        }

        public void Save(){
            string sql = "insert into actions (item_id,name,type,action_io_id,state,auto_delay,image_name,type_image,threshold_percent,delay,created_at,updated_at) values (@item_id,@name,@type,@action_io_id,@state,@auto_delay,@image_name,@type_image,@threshold_percent,@delay,@created_at,@updated_at)";
            SQliteDataAccess.Execute(sql, CreateParameters());
        }

        public void Update(){
            string sql = "update actions set item_id = @item_id, name = @name, type = @type, action_io_id = @action_io_id, state = @state, auto_delay = @auto_delay, image_name = @image_name, type_image = @type_image, threshold_percent = @threshold_percent, delay = @delay, updated_at = @updated_at where id = @id";
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
    }
}
