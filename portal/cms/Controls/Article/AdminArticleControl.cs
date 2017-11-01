using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ArticleEntity = CMS.Entities.Article;
using ArticleTagEntity = CMS.Entities.ArticleTag;
using CMSEditor = CMS.Controls.RadEditor.RadEditor;
using RoleEntity = CMS.Entities.Role;
using System.Text;
using CMS.Utilities;
using System.IO;

namespace CMS.Controls.Article
{
		public class AdminArticleControl: CmsControl
		{
				protected FileUpload iconUpload = null;
				protected Image icon = null;
				protected Button iconRemove = null;

				private TextBox txtTitle = null;
				private TextBox txtTeaser = null;
				private DropDownList ddlCategory = null;
				private DropDownList ddlRole = null;
				private ASPxDatePicker dtpReleaseDate = null;
				private ASPxDatePicker dtpExpireDate = null;
				private CMSEditor edtContent;
				private TextBox txtCountry = null;
				private TextBox txtCity = null;
				private TextBox txtTags = null;
				private CheckBox cbEnableComments = null;
				private CheckBox cbApproved = null;
				private CheckBox cbVisible = null;
				private CMS.Controls.ASPxUrlAliasTextBox txtUrlAlis;

				private Button btnSave = null;
				private Button btnCancel = null;

				private ArticleEntity article = null;

				public AdminArticleControl()
				{
				}

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? ArticleId
				{
						get
						{
								object o = ViewState["ArticleId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["ArticleId"] = value; }
				}
				#region IURLAlias
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
				#endregion

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						Control articleControl = CreateDetailControl();
						if ( articleControl != null )
								this.Controls.Add( articleControl );

						//Priradenie id fieldu z ktoreho sa generuje alias
						this.txtUrlAlis.FieldID = this.txtTitle.ClientID;
						this.txtUrlAlis.UrlAliasPrefixId = this.UrlAliasPrefixId;

						//Binding
						if ( !this.ArticleId.HasValue ) this.article = new CMS.Entities.Article();
						else this.article = Storage<ArticleEntity>.ReadFirst( new ArticleEntity.ReadById { ArticleId = this.ArticleId.Value } );

						//Nastavenie zobrazenia icony
						if ( string.IsNullOrEmpty( this.article.Icon ) ) { this.iconRemove.Visible = false; this.icon.Visible = false; }
						else this.iconUpload.Visible = false;

						if ( !IsPostBack )
						{
								this.icon.ImageUrl = this.article.Icon != null ? Page.ResolveUrl( this.article.Icon ) : string.Empty;
								this.icon.Style.Add( "max-width", ImageGalleryControl.IMAGE_WIDTH.ToString() + "px" );
								this.icon.Style.Add( "max-height", ImageGalleryControl.IMAGE_WIDTH.ToString() + "px" );

								//Category
								this.ddlCategory.DataSource = Storage<CMS.Entities.Classifiers.ArticleCategory>.Read();
								this.ddlCategory.DataTextField = "Name";
								this.ddlCategory.DataValueField = "Id";
								this.ddlCategory.DataBind();

								//Role
								List<RoleEntity> roles = Storage<RoleEntity>.Read();
								roles.Add( new RoleEntity { Id = 0, Name = "" } );
								roles = roles.OrderBy( p => p.Name ).ToList();
								this.ddlRole.DataSource = roles;
								this.ddlRole.DataTextField = "Name";
								this.ddlRole.DataValueField = "Id";
								this.ddlRole.DataBind();

								//Systemove role su podfarbene
								foreach ( ListItem li in ddlRole.Items )
								{
										RoleEntity role = roles.FirstOrDefault( p => p.Id.ToString() == li.Value );
										if ( role == default( RoleEntity ) ) continue;
										if ( role.Id >= 0 ) continue;
										li.Attributes.Add( "style", "font-weight: bold; background-color: silver;" );
								}

								if ( this.ddlRole.Items.Count != 0 && this.article.RoleId != 0 )
										this.ddlRole.SelectedValue = this.article.RoleId.ToString();

								if ( this.ddlCategory.Items.Count != 0 && this.article.ArticleCategoryId != 0 )
										this.ddlCategory.SelectedValue = this.article.ArticleCategoryId.ToString();

								this.dtpReleaseDate.Value = this.article.ReleaseDate;
								this.txtTitle.Text = this.article.Title;
								this.txtTeaser.Text = this.article.Teaser;
								this.edtContent.Content = this.article.Content;

								this.txtCountry.Text = this.article.Country;
								this.txtCity.Text = this.article.City;
								this.txtTags.Text = this.GetTagsString( this.article );
								this.cbEnableComments.Checked = this.article.EnableComments;
								this.cbApproved.Checked = this.article.Approved;
								this.cbVisible.Checked = this.article.Visible;

								//Nastavenie controlsu pre UrlAlias
								this.txtUrlAlis.AutoCompletteAlias = !this.ArticleId.HasValue;
								this.txtUrlAlis.Text = this.article.Alias.StartsWith( "~" ) ? this.article.Alias.Remove( 0, 1 ) : this.article.Alias;
						}
				}

				private string GetTagsString( ArticleEntity article )
				{
						StringBuilder sb = new StringBuilder();
						foreach ( ArticleTagEntity at in article.ArticleTags )
						{
								if ( sb.Length != 0 ) sb.Append( ";" );
								sb.Append( at.Name );
						}

						return sb.ToString();
				}
				#endregion

				/// <summary>
				/// Vytvori Control Clanku
				/// </summary>
				private Control CreateDetailControl()
				{
						this.icon = new Image();
						this.icon.ID = "icon";
						this.iconUpload = new FileUpload();
						this.iconUpload.ID = "iconUpload";
						this.iconRemove = new Button();
						this.iconRemove.Text = Resources.Controls.AdminArticleControl__RemoveIcon;
						this.iconRemove.ID = "iconRemove";
						this.iconRemove.Click += ( s, e ) =>
						{
								if ( this.article == null ) return;
								this.RemoveIcon( this.article );
								this.icon.ImageUrl = string.Empty;

								//Nastavenie viditelnosti
								this.iconRemove.Visible = false;
								this.icon.Visible = false;
								this.iconUpload.Visible = true;
						};

						this.txtTitle = new TextBox();
						this.txtTitle.ID = "txtTitle";
						this.txtTitle.Width = Unit.Percentage( 100 );

						this.ddlCategory = new DropDownList();
						this.ddlCategory.ID = "ddlCategory";
						this.ddlCategory.Width = Unit.Percentage( 100 );

						this.ddlRole = new DropDownList();
						this.ddlRole.ID = "dlRole";
						this.ddlRole.Width = Unit.Percentage( 100 );

						this.dtpReleaseDate = new ASPxDatePicker();
						this.dtpReleaseDate.ID = "dtpReleaseDate";

						this.dtpExpireDate = new ASPxDatePicker();
						this.dtpExpireDate.ID = "dtpExpireDate";

						this.txtTeaser = new TextBox();
						this.txtTeaser.ID = "txtTeaser";
						this.txtTeaser.TextMode = TextBoxMode.MultiLine;
						this.txtTeaser.Width = Unit.Percentage( 100 );
						this.txtTeaser.Height = Unit.Pixel( 100 );

						this.edtContent = new CMSEditor();
						this.edtContent.ID = "edtContent";

						this.txtCountry = new TextBox();
						this.txtCountry.ID = "txtCountry";
						this.txtCountry.Width = Unit.Percentage( 100 );

						this.txtCity = new TextBox();
						this.txtCity.ID = "txtCity";
						this.txtCity.Width = Unit.Percentage( 100 );

						this.txtTags = new TextBox();
						this.txtTags.ID = "txtTags";
						this.txtTags.TextMode = TextBoxMode.SingleLine;
						this.txtTags.Width = Unit.Percentage( 100 );

						this.cbEnableComments = new CheckBox();
						this.cbEnableComments.ID = "cbEnableComments";
						this.cbEnableComments.Checked = true;

						this.cbApproved = new CheckBox();
						this.cbApproved.ID = "cbApproved";
						this.cbApproved.Checked = false;

						this.cbVisible = new CheckBox();
						this.cbVisible.ID = "cbVisible";
						this.cbVisible.Checked = true;

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
						cell.Text = Resources.Controls.AdminArticleControl_Icon;
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

						table.Rows.Add( CreateTableRow( Resources.Controls.AdminArticleControl_ReleaseDate, this.dtpReleaseDate, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminArticleControl_ExpireDate, this.dtpExpireDate, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminArticleControl_Category, this.ddlCategory, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminArticleControl_Role, this.ddlRole, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminArticleControl_Country, this.txtCountry, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminArticleControl_City, this.txtCity, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminArticleControl_Tags, this.txtTags, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminArticleControl_Title, this.txtTitle, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminArticleControl_Teaser, this.txtTeaser, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminArticleControl_Content, this.edtContent, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminArticleControl_EnableComments, this.cbEnableComments, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminArticleControl_Approved, this.cbApproved, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminArticleControl_Visible, this.cbVisible, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminArticleControl_UrlAlias, this.txtUrlAlis, true ) );

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

				protected void RemoveIcon( ArticleEntity entity )
				{
						string filePath = Server.MapPath( entity.Icon );
						if ( File.Exists( filePath ) ) File.Delete( filePath );

						entity.Icon = null;
						Storage<ArticleEntity>.Update( entity );
				}

				protected string UploadIcon( ArticleEntity entity )
				{
						if ( !this.iconUpload.HasFile ) return entity.Icon;

						string storagePath = CMS.Utilities.ConfigUtilities.GetEntityIconStoragePath( article.GetType() );
						string filePath = Server.MapPath( article.Icon );
						if ( File.Exists( filePath ) ) File.Delete( filePath );

						string storageDirectoty = Server.MapPath( storagePath );
						if ( !Directory.Exists( storageDirectoty ) ) Directory.CreateDirectory( storageDirectoty );
						string dstFileName = string.Format( "{0}.png", CMS.Utilities.StringUtilities.RemoveDiacritics( article.Title ) );
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
						this.article.ReleaseDate = Convert.ToDateTime( this.dtpReleaseDate.Value );
						if ( !this.dtpExpireDate.IsNullDate )
								this.article.ExpiredDate = Convert.ToDateTime( this.dtpExpireDate.Value );

						this.article.ArticleCategoryId = Convert.ToInt32( this.ddlCategory.SelectedValue );
						this.article.Title = this.txtTitle.Text;
						this.article.Icon = this.UploadIcon( this.article );
						this.article.Teaser = this.txtTeaser.Text;
						this.article.Content = this.edtContent.Content;
						this.article.ContentKeywords = StringUtilities.RemoveDiacritics( this.edtContent.Text );
						this.article.Locale = Security.Account.Locale;

						this.article.Country = this.txtCountry.Text;
						this.article.City = this.txtCity.Text;
						this.article.EnableComments = this.cbEnableComments.Checked;
						this.article.Visible = this.cbVisible.Checked;
						this.article.Approved = this.cbApproved.Checked;

						//Tags
						this.article.ArticleTags.Clear();
						string tagString = this.txtTags.Text.Replace( ",", ";" );
						string[] tags = tagString.Split( ';' );
						foreach ( string t in tags )
						{
								ArticleTagEntity at = new ArticleTagEntity();
								at.Name = t.Trim();

								this.article.ArticleTags.Add( at );
						}

						this.article.RoleId = string.IsNullOrEmpty( this.ddlRole.SelectedValue ) ? 0 : Convert.ToInt32( this.ddlRole.SelectedValue );
						if ( this.article.RoleId == 0 ) this.article.RoleId = null;

						if ( !this.ArticleId.HasValue ) Storage<ArticleEntity>.Create( this.article );
						else Storage<ArticleEntity>.Update( this.article );

						#region Vytvorenie URLAliasu
						string alias = this.txtUrlAlis.GetUrlAlias( string.Format( "~/{0}", this.article.Title ) );
						if ( !Utilities.AliasUtilities.CreateUrlAlias<ArticleEntity>( this.Page, this.DisplayUrlFormat, this.article.Title, alias, this.article ) )
								return;

						//Vytvorenie URL Aliasu pre komentare
						if ( !string.IsNullOrEmpty( this.CommentsFormatUrl ) )
						{
								string urlComment = string.Format( this.CommentsFormatUrl, this.article.Id );
								string aliasComment = string.Format( "{0}/{1}", alias, Resources.Controls.Comment_AliasText );
								if ( !Utilities.AliasUtilities.CreateUrlAlias( this.Page, urlComment, this.article.Title, aliasComment ) )
										return;
						}
						#endregion

						Response.Redirect( this.ReturnUrl );
				}

				void OnCancel( object sender, EventArgs e )
				{
						Response.Redirect( this.ReturnUrl );
				}
		}
}
