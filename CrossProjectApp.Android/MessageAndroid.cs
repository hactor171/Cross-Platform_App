using System;
using Android.Widget;
using CrossProjectApp.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(MessageAndroid))]
namespace CrossProjectApp.Droid
{
    public class MessageAndroid : IMessage
    {
  
        public void Show(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }
    }
}