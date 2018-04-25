using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ShpProductEntity = Eurona.Common.DAL.Entities.Product;
using ShpProductRelation = Eurona.Common.DAL.Entities.ProductRelation;
using ShpProductReviews = SHP.Entities.ProductReviews;
using ShpResources = SHP.Resources.Controls;
using ShpCultureUtilities = Eurona.Common.Utilities.CultureUtilities;
using SHP.Controls.Cart;
using System.Text;
using CMS.Utilities;
using System.IO;
using System.Data;
using Eurona.Common.Controls.Product;
using Eurona.Common;
using Eurona.Common.Controls.Cart;
using Eurona.Controls.Product;
using Eurona.Controls;
using CMS.Controls;

namespace Eurona.EShop {
    public partial class Product : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(Request["id"])) return;
            string id = Request["id"];
            string[] ids = id.Split(',');
            if (ids.Length > 1) {
                id = ids[0];
            }

            this.productControl.ProductId = Convert.ToInt32(id);
            if (this.productControl.Product == null) return;
            this.Title = this.productControl.Name;

            (this.Master as Eurona.EShop.DefautMasterPage).SelectedProduct = this.productControl.ProductId;

            if (!string.IsNullOrEmpty(Request["c"]))
                (this.Master as Eurona.EShop.DefautMasterPage).SelectedCategory = Convert.ToInt32(Request["c"]);

            this.rpVlastnostiProduktu.DataSource = this.ProductEntity.VlastnostiProduktu;
            this.tdVlastnostiProduktu.Visible = this.ProductEntity.VlastnostiProduktu.Count != 0;

            this.rpPiktogramyProduktu.DataSource = this.ProductEntity.PiktogramyProduktu;
            //this.tdPiktogramyProduktu.Visible = this.ProductEntity.PiktogramyProduktu.Count != 0;
            this.imgPiktogram.Visible = this.ProductEntity.PiktogramyProduktu.Count == 0;

            ImageGalleryControl.ImageAttribute attr = new ImageGalleryControl.ImageAttribute("rel", "lightbox[zadniEtiketa]");
            string zadniEtiketaImageUrl = Page.ResolveUrl("~/images/ZadniEtikety/" + this.ProductEntity.ZadniEtiketa);
            this.lblZadniEtiketa.Attributes.Add(attr.Key, attr.Value);
            this.lblZadniEtiketa.Attributes.Add("href", zadniEtiketaImageUrl);
            this.lblZadniEtiketa.Attributes.Add("title", string.Empty);
            this.lblZadniEtiketa.Visible = this.ProductEntity.ZobrazovatZadniEtiketu && !string.IsNullOrEmpty(this.ProductEntity.ZadniEtiketa);

            this.rpUcinkyProduktu.DataSource = this.ProductEntity.UcinkyProduktu;
            this.tdUcinkyProduktu.Visible = this.ProductEntity.UcinkyProduktu.Count != 0 || this.lblZadniEtiketa.Visible;

            this.trParfumacie.Visible = this.ProductEntity.Parfumacia.HasValue;
            this.tdInstructionsForUse.Visible = !String.IsNullOrEmpty(this.ProductEntity.InstructionsForUse);
            this.tdAdditionalInformation.Visible = !String.IsNullOrEmpty(this.ProductEntity.AdditionalInformation);

            this.rpVlastnostiProduktu.DataBind();
            this.rpPiktogramyProduktu.DataBind();
            this.rpUcinkyProduktu.DataBind();

            this.lvAlternateProducts.DataSource = Storage<ShpProductRelation>.Read(new ShpProductRelation.ReadBy { ParentProductId = this.ProductEntity.Id, RelationType = ShpProductRelation.Relation.Alternate });
            this.lvAlternateProducts.DataBind();

            //this.ratingControl.OnRate += ratingControl_Rate;

            if (!IsPostBack) {
                //Zaevidovanie bonusovych kreditov
                if (Security.IsLogged(false)) {
                    string kod = this.ProductEntity.Id.ToString();
                    BonusovyKreditUzivateleHelper.ZaevidujKredit(Security.Account.Id, DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktDetail, null, kod);
                }
            }

            if (this.ProductEntity.LimitDate.HasValue) {
                this.lblLimitovanaAkce.Visible = true;
                VocabularyUtilities vu = new VocabularyUtilities("TextoveHlaseni");
                CMS.Entities.Translation translation = vu.Translate("LimitovanaAkceInfo");

                int storageCount = this.ProductEntity.InternalStorageCount;
                if (storageCount == Eurona.Common.DAL.Entities.Product.INTERNAL_STORAGE_NOT_AVAILABLE)
                    storageCount = 99;
                this.lblLimitovanaAkce.Text = string.Format(translation.Trans, this.ProductEntity.LimitDate.Value, storageCount);
            }

            if (!String.IsNullOrEmpty(Request["cmd"])) {
                string cmd = Request["cmd"].ToUpper();
                if (cmd == "ADDTOCART")
                    this.AddToCart();

                return;
            }
        }
        public string Root {
            get {
                string root = CMS.Utilities.ServerUtilities.Root(this.Page.Request);
                if (root.EndsWith("/")) root = root.Remove(root.Length - 1, 1);
                return root;
            }
        }
        protected ShpProductEntity ProductEntity {
            get { return this.productControl.Product; }
        }
        public string GetShpResourceString(string key) {
            return ShpResources.ResourceManager.GetString(key);
        }
        public string GetProductPrice(decimal price, string mena) {
            string text = ShpCultureUtilities.CurrencyInfo.ToString(price, mena);
            return text;
        }

        protected override void CreateChildControls() {
            ProductControl.TOP_IMAGE_WIDTH = 533;
            ProductControl.TOP_IMAGE_HEIGHT = 400;

            if (this.ProductEntity == null) return;

            base.CreateChildControls();

            DataTable dt = ProductControl.CreateImageGaleryDatasource(this.ProductEntity, this);
            if (dt.Rows.Count <= 1) dt.Rows.Clear();
            CMS.Controls.ImageGalleryControl imgGalery = new CMS.Controls.ImageGalleryControl();
            imgGalery.UseMaxHeightAttribute = false;
            imgGalery.ID = "imgGallery";
            imgGalery.CssClass = "imageGallery";
            imgGalery.HorizontalAlign = HorizontalAlign.NotSet;
            imgGalery.IdFieldName = "ImagePath";
            imgGalery.ImageUrlFieldName = "ImageUrl";
            imgGalery.ImageUrlThumbnailFieldName = "ImageUrlThumbnail";
            imgGalery.DataSource = dt;
            Control galleryControl = ProductControl.CreateGaleryControl(this.productControl.Product, imgGalery);
            imgGalleryContainer.Controls.Add(galleryControl);
            imgGalleryContainer.DataBind();

            /*
            CMS.Controls.Vote.RaitingControl ratingControl = ProductControl.CreateRatingControl( this.ProductEntity );
            ratingControl.CssClass = "rating";
            ratingControl.IsEditing = Security.IsLogged( false );
            this.ratingContainer.Controls.Add( ratingControl );
            ratingControl.OnVote += new CMS.Controls.Vote.RaitingControl.VoteEventHandler( ratingControl_OnVote );
            */
            ratingControl.ReadOnly = !Security.IsLogged(false);
            ratingControl.Value = (decimal)this.ProductEntity.RatingResult;

            if (!string.IsNullOrEmpty(this.productControl.CommentsFormatUrl)) {
                string url = string.Format(this.productControl.CommentsFormatUrl, this.ProductEntity.Id);
                AliasUtilities aliasUtils = new AliasUtilities();
                url = aliasUtils.Resolve(url, this);

                string commentsFormatUrl = url + (url.Contains("?") ? "&" : "?") + this.productControl.BuildReturnUrlQueryParam();

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("<a href='{1}' class='{2}'>{0} ({3})</a>",
                global::SHP.Resources.Controls.ProductControl_CommentsCount,
                        this.ResolveUrl(commentsFormatUrl),
                        this.productControl.CssClass + "_comment",
                        this.ProductEntity.CommentsCount);
                this.commentsContainer.InnerHtml = sb.ToString();
            }
        }

        protected void ratingControl_Rate(object sender, EventArgs e) {
            //Zaevidovanie bonusovych kreditov
            decimal? hodnota = (ratingControl.Value * 100m) / 5;
            if (Security.IsLogged(false)) {
                ShpProductEntity.IncrementVoteCommand cmdVote = new ShpProductEntity.IncrementVoteCommand();
                cmdVote.AccountId = Security.Account.Id;
                cmdVote.ProductId = this.ProductEntity.Id;
                cmdVote.Rating = (int)ratingControl.Value;
                Storage<ShpProductEntity>.Execute(cmdVote);

                string kod = this.ProductEntity.Id.ToString();
                BonusovyKreditUzivateleHelper.ZaevidujKredit(Security.Account.Id, DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktHodnoceni, hodnota, kod);
            }

            Response.Redirect(Request.RawUrl);
        }

        protected void AddToCart() {
            int quantity = 1;
            int productId = this.ProductEntity.Id;

            ShpProductEntity product = Storage<ShpProductEntity>.ReadFirst(new ShpProductEntity.ReadById { ProductId = productId });
            if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(product.Code, product, quantity, this))
                return;
            if (!EuronaCartHelper.AddProductToCart(this.Page, product.Id, quantity, this))
                return;

            this.UpdateCartInfo();

            string url = Page.ResolveUrl(this.ProductEntity.Alias);
            url = (url.StartsWith("/") ? url.Remove(0, 1) : url);
            string root = ServerUtilities.Root(this.Request);
            url = root + url;
            //Alert s informaciou o pridani do nakupneho kosika
            string js = string.Format("alert('{0}');window.location.href='{1}?nocache='+(new Date()).getSeconds();",
                 string.Format(ShpResources.AdminProductControl_ProductWasAddedToCart_Message, this.ProductEntity.Name, quantity),
                 url);
            this.ClientScript.RegisterStartupScript(this.GetType(), "addProductToCart", js, true);
        }

        /// <summary>
        /// Update informácie v nákupnom košiku.
        /// </summary>
        public void UpdateCartInfo() {
            (this.Master as Eurona.EShop.DefautMasterPage).UpdateCartInfo();
        }

        protected void OnAddCart(object sender, ImageClickEventArgs e) {
            //Button btn = ( sender as Button );

            int quantity = 1;
            int productId = this.ProductEntity.Id;

            if (!Int32.TryParse(this.txtQuantity.Text, out quantity))
                quantity = 1;

            ShpProductEntity product = Storage<ShpProductEntity>.ReadFirst(new ShpProductEntity.ReadById { ProductId = productId });
            if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(product.Code, product, quantity, this))
                return;
            if (!EuronaCartHelper.AddProductToCart(this.Page, product.Id, quantity, this))
                return;

            this.UpdateCartInfo();

            //Alert s informaciou o pridani do nakupneho kosika
            string js = string.Format("alert('{0}');window.location.href=window.location.href+'{1}nocache='+(new Date()).getSeconds();",
                 string.Format(ShpResources.AdminProductControl_ProductWasAddedToCart_Message, this.ProductEntity.Name, quantity),
                 this.Request.RawUrl.Contains("?") ? "&" : "?");
            this.ClientScript.RegisterStartupScript(this.GetType(), "addProductToCart", js, true);
        }

        protected void OnDetail(object sender, EventArgs e) {
            Button btn = (sender as Button);
            if (string.IsNullOrEmpty(btn.CommandArgument))
                return;

            Response.Redirect(Page.ResolveUrl(btn.CommandArgument));
        }
        public string RenderImage(int productId) {
            string storageUrl = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:ImageGallery:Product:StoragePath", this);

            string storagePath = string.Format("{0}{1}/", storageUrl, productId);
            string productImagesPath = this.Server.MapPath(storagePath);

            if (!Directory.Exists(productImagesPath))
                return null;

            DirectoryInfo di = new DirectoryInfo(productImagesPath);
            FileInfo[] fileInfos = di.GetFiles("*.*");

            if (fileInfos.Length == 0)
                return null;

            string urlThumbnail = storagePath + "_t/" + fileInfos[fileInfos.Length-1].Name;
            string img = string.Format("<img src='{0}' style='border:0px none #fff;'>", this.ResolveUrl(urlThumbnail));

            return img;
        }

        protected string RenderParfumacia(int parfumacia) {
            string img = string.Format("<img src='{0}' style='width:390px'>", this.ResolveUrl(string.Format("~/images/Parfumacie/{0}.png", parfumacia)));
            return img;
        }
        protected void OnSendToFacebook(object sender, EventArgs e) {
            //Zaevidovanie bonusovych kreditov
            if (Security.IsLogged(false)) {
                string kod = this.ProductEntity.Id.ToString();
                BonusovyKreditUzivateleHelper.ZaevidujKredit(Security.Account.Id, DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktEmailFacebook, null, kod);
            }
            Response.Redirect("http://www.facebook.com/sharer.php?u=" + this.Root + Request.RawUrl);
        }
    }
}
