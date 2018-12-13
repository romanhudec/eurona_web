using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Eurona.Common.DAL.Entities {
    [Serializable]
    public class AngelTeamSettings {
        public bool DisableATP { get; set; }
        public int MaxViewPerMinute { get; set; }
        public int BlockATPHours { get; set; }
    }
}
