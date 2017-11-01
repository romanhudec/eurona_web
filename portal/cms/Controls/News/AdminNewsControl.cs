using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMSEditor = CMS.Controls.RadEditor.RadEditor;
using NewsEntity = CMS.Entities.News;
using CMS.Utilities;
using System.IO;

namespace CMS.Controls.News
{
		public class AdminNewsControl: CmsControl
		{
				protected FileUpload iconUpload = null;
				protected Image icon = null;
				protected Button iconRemove = null;

				private TextBox txtTitle = null;
				private TextBox txtTeaser = null;
				private ASPxDatePicker dtpDate = null;
				private CMSEditor edtContent;
				private CMS.Controls.ASPxUrlAliasTextBox txtUrlAlis;

				private Button btnSave = null;
				private Button btnCancel = null;

				private NewsEntity news = null;

				public AdminNewsControl()
				{
				}

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? NewsId
				{
						get
						{
								object o = ViewState["NewsId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["NewsId"] = value; }
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

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						Control pollControl = CreateDetailControl();
						if ( pollControl != null )
								this.Controls.Add( pollControl );

						//Priradenie id fieldu z ktoreho sa generuje alias
						this.txtUrlAlis.FieldID = this.txtTitle.ClientID;
						this.txtUrlAlis.UrlAliasPrefixId = this.UrlAliasPrefixId;

						//Binding
						if ( !this.NewsId.HasValue ) this.news = new CMS.Entities.News();
						else this.news = Storage<NewsEntity>.ReadFirst( new NewsEntity.ReadById { NewsId = this.NewsId.Value } );

						//Nastavenie zobrazenia icony
						if ( string.IsNullOrEmpty( this.news.Icon ) ) { this.iconRemove.Visible = false; this.icon.Visible = false; }
						else this.iconUpload.Visible = false;

						if ( !IsPostBack )
						{
								this.icon.ImageUrl = this.news.Icon != null ? Page.ResolveUrl( this.news.Icon ) : string.Empty;
								this.icon.Style.Add( "max-width", ImageGalleryControl.IMAGE_WIDTH.ToString() + "px" );
								this.icon.Style.Add( "max-height", ImageGalleryControl.IMAGE_WIDTH.ToString() + "px" );

								this.dtpDate.Value = this.news.Date;
								this.txtTitle.Text = this.news.Title;
								this.txtTeaser.Text = this.news.Teaser;
								this.edtContent.Content = this.news.Content;

								//Nastavenie controlsu pre UrlAlias
								this.txtUrlAlis.AutoCompletteAlias = !this.NewsId.HasValue;
								this.txtUrlAlis.Text = this.news.Alias.StartsWith( "~" ) ? this.news.Alias.Remove( 0, 1 ) : this.news.Alias;
								this.DataBind();
						}

				}
				#endregion

				/// <summary>
				/// Vytvori Control Novinky
				/// </summary>
				private Control CreateDetailControl()
				{
						this.icon = new Image();
						this.icon.ID = "icon";
						this.iconUpload = new FileUpload();
						this.iconUpload.ID = "iconUpload";
						this.iconRemove = new Button();
						this.iconRemove.Text = Resources.Controls.AdminNewsControl__RemoveIcon;
						this.iconRemove.ID = "iconRemove";
						this.iconRemove.Click += ( s, e ) =>
						{
								if ( this.news == null ) return;
								this.RemoveIcon( this.news );
								this.icon.ImageUrl = string.Empty;

								//Nastavenie viditelnosti
								this.iconRemove.Visible = false;
								this.icon.Visible = false;
								this.iconUpload.Visible = true;
						};

						this.txtTitle = new TextBox();
						this.txtTitle.ID = "txtTitle";
						this.txtTitle.Width = Unit.Percentage( 100 );

						this.dtpDate = new ASPxDatePicker();
						this.dtpDate.ID = "dtpDate";

						this.txtTeaser = new TextBox();
						this.txtTeaser.ID = "txtTeaser";
						this.txtTeaser.TextMode = TextBoxMode.MultiLine;
						this.txtTeaser.Width = Unit.Percentage( 100 );
						this.txtTeaser.Height = Unit.Pixel( 200 );

						this.edtContent = new CMSEditor();
						this.edtContent.ID = "edtContent";

						this.txtUrlAlis = new ASPxUrlAliasTextBox();
						this.txtUrlAlis.ID = "txtUrlAlis";
						this.txtUrlAlis.Width = Unit.Percentage( 100 );

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

						#region Icon
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = "form_label";
						cell.Text = Resources.Controls.AdminNewsControl_Icon;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.VerticalAlign = VerticalAlign.Middle;
						cell.Controls.Add( this.icon );
						cell.Controls.Add( this.iconRemove );
						cell.Controls.Add( this.iconUpload );
						row.Cells.Add( cell );
						table.Rows.Add( row );
						#endregion

						table.Rows.Add( CreateTableRow( Resources.Controls.AdminNewsControl_Date, this.dtpDate, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminNewsControl_Title, this.txtTitle, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminNewsControl_Description, this.txtTeaser, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminNewsControl_Content, this.edtContent, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminNewsControl_UrlAlias, this.txtUrlAlis, true ) );

						//Save Cancel Buttons
						row = new TableRow();
						cell = new TableCell();
						cell.ColumnSpan = 2;
						cell.Controls.Add( this.btnSave );
						cell.Controls.Add( this.btnCancel );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						return table;
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

				protected void RemoveIcon( NewsEntity entity )
				{
						string filePath = Server.MapPath( entity.Icon );
						if ( File.Exists( filePath ) ) File.Delete( filePath );

						entity.Icon = null;
						Storage<NewsEntity>.Update( entity );
				}

				protected string UploadIcon( NewsEntity entity )
				{
						if ( !this.iconUpload.HasFile ) return entity.Icon;

						string storagePath = CMS.Utilities.ConfigUtilities.GetEntityIconStoragePath( news.GetType() );
						string filePath = Server.MapPath( news.Icon );
						if ( File.Exists( filePath ) ) File.Delete( filePath );

						string storageDirectoty = Server.MapPath( storagePath );
						if ( !Directory.Exists( storageDirectoty ) ) Directory.CreateDirectory( storageDirectoty );
						string dstFileName = string.Format( "{0}.png", CMS.Utilities.StringUtilities.RemoveDiacritics( news.Title ) );
						dstFileName = dstFileName.Replace( ":", "-" ); dstFileName = dstFileName.Replace( "&", "" );
						string dstFilePath = Path.Combine( storageDirectoty, dstFileName );

						//Zapis suboru
						Stream stream = this.iconUpload.PostedFile.InputStream;
						int len = (int)stream.Length;
						if ( len == 0 ) return null;
						byte[] data = new byte[len];
						stream.Read( data, 0, len );
						stream.Close();
						using ( FileStream fs = new FileStream( dstFilePath, FileMode.Create, FileAccess.Write ) )
						{
								fs.Write( data, 0, len );
						}

						return string.Format( "{0}{1}", storagePath, dstFileName );
				}

				void OnSave( object sender, EventArgs e )
				{
						this.news.Date = Convert.ToDateTime( this.dtpDate.Value );
						this.news.Title = this.txtTitle.Text;
						this.news.Icon = this.UploadIcon( this.news );
						this.news.Teaser = this.txtTeaser.Text;
						this.news.Content = this.edtContent.Content;
						this.news.ContentKeywords = StringUtilities.RemoveDiacritics( this.edtContent.Text );
						this.news.Locale = Security.Account.Locale;

						if ( !this.NewsId.HasValue ) Storage<NewsEntity>.Create( this.news );
						else Storage<NewsEntity>.Update( this.news );

						#region Vytvorenie URLAliasu
						string alias = this.txtUrlAlis.GetUrlAlias( string.Format( "~/{0}", this.news.Title ) );
						if ( !Utilities.AliasUtilities.CreateUrlAlias<NewsEntity>( this.Page, this.DisplayUrlFormat, this.news.Title, alias, this.news ) )
								return;
						#endregion

						Response.Redirect( this.ReturnUrl );
				}

				void OnCancel( object sender, EventArgs e )
				{
						Response.Redirect( this.ReturnUrl );
				}
		}
}
