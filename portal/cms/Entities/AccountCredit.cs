using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class AccountCredit: Entity
		{
				public class ReadByAccountId
				{
						public int AccountId { get; set; }
				}

				public int InstanceId { get; set; }
				//FK Account
				public int AccountId { get; set; }
				private Account account = null;
				public Account Account
				{
						get
						{
								if ( account != null ) return account;
								account = Storage<Account>.ReadFirst( new Account.ReadById { AccountId = this.AccountId } );
								return account;
						}
				}

				public decimal Credit { get; set; }
		}
}
