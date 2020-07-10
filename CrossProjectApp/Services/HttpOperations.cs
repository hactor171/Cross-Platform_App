using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using CrossProjectApp.Models;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace CrossProjectApp.Services
{
    public class HttpOperations
    {

        public HttpOperations()
        {

        }

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(5);
            return client;
        }

        public async Task<RootObject> GetTaskLists()
        {
            HttpClient client = GetClient();
           
            try
            {
                HttpResponseMessage response = await client.GetAsync(Api.URL_GET_TASKS_LISTS);
                HttpContent responseContent = response.Content;
                return JsonConvert.DeserializeObject<RootObject>(await responseContent.ReadAsStringAsync());
            }
            catch (Exception)
            {
                DependencyService.Get<IMessage>()
                    .Show("Sorry, servers are current unreachable or your Internet connction is loss");
                return null;
            }

        }

        public async Task<RootObjectTasks> GetTasks()
        {
            HttpClient client = GetClient();


            try
            {
                HttpResponseMessage response = await client.GetAsync(Api.URL_GET_TASKS+"1");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("OK");
                }
                HttpContent responseContent = response.Content;
                Console.WriteLine(response);
                return JsonConvert.DeserializeObject<RootObjectTasks>(await responseContent.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                DependencyService.Get<IMessage>().Show("Sorry, servers are current unreachable or your Internet connction is loss");
                return null;
            }

        }

        public async Task<Response> PostOperation(String url, Dictionary<String, String> parameters, string optype)
        {
            HttpClient client = GetClient();

            try
            {
                var content = new FormUrlEncodedContent(parameters);
                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    if (optype.Equals("InsTask"))
                    {
                        int id = App.Database.getLastTaskid();
                        App.Database.updateTaskStatus(id, 1);
                    }
                    else if (optype.Equals("InsTaskList"))
                    {
                        int id = App.Database.getLastTaskListid();
                        App.Database.updateTaskListStatus(id, 1);
                    }
                    else if (optype.Equals("UpdateTaskList"))
                    {
                        int id = App.Database.getLastTaskListid();
                        App.Database.updateTaskListStatus(id, 1);
                    }
                    else if (optype.Equals("ServerUpdatesList"))
                    {
                        int id = App.Database.getLastTaskListid();
                        App.Database.updateTaskListStatus(id, 1);
                    }
                    else if (optype.Equals("ServerUpdatesTask"))
                    {
                        int id = App.Database.getLastTaskid();
                        App.Database.updateTaskStatus(id, 1);
                    }
                    else if (optype.Equals("UpdatetTaskcCompletioStatus"))
                    {
                        int id = int.Parse(parameters["id"]);
                        App.Database.updateTaskStatus(id, 1);
                    }
                    else if (optype.Equals("DeleteTask"))
                    {
                        int id = int.Parse(parameters["id"]);
                        App.Database.droptmpId(id);
                    }
                    else if (optype.Equals("DeleteTaskList"))
                    {
                        int id = int.Parse(parameters["id"]);
                        App.Database.droptmpId(id);
                    }

                }
                HttpContent responseContent = response.Content;
                Console.WriteLine(response);
                return JsonConvert.DeserializeObject<Response>(await responseContent.ReadAsStringAsync());
            }
            catch (Exception)
            {
                DependencyService.Get<IMessage>().Show("Sorry, servers are current unreachable or your Internet connection is too low. Please,try again");
                return null;
            }
        }
    }

}
