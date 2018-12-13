using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Eurona.Common.DAL.Entities {
    [Serializable]
    public class BonusovyKredit : CMS.Entities.Entity {
        public class ReadById {
            public int Id { get; set; }
        }
        public class ReadByTyp {
            public int Typ { get; set; }
        }

        public int InstanceId { get; set; }
        public int Typ { get; set; }
        public decimal? HodnotaOd { get; set; }
        public decimal? HodnotaDo { get; set; }
        public decimal? HodnotaOdSK { get; set; }
        public decimal? HodnotaDoSK { get; set; }
        public decimal? HodnotaOdPL { get; set; }
        public decimal? HodnotaDoPL { get; set; }
        public decimal Kredit { get; set; }
        public bool Aktivni { get; set; }

    }
}
