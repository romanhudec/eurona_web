using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using MenuEntity = CMS.Entities.NavigationMenuItem;
using UrlAliasEntity = CMS.Entities.UrlAlias;
using RoleEntity = CMS.Entities.Role;
using System.IO;

namespace CMS.Controls.Menu
{
		public class AdminNavigationMenuItemControl: CmsControl
		{
				private TextBox tbOrder;
				private TextBox tbName;
				private DropDownList ddlRole;
				private DropDownList ddlUrlAlias;

				//Icon support
				protected FileUpload iconUpload = null;
				protected Image icon = null;
				protected Button iconRemove = null;

				private Button btnSave;
				private Button btnCancel;

				private bool isNew = false;
				private MenuEntity menuEntity = null;

				public int? NavigationMenuId
				{
						get
						{
								object o = ViewState["NavigationMenuId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["NavigationMenuId"] = value; }
				}

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						isNew = Request["Id"] == null;
						if ( isNew )
						{
								if ( !this.NavigationMenuId.HasValue )
										throw new InvalidOperationException( "Mus specify query parameter for property 'NavigationMenuId'" );

								menuEntity = new MenuEntity();
								menuEntity.NavigationMenuId = this.NavigationMenuId.Value;
						}
						else
						{
								int menuId = Convert.ToInt32( Request["id"] );
								menuEntity = Storage<MenuEntity>.ReadFirst( new MenuEntity.ReadById { NavigationMenuItemId = menuId } );
						}

						Table table = new Table();
						table.Width = this.Width;

						TableRow trOrder = new TableRow();
						trOrder.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminMenuControl_LabelOrder,
								CssClass = "form_label_required"
						} );
						trOrder.Cells.Add( CreateOrderInput() );
						table.Rows.Add( trOrder );

						TableRow trIcon = CreateIconInput();
						table.Rows.Add( trIcon );

						TableRow trName = new TableRow();
						trName.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminMenuControl_LabelName,
								CssClass = "form_label_required"
						} );
						trName.Cells.Add( CreateNameInput() );
						table.Rows.Add( trName );

						TableRow trRole = new TableRow();
						trRole.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminMenuControl_LabelRole,
								CssClass = "form_label_required"
						} );
						trRole.Cells.Add( CreateRoleList() );
						table.Rows.Add( trRole );

						TableRow trPage = new TableRow();
						trPage.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminMenuControl_LabelAlias,
								CssClass = "form_label_required"
						} );
						trPage.Cells.Add( CreatePageList() );
						table.Rows.Add( trPage );

						CreateSaveButton();
						CreateCancelButton();

						TableRow trButtons = new TableRow();
						TableCell tdButtons = new TableCell();
						tdButtons.ColumnSpan = 2;
						tdButtons.Controls.Add( btnSave );
						tdButtons.Controls.Add( btnCancel );
						trButtons.Cells.Add( tdButtons );
						table.Rows.Add( trButtons );

						Controls.Add( table );

						//Nastavenie zobrazenia icony
						if ( string.IsNullOrEmpty( this.menuEntity.Icon ) ) { this.iconRemove.Visible = false; this.icon.Visible = false; }
						else this.iconUpload.Visible = false;

						if ( !IsPostBack )
						{
								//Icon support
								this.icon.ImageUrl = this.menuEntity.Icon != null ? Page.ResolveUrl( this.menuEntity.Icon ) : string.Empty;
								this.icon.Style.Add( "max-width", ImageGalleryControl.IMAGE_WIDTH.ToString() + "px" );
								this.icon.Style.Add( "max-height", ImageGalleryControl.IMAGE_WIDTH.ToString() + "px" );
						}
				}

				private TableRow CreateIconInput()
				{
						//Icon Support
						this.icon = new Image();
						this.icon.ID = "icon";
						this.iconUpload = new FileUpload();
						this.iconUpload.ID = "iconUpload";
						this.iconRemove = new Button();
						this.iconRemove.Text = Resources.Controls.AdminMenuControl_RemoveIcon;
						this.iconRemove.ID = "iconRemove";
						this.iconRemove.Click += ( s, e ) =>
						{
								if ( this.menuEntity == null ) return;
								this.RemoveIcon( this.menuEntity );
								this.icon.ImageUrl = string.Empty;

								//Nastavenie viditelnosti
								this.iconRemove.Visible = false;
								this.icon.Visible = false;
								this.iconUpload.Visible = true;
						};

						//Icon
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = "form_label";
						cell.Text = Resources.Controls.AdminMenuControl_Icon;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.VerticalAlign = VerticalAlign.Middle;
						cell.Controls.Add( this.icon );
						cell.Controls.Add( this.iconRemove );
						cell.Controls.Add( this.iconUpload );
						row.Cells.Add( cell );
						return row;
				}

				private TableCell CreateOrderInput()
				{
						TableCell cell = new TableCell();
						cell.CssClass = "form_control";
						tbOrder = new TextBox
						{
								ID = "tbOrder",
								Text = menuEntity.Order.HasValue ? menuEntity.Order.ToString() : String.Empty,
								Width = Unit.Percentage( 80 )
						};
						cell.Controls.Add( tbOrder );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( tbOrder.ID ) );
						cell.Controls.Add( CreateNumberValidatorControl( tbOrder.ID ) );
						return cell;
				}

				private TableCell CreateNameInput()
				{
						TableCell cell = new TableCell();
						cell.CssClass = "form_control";
						tbName = new TextBox
						{
								ID = "tbName",
								Text = menuEntity.Name,
								Width = Unit.Percentage( 80 )
						};
						cell.Controls.Add( tbName );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( tbName.ID ) );
						return cell;
				}

				private TableCell CreatePageList()
				{
						List<UrlAliasEntity> urlAliasList = Storage<UrlAliasEntity>.Read().OrderBy( p => p.Alias ).ToList();
						TableCell cell = new TableCell();
						ddlUrlAlias = new DropDownList();
						ddlUrlAlias.Width = Unit.Percentage( 80 );
						ddlUrlAlias.ID = "ddlUrlAlias";
						ddlUrlAlias.DataSource = urlAliasList;
						ddlUrlAlias.DataTextField = "Display";
						ddlUrlAlias.DataValueField = "Id";
						ddlUrlAlias.DataBind();
						foreach ( ListItem li in ddlUrlAlias.Items )
						{
								UrlAliasEntity urlAlias = urlAliasList.FirstOrDefault( p => p.Id.ToString() == li.Value );
								if ( urlAlias == default( UrlAliasEntity ) ) continue;
								if ( String.IsNullOrEmpty( urlAlias.Alias ) ) continue;
								li.Attributes.Add( "style", "font-weight: bold; background-color: silver;" );
						}
						cell.Controls.Add( ddlUrlAlias );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( ddlUrlAlias.ID ) );
						ddlUrlAlias.SelectedValue = menuEntity.UrlAliasId.ToString();
						return cell;
				}

				private TableCell CreateRoleList()
				{
						List<RoleEntity> roles = Storage<RoleEntity>.Read();
						roles.Add( new RoleEntity { Id = 0, Name = String.Empty } );
						roles = roles.OrderBy( p => p.Name ).ToList();
						TableCell cell = new TableCell();
						cell.CssClass = "form_control";
						ddlRole = new DropDownList();
						ddlRole.ID = "ddlRole";
						ddlRole.Width = Unit.Percentage( 80 );
						ddlRole.DataSource = roles;
						ddlRole.DataTextField = "Name";
						ddlRole.DataValueField = "Id";
						ddlRole.DataBind();

						//Systemove role su podfarbene
						foreach ( ListItem li in ddlRole.Items )
						{
								RoleEntity role = roles.FirstOrDefault( p => p.Id.ToString() == li.Value );
								if ( role == default( RoleEntity ) ) continue;
								if ( role.Id >= 0 ) continue;
								li.Attributes.Add( "style", "font-weight: bold; background-color: silver;" );
						}

						cell.Controls.Add( ddlRole );
						ddlRole.SelectedValue = menuEntity.RoleId.HasValue ? menuEntity.RoleId.ToString() : "0";
						return cell;
				}

				#region Icon Support Methods

				protected void RemoveIcon( MenuEntity entity )
				{
						string filePath = Server.MapPath( entity.Icon );
						if ( File.Exists( filePath ) ) File.Delete( filePath );

						entity.Icon = null;
						Storage<MenuEntity>.Update( entity );
				}

				protected string UploadIcon( MenuEntity entity )
				{
						if ( !this.iconUpload.HasFile ) return entity.Icon;

						string storagePath = CMS.Utilities.ConfigUtilities.GetEntityIconStoragePath( entity.GetType() );
						string filePath = Server.MapPath( entity.Icon );
						if ( File.Exists( filePath ) ) File.Delete( filePath );

						string storageDirectoty = Server.MapPath( storagePath );
						if ( !Directory.Exists( storageDirectoty ) ) Directory.CreateDirectory( storageDirectoty );
						string dstFileName = string.Format( "{0}.png", CMS.Utilities.StringUtilities.RemoveDiacritics( entity.Name ) );
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
				#endregion

				private void CreateCancelButton()
				{
						btnCancel = new Button
						{
								Text = Resources.Controls.CancelButton_Text,
								CausesValidation = false
						};
						btnCancel.Click += ( s1, e1 ) => Response.Redirect( this.ReturnUrl );
				}

				private void CreateSaveButton()
				{
						btnSave = new Button
						{
								Text = Resources.Controls.SaveButton_Text
						};
						btnSave.Click += ( s1, e1 ) =>
						{
								menuEntity.Order = Convert.ToInt32( tbOrder.Text );
								menuEntity.Name = tbName.Text;
								menuEntity.Icon = this.UploadIcon( menuEntity );

								menuEntity.RoleId = Convert.ToInt32( ddlRole.SelectedValue );
								if ( menuEntity.RoleId == 0 ) menuEntity.RoleId = null;
								menuEntity.UrlAliasId = Convert.ToInt32( ddlUrlAlias.SelectedValue );

								if ( isNew )
										Storage<MenuEntity>.Create( menuEntity );
								else
										Storage<MenuEntity>.Update( menuEntity );

								Response.Redirect( this.ReturnUrl );
						};
				}
		}
}
