using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using CMS.Entities;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using PollEntity = CMS.Entities.Poll;

namespace CMS.Controls.Poll
{
		public class ArchivedPollsControl: CmsControl
		{
				public ArchivedPollsControl()
				{
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						Control pollControl = CreateArchivedPollControl();
						if ( pollControl != null )
								this.Controls.Add( pollControl );

				}
				#endregion

				#region Public Styles Properties
				public string CssPollClass { get; set; }
				#endregion

				/// <summary>
				/// Vytvori Control ArchivovaneAnkety
				/// </summary>
				private Control CreateArchivedPollControl()
				{
						List<PollEntity> polls = Storage<PollEntity>.Read();

						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Attributes.Add( "class", this.CssClass );

						// Poll Header
						Table pollsTable = new Table();
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = this.CssClass + "_header";
						cell.Text = Resources.Controls.ArchivedPollsControl_Title;
						row.Cells.Add( cell );
						pollsTable.Rows.Add( row );

						// Poll options
						foreach ( PollEntity poll in polls )
						{
								row = new TableRow();
								cell = new TableCell();
								cell.Controls.Add( CreatePollDetail( poll ) );
								row.Cells.Add( cell );
								pollsTable.Rows.Add( row );
						}

						div.Controls.Add( pollsTable );

						return div;
				}

				private Control CreatePollDetail( PollEntity poll )
				{
						string divId = string.Format( "ap_{0}", poll.Id );

						Table table = new Table();
						TableRow row = new TableRow();
						TableCell cell = new TableCell();

						string header = string.Format( "» {0} - {1} {2}",
								poll.DateFrom.ToShortDateString(),
								poll.DateTo.HasValue ? poll.DateTo.Value.ToShortDateString() : string.Empty,
								poll.Question );

						string jsOnClick = string.Format( @"var elm = getElementById('{0}'); var disp = elm.style.display;
										elm.style.display = (disp==''||disp=='block')?'none':'block'",
								divId );

						string navigationLink = string.Format( "<a href='#' onclick=\"{0}\" class='{2}' >{1}</a>", jsOnClick, header, this.CssClass + "_pollLink" );
						cell.Controls.Add( new LiteralControl( navigationLink ) );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//Detail ankety
						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Attributes.Add( "id", divId );
						div.Attributes.Add( "class", this.CssClass + "_content" );
						div.Style.Add( "display", "none" );

						PollControl pollControl = new PollControl();
						pollControl.HideHeader = true;
						pollControl.PollId = poll.Id;
						pollControl.CssClass = this.CssPollClass;
						div.Controls.Add( pollControl );

						row = new TableRow();
						cell = new TableCell();
						cell.Controls.Add( div );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						return table;
				}
		}
}
