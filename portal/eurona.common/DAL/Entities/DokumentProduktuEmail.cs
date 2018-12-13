using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace Eurona.Common.DAL.Entities {
    [Serializable]
    public class DokumentProduktuEmail : CMS.Entities.Entity {
        public class ReadByProduct {
            public int ProductId { get; set; }
        }

        public class ReadByDate {
            public DateTime? DateFrom { get; set; }
            public DateTime? DateTo { get; set; }
        }

        public int ProductId { get; set; }
        public string Info { get; set; }
        public string Email { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
