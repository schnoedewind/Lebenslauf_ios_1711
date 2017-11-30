using System.IO;
using Xamarin.Forms;
using Lebenslauf.IOS;
using SQLite;

[assembly: Dependency(typeof(SQLiteClientIOS))]
namespace Lebenslauf.IOS
{
    public class SQLiteClientIOS : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "DatabaseCV.db3";
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            var path = Path.Combine(documentsPath, sqliteFilename);

            var conn = new SQLite.SQLiteConnection(path);

            return conn;
        }
    }
}