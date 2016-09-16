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

namespace OwnRadio
{
	class StatusTrack : ISetStatusTrack
	{
		public StatusTrack() { }

		public void SetStatusTrack(String DeviceID, String TrackID, bool ListenYesNo, DateTime DateTimeListen)
		{
			ISQLite db = new SQLite_Android();
			db.AddStatusToDB(DeviceID, TrackID, ListenYesNo, DateTimeListen);
		}
	}
}