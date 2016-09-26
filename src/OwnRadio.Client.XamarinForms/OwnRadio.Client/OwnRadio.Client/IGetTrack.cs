using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnRadio.Client
{
    public interface IGetTrack
    {
        String GetTrackByID(String TrackID);
    }
}
