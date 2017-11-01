using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Common.DAL.Entities;
using CMS.Utilities;
using CMS;

namespace Eurona.Admin {
    public partial class CheckAccountsPolicy : WebPage {
        private  List<DAL.Entities.Account> invalidList = null;
        protected void Page_Load(object sender, EventArgs e) {
        }

        public void adminAccounts_OnDataLoad(Telerik.Web.UI.RadGrid gridView, bool bind) {
            List<DAL.Entities.Account> list = Storage<DAL.Entities.Account>.Read();
            invalidList = new List<DAL.Entities.Account>();
            foreach (DAL.Entities.Account account in list) {
                string loginPwd = Cryptographer.MD5Hash(account.Login);
                if (loginPwd == account.Password) {
                    invalidList.Add(account);
                }
            }
            gridView.DataSource = invalidList;
            if (bind) gridView.DataBind();
        }

        protected void OnExport(object sender, EventArgs e) {
            adminAccounts.ExportToExcel();
        }

        private void RepairPasswordPolicy() {
            if (invalidList == null || invalidList.Count == 0) return;
            foreach (DAL.Entities.Account account in invalidList) {
                string newPwd = RandomPassword.Generate();
                account.Password = Cryptographer.MD5Hash(newPwd);
                account.MustChangeAccountPassword = true;
                Storage<DAL.Entities.Account>.Update(account);

                EmailNotification email = new EmailNotification();
                email.Subject = "Nové heslo pro EURONA.cz";
                email.Message = string.Format("Dobrý den<br/><br/>právě Vám bylo vygenerováno neové heslo pro vstup na portál EURONA.cz.<br/>Vaše přihlašovací jméno je : {0}<br/>Vaše nové heslo : {1}<br/><br/>S pozdravem<br/>{2}",
                        account.Login, newPwd,
                        CMS.Utilities.ConfigUtilities.ConfigValue("SHP:SMTP:FromDisplay"));
                email.To = account.Email;
                email.Notify(true);
            }

        }

      
        protected void btnRepair_Click(object sender, EventArgs e) {
            RepairPasswordPolicy();
        }
    }
}
