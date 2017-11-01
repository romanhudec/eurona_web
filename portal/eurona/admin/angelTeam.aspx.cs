using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AngelTeamEntity = Eurona.Common.DAL.Entities.AngelTeam;
using AngelTeamSettingsEntity = Eurona.Common.DAL.Entities.AngelTeamSettings;


namespace Eurona.Admin
{
	public partial class AngelTeamPage : WebPage
	{
		private AngelTeamEntity angelTeam = null;
		private Eurona.Common.DAL.Entities.AngelTeamSettings atpSettings = null;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				this.atpSettings = Storage<AngelTeamSettingsEntity>.ReadFirst();
				if (this.atpSettings == null)
				{
					this.atpSettings = new Common.DAL.Entities.AngelTeamSettings();
					this.atpSettings.DisableATP = false;
					this.atpSettings.BlockATPHours = 1;
					this.atpSettings.MaxViewPerMinute = 100;
					Storage<AngelTeamSettingsEntity>.Create(this.atpSettings);
				}

				this.cbDisableATP.Checked = this.atpSettings.DisableATP;
				this.txtMaxViewPerMinute.Text = this.atpSettings.MaxViewPerMinute.ToString();
				this.txtBlockATPHours.Text = this.atpSettings.BlockATPHours.ToString();
			}

		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			this.angelTeam = Storage<AngelTeamEntity>.ReadFirst(new AngelTeamEntity.ReadById { AngelTeamId = (int)AngelTeamEntity.AngelTeamId.Eurona });
			if (angelTeam == null) return;

			this.txtPocetProUdrzeni.Text = angelTeam.PocetEuronaStarProUdrzeni.ToString();
			this.txtPocetProVstup.Text = angelTeam.PocetEuronaStarProVstup.ToString();
			this.lblDisableATPMessage.Visible = this.cbDisableATP.Checked;
		}

		protected void OnSave(object sender, EventArgs e)
		{
			int pocetProUdrzeni = 0;
			int pocetProVstup = 0;

			Int32.TryParse(this.txtPocetProUdrzeni.Text, out pocetProUdrzeni);
			Int32.TryParse(this.txtPocetProVstup.Text, out pocetProVstup);

			this.angelTeam.PocetEuronaStarProUdrzeni = pocetProUdrzeni;
			this.angelTeam.PocetEuronaStarProVstup = pocetProVstup;
			Storage<AngelTeamEntity>.Update(this.angelTeam);

			int blockATPHours = 1;
			Int32.TryParse(this.txtBlockATPHours.Text, out blockATPHours);

			int maxViewPerMinute = 100;
			Int32.TryParse(this.txtMaxViewPerMinute.Text, out maxViewPerMinute);

			this.atpSettings = new Common.DAL.Entities.AngelTeamSettings();
			this.atpSettings.DisableATP = this.cbDisableATP.Checked;
			this.atpSettings.BlockATPHours = blockATPHours;
			this.atpSettings.MaxViewPerMinute = maxViewPerMinute;
			Storage<AngelTeamSettingsEntity>.Update(this.atpSettings);

			Response.Redirect(Page.ResolveUrl("~/admin/default.aspx"));
		}

		protected void OnCancel(object sender, EventArgs e)
		{
			Response.Redirect(Page.ResolveUrl("~/admin/default.aspx"));
		}
	}
}
