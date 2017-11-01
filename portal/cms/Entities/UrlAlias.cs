using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CMS.Entities
{
    public class UrlAlias : Entity
    {
        public UrlAlias()
        {
            this.Url = string.Empty;
            this.Locale = string.Empty;
            this.Alias = string.Empty;
            this.Name = string.Empty;
        }

        public class ReadById
        {
            public int UrlAliasId { get; set; }
        }

        /// <summary>
        /// Read by url for current locale
        /// </summary>
        public class ReadByUrl
        {
            public string Url { get; set; }
        }

        /// <summary>
        /// Read by alias for current locale
        /// </summary>
        public class ReadByAlias
        {
            public string Alias { get; set; }
        }

        public class ReadByAliasType
        {
            public class Pages { }
            public class Articles { }
            public class Blogs { }
            public class ImageGalleries { }
            public class News { }
            public class Custom { }
        }

        /// <summary>
        /// Read by alias for locale
        /// </summary>
        public class ReadByLocaleAlias
        {
            public string Alias { get; set; }
            public string Locale { get; set; }
            public bool IgnoreInstance { get; set; }
        }

        public int InstanceId { get; set; }
        public string Url { get; set; }
        public string Locale { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }

        public string AliasUI
        {
            get { return this.Alias.Length != 0 ? this.Alias.Remove(0, 1) : string.Empty; }
        }
        public string Display
        {
            get { return string.Format("{0} ({1})", this.Name, this.AliasUI); }
        }
    }
}
