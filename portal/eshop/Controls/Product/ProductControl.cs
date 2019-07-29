using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using CMS.Controls;
using ProductEntity = SHP.Entities.Product;
using CategoryEntity = SHP.Entities.Category;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data;
using System.Collections.Generic;
using SHP.Controls.Cart;
using System.ComponentModel;
using System.Text;
using CMS.Utilities;

namespace SHP.Controls.Product {
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
        private const int TOP_IMAGE_WIDTH = 305;
        private const int TOP_IMAGE_HEIGHT = 229;
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
            private ImageGalleryControl imgGalery = null;

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
                Control control = CreateDetailControl(container);
                if (control != null)
                    container.Controls.Add(control);
            }
            #endregion

            /// <summary>
            /// Vytvori Control Produktu
            /// </summary>
            private Control CreateDetailControl(Control container) {
                ProductEntity product = (container as ProductDataControl).DataItem;

                if (product == null)
                    return null;

                #region Create controls
                this.imgGalery = new ImageGalleryControl(ImageGalleryControl.DisplayMode.Display);
                this.imgGalery.ID = "imgGallery";
                this.imgGalery.CssClass = string.Empty;
                this.imgGalery.HorizontalAlign = HorizontalAlign.NotSet;
                this.imgGalery.IdFieldName = "ImagePath";
                this.imgGalery.ImageUrlFieldName = "ImageUrl";
                this.imgGalery.ImageUrlThumbnailFieldName = "ImageUrlThumbnail";
                this.imgGalery.DataBinding += (s, e) => {
                            if (product == null)
                                return;

                            this.imgGalery.DataSource = ProductControl.CreateImageGaleryDatasource(product, this.imgGalery.Page);
                        };
                #endregion
                Table mainTable = new Table();
                mainTable.CssClass = this.CssClass;
                //mainTable.Attributes.Add( "border", "1px;" );
                mainTable.Width = owner.Width;
                mainTable.Height = owner.Height;

                TableRow mainRow = new TableRow();
                TableCell mainCell = new TableCell();
                mainCell.VerticalAlign = VerticalAlign.Top;

                //Photos
                HtmlGenericControl div = new HtmlGenericControl("div");
                div.Attributes.Add("class", this.CssClass + "_imageGallery");
                div.Controls.Add(CreateGaleryControl(product, this.imgGalery));

                mainCell = new TableCell();
                mainCell.VerticalAlign = VerticalAlign.Top;
                mainCell.HorizontalAlign = HorizontalAlign.Left;
                mainCell.Controls.Add(div);
                mainRow.Cells.Add(mainCell);

                #region Add control to table
                Table table = new Table();
                table.CellPadding = 0;
                table.CellSpacing = 0;
                table.Width = Unit.Percentage(100);

                //Base product info
                table.Rows.Add(CreateTableRow(CreateInfoHeaderControl(product)));

                //Description
                table.Rows.Add(CreateTableRow(product.Description, 0, this.CssClass + "_labelDescription"));

                #region Vote & Comments
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.CssClass = this.CssClass + "_rating";

                //Vote
                CMS.Controls.Vote.RaitingControl rc = CreateRatingControl(product);
                rc.CssClass = this.CssRating;
                cell.Controls.Add(rc); row.Cells.Add(cell);
                table.Rows.Add(row);

                //Comment
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(commentsFormatUrl)) {
                    sb.AppendFormat("<a href='{1}' class='{2}'>{0} ({3})</a>",
                            Resources.Controls.ProductControl_CommentsCount,
                            container.ResolveUrl(commentsFormatUrl),
                            this.CssClass + "_comment",
                            product.CommentsCount);

                    row = new TableRow();
                    cell = new TableCell();
                    cell.HorizontalAlign = HorizontalAlign.Right;
                    cell.Controls.Add(new LiteralControl(sb.ToString()));
                    row.Cells.Add(cell);
                    table.Rows.Add(row);
                }
                #endregion

                mainCell = new TableCell();
                mainCell.VerticalAlign = VerticalAlign.Top;
                mainCell.Controls.Add(table);
                mainRow.Cells.Add(mainCell);
                #endregion

                mainTable.Rows.Add(mainRow);

                //Long description
                mainRow = new TableRow();
                mainCell = new TableCell();
                mainCell.ColumnSpan = 2;
                mainCell.Controls.Add(new LiteralControl(product.DescriptionLong));
                mainRow.Cells.Add(mainCell);
                mainTable.Rows.Add(mainRow);

                //Back
                mainRow = new TableRow();
                mainCell = new TableCell();
                mainCell.ColumnSpan = 2;
                mainCell.HorizontalAlign = HorizontalAlign.Right;
                mainCell.Controls.Add(new LiteralControl(string.Format("<a href='{0}'>{1}<a>", this.owner.ReturnUrl, Resources.Controls.BackLink)));
                mainRow.Cells.Add(mainCell);
                mainTable.Rows.Add(mainRow);

                return mainTable;
            }

            private Control CreateInfoHeaderControl(ProductEntity product) {
                Table table = new Table();
                table.CssClass = this.CssClass + "_infoHeader";
                //table.Attributes.Add( "border", "1px;" );

                LiteralControl lc1 = new LiteralControl();
                lc1.DataBinding += (s, e) => { lc1.Text = Utilities.CultureUtilities.CurrencyInfo.ToString(product.PriceTotal, lc1.Page.Session); };
                LiteralControl lc2 = new LiteralControl();
                lc2.DataBinding += (s, e) => { lc2.Text = Utilities.CultureUtilities.CurrencyInfo.ToString(product.PriceTotalWVAT, lc2.Page.Session); };
                table.Rows.Add(CreateTableRow(Resources.Controls.ProductControl_Price, table.CssClass + "_label", lc1, table.CssClass + "_value"));
                table.Rows.Add(CreateTableRow(Resources.Controls.ProductControl_PriceWVAT, table.CssClass + "_label", lc2, table.CssClass + "_value"));
                table.Rows.Add(CreateTableRow(Resources.Controls.ProductControl_Availability, table.CssClass + "_label", new LiteralControl(product.Availability), table.CssClass + "_value"));

                //Label tQuantity
                TableRow row = new TableRow();

                TableCell cell = new TableCell();
                cell.CssClass = table.CssClass + "_cart";
                cell.ColumnSpan = 2;
                cell.VerticalAlign = VerticalAlign.Middle;
                cell.HorizontalAlign = HorizontalAlign.Left;
                Label lblQuantity = new Label();
                lblQuantity.Text = Resources.Controls.AdminProductControl_Quantity;
                lblQuantity.CssClass = cell.CssClass + "_labelQuantity";
                cell.Controls.Add(lblQuantity);

                // TextBox Cart
                TextBox txtQuantity = new TextBox();
                txtQuantity.ID = "txtQuantity";
                txtQuantity.CssClass = cell.CssClass + "_inputQuantity";
                txtQuantity.Text = "1";
                cell.Controls.Add(txtQuantity);
                cell.Controls.Add(owner.CreateNumberValidatorControl(txtQuantity.ID));

                //Button Add cart
                Button btnAddCart = new Button();
                btnAddCart.CssClass = cell.CssClass + "_buttonAddCart";
                btnAddCart.Click += (s, e) => {
                    int quantity = 1;
                    Int32.TryParse(txtQuantity.Text, out quantity);

                    CartHelper.AddProductToCart(btnAddCart.Page, product.Id, quantity);
                    //Alert s informaciou o pridani do nakupneho kosika
                    string js = string.Format("alert('{0}');window.location.href=window.location.href+'{1}nocache='+(new Date()).getSeconds();",
                         string.Format(Resources.Controls.AdminProductControl_ProductWasAddedToCart_Message, product.Name, quantity),
                         this.owner.Request.RawUrl.Contains("?") ? "&" : "?");
                    btnAddCart.Page.ClientScript.RegisterStartupScript(btnAddCart.Page.GetType(), "addProductToCart", js, true);
                };
                cell.Controls.Add(btnAddCart);
                row.Cells.Add(cell);

                table.Rows.Add(row);
                return table;

            }
            private TableRow CreateTableRow(Control control) {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.Controls.Add(control);
                row.Cells.Add(cell);

                return row;
            }
            private TableRow CreateTableRow(string labelText, int colSpan, string cssClass) {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.ColumnSpan = colSpan;
                cell.Text = labelText;
                if (!string.IsNullOrEmpty(cssClass))
                    cell.CssClass = cssClass;
                row.Cells.Add(cell);

                return row;
            }
            private TableRow CreateTableRow(string labelText, string labelCss, Control control, string controlCss) {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.CssClass = labelCss;
                cell.Text = labelText;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.CssClass = controlCss;
                cell.Controls.Add(control);
                row.Cells.Add(cell);

                return row;
            }
        }

        public new string BuildReturnUrlQueryParam() {
            return base.BuildReturnUrlQueryParam();
        }
        public static Control CreateGaleryControl(ProductEntity product, ImageGalleryControl imageGalery) {
            ImageGalleryControl.ImageAttribute attr = new ImageGalleryControl.ImageAttribute("rel", string.Format("lightbox[product{0}]", product.Id));
            imageGalery.ImageAttributes.Add(attr);

            HtmlGenericControl div = new HtmlGenericControl("div");
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

            //Fill dataTable 
            string storagePath = string.Format("{0}{1}/", CMS.Utilities.ConfigUtilities.ConfigValue("SHP:ImageGallery:Product:StoragePath", page), product.Id.ToString());
            string productImagesPath = page.Server.MapPath(storagePath);

            if (!Directory.Exists(productImagesPath))
                return dt;

            DirectoryInfo di = new DirectoryInfo(productImagesPath);
            FileInfo[] fileInfos = di.GetFiles("*.*");

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

            //Defaul sort by position
            dt.DefaultView.Sort = "Position ASC";

            return dt;
        }

        public static CMS.Controls.Vote.RaitingControl CreateRatingControl(ProductEntity product) {
            CMS.Controls.Vote.RaitingControl ratingControl = new CMS.Controls.Vote.RaitingControl();
            ratingControl.IsEditing = Security.IsLogged(false);
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
