using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ForumEntity = CMS.Entities.Forum;
using ForumThreadEntity = CMS.Entities.ForumThread;
using CMSEditor = CMS.Controls.RadEditor.RadEditor;
using System.Text;
using CMS.Utilities;
using System.IO;

namespace CMS.Controls.Forum
{
		public class AdminForumControl: CmsControl
		{
				protected FileUpload iconUpload = null;
				protected Image icon = null;
				protected Button iconRemove = null;

				private TextBox txtName = null;
				private TextBox txtDescription = null;
				private CheckBox cbPinned = null;
				private CheckBox cbLocked = null;
				private CMS.Controls.ASPxUrlAliasTextBox txtUrlAlis;

				private Button btnSave = null;
				private Button btnCancel = null;

				private ForumEntity Forum = null;

				public AdminForumControl()
				{
				}

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? ForumId
				{
						get
						{
								object o = ViewState["ForumId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["ForumId"] = value; }
				}
				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? ForumThreadId
				{
						get
						{
								object o = ViewState["ForumThreadId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["ForumThreadId"] = value; }
				}

				public string DisplayUrlFormat { get; set; }

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();
						
						//Get data
						if ( !this.ForumId.HasValue )
						{
								if ( !this.ForumThreadId.HasValue ) throw new InvalidOperationException( "If create new Forum, Property ForumThreadId must be set!!" );
								this.Forum = new CMS.Entities.Forum();
								this.Forum.ForumThreadId = this.ForumThreadId.Value;
						}
						else this.Forum = Storage<ForumEntity>.ReadFirst( new ForumEntity.ReadById { ForumId = this.ForumId.Value } );
						ForumThreadEntity forumThread = Storage<ForumThreadEntity>.ReadFirst( new ForumThreadEntity.ReadById{ ForumThreadId = this.Forum.ForumThreadId });

						Label lblForumThreadName = new Label();
						lblForumThreadName.Font.Bold = true;
						lblForumThreadName.Text = forumThread.Name;
						this.Controls.Add( lblForumThreadName );

						this.Controls.Add( new LiteralControl("<div><br/></div>") );

						Control ForumControl = CreateDetailControl();
						if ( ForumControl != null )
								this.Controls.Add( ForumControl );

						//Priradenie id fieldu z ktoreho sa generuje alias
						this.txtUrlAlis.FieldID = this.txtName.ClientID;
						//this.txtUrlAlis.UrlAliasPrefixId = this.UrlAliasPrefixId;

						//Nastavenie zobrazenia icony
						if ( string.IsNullOrEmpty( this.Forum.Icon ) ) { this.iconRemove.Visible = false; this.icon.Visible = false; }
						else this.iconUpload.Visible = false;

						//Binding
						if ( !IsPostBack )
						{
								this.icon.ImageUrl = this.Forum.Icon != null ? Page.ResolveUrl( this.Forum.Icon ) : string.Empty;
								this.icon.Style.Add( "max-width", ImageGalleryControl.IMAGE_WIDTH.ToString() + "px" );
								this.icon.Style.Add( "max-height", ImageGalleryControl.IMAGE_WIDTH.ToString() + "px" );

								//Category
								
								this.txtName.Text = this.Forum.Name;
								this.txtDescription.Text = this.Forum.Description;
								this.cbPinned.Checked = this.Forum.Pinned;
								this.cbLocked.Checked = this.Forum.Locked;

								//Nastavenie controlsu pre UrlAlias
								this.txtUrlAlis.AutoCompletteAlias = !this.ForumId.HasValue;
								this.txtUrlAlis.Text = this.Forum.Alias.StartsWith( "~" ) ? this.Forum.Alias.Remove( 0, 1 ) : this.Forum.Alias;
						}
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
						this.iconRemove.Text = Resources.Controls.AdminForumControl_RemoveIcon;
                        this.iconRemove.CssClass = this.CssClass + "_remove";
						this.iconRemove.ID = "iconRemove";
						this.iconRemove.Click += ( s, e ) =>
						{
								if ( this.Forum == null ) return;
								this.RemoveIcon( this.Forum );
								this.icon.ImageUrl = string.Empty;

								//Nastavenie viditelnosti
								this.iconRemove.Visible = false;
								this.icon.Visible = false;
								this.iconUpload.Visible = true;
						};

						this.txtName = new TextBox();
						this.txtName.ID = "txtName";
						this.txtName.Width = Unit.Percentage( 100 );

						this.txtDescription = new TextBox();
						this.txtDescription.ID = "txtDescription";
						this.txtDescription.TextMode = TextBoxMode.MultiLine;
						this.txtDescription.Width = Unit.Percentage( 100 );
						this.txtDescription.Height = Unit.Pixel( 100 );

						this.cbPinned = new CheckBox();
						this.cbPinned.ID = "cbPinned";
						this.cbPinned.Checked = false;

						this.cbLocked = new CheckBox();
						this.cbLocked.ID = "cbLocked";
						this.cbLocked.Checked = true;

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
						cell.Text = Resources.Controls.AdminForumControl_Icon;
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

						table.Rows.Add( CreateTableRow( Resources.Controls.AdminForumControl_Name, this.txtName, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminForumControl_Description, this.txtDescription, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminForumControl_Pinned, this.cbPinned, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminForumControl_Locked, this.cbLocked, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminForumControl_UrlAlias, this.txtUrlAlis, true ) );

						//Save Cancel Buttons
						row = new TableRow();
						cell = new TableCell();
						cell.ColumnSpan = 2;
						cell.Controls.Add( this.btnSave );
                        cell.Controls.Add(new LiteralControl
                        {
                            Text = ""
                        });
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

				protected void RemoveIcon( ForumEntity entity )
				{
						string filePath = Server.MapPath( entity.Icon );
						if ( File.Exists( filePath ) ) File.Delete( filePath );

						entity.Icon = null;
						Storage<ForumEntity>.Update( entity );
				}

				protected string UploadIcon( ForumEntity entity )
				{
						if ( !this.iconUpload.HasFile ) return entity.Icon;

						string storagePath = CMS.Utilities.ConfigUtilities.GetEntityIconStoragePath( Forum.GetType() );
						string filePath = Server.MapPath( Forum.Icon );
						if ( File.Exists( filePath ) ) File.Delete( filePath );

						string storageDirectoty = Server.MapPath( storagePath );
						if ( !Directory.Exists( storageDirectoty ) ) Directory.CreateDirectory( storageDirectoty );
						string dstFileName = string.Format( "{0}.png", CMS.Utilities.StringUtilities.RemoveDiacritics( Forum.Name ) );
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
						this.Forum.Name = this.txtName.Text;
						this.Forum.Icon = this.UploadIcon( this.Forum );
						this.Forum.Description = this.txtDescription.Text;
						this.Forum.Locked = this.cbLocked.Checked;
						this.Forum.Pinned = this.cbPinned.Checked;

						if ( !this.ForumId.HasValue ) Storage<ForumEntity>.Create( this.Forum );
						else Storage<ForumEntity>.Update( this.Forum );

						#region Vytvorenie URLAliasu
						ForumThreadEntity forumThread = Storage<ForumThreadEntity>.ReadFirst( new ForumThreadEntity.ReadById { ForumThreadId = this.Forum.ForumThreadId } );
						string alias = string.Format( "{0}/{1}", forumThread.Alias, Utilities.AliasUtilities.GetAliasString( this.Forum.Name ) );
						if ( !Utilities.AliasUtilities.CreateUrlAlias<ForumEntity>( this.Page, this.DisplayUrlFormat, this.Forum.Name, alias, this.Forum ) )
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
