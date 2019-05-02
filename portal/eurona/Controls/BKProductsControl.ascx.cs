using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProductEntity = Eurona.Common.DAL.Entities.Product;
using CartEntity = Eurona.Common.DAL.Entities.Cart;
using CartProductEntity = Eurona.Common.DAL.Entities.CartProduct;
using System.IO;
using Eurona.Common.Controls.Cart;

namespace Eurona.Controls {
    public partial class BKProductsControl : System.Web.UI.UserControl {
        protected void Page_Load(object sender, EventArgs e) {
            List<ProductEntity> list = Storage<ProductEntity>.Read(new ProductEntity.ReadWithBK());
            this.rpBKProducts.DataSource = list;

            if (!IsPostBack)
                this.rpBKProducts.DataBind();

            DateTime date = DateTime.Now.AddMonths(-1);
            //decimal narokKreditCelkem = BonusovyKreditUzivateleHelper.GetPlatnyKreditCelkem(Security.Account, date.Year, date.Month);
            decimal platnychNaTentoMesic = BonusovyKreditUzivateleHelper.GetPlatnyKreditCelkem(Security.Account, date.Year, date.Month);
            decimal cerpanoTentoMesic = BonusovyKreditUzivateleHelper.GetCerpaniKredituCelkem(Security.Account, date.Year, date.Month);
            decimal narokKreditCelkem = platnychNaTentoMesic - cerpanoTentoMesic;
            if (narokKreditCelkem <= 0 ) {
                this.addToCart.Visible = false;
            }
        }

        public string GetImageSrc(object oProductId) {
            int productId = Convert.ToInt32(oProductId);
            string noImageUrl = this.ResolveUrl("~/images/noimage.png");

            string storageUrl = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:ImageGallery:Product:StoragePath", this.Page);

            string storagePath = string.Format("{0}{1}/", storageUrl, productId);
            string productImagesPath = this.Server.MapPath(storagePath);

            if (!Directory.Exists(productImagesPath))
                return noImageUrl;

            DirectoryInfo di = new DirectoryInfo(productImagesPath);
            FileInfo[] fileInfos = di.GetFiles("*.*");

            //Sort files by name
            Comparison<FileInfo> comparison = new Comparison<FileInfo>(delegate(FileInfo a, FileInfo b) {
                return String.Compare(a.Name, b.Name);
            });
            Array.Sort(fileInfos, comparison);
            if (fileInfos.Length == 0)
                return noImageUrl;


            string urlThumbnail = storagePath + "_t/" + fileInfos[0].Name;
            return this.ResolveUrl(urlThumbnail);
        }

        protected void OnAddCart(object sender, EventArgs e) {
            decimal productKreditTotal = 0;

            //Get credit jiz v kosiku
            int bkKreditInCart = 0;
            CartEntity cart = EuronaCartHelper.GetAccountCart(this.Page);
            if (cart != null) {
                foreach (CartProductEntity cp in cart.CartProducts) {
                    ProductEntity product = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = cp.ProductId });
                    if( product == null ) continue;
                    if (product.BonusovyKredit == null) continue;
                    if (cp.CerpatBonusoveKredity) {
                        bkKreditInCart += ((int)product.BonusovyKredit.Value * cp.Quantity);
                    }
                }
            }

            //Get credit prave pridavaneho produktu/produktov
            foreach (Control ctrl in this.rpBKProducts.Items) {
                HiddenField hfId = (HiddenField)ctrl.Controls[1];
                TextBox txtMnozstvi = (TextBox)ctrl.Controls[2];

                if (string.IsNullOrEmpty(txtMnozstvi.Text)) continue;
                int quantity = 0;
                Int32.TryParse(txtMnozstvi.Text, out quantity);
                if (quantity == 0) continue;

                int productId = Convert.ToInt32(hfId.Value);

                ProductEntity p = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = productId });
                productKreditTotal += p.BonusovyKredit.Value*quantity;
            }

            DateTime date = DateTime.Now.AddMonths(-1);
            //decimal kreditNarok = BonusovyKreditUzivateleHelper.GetPlatnyKreditCelkem(Security.Account, date.Year, date.Month);//BonusovyKreditUzivateleHelper.GetKreditNarokCelkem(Security.Account, DateTime.Now.Year, DateTime.Now.Month);
            decimal platnychNaTentoMesic = BonusovyKreditUzivateleHelper.GetPlatnyKreditCelkem(Security.Account, date.Year, date.Month);
            decimal cerpanoTentoMesic = BonusovyKreditUzivateleHelper.GetCerpaniKredituCelkem(Security.Account, date.Year, date.Month);
            decimal kreditNarok = platnychNaTentoMesic - cerpanoTentoMesic;
            if (kreditNarok < (productKreditTotal + bkKreditInCart)) {
                //Alert s informaciou o pridani do nakupneho kosika
                string js = string.Format("alert('{0}');", "Na nákup takového množství nemáte dostatečný kredit!");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "addProductToCart", js, true);
                return;
            }

            int addedCount = 0;
            foreach (Control ctrl in this.rpBKProducts.Items) {
                HiddenField hfId = (HiddenField)ctrl.Controls[1];
                TextBox txtMnozstvi = (TextBox)ctrl.Controls[2];

                if (string.IsNullOrEmpty(txtMnozstvi.Text)) continue;
                int quantity = 0;
                Int32.TryParse(txtMnozstvi.Text, out quantity);
                if (quantity == 0) continue;

                int productId = Convert.ToInt32(hfId.Value);

                ProductEntity p = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = productId });
                if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(p.Code, p, quantity, true, this, false))
                    return;
                if (!EuronaCartHelper.AddProductToCart(this.Page, productId, quantity, true, this, false))
                    continue;

                addedCount += quantity;

            }

            if (addedCount != 0) {
                //Alert s informaciou o pridani do nakupneho kosika
                string js = string.Format("alert('{0}');window.location.href=window.location.href+'{1}nocache='+(new Date()).getSeconds();",
                        string.Format("Produkty byly přidány do nákupního košíku v počtu {0}", addedCount),
                     this.Request.RawUrl.Contains("?") ? "&" : "?");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "addProductToCart", js, true);

                Response.Redirect("~/user/advisor/cart.aspx");
            }
        }
    }
}