using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace Eurona.Common.DAL.Entities {
    [Serializable]
    public class Error : CMS.Entities.Entity {
        public class ReadById {
            public int Id { get; set; }
        }
        public int AccountId { get; set; }
        public DateTime Stamp { get; set; }
        public string Location { get; set; }
        public int InstanceId { get; set; }
        public string Exception { get; set; }
        public string StackTrace { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
    }
}
