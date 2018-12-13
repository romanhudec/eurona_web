using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    [Serializable]
    public class BlogTag : Entity {
        public class ReadByTagId {
            public int TagId { get; set; }
        }
        public class ReadByBlogId {
            public int BlogId { get; set; }
        }

        public int TagId { get; set; }
        public int BlogId { get; set; }
        public string Name { get; set; }
    }
}
