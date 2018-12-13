using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    [Serializable]
    public class Forum : Entity, IUrlAliasEntity {
        public Forum() {
            this.Pinned = false;
            this.Locked = false;
            this.Alias = string.Empty;
        }

        public class ReadById {
            public int ForumId { get; set; }
        }
        public class ReadByForumThreadId {
            public int ForumThreadId { get; set; }
        }
        public class ReadBy {
            /// <summary>
            /// Read all threads when forum is created or posted by account
            /// </summary>
            public int AccountId { get; set; }
        }
        public int ForumThreadId { get; set; }
        public int InstanceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public bool Pinned { get; set; }
        public bool Locked { get; set; }
        public int ForumPostCount { get; set; }

        public int? LastPostId { get; set; }
        public DateTime? LastPostDate { get; set; }
        public int? LastPostAccountId { get; set; }
        public string LastPostAccountName { get; set; }

        public int CreatedByAccountId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByAccountName { get; set; }

        public int ViewCount { get; set; }

        public class IncrementViewCountCommand {
            public int ForumId { get; set; }
        }

        #region IUrlAliasEntity Members
        public int? UrlAliasId { get; set; }
        public string Alias { get; set; }
        #endregion
    }
}
