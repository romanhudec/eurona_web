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
		public class AdminProductReviewsControl: CmsControl
		{
				private RadListBox lbReviews = null;
				private Button btnAddArticle = null;
				private Button btnRemoveArticle = null;
				private DropDownList ddlCategory;
				private RadListBox lbArticles = null;

				public int? ProductId
				{
						get
						{
								object o = ViewState["ProductId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["ProductId"] = value; }
				}

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						this.lbReviews = new RadListBox();
						this.lbReviews.ID = "lbReviews";
						this.lbReviews.Width = Unit.Percentage( 100 );
						this.lbReviews.Height = Unit.Percentage( 100 );

						this.btnAddArticle = new Button();
						this.btnAddArticle.ID = "btnAddArticle";
						this.btnAddArticle.Text = "<<";
						this.btnAddArticle.Click += new EventHandler( btnAddArticle_Click );
						this.btnRemoveArticle = new Button();
						this.btnRemoveArticle.ID = "btnRemoveArticle";
						this.btnRemoveArticle.Text = ">>";
						this.btnRemoveArticle.Click += new EventHandler( btnRemoveArticle_Click );

						this.ddlCategory = new DropDownList();
						this.ddlCategory.ID = "ddlCategory";
						this.ddlCategory.AutoPostBack = true;
						this.ddlCategory.SelectedIndexChanged += new EventHandler( ddlCategory_SelectedIndexChanged );

						this.lbArticles = new RadListBox();
						this.lbArticles.ID = "lbArticles";
						this.lbArticles.Width = Unit.Percentage( 100 );
						this.lbArticles.Height = Unit.Percentage( 100 );

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
						cell.Controls.Add( new LiteralControl( Resources.Controls.AdminProductReviewsControl_Category ) );
						cell.Controls.Add( this.ddlCategory );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//Controls
						row = new TableRow();
						cell = new TableCell();
						cell.Width = Unit.Percentage( 50 );
						cell.Height = Unit.Percentage( 100 );
						cell.Controls.Add( this.lbReviews );
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.Height = Unit.Percentage( 100 );
						cell.Controls.Add( this.btnAddArticle );
						cell.Controls.Add( new LiteralControl( "<br />" ) );
						cell.Controls.Add( this.btnRemoveArticle );
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.Width = Unit.Percentage( 50 );
						cell.Height = Unit.Percentage( 100 );
						cell.Controls.Add( this.lbArticles ); row.Cells.Add( cell );

						table.Rows.Add( row );
						this.Controls.Add( table );

						//Binding
						if ( !IsPostBack )
						{
								List<CMS.Entities.Classifiers.ArticleCategory> list = Storage<CMS.Entities.Classifiers.ArticleCategory>.Read();
								this.ddlCategory.DataSource = list;
								this.ddlCategory.DataTextField = "Name";
								this.ddlCategory.DataValueField = "Id";
								this.ddlCategory.DataBind();
								ddlCategory_SelectedIndexChanged( this, null );

								List<Entities.ProductReviews> listPR = Storage<Entities.ProductReviews>.Read( new Entities.ProductReviews.ReadByProduct { ProductId = this.ProductId } );
								this.lbReviews.DataSource = listPR;
								this.lbReviews.DataTextField = "Title";
								this.lbReviews.DataValueField = "ArticleId";
								this.lbReviews.DataBind();
						}
				}

				private void btnRemoveArticle_Click( object sender, EventArgs e )
				{
						if ( string.IsNullOrEmpty( this.lbReviews.SelectedValue ) ) return;
						this.lbReviews.Items.Remove( this.lbReviews.Items[this.lbReviews.SelectedIndex] );
				}

				private void btnAddArticle_Click( object sender, EventArgs e )
				{
						if ( string.IsNullOrEmpty( this.lbArticles.SelectedValue ) ) return;
						RadListBoxItem li = this.lbReviews.Items.FirstOrDefault( x => x.Value == this.lbArticles.SelectedValue );
						if ( li != null ) return;

						this.lbReviews.Items.Add( this.lbArticles.Items[this.lbArticles.SelectedIndex].Clone() );
				}

				private void ddlCategory_SelectedIndexChanged( object sender, EventArgs e )
				{
						List<CMS.Entities.Article> list = new List<CMS.Entities.Article>();

						int categoryId = 0;
						Int32.TryParse( this.ddlCategory.SelectedValue, out categoryId );
						if ( categoryId != 0 ) list = Storage<CMS.Entities.Article>.Read( new CMS.Entities.Article.ReadReleased { CategoryId = categoryId } );

						this.lbArticles.DataSource = list;
						this.lbArticles.DataTextField = "Title";
						this.lbArticles.DataValueField = "Id";
						this.lbArticles.DataBind();
				}

				/// <summary>
				/// Ulozi data.
				/// </summary>
				public void Save()
				{
						if( !this.ProductId.HasValue ) return;

						List<Entities.ProductReviews> list = Storage<Entities.ProductReviews>.Read( new Entities.ProductReviews.ReadByProduct { ProductId = this.ProductId.Value } );
						foreach ( Entities.ProductReviews pr in list )
								Storage<Entities.ProductReviews>.Delete( pr );

						foreach ( Telerik.Web.UI.RadListBoxItem li in this.lbReviews.Items )
						{
								Entities.ProductReviews pr = new SHP.Entities.ProductReviews();
								pr.ProductId = this.ProductId.Value;
								pr.ArticleId = Convert.ToInt32(li.Value);
								Storage<Entities.ProductReviews>.Create( pr );
						}
				}
		}
}
