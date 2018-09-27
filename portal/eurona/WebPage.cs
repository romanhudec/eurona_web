using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Eurona {
    public class WebPage : System.Web.UI.Page {

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
        }

        protected override void OnError(EventArgs e) {
            base.OnError(e);

            // Get the last exception thrown
            Exception ex = Server.GetLastError();
            if (ex != null) {
                string st = ex != null ? ex.StackTrace : "";
                if (ex.InnerException != null)
                    st = ex.InnerException.Message + "<br/>" + st;

                try {
                    Eurona.Common.DAL.Entities.Error error = new Common.DAL.Entities.Error();
                    error.AccountId = Security.IsLogged(false) ? Security.Account.Id : 0;
                    error.Exception = ex != null ? ex.Message : "";
                    error.StackTrace = st;
                    error.Location = this.Request.RawUrl;
                    Storage<Eurona.Common.DAL.Entities.Error>.Create(error);
                } catch {
                }
            }

#if __CUSTOM_ERROR
			Response.Redirect("~/error.aspx?code=500");
#endif
        }
    }
}