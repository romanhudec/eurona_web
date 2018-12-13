using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace Eurona.DAL.Entities {
    [Serializable]
    public class MimoradnaNabidka : Entity, IUrlAliasEntity {
        public MimoradnaNabidka() {
            this.Alias = string.Empty;
        }

        public class ReadById {
            public int MimoradnaNabidkaId { get; set; }
        }

        public int InstanceId { get; set; }
        public string Locale { get; set; }
        public DateTime? Date { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Teaser { get; set; }
        public string Content { get; set; }

        #region IUrlAliasEntity Members
        public int? UrlAliasId { get; set; }
        public string Alias { get; set; }
        #endregion
    }
}
