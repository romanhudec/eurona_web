using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using CartEntity = Eurona.Common.DAL.Entities.Cart;

namespace Eurona.DAL.Entities
{
    public class ReklamniZasilkySouhlas : CMS.Entities.Entity
    {
        public int Id_zasilky { get; set; }
        public int Id_odberatele { get; set; }
        public bool Souhlas { get; set; }
        public DateTime Datum_zmeny { get; set; }

        public class ReadByOdberatel {
            public int Id_zasilky { get; set; }
            public int Id_odberatele { get; set; }
        }
    }
}
