using System;
using System.Collections.Generic;
using CrossProjectApp.ViewModels;
using CrossProjectApp.Services;
using Xamarin.Forms;
using CrossProjectApp.Models;

namespace CrossProjectApp.Views
{
    public partial class NewTaskPage : ContentPage
    {
        public string Model { get; private set; }
        public ApplicationViewModel ViewModel { get; private set; }
        DateTime Date;
        string Priority;
        string Name;

        public NewTaskPage(ApplicationViewModel viewModel, DateTime date, string title)
        {
            InitializeComponent();
            ViewModel = viewModel;
            Date = date;
            Title = title;
            this.BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }


        void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Your choose: " + picker.Items[picker.SelectedIndex]);
            if (picker.SelectedIndex == -1)
            {
                return;
            }
        }

        void DatePicker_DateSelected(System.Object sender, Xamarin.Forms.DateChangedEventArgs e)
        {
        }
    }
}
