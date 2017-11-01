using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using CMS.Controls;
using AttributeEntity = SHP.Entities.Attribute;

namespace SHP.Controls.Attribute
{
		public class AdminAttributeControl: CmsControl
		{
				private Category.CategoryPathControl attributePathControl;
				private TextBox txtName = null;
				private DropDownList ddlAttributeType = null;
				private AdminAttributeTypeControl attributeTypeCtrl = null;
				private TextBox txtDefaultValue = null;

				private Button btnSave = null;
				private Button btnCancel = null;

				private AttributeEntity attribute = null;

				public AdminAttributeControl()
				{
				}

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? AttributeId
				{
						get
						{
								object o = ViewState["AttributeId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["AttributeId"] = value; }
				}

				public int CategoryId
				{
						get
						{
								object o = ViewState["CategoryId"];
								return o != null ? (int)Convert.ToInt32( o ) : 0;
						}
						set { ViewState["CategoryId"] = value; }
				}


				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						//Binding
						if ( !this.AttributeId.HasValue ) this.attribute = new SHP.Entities.Attribute();
						else
						{
								this.attribute = Storage<AttributeEntity>.ReadFirst( new AttributeEntity.ReadById { AttributeId = this.AttributeId.Value } );
								this.CategoryId = this.attribute.CategoryId;
						}

						//Category path control
						this.attributePathControl = new Category.CategoryPathControl();
						this.attributePathControl.CategoryId = this.CategoryId;
						this.Controls.Add( this.attributePathControl );
						this.Controls.Add( new LiteralControl( "<br />" ) );

						Control attributeControl = CreateDetailControl();
						if ( attributeControl != null )
								this.Controls.Add( attributeControl );

						if ( !IsPostBack )
						{
								this.ddlAttributeType.DataSource = Entities.AttributeType.LoadLocalizedTypes();
								this.ddlAttributeType.DataTextField = "Name";
								this.ddlAttributeType.DataValueField = "Id";
								this.ddlAttributeType.DataBind();
								this.ddlAttributeType.SelectedValue = ((int)this.attribute.Type).ToString();

								this.txtName.Text = this.attribute.Name;
								this.txtDefaultValue.Text = this.attribute.DefaultValue;

								this.attributeTypeCtrl.Limit = this.attribute.TypeLimit;
								this.attributeTypeCtrl.AttributeType = this.attribute.Type;
						}

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

						this.ddlAttributeType = new DropDownList();
						this.ddlAttributeType.AutoPostBack = true;
						this.ddlAttributeType.ID = "ddlAttributeType";
						this.ddlAttributeType.Width = Unit.Percentage( 100 );
						this.ddlAttributeType.SelectedIndexChanged += ( s, e ) =>
						{
								int type = Convert.ToInt32( this.ddlAttributeType.SelectedValue );
								this.attributeTypeCtrl.AttributeType = (SHP.Entities.AttributeType.Type)type;
						};

						this.attributeTypeCtrl = new AdminAttributeTypeControl();
						this.attributeTypeCtrl.ID = "attributeTypeCtrl";
						this.attributeTypeCtrl.Width = Unit.Percentage( 100 );

						this.txtDefaultValue = new TextBox();
						this.txtDefaultValue.ID = "txtDefaultValue";
						this.txtDefaultValue.Width = Unit.Percentage( 100 );

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

						table.Rows.Add( CreateTableRow( Resources.Controls.AdminAttributeControl_Name, this.txtName, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminAttributeControl_Type, this.ddlAttributeType, this.attributeTypeCtrl, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminAttributeControl_DefaultValue, this.txtDefaultValue, false ) );

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

				private TableRow CreateTableRow( string labelText, Control control1, Control control2, bool required )
				{
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = required ? "form_label_required" : "form_label";
						cell.VerticalAlign = VerticalAlign.Top;
						cell.Text = labelText;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( control1 );
						cell.Controls.Add( control2 );
						if ( required ) cell.Controls.Add( base.CreateRequiredFieldValidatorControl( control1.ID ) );
						row.Cells.Add( cell );

						return row;
				}


				void OnSave( object sender, EventArgs e )
				{
						this.attribute.Name = this.txtName.Text;
						this.attribute.Type = this.attributeTypeCtrl.AttributeType;
						this.attribute.TypeLimit = this.attributeTypeCtrl.GetLimitValue();
						this.attribute.DefaultValue = this.txtDefaultValue.Text;
						this.attribute.Locale = Security.Account.Locale;

						if ( !this.AttributeId.HasValue )
						{
								this.attribute.CategoryId = this.CategoryId;
								Storage<AttributeEntity>.Create( this.attribute );
						}
						else Storage<AttributeEntity>.Update( this.attribute );

						Response.Redirect( this.ReturnUrl );
				}

				void OnCancel( object sender, EventArgs e )
				{
						Response.Redirect( this.ReturnUrl );
				}
		}
}
