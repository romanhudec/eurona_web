using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Common.DAL.Entities;
using Eurona.Common.Controls.Cart;
using ProductsEntity = Eurona.Common.DAL.Entities.Product;
using Telerik.Web.UI;
using System.IO;
using System.Configuration;
using Eurona.Common;

namespace Eurona.User.Anonymous {
    public partial class _default : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
            this.btnContinue.Visible = false;
            if (!Security.IsLogged(false))
                this.btnContinue.NavigateUrl = aliasUtilities.Resolve("~/user/anonymous/register.aspx");
            else this.btnContinue.NavigateUrl = aliasUtilities.Resolve("~/user/anonymous/cart.aspx");

            if (this.cartControl.CartEntity != null && this.cartControl.CartEntity.CartProductsCount != 0)
                this.btnContinue.Visible = true;

            this.cartControl.OnCartItemsChanged += new EventHandler(cartControl_OnCartItemsChanged);

            ProductsEntity.ReadVamiNejviceNakupovane filter = new ProductsEntity.ReadVamiNejviceNakupovane();
            List<ProductsEntity> list = Storage<ProductsEntity>.Read(filter);
            this.radRotatorNews.RotatorType = RotatorType.Buttons;
            this.radRotatorNews.WrapFrames = true;
            this.radRotatorNews.DataSource = list;
            this.radRotatorNews.DataBind();

            //Info o placeni postovneho
            this.SetupInfoOplaceniPostovneho();
        }

        public string GetImageSrc(int productId) {
            string noImageUrl = this.ResolveUrl("~/images/noimage.png");

            string storageUrl = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:ImageGallery:Product:StoragePath", this.Page);

            string storagePath = string.Format("{0}{1}/", storageUrl, productId);
            string productImagesPath = this.Server.MapPath(storagePath);

            if (!Directory.Exists(productImagesPath))
                return noImageUrl;

            DirectoryInfo di = new DirectoryInfo(productImagesPath);
            FileInfo[] fileInfos = di.GetFiles("*.*");

            if (fileInfos.Length == 0)
                return noImageUrl;

            //Sort files by name
            Comparison<FileInfo> comparison = new Comparison<FileInfo>(delegate(FileInfo a, FileInfo b) {
                return String.Compare(a.Name, b.Name);
            });
            Array.Sort(fileInfos, comparison);

            string urlThumbnail = storagePath + "_t/" + fileInfos[0].Name;
            return this.ResolveUrl(urlThumbnail);
        }

        private void SetupInfoOplaceniPostovneho() {
            this.lblPostovneInfo.Visible = false;
            /*
			if (this.cartControl.CartEntity == null) return;
			//Info o placeni postovneho\
			string locale = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
			string key = string.Format("EURONA:NeplaceniPostovneho:{0}", locale);
			if (ConfigurationManager.AppSettings[key] == null) this.lblPostovneInfo.Visible = false;
			else
			{
				decimal sumaBezPostovneho = Convert.ToDecimal(ConfigurationManager.AppSettings[key]);
				decimal totalWVAT = this.cartControl.CartEntity.PriceTotalWVAT.HasValue ? this.cartControl.CartEntity.PriceTotalWVAT.Value : 0m;
				VocabularyUtilities vu = null;
				if (totalWVAT < sumaBezPostovneho)
				{
					if (totalWVAT == 0) this.lblPostovneInfo.Visible = false;
					else
					{
						vu = new VocabularyUtilities("TextoveHlaseni");
						CMS.Entities.Translation translation = vu.Translate("DoPostovnehoZdarmaChybiObjednatJesteZa");
						this.lblPostovneInfo.Text = string.Format(translation.Trans, sumaBezPostovneho - totalWVAT);
					}
				}
				else
				{
					vu = new VocabularyUtilities("TextoveHlaseni");
					CMS.Entities.Translation translation = vu.Translate("GratulujemeMatePostovneZdarma");
					this.lblPostovneInfo.Text = translation.Trans;
				}
			}
             * */
        }

        void cartControl_OnCartItemsChanged(object sender, EventArgs e) {
            (this.Page.Master as PageMasterPage).UpdateCartInfo();
            //Info o placeni postovneho
            this.SetupInfoOplaceniPostovneho();
        }

        protected void OnAddCartFromRotator(object sender, EventArgs e) {
            Button btn = (sender as Button);
            if (string.IsNullOrEmpty(btn.CommandArgument))
                return;
            int quantity = 1;
            int productId = 0;

            Int32.TryParse(btn.CommandArgument, out productId);

            ProductsEntity product = Storage<ProductsEntity>.ReadFirst(new ProductsEntity.ReadById { ProductId = productId });
            if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(product.Code, product, quantity, this))
                return;
            if (!EuronaCartHelper.AddProductToCart(this.Page, productId, quantity, this))
                return;

            //Alert s informaciou o pridani do nakupneho kosika
            string js = string.Format("alert('{0}');window.location.href=window.location.href+'{1}nocache='+(new Date()).getSeconds();",
                 string.Format(SHP.Resources.Controls.AdminProductControl_ProductWasAddedToCart_Message, btn.CommandName, quantity),
                 this.Request.RawUrl.Contains("?") ? "&" : "?");
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "addProductToCart", js, true);
        }

        protected void OnAddCart(object sender, EventArgs e) {
            int quantity = 1;
            if (!Int32.TryParse(this.txtMnozstvi.Text, out quantity)) quantity = 1;

            Product p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod.Text });
            if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod.Text, p, quantity, this))
                return;
            if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, this))
                return;

            ////Alert s informaciou o pridani do nakupneho kosika
            //string js = string.Format( "alert('{0}');window.location.href=window.location.href+'{1}nocache='+(new Date()).getSeconds();",
            //    string.Format( SHP.Resources.Controls.AdminProductControl_ProductWasAddedToCart_Message, p.Name, quantity ),
            //   this.Request.RawUrl.Contains( "?" ) ? "&" : "?" );
            //this.Page.ClientScript.RegisterStartupScript( this.GetType(), "addProductToCart", js, true );

            Response.Redirect(this.Request.RawUrl);
        }
    }
}