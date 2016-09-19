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

namespace OwnRadio
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection(String filename);
		void CreateTableTrack(String ID, String DateTimeListen);
		void CreateTableHistory(String ID, String TrackID, DateTime DateTimeListen, int ListenYesNo);
		void ExecuteQuery(String txtQuery);
		void AddStatusToDB(String DeviceID, String TrackID, int ListenYesNo, DateTime DateTimeListen);
		void AddTrackToDB(String TrackID, DateTime DateTimeListen);
		void UpdateHistory(String TrackID, String DateTimeListen, int ListenYesNo);
		String ShowTable(String tableName);
	}
}