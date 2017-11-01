using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Controls;
using System.Web.UI.WebControls;
using System.Web.UI;
using SHP.Entities.Classifiers;
using SHP.Entities;

namespace SHP.Controls.Product
{
		public class AdminProductHighlightsControl: CmsControl
		{
				private CheckBoxList cbl = null;
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

						this.cbl = new CheckBoxList();
						this.cbl.ID = "cbl";
						this.cbl.Width = Unit.Percentage( 100 );
						this.cbl.Height = Unit.Percentage( 100 );

						this.Controls.Add( this.cbl );

						//Datasource for DropDownList
						this.cbl.DataSource = Storage<Highlight>.Read();
						this.cbl.RepeatDirection = RepeatDirection.Horizontal;
						this.cbl.DataValueField = "Id";
						this.cbl.DataTextField = "Name";

						//Binding
						if ( !IsPostBack )
						{
								this.cbl.DataBind();

								List<ProductHighlights> list = Storage<ProductHighlights>.Read( new ProductHighlights.ReadByProduct { ProductId = this.ProductId } );
								foreach ( ProductHighlights ph in list )
								{
										ListItem li = this.cbl.Items.FindByValue( ph.HighlightId.ToString() );
										if ( li == null ) continue;

										li.Selected = true;
								}
						}
				}

				/// <summary>
				/// Ulozi data.
				/// </summary>
				public void Save()
				{
						if( !this.ProductId.HasValue ) return;

						List<ProductHighlights> list = Storage<ProductHighlights>.Read( new ProductHighlights.ReadByProduct { ProductId = this.ProductId.Value } );
						foreach ( ProductHighlights pr in list )
								Storage<ProductHighlights>.Delete( pr );

						foreach (ListItem li in this.cbl.Items )
						{
								if ( !li.Selected ) continue;

								ProductHighlights pr = new ProductHighlights();
								pr.ProductId = this.ProductId.Value;
								pr.HighlightId = Convert.ToInt32( li.Value );
								Storage<ProductHighlights>.Create( pr );
						}
				}
		}
}
