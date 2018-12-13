using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    [Serializable]
    public class Address : Entity {
        public class ReadById {
            public int AddressId { get; set; }
        }

        public int InstanceId { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string State { get; set; }

        public string Zip { get; set; }
        public string Street { get; set; }
        public string Notes { get; set; }
    }
}
