﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC_M4.Modules
{
    internal class Setting
    {
        public int id { get; set; }
        public string path_image { get; set; }
        public int _type { get; set; }              // 0 = cam1, 1=cam2 ,2= cam2QR

        public int percent { get; set; }
        public int state { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set;}

        public void Save()
        {
            string sql = "insert into settings (path_image, _type, percent, state, created_at, updated_at) values (@path_image, @_type, @percent, @state, @created_at, @updated_at)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@path_image", path_image);
            parameters.Add("@_type", _type);
            parameters.Add("@percent", percent);
            parameters.Add("@state", state);
            parameters.Add("@created_at", SQliteDataAccess.GetDateTimeNow());
            parameters.Add("@updated_at", SQliteDataAccess.GetDateTimeNow());
            SQliteDataAccess.Execute(sql, parameters);
        }

        public void Update()
        {
            string sql = "update settings set path_image = @path_image, _type = @_type, percent = @percent, state = @state, updated_at = @updated_at where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@path_image", path_image);
            parameters.Add("@_type", _type);
            parameters.Add("@percent", percent);
            parameters.Add("@state", state);
            parameters.Add("@updated_at", SQliteDataAccess.GetDateTimeNow());
            parameters.Add("@id", id);
            SQliteDataAccess.Execute(sql, parameters);
        }

        public void Delete(){
            string sql = "delete from settings where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            SQliteDataAccess.Execute(sql, parameters);
        }

        public static void RemoveBinding(int id)
        {
            string sql = "update settings set state = 0 where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            SQliteDataAccess.Execute(sql, parameters);
        }

        public static List<Setting> GetSetting(int _type) => SQliteDataAccess.GetRow<Setting>("select * from settings where _type = " + _type+" order by id desc limit 1");

        public static List<Setting> GetSettingID(int id) => SQliteDataAccess.GetRow<Setting>("select * from settings where id = " + id + " ");

        public static List<Setting> GetSettingRemove() => SQliteDataAccess.GetRow<Setting>("select * from settings where state = 0 order by id desc");
        
        public static List<Setting> GetListImage(int _type) => SQliteDataAccess.GetRow<Setting>("select * from settings where _type = " + _type + " and state = 1 order by id desc");

    }
}
