using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CrossProjectApp.ViewModels;
using CrossProjectApp.Models;
using CrossProjectApp.Tests;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using Plugin.Connectivity;
using System.Text;

namespace CrossProjectApp.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    //[DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        ///test
        DateTime startTime;
        DateTime endTime;
        public DatabaseTest tests = new DatabaseTest();
        ///

        public object Model;
        public ApplicationViewModel viewModel;


        public MainPage()
        {
            InitializeComponent();
            viewModel = new ApplicationViewModel() { Navigation = this.Navigation };
            BindingContext = viewModel;
         
            viewModel.GetTaskLists();

        }


        protected override void OnAppearing()
        {            
            TaskListView.SelectedItem = null;
            viewModel.refreshFrame();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            viewModel.GetTaskLists();
            base.OnDisappearing();
        }

    }
}
