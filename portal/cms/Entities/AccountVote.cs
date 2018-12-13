using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    [Serializable]
    public class AccountVote : Entity {
        public enum ObjectType : int {
            Article = 100,
            ArticleComment = 101,
            Blog = 200,
            BlogComment = 201,
            ImageGalleryComment = 301,
            ImageGalleryItem = 302,
            ImageGalleryItemComment = 303,
            ForumPost = 400
        }

        public class ReadById {
            public int AccountVoteId { get; set; }
        }

        public class ReadBy {
            public int AccountId { get; set; }
            public int ObjectId { get; set; }
        }

        public class CountBy {
            public int AccountId { get; set; }
            public int ObjectTypeId { get; set; }
            public int ObjectId { get; set; }
        }

        public int InstanceId { get; set; }
        public int AccountId { get; set; }
        public int ObjectTypeId { get; set; }
        public int ObjectId { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
    }
}
