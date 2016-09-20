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

		public void SetStatusTrack(String DeviceID, String TrackID, int IsListen, DateTime DateTimeListen)
		{
			//ISQLite db = new SQLite_Android();
			//db.AddStatusToDB(DeviceID, TrackID, ListenYesNo, DateTimeListen);
			//Uri URLRequest = new Uri("http://10.10.0.45:5000/api/track/SetStatusTrack/" + DeviceID + "," + TrackID + "," + IsListen + "," + DateTimeListen.ToString()); 

		}
	}
}