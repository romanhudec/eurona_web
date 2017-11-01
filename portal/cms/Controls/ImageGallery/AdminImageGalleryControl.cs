using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ImageGalleryEntity = CMS.Entities.ImageGallery;
using ImageGalleryTagEntity = CMS.Entities.ImageGalleryTag;
using RoleEntity = CMS.Entities.Role;
using CMSEditor = CMS.Controls.RadEditor.RadEditor;
using System.Text;

namespace CMS.Controls.ImageGallery
{
		public class AdminImageGalleryControl: CmsControl
		{
				private TextBox txtName = null;
				private ASPxDatePicker dtpDate = null;
				//private DropDownList ddlCategory = null;
				private DropDownList ddlRole = null;
				private TextBox txtTags = null;
				private CheckBox cbEnableComments = null;
				private CheckBox cbEnableVotes = null;
				private CheckBox cbVisible = null;
				private CMS.Controls.ASPxUrlAliasTextBox txtUrlAlis;
				private CMSEditor edtDescription;

				private Button btnSave = null;
				private Button btnCancel = null;

				private ImageGalleryEntity imgGallery = null;

				public AdminImageGalleryControl()
				{
				}

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? ImageGalleryId
				{
						get
						{
								object o = ViewState["ImageGalleryId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["ImageGalleryId"] = value; }
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

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						Control articleControl = CreateDetailControl();
						if ( articleControl != null )
								this.Controls.Add( articleControl );

						//Priradenie id fieldu z ktoreho sa generuje alias
						this.txtUrlAlis.FieldID = this.txtName.ClientID;
						this.txtUrlAlis.UrlAliasPrefixId = this.UrlAliasPrefixId;

						//Binding
						if ( !this.ImageGalleryId.HasValue ) this.imgGallery = new CMS.Entities.ImageGallery();
						else
								this.imgGallery = Storage<ImageGalleryEntity>.ReadFirst( new ImageGalleryEntity.ReadById { ImageGalleryId = this.ImageGalleryId.Value } );

						if ( !IsPostBack )
						{
								////Category
								//this.ddlCategory.DataSource = Storage<CMS.Entities.Classifiers.ImageGalleryCategory>.Read();
								//this.ddlCategory.DataTextField = "Name";
								//this.ddlCategory.DataValueField = "Id";
								//this.ddlCategory.DataBind();

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

								if ( this.ddlRole.Items.Count != 0 && this.imgGallery.RoleId != 0 )
										this.ddlRole.SelectedValue = this.imgGallery.RoleId.ToString();

								//if ( this.ddlCategory.Items.Count != 0 && this.imgGallery.ImageGalleryCategoryId != 0 )
								//    this.ddlCategory.SelectedValue = this.imgGallery.ImageGalleryCategoryId.ToString();

								this.txtName.Text = this.imgGallery.Name;
								this.edtDescription.Content = this.imgGallery.Description;
								this.dtpDate.Value = this.imgGallery.Date;
								this.txtTags.Text = this.GetTagsString( this.imgGallery );
								this.cbEnableComments.Checked = this.imgGallery.EnableComments;
								this.cbEnableVotes.Checked = this.imgGallery.EnableVotes;
								this.cbVisible.Checked = this.imgGallery.Visible;

								//Nastavenie controlsu pre UrlAlias
								this.txtUrlAlis.AutoCompletteAlias = !this.ImageGalleryId.HasValue;
								this.txtUrlAlis.Text = this.imgGallery.Alias.StartsWith( "~" ) ? this.imgGallery.Alias.Remove( 0, 1 ) : this.imgGallery.Alias;
						}
				}

				private string GetTagsString( ImageGalleryEntity imgGallery )
				{
						StringBuilder sb = new StringBuilder();
						foreach ( ImageGalleryTagEntity at in imgGallery.ImageGalleryTags )
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
						this.txtName = new TextBox();
						this.txtName.ID = "txtName";
						this.txtName.Width = Unit.Percentage( 100 );

						this.edtDescription = new CMSEditor();
						this.edtDescription.ID = "edtDescription";

						this.dtpDate = new ASPxDatePicker();
						this.dtpDate.ID = "dtpReleaseDate";

						//this.ddlCategory = new DropDownList();
						//this.ddlCategory.ID = "ddlCategory";
						//this.ddlCategory.Width = Unit.Percentage( 100 );

						this.ddlRole = new DropDownList();
						this.ddlRole.ID = "ddlRole";
						this.ddlRole.Width = Unit.Percentage( 100 );

						this.txtTags = new TextBox();
						this.txtTags.ID = "txtTags";
						this.txtTags.TextMode = TextBoxMode.SingleLine;
						this.txtTags.Width = Unit.Percentage( 100 );

						this.cbEnableComments = new CheckBox();
						this.cbEnableComments.ID = "cbEnableComments";
						this.cbEnableComments.Checked = true;

						this.cbEnableVotes = new CheckBox();
						this.cbEnableVotes.ID = "cbEnableVotes";
						this.cbEnableVotes.Checked = false;

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

						table.Rows.Add( CreateTableRow( Resources.Controls.AdminImageGalleryControl_Name, this.txtName, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminImageGalleryControl_Description, this.edtDescription, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminImageGalleryControl_Date, this.dtpDate, true ) );
						//table.Rows.Add( CreateTableRow( Resources.Controls.AdminImageGalleryControl_Category, this.ddlCategory, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminImageGalleryControl_Role, this.ddlRole, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminImageGalleryControl_Tags, this.txtTags, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminImageGalleryControl_EnableComments, this.cbEnableComments, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminImageGalleryControl_EnableVotes, this.cbEnableVotes, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminImageGalleryControl_Visible, this.cbVisible, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminImageGalleryControl_UrlAlias, this.txtUrlAlis, true ) );

						//Save Cancel Buttons
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
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


				void OnSave( object sender, EventArgs e )
				{
						if ( !this.dtpDate.IsNullDate )
								this.imgGallery.Date = Convert.ToDateTime( this.dtpDate.Value );

						//this.imgGallery.ImageGalleryCategoryId = Convert.ToInt32( this.ddlCategory.SelectedValue );
						this.imgGallery.Name = this.txtName.Text;
						this.imgGallery.Description = this.edtDescription.Content;
						this.imgGallery.EnableComments = this.cbEnableComments.Checked;
						this.imgGallery.Visible = this.cbVisible.Checked;
						this.imgGallery.EnableVotes = this.cbEnableVotes.Checked;

						//Tags
						this.imgGallery.ImageGalleryTags.Clear();
						string tagString = this.txtTags.Text.Replace( ",", ";" );
						string[] tags = tagString.Split( ';' );
						foreach ( string t in tags )
						{
								ImageGalleryTagEntity at = new ImageGalleryTagEntity();
								at.Name = t.Trim();

								this.imgGallery.ImageGalleryTags.Add( at );
						}

						this.imgGallery.RoleId = string.IsNullOrEmpty( this.ddlRole.SelectedValue ) ? 0 : Convert.ToInt32( this.ddlRole.SelectedValue );
						if ( this.imgGallery.RoleId == 0 ) this.imgGallery.RoleId = null;

						if ( !this.ImageGalleryId.HasValue ) Storage<ImageGalleryEntity>.Create( this.imgGallery );
						else Storage<ImageGalleryEntity>.Update( this.imgGallery );

						#region Vytvorenie URLAliasu
						string alias = this.txtUrlAlis.GetUrlAlias( string.Format( "~/{0}", this.imgGallery.Name ) );
						if ( !Utilities.AliasUtilities.CreateUrlAlias<ImageGalleryEntity>( this.Page, this.DisplayUrlFormat, this.imgGallery.Name, alias, this.imgGallery ) )
								return;

						//Vytvorenie URL Aliasu pre komentare
						if ( !string.IsNullOrEmpty( this.CommentsFormatUrl ) )
						{
								string urlComment = string.Format( this.CommentsFormatUrl, this.imgGallery.Id );
								string aliasComment = string.Format( "{0}/{1}", alias, Resources.Controls.Comment_AliasText );
								if ( !Utilities.AliasUtilities.CreateUrlAlias( this.Page, urlComment, this.imgGallery.Name, aliasComment ) )
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
