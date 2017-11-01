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
using CMS.Controls.RadEditor;
using VATEntity = SHP.Entities.Classifiers.VAT;
using System.Globalization;

[assembly: WebResource( "SHP.Controls.Product.VAT.js", "application/x-javascript" )]
namespace SHP.Controls.Product
{
		public class AdminProductControl: CmsControl
		{
				private RadEditor edtDescription;
				private TextBox txtManufacturer = null;
				private TextBox txtCode = null;
				private TextBox txtName = null;
				private TextBox txtDescription = null;
				private TextBox txtAvailability = null;
				private TextBox txtStorageCount = null;
				private TextBox txtPrice = null;
				private TextBox txtPriceWVAT = null;
				private DropDownList ddlVAT = null;
				private DropDownList ddlDiscountType = null;
				private TextBox txtDiscount = null;
				private CMS.Controls.ASPxUrlAliasTextBox txtUrlAlis;
				private TreeView tvCategories = null;

				private Telerik.Web.UI.RadPanelBar expanderBar = null;
				private AdminProductHighlightsControl highlightsControl = null;
				private AdminProductRelationControl alternateProductsControl = null;
				private AdminProductRelationControl relatedProductsControl = null;
				private AdminProductReviewsControl reviewsControl = null;

				private ImageGalleryControl imgGalery = null;
				private ASPxMultipleFileUpload mfuPhotos = null;

				private Button btnSave = null;
				private Button btnCancel = null;

				private ProductEntity product = null;

				public AdminProductControl()
				{
						this.MaxImagesToUpload = 10;
				}

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? ProductId
				{
						get
						{
								object o = ViewState["ProductId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["ProductId"] = value; }
				}
				/// <summary>
				/// ID Url Alis prefixu, ktory sa ma doplnat pre samotny alias.
				/// </summary>
				public int? UrlAliasPrefixId
				{
						get
						{
								object o = ViewState["UrlAliasPrefixId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["UrlAliasPrefixId"] = value; }
				}

				public string DisplayUrlFormat { get; set; }
				public string CommentsFormatUrl { get; set; }
				public string CssImageGalery { get; set; }
				public string CssTreeView { get; set; }

				/// <summary>
				/// Default value 10
				/// </summary>
				public int MaxImagesToUpload { get; set; }

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						//Binding
						if ( !this.ProductId.HasValue )
						{
								this.product = new SHP.Entities.Product();
								this.product.Locale = Security.Account.Locale;
						}
						else
								this.product = Storage<ProductEntity>.ReadFirst( new ProductEntity.ReadById { ProductId = this.ProductId.Value } );

						Control productControl = CreateDetailControl( this.product );
						if ( productControl != null )
						{
								this.Controls.Add( productControl );

								this.txtUrlAlis.FieldID = this.txtName.ClientID;
								this.txtUrlAlis.UrlAliasPrefixId = this.UrlAliasPrefixId;
						}

						//Datasource for DropDownList
						this.ddlVAT.DataSource = Storage<VATEntity>.Read( null );
						this.ddlVAT.DataValueField = "Id";
						this.ddlVAT.DataTextField = "Display";

						//Datasource for DropDownList
						this.ddlDiscountType.Items.Add( new ListItem { Value = ( (int)ProductEntity.DiscountType.Percent ).ToString(), Text = Resources.Controls.AdminProductControl_DiscountType_Percent } );
						this.ddlDiscountType.Items.Add( new ListItem { Value = ( (int)ProductEntity.DiscountType.Price ).ToString(), Text = Resources.Controls.AdminProductControl_DiscountType_Price } );

						//Binding
						if ( !IsPostBack )
						{
								this.ddlVAT.DataBind();

								this.txtManufacturer.Text = this.product.Manufacturer;
								this.txtCode.Text = this.product.Code;
								this.txtName.Text = this.product.Name;
								this.txtDescription.Text = this.product.Description;
								this.edtDescription.Editor.Content = this.product.DescriptionLong;
								this.txtPrice.Text = this.product.Price.ToString( "F2" );
								this.txtPriceWVAT.Text = this.product.PriceWVAT.ToString( "F2" );
								if ( this.product.VATId.HasValue ) this.ddlVAT.SelectedValue = this.product.VATId.ToString();
								this.ddlDiscountType.SelectedValue = ( (int)this.product.DiscountTypeId ).ToString();
								this.txtDiscount.Text = this.product.Discount.ToString();
								this.txtAvailability.Text = this.product.Availability;
								this.txtStorageCount.Text = this.product.StorageCount.HasValue ? this.product.StorageCount.Value.ToString() : string.Empty;

								this.txtUrlAlis.AutoCompletteAlias = !this.ProductId.HasValue;
								this.txtUrlAlis.Text = this.product.Alias.StartsWith( "~" ) ? this.product.Alias.Remove( 0, 1 ) : this.product.Alias;

								this.imgGalery.IdFieldName = "ImagePath";
								this.imgGalery.PositionFieldName = "Position";
								this.imgGalery.ImageUrlFieldName = "ImageUrl";
								this.imgGalery.ImageUrlThumbnailFieldName = "ImageUrlThumbnail";
								this.imgGalery.DataSource = CreateImageGaleryDatasource( this.product );
								this.imgGalery.DataBind();

								//Expand all checked modes
								foreach ( TreeNode node in this.tvCategories.CheckedNodes )
								{
										TreeNode parent = node.Parent;
										while ( parent != null )
										{
												parent.Expand();
												parent = parent.Parent;
										}
								}
						}

						string separator = new CultureInfo( CultureInfo.CurrentUICulture.LCID, false ).NumberFormat.NumberDecimalSeparator;
						this.txtPrice.Attributes.Add( "onkeyup", string.Format( "calculatePriceWithVAT('{0}','{1}','{2}','{3}')", this.ddlVAT.ClientID, this.txtPrice.ClientID, this.txtPriceWVAT.ClientID, separator ) );
						this.txtPriceWVAT.Attributes.Add( "onkeyup", string.Format( "calculatePriceWithoutVAT('{0}','{1}','{2}','{3}')", this.ddlVAT.ClientID, this.txtPrice.ClientID, this.txtPriceWVAT.ClientID, separator ) );

						ClientScriptManager cs = this.Page.ClientScript;
						Type cstype = this.GetType();
						string urlInclude = cs.GetWebResourceUrl( cstype, "SHP.Controls.Product.VAT.js" );
						cs.RegisterClientScriptInclude( cstype, "VATJs", urlInclude );

				}
				#endregion

				/// <summary>
				/// Vytvori Control Produktu
				/// </summary>
				private Control CreateDetailControl( SHP.Entities.Product product )
				{
						this.txtManufacturer = new TextBox();
						this.txtManufacturer.ID = "txtManufacturer";
						this.txtManufacturer.Width = Unit.Percentage( 100 );

						this.txtCode = new TextBox();
						this.txtCode.ID = "txtCode";
						this.txtCode.Width = Unit.Percentage( 100 );

						this.txtName = new TextBox();
						this.txtName.ID = "txtName";
						this.txtName.Width = Unit.Percentage( 100 );

						this.txtDescription = new TextBox();
						this.txtDescription.ID = "txtDescription";
						this.txtDescription.TextMode = TextBoxMode.MultiLine;
						this.txtDescription.Rows = 3;
						this.txtDescription.Width = Unit.Percentage( 99 );

						this.edtDescription = new RadEditor();
						this.edtDescription.ID = "edtDescription";

						this.ddlVAT = new DropDownList();
						this.ddlVAT.ID = "ddlVAT";
						this.ddlVAT.Width = Unit.Pixel( 100 );

						this.txtPrice = new TextBox();
						this.txtPrice.ID = "txtPrice";
						this.txtPrice.Width = Unit.Pixel( 100 );

						this.txtPriceWVAT = new TextBox();
						this.txtPriceWVAT.ID = "txtPriceWVAT";
						this.txtPriceWVAT.Width = Unit.Pixel( 100 );

						this.ddlDiscountType = new DropDownList();
						this.ddlDiscountType.ID = "ddlDiscountType";
						this.ddlDiscountType.Width = Unit.Pixel( 100 );

						this.txtDiscount = new TextBox();
						this.txtDiscount.ID = "txtDiscount";
						this.txtDiscount.Width = Unit.Pixel( 100 );

						this.txtAvailability = new TextBox();
						this.txtAvailability.ID = "txtAvailability";
						this.txtAvailability.Width = Unit.Percentage( 100 );

						this.txtStorageCount = new TextBox();
						this.txtStorageCount.ID = "txtStorageCount";
						this.txtStorageCount.Width = Unit.Percentage( 100 );

						//Priradenie id fieldu z ktoreho sa generuje alias
						this.txtUrlAlis = new ASPxUrlAliasTextBox();
						this.txtUrlAlis.ID = "txtUrlAlis";
						this.txtUrlAlis.Width = Unit.Percentage( 100 );

						this.tvCategories = CreateCategoriesControl( this.product );
						this.tvCategories.ID = "tvCategories";
						this.tvCategories.Width = Unit.Percentage( 100 );

						#region Highlight
						this.highlightsControl = new AdminProductHighlightsControl();
						this.highlightsControl.ID = "highlightsControl";
						this.highlightsControl.Width = Unit.Percentage( 100 );
						this.highlightsControl.ProductId = product.Id;
						#endregion

						#region Image gallery
						this.imgGalery = new ImageGalleryControl( ImageGalleryControl.DisplayMode.Edit );
						this.imgGalery.CssClass = this.CssImageGalery;
						this.imgGalery.ImageDelete += ( s, e ) =>
						{
								object id = ( e as object );
								if ( id == null )
										return;

								if ( File.Exists( id.ToString() ) )
										File.Delete( id.ToString() );

								string directory = Path.GetDirectoryName( id.ToString() );
								string fileName = Path.GetFileName( id.ToString() );
								string fileThumbnailPath = Path.Combine( directory, "_t\\" + fileName );
								if ( File.Exists( fileThumbnailPath ) )
										File.Delete( fileThumbnailPath );

								this.imgGalery.DataSource = CreateImageGaleryDatasource( this.product );
								this.imgGalery.DataBind();
						};

						this.mfuPhotos = new ASPxMultipleFileUpload();
						this.mfuPhotos.ID = "mfuPhotos";
						this.mfuPhotos.MaxfilesToUpload = this.MaxImagesToUpload;

						HtmlGenericControl divPhotos = new HtmlGenericControl( "div" );
						divPhotos.Controls.Add( this.imgGalery );
						divPhotos.Controls.Add( this.mfuPhotos );
						#endregion

						//Expander
						UpdatePanel updatePanelExpander = new UpdatePanel();
						updatePanelExpander.ID = "updatePanelExpander";
						updatePanelExpander.UpdateMode = UpdatePanelUpdateMode.Conditional;
						this.expanderBar = new Telerik.Web.UI.RadPanelBar();
						this.expanderBar.ID = "paExpander";
						this.expanderBar.Width = Unit.Percentage( 100 );
						this.expanderBar.ExpandMode = Telerik.Web.UI.PanelBarExpandMode.SingleExpandedItem;
						updatePanelExpander.ContentTemplateContainer.Controls.Add( this.expanderBar );

						#region Alternate & Related Products
						this.alternateProductsControl = new AdminProductRelationControl();
						this.alternateProductsControl.Relation = SHP.Entities.ProductRelation.Relation.Alternate;
						this.alternateProductsControl.ProductId = product.Id;
						this.alternateProductsControl.ID = "altPc";
						this.alternateProductsControl.Height = Unit.Pixel( 200 );

						this.relatedProductsControl = new AdminProductRelationControl();
						this.relatedProductsControl.Relation = SHP.Entities.ProductRelation.Relation.Related;
						this.relatedProductsControl.ProductId = product.Id;
						this.relatedProductsControl.ID = "relPc";
						this.relatedProductsControl.Height = Unit.Pixel( 200 );
						#endregion

						#region Reviews
						this.reviewsControl = new AdminProductReviewsControl();
						this.reviewsControl.ProductId = product.Id;
						this.reviewsControl.ID = "revControl";
						this.reviewsControl.Height = Unit.Pixel( 200 );
						#endregion

						this.btnSave = new Button();
						this.btnSave.CausesValidation = true;
						this.btnSave.Text = Resources.Controls.SaveButton_Text;
						this.btnSave.Click += new EventHandler( OnSave );
						this.btnCancel = new Button();
						this.btnCancel.CausesValidation = false;
						this.btnCancel.Text = Resources.Controls.CancelButton_Text;
						this.btnCancel.Click += new EventHandler( OnCancel );

						Table table = new Table();
						table.Width = this.Width;
						table.Height = this.Height;

						//Zakladne vlastnosti
						HtmlGenericControl h2Separator = new HtmlGenericControl( "H2" );
						h2Separator.InnerText = Resources.Controls.AdminProductControl_BasicProperties;
						h2Separator.Attributes.Add( "class", "horizontalSeparator" );
						table.Rows.Add( CreateTableRow( h2Separator, 2 ) );

						table.Rows.Add( CreateTableRow( Resources.Controls.AdminProductControl_Manufacturer, this.txtManufacturer, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminProductControl_Code, this.txtCode, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminProductControl_Name, this.txtName, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminProductControl_ShortDescription, this.txtDescription, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminProductControl_Description, this.edtDescription, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminProductControl_VAT, this.ddlVAT, 0, true, ValidationDataType.Double ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminProductControl_Price, this.txtPrice, 0, true, ValidationDataType.Double ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminProductControl_PriceWVAT, this.txtPriceWVAT, 0, false, ValidationDataType.Double ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminProductControl_DiscountType, this.ddlDiscountType, 0, true, ValidationDataType.Integer ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminProductControl_Discount, this.txtDiscount, 0, true, ValidationDataType.Double ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminProductControl_Availability, this.txtAvailability, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminProductControl_StorageCount, this.txtStorageCount, 0, false, ValidationDataType.Integer ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminProductControl_UrlAlias, this.txtUrlAlis, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminProductControl_Categories, this.tvCategories, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminProductControl_Highlights, this.highlightsControl, false ) );

						//-------------------------------------------------------------------------
						table.Rows.Add( CreateTableRow( updatePanelExpander, 2 ) );
						//-------------------------------------------------------------------------
						Telerik.Web.UI.RadPanelItem paItem = new Telerik.Web.UI.RadPanelItem();
						this.expanderBar.Items.Add( paItem );
						paItem.Expanded = true;
						paItem.Text = Resources.Controls.AdminProductControl_Photos;

						Telerik.Web.UI.RadPanelItem paSubItem = new Telerik.Web.UI.RadPanelItem();
						paSubItem.ItemTemplate = new RadPanelBarItemTemplate( divPhotos );
						paItem.Items.Add( paSubItem );
						//-------------------------------------------------------------------------
						//-------------------------------------------------------------------------
						paItem = new Telerik.Web.UI.RadPanelItem();
						this.expanderBar.Items.Add( paItem );
						paItem.Expanded = true;
						paItem.Text = Resources.Controls.AdminProductControl_AlternateProducts;

						paSubItem = new Telerik.Web.UI.RadPanelItem();
						paSubItem.ItemTemplate = new RadPanelBarItemTemplate( this.alternateProductsControl );
						paItem.Items.Add( paSubItem );
						//-------------------------------------------------------------------------
						//-------------------------------------------------------------------------
						paItem = new Telerik.Web.UI.RadPanelItem();
						this.expanderBar.Items.Add( paItem );
						paItem.Expanded = true;
						paItem.Text = Resources.Controls.AdminProductControl_RelatedProducts;

						paSubItem = new Telerik.Web.UI.RadPanelItem();
						paSubItem.ItemTemplate = new RadPanelBarItemTemplate( this.relatedProductsControl );
						paItem.Items.Add( paSubItem );
						//-------------------------------------------------------------------------
						//-------------------------------------------------------------------------
						paItem = new Telerik.Web.UI.RadPanelItem();
						this.expanderBar.Items.Add( paItem );
						paItem.Expanded = true;
						paItem.Text = Resources.Controls.AdminProductControl_Reviews;

						paSubItem = new Telerik.Web.UI.RadPanelItem();
						paSubItem.ItemTemplate = new RadPanelBarItemTemplate( this.reviewsControl );
						paItem.Items.Add( paSubItem );
						//-------------------------------------------------------------------------

						////Doplnkove vlastnosti
						//h2Separator = new HtmlGenericControl( "H2" );
						//h2Separator.InnerText = Resources.Controls.AdminProductControl_AdvancedProperties;
						//h2Separator.Attributes.Add( "class", "horizontalSeparator" );
						//table.Rows.Add( CreateTableRow( h2Separator, 2 ) );

						//Save Cancel Buttons
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.ColumnSpan = 2;
						cell.Controls.Add( this.btnSave );
						cell.Controls.Add( this.btnCancel );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						table.CssClass = this.CssClass;
						return table;
				}

				private TreeView CreateCategoriesControl( ProductEntity product )
				{
						TreeView tv = new TreeView();
						tv.CssClass = this.CssTreeView;
						tv.ShowCheckBoxes = TreeNodeTypes.All;
						tv.ShowLines = true;
						tv.ShowExpandCollapse = true;
						tv.NodeWrap = true;
						tv.SelectedNodeStyle.CssClass = this.CssTreeView + "_selectedItem";

						List<CategoryEntity> categories = Storage<CategoryEntity>.Read();
						foreach ( CategoryEntity entity in categories )
						{
								if ( entity.ParentId.HasValue )
										continue;

								TreeNode node = new TreeNode( entity.Name, entity.Id.ToString(), string.Empty, string.Empty, string.Empty );
								PopulateTreeViewChilds( node, product, categories );
								node.Collapse();
								tv.Nodes.Add( node );

								int index = product.ProductCategories.FindIndex( x => x.CategoryId == entity.Id );
								if ( index != -1 )
										node.Checked = true;
						}

						return tv;
				}

				private void PopulateTreeViewChilds( TreeNode rootNode, ProductEntity product, List<CategoryEntity> categories )
				{
						foreach ( CategoryEntity entity in categories )
						{
								if ( entity.ParentId != Convert.ToInt32( rootNode.Value ) )
										continue;

								TreeNode node = new TreeNode( entity.Name, entity.Id.ToString(), string.Empty, string.Empty, string.Empty );
								PopulateTreeViewChilds( node, product, categories );
								rootNode.ChildNodes.Add( node );
								node.Collapse();

								int index = product.ProductCategories.FindIndex( x => x.CategoryId == entity.Id );
								if ( index != -1 )
										node.Checked = true;
						}
				}

				private TableRow CreateTableRow( Control control, int colspan )
				{
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = "form_control";
						cell.ColumnSpan = colspan;
						cell.Controls.Add( control );
						row.Cells.Add( cell );

						return row;
				}

				private TableRow CreateTableRow( string labelText, Control control, bool required )
				{
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = required ? "form_label_required" : "form_label";
						cell.Text = labelText;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( control );
						if ( required ) cell.Controls.Add( base.CreateRequiredFieldValidatorControl( control.ID ) );
						row.Cells.Add( cell );

						return row;
				}

				private TableRow CreateTableRow( string labelText, Control control, int controlColspan, bool required, ValidationDataType? valDataType )
				{
						return CreateTableRow( labelText, control, null, controlColspan, required, valDataType );
				}

				private TableRow CreateTableRow( string labelText, Control control, string label2Text, int controlColspan, bool required, ValidationDataType? valDataType )
				{
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = required ? "form_label_required" : "form_label";
						cell.Text = labelText;
						row.Cells.Add( cell );

						if ( control == null )
								return row;

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( control );
						cell.ColumnSpan = controlColspan;

						if ( !string.IsNullOrEmpty( label2Text ) )
								cell.Controls.Add( new LiteralControl( label2Text ) );

						if ( required ) cell.Controls.Add( base.CreateRequiredFieldValidatorControl( control.ID ) );

						switch ( valDataType )
						{
								case ValidationDataType.Integer:
										cell.Controls.Add( base.CreateNumberValidatorControl( control.ID ) );
										break;
								case ValidationDataType.Double:
										cell.Controls.Add( base.CreateDoubleValidatorControl( control.ID ) );
										break;
						}

						row.Cells.Add( cell );
						return row;
				}

				#region Product Photo methods

				/// <summary>
				/// Metóda vytvorí datasource pre ImageGalery control.
				/// </summary>
				private DataTable CreateImageGaleryDatasource( ProductEntity product )
				{
						DataTable dt = new DataTable( "product_image_galery" );

						DataColumn idColumn = new DataColumn( "ImagePath", typeof( string ) );
						DataColumn urlColumn = new DataColumn( "ImageUrl", typeof( string ) );
						DataColumn urlThumbnailColumn = new DataColumn( "ImageUrlThumbnail", typeof( string ) );
						DataColumn positionColumn = new DataColumn( "Position", typeof( Int32 ) );
						dt.Columns.AddRange( new DataColumn[] { idColumn, urlColumn, urlThumbnailColumn, positionColumn } );

						//Fill dataTable 
						string storagePath = string.Format( "{0}{1}/", ConfigValue( "SHP:ImageGallery:Product:StoragePath" ), product.Id.ToString() );
						string productImagesPath = Server.MapPath( storagePath );

						if ( !Directory.Exists( productImagesPath ) )
								return dt;

						DirectoryInfo di = new DirectoryInfo( productImagesPath );
						FileInfo[] fileInfos = di.GetFiles( "*.*" );

						foreach ( FileInfo fileInfo in fileInfos )
						{
								int position = 0;
								string[] tmp = fileInfo.Name.Split( '_' );
								if ( tmp.Length != 0 )
										Int32.TryParse( tmp[0], out position );

								string id = Path.Combine( productImagesPath, fileInfo.Name );
								string url = storagePath + fileInfo.Name;
								string urlThumbnail = storagePath + "_t/" + fileInfo.Name;

								dt.Rows.Add( new object[] { id, url, urlThumbnail, position } );
						}

						//Defaul sort by position
						dt.DefaultView.Sort = "Position ASC";

						return dt;
				}

				/// <summary>
				/// Metóda vymaže existujúci obrázok na danej pozíci z file systému.
				/// </summary>
				private void RemoveExistingProductPhotoByPosition( string productImagesPath, int position )
				{
						//Delete image
						DirectoryInfo di = new DirectoryInfo( productImagesPath );
						FileInfo[] fileInfos = di.GetFiles( string.Format( "{0:0#}_*.*", position ) );
						foreach ( FileInfo fileInfo in fileInfos )
								fileInfo.Delete();

						//Delete thumbnail
						di = new DirectoryInfo( Path.Combine( productImagesPath, "_t\\" ) );
						fileInfos = di.GetFiles( string.Format( "{0:0#}_*.*", position ) );
						foreach ( FileInfo fileInfo in fileInfos )
								fileInfo.Delete();
				}
				/// <summary>
				/// Metóda Uploadne/Nahradí fotografiu produktu pre daný produkt.
				/// V pripade, že už sa fotografia na tomto poradi nachádza, nahradí sa.
				/// </summary>
				private void UpdateProductPhotos( ProductEntity product, FileCollectionEventArgs mfuArgs )
				{
						if ( product == null || mfuArgs == null )
								return;

						if ( mfuArgs.PostedFilesInfo.Count == 0 )
								return;

						string productImagesPath = Path.Combine( Server.MapPath( ConfigValue( "SHP:ImageGallery:Product:StoragePath" ) ),
								product.Id.ToString() );

						string productImagesThumbnailPath = Path.Combine( productImagesPath, "_t" );

						if ( !Directory.Exists( productImagesPath ) )
								Directory.CreateDirectory( productImagesPath );

						if ( !Directory.Exists( productImagesThumbnailPath ) )
								Directory.CreateDirectory( productImagesThumbnailPath );

						foreach ( PostedFileInfo fi in mfuArgs.PostedFilesInfo )
						{
								string desc = fi.Description;

								//Delete existing Product photo on position from file system.
								RemoveExistingProductPhotoByPosition( productImagesPath, fi.Positon );

								string fileName = string.Format( "{0:0#}_{1}", fi.Positon, Path.GetFileName( fi.File.FileName ) );
								string filePath = Path.Combine( productImagesPath, fileName );
								string filePathThumbnail = Path.Combine( productImagesThumbnailPath, fileName );

								//Read input stream
								Stream stream = fi.File.InputStream;
								int len = (int)stream.Length;
								if ( len == 0 ) return;
								byte[] data = new byte[len];
								stream.Read( data, 0, len );
								stream.Flush();
								stream.Close();

								//Write new product photo.
								using ( FileStream fs = new FileStream( filePath, FileMode.Create, FileAccess.Write ) )
										fs.Write( data, 0, len );

								//Write Thumbnail photo
								int w = 0;
								int h = 0;
								using ( System.Drawing.Image img = System.Drawing.Image.FromFile( filePath ) )
								{
										w = img.Width;
										h = img.Height;
								}

								int newWidth = w;
								int newHeight = h;
								ImageGalleryControl.RecalculateImageSize( w, h, ImageGalleryControl.IMAGE_WIDTH * 2, ImageGalleryControl.IMAGE_HEIGHT * 2, ref newWidth, ref newHeight );
								ImageGalleryControl.ResizeImage( filePath, filePathThumbnail, newWidth, newHeight );
						}
				}
				#endregion

				void OnSave( object sender, EventArgs e )
				{
						int storageCount = 0;
						Int32.TryParse( this.txtStorageCount.Text, out storageCount );

						this.product.Manufacturer = this.txtManufacturer.Text;
						this.product.Name = this.txtName.Text;
						this.product.Code = this.txtCode.Text;
						this.product.Description = this.txtDescription.Text;
						this.product.DescriptionLong = this.edtDescription.Editor.Content;
						this.product.Price = Convert.ToDecimal( this.txtPrice.Text );
						this.product.VATId = Convert.ToInt32( this.ddlVAT.SelectedValue );
						this.product.DiscountTypeId = (ProductEntity.DiscountType)Convert.ToInt32( this.ddlDiscountType.SelectedValue );
						this.product.Discount = Convert.ToDecimal( this.txtDiscount.Text );
						this.product.Availability = this.txtAvailability.Text;
						this.product.StorageCount = storageCount == 0 ? null : (int?)storageCount;
						this.product.Locale = Security.Account.Locale;

						//Create product catgories
						this.product.ProductCategories.Clear();
						foreach ( TreeNode node in this.tvCategories.CheckedNodes )
								this.product.ProductCategories.Add( new SHP.Entities.ProductCategories { CategoryId = Convert.ToInt32( node.Value ) } );

						if ( !this.ProductId.HasValue )
								this.product = Storage<ProductEntity>.Create( this.product );
						else Storage<ProductEntity>.Update( this.product );

						#region Vytvorenie URLAliasu
						string alias = this.txtUrlAlis.GetUrlAlias( string.Format( "~/{0}", this.product.Name ) );
						if ( !CMS.Utilities.AliasUtilities.CreateUrlAlias<ProductEntity>( this.Page, this.DisplayUrlFormat, this.product.Name, alias, this.product, Storage<ProductEntity>.Instance ) )
								return;

						//Vytvorenie URL Aliasu pre komentare
						if ( !string.IsNullOrEmpty( this.CommentsFormatUrl ) )
						{
								string urlComment = string.Format( this.CommentsFormatUrl, this.product.Id );
								string aliasComment = string.Format( "{0}/{1}", alias, Resources.Controls.Comment_AliasText );
								if ( !CMS.Utilities.AliasUtilities.CreateUrlAlias( this.Page, urlComment, this.product.Name, aliasComment ) )
										return;
						}
						#endregion

						//Save product Highlights
						this.highlightsControl.ProductId = this.product.Id;
						this.highlightsControl.Save();

						//Update car photos
						UpdateProductPhotos( this.product, this.mfuPhotos.GetUploadEventArgs() );

						//Save Product Relation
						this.alternateProductsControl.ProductId = this.product.Id;
						this.alternateProductsControl.Save();
						this.relatedProductsControl.ProductId = this.product.Id;
						this.relatedProductsControl.Save();

						//Save product Reviews
						this.reviewsControl.ProductId = this.product.Id;
						this.reviewsControl.Save();

						if ( !string.IsNullOrEmpty( this.ReturnUrl ) )
								Response.Redirect( this.ReturnUrl );
						else
						{
								this.imgGalery.DataSource = CreateImageGaleryDatasource( this.product );
								this.imgGalery.DataBind();
						}
				}

				void OnCancel( object sender, EventArgs e )
				{
						Response.Redirect( this.ReturnUrl );
				}

				#region RadPanelBar ItemTemplates
				public class RadPanelBarItemTemplate: ITemplate
				{
						private Control ctrl = null;
						public RadPanelBarItemTemplate( Control ctrl )
						{
								this.ctrl = ctrl;
						}

						#region ITemplate Members

						public void InstantiateIn( Control container )
						{
								container.Controls.Add( this.ctrl );
						}

						#endregion
				}
				#endregion
		}
}
