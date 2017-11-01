using Eurona.Common.DAL.Entities;
using Eurona.Common.DAL.MSSQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona {
    public partial class DatovyListPage : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
        }

        protected void btnOdeslat_Click(object sender, EventArgs e) {
            Product produckt = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKodProduktu.Text });
            if (produckt == null) {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "alert", "alert('" + Resources.Strings.DatovyList_ProductNenalezen + "');", true);
                return;
            }

            //Get product attachmenst
            string[] files = GetProductAttachments(produckt);
            if (files == null || files.Length == 0) {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "alert", "alert('" + Resources.Strings.DatovyList_ZadneDokumentyProduktu + "');", true);
                return;
            }

            SHP.EmailNotification notification = new SHP.EmailNotification();
            notification.To = this.txtEmail.Text;
            notification.Subject = "Datový list pro produkt:" + this.txtKodProduktu.Text;
            notification.Message = Resources.Strings.DatovyList_Email;
            if (notification.Notify(false, files)) {
                DokumentProduktuEmail dpe = new DokumentProduktuEmail();
                dpe.Email = this.txtEmail.Text;
                dpe.ProductId = produckt.Id;
                dpe.Info = produckt.Code;
                Storage<DokumentProduktuEmail>.Create(dpe);
                this.txtKodProduktu.Text = string.Empty;
                this.Page.ClientScript.RegisterStartupScript(GetType(), "alert", "alert('" + Resources.Strings.DatovyList_EmailBylOdeslan + "');", true);
            } else this.Page.ClientScript.RegisterStartupScript(GetType(), "alert", "alert('" + Resources.Strings.DatovyList_EmailNEBylOdeslan + "');", true);
        }

        private string[] GetProductAttachments(Product product) {
            CMS.Utilities.ConfigUtilities.ConfigValue("SHP:DocumentGallery:Product:StoragePath");
            string productAttachmentsVirtualPath = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:DocumentGallery:Product:StoragePath");
            string productAttachmentsPath = Path.Combine(Server.MapPath(productAttachmentsVirtualPath), product.Id.ToString());
            if (!Directory.Exists(productAttachmentsPath))
                return null;

            string[] files = Directory.GetFiles(productAttachmentsPath);
            return files;
        }
    }

}
