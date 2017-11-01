using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UrlAliasEntity = CMS.Entities.UrlAlias;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;


namespace CMS.Controls.UrlAlias
{
		public class AdminUrlAliasControl: CmsControl
		{
				private TextBox txtUrl;
				private TextBox txtName;
				private TextBox txtAlias;
				private Button btnSave;
				private Button btnCancel;

				private bool isNew = false;
				private UrlAliasEntity urlAlias = null;

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? UrlAliasId
				{
						get
						{
								object o = ViewState["UrlAliasId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["UrlAliasId"] = value; }
				}

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						isNew = !this.UrlAliasId.HasValue;
						if ( isNew ) urlAlias = new UrlAliasEntity();
						else urlAlias = Storage<UrlAliasEntity>.ReadFirst( new UrlAliasEntity.ReadById { UrlAliasId = this.UrlAliasId.Value } );

						Table table = new Table();
						table.CssClass = this.CssClass;
						table.Width = this.Width;

						TableRow trUrl = new TableRow();
						trUrl.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminUrlAliasControl_LabelUrl,
								CssClass = "form_label_required"
						} );
						trUrl.Cells.Add( CreateUrlInput() );
						table.Rows.Add( trUrl );

						TableRow trName = new TableRow();
						trName.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminUrlAliasControl_LabelName,
								CssClass = "form_label_required"
						} );
						trName.Cells.Add( CreateNameInput() );
						table.Rows.Add( trName );

						TableRow trAlias = new TableRow();
						trAlias.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminUrlAliasControl_LabelAlias,
								CssClass = "form_label_required"
						} );
						trAlias.Cells.Add( CreateAliasInput() );
						table.Rows.Add( trAlias );


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
				}

				private TableCell CreateUrlInput()
				{
						TableCell cell = new TableCell();
						this.txtUrl = new TextBox()
						{
								ID = "txtUrl",
								Text = urlAlias.Url,
								Width = Unit.Percentage( 80 )
						};
						cell.Controls.Add( this.txtUrl );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( this.txtUrl.ID ) );
						return cell;

				}
				private TableCell CreateNameInput()
				{
						TableCell cell = new TableCell();
						this.txtName = new TextBox()
						{
								ID = "txtName",
								Text = urlAlias.Name,
								Width = Unit.Percentage( 80 )
						};
						cell.Controls.Add( this.txtName );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( this.txtName.ID ) );
						return cell;
				}

				private TableCell CreateAliasInput()
				{
						string root = Utilities.ServerUtilities.Root( this.Request );
						if ( root.EndsWith( "/" ) ) root = root.Remove( root.Length - 1, 1 );

						TableCell cell = new TableCell();
						this.txtAlias = new TextBox()
						{
								ID = "txtAlias",
								Text = urlAlias.Alias.StartsWith( "~" ) ? urlAlias.Alias.Remove( 0, 1 ) : urlAlias.Alias,
								Width = Unit.Percentage( 80 ),
						};
						cell.Controls.Add( new LiteralControl( root ) );
						cell.Controls.Add( this.txtAlias );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( this.txtAlias.ID ) );
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
								urlAlias.Url = txtUrl.Text;
								urlAlias.Alias = txtAlias.Text;
								urlAlias.Name = txtName.Text;

								//Alias musy byt v tvare ~/XXXX-XXX/YYYYY ....
								if ( urlAlias.Alias.StartsWith( "~" ) ) urlAlias.Alias = urlAlias.Alias.Remove( 0, 1 );
								if ( urlAlias.Alias.StartsWith( "/" ) ) urlAlias.Alias = urlAlias.Alias.Remove( 0, 1 );
								urlAlias.Alias = "~/" + urlAlias.Alias;

								//Kontrola existencie URL Aliasu
								UrlAliasEntity urlAliasExists = Storage<UrlAliasEntity>.ReadFirst( new UrlAliasEntity.ReadByAlias { Alias = urlAlias.Alias } );
								if ( isNew )
								{

										//Kontrola existencie URL Aliasu
										if ( urlAliasExists != null )
										{
												this.Page.ClientScript.RegisterStartupScript( GetType(), "alert", "alert('" + Resources.Controls.AdminUrlAliasesControl_UrlAliasExist_Message + "');", true );
												return;
										}

										Storage<UrlAliasEntity>.Create( urlAlias );
								}
								else
								{
										//Kontrola existencie URL Aliasu
										if ( urlAliasExists != null && urlAliasExists.Id != urlAlias.Id )
										{
												this.Page.ClientScript.RegisterStartupScript( GetType(), "alert", "alert('" + Resources.Controls.AdminUrlAliasesControl_UrlAliasExist_Message + "');", true );
												return;
										}
										Storage<UrlAliasEntity>.Update( urlAlias );
								}

								Response.Redirect( this.ReturnUrl );
						};
				}

		}
}
