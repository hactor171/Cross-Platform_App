using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace CrossProjectApp.Models
{
    [Table("tasklist")]
    public class tasklist
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int id { get; set; }

        public string name { get; set; }
        public int done { get; set; }
        public int user_id { get; set; }
        public string createtime { get; set; }
        public string optype { get; set; }
        public int status { get; set; }
        public int counter { get; set; }
    }

    [Table("tasks")]
    public class tasks
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int id { get; set; }

        public string name { get; set; }
        public int done { get; set; }
        public int tasklist_id { get; set; }
        public int user_id { get; set; }
        public string date { get; set; }
        public string createtime { get; set; }
        public string priority { get; set; }
        public string optype { get; set; }
        public int status { get; set; }
    }

    [Table("tmpid")]
    public class tmpid
    { 
        public int id { get; set; }
        public int checklist { get; set; }
    }

    public class DatabaseHelper
    {
        SQLiteConnection database;
        public DatabaseHelper(string databasePath)
        {
            database = new SQLiteConnection(databasePath);
            database.CreateTable<tasklist>();
            database.CreateTable<tasks>();
            database.CreateTable<tmpid>();
        }

        //tmpid
        public IEnumerable<tmpid> gettmpList()
        {
            return database.Query<tmpid>("Select * From [tmpid]");
        }

        public int addtmpId(tmpid item)
        {
            return database.Insert(item);
        }

        public int droptmpId(int id)
        {
            return database.Execute("DELETE FROM [tmpid] WHERE id=" + id);
        }
        //tmpid

        //TaskList
        public int addTaskList(tasklist item)
        {
            return database.Insert(item);
        }

        public IEnumerable<tasklist> getTaskLists()
        {
            return database.Table<tasklist>().ToList();
        }


        public IEnumerable<tasklist> getUnsyncedLists()
        {
            return database.Query<tasklist>("Select * From [tasklist] WHERE status = 0");
        }

        
        public int droptTasks(int tasklist_id)
        {
            return database.Execute("DELETE FROM [tasks] WHERE tasklist_id=" + tasklist_id);
        }

        public int updateTaskListStatus(int id, int status)
        {
            var item = database.Query<tasklist>("Select * From [tasklist] WHERE _id =" + id).FirstOrDefault();
            if (item != null)
            {
                item.status = status;
            }
            return database.Update(item);
        }

        public int updateTaskListName(int id, string name)
        {
            var item = database.Query<tasklist>("Select * From [tasklist] WHERE _id =" + id).FirstOrDefault();
            if (item != null)
            {
                item.name = name;
            }
            return database.Update(item);
        }

        public int getLastTaskListid()
        {
            var item = database.Query<tasklist>("SELECT _id FROM [tasklist] WHERE _id = (SELECT MIN(_id) FROM [tasklist] WHERE status = 0)");
            Console.WriteLine(item.First().id);
            return item.First().id;
        }

        public int deleteTaskList(int id)
        {
            return database.Delete<tasklist>(id);
        }

        //Tasklist

        //Tasks
        public int addTask(tasks item)
        {
            return database.Insert(item);
        }

        public IEnumerable<tasks> getTasks(int tasklist_id)
        {
            return database.Query<tasks>("Select * From [tasks] WHERE tasklist_id =" + tasklist_id);
        }

        public IEnumerable<tasks> getAllTasks()
        {
            return database.Query<tasks>("Select * From [tasks]");
        }

        public IEnumerable<tasks> getAllIncompleteTasks()
        {
            return database.Query<tasks>("Select * From [tasks] WHERE done = 0");
        }

        public IEnumerable<tasks> getUnsyncedTasks()
        {
            return database.Query<tasks>("Select * From [tasks] WHERE status = 0");
        }


        public int updateTaskStatus(int id, int status)
        {
            var item = database.Query<tasks>("Select * From [tasks] WHERE _id =" + id).FirstOrDefault();
            if (item != null)
            {
                item.status = status;
            }
            return database.Update(item);
        }

        public int updateTask(int id, string name, string date, string priority)
        {
            var item = database.Query<tasks>("Select * From [tasks] WHERE _id =" + id).FirstOrDefault();
            if (item != null)
            {
                item.name = name;
                item.date = date;
                item.priority = priority;
            }
            return database.Update(item);
        }

        public int TaskCompleteness(int id, int done, string optype, int status)
        {
            var item = database.Query<tasks>("Select * From [tasks] WHERE _id =" + id).FirstOrDefault();
            if (item != null)
            {
                item.done = done;
                item.optype = optype;
                item.status = status;
            }
            return database.Update(item);
        }


        public int getLastTaskid()
        {
            var item = database.Query<tasks>("SELECT _id FROM [tasks] WHERE _id = (SELECT MIN(_id) FROM [tasks] WHERE status = 0)");
            Console.WriteLine(item.First().id);
            return item.First().id;
        }

        public int deleteTask(int id)
        {
            return database.Delete<tasks>(id);
        }
        //Tasks
    }

}
