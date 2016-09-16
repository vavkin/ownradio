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
	public interface IGetNextTrackID
	{
		String GetNextTrackID(String DeviceID, String GUID, String Method, bool ListedTillTheEnd);

	}
}