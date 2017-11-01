using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eurona.Common.DAL.Entities
{
		public class Role : CMS.Entities.Role
		{
				public static readonly string ADVISOR = "Advisor";
				public static readonly string OPERATOR = "Operator";
				public static readonly string ANONYMOUSADVISOR = "AnonymousAdvisor";
		}
}
