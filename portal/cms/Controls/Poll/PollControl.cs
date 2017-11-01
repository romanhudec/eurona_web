using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Entities;
using PollEntity = CMS.Entities.Poll;

namespace CMS.Controls.Poll
{
		public class PollControl: CmsControl
		{
				public PollControl()
				{
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						Control pollControl = CreatePollControl();
						if ( pollControl != null ) this.Controls.Add( pollControl );
						else this.Visible = false;

				}
				#endregion

				/// <summary>
				/// Vráti-Nastaví anketu, ktorá sa zobrazuje. 
				/// Ak nie je vyplnená vráti aktuálnu anketu.
				/// </summary>
				public int? PollId
				{
						get
						{
								object o = ViewState["PollId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["PollId"] = value; }
				}

				/// <summary>
				/// Ak je True, header (otazka) sa nebude zobrazovat.
				/// </summary>
				public bool HideHeader { get; set; }


				/// <summary>
				/// Vytvori Control Ankety
				/// </summary>
				private Control CreatePollControl()
				{
						PollEntity poll = null;
						if ( !PollId.HasValue ) poll = Storage<PollEntity>.ReadFirst( new PollEntity.ReadActivePoll() );
						else poll = Storage<PollEntity>.ReadFirst( new PollEntity.ReadById() { PollId = this.PollId.Value } );
						if ( poll == null )
								return null;

						//Zisti ci uz sa z danej IP hlasovalo.
						PollAnswer answer = Storage<PollAnswer>.ReadFirst( new PollAnswer.ReadByPollAndIP() { PollId = poll.Id, IP = this.UniqueVoteId } );
						bool readOnly = answer != null || poll.Closed;

						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Attributes.Add( "class", this.CssClass );

						// Poll Header
						Table pollBoxTable = new Table();
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = this.CssClass + "_header";
						cell.Text = poll.Question;
						row.Cells.Add( cell );

						if ( !this.HideHeader )
								pollBoxTable.Rows.Add( row );

						// Poll options
						foreach ( PollOption option in poll.Options )
						{
								row = new TableRow();
								cell = new TableCell();
								cell.Controls.Add( CreatePollOption( option, poll.VotesTotal, readOnly ) );
								row.Cells.Add( cell );
								pollBoxTable.Rows.Add( row );
						}

						//Total votes
						row = new TableRow();
						cell = new TableCell();
						cell.CssClass = this.CssClass + "_footer";
						cell.Text = string.Format( "{0}: {1}", Resources.Controls.PollControl_VoteTotal, poll.VotesTotal );
						row.Cells.Add( cell );
						pollBoxTable.Rows.Add( row );

						div.Controls.Add( pollBoxTable );

						return div;
				}

				private Control CreatePollOption( PollOption option, int votesTotal, bool readOnly )
				{
						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Attributes.Add( "class", this.CssClass + "_option" );

						//Option Name
						HtmlGenericControl divName = new HtmlGenericControl( "div" );
						divName.Attributes.Add( "class", this.CssClass + "_optionName" );
						divName.Controls.Add( CreatePollOptionLink( option, readOnly ) );

						//Option Votes
						double percent = 0.0F;
						if ( votesTotal != 0 )
								percent = ( ( option.Votes * 100.0 ) / votesTotal );

						#region Vote Bar
						Table tableVote = new Table();
						tableVote.CssClass = this.CssClass + "_voteBar";
						tableVote.Width = Unit.Percentage( Math.Max( 0.01F, percent ) );
						tableVote.CellPadding = 0;
						tableVote.CellSpacing = 0;
						TableRow row = new TableRow();
						TableCell cellLeft = new TableCell();
						TableCell cellMidle = new TableCell();
						TableCell cellRight = new TableCell();

						cellLeft.CssClass = this.CssClass + "_optionVoteBarLeft";
						cellMidle.CssClass = this.CssClass + "_optionVoteBar";
						cellRight.CssClass = this.CssClass + "_optionVoteBarRight";

						row.Cells.Add( cellLeft );
						row.Cells.Add( cellMidle );
						row.Cells.Add( cellRight );
						tableVote.Rows.Add( row );
						#endregion

						HtmlGenericControl divVotePercent = new HtmlGenericControl( "div" );
						divVotePercent.Controls.Add( new LiteralControl( string.Format( "{0:F2}%", percent ) ) );

						div.Controls.Add( divName );
						div.Controls.Add( tableVote );
						div.Controls.Add( divVotePercent );

						return div;
				}

				private Control CreatePollOptionLink( PollOption option, bool readOnly )
				{
						if ( readOnly )
								return new LiteralControl( option.Name );

						LinkButton lb = new LinkButton();
						lb.Text = option.Name;
						lb.CommandArgument = option.Id.ToString();
						lb.Click += new EventHandler( OnVote );
						return lb;
				}

				void OnVote( object sender, EventArgs e )
				{
						string arg = ( sender as LinkButton ).CommandArgument;
						if ( string.IsNullOrEmpty( arg ) )
								return;

						PollAnswer answer = new PollAnswer();
						answer.PollOptionId = Convert.ToInt32( arg );
						answer.IP = this.UniqueVoteId;

						Storage<PollAnswer>.Create( answer );

						this.Controls.Clear();
						Control pollControl = CreatePollControl();
						if ( pollControl != null )
								this.Controls.Add( pollControl );
				}

				/// <summary>
				/// Vráti jedinečný identifikátor voliča.
				/// </summary>
				private string UniqueVoteId
				{
						get
						{
								return Request.UserHostAddress;
						}
				}
		}
}
