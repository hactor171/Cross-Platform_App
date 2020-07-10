using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
using CrossProjectApp.Models;
using CrossProjectApp.Services;
using CrossProjectApp.Views;
using Plugin.Connectivity;
using CrossProjectApp.Tests;

namespace CrossProjectApp.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private bool isBusy;
        private string _conn;

        public string NameOfNewTaskList { get; set; }
        public string NameOfNewTask { get; set; }
        public string TaskDueDate { get; set; }
        public string TaskPriority { get; set; }
        public int CheckUpdate { get; set; }
        public int TaskInxdex { get; set; }
        public int TaskID { get; set; }
        public int TaskCompletned { get; set; }
        public int TaskListId { get; set; }
        tasklist tmplist;

        public int AllTasksCounter { get; set; }
        public int IncompleteTasksCounter { get; set; }

        public ObservableCollection<tasklist> tasklists { get; set; }
        public ObservableCollection<TaskDataModel> tasks_list { get; set; }
        public ObservableCollection<ListDataModel> lists { get; set; }
        HttpOperations httpService = new HttpOperations();
        public event PropertyChangedEventHandler PropertyChanged;
        public DatabaseTest tests = new DatabaseTest();


        public ICommand BackCommand { protected set; get; }
        public ICommand ImageClicked { protected set; get; }
        public ICommand DeleteTaskClicked { protected set; get; }
        public ICommand EditTaskClicked { protected set; get; }
        public ICommand DeleteListClicked { protected set; get; }
        public ICommand EditListClicked { protected set; get; }
        public ICommand OpenNewTaskPage { protected set; get; }
        public ICommand AddTaskList { protected set; get; }
        public ICommand CreateTaskCommand { protected set; get; }
        

        public INavigation Navigation { get; set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
                OnPropertyChanged("IsLoaded");
            }
        }
        public string Conn
        {
            get => _conn;
            set
            {
                _conn = value;
            }
        }


        public bool IsLoaded
        {
            get { return !isBusy; }
        }

        public ApplicationViewModel()
        {
            tasklists = new ObservableCollection<tasklist>();
            tasks_list = new ObservableCollection<TaskDataModel>();
            lists = new ObservableCollection<ListDataModel>();
            IsBusy = false;
            AddTaskList = new Command(CreateTaskList);
            ImageClicked = new Command(OnImageButtonClickedAsync);
            OpenNewTaskPage = new Command(NewTaskPage);
            CreateTaskCommand = new Command(CreateTask);
            DeleteListClicked = new Command(DeleteList);
            EditListClicked = new Command(UpdateList);
            EditTaskClicked = new Command(EditTask);
            DeleteTaskClicked = new Command(DeleteTask);
            BackCommand = new Command(Back);
            TaskDueDate = DateTime.Now.ToString(format: "MM/dd/yyyy");
            CheckWifiOnStart();
            CheckWifiContinuously();

            AllTasksCounter = App.Database.getAllTasks().Count();
            IncompleteTasksCounter = App.Database.getAllIncompleteTasks().Count();


        }

        public void CheckWifiOnStart()
        {
            bool conn = CrossConnectivity.Current.IsConnected;
        }

        public void CheckWifiContinuously()
        {
            CrossConnectivity.Current.ConnectivityChanged += async (sender, args) =>
            {
                if (args.IsConnected)
                {
                    IEnumerable<tasklist> tmp = App.Database.getUnsyncedLists();

                    foreach (tasklist tl in tmp)
                    {
                        //tasklists.Add(tl);
                        var parameters = new Dictionary<string, string> { };
                        parameters.Add("id", tl.id.ToString());
                        parameters.Add("name", tl.name);
                        parameters.Add("done", tl.done.ToString());
                        parameters.Add("user_id", tl.user_id.ToString());
                        parameters.Add("createtime", tl.createtime);

                        try
                        {
                            _ = await httpService.PostOperation(Api.URL_SERVER_LIST_UPDATES, parameters, "ServerUpdatesList");
                        }
                        catch (NullReferenceException)
                        {
                        }
                    }
                    DependencyService.Get<IMessage>().Show("OK");

                    IEnumerable<tasks> tmpTask = App.Database.getUnsyncedTasks();

                    foreach (tasks tl in tmpTask)
                    {
                        //tasklists.Add(tl);
                        var parameters = new Dictionary<string, string> { };
                        parameters.Add("id", tl.id.ToString());
                        parameters.Add("name", tl.name);
                        parameters.Add("done", tl.done.ToString());
                        parameters.Add("tasklist_id", tl.tasklist_id.ToString());
                        parameters.Add("user_id", tl.user_id.ToString());
                        parameters.Add("date", tl.date);
                        parameters.Add("priority", tl.priority);
                        parameters.Add("createtime", tl.createtime);

                        try
                        {
                            _ = await httpService.PostOperation(Api.URL_SERVER_TASK_UPDATES, parameters, "ServerUpdatesTask");

                        }
                        catch (NullReferenceException)
                        {
                        }
                    }

                    IEnumerable<tmpid> deletedTasksAndLists = App.Database.gettmpList();

                    foreach (tmpid tl in deletedTasksAndLists)
                    {
                        var parameters = new Dictionary<string, string> { };
                        parameters.Add("id", tl.id.ToString());
                        if (tl.checklist == 1)
                        {
                            try
                            {
                                _ = await httpService.PostOperation(Api.URL_DELETE_TASK, parameters, "DeleteTask");
                            }
                            catch (NullReferenceException)
                            {
                            }
                        }
                        else if (tl.checklist == 0)
                        {
                            try
                            {
                                _ = await httpService.PostOperation(Api.URL_DELETE_TASKLIST, parameters, "DeleteTaskList");
                            }
                            catch (NullReferenceException)
                            {
                            }
                        }
                    }
                }

            };
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public tasklist SelectedList
        {
            get { return tmplist; }
            set
            {
                if (tmplist != value)
                {
                    Console.WriteLine(value.id);
                    TaskListId = value.id;
                    Navigation.PushAsync(new TasksActivity(this,value.name, value.id));
                }
            }
        }

        private void NewTaskPage()
        {
            string dateTime = DateTime.Now.ToString(format: "dd/MM/yyyy");
            DateTime date = DateTime.ParseExact(dateTime, "dd/MM/yyyy", null);
            Console.WriteLine(date);
      
            Navigation.PushAsync(new NewTaskPage(this, date, "New Task"));
            CheckUpdate = 0;

    }

    private async void EditTask(object sender)
        {
            TaskDataModel tmp = sender as TaskDataModel;
            var index = tasks_list.IndexOf(tmp);
            NameOfNewTask = tmp.name;
            TaskDueDate = tmp.date;
            TaskPriority = tmp.priority;
            CheckUpdate = 1;
            TaskInxdex = index;
            TaskID = tmp.id;
            TaskCompletned = tmp.done;
            await Navigation.PushAsync(new NewTaskPage(this, DateTime.Now, "Update Task"));
            
        }

        private async void DeleteTask(object sender)
        {
            TaskDataModel tmp = sender as TaskDataModel;
            var index = tasks_list.IndexOf(tmp);
            tasks_list.RemoveAt(index);
            tmpid item = new tmpid
            {
                id = tmp.id,
                checklist = 1
            };
            App.Database.addtmpId(item);
            App.Database.deleteTask(tmp.id);
            try
            {
                var parameters = new Dictionary<string, string> { };
                parameters.Add("id", tmp.id.ToString());
                var op = await httpService.PostOperation(Api.URL_DELETE_TASK, parameters, "DeleteTask");

            }
            catch (NullReferenceException)
            {
            }
        }

        private async void UpdateList(object sender)
        {
            tasklist tmp = sender as tasklist;
            var index = tasklists.IndexOf(tmp);
            
            string newlistname = await App.Current.MainPage.DisplayPromptAsync("Enter list name", "", "OK", "Cancel", null, -1, null, tmp.name);
            if ((newlistname == null) || (newlistname == tmp.name))
            {
            }
            else
            {
                Console.WriteLine("Zmienione");
                tmp.name = newlistname;
                tasklists[index] = tmp;
                App.Database.updateTaskListName(tmp.id, newlistname);
                try
                {
                    var parameters = new Dictionary<string, string> { };
                    parameters.Add("id", tmp.id.ToString());
                    parameters.Add("name", newlistname);
                    var op = await httpService.PostOperation(Api.URL_UPDATE_TASKLIST, parameters, "UpdateTaskList");
                }
                catch (NullReferenceException)
                {
                }
            }
     
        }

        private async void DeleteList(object sender)
        {
            tasklist tmp = sender as tasklist;
            var index = tasklists.IndexOf(tmp);
            tasklists.RemoveAt(index);
            tmpid item = new tmpid
            {
                id = tmp.id,
                checklist = 0
            };
            App.Database.addtmpId(item);
            App.Database.droptTasks(tmp.id);
            App.Database.deleteTaskList(tmp.id);
            try
            {
                var parameters = new Dictionary<string, string> { };
                parameters.Add("id", tmp.id.ToString());
                var op = await httpService.PostOperation(Api.URL_DELETE_TASKLIST, parameters, "DeleteTaskList");

            }
            catch (NullReferenceException)
            {
            }
            DependencyService.Get<IMessage>().Show("Delete");
        }

        private async void OnImageButtonClickedAsync(object sender)
        {
            TaskDataModel tmp = sender as TaskDataModel;
            var index = tasks_list.IndexOf(tmp);
            if (tmp.done == 0)
            {
                tmp.done = 1;
                tmp.icon = "check_icon.png";
                tasks_list[index] = tmp;
                App.Database.TaskCompleteness(tmp.id, 1, "UpdatetTaskcCompletioStatus", 0);
                try
                {
                    var parameters = new Dictionary<string, string> { };
                    parameters.Add("id", tmp.id.ToString());
                    parameters.Add("done", "1");
                    _ = await httpService.PostOperation(Api.URL_UPDATEST_TASK, parameters, "UpdatetTaskcCompletioStatus");

                }
                catch (NullReferenceException)
                {
                }
                
            }
            else
            {
                tmp.done = 0;
                tmp.icon = "unchecked_icon.png";
                tasks_list[index] = tmp;
                App.Database.TaskCompleteness(tmp.id,0, "UpdatetTaskcCompletioStatus", 0);
                try
                {
                    var parameters = new Dictionary<string, string> { };
                    parameters.Add("id", tmp.id.ToString());
                    parameters.Add("done", "0");
                    _ = await httpService.PostOperation(Api.URL_UPDATEST_TASK, parameters, "UpdatetTaskcCompletioStatus");

                }
                catch (NullReferenceException)
                {
                }

            }
            
        }

        private void Back()
        {
            Navigation.PopAsync();
        }

        public async void CreateTask()
        {

            if (TaskPriority != null && NameOfNewTask != null)
            {
                IsBusy = true;
                if (CheckUpdate == 0)
                {
                    var parameters = new Dictionary<string, string> { };
                    parameters.Add("name", NameOfNewTask);
                    parameters.Add("done", "0");
                    parameters.Add("tasklist_id", TaskListId.ToString());
                    parameters.Add("user_id", "1");
                    parameters.Add("date", TaskDueDate.Substring(0, 10));
                    parameters.Add("createtime", (DateTime.Now).ToString());
                    parameters.Add("priority", TaskPriority);
                    tasks item = new tasks
                    {
                        name = NameOfNewTask,
                        done = 0,
                        tasklist_id = TaskListId,
                        user_id = 1,
                        date = TaskDueDate,
                        createtime = (DateTime.Now).ToString(),
                        priority = TaskPriority,
                        optype = "InsTask",
                        status = 0
                    };
                    App.Database.addTask(item);
                    GetTaskLists();
                    await Navigation.PopAsync();
                    try
                    {
                        var tmp = await httpService.PostOperation(Api.URL_POST_TASK, parameters, "InsTask");
                        DependencyService.Get<IMessage>().Show(tmp.message);
                    }
                    catch (NullReferenceException)
                    {
                    }
                }
                else if(CheckUpdate == 1)
                {
                    var parameters = new Dictionary<string, string> { };
                    parameters.Add("id", TaskID.ToString());
                    parameters.Add("name", NameOfNewTask);
                    parameters.Add("done", TaskCompletned.ToString());
                    parameters.Add("tasklist_id", TaskListId.ToString());
                    parameters.Add("user_id", "1");
                    parameters.Add("date", TaskDueDate.Substring(0, 10));
                    parameters.Add("createtime", (DateTime.Now).ToString());
                    parameters.Add("priority", TaskPriority);

                    App.Database.updateTask(TaskID, NameOfNewTask, TaskDueDate.Substring(0, 10), TaskPriority);
                    GetTaskLists();
                    await Navigation.PopAsync();
                    try
                    {
                        var tmp = await httpService.PostOperation(Api.URL_SERVER_TASK_UPDATES, parameters, "ServerUpdatesTask");
                    }
                    catch (NullReferenceException)
                    {
                    }
                }

                IsBusy = false;
                
            }
            else
            {
                DependencyService.Get<IMessage>().Show("Some value is empty");
                return;
            }

        }

        public async void CreateTaskList()
        {
            string listname = await App.Current.MainPage.DisplayPromptAsync("Enter list name", "");
            if ((listname == "") || (listname == null))
            {
                Console.WriteLine("Empty");
            }
            else
            {
                Console.WriteLine(listname);
                IsBusy = true;
                var parameters = new Dictionary<string, string> { };
                parameters.Add("name", listname);
                parameters.Add("done", "0");
                parameters.Add("user_id", "1");
                parameters.Add("createtime", (DateTime.Now).ToString());
                //InsTaskList
                tasklist item = new tasklist
                {
                    name = listname,
                    done = 0,
                    user_id = 1,
                    createtime = (DateTime.UtcNow.Date).ToString().Substring(0, 10),
                    optype = "InsTaskList",
                    status = 0
                };
                App.Database.addTaskList(item);
                tasklists.Insert(0, item);

                IsBusy = false;
            }

        }

        public void GetTasks(int tasklist_id)
        {
            IsBusy = true;

            while (tasks_list.Any())
            {
               tasks_list.RemoveAt(tasks_list.Count - 1);
            }

            IEnumerable<tasks> tmp = App.Database.getTasks(tasklist_id);
 
            foreach (tasks tl in tmp)
            {
                var icon = "";
                if (tl.done == 0)
                {
                    icon = "unchecked_icon.png";
                }
                else
                {
                    icon = "check_icon.png";
                }
                TaskDataModel item = new TaskDataModel
                {
                    id = tl.id,
                    name = tl.name,
                    done = tl.done,
                    date = tl.date.Substring(0, 10),
                    priority = tl.priority,
                    icon = icon
                };
                tasks_list.Add(item);
            }

            IsBusy = false;
        }

        public void GetTaskLists()
        {
            IsBusy = true;

           while (tasklists.Any())
             {
                tasklists.RemoveAt(tasklists.Count - 1);
             }

            IEnumerable<tasklist> tmp = App.Database.getTaskLists();

            foreach (tasklist tl in tmp)
            {
                tl.counter = TaskCounter(tl.id);
                tasklists.Add(tl);
                
            }
        
            IsBusy = false;
        }

        public void refreshFrame()
        { 
            AllTasksCounter = App.Database.getAllTasks().Count();
            IncompleteTasksCounter = App.Database.getAllIncompleteTasks().Count();
            OnPropertyChanged("AllTasksCounter");
            OnPropertyChanged("IncompleteTasksCounter");
        }
        public int TaskCounter(int tasklist_id)
        {
            int count = App.Database.getTasks(tasklist_id).Count();
            return count;
        }

    }
}
