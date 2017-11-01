using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Common.DAL.Entities;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using Eurona.Controls.UserManagement;
using System.Text;
using System.IO;
using CMS;
using System.Configuration;

namespace Eurona.User.Advisor.AngelTeam
{
	public partial class VIPMaminkaPage : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Security.IsLogged(true);

			if (!Security.IsInRole(Role.ADMINISTRATOR))
			{
				if (this.LogedAdvisor == null || (this.LogedAdvisor.AngelTeamClen == false && this.LogedAdvisor.AngelTeamManager == false))
				{
					Response.Redirect("~/user/advisor/angel-team/");
					return;
				}
			}
			List<Organization> listATPManager = Storage<Organization>.Read(new Organization.ReadByAngelTeam { AngelTeamManager = true, AngelTeamManagerTyp = (int)ATPManagerTyp.Maminka });
			this.rpMaminky.DataSource = listATPManager;
			if (!IsPostBack)
			{
				this.rpMaminky.DataBind();
			}

			if (this.LogedAdvisor != null)
			{
				this.lblName.Text = this.LogedAdvisor.Name;
				this.lblCity.Text = this.LogedAdvisor.RegisteredAddress.City;
				this.lblEmail.Text = this.LogedAdvisor.ContactEmail;
				this.lblPhone.Text = this.LogedAdvisor.ContactPhone;
				this.hlDiskuze.HRef = Page.ResolveUrl("~/user/advisor/angel-team/diskuze.aspx?id=" + this.LogedAdvisor.AccountId.ToString());
			}
		}

		private OrganizationEntity logedAdvisor = null;
		public OrganizationEntity LogedAdvisor
		{
			get
			{
				if (logedAdvisor != null) return logedAdvisor;
				logedAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByAccountId { AccountId = Security.Account.Id });
				return logedAdvisor;
			}
		}


		protected void OnItemDataBound(object Sender, RepeaterItemEventArgs e)
		{
		}
	}
}