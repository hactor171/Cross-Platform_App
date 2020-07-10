using System;
namespace CrossProjectApp.Models
{
    public class TaskDataModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int done { get; set; }
        public string date { get; set; }
        public string icon { get; set; }
        public string priority { get; set; }
    }

    public class ListDataModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string date { get; set; }
    }
}
