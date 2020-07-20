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
    }
}
