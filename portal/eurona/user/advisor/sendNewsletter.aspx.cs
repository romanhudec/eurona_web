using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;

namespace Eurona.User.Advisor
{
	public partial class sendNewsletter : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Security.IsLogged(false)) return;

			if (this.LogedAdvisor == null) return;

			if (!IsPostBack)
			{
				this.cbZasilaniKatalogu.Checked = this.LogedAdvisor.ZasilaniKatalogu;
				this.cbZasilaniNewsletter.Checked = this.LogedAdvisor.ZasilaniNewsletter;
				this.cbZasilaniTiskovin.Checked = this.LogedAdvisor.ZasilaniTiskovin;
			}
		}

		public OrganizationEntity LogedAdvisor
		{
			get { return (this.Master as PageMasterPage).LogedAdvisor; }
		}

		protected void OnSaveClick(object sender, EventArgs e)
		{
			this.LogedAdvisor.ZasilaniKatalogu = this.cbZasilaniKatalogu.Checked;
			this.LogedAdvisor.ZasilaniNewsletter = this.cbZasilaniNewsletter.Checked;
			this.LogedAdvisor.ZasilaniTiskovin = this.cbZasilaniTiskovin.Checked;
			Storage<OrganizationEntity>.Update(this.LogedAdvisor);

			Response.Redirect("~/user/advisor/default.aspx");
		}

		protected void OnCancelClick(object sender, EventArgs e)
		{
			Response.Redirect("~/user/advisor/default.aspx");
		}

	}
}