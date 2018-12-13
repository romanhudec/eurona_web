using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace Eurona.Common.DAL.Entities {
    [Serializable]
    public class PiktogramyProduktu : CMS.Entities.Entity {
        public class ReadByProduct {
            public int ProductId { get; set; }
        }
        //Id
        //[ProductId] [int] NOT NULL,
        public string Name { get; set; }
        public string Locale { get; set; }
        public string ImageUrl { get; set; }
    }
}
