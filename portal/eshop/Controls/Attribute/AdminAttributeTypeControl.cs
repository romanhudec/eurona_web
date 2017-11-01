using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Controls;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace SHP.Controls.Attribute
{
		public class AdminAttributeTypeControl : CmsControl
		{
				public AdminAttributeTypeControl()
				{
				}

				private TextBox txtValueFrom = null;
				private TextBox txtValueTo = null;

				private TextBox txtValue = null;
				private ListBox lbxValues = null;
				private Button btnAddValue = null;
				private Button btnRemoveValue = null;

				public Entities.AttributeType.Type AttributeType
				{
						get
						{
								object o = this.ViewState["AttributeType"];
								return o != null ? (Entities.AttributeType.Type)Convert.ToInt32( o ) : Entities.AttributeType.Type.None;
						}

						set
						{
								EnsureChildControls();
								ViewState["AttributeType"] = (int)value;
								CreateAttributeTypeControls();
						}
				}

				public string Limit
				{
						get
						{
								object o = this.ViewState["value"];
								return o != null ? o.ToString() : string.Empty;
						}

						set { ViewState["value"] = value; }
				}

				protected override void CreateChildControls()
				{
						base.CreateChildControls();
						this.CreateAttributeTypeControls();

				}

				private void CreateAttributeTypeControls()
				{
						this.Controls.Clear();

						if ( this.AttributeType == SHP.Entities.AttributeType.Type.String || 
								this.AttributeType == SHP.Entities.AttributeType.Type.Picture || 
								this.AttributeType == SHP.Entities.AttributeType.Type.Boolean )
								return;

						#region Číselné hodnoty
						if ( this.AttributeType == SHP.Entities.AttributeType.Type.Int || 
								this.AttributeType ==  SHP.Entities.AttributeType.Type.Double )
						{
								this.txtValueFrom = new TextBox();
								this.txtValueFrom.ID = "txtValueFrom";
								this.txtValueTo = new TextBox();
								this.txtValueTo.ID = "txtValueTo";

								Table table = new Table();
								table.Attributes.Add( "border", "0" );
								table.Width = this.Width;
								TableRow row = new TableRow();
								TableCell cell = new TableCell();

								//Od
								cell.Controls.Add( new LiteralControl( Resources.Controls.AdminAttributeTypeControl_FromLabel ) );
								row.Cells.Add( cell );
								cell = new TableCell();
								cell.Controls.Add( this.txtValueFrom );
								if ( this.AttributeType == SHP.Entities.AttributeType.Type.Int ) cell.Controls.Add( CreateNumberValidatorControl( this.txtValueFrom.ID ) );
								else cell.Controls.Add( CreateDoubleValidatorControl( this.txtValueFrom.ID ) );
								row.Cells.Add( cell );

								//Do
								cell = new TableCell();
								cell.Controls.Add( new LiteralControl( Resources.Controls.AdminAttributeTypeControl_ToLabel ) );
								row.Cells.Add( cell );
								cell = new TableCell();
								cell.Controls.Add( this.txtValueTo );
								if ( this.AttributeType == SHP.Entities.AttributeType.Type.Int ) cell.Controls.Add( CreateNumberValidatorControl( this.txtValueTo.ID ) );
								else cell.Controls.Add( CreateDoubleValidatorControl( this.txtValueTo.ID ) );
								row.Cells.Add( cell );

								table.Rows.Add( row );
								this.Controls.Add( table );

								//binding
								if ( !IsPostBack )
								{
										string[] values = this.Limit.Split( Entities.AttributeType.LimitFromToSeparator[0] );
										if ( values.Length != 0 )
										{
												this.txtValueFrom.Text = values[0];
												if ( values.Length > 1 )
														this.txtValueTo.Text = values[1];
										}
								}

						}
						#endregion

						#region Výber, Multi vyber
						if ( this.AttributeType == SHP.Entities.AttributeType.Type.MultiChoice || 
								this.AttributeType ==  SHP.Entities.AttributeType.Type.Choice )
						{
								this.txtValue = new TextBox();
								this.lbxValues = new ListBox();
								this.lbxValues.Width = Unit.Percentage( 100 );
								this.lbxValues.Height = Unit.Pixel( 150 );

								this.btnAddValue = new Button();
								this.btnAddValue.CausesValidation = false;
								this.btnAddValue.Click += new EventHandler( btnAddValue_Click );
								this.btnAddValue.Text = Resources.Controls.AdminAttributeTypeControl_AddValueButtonText;

								this.btnRemoveValue = new Button();
								this.btnRemoveValue.CausesValidation = false;
								this.btnRemoveValue.Click += new EventHandler( btnRemoveValue_Click );
								this.btnRemoveValue.Text = Resources.Controls.AdminAttributeTypeControl_RemoveSelectedValueButtonText;

								Table table = new Table();
								table.Width = this.Width;
								table.Attributes.Add( "border", "0" );
								TableRow row = new TableRow();
								TableCell cell = new TableCell();

								//Hodnota
								cell.Controls.Add( new LiteralControl( Resources.Controls.AdminAttributeTypeControl_ValueLabelText ) );
								row.Cells.Add( cell );
								cell = new TableCell();
								cell.Controls.Add( this.txtValue );
								row.Cells.Add( cell );
								cell = new TableCell();
								cell.Width = Unit.Percentage( 100 );
								cell.Controls.Add( this.btnAddValue );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								//Hodnota
								row = new TableRow();
								cell = new TableCell();
								cell.Controls.Add( this.lbxValues );
								cell.ColumnSpan = 3;
								row.Cells.Add( cell );
								table.Rows.Add( row );

								//Button
								row = new TableRow();
								cell = new TableCell();
								cell.Controls.Add( this.btnRemoveValue );
								cell.ColumnSpan = 3;
								row.Cells.Add( cell );
								table.Rows.Add( row );

								this.Controls.Add( table );

								//binding
								if ( !this.Page.IsPostBack )
								{
										string[] values = this.Limit.Split( Entities.AttributeType.LimitChoiceSeparator[0] );
										this.lbxValues.Items.Clear();
										foreach ( string value in values )
										{
												if ( value.Length == this.Limit.Length )
														return;

												ListItem item = new ListItem( value );
												this.lbxValues.Items.Add( item );
										}
								}
						}
						#endregion
				}

				void btnAddValue_Click( object sender, EventArgs e )
				{
						if ( string.IsNullOrEmpty( this.txtValue.Text ) )
								return;

						ListItem item = new ListItem( this.txtValue.Text );
						this.lbxValues.Items.Add( item );

						this.txtValue.Text = string.Empty;
				}

				void btnRemoveValue_Click( object sender, EventArgs e )
				{
						if ( this.lbxValues.SelectedIndex != -1 )
								this.lbxValues.Items.RemoveAt( this.lbxValues.SelectedIndex );
				}

				public string GetLimitValue()
				{
						if ( this.AttributeType == SHP.Entities.AttributeType.Type.Int || 
								this.AttributeType == SHP.Entities.AttributeType.Type.Double )
						{
								if ( string.IsNullOrEmpty( this.txtValueFrom.Text ) || string.IsNullOrEmpty( this.txtValueTo.Text ) )
										return string.Empty;

								return string.Format( "{0}{1}{2}", this.txtValueFrom.Text, Entities.AttributeType.LimitFromToSeparator, this.txtValueTo.Text );
						}
						else if ( this.AttributeType ==  SHP.Entities.AttributeType.Type.MultiChoice || 
								this.AttributeType ==  SHP.Entities.AttributeType.Type.Choice )
						{
								StringBuilder sb = new StringBuilder();
								foreach ( ListItem item in this.lbxValues.Items )
								{
										if ( sb.Length != 0 ) sb.Append( Entities.AttributeType.LimitChoiceSeparator );
										sb.Append( item.Text );
								}

								return sb.ToString();
						}

						return string.Empty;
				}
		}
}
