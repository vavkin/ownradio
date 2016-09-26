using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnRadio.Client
{
    public interface ITrackPlay
    {
        void CurrentTrackPlay(bool PlayerExistFlag, String FileName);
        void NextTrackPlay();
    }
}
