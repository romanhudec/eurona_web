using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace CMS.Controls
{
		/// <summary>
		/// Button field implementjuci podporu JS OnClientClick.
		/// </summary>
		public class CMSButtonField: ButtonField
		{
				private string tooltip = string.Empty;
				#region Properties
				public virtual string OnClientClick
				{
						get
						{
								object o = this.ViewState["OnClientClick"];
								return o == null ? string.Empty : o.ToString();
						}
						set { this.ViewState["OnClientClick"] = value; }
				}

				public string ToolTip
				{
						get { return this.tooltip; }
						set { this.tooltip = value; }
				}

				#endregion

				public override void InitializeCell( DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex )
				{
						base.InitializeCell( cell, cellType, rowState, rowIndex );

						if ( cell.Controls.Count == 0 )
								return;

						Control control = cell.Controls[0];

						//Nastavenie client script.
						if ( control is Button )
						{
								Button btn = control as Button;
								if ( !string.IsNullOrEmpty( this.OnClientClick ) )
										btn.Attributes.Add( "onclick", OnClientClick );

								btn.ToolTip = this.ToolTip;
						}
						else if ( control is LinkButton )
						{
								LinkButton btn = control as LinkButton;
								if ( !string.IsNullOrEmpty( this.OnClientClick ) )
										btn.Attributes.Add( "onclick", OnClientClick );

								btn.ToolTip = this.ToolTip;
						}
						else if ( control is ImageButton )
						{
								ImageButton btn = new ImageButton();
								btn.ImageUrl = ( (ImageButton)control ).ImageUrl;
								btn.CommandName = ( (ImageButton)control ).CommandName;
								btn.CommandArgument = rowIndex.ToString();
								btn.ToolTip = this.ToolTip;
								btn.Attributes.Add( "onclick", OnClientClick );

								cell.Controls.Clear();
								cell.Controls.Add( btn );
						}
				}

				protected override void CopyProperties( DataControlField newField )
				{
						if ( !string.IsNullOrEmpty( this.OnClientClick ) )
								( (CMSButtonField)newField ).OnClientClick = this.OnClientClick;

						base.CopyProperties( newField );
				}

				protected override DataControlField CreateField()
				{
						return new CMSButtonField();
				}
		}
}
