﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona {
    public partial class Test : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

            //Funkcia volana v grid showOrder(xxx)
            string url = Page.ResolveUrl("~/user/advisor/reports/Objednavka.aspx");
            Utilities.RegisterShowOrderPOSTFunction(this.Page, url);
        }

        public string GetHash(string value) {
            return CMS.Utilities.Cryptographer.Encrypt(value);
        }
    }
}