using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Eurona.Common.DAL.Entities {
    public class BonusovyKreditLog : CMS.Entities.Entity {
        public int AccountId { get; set; }
        public int? BonusovyKreditTyp { get; set; }
        public String Message { get; set; }
        public String Status { get; set; }
        public DateTime Timestamp{ get; set; }
    }
}
