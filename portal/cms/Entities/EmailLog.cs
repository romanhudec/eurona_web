using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace CMS.Entities {
    public class EmailLog : CMS.Entities.Entity {

        public class ReadById {
            public int Id { get; set; }
        }
        //Id
        public String Email { get; set; }
        public String Subject { get; set; }
        public String Message { get; set; }
        public bool Status { get; set; }
        public String Error { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
