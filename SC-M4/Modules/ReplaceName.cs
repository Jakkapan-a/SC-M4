using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC_M4.Modules
{
    public class ReplaceName
    {
        public int id { get; set; }
        public string oldName { get; set; }
        public string newName { get; set; }
        public int _type { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

        public void Save()
        {
            string sql = "INSERT INTO replace_name (oldName, newName, _type, created_at, updated_at) VALUES (@oldName, @newName, @_type, @created_at, @updated_at)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@oldName", oldName);
            parameters.Add("@newName", newName);
            parameters.Add("@_type", _type);
            parameters.Add("@created_at", SQliteDataAccess.GetDateTimeNow());
            parameters.Add("@updated_at", SQliteDataAccess.GetDateTimeNow());
            SQliteDataAccess.Execute(sql, parameters);
            
        }

        public void Update()
        {
            string sql = "UPDATE replace_name SET oldName = @oldName, newName = @newName, _type = @_type, updated_at = @updated_at WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            parameters.Add("@oldName", oldName);
            parameters.Add("@newName", newName);
            parameters.Add("@_type", _type);
            parameters.Add("@updated_at", SQliteDataAccess.GetDateTimeNow());
            SQliteDataAccess.Execute(sql, parameters);
        }

        public void Delete()
        {
            string sql = "DELETE FROM replace_name WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            SQliteDataAccess.Execute(sql, parameters);
        }
        public static int isExist(string oldName)
        {
            string sql = "SELECT * FROM replace_name WHERE oldName = @oldName";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@oldName", oldName);
            return SQliteDataAccess.Query<ReplaceName>(sql, parameters).Count;
        }

        public static int isExist(string oldName,int _type)
        {
            string sql = "SELECT * FROM replace_name WHERE oldName = @oldName AND _type = @_type";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@oldName", oldName);
            parameters.Add("@_type", _type);
            return SQliteDataAccess.Query<ReplaceName>(sql, parameters).Count;
        }        

        public static List<ReplaceName> GetList()
        {
            string sql = "SELECT * FROM replace_name";
            return SQliteDataAccess.Query<ReplaceName>(sql);
        }

        public static List<ReplaceName> GetList(int _type)
        {
            string sql = "SELECT * FROM replace_name WHERE _type = @_type";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@_type", _type);
            return SQliteDataAccess.Query<ReplaceName>(sql, parameters);
        }

        public static ReplaceName Get(int id)
        {
            string sql = "SELECT * FROM replace_name WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            return SQliteDataAccess.Query<ReplaceName>(sql, parameters).FirstOrDefault();
        }

        public static ReplaceName Get(string oldName)
        {
            string sql = "SELECT * FROM replace_name WHERE oldName = @oldName";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@oldName", oldName);
            return SQliteDataAccess.Query<ReplaceName>(sql, parameters).FirstOrDefault();
        }

        public static ReplaceName Get(string oldName, int _type)
        {
            string sql = "SELECT * FROM replace_name WHERE oldName = @oldName AND _type = @_type";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@oldName", oldName);
            parameters.Add("@_type", _type);
            return SQliteDataAccess.Query<ReplaceName>(sql, parameters).FirstOrDefault();
        }

    }
}
