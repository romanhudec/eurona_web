using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMSEditor = CMS.Controls.RadEditor.RadEditor;
using NewsletterEntity = CMS.Entities.Newsletter;
using RoleEntity = CMS.Entities.Role;
using System.Text;

namespace CMS.Controls.Newsletter
{
		public class AdminNewsletterControl: CmsControl
		{
				private Label lblDate = null;
				private CheckBoxList clbRoles;
				private TextBox txtSubject = null;
				private CMSEditor edtContent;

				private Button btnSave = null;
				private Button btnCancel = null;

				private NewsletterEntity newsletter = null;

				public AdminNewsletterControl()
				{
				}

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? NewsletterId
				{
						get
						{
								object o = ViewState["NewsletterId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["NewsletterId"] = value; }
				}


				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						Control pollControl = CreateDetailControl();
						if ( pollControl != null )
								this.Controls.Add( pollControl );

						//Binding
						if ( !this.NewsletterId.HasValue )
						{
								this.newsletter = new NewsletterEntity();
								this.newsletter.Date = DateTime.Now;
								this.newsletter.Roles = RoleEntity.NEWSLETTER;
						}
						else
								this.newsletter = Storage<NewsletterEntity>.ReadFirst( new NewsletterEntity.ReadById { NewsletterId = this.NewsletterId.Value } );

						if ( !IsPostBack )
						{
								this.lblDate.Text = this.newsletter.Date.ToString();
								this.txtSubject.Text = this.newsletter.Subject;
								this.edtContent.Content = this.newsletter.Content;

								List<RoleEntity> roles = Storage<RoleEntity>.Read().OrderBy( p => p.Name ).ToList();
								this.clbRoles.DataSource = roles;
								this.clbRoles.DataTextField = "Name";
								this.clbRoles.DataValueField = "Id";
								this.clbRoles.DataBind();

								this.DataBind();

								foreach ( ListItem li in clbRoles.Items )
										li.Selected = this.newsletter.Roles.IndexOf( li.Text ) > -1;

						}

				}
				#endregion

				private Control CreateDetailControl()
				{
						this.lblDate = new Label();
						this.lblDate.ID = "lblDate";

						this.clbRoles = new CheckBoxList();
						this.clbRoles.ID = "clbRoles";
						this.clbRoles.RepeatColumns = 3;
						this.clbRoles.Width = Unit.Percentage( 100 );

						this.txtSubject = new TextBox();
						this.txtSubject.ID = "txtSubject";
						this.txtSubject.Width = Unit.Percentage( 100 );

						this.edtContent = new CMSEditor();
						this.edtContent.ID = "edtContent";

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

						table.Rows.Add( CreateTableRow( Resources.Controls.AdminNewsletterControl_Date, this.lblDate, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminNewsletterControl_Roles, this.clbRoles, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminNewsletterControl_Subject, this.txtSubject, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminNewsletterControl_Content, this.edtContent, false ) );


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

				private string GetRoleStringFromUI()
				{
						StringBuilder sbRoles = new StringBuilder();
						foreach ( ListItem li in clbRoles.Items )
						{
								if ( !li.Selected ) continue;
								sbRoles.AppendFormat( "{0};", li.Text );
						}
						return sbRoles.ToString();
				}


				void OnSave( object sender, EventArgs e )
				{
						this.newsletter.Roles = GetRoleStringFromUI();
						this.newsletter.Subject = this.txtSubject.Text;
						this.newsletter.Content = this.edtContent.Content;
						this.newsletter.Locale = Security.Account.Locale;

						if ( !this.NewsletterId.HasValue ) Storage<NewsletterEntity>.Create( this.newsletter );
						else Storage<NewsletterEntity>.Update( this.newsletter );

						Response.Redirect( this.ReturnUrl );
				}

				void OnCancel( object sender, EventArgs e )
				{
						Response.Redirect( this.ReturnUrl );
				}
		}
}
