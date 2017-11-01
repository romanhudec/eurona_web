using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class AccountProfile: Entity
		{
				public AccountProfile()
				{
				}

				public class ReadById
				{
						public int AccountProfileId { get; set; }
				}

				public class ReadByAccountId
				{
						public int AccountId { get; set; }
				}

				public class ReadByAccountAndProfileName
				{
						public int AccountId { get; set; }
						public string ProfileName { get; set; }
				}

				public class ReadByAccountAndProfile
				{
						public int AccountId { get; set; }
						public int ProfileId { get; set; }
				}

				public class ReadByAccountAndProfileType
				{
						public int AccountId { get; set; }
						public int ProfileType { get; set; }
				}

				public int InstanceId { get; set; }
				public int AccountId { get; set; }
				public int ProfileId { get; set; }
				public string Value { get; set; }
				public int ProfileType { get; set; }
				public string ProfileName { get; set; }
		}
}
