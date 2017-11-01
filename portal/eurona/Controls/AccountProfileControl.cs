using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS;
using CMS.Controls;
using CMS.Entities;
using Eurona.DAL.Entities;
using System;

namespace Eurona.Controls
{
		public class AccountProfileControl: CmsControl
		{
				public int AccountId
				{
						get
						{
								object o = ViewState["AccountId"];
								return o != null ? (int)Convert.ToInt32( o ) : 0;
						}
						set { ViewState["AccountId"] = value; }
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						List<AccountProfile> list = Storage<AccountProfile>.Read( new AccountProfile.ReadByAccountId { AccountId = this.AccountId } );
						if ( list.Count == 0 )
								return;

						HtmlGenericControl div = new HtmlGenericControl("div");
						Table table = new Table();
						foreach ( AccountProfile ap in list )
						{
								TableRow row = new TableRow();
								TableCell cell = new TableCell();
								row.Cells.Add( cell );

								switch ( (ProfileType)ap.ProfileType )
								{
										case ProfileType.Text:
										case ProfileType.HtmlText:
												cell.Controls.Add( new LiteralControl( ap.Value ) );
												break;
										case ProfileType.Picture:
												Image img = new Image();
												if ( !string.IsNullOrEmpty( ap.Value ) )
														img.ImageUrl = Page.ResolveUrl( ap.Value );
												else img.Visible = false;
												cell.Controls.Add( img );
												break;
								}
								table.Rows.Add( row );
						}

						div.Controls.Add( table );
						this.Controls.Add( div );
				}
				#endregion
		}
}
