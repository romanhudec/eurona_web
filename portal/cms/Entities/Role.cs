using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    [Serializable]
    public class Role : Entity {
        public static readonly string ADMINISTRATOR = "Administrator";
        public static readonly string REGISTEREDUSER = "RegisteredUser";
        public static readonly string NEWSLETTER = "Newsletter";

        public class ReadById {
            public int RoleId { get; set; }
        }

        public int InstanceId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
    }
}
