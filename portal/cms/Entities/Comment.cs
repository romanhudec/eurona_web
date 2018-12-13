using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    [Serializable]
    public class Comment : Entity {
        public class ReadById {
            public int CommentId { get; set; }
        }

        public class IncrementVoteCommand {
            public int AccountId { get; set; }
            public int ObjectTypeId { get; set; }
            public int CommentId { get; set; }
            public int Rating { get; set; }
        }

        public int InstanceId { get; set; }
        public int? ParentId { get; set; }
        public int AccountId { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int Votes { get; set; }
        public int TotalRating { get; set; }
    }
}
