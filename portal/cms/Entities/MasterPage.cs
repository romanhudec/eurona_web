using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    [Serializable]
    public class MasterPage : Entity {
        public class ReadById {
            public int MasterPageId { get; set; }
        }

        public int InstanceId { get; set; }
        public bool Default { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int Contents { get; set; }
        public string PageUrl { get; set; }
    }
}
