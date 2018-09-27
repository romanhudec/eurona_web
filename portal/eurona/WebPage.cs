using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Eurona {
    public class WebPage : System.Web.UI.Page {

        private const string ViewStateFieldName = "EURONA_VIEWSTATE";

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            if (Response != null && Response.Filter != null) {
                Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);
                Response.AddHeader("Content-Encoding", "gzip");
            }
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

        /*
        protected override void SavePageStateToPersistenceMedium(object state) {
            SaveCompressedPageState(state);
        }

        private void SaveCompressedPageState(object state) {
            byte[] viewStateBytes;
            using (MemoryStream stream = new MemoryStream()) {
                ObjectStateFormatter formatter = new ObjectStateFormatter();
                formatter.Serialize(stream, state);
                viewStateBytes = stream.ToArray();
            }

            byte[] compressed = ZipCompressor.Compress(viewStateBytes);
            string compressedBase64 = Convert.ToBase64String(compressed);

            ClientScript.RegisterHiddenField(ViewStateFieldName, compressedBase64);
        }

        protected override object LoadPageStateFromPersistenceMedium() {
            return LoadCompressedPageState();
        }

        private object LoadCompressedPageState() {
            string viewState = Request.Form[ViewStateFieldName];
            if (string.IsNullOrEmpty(viewState)) {
                return string.Empty;
            }

            byte[] bytes = Convert.FromBase64String(viewState);
            byte[] decompressed = ZipCompressor.Decompress(bytes);
            string decompressedBase64 = Convert.ToBase64String(decompressed);

            ObjectStateFormatter formatter = new ObjectStateFormatter();
            return formatter.Deserialize(decompressedBase64);
        }
        */

    }
}