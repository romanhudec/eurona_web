using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using AccountVoteEntity = CMS.Entities.AccountVote;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;
using System.ComponentModel;

namespace CMS.Controls.Vote
{
		public class CarmaControl: CmsControl
		{
				public delegate double VoteEventHandler( int objectId, int rating );
				public event VoteEventHandler OnVote;

				private Label lblRatingResult = null;

				private Button btnPositiveVote = null;
				private Button btnNegativeVote = null;

				public CarmaControl()
				{
				}

				/// <summary>
				/// Id objektu, ktoreho sa karma tyka.
				/// </summary>
				public int ObjectId
				{
						get
						{
								object o = ViewState["ObjectId"];
								return o != null ? Convert.ToInt32( o ) : 0;
						}
						set { ViewState["ObjectId"] = value; }
				}

				/// <summary>
				/// Id objektu, ktoreho sa karma tyka.
				/// </summary>
				public int ObjectTypeId
				{
						get
						{
								object o = ViewState["ObjectTypeId"];
								return o != null ? Convert.ToInt32( o ) : 0;
						}
						set { ViewState["ObjectTypeId"] = value; }
				}

				public double RatingResult { get; set; }


				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Attributes.Add( "class", this.CssClass );
						div.Attributes.Add( "id", this.ClientID );

						Control control = CreateDetailControl();
						if ( control != null )
								div.Controls.Add( control );

						this.Controls.Add( div );
				}
				#endregion

				/// <summary>
				/// Vytvori Control Carmy
				/// </summary>
				private Control CreateDetailControl()
				{
						this.lblRatingResult = new Label();
						this.lblRatingResult.ID = "lblRatingResult";

						this.btnNegativeVote = new Button();
						this.btnNegativeVote.ID = "btnNegativeVote";
						this.btnNegativeVote.CausesValidation = false;
						this.btnNegativeVote.Click += new EventHandler( btnNegativeVote_Click );

						this.btnPositiveVote = new Button();
						this.btnPositiveVote.ID = "btnPositiveVote";
						this.btnPositiveVote.CausesValidation = false;
						this.btnPositiveVote.Click += new EventHandler( btnPositiveVote_Click );

						Table table = new Table();
						table.CellPadding = 0;
						table.CellSpacing = 0;
						//table.Attributes.Add( "border", "1" );

						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						this.btnNegativeVote.CssClass = this.CssClass + "_negative_carma_button";
						cell.HorizontalAlign = HorizontalAlign.Left;
						cell.Controls.Add( this.btnNegativeVote );
						row.Cells.Add( cell );

						//Carma
						this.lblRatingResult.Text = this.RatingResult.ToString( "F2" );
						cell = new TableCell();
						cell.CssClass = this.CssClass + ( this.RatingResult >= 0 ? "_positive_carma_label" : "_negative_carma_label" );
						cell.HorizontalAlign = HorizontalAlign.Center;
						cell.Controls.Add( this.lblRatingResult );
						row.Cells.Add( cell );

						//Positive carma button
						cell = new TableCell();
						this.btnPositiveVote.CssClass = this.CssClass + "_positive_carma_button";
						cell.HorizontalAlign = HorizontalAlign.Right;
						cell.Controls.Add( this.btnPositiveVote );
						row.Cells.Add( cell );

						table.Rows.Add( row );
						return table;
				}

				void btnPositiveVote_Click( object sender, EventArgs e )
				{
						if ( !Security.IsLogged( true ) )
								return;

						//Administrator moze hlasovat kolko chce.
						if ( !Security.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) )
						{
								int count = Storage<AccountVoteEntity>.Count( new AccountVoteEntity.CountBy
								{
										AccountId = Security.Account.Id,
										ObjectId = this.ObjectId,
										ObjectTypeId = (int)this.ObjectTypeId
								} );
								if ( count != 0 )
										return;
						}

						if ( OnVote != null )
						{
								double carma = OnVote( this.ObjectId, 1 );
								this.lblRatingResult.Text = carma.ToString( "F2" );
						}
				}

				void btnNegativeVote_Click( object sender, EventArgs e )
				{
						if ( !Security.IsLogged( true ) )
								return;

						int count = Storage<AccountVoteEntity>.Count( new AccountVoteEntity.CountBy
						{
								AccountId = Security.Account.Id,
								ObjectId = this.ObjectId,
								ObjectTypeId = this.ObjectTypeId
						} );
						if ( count != 0 )
								return;

						if ( OnVote != null )
						{
								double carma = OnVote( this.ObjectId, -1 );
								this.lblRatingResult.Text = carma.ToString( "F2" );
						}
				}
		}
}
