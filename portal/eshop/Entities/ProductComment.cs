using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace SHP.Entities {
    [Serializable]
    public class ProductComment : Entity {
        public class ReadById {
            public int ProductCommentId { get; set; }
        }
        public class ReadByProductId {
            public int ProductId { get; set; }
        }

        public class ReadByCommentId {
            public int CommentId { get; set; }
        }

        public class IncrementVoteCommand {
            public int AccountId { get; set; }
            public int CommentId { get; set; }
            public int Rating { get; set; }
        }

        public int InstanceId { get; set; }
        public int ProductId { get; set; }
        public int CommentId { get; set; }
        public int? ParentId { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int Votes { get; set; }
        public int TotalRating { get; set; }
        public double RatingResult { get; set; }

        public static AccountVote.ObjectType AccountVoteType { get { return AccountVote.ObjectType.ProductComment; } }
    }
}
