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

namespace OwnRadio
{
	class SQLite_Android : ISQLite
	{
		public SQLite_Android() { }

		[Table("track")]
		public class track
		{
			[PrimaryKey, Column("id"), NotNull, Unique]
			public string id { get; set; }
			public string datetimelastlisten { get; set; }
		}

		[Table("history")]
		public class history
		{
			[PrimaryKey, Column("id"), NotNull, AutoIncrement]
			public string id { get; set; } 
			public string trackid { get; set; }
			public string datetimelisten { get; set; }
			public int listenyesno { get; set; }
		}

		public SQLiteConnection GetConnection(string sqliteFilename)
		{
			string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var path = Path.Combine(dbPath, sqliteFilename);
			// создаем подключение
			var db = new SQLiteConnection(path);
			return db;
		}

		public void CreateDB(SQLiteConnection db)
		{
			// string sql = "create table User (ID int, Name varchar(20))";
			// conn.CreateCommand();
			//SQLiteCommand command = new SQLiteCommand(sql, conn);
		}


		public void CreateTableTrack(String ID, String DateTimeListen)
		{
			var db = GetConnection("ownradio.db");
			var toTrack = new track { id = ID, datetimelastlisten = DateTimeListen };
			db.CreateTable<track>();
		}

		public void CreateTableHistory(String ID, String TrackID, DateTime DateTimeListen, int ListenYesNo)
		{
			var db = GetConnection("ownradio.db");
			var toHistory = new history { id = ID, trackid = TrackID, datetimelisten = DateTimeListen.ToString(), listenyesno = ListenYesNo };
			db.CreateTable<history>();
		}
		public void ExecuteQuery(String txtQuery)
		{
			var db = GetConnection("ownradio.db");
			//var sqlCommand = db.CreateCommand(txtQuery);
			//sqlCommand.ExecuteNonQuery();
			
			db.Close();
		}

		public void AddStatusToDB(String ID, String TrackID, int ListenYesNo, DateTime DateTimeListen)
		{
			string txtSQLQuery = "INSERT INTO history (id, trackid, datetimelisten, listenyesno) VALUES ('" + ID + "," + TrackID + "," + DateTimeListen.ToString() + "," + ListenYesNo + "')";
			ExecuteQuery(txtSQLQuery);
		}

		public void AddTrackToDB(String TrackID, DateTime DateTimeListen)
		{
			string txtSQLQuery = "INSERT INTO track (id, datetimelisten) VALUES ('" + TrackID + "," + DateTimeListen.ToString() + "')";
			ExecuteQuery(txtSQLQuery);
		}

		public void UpdateHistory(String TrackID, String DateTimeListen, int ListenYesNo)
		{
			string txtSQLQuery = "UPDATE history SET datetimelisten = '" + DateTimeListen.ToString() + "', listenyesno = '" + ListenYesNo + "' WHERE id = '" + TrackID + "'";
			ExecuteQuery(txtSQLQuery);
		}

		public String ShowTable(String tableName)
		{
			string txtSQLQuery = "SELECT * FROM " + tableName;
			
			//ExecuteQuery(txtSQLQuery);
			var db = GetConnection("ownradio.db");
			var sqlCommand = db.CreateCommand(txtSQLQuery);
			
			sqlCommand.ExecuteNonQuery();
			db.Close();
			return sqlCommand.ToString();
		}
	}
}