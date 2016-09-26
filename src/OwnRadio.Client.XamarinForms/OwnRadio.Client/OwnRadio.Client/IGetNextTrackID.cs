using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnRadio.Client
{
    public interface IGetNextTrackID
    {
        String GetNextTrackID(String DeviceID, out String Method);
    }
}
