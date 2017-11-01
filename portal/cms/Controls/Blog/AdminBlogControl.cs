using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlogEntity = CMS.Entities.Blog;
using BlogTagEntity = CMS.Entities.BlogTag;
using CMSEditor = CMS.Controls.RadEditor.RadEditor;
using RoleEntity = CMS.Entities.Role;
using System.Text;
using CMS.Utilities;

namespace CMS.Controls.Blog
{
		public class AdminBlogControl: CmsControl
		{
				private TextBox txtTitle = null;
				private TextBox txtTeaser = null;
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

				private BlogEntity blog = null;

				public AdminBlogControl()
				{
				}

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? BlogId
				{
						get
						{
								object o = ViewState["BlogId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["BlogId"] = value; }
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
						//Povolene len pre prihlaseneho pouzivatela
						if ( !Security.IsLogged( true ) ) return;

						base.CreateChildControls();

						Control blogControl = CreateDetailControl();
						if ( blogControl != null )
								this.Controls.Add( blogControl );

						//Priradenie id fieldu z ktoreho sa generuje alias
						this.txtUrlAlis.FieldID = this.txtTitle.ClientID;
						this.txtUrlAlis.UrlAliasPrefixId = this.UrlAliasPrefixId;

						//Binding
						if ( !this.BlogId.HasValue ) this.blog = new CMS.Entities.Blog();
						else
								this.blog = Storage<BlogEntity>.ReadFirst( new BlogEntity.ReadById { BlogId = this.BlogId.Value } );

						if ( !IsPostBack )
						{
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
								if ( this.ddlRole.Items.Count != 0 && this.blog.RoleId != 0 )
										this.ddlRole.SelectedValue = this.blog.RoleId.ToString();

								this.dtpReleaseDate.Value = this.blog.ReleaseDate;
								this.txtTitle.Text = this.blog.Title;
								this.txtTeaser.Text = this.blog.Teaser;
								this.edtContent.Content = this.blog.Content;

								this.txtCountry.Text = this.blog.Country;
								this.txtCity.Text = this.blog.City;
								this.txtTags.Text = this.GetTagsString( this.blog );
								this.cbEnableComments.Checked = this.blog.EnableComments;
								this.cbApproved.Checked = this.blog.Approved;
								this.cbVisible.Checked = this.blog.Visible;

								//Nastavenie controlsu pre UrlAlias
								this.txtUrlAlis.AutoCompletteAlias = !this.BlogId.HasValue;
								this.txtUrlAlis.Text = this.blog.Alias.StartsWith( "~" ) ? this.blog.Alias.Remove( 0, 1 ) : this.blog.Alias;
						}
				}

				private string GetTagsString( BlogEntity blog )
				{
						StringBuilder sb = new StringBuilder();
						foreach ( BlogTagEntity at in blog.BlogTags )
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
						this.txtTitle = new TextBox();
						this.txtTitle.ID = "txtTitle";
						this.txtTitle.Width = Unit.Percentage( 100 );

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

						table.Rows.Add( CreateTableRow( Resources.Controls.AdminBlogControl_ReleaseDate, this.dtpReleaseDate, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminBlogControl_ExpireDate, this.dtpExpireDate, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminBlogControl_Role, this.ddlRole, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminBlogControl_Country, this.txtCountry, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminBlogControl_City, this.txtCity, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminBlogControl_Tags, this.txtTags, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminBlogControl_Title, this.txtTitle, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminBlogControl_Teaser, this.txtTeaser, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminBlogControl_Content, this.edtContent, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminBlogControl_EnableComments, this.cbEnableComments, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminBlogControl_Approved, this.cbApproved, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminBlogControl_Visible, this.cbVisible, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminBlogControl_UrlAlias, this.txtUrlAlis, true ) );

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
						this.blog.ReleaseDate = Convert.ToDateTime( this.dtpReleaseDate.Value );
						if ( !this.dtpExpireDate.IsNullDate )
								this.blog.ExpiredDate = Convert.ToDateTime( this.dtpExpireDate.Value );

						if ( !this.BlogId.HasValue ) this.blog.AccountId = Security.Account.Id;
						this.blog.Title = this.txtTitle.Text;
						this.blog.Teaser = this.txtTeaser.Text;
						this.blog.Content = this.edtContent.Content;
						this.blog.ContentKeywords = StringUtilities.RemoveDiacritics(this.edtContent.Text);
						this.blog.Locale = Security.Account.Locale;

						this.blog.Country = this.txtCountry.Text;
						this.blog.City = this.txtCity.Text;
						this.blog.EnableComments = this.cbEnableComments.Checked;
						this.blog.Visible = this.cbVisible.Checked;
						this.blog.Approved = this.cbApproved.Checked;

						//Tags
						this.blog.BlogTags.Clear();
						string tagString = this.txtTags.Text.Replace( ",", ";" );
						string[] tags = tagString.Split( ';' );
						foreach ( string t in tags )
						{
								BlogTagEntity at = new BlogTagEntity();
								at.Name = t.Trim();

								this.blog.BlogTags.Add( at );
						}

						this.blog.RoleId = string.IsNullOrEmpty( this.ddlRole.SelectedValue ) ? 0 : Convert.ToInt32( this.ddlRole.SelectedValue );
						if ( this.blog.RoleId == 0 ) this.blog.RoleId = null;

						if ( !this.BlogId.HasValue ) Storage<BlogEntity>.Create( this.blog );
						else Storage<BlogEntity>.Update( this.blog );

						#region Vytvorenie URLAliasu
						string alias = this.txtUrlAlis.GetUrlAlias( string.Format( "~/{0}", this.blog.Title ) );
						if ( !Utilities.AliasUtilities.CreateUrlAlias<BlogEntity>( this.Page, this.DisplayUrlFormat, this.blog.Title, alias, this.blog ) )
								return;

						//Vytvorenie URL Aliasu pre komentare
						if ( !string.IsNullOrEmpty( this.CommentsFormatUrl ) )
						{
								string urlComment = string.Format( this.CommentsFormatUrl, this.blog.Id );
								string aliasComment = string.Format( "{0}/{1}", alias, Resources.Controls.Comment_AliasText );
								if ( !Utilities.AliasUtilities.CreateUrlAlias( this.Page, urlComment, this.blog.Title, aliasComment ) )
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
