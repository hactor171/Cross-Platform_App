using System;
using CrossProjectApp.Droid;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteAndroid))]
namespace CrossProjectApp.Droid
{
    public class SQLiteAndroid:SqliteInterface
    {
        public SQLiteAndroid(){}
        public string GetDatabasePath(string sqliteFilename)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            return path;
        }
    }
}
