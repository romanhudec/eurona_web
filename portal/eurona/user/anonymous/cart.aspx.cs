using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Common.DAL.Entities;
using Eurona.Common.Controls.Cart;
using ShipmentEntity = SHP.Entities.Classifiers.Shipment;
using ProductsEntity = Eurona.Common.DAL.Entities.Product;
using Telerik.Web.UI;
using System.Configuration;
using Eurona.Common;

namespace Eurona.User.Anonymous {
    public partial class CartPage : WebPage {
        protected void Page_Load(object sender, EventArgs e) {

            //Ak je uzavierka nie je mozne vytvarat objednavky
            if (Eurona.Common.Application.EuronaUzavierka.IsUzavierka4Advisor()) {
                this.cartControl.Visible = false;
                this.lblPostovneInfo.Text = String.Format("Probíhá uzávěrka. Vytvářet objednávky bude možné : {0}", Eurona.Common.Application.EuronaUzavierka.GeUzavierka4AdvisorTo());
                return;
            }

            this.rblPreprava.DataSource = Storage<ShipmentEntity>.Read();
            this.rblPreprava.DataTextField = "Name";
            this.rblPreprava.DataValueField = "Code";
            if (!IsPostBack) {
                if (this.cartControl.CartEntity == null || this.cartControl.CartEntity.CartProductsCount == 0) {
                    Response.Redirect(aliasUtilities.Resolve("~/user/anonymous/default.aspx"));
                    return;
                }
                if (this.cartControl.CartEntity != null && !string.IsNullOrEmpty(this.cartControl.CartEntity.ShipmentCode))
                    this.rblPreprava.SelectedValue = this.cartControl.CartEntity.ShipmentCode;
                this.rblPreprava.DataBind();
            }

            if (string.IsNullOrEmpty(this.rblPreprava.SelectedValue) && this.rblPreprava.Items != null && this.rblPreprava.Items.Count != 0) {
                this.rblPreprava.Items[0].Selected = true;
            }

            this.cartControl.OnCartItemsChanged += new EventHandler(cartControl_OnCartItemsChanged);

            //Info o placeni postovneho
            this.SetupInfoOplaceniPostovneho();
        }

        private void SetupInfoOplaceniPostovneho() {
            this.lblPostovneInfo.Visible = false;
            /*
			//Info o placeni postovneho
			string locale = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
			string key = string.Format("EURONA:NeplaceniPostovneho:{0}", locale);
			if (ConfigurationManager.AppSettings[key] == null) this.lblPostovneInfo.Visible = false;
			else
			{
				decimal sumaBezPostovneho = Convert.ToDecimal(ConfigurationManager.AppSettings[key]);
				decimal totalWVAT = this.cartControl.CartEntity.PriceTotalWVAT.HasValue ? this.cartControl.CartEntity.PriceTotalWVAT.Value : 0m;
				VocabularyUtilities vu = null;
				if (totalWVAT < sumaBezPostovneho)
				{
					vu = new VocabularyUtilities("TextoveHlaseni");
					CMS.Entities.Translation translation = vu.Translate("DoPostovnehoZdarmaChybiObjednatJesteZa");
					this.lblPostovneInfo.Text = string.Format(translation.Trans, sumaBezPostovneho - totalWVAT);
				}
				else
				{
					if (totalWVAT == 0) this.lblPostovneInfo.Visible = false;
					else
					{
						vu = new VocabularyUtilities("TextoveHlaseni");
						CMS.Entities.Translation translation = vu.Translate("GratulujemeMatePostovneZdarma");
						this.lblPostovneInfo.Text = translation.Trans;
					}
				}
			}
             * */
        }

        void cartControl_OnCartItemsChanged(object sender, EventArgs e) {
            (this.Page.Master as PageMasterPage).UpdateCartInfo();
            //Info o placeni postovneho
            this.SetupInfoOplaceniPostovneho();
        }

        protected void OnContinue(object sender, EventArgs e) {
            if (cartControl == null) {
                Response.Redirect(aliasUtilities.Resolve("~/user/anonymous/default.aspx"));
                return;
            }
            Cart cart = this.cartControl.CartEntity;
            if (cart == null) {
                Response.Redirect(aliasUtilities.Resolve("~/user/anonymous/default.aspx"));
                return;
            }
            cart.ShipmentCode = this.rblPreprava.SelectedValue;
            Storage<Cart>.Update(cart);

            this.cartControl.CreateOrder(Security.Account.Id, true, aliasUtilities.Resolve("~/user/anonymous/order.aspx") + "?id={0}");
        }
    }
}