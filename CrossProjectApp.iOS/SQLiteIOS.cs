using System;
using Xamarin.Forms;
using System.IO;
using CrossProjectApp.iOS;

[assembly: Dependency(typeof(SQLiteIOS))]

namespace CrossProjectApp.iOS
{
    public class SQLiteIOS: SqliteInterface
    {
        public SQLiteIOS(){}

        public string GetDatabasePath(string sqliteFilename)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, sqliteFilename);

            return path;
        }
    }
}
