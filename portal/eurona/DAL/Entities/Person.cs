using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AddressEntity = CMS.Entities.Address;
using System.Text;

namespace Eurona.DAL.Entities
{
		public class HostPerson: CMS.Entities.Entity
		{
				public class ReadById
				{
						public int PersonId { get; set; }
				}

				public class ReadByAccountId
				{
						public int AccountId { get; set; }
				}

				public class ReadByAdvisorId
				{
						public int AdvisorId { get; set; }
				}

				public int InstanceId { get; set; }
				//FK Account
				public int? AccountId { get; set; }
				private Account account = null;
				public Account Account
				{
						get
						{
								if ( !this.AccountId.HasValue ) return null;

								if ( account != null ) return account;
								account = Storage<Account>.ReadFirst( new Account.ReadById { AccountId = this.AccountId.Value } );
								return account;
						}
				}

				public string Title { get; set; }
				public string FirstName { get; set; }
				public string LastName { get; set; }
				public string Email { get; set; }
				public string Notes { get; set; }
				public string Phone { get; set; }
				public string Mobile { get; set; }

				public int? AddressHomeId { get; set; }
				public int? AddressTempId { get; set; }

				public int? AdvisorPersonId { get; set; }

				// Format Display
				public string Display { get; set; }
				public void MakeDisplay()
				{
						StringBuilder sb = new StringBuilder();
						if ( !String.IsNullOrEmpty( Title ) ) sb.AppendFormat( "{0}. ", Title );
						if ( !String.IsNullOrEmpty( FirstName ) ) sb.AppendFormat( "{0} ", FirstName );
						if ( !String.IsNullOrEmpty( LastName ) ) sb.Append( LastName );
						Display = sb.ToString();
				}

		}
}
