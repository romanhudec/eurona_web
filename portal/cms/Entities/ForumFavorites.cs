using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    [Serializable]
    public class ForumFavorites : Entity {
        public ForumFavorites() {
        }

        public class ReadById {
            public int ForumFavoritesId { get; set; }
        }
        public class ReadBy {
            public int? ForumId { get; set; }
            public int? AccountId { get; set; }
        }

        public int ForumId { get; set; }
        public int AccountId { get; set; }
    }
}
