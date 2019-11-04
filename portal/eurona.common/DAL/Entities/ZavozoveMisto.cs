using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eurona.Common.DAL.Entities {
    [Serializable]
    public class ZavozoveMisto : CMS.Entities.Entity {
        public class ReadById {
            public int Id { get; set; }
        }
        public class ReadByMesto {
            public string Mesto { get; set; }
        }
        public class ReadJenAktualiByMesto {
            public string Mesto { get; set; }
        }
        public class ReadOnlyMestoDistinct {
        }
        public string Mesto { get; set; }
        public DateTime DatumACas { get; set; }
    }
}
