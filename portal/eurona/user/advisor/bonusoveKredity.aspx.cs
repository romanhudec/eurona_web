using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Common.DAL.Entities;
using Eurona.Common.Controls.Cart;
using Eurona.Controls;

namespace Eurona.user.advisor {
    public partial class bonusoveKredity : WebPage {
        private decimal narokKreditCelkem = 0;
        protected void Page_Load(object sender, EventArgs e) {
            this.bonusoveKredityUzivateleInfoControl.AccountId = Security.Account.Id;
            this.divKreditErrorMessage.Visible = false;
            this.divOrderErrorMessage.Visible = false;

            //this.divMaxPocetBKDosazen.Visible = false;

            decimal maxBKZaMesic = BonusovyKreditUzivateleHelper.GetMaximalniPocetBKZaMesic();
            decimal dosazenoTentoMesic = BonusovyKreditUzivateleHelper.GetBonusoveKredityUzivateleNazbiraneTentoMesicCelkem(Security.Account.Id);
            /*
            if (dosazenoTentoMesic == maxBKZaMesic)
                this.divMaxPocetBKDosazen.Visible = true;
            */
            DateTime date = DateTime.Now.AddMonths(-1);
            narokKreditCelkem = BonusovyKreditUzivateleHelper.GetPlatnyKreditCelkem(Security.Account, date.Year, date.Month);
            if (narokKreditCelkem == 0) {
                this.divKreditErrorMessage.Visible = true;
            }

            if (this.divOrderErrorMessage.Visible && this.divKreditErrorMessage.Visible) {
                this.divKreditErrorMessage.Visible = false;
            }
        }
    }
}