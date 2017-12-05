using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SupportedLocaleEntity = CMS.Entities.Classifiers.SupportedLocale;
using System.Web.UI.WebControls;
using AccountEntity = CMS.Entities.Account;
using System.Globalization;
using System.Threading;
using System.Configuration;
using System.Web;
using System.Web.UI.HtmlControls;

namespace CMS.Controls {
    public class LocaleSwitchControl : CmsControl {
        public LocaleSwitchControl() {
        }

        protected override void CreateChildControls() {
            base.CreateChildControls();
            List<SupportedLocaleEntity> list = Storage<SupportedLocaleEntity>.Read();

            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes.Add("class", this.CssClass);

            Table table = new Table();
            TableRow row = new TableRow();
            table.Rows.Add(row);

            foreach (SupportedLocaleEntity locale in list) {
                if (string.IsNullOrEmpty(locale.Icon))
                    continue;

                ImageButton btn = new ImageButton();
                btn.CausesValidation = false;
                btn.ID = "btnLocale" + locale.Code;
                btn.ImageUrl = Page.ResolveUrl(locale.Icon);
                btn.CommandArgument = locale.Code;
                btn.Click += OnSwitchLocale;
                if (locale.Code == System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower()) {
                    btn.Enabled = false;

                    btn.Style.Add("filter", "alpha(opacity=20)");
                    btn.Style.Add("opacity", ".2");
                    btn.Style.Add("-moz-opacity", ".2");
                }
                TableCell cell = new TableCell();
                cell.Controls.Add(btn);
                row.Cells.Add(cell);
            }

            div.Controls.Add(table);
            this.Controls.Add(div);
        }

        protected void OnSwitchLocale(Object sender, EventArgs e) {
            ImageButton lb = sender as ImageButton;
            string locale = lb.CommandArgument;
            if (Security.IsLogged(false)) {
                AccountEntity account = Security.Account;
                account.Locale = locale;
                CMS.Storage<AccountEntity>.Update(account);
            }

            //Save to cooke
            if (ConfigurationManager.AppSettings["CookieLocaleName"] != null) {
                HttpCookie c = new HttpCookie(ConfigurationManager.AppSettings["CookieLocaleName"], locale);
                c.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(c);
            }

            Response.Redirect(Page.ResolveUrl("~/"));//Request.RawUrl );
            Response.End();
        }
    }
}
