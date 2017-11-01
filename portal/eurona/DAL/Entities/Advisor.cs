using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Eurona.DAL.Entities
{
	public class AdvisorAccount : Eurona.DAL.Entities.Account
	{
		public class ReadDisabled { }
		public class ReadEnabled { }
		public class ReadNewsletter
		{
		}

        public class ReadByFilter {
            public DateTime? RegistrationDate{ get;set;}
            public String AdvisorCode { get; set; }
            public String Login { get; set; }
            public String Email { get; set; }

            public bool isEmpty() {
                return RegistrationDate.HasValue == false && String.IsNullOrEmpty(AdvisorCode) &&
                    String.IsNullOrEmpty(Login) && String.IsNullOrEmpty(Email);
            }
        }

		public string AdvisorCode { get; set; }

		public string Name { get; set; }
		public string Mobile { get; set; }
		public string Phone { get; set; }

		public string RegisteredAddress { get; set; }
		public string CorrespondenceAddress { get; set; }

		public bool ZasilaniTiskovin { get; set; }
		public bool ZasilaniNewsletter { get; set; }
		public bool ZasilaniKatalogu { get; set; }

		public bool IsValidRegistered
		{
			get
			{
				if (string.IsNullOrEmpty(this.AdvisorCode)) return false;
				if (!this.RoleString.ToUpper().Contains(Role.ADVISOR.ToUpper())) return false;
				return true;
			}
		}
	}
}
