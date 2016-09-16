using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using SQLite;

namespace ownradio
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection(string filename);
		void CreateTables();
		void ExecuteQuery(string txtQuery);
		void AddStatusToDB(String DeviceID, String TrackID, bool ListenYesNo, DateTime DateTimeListen);
		void AddTrackToDB(String TrackID, DateTime DateTimeListen);
		void UpdateHistory(String TrackID, String DateTimeListen, bool ListenYesNo);
		String ShowTable(String tableName);
	}
}