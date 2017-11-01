using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AngelTeamEntity = Eurona.Common.DAL.Entities.AngelTeam;
using Eurona.Controls;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;

namespace Eurona.User.Advisor.AngelTeam
{
	public partial class MojeCestaPage : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			AngelTeamEntity angelTeam = Storage<AngelTeamEntity>.ReadFirst(new AngelTeamEntity.ReadById { AngelTeamId = (int)AngelTeamEntity.AngelTeamId.Eurona });
			if (angelTeam == null) return;

			this.lblPocetEuronaStarProUdrzeni.Text = angelTeam.PocetEuronaStarProUdrzeni.ToString();
			this.lblPocetEuronaStarProVstup.Text = angelTeam.PocetEuronaStarProVstup.ToString();

			int aktualniPocetEuronaStars = AngelTeamHelper.GetPocetEuronaStars(Security.Account);
			this.lblPocetEuronaStar.Text = aktualniPocetEuronaStars.ToString();

			if (this.LogedAdvisor != null && this.LogedAdvisor.AngelTeamClen)
			{
				if (aktualniPocetEuronaStars < angelTeam.PocetEuronaStarProUdrzeni)
					this.divGratulace.Visible = false;
			}
			else
			{
				if (aktualniPocetEuronaStars < angelTeam.PocetEuronaStarProVstup)
					this.divGratulace.Visible = false;
			}

			this.genericPage.Visible = !this.divGratulace.Visible;

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
	}
}