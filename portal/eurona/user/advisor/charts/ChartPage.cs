using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using System.Configuration;

namespace Eurona.User.Advisor.Charts
{
		public class ChartPage: WebPage
		{
				public OrganizationEntity LogedAdvisor
				{
						get
						{
								return ( this.Page.Master as User.Advisor.Charts.ChartMasterPage ).LogedAdvisor;
						}
				}

				/// <summary>
				/// Vrati connection string do TVD
				/// </summary>
				public string ConnectionString
				{
						get
						{
								return ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
						}
				}
		}
}