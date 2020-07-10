using System;
using System.Collections.Generic;
using System.Linq;
using CrossProjectApp.ViewModels;
using Xamarin.Forms;

namespace CrossProjectApp.Views
{
    public partial class TasksActivity : ContentPage
    {

        public ApplicationViewModel ViewModel { get; private set; }
        public int Tasklist_id; 

        public TasksActivity(ApplicationViewModel viewModel, String title, int tasklist_id)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.BindingContext = this;
            Tasklist_id = tasklist_id;
            this.Title = title;
        }

        protected override void OnAppearing()
        {

            ViewModel.GetTasks(Tasklist_id);

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            var listview = ViewModel.tasks_list;
            while (listview.Any())
            {
                listview.RemoveAt(listview.Count - 1);
            }
            base.OnDisappearing();
        }


    }
}
