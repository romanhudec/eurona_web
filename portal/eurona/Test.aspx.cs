using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona {
    public partial class Test : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

            ////Funkcia volana v grid showOrder(xxx)
            //string url = Page.ResolveUrl("~/user/advisor/reports/Objednavka.aspx");
            //Utilities.RegisterShowOrderPOSTFunction(this.Page, url);

            string[] decoded = decodeVerifyCode("ae9c4f9980d62ccdf6584b13269f40e90b87e45b9ee62727558f5b9d8b9643409e0114e102c38eb9feba3748702f08546fc1d7aaa88af9ed04bc0e3ca0a98a2fd1b67dfb876c9d1a9a50c842b966cdb13836ce8ccc8cdfa1e7ca6bb18c4170f4");
            string emailFromCode = decoded[0];
            int accountFromCode = Convert.ToInt32(decoded[1]);
            string ipAddress = decoded[2];
        }

        public string GetHash(string value) {
            return CMS.Utilities.Cryptographer.Encrypt(value);
        }

        public static string[] decodeVerifyCode(string codeEncrypted) {
            string code = CMS.Utilities.Cryptographer.Decrypt(codeEncrypted);
            string[] data = code.Split('|');
            return data;
        }
    }
}