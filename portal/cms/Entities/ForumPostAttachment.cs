using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    [Serializable]
    public class ForumPostAttachment : Entity {
        public enum AttachmentType : int {
            File = 1,
            Image = 2
        }

        public ForumPostAttachment() {
        }

        public class ReadById {
            public int ForumPostAttachmentId { get; set; }
        }
        public class ReadBy {
            public int? ForumPostId { get; set; }
        }

        public int ForumPostId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public AttachmentType Type { get; set; }
        public int Size { get; set; }
        public int Order { get; set; }
    }
}
