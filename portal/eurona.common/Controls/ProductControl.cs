using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using CMS.Controls;
using ProductEntity = Eurona.Common.DAL.Entities.Product;
using CategoryEntity = SHP.Entities.Category;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using CMS.Utilities;
using Eurona.Common.Controls.Cart;

namespace Eurona.Common.Controls.Product {
    public class ProductControl : CmsControl {
        [ToolboxItem(false)]
        public class ProductDataControl : WebControl, INamingContainer {
            private ProductEntity product;

            internal ProductDataControl(ProductControl owner, ProductEntity product)
                : base("div") {
                this.product = product;
            }

            [Bindable(true)]
            public ProductEntity DataItem {
                get { return this.product; }
            }
        }


        //Obrazok musi byt v pomere 4x3 !!!
        public static int TOP_IMAGE_WIDTH = 305;
        public static int TOP_IMAGE_HEIGHT = 229;
        private ProductEntity product = null;

        public ProductControl() {
        }
        public string CssRating { get; set; }

        [Browsable(false)]
        [DefaultValue(null)]
        [Description("Product template.")]
        [TemplateContainer(typeof(ProductDataControl))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate ProductTemplate { get; set; }

        public string CommentsFormatUrl { get; set; }
        /// <summary>
        /// Ak je property null, komponenta pracuje v rezime New.
        /// </summary>
        public int ProductId {
            get {
                object o = ViewState["ProductId"];
                return o != null ? Convert.ToInt32(o) : 0;
            }
            set { ViewState["ProductId"] = value; }
        }


        /// <summary>
        /// Vráti názov produktu
        /// </summary>
        public string Name {
            get { return this.Product.Name; }
        }

        /// <summary>
        /// Vráti prvú kategoriu v ktorej je produkt zaradený
        /// </summary>
        public int? CategoryId {
            get {
                if (this.Product.ProductCategories.Count != 0)
                    return this.Product.ProductCategories[0].CategoryId;
                return null;
            }
        }
        public ProductEntity Product {
            get {
                if (this.product == null)
                    this.product = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = this.ProductId });
                return this.product;
            }
        }

        /// <summary>
        /// Default value 10
        /// </summary>
        public int MaxImagesToUpload { get; set; }

        #region Protected overrides
        protected override void CreateChildControls() {
            base.CreateChildControls();

            //Binding
            this.product = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = this.ProductId });

            ////Update ViewCount
            //ProductEntity.IncrementViewCountCommand cmd = new CMS.Entities.Article.IncrementViewCountCommand();
            //cmd.ArticleId = Product.Id;
            //Storage<ProductEntity>.Execute<ProductEntity.IncrementViewCountCommand>( cmd );

            //Select template
            ProductDataControl data = new ProductDataControl(this, this.product);
            ITemplate template = this.ProductTemplate;
            if (template == null) template = new DefaultTemplate(this, this.CommentsFormatUrl);
            template.InstantiateIn(data);

            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes.Add("class", this.CssClass);
            div.Controls.Add(data);
            this.Controls.Add(div);

            data.DataBind();
        }
        #endregion

        private class DefaultTemplate : ITemplate {
            #region Public properties

            public string CssClass { get; set; }
            public string CssRating { get; set; }
            public bool ShowHeader { get; set; }
            private string archivUrl;
            private string commentsFormatUrl;
            private ProductControl owner = null;
            #endregion

            #region ITemplate Members

            public DefaultTemplate(ProductControl owner, string commentsFormatUrl) {
                this.owner = owner;
                this.CssClass = owner.CssClass;
                this.CssRating = owner.CssRating;

                if (!string.IsNullOrEmpty(archivUrl)) {
                    AliasUtilities aliasUtils = new AliasUtilities();
                    this.archivUrl = aliasUtils.Resolve(archivUrl, owner.Page);
                }

                if (!string.IsNullOrEmpty(commentsFormatUrl)) {
                    string url = string.Format(commentsFormatUrl, owner.ProductId);
                    AliasUtilities aliasUtils = new AliasUtilities();
                    url = aliasUtils.Resolve(url, owner.Page);

                    this.commentsFormatUrl = url + (url.Contains("?") ? "&" : "?") + owner.BuildReturnUrlQueryParam();
                }
            }

            public void InstantiateIn(Control container) {
                container.Controls.Add(new LiteralControl("Implement in Eurona OR CL!"));
            }
            #endregion

        }

        public new string BuildReturnUrlQueryParam() {
            return base.BuildReturnUrlQueryParam();
        }
        public static Control CreateGaleryControl(ProductEntity product, ImageGalleryControl imageGalery) {
            HtmlGenericControl div = new HtmlGenericControl("div");
            if (product == null) {
                return div;
            }
            ImageGalleryControl.ImageAttribute attr = new ImageGalleryControl.ImageAttribute("rel", string.Format("lightbox[product{0}]", product.Id));
            imageGalery.ImageAttributes.Add(attr);

            div.DataBinding += (s, e) => {
                        DataTable dt = CreateImageGaleryDatasource(product, (s as HtmlGenericControl).Page);
                        if (dt.Rows.Count == 0)
                            return;

                        Image img = new Image();
                        img.ImageUrl = dt.Rows[0]["ImageUrl"].ToString();
                        //img.Style.Add( "width", TOP_IMAGE_WIDTH.ToString() + "px" );
                        img.Style.Add("max-width", TOP_IMAGE_WIDTH.ToString() + "px");
                        img.Style.Add("max-height", TOP_IMAGE_HEIGHT.ToString() + "px");

                        HtmlGenericControl imageControl = new HtmlGenericControl("a");
                        imageControl.Attributes.Add(attr.Key, attr.Value);
                        imageControl.Attributes.Add("href", img.ImageUrl);
                        imageControl.Attributes.Add("title", string.Empty);
                        imageControl.Controls.Add(img);

                        div.Controls.AddAt(0, imageControl);
                    };

            div.Controls.Add(imageGalery);

            return div;
        }


        /// <summary>
        /// Metóda vytvorí datasource pre ImageGalery control.
        /// </summary>
        public static DataTable CreateImageGaleryDatasource(ProductEntity product, Page page) {
            DataTable dt = new DataTable("product_image_galery");

            DataColumn idColumn = new DataColumn("ImagePath", typeof(string));
            DataColumn urlColumn = new DataColumn("ImageUrl", typeof(string));
            DataColumn urlThumbnailColumn = new DataColumn("ImageUrlThumbnail", typeof(string));
            DataColumn positionColumn = new DataColumn("Position", typeof(Int32));
            dt.Columns.AddRange(new DataColumn[] { idColumn, urlColumn, urlThumbnailColumn, positionColumn });

            if (product == null) return dt;

            //Fill dataTable 
            string storagePath = string.Format("{0}{1}/", CMS.Utilities.ConfigUtilities.ConfigValue("SHP:ImageGallery:Product:StoragePath", page), product.Id.ToString());
            string productImagesPath = page.Server.MapPath(storagePath);

            if (!Directory.Exists(productImagesPath))
                return dt;

            DirectoryInfo di = new DirectoryInfo(productImagesPath);
            FileInfo[] fileInfos = di.GetFiles("*.*");

            //Sort files by name
            Comparison<FileInfo> comparison = new Comparison<FileInfo>(delegate(FileInfo a, FileInfo b) {
                return String.Compare(a.Name, b.Name);
            });
            Array.Sort(fileInfos, comparison);
            foreach (FileInfo fileInfo in fileInfos) {
                int position = 0;
                string[] tmp = fileInfo.Name.Split('_');
                if (tmp.Length != 0)
                    Int32.TryParse(tmp[0], out position);

                string id = Path.Combine(productImagesPath, fileInfo.Name);
                string url = storagePath + fileInfo.Name;
                string urlThumbnail = storagePath + "_t/" + fileInfo.Name;

                dt.Rows.Add(new object[] { id, url, urlThumbnail, position });
            }
            return dt;
        }

        public static CMS.Controls.Vote.RaitingControl CreateRatingControl(ProductEntity product) {
            CMS.Controls.Vote.RaitingControl ratingControl = new CMS.Controls.Vote.RaitingControl();
            ratingControl.ObjectId = product.Id;
            ratingControl.ObjectTypeId = (int)ProductEntity.AccountVoteType;
            ratingControl.RatingResult = product.RatingResult;
            ratingControl.OnVote += (objectId, rating) => {
                ProductEntity.IncrementVoteCommand cmdVote = new ProductEntity.IncrementVoteCommand();
                cmdVote.AccountId = Security.Account.Id;
                cmdVote.ProductId = objectId;
                cmdVote.Rating = rating;
                Storage<ProductEntity>.Execute(cmdVote);

                ProductEntity comment = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = objectId });
                return comment.RatingResult;
            };

            return ratingControl;
        }
    }
}
