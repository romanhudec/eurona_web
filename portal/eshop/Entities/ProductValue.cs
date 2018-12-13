using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace SHP.Entities {
    [Serializable]
    public class ProductValue : Entity {
        public class ReadById {
            public int ProductValueId { get; set; }
        }

        public int ProductId { get; set; }
        public int AttributeId { get; set; }
        public string Value { get; set; }
    }
}
