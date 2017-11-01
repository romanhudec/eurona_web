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
		public class RaitingControl: CmsControl
		{
				private const int MAX_RATING = 5;
				private const string ButtonIDFormat = "btn_{0}";

				public delegate double VoteEventHandler( int objectId, int rating );
				public event VoteEventHandler OnVote;

				private Label lblRating;

				public RaitingControl()
				{
				}

				/// <summary>
				/// Id objektu, ktoreho sa hlasovanie tyka.
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
				/// Id objektu, ktoreho sa hlasovanie tyka.
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

				public double RatingResult
				{
						get
						{
								object o = ViewState["RatingResult"];
								return o != null ? Convert.ToDouble( o ) : 0.0F;
						}
						set { ViewState["RatingResult"] = value; }
				}


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
						this.lblRating = new Label();
						this.lblRating.ID = "lblRating";

						//Zaokruhlenie na cele hviezdicky
						int currentRating = (int)Math.Round( this.RatingResult );

						Table table = new Table();
						table.CellPadding = 0;
						table.CellSpacing = 0;
						//table.Attributes.Add( "border", "1" );

						TableRow row = new TableRow();
						TableCell cell = null;
						for ( int star = 1; star <= MAX_RATING; star++ )
						{
								Button btn = new Button();
								btn.ID = string.Format( ButtonIDFormat, star );
								btn.CausesValidation = false;
								btn.Enabled = this.IsEditing;
								btn.CommandArgument = star.ToString();
								btn.Click += ( sender, e ) =>
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
														int rating = Convert.ToInt32( ( sender as Button ).CommandArgument );
														double ratingResult = OnVote( this.ObjectId, rating );
														this.lblRating.Text = string.Format( "{0} : {1}", Resources.Controls.RaitingControl_RatingLabel, ratingResult.ToString( "F2" ) );

														UpdateVisualRating( ratingResult );
												}
										};

								if ( currentRating >= star ) btn.CssClass = this.CssClass + "_positive_star_button";
								else btn.CssClass = this.CssClass + "_negative_star_button";

								cell = new TableCell();
								cell.HorizontalAlign = HorizontalAlign.Left;
								cell.Controls.Add( btn );
								row.Cells.Add( cell );
						}

						this.lblRating.Text = string.Format( "{0} : {1}", Resources.Controls.RaitingControl_RatingLabel, this.RatingResult.ToString( "F2" ) );
						cell = new TableCell();
						cell.CssClass = this.CssClass + "_rating";
						cell.Wrap = false;
						cell.HorizontalAlign = HorizontalAlign.Left;
						cell.Controls.Add( this.lblRating );
						row.Cells.Add( cell );

						table.Rows.Add( row );
						return table;
				}

				/// <summary>
				/// Metoda nastavy css styl pre vsetky tlacidla podla aktualneho ratingu.
				/// </summary>
				private void UpdateVisualRating( double ratingResult )
				{
						int currentRating = (int)Math.Round( ratingResult );
						for ( int star = 1; star <= MAX_RATING; star++ )
						{
								Button btn = (Button)this.FindControl( string.Format( ButtonIDFormat, star ) );
								if ( btn == null ) continue;

								if ( currentRating >= star ) btn.CssClass = this.CssClass + "_positive_star_button";
								else btn.CssClass = this.CssClass + "_negative_star_button";
						}
				}
		}
}
