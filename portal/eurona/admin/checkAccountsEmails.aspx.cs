using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Common.DAL.Entities;
using CMS.Utilities;
using CMS;
using Telerik.Web.UI;

namespace Eurona.Admin {
    public partial class CheckAccountsEmails : WebPage {
        private  List<DAL.Entities.Account> invalidList = null;
        protected void Page_Load(object sender, EventArgs e) {
        }

        public void adminAccounts_OnDataLoad(Telerik.Web.UI.RadGrid gridView, bool bind) {
            List<DAL.Entities.Account> list = Storage<DAL.Entities.Account>.Read();
            invalidList = new List<DAL.Entities.Account>();
            foreach (DAL.Entities.Account account in list) {
                if (account.Email == null || account.Email == string.Empty) continue;
                if (invalidList.Contains(account)) continue;
                List<DAL.Entities.Account> foundsList = list.FindAll(x => x.Email == account.Email);
                if (foundsList != null && foundsList.Count >= 2)
                    invalidList.AddRange(foundsList);

            }
            gridView.DataSource = invalidList;
            if (bind) gridView.DataBind();
        }

        protected void OnExport(object sender, EventArgs e) {
            adminAccounts.ExportToExcel();
        }
    }
}
