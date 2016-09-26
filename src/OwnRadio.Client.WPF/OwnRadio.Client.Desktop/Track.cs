using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnRadio.Client.Desktop
{
    /// <summary>
    /// Class keeps info about single track
    /// </summary>
    public class Track
    {
        public enum Statuses
        {
            Skipped = -1,
            Listened = 1,
        }

        /// <summary>
        /// Track ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Track URI to obtain media content
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Listen status
        /// </summary>
        public Statuses Status { get; set; } = Statuses.Skipped;

        /// <summary>
        /// DateTime then track was finished (by reaching the end or by skipping)
        /// </summary>
        public DateTime ListenEnd { get; set; }
    }
}
