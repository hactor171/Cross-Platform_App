using System;
namespace CrossProjectApp
{
    public interface SqliteInterface
    {
        string GetDatabasePath(string filename);
    }
}
