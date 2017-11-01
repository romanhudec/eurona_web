using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace SHP.Entities
{
    public class Category : Entity, IUrlAliasEntity
    {
        public Category()
        {
            this.Alias = string.Empty;
        }

        public class ReadById
        {
            public int CategoryId { get; set; }
        }

        public class ReadByInstance
        {
            public int InstanceId { get; set; }
        }

        public class ReadByParentId
        {
            public int? ParentId { get; set; }
            public int? InstanceId { get; set; }
        }

        /// <summary>
        /// Načíta všetky kategorie v ktorzch je zaradený daný produkt.
        /// </summary>
        public class ReadByProductId
        {
            public int ProductId { get; set; }
        }

        public int InstanceId { get; set; }
        public int? Order { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Locale { get; set; }
        public string Icon { get; set; }

        #region IUrlAliasEntity Members
        public int? UrlAliasId { get; set; }
        public string Alias { get; set; }
        #endregion
    }
}
