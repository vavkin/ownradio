using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
//using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.IO;
using SQLite;

namespace ownradio
{
	class SQLite_Android : ISQLite
	{
		public SQLite_Android() { }
		public SQLiteConnection GetConnection(string sqliteFilename)
		{
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var path = Path.Combine(documentsPath, sqliteFilename);
			// создаем подключение
			var conn = new SQLiteConnection(path);
			return conn;
		}

        public void CreateDB(SQLiteConnection conn)
        {
           // string sql = "create table User (ID int, Name varchar(20))";
           // conn.CreateCommand();
            //SQLiteCommand command = new SQLiteCommand(sql, conn);
        }
	}
}