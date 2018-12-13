using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using CartEntity = Eurona.Common.DAL.Entities.Cart;

namespace Eurona.DAL.Entities {
    [Serializable]
    public class ReklamniZasilky : CMS.Entities.Entity {
        public string Popis { get; set; }
        public bool Default_souhlas { get; set; }
    }
}
