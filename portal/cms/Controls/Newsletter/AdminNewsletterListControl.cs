using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using NewsletterEntity = CMS.Entities.Newsletter;
using AccountEntity = CMS.Entities.Account;
using System.Configuration;
using System.Net.Mail;
using Telerik.Web.UI;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.Mime;

namespace CMS.Controls.Newsletter
{
		public class AdminNewsletterListControl: CmsControl
		{
				public enum EmailImageMethod
				{
						Html = 1,
						InlineAttachment = 2,
						LinkedResources = 3
				}

				private const string DELETE_COMMAND = "DELETE_ITEM";
				private const string EDIT_COMMAND = "EDIT_ITEM";
				private const string SEND_COMMAND = "SEND_ITEM";

				private RadGrid dataGrid = null;

				public SortDirection SortDirection
				{
						get { return GetState<SortDirection>( "AdminNewsletterListControl-SortDirection", SortDirection.Descending ); }
						set { SetState<SortDirection>( "AdminNewsletterListControl-SortDirection", value ); }
				}
				public string SortExpression
				{
						get { return GetState<string>( "AdminNewsletterListControl-SortExpression" ); }
						set { SetState<string>( "AdminNewsletterListControl-SortExpression", value ); }
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						this.dataGrid = CreateGridControl();
						this.Controls.Add( this.dataGrid );

						//Binding
						this.dataGrid.DataSource = GetDataGridData();
						if ( !IsPostBack )
								this.dataGrid.DataBind();

				}
				#endregion

				public string NewUrl { get; set; }
				public string EditUrlFormat { get; set; }

				/// <summary>
				/// Vytvori DataGrid control s pozadovanymi stlpcami a 
				/// pripravenym bindingom stlpcou.
				/// </summary>
				private RadGrid CreateGridControl()
				{
						RadGrid grid = new RadGrid();
						CMS.Utilities.RadGridUtilities.Localize( grid );
						grid.MasterTableView.DataKeyNames = new string[] { "Id" };

						grid.AllowAutomaticInserts = true;
						grid.AllowFilteringByColumn = true;
						grid.ShowStatusBar = false;
						grid.ShowGroupPanel = true;
						grid.GroupingEnabled = true;
						grid.GroupingSettings.ShowUnGroupButton = true;
						grid.ClientSettings.AllowDragToGroup = true;
						grid.ClientSettings.AllowColumnsReorder = true;

						grid.MasterTableView.ShowHeader = true;
						grid.MasterTableView.ShowFooter = false;
						grid.MasterTableView.AllowPaging = true;
						grid.MasterTableView.PageSize = 25;
						grid.MasterTableView.PagerStyle.AlwaysVisible = true;
						grid.MasterTableView.AllowSorting = true;
						grid.MasterTableView.GridLines = GridLines.None;
						grid.MasterTableView.AutoGenerateColumns = false;

						grid.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.TopAndBottom;
						grid.MasterTableView.CommandItemSettings.AddNewRecordText = Resources.Controls.AdminNewsletterListControl_NewNewsletter;

						GridBoundColumn bf = new GridBoundColumn();
						bf.DataField = "Date";
						bf.HeaderText = Resources.Controls.AdminNewsletterListControl_ColumnDate;
						bf.SortExpression = "Date";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						bf.DataFormatString = "{0:d}";
						grid.Columns.Add( bf );

						bf = new GridBoundColumn();
						bf.DataField = "Subject";
						bf.HeaderText = Resources.Controls.AdminNewsletterListControl_ColumnSubject;
						bf.SortExpression = "Subject";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						bf = new GridBoundColumn();
						bf.DataField = "SendDate";
						bf.HeaderText = Resources.Controls.AdminNewsletterListControl_ColumnSendDate;
						bf.SortExpression = "Date";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						bf.DataFormatString = "{0:d}";
						grid.Columns.Add( bf );

						GridButtonColumn btnSend = new GridButtonColumn();
						btnSend.HeaderStyle.Width = Unit.Pixel( 16 );
						btnSend.ImageUrl = ConfigValue( "CMS:EmailButtonImage" );
						btnSend.Text = Resources.Controls.GridView_ToolTip_SendEmail;
						btnSend.ButtonType = GridButtonColumnType.ImageButton;
						btnSend.CommandName = SEND_COMMAND;
						grid.Columns.Add( btnSend );

						GridButtonColumn btnEdit = new GridButtonColumn();
						btnEdit.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnEdit.ImageUrl = ConfigValue( "CMS:EditButtonImage" );
						btnEdit.Text = Resources.Controls.GridView_ToolTip_EditItem;
						btnEdit.ButtonType = GridButtonColumnType.ImageButton;
						btnEdit.CommandName = EDIT_COMMAND;
						grid.MasterTableView.Columns.Add( btnEdit );

						GridButtonColumn btnDelete = new GridButtonColumn();
						btnDelete.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnDelete.ImageUrl = ConfigValue( "CMS:DeleteButtonImage" );
						btnDelete.Text = Resources.Controls.GridView_ToolTip_DeleteItem;
						btnDelete.ButtonType = GridButtonColumnType.ImageButton;
						btnDelete.ConfirmTitle = Resources.Controls.DeleteItemQuestion;
						btnDelete.ConfirmText = Resources.Controls.DeleteItemQuestion;
						btnDelete.CommandName = DELETE_COMMAND;
						grid.MasterTableView.Columns.Add( btnDelete );

						grid.ItemCommand += OnRowCommand;

						return grid;
				}

				private List<NewsletterEntity> GetDataGridData()
				{
						List<NewsletterEntity> list = Storage<NewsletterEntity>.Read();
						SortDirection previous = SortDirection;
						string sortExpression = String.IsNullOrEmpty( SortExpression ) ? "Date" : SortExpression;
						var ordered = list.AsQueryable().OrderBy( sortExpression + " " + ( previous == SortDirection.Ascending ? "ascending" : "descending" ) );
						return ordered.ToList();
				}

				#region Event handlers
				void OnRowCommand( object sender, GridCommandEventArgs e )
				{
						if ( e.CommandName == RadGrid.InitInsertCommandName ) OnNewCommand( sender, e );
						if ( e.CommandName == EDIT_COMMAND ) OnEditCommand( sender, e );
						if ( e.CommandName == DELETE_COMMAND ) OnDeleteCommand( sender, e );
						if ( e.CommandName == SEND_COMMAND ) OnSendCommand( sender, e );
				}
				private void OnNewCommand( object sender, GridCommandEventArgs e )
				{
						string url = Page.ResolveUrl( string.Format( "{0}?{1}", this.NewUrl, base.BuildReturnUrlQueryParam() ) );
						Response.Redirect( url );
				}
				private void OnEditCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int id = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						string url = Page.ResolveUrl( string.Format( EditUrlFormat, id ) + "&" + base.BuildReturnUrlQueryParam() );
						Response.Redirect( url );
				}
				private void OnDeleteCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int id = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						NewsletterEntity newletter = Storage<NewsletterEntity>.ReadFirst( new NewsletterEntity.ReadById { NewsletterId = id } );
						Storage<NewsletterEntity>.Delete( newletter );

						this.dataGrid.DataSource = GetDataGridData();
						this.dataGrid.DataBind();
				}
				private void OnSendCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int id = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						NewsletterEntity newletter = Storage<NewsletterEntity>.ReadFirst( new NewsletterEntity.ReadById { NewsletterId = id } );
						string[] roleArray = newletter.Roles.Split( new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries );

						List<AccountEntity> list = Storage<AccountEntity>.Read();
						if ( list.Count == 0 )
								return;

						using ( MailMessage msg = new MailMessage() )
						{
								msg.From = new MailAddress( ConfigurationManager.AppSettings["SMTP:From"],
								ConfigurationManager.AppSettings["SMTP:FromDisplay"] );

								foreach ( AccountEntity account in list )
								{
										foreach ( string role in roleArray )
										{
												if ( !account.IsInRole( role ) )
														continue;

												msg.Bcc.Add( new MailAddress( account.Email, account.Email ) );
												break;
										}
								}

								if ( msg.Bcc.Count == 0 )
										return;

								bool useSSL = false;
								string strUseSSL = ConfigurationManager.AppSettings["SMTP:UseSSL"];
								if ( !string.IsNullOrEmpty( strUseSSL ) )
										Boolean.TryParse( strUseSSL, out useSSL );

								msg.Subject = newletter.Subject;
								msg.IsBodyHtml = true;
								FormatEmaiBodyLinks( msg, newletter.Content, EmailImageMethod.Html );

								SmtpClient smtp = new SmtpClient();
								smtp.EnableSsl = useSSL;
								smtp.Send( msg );
						}

						newletter.SendDate = DateTime.Now;
						Storage<NewsletterEntity>.Update( newletter );

						//Update data view
						this.dataGrid.DataSource = GetDataGridData();
						this.dataGrid.DataBind();
				}

				private void FormatEmaiBodyLinks( MailMessage email, string content, EmailImageMethod method )
				{
						if ( method == EmailImageMethod.InlineAttachment )
						{
								List<ContentImageInfo> images = GetImagesFromContent( content );
								foreach ( ContentImageInfo img in images )
								{
										content = content.Replace( img.VirtualPath, "cid:" + img.CID );

										// create the INLINE attachment
										Attachment inline = new Attachment( img.ImagePath );
										inline.ContentDisposition.Inline = true;
										inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
										inline.ContentId = img.CID;
										inline.ContentType.MediaType = img.MediaType;
										inline.ContentType.Name = Path.GetFileName( img.ImagePath );
										email.Attachments.Add( inline );
								}

								StringBuilder sb = new StringBuilder();
								if ( !content.ToLower().Contains( "<html>" ) )
								{
										sb.Append( "<html><head></head><body>" );
										sb.Append( content );
										sb.Append( "</body></html>" );
								}
								else sb.Append( content );

								email.Body = sb.ToString();
						}
						else if ( method == EmailImageMethod.LinkedResources )
						{
								List<ContentImageInfo> images = GetImagesFromContent( content );
								foreach ( ContentImageInfo img in images )
										content = content.Replace( img.VirtualPath, "cid:" + img.CID );

								StringBuilder sb = new StringBuilder();
								if ( !content.ToLower().Contains( "<html>" ) )
								{
										sb.Append( "<html><head></head><body>" );
										sb.Append( content );
										sb.Append( "</body></html>" );
								}
								else sb.Append( content );

								AlternateView av = AlternateView.CreateAlternateViewFromString( sb.ToString(), null, MediaTypeNames.Text.Html );
								foreach ( ContentImageInfo img in images )
								{
										LinkedResource lr = new LinkedResource( img.ImagePath );
										lr.ContentId = img.CID;
										av.LinkedResources.Add( lr );
								}

								email.AlternateViews.Add( av );

						}
						else
						{
								string path = Page.ResolveUrl( ConfigValue( "RadEditor:UserFilesPath" ) );
								string url = Utilities.ServerUtilities.Root( this.Request ) + ( path.StartsWith( "/" ) ? path.Remove( 0, 1 ) : path );
								content = content.Replace( path, url );

								StringBuilder sb = new StringBuilder();
								if ( !content.ToLower().Contains( "<html>" ) )
								{
										sb.Append( "<html><head></head><body>" );
										sb.Append( content );
										sb.Append( "</body></html>" );
								}
								else sb.Append( content );

								content = Server.HtmlDecode( sb.ToString() );
								email.Body = content;
						}

				}

				/// <summary>
				/// Extract all images from html content
				/// </summary>
				private List<ContentImageInfo> GetImagesFromContent( string content )
				{
						List<ContentImageInfo> imageList = new List<ContentImageInfo>();

						MatchCollection match = Regex.Matches( content.ToLower(), @"(<img.*?/>)", RegexOptions.Singleline );
						foreach ( Match m in match )
						{
								string img = m.Groups[1].Value;
								Match mSrc = Regex.Match( img, @"src=\""(.*?)\""", RegexOptions.Singleline );
								if ( mSrc.Success )
								{
										string virtualPath = mSrc.Groups[1].Value;
										string filePath = Server.MapPath( virtualPath );

										ContentImageInfo imgInfo = new ContentImageInfo();
										imgInfo.VirtualPath = virtualPath;
										imgInfo.ImagePath = filePath;
										imgInfo.CID = "@IMG_" + Path.GetFileName( filePath ).ToUpper().Replace( ".", "" );
										imgInfo.MediaType = string.Format( "image/{0}", Path.GetExtension( filePath ).Replace( ".", "" ).ToLower() );
										imageList.Add( imgInfo );
								}
						}

						return imageList;
				}

				private class ContentImageInfo
				{
						public string VirtualPath { get; set; }
						public string ImagePath { get; set; }
						public string CID { get; set; }
						public string MediaType { get; set; }
				}
				#endregion
		}
}
