using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PageEntity = CMS.Entities.Page;
using UrlAliasEntity = CMS.Entities.UrlAlias;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using RoleEntity = CMS.Entities.Role;
using MasterPageEntity = CMS.Entities.MasterPage;

namespace CMS.Controls.Page
{
		public class AdminPageControl: CmsControl
		{
				private TextBox tbTitle;
				private TextBox tbName;
				private DropDownList ddlRole;
				private DropDownList ddlMasterPage;
				private CMS.Controls.ASPxUrlAliasTextBox tbUrlAlis;
				private Button btnSave;
				private Button btnCancel;

				private bool isNew = false;
				private PageEntity pageEntity = null;

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

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						isNew = Request["Id"] == null;
						if ( isNew ) pageEntity = new PageEntity();
						else
						{
								int pageId = Convert.ToInt32( Request["id"] );
								pageEntity = Storage<PageEntity>.ReadFirst( new PageEntity.ReadById { PageId = pageId } );
						}

						Table table = new Table();
						table.Width = this.Width;

						TableRow trOrder = new TableRow();
						trOrder.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminPageControl_LabelTitle,
								CssClass = "form_label_required"
						} );
						trOrder.Cells.Add( CreateTitleInput() );
						table.Rows.Add( trOrder );

						TableRow trName = new TableRow();
						trName.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminPageControl_LabelName,
								CssClass = "form_label_required",
						} );
						trName.Cells.Add( CreateNameInput() );
						table.Rows.Add( trName );

						TableRow trMasterPage = new TableRow();
						trMasterPage.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminPageControl_LabelMasterPage,
								CssClass = "form_label_required",
						} );
						trMasterPage.Cells.Add( CreateMasterPageList() );
						table.Rows.Add( trMasterPage );

						TableRow trRole = new TableRow();
						trRole.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminPageControl_LabelRole,
								CssClass = "form_label",
						} );
						trRole.Cells.Add( CreateRoleList() );
						table.Rows.Add( trRole );

						TableRow trUrlAlias = new TableRow();
						trUrlAlias.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminPageControl_LabelUrlAlias,
								CssClass = "form_label_required",
						} );
						trUrlAlias.Cells.Add( CreateUrlAliasInput() );
						table.Rows.Add( trUrlAlias );

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

						this.tbUrlAlis.FieldID = this.tbName.ClientID;
				}

				private TableCell CreateTitleInput()
				{
						TableCell cell = new TableCell();
						cell.Attributes.Add( "class", "form_control" );
						tbTitle = new TextBox
						{
								ID = "tbTitle",
								Text = pageEntity.Title,
								Width = Unit.Percentage( 80 )
						};
						cell.Controls.Add( tbTitle );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( tbTitle.ID ) );
						return cell;
				}

				private TableCell CreateNameInput()
				{
						TableCell cell = new TableCell();
						cell.Attributes.Add( "class", "form_control" );
						tbName = new TextBox
						{
								ID = "tbName",
								Text = pageEntity.Name,
								Width = Unit.Percentage( 80 ),
								Enabled = pageEntity.Id >= 0 //U systemovych stranok sa tento udaj editovat neda
						};
						cell.Controls.Add( tbName );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( tbName.ID ) );
						return cell;
				}

				private TableCell CreateMasterPageList()
				{
						List<MasterPageEntity> masterPages = Storage<MasterPageEntity>.Read();
						masterPages = masterPages.OrderBy( p => p.Id ).ToList();
						TableCell cell = new TableCell();
						ddlMasterPage = new DropDownList();
						ddlMasterPage.ID = "ddlMasterPage";
						ddlMasterPage.Width = Unit.Percentage( 80 );
						ddlMasterPage.DataSource = masterPages;
						ddlMasterPage.DataTextField = "Name";
						ddlMasterPage.DataValueField = "Id";
						ddlMasterPage.DataBind();
						cell.Controls.Add( ddlMasterPage );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( ddlMasterPage.ID ) );
						ddlMasterPage.SelectedValue = pageEntity.MasterPageId.ToString();
						ddlMasterPage.Enabled = pageEntity.Id >= 0; //U systemovych stranok sa tento udaj editovat neda
						return cell;
				}

				private TableCell CreateRoleList()
				{
						List<RoleEntity> roles = Storage<RoleEntity>.Read();
						roles.Add( new RoleEntity { Id = 0, Name = "" } );
						roles = roles.OrderBy( p => p.Name ).ToList();
						TableCell cell = new TableCell();
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

						ddlRole.Enabled = pageEntity.Id >= 0; //U systemovych stranok sa tento udaj editovat neda
						cell.Controls.Add( ddlRole );
						ddlRole.SelectedValue = pageEntity.RoleId.HasValue ? pageEntity.RoleId.ToString() : "0";
						return cell;
				}

				private TableCell CreateUrlAliasInput()
				{
						TableCell cell = new TableCell();
						cell.Attributes.Add( "class", "form_control" );
						tbUrlAlis = new CMS.Controls.ASPxUrlAliasTextBox
						{
								ID = "tbUrlAlis",
								Text = pageEntity.Alias.StartsWith( "~" ) ? pageEntity.Alias.Remove( 0, 1 ) : pageEntity.Alias,
								Width = Unit.Percentage( 80 ),
								FieldID = this.tbName.ClientID,
								AutoCompletteAlias = this.isNew
						};
						cell.Controls.Add( tbUrlAlis );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( tbUrlAlis.ID ) );
						return cell;
				}

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
								pageEntity.Title = tbTitle.Text;
								pageEntity.Name = tbName.Text;
								pageEntity.MasterPageId = Convert.ToInt32( ddlMasterPage.SelectedValue );
								pageEntity.RoleId = Convert.ToInt32( ddlRole.SelectedValue );
								if ( pageEntity.RoleId == 0 ) pageEntity.RoleId = null;

								MasterPageEntity masterPage = Storage<MasterPageEntity>.ReadFirst( new MasterPageEntity.ReadById { MasterPageId = pageEntity.MasterPageId } );
								UrlAliasEntity urlAlias = null;
								#region Vytvorenie URLAliasu
								string alias = this.tbUrlAlis.GetUrlAlias( string.Format( "~/{0}", pageEntity.Name ) );
								UrlAliasEntity urlAliasExists = Storage<UrlAliasEntity>.ReadFirst( new UrlAliasEntity.ReadByAlias { Alias = alias } );
								if ( !pageEntity.UrlAliasId.HasValue )
								{
										//Kontrola existencie URL Aliasu
										if ( urlAliasExists != null )
										{
												this.Page.ClientScript.RegisterStartupScript( GetType(), "alert", "alert('" + Resources.Controls.AdminUrlAliasesControl_UrlAliasExist_Message + "');", true );
												return;
										}

										urlAlias = new UrlAliasEntity();
										urlAlias.Alias = alias;
										urlAlias.Url = string.Format( "{0}{1}", masterPage.PageUrl, pageEntity.Name );
										urlAlias.Name = pageEntity.Title;
										Storage<UrlAliasEntity>.Create( urlAlias );

										pageEntity.UrlAliasId = urlAlias.Id;
								}
								else
								{
										//Update aliasu, ak dojde ku zmene nazvu stranky, musi sa zmenit aj url.
										urlAlias = Storage<UrlAliasEntity>.ReadFirst( new UrlAliasEntity.ReadById { UrlAliasId = pageEntity.UrlAliasId.Value } );
										urlAlias.Alias = alias;
										if ( urlAlias.Url.Contains( masterPage.PageUrl ) )//Url sa updatuje iba u entit co su contentove
												urlAlias.Url = string.Format( "{0}{1}", masterPage.PageUrl, pageEntity.Name );
										urlAlias.Name = pageEntity.Title;

										//Kontrola existencie URL Aliasu
										if ( urlAliasExists != null && urlAliasExists.Id != urlAlias.Id )
										{
												this.Page.ClientScript.RegisterStartupScript( GetType(), "alert", "alert('" + Resources.Controls.AdminUrlAliasesControl_UrlAliasExist_Message + "');", true );
												return;
										}
										Storage<UrlAliasEntity>.Update( urlAlias );
								}
								#endregion

								if ( isNew )
										pageEntity = Storage<PageEntity>.Create( pageEntity );
								else Storage<PageEntity>.Update( pageEntity );

								// Update Url Alias URL by Page Name,
								// Dovodom je, ze nazov page sa pri vytvoreni v databazi zbavi diakritiky
								pageEntity = Storage<PageEntity>.ReadFirst( new PageEntity.ReadById { PageId = pageEntity.Id } );
								urlAlias = Storage<UrlAliasEntity>.ReadFirst( new UrlAliasEntity.ReadById { UrlAliasId = pageEntity.UrlAliasId.Value } );
								if ( urlAlias.Url != string.Format( "{0}{1}", masterPage.PageUrl, pageEntity.Name ) )
								{
										urlAlias.Url = string.Format( "{0}{1}", masterPage.PageUrl, pageEntity.Name );
										Storage<UrlAliasEntity>.Update( urlAlias );
								}

								Response.Redirect( this.ReturnUrl );
						};
				}

		}
}
