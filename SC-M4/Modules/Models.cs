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
        public string updated_at { get; set;}
    }

    public class Items
    {
        public int id { get; set; }
        public int model_id { get; set; }
        public string name { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }

    public class Actions
    {
        public int id { get; set; }
        public int items_id { get; set; }
        public int fileimage_id { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public string pin { get; set; }
        public int  state { get; set; }
        public int delay { get; set; }
        public int auto_delay { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }

    public class FileImage
    {
        public int id { get; set; }
        public string name { get; set; }
        public string created_at { get; set;}
        public string updated_at { get; set;}
    }

    public class Rectangles
    {
        public int id { get; set; }
        public int fileimage_id { get; set;}
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int percent { get; set; }
        public string created_at { get; set;}
        public string updated_at { get; set;}
    }
}
