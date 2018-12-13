using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    [Serializable]
    public class ForumTracking : Entity {
        public ForumTracking() {
        }

        public class ReadById {
            public int ForumTrackingId { get; set; }
        }
        public class ReadBy {
            public int? ForumId { get; set; }
            public int? AccountId { get; set; }
        }

        public int ForumId { get; set; }
        public int AccountId { get; set; }
        public string Email { get; set; }
        public string AccountName { get; set; }
    }
}
