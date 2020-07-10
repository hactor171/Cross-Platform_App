using System;
namespace CrossProjectApp.Models
{
    public class Api
    {
        public const string Url = "http://192.168.64.2/ConApi/v1/Api.php?apicall";

        public const string URL_POST_TASK = Url + "=inserttask";
        public const string URL_DELETE_TASK = Url + "=deletetask";
        public const string URL_UPDATEST_TASK = Url + "=updatetaskstatus";
        public const string URL_UPDATENAME_TASK = Url + "=updatetaskname";
        public const string URL_POST_TASKLIST = Url + "=create_t_list";
        public const string URL_UPDATE_TASKLIST = Url + "=update_t_list";
        public const string URL_DELETE_TASKLIST = Url + "=delete_t_list";
        public const string URL_SERVER_LIST_UPDATES = Url + "=tasklistchanges";
        public const string URL_SERVER_TASK_UPDATES = Url + "=taskchanges"; 
        public const string URL_GET_TASKS_LISTS = Url + "=get_t_list_id&user_id=";
        public const string URL_GET_TASKS = Url + "=get_tasks&tasklist_id=";

    }
}
