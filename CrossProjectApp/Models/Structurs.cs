using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CrossProjectApp.Models
{
    public class TaskListStruct
    {
        public int id { get; set; }
        public string name { get; set; }
        public string user_id { get; set; }
        public string createtime { get; set; }

        public static implicit operator ObservableCollection<object>(TaskListStruct v)
        {
            throw new NotImplementedException();
        }
    }

    public class RootObject
    {
        public string error { get; set; }
        public string message { get; set; }
        public List<TaskListStruct> heroes { get; set; }
    }

    public class TasksStruct
    {
        public int id { get; set; }
        public string name { get; set; }
        public int done { get; set; }
        public int tasklist_id { get; set; }
        public int user_id { get; set; }
        public string date { get; set; }
        public string priority { get; set; }
        public string createtime { get; set; }

        public static implicit operator ObservableCollection<object>(TasksStruct v)
        {
            throw new NotImplementedException();
        }
    }

    public class RootObjectTasks
    {
        public string error { get; set; }
        public string message { get; set; }
        public List<TasksStruct> heroes { get; set; }
    }

    public class Response
    {
        public string error { get; set; }
        public string message { get; set; }
        public string heroes { get; set; }
    }
}