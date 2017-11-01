using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace CMS.Controls
{
		public class GridIconColumn: Telerik.Web.UI.GridImageColumn
		{
				public override void PrepareCell( TableCell cell, Telerik.Web.UI.GridItem item )
				{
						if ( item.ItemType == Telerik.Web.UI.GridItemType.Item ||
								item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem )
						{
								Image img = ( cell.Controls[0] as Image );
								if ( string.IsNullOrEmpty( img.ImageUrl ) )
								{
										img.Style.Add( "display", "none" );

										if ( !string.IsNullOrEmpty( img.AlternateText ) )
												cell.Controls.Add( new LiteralControl( img.AlternateText ) );
								}
								else
								{
										if ( img.ImageUrl.StartsWith( "~" ) )
												img.ImageUrl = img.Page.ResolveUrl( img.ImageUrl );
								}
						}

						base.PrepareCell( cell, item );
				}
		}
}
