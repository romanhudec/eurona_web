using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Entities.Classifiers {
    [Serializable]
    public class ClassifierBase : Entity {
        public class ReadById {
            public int Id { get; set; }
        }

        public class ReadByCode {
            public string Code { get; set; }
        }

        public int InstanceId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Locale { get; set; }
        public string Notes { get; set; }
        public string Icon { get; set; }
        public int? Order { get; set; }
        public bool Hide { get; set; }
    }
}
