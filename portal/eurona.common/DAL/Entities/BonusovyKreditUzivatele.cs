using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Eurona.Common.DAL.Entities {
    [Serializable]
    public class BonusovyKreditUzivatele : CMS.Entities.Entity {
        public BonusovyKreditUzivatele() {
            this.Poznamka = string.Empty;
            this.Kod = string.Empty;
        }
        public class ReadById {
            public int Id { get; set; }
        }

        public class ReadByAccount {
            public int AccountId { get; set; }
        }

        public class ReadByBonusovyKredit {
            public int BonusovyKreditId { get; set; }
        }

        public class ReadByBonusovyKreditAndAccount {
            public int BonusovyKreditId { get; set; }
            public int AccountId { get; set; }
        }

        public class ReadLast {
            public int AccountId { get; set; }
            public int Typ { get; set; }
            public string Kod { get; set; }
        }

        public int AccountId { get; set; }
        public int? TVD_Id { get; set; }
        public int BonusovyKreditId { get; set; }
        public int Typ { get; set; }
        public DateTime Datum { get; set; }
        public DateTime PlatnostOd { get; set; }
        public DateTime PlatnostDo { get; set; }

        public string Kod { get; set; }
        public decimal Hodnota { get; set; }
        public string Poznamka { get; set; }
    }
}
