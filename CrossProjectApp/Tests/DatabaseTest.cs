using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using CrossProjectApp.Models;
using CrossProjectApp.Services;
using CrossProjectApp.Views;
using Xamarin.Forms;

namespace CrossProjectApp.Tests
{
    public class DatabaseTest
    {

        public ObservableCollection<tasklist> tasklists { get; set; }
        DateTime startTime;
        DateTime endTime; 
        public DatabaseTest()
        {
            tasklists = new ObservableCollection<tasklist>();
        }

        public void TestRowsUpdate()
        {
            startTime = DateTime.Now;
            Console.WriteLine("Wpis");
            Console.WriteLine(startTime);
            for (int i = 0; i < 1000; i++)
            {
                App.Database.updateTaskListName(i, "Name" + i);
            }
            endTime = DateTime.Now;

            Console.WriteLine(endTime);
            TimeSpan ts = endTime - startTime;
            Console.WriteLine(ts.TotalMilliseconds);
            Console.WriteLine("End Wpis");
        }

        public void TestGetAll()
        {
            startTime = DateTime.Now;
            Console.WriteLine("Wpis");
            Console.WriteLine(startTime);
            App.Database.getTaskLists();
            endTime = DateTime.Now;

            IEnumerable<tasklist> tmp = App.Database.getTaskLists();

            foreach (tasklist tl in tmp)
            {
                tasklists.Add(tl);
            }


            Console.WriteLine(endTime);
            TimeSpan ts = endTime - startTime;
            Console.WriteLine(ts.TotalMilliseconds);
            Console.WriteLine("End Wpis");
        }
    }
}
