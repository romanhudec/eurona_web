﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CategoryEntity = SHP.Entities.Category;
using CMS.Controls;
using System.Web.UI;
using CMS;
using System.Web.UI.HtmlControls;
using System.IO;
using SHP.Controls.Category;

namespace Eurona.Common.Controls.Category
{
		public class AdminCategoryControl: CmsControl
		{
				private CategoryPathControl categoryPathControl;
				private TextBox txtName = null;
				private TextBox txtOrder = null;
				private CMS.Controls.ASPxUrlAliasTextBox txtUrlAlis;

				private Button btnSave = null;
				private Button btnCancel = null;

				private CategoryEntity category = null;

				public AdminCategoryControl()
				{
				}

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? CategoryId
				{
						get
						{
								object o = ViewState["CategoryId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["CategoryId"] = value; }
				}

				public int? ParentId
				{
						get
						{
								object o = ViewState["ParentId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["ParentId"] = value; }
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


				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						this.categoryPathControl = new CategoryPathControl();
						this.categoryPathControl.CategoryId = this.ParentId;
						this.Controls.Add( this.categoryPathControl );
						this.Controls.Add( new LiteralControl( "<br />" ) );

						Control categoryControl = CreateDetailControl();
						if ( categoryControl != null )
								this.Controls.Add( categoryControl );

						//Priradenie id fieldu z ktoreho sa generuje alias
						this.txtUrlAlis.FieldID = this.txtName.ClientID;
						this.txtUrlAlis.UrlAliasPrefixId = this.UrlAliasPrefixId;
						this.txtUrlAlis.OnGetUrlAliasPrefix += new ASPxUrlAliasTextBox.UrlAliasPrefixEventHandler( txtUrlAlis_OnGetUrlAliasPrefix );

						//Binding
						if ( !this.CategoryId.HasValue ) this.category = new SHP.Entities.Category();
						else this.category = Storage<CategoryEntity>.ReadFirst( new CategoryEntity.ReadById { CategoryId = this.CategoryId.Value } );

						if ( !IsPostBack )
						{
								this.txtName.Text = this.category.Name;
								if( this.category.Order.HasValue ) this.txtOrder.Text = this.category.Order.ToString();
								
								//Nastavenie controlsu pre UrlAlias
								this.txtUrlAlis.AutoCompletteAlias = !this.CategoryId.HasValue;
								this.txtUrlAlis.Text = this.category.Alias.StartsWith( "~" ) ? this.category.Alias.Remove( 0, 1 ) : this.category.Alias;
						}
				}

				void txtUrlAlis_OnGetUrlAliasPrefix( string prefix, out string newPrefix )
				{
						newPrefix = prefix;
						//Ak je kategoria podkategoriou, priradim prefix z parent kategorie.
						if( this.ParentId.HasValue )
						{
								CategoryEntity parentCategory = Storage<CategoryEntity>.ReadFirst( new CategoryEntity.ReadById { CategoryId = this.ParentId.Value } );
								string alias = parentCategory.Alias;
								if ( alias.StartsWith( "~" ) ) alias = alias.Remove( 0, 1 );
								if ( alias.StartsWith( "/" ) ) alias = alias.Remove( 0, 1 );
								newPrefix = alias;
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
						this.txtName.Enabled = false;

						this.txtOrder = new TextBox();
						this.txtOrder.ID = "txtOrder";
					
						this.txtUrlAlis = new ASPxUrlAliasTextBox();
						this.txtUrlAlis.ID = "txtUrlAlis";
						this.txtUrlAlis.Width = Unit.Percentage( 100 );
						this.txtUrlAlis.Enabled = false;

						this.btnSave = new Button();
						this.btnSave.CausesValidation = true;
						this.btnSave.Text = SHP.Resources.Controls.SaveButton_Text;
						this.btnSave.Click += new EventHandler( OnSave );
						this.btnCancel = new Button();
						this.btnCancel.CausesValidation = false;
						this.btnCancel.Text = SHP.Resources.Controls.CancelButton_Text;
						this.btnCancel.Click += new EventHandler( OnCancel );

						Table table = new Table();
						table.Width = this.Width;
						table.Height = this.Height;

						table.Rows.Add( CreateTableRow( SHP.Resources.Controls.AdminCategoryControl_Name, this.txtName, true, false ) );
						table.Rows.Add( CreateTableRow( SHP.Resources.Controls.AdminCategoryControl_Order, this.txtOrder, true, true ) );

						table.Rows.Add( CreateTableRow( SHP.Resources.Controls.AdminCategoryControl_UrlAlias, this.txtUrlAlis, true, false ) );

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

				private TableRow CreateTableRow( string labelText, Control control, bool required, bool isNumber )
				{
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = required ? "form_label_required" : "form_label";
						cell.Text = labelText;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( control );
						if ( required && !( control is ASPxDatePicker ) ) cell.Controls.Add( base.CreateRequiredFieldValidatorControl( control.ID ) );
						if ( isNumber ) cell.Controls.Add( base.CreateNumberValidatorControl( control.ID ) );
						row.Cells.Add( cell );

						return row;
				}

				protected void RemoveIcon( CategoryEntity classifier )
				{
						string filePath = Server.MapPath( classifier.Icon );
						if ( File.Exists( filePath ) ) File.Delete( filePath );

						classifier.Icon = null;
						Storage<CategoryEntity>.Update( classifier );
				}

				void OnSave( object sender, EventArgs e )
				{
						this.category.Order = Convert.ToInt32( this.txtOrder.Text );
						this.category.Name = this.txtName.Text;
						this.category.Locale = Security.Account.Locale;

						if ( !this.CategoryId.HasValue )
						{
								this.category.ParentId = this.ParentId;
								Storage<CategoryEntity>.Create( this.category );
						}
						else Storage<CategoryEntity>.Update( this.category );

						#region Vytvorenie URLAliasu
						string alias = this.txtUrlAlis.GetUrlAlias( string.Format( "~/{0}", this.category.Name ) );
						if ( !CMS.Utilities.AliasUtilities.CreateUrlAlias<CategoryEntity>( this.Page, this.DisplayUrlFormat, this.category.Name, alias, this.category, Storage<CategoryEntity>.Instance ) )
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
