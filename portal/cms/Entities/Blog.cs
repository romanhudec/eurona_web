using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    [Serializable]
    public class Blog : Entity, IUrlAliasEntity {
        public Blog() {
            this.ReleaseDate = DateTime.Now;
            this.Visible = true;
            this.Alias = string.Empty;
        }

        public class ReadById {
            public int BlogId { get; set; }
        }

        public class ReadByAccountId {
            public int AccountId { get; set; }
        }

        public class ReadLatest {
            public int Count { get; set; }
        }
        public class ReadReleased {
            public int? AccountId { get; set; }
            public int? TagId { get; set; }
        }

        public class IncrementViewCountCommand {
            public int BlogId { get; set; }
        }

        public class IncrementVoteCommand {
            public int AccountId { get; set; }
            public int BlogId { get; set; }
            public int Rating { get; set; }
        }

        public int InstanceId { get; set; }
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public string Locale { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Teaser { get; set; }
        public string Content { get; set; }
        public string ContentKeywords { get; set; }
        public int? RoleId { get; set; }
        public string Country { get; set; }
        public string @City { get; set; }
        public bool Approved { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public bool EnableComments { get; set; }
        public int CommentsCount { get; set; }
        public bool Visible { get; set; }

        public int ViewCount { get; set; }
        public int Votes { get; set; }
        public int TotalRating { get; set; }
        public double RatingResult { get; set; }

        //FK TAGS
        private List<BlogTag> blogTags = null;
        public List<BlogTag> BlogTags {
            get {
                if (blogTags != null) return blogTags;
                blogTags = Storage<BlogTag>.Read(new BlogTag.ReadByBlogId { BlogId = this.Id });
                return blogTags;
            }
        }

        public static AccountVote.ObjectType AccountVoteType { get { return AccountVote.ObjectType.Blog; } }

        #region IUrlAliasEntity Members
        public int? UrlAliasId { get; set; }
        public string Alias { get; set; }
        #endregion
    }
}
