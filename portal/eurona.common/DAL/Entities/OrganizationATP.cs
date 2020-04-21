using CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eurona.Common.DAL.Entities {
    [Serializable]
    public class OrganizationATP : CMS.Entities.Entity {
      
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string RegisteredAddressCity { get; set; }

        public int? BankContactId { get; set; }
        public string Code{ get; set; }
        public string AnonymousAssignToCode { get; set; }
        public DateTime? AnonymousCreatedAt { get; set; }
        public string AnonymousAssignStatus { get; set; }
        public string AnonymousAssignByCode { get; set; }
        public bool AnonymousOvereniSluzeb { get; set; }
        public bool AnonymousZmenaNaJineRegistracniCislo { get; set; }
        public string AnonymousZmenaNaJineRegistracniCisloText { get; set; }
        public bool AnonymousSouhlasStavajicihoPoradce { get; set; }
        public bool AnonymousSouhlasNavrzenehoPoradce { get; set; }

        public class ReadByAnonymous {
            public string RegionCode { get; set; }
            public bool AnonymousRegistration { get; set; }
            public bool? Assigned { get; set; }
            public bool? AssignedAndConfirmed { get; set; }
            public DateTime? AnonymousCreatedAt { get; set; }
            public DateTime? AnonymousAssignAt { get; set; }
            public DateTime? AnonymousTempAssignAt { get; set; }

            public int? CreatedAtDay { get; set; }
            public int? CreatedAtMonth { get; set; }
            public int? CreatedAtYear { get; set; }

            public int? NotAssignedDays { get; set; }
            public int? NotAssignedHours { get; set; }
            public int? NotAssignedMinutes { get; set; }

            public DateTime? AnonymousCreatedFrom { get; set; }
            public DateTime? AnonymousCreatedTo { get; set; }
        }
    }
}

