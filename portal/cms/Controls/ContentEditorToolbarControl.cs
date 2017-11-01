using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CMS.Controls
{
		public class ContentEditorToolbarControl : CmsControl
		{
				public ContentEditorToolbarControl()
						: base()
				{
						this.Items = new List<Control>();
				}

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						Table table = new Table();
						table.CellSpacing = 0;
						table.CellPadding = 0;
						TableRow row = new TableRow();
						table.Rows.Add( row );

						foreach ( Control ctrl in this.Items )
						{
								TableCell cell = new TableCell();
								cell.HorizontalAlign = HorizontalAlign.Center;
								cell.CssClass = this.CssClass + "_item";
								cell.Controls.Add( ctrl );
								row.Cells.Add( cell );
						}
						this.Controls.Add( table );
				}

				/// <summary>
				/// Každy toolbar sa bude renderovat ako div s požadovanými attribútmi.
				/// </summary>
				protected override void RenderChildren( HtmlTextWriter writer )
				{
						writer.WriteBeginTag("div");
						writer.WriteAttribute( "id", this.ClientID );
						writer.WriteAttribute( "class", this.CssClass );
						foreach( string key in Attributes.Keys )
								writer.WriteAttribute( key, Attributes[key] );
						
						writer.Write( HtmlTextWriter.TagRightChar );

						base.RenderChildren( writer );
						writer.WriteEndTag("div");
				}


				/// <summary>
				/// Položky toolbaru.
				/// </summary>
				public List<Control> Items { get; set; }

		}
}
