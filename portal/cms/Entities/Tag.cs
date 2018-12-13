using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    [Serializable]
    public class Tag : Entity {
        public class ReadById {
            public int TagId { get; set; }
        }

        public int InstanceId { get; set; }
        public string Name { get; set; }
    }
}
