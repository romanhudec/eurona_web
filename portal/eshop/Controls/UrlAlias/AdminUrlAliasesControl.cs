using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI.WebControls;
using UrlAliasEntity = SHP.Entities.UrlAlias;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CMS.Controls;
using Telerik.Web.UI;

namespace SHP.Controls.UrlAlias
{
		public class AdminUrlAliasesControl: CMS.Controls.UrlAlias.AdminUrlAliasesControl
		{
				internal enum AliasType: int
				{
						All = 0,
						Categories = 1,
						Products = 2
				}

				protected override void BindControls()
				{
						int aliasTypeId = (int)AliasType.Categories;
						if( !string.IsNullOrEmpty(this.ddlAliasType.SelectedValue))
								aliasTypeId = Convert.ToInt32( this.ddlAliasType.SelectedValue );
						GridViewDataBind( !this.IsPostBack, aliasTypeId );

						if ( this.IsPostBack )
								return;

						ListItem li = new ListItem( Resources.Controls.AdminUrlAliasesControl_AliasType_Categories, ((int)AliasType.Categories).ToString() );
						this.ddlAliasType.Items.Add( li );
						li = new ListItem( Resources.Controls.AdminUrlAliasesControl_AliasType_Products, ( (int)AliasType.Products ).ToString() );
						this.ddlAliasType.Items.Add( li );
				}

				protected override void GridViewDataBind( bool bind, object type )
				{
						AliasType aliasType = (AliasType)Convert.ToInt32( type );
						object condition = null;
						switch ( aliasType )
						{
								case AliasType.All:
										condition = null;
										break;

								case AliasType.Categories:
										condition = new UrlAliasEntity.ReadByAliasType.Categories();
										break;

								case AliasType.Products:
										condition = new UrlAliasEntity.ReadByAliasType.Products();
										break;

								default:
										condition = null;
										break;
						}

						List<UrlAliasEntity> aliasses = Storage<UrlAliasEntity>.Read( condition );
						var ordered = aliasses.AsQueryable().OrderBy( SortExpression + " " + SortDirection );
						base.gridView.DataSource = ordered.ToList();
						if ( bind ) base.gridView.DataBind();

				}
		}
}
