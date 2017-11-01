using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using CartEntity = Eurona.Common.DAL.Entities.Cart;

namespace Eurona.DAL.Entities {
    public class SingleUserCookieLinkActivity : CMS.Entities.Entity {
        public string Url { get; set; }
        public DateTime UrlTimestamp { get; set; }
        public string IPAddress { get; set; }
        public int CookieAccountId { get; set; }
        public int? RegistrationAccountId { get; set; }
        public DateTime? RegistrationTimestamp { get; set; }
        public DateTime Timestamp { get; set; }

        public class ReadBy {
            public string IPAddress { get; set; }
            public int? CookieAccountId { get; set; }
        }
    }
}
