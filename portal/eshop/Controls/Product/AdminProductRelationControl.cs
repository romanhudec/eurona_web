using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Controls;
using System.Web.UI.WebControls;
using System.Web.UI;
using Telerik.Web.UI;

namespace SHP.Controls.Product
{
		public class AdminProductRelationControl: CmsControl
		{
				private RadListBox lbRelationProducts = null;
				private Button btnAddProduct = null;
				private Button btnRemoveProduct = null;
				private DropDownList ddlCategory;
				private RadListBox lbProducts = null;

				public int? ProductId
				{
						get
						{
								object o = ViewState["ProductId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["ProductId"] = value; }
				}

				public SHP.Entities.ProductRelation.Relation Relation
				{
						get
						{
								object o = ViewState["Relation"];
								return (SHP.Entities.ProductRelation.Relation)(int)Convert.ToInt32( o );
						}
						set { ViewState["Relation"] = value; }
				}

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						this.lbRelationProducts = new RadListBox();
						this.lbRelationProducts.ID = "lbRelationProducts";
						this.lbRelationProducts.Width = Unit.Percentage( 100 );
						this.lbRelationProducts.Height = Unit.Percentage( 100 );

						this.btnAddProduct = new Button();
						this.btnAddProduct.ID = "btnAddProduct";
						this.btnAddProduct.Text = "<<";
						this.btnAddProduct.Click += new EventHandler( btnAddProduct_Click );
						this.btnRemoveProduct = new Button();
						this.btnRemoveProduct.ID = "btnRemoveProduct";
						this.btnRemoveProduct.Text = ">>";
						this.btnRemoveProduct.Click += new EventHandler( btnRemoveProduct_Click );

						this.ddlCategory = new DropDownList();
						this.ddlCategory.ID = "ddlCategory";
						this.ddlCategory.AutoPostBack = true;
						this.ddlCategory.SelectedIndexChanged += new EventHandler( ddlCategory_SelectedIndexChanged );

						this.lbProducts = new RadListBox();
						this.lbProducts.ID = "lbProducts";
						this.lbProducts.Width = Unit.Percentage( 100 );
						this.lbProducts.Height = Unit.Percentage( 100 );

						Table table = new Table();
						//table.Attributes.Add( "border", "1" );
						table.Width = Unit.Percentage( 100 );
						table.Height = this.Height;
						TableRow row = null;
						TableCell cell = null;

						//Labels 
						row = new TableRow();
						cell = new TableCell(); cell.Width = Unit.Percentage( 50 ); row.Cells.Add( cell );
						cell = new TableCell(); row.Cells.Add( cell );

						cell = new TableCell();
						cell.Width = Unit.Percentage( 50 );
						cell.HorizontalAlign = HorizontalAlign.Right;
						cell.Controls.Add( new LiteralControl( Resources.Controls.AdminProductRelationControl_Category ) );
						cell.Controls.Add( this.ddlCategory );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//Controls
						row = new TableRow();
						cell = new TableCell();
						cell.Width = Unit.Percentage( 50 );
						cell.Height = Unit.Percentage( 100 );
						cell.Controls.Add( this.lbRelationProducts );
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.Height = Unit.Percentage( 100 );
						cell.Controls.Add( this.btnAddProduct );
						cell.Controls.Add( new LiteralControl( "<br />" ) );
						cell.Controls.Add( this.btnRemoveProduct );
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.Width = Unit.Percentage( 50 );
						cell.Height = Unit.Percentage( 100 );
						cell.Controls.Add( this.lbProducts ); row.Cells.Add( cell );

						table.Rows.Add( row );
						this.Controls.Add( table );

						//Binding

						if ( !IsPostBack )
						{
								List<Entities.Category> list = Storage<Entities.Category>.Read();
								this.ddlCategory.Items.Clear();
								this.FillCategories( this.ddlCategory, list );
								this.ddlCategory.DataBind();
								ddlCategory_SelectedIndexChanged( this, null );

								List<Entities.ProductRelation> listPR = Storage<Entities.ProductRelation>.Read( new Entities.ProductRelation.ReadBy { ParentProductId = this.ProductId, RelationType = this.Relation } );
								this.lbRelationProducts.DataSource = listPR;
								this.lbRelationProducts.DataTextField = "ProductName";
								this.lbRelationProducts.DataValueField = "ProductId";
								this.lbRelationProducts.DataBind();
						}
				}

				private void btnRemoveProduct_Click( object sender, EventArgs e )
				{
						if ( string.IsNullOrEmpty( this.lbRelationProducts.SelectedValue ) ) return;
						this.lbRelationProducts.Items.Remove( this.lbRelationProducts.Items[this.lbRelationProducts.SelectedIndex] );
				}

				private void btnAddProduct_Click( object sender, EventArgs e )
				{
						if ( string.IsNullOrEmpty( this.lbProducts.SelectedValue ) ) return;
						RadListBoxItem li = this.lbRelationProducts.Items.FirstOrDefault( x => x.Value == this.lbProducts.SelectedValue );
						if ( li != null ) return;

						this.lbRelationProducts.Items.Add( this.lbProducts.Items[this.lbProducts.SelectedIndex].Clone() );
				}

				private void ddlCategory_SelectedIndexChanged( object sender, EventArgs e )
				{
						List<Entities.Product> list = new List<SHP.Entities.Product>();

						int categoryId = 0;
						Int32.TryParse( this.ddlCategory.SelectedValue, out categoryId );
						if ( categoryId != 0 ) list = Storage<Entities.Product>.Read( new Entities.Product.ReadByCategory { CategoryId = categoryId } );

						this.lbProducts.DataSource = list;
						this.lbProducts.DataTextField = "Name";
						this.lbProducts.DataValueField = "Id";
						this.lbProducts.DataBind();
				}

				private void FillCategories( DropDownList ddl, List<Entities.Category> list )
				{
						foreach ( Entities.Category c in list )
						{
								if ( c.ParentId.HasValue ) continue;

								ListItem li = new ListItem( c.Name, c.Id.ToString() );
								li.Attributes.Add( "style", "font-weight:bold; background-color:#EDEDED;" );
								ddl.Items.Add( li );
								PopulateChilds( ddl, list, c.Id, 1 );
						}
				}

				private void PopulateChilds( DropDownList ddl, List<Entities.Category> list, int parentId, int level )
				{
						foreach ( Entities.Category c in list )
						{
								if ( !c.ParentId.HasValue ) continue;
								if ( c.ParentId.Value != parentId ) continue;

								string prefix = string.Empty;
								for ( int i = 0; i < level; i++ ) prefix += Server.HtmlDecode( "&nbsp;&nbsp;" );
								prefix += Server.HtmlDecode( "--&nbsp;" );

								ListItem li = new ListItem( prefix + c.Name, c.Id.ToString() );
								//li.Attributes.Add("class", "font-weight:bold;margin-left:5px;");
								ddl.Items.Add( li );
								PopulateChilds( ddl, list, c.Id, level + 1 );
						}
				}


				/// <summary>
				/// Ulozi data.
				/// </summary>
				public void Save()
				{
						if( !this.ProductId.HasValue ) return;

						List<Entities.ProductRelation> list = Storage<Entities.ProductRelation>.Read( new Entities.ProductRelation.ReadBy { ParentProductId = this.ProductId, RelationType = this.Relation } );
						foreach ( Entities.ProductRelation pr in list )
						{
								try
								{
										Storage<Entities.ProductRelation>.Delete( pr );
								}
								catch ( Exception ex )
								{
										CMS.EvenLog.WritoToEventLog( ex );
										throw ex;
								}
						}

						foreach ( Telerik.Web.UI.RadListBoxItem li in this.lbRelationProducts.Items )
						{
								Entities.ProductRelation pr = new SHP.Entities.ProductRelation();
								pr.ParentProductId = this.ProductId.Value;
								pr.ProductId = Convert.ToInt32(li.Value);
								pr.RelationType = (int)this.Relation;
								Storage<Entities.ProductRelation>.Create( pr );
						}
				}
		}
}
