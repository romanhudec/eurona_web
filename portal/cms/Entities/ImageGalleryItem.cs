using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    [Serializable]
    public class ImageGalleryItem : Entity {
        public ImageGalleryItem() {
        }

        public class ReadById {
            public int ImageGalleryItemId { get; set; }
        }

        public class ReadByImageGalleryId {
            public int ImageGalleryId { get; set; }
        }

        public class ReadByImageGalleryAndPosition {
            public int ImageGalleryId { get; set; }
            public int Position { get; set; }
        }

        public class ReadTopByImageGalleryId {
            public int Top { get; set; }
            public int ImageGalleryId { get; set; }
        }

        public class IncrementViewCountCommand {
            public int ImageGalleryItemId { get; set; }
        }
        public class IncrementVoteCommand {
            public int AccountId { get; set; }
            public int ImageGalleryItemId { get; set; }
            public int Rating { get; set; }
        }

        public int InstanceId { get; set; }
        public int ImageGalleryId { get; set; }
        public string VirtualPath { get; set; }
        public string VirtualThumbnailPath { get; set; }
        public int Position { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public int CommentsCount { get; set; }
        public int ViewCount { get; set; }
        public int Votes { get; set; }
        public int TotalRating { get; set; }
        public double RatingResult { get; set; }

        public static AccountVote.ObjectType AccountVoteType { get { return AccountVote.ObjectType.ImageGalleryItem; } }

    }
}
