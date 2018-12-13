using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace SHP.Entities {
    [Serializable]
    public class ProductCategories : Entity {
        public ProductCategories() {
        }

        public class ReadByProductId {
            public int ProductId { get; set; }
        }
        public class ReadByCategoryId {
            public int CategoryId { get; set; }
        }

        public int InstanceId { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

    }
}
