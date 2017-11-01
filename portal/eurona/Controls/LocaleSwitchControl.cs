using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SupportedLocaleEntity = CMS.Entities.Classifiers.SupportedLocale;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Configuration;
using Eurona.DAL.Entities;
using CMS.Controls;
using CMS.Utilities;

namespace Eurona.Controls {
    public class LocaleSwitchControl : CmsControl {
        public LocaleSwitchControl() {
        }

        protected override void CreateChildControls() {
            base.CreateChildControls();
            List<SupportedLocaleEntity> list = Storage<SupportedLocaleEntity>.Read();

            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes.Add("class", this.CssClass);

            Table table = new Table();
            //TableRow row = new TableRow();
            //table.Rows.Add(row);
            /*
            if (Security.IsLogged(false) && Security.Account.IsInRole(Role.ADVISOR))
            {
                HyperLink hlCart = new HyperLink();
                //Check na URL Alias
                AliasUtilities aliasUtils = new AliasUtilities();
                string url = aliasUtils.Resolve("~/user/advisor/cart.aspx", this.Page);
                if (Security.Account.IsInRole(Role.ANONYMOUSADVISOR))
                    url = aliasUtils.Resolve("~/user/anonymous/cart.aspx", this.Page);

                hlCart.Text = Resources.EShopStrings.Navigation_MyCart;
                hlCart.NavigateUrl = url;

                TableCell cell = new TableCell();
                cell.Controls.Add(hlCart);
                row.Cells.Add(cell);
            }
            */
            foreach (SupportedLocaleEntity locale in list) {
                TableRow row = new TableRow();
                table.Rows.Add(row);

                LinkButton btn = new LinkButton();
                btn.CausesValidation = false;
                btn.Text = locale.Notes.ToUpper();
                btn.ID = "btnLocale" + locale.Code;
                btn.CommandArgument = locale.Code;
                btn.Click += OnSwitchLocale;
                if (locale.Code == System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower()) {
                    btn.Enabled = false;

                    btn.Style.Add("filter", "alpha(opacity=20)");
                    btn.Style.Add("opacity", ".2");
                    btn.Style.Add("-moz-opacity", ".2");
                    btn.Style.Add("color", "#878787!important");
                }

                if (row.Cells.Count != 0) {
                    TableCell cell = new TableCell();
                    cell.Controls.Add(new LiteralControl("/"));
                    row.Cells.Add(cell);
                }
                TableCell cellBtn = new TableCell();
                cellBtn.Controls.Add(btn);
                row.Cells.Add(cellBtn);
            }

            div.Controls.Add(table);
            this.Controls.Add(div);
        }

        protected void OnSwitchLocale(Object sender, EventArgs e) {
            LinkButton lb = sender as LinkButton;
            string locale = lb.CommandArgument;

            if (Security.IsLogged(false)) {
                Account account = Security.Account;
                account.Locale = locale;
                Storage<Account>.Update(account);
            }

            //Nastavenie spravnej meny do Session podla locale
            List<SHP.Entities.Classifiers.Currency> currencies = Storage<SHP.Entities.Classifiers.Currency>.Read(new SHP.Entities.Classifiers.Currency.ReadByLocale { Locale=locale });
            foreach (SHP.Entities.Classifiers.Currency ce in currencies) {
                if (ce.Locale == locale) {
                    Session["SHP:Currency:Id"] = ce.Id;
                    Session["SHP:Currency:Rate"] = ce.Rate;
                    Session["SHP:Currency:Symbol"] = ce.Symbol;
                }
            }

            //Save to cooke
            if (ConfigurationManager.AppSettings["CookieLocaleName"] != null) {
                HttpCookie c = new HttpCookie(ConfigurationManager.AppSettings["CookieLocaleName"], locale);
                c.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(c);
            }

            //Vymazanie kosika
            Eurona.Common.DAL.Entities.Cart cart = Eurona.Common.Controls.Cart.EuronaCartHelper.GetAccountCart(this.Page);
            if (cart != null) {
                foreach (Common.DAL.Entities.CartProduct cp in cart.CartProducts)
                    Storage<Common.DAL.Entities.CartProduct>.Delete(cp);

                cart.PriceTotal = 0m;
                cart.PriceTotalWVAT = 0m;
                Storage<Eurona.Common.DAL.Entities.Cart>.Update(cart);
            }

            Response.Redirect(Request.RawUrl);
            Response.End();
        }
    }
}