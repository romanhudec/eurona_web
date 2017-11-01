using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using ForumPostEntity = CMS.Entities.ForumPost;
using ForumEntity = CMS.Entities.Forum;
using ForumThreadEntity = CMS.Entities.ForumThread;
using ForumPostAttachmentEntity = CMS.Entities.ForumPostAttachment;
using AccountProfileEntity = CMS.Entities.AccountProfile;
using System.Web.UI;
using System.Data;
using System.ComponentModel;
using System.Web.UI.HtmlControls;

[assembly: WebResource( "CMS.Controls.Forum.ForumPosts.js", "application/x-javascript", PerformSubstitution = true )]
namespace CMS.Controls.Forum
{
		// do not show the StatsData class in the Toolbox...
		[ToolboxItem( false )]
		public class ForumPostData: WebControl, INamingContainer
		{
				private ForumPostEntity post;
				private ForumPostsControl owner;

				internal ForumPostData( ForumPostsControl owner, ForumPostEntity post )
						: base( "div" )
				{
						this.owner = owner;
						this.post = post;
						this.CssClass = owner.CssClass + "_container";
						this.Childs = new List<ForumPostData>();
				}

				[Bindable( true )]
				public int Id
				{
						get { return this.post.Id; }
				}

				[Bindable( true )]
				public int? ParentId
				{
						get { return this.post.ParentId; }
				}

				[Bindable( true )]
				public int AccountId
				{
						get { return this.post.AccountId; }
				}

				[Bindable( true )]
				public DateTime Date
				{
						get { return this.post.Date; }
				}

				[Bindable( true )]
				public string AccountName
				{
						get
						{
								//Zisti sa polozka profilu pouzivatela, ktora sa bude zobrazovat ako autor.
								AccountProfileEntity ap = Storage<AccountProfileEntity>.ReadFirst( new AccountProfileEntity.ReadByAccountAndProfileName { AccountId = this.post.AccountId, ProfileName = owner.AccountProfileItemName } );
								string accountName = ( ap != null && ap.Value != string.Empty ) ? ap.Value : this.post.AccountName;

								int atIndex = accountName.IndexOf( "@" );
								if ( atIndex != -1 )
										accountName = accountName.Substring( 0, atIndex+1 ) + "...";

								return accountName;
						}
				}

				[Bindable( true )]
				public string AccountPhoto
				{
						get
						{
								//Zisti sa polozka profilu pouzivatela, ktora sa bude zobrazovat ako autor.
								AccountProfileEntity ap = Storage<AccountProfileEntity>.ReadFirst( new AccountProfileEntity.ReadByAccountAndProfileName { AccountId = this.post.AccountId, ProfileName = owner.AccountProfileItemPhoto } );
								return ( ap != null && ap.Value != string.Empty ) ? ap.Value : null;
						}
				}

				[Bindable( true )]
				public string Title
				{
						get { return this.post.Title; }
				}

				[Bindable( true )]
				public string Content
				{
						get { return this.post.Content; }
				}

				[Bindable( true )]
				public double RatingResult
				{
						get { return this.post.RatingResult; }
				}

				[Bindable( true )]
				public List<ForumPostAttachmentEntity> Attachments
				{
						get { return this.post.Attachments; }
				}

				[Bindable( true )]
				public List<ForumPostData> Childs { get; set; }

		}

		public class ForumPostsControl: CmsControl
		{
				public delegate string ContentProcessorHandler( ForumPostsControl sender, string content );
				public event ContentProcessorHandler ContentProcessor;
				public enum ViewType: int
				{
						List = 1,
						Tree = 2
				}

				[DefaultValue( 5 )]
				public int PagerSize { get; set; }

				[DefaultValue( true )]
				public bool ShowForumHeader { get; set; }

				[DefaultValue( false )]
				public bool ColapsibleAttachment { get; set; }

				/// <summary>
				/// Nastavi názov položky z profilu používateľa, ktorá sa má zobrazovať ako autor.
				/// </summary>
				public string AccountProfileItemName { get; set; }
				/// <summary>
				/// Nastavi fotografia položky z profilu používateľa, ktorá sa má zobrazovať ako autor.
				/// </summary>
				public string AccountProfileItemPhoto { get; set; }
				/// <summary>
				/// Url na obsah fora (forumThreads)
				/// </summary>
				public string ForumContentUrl { get; set; }

				/// <summary>
				/// Url na forum, daneho postu
				/// </summary>
				public string DisplayUrlFormat { get; set; }
				public string CssCarma { get; set; }

				private ForumEntity forum = null;
				public ForumEntity Forum
				{
						get
						{
								if ( this.forum != null ) return this.forum;
								this.forum = Storage<ForumEntity>.ReadFirst( new ForumEntity.ReadById { ForumId = this.ForumId } );
								return this.forum;
						}
				}

				private RadioButton rbViewTree = null;
				private RadioButton rbViewList = null;
				private HtmlGenericControl divDataContainer = null;

				public int ForumId
				{
						get { return ViewState["ForumId"] == null ? 0 : Convert.ToInt32( ViewState["ForumId"] ); }
						set { ViewState["ForumId"] = value; }
				}
				public string CommentFormID
				{
						get { return ViewState["CommentFormID"] == null ? string.Empty : ViewState["CommentFormID"].ToString(); }
						set { ViewState["CommentFormID"] = value; }
				}

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "The post template." )]
				[TemplateContainer( typeof( ForumPostData ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ForumPostsTemplate { get; set; }

				#region Protected overrides
				private List<ForumPostData> BuildTreeData( List<ForumPostEntity> entityList )
				{
						List<ForumPostData> list = new List<ForumPostData>();
						if ( this.rbViewTree.Checked )
						{
								//Zobrazenie v strome
								foreach ( ForumPostEntity entity in entityList )
								{
										if ( entity.ParentId != null ) continue;

										ForumPostData data = new ForumPostData( this, entity );
										data.Childs.AddRange( GetChilds( data.Id, entityList ) );
										list.Add( data );
								}
						}
						else
						{
								//Zobrazenie listu
								foreach ( ForumPostEntity entity in entityList )
								{
										ForumPostData data = new ForumPostData( this, entity );
										list.Add( data );
								}
						}

						return list;
				}
				private List<ForumPostData> GetChilds( int parentId, List<ForumPostEntity> entityList )
				{
						List<ForumPostData> list = new List<ForumPostData>();
						foreach ( ForumPostEntity entity in entityList )
						{
								if ( entity.ParentId != parentId ) continue;

								ForumPostData data = new ForumPostData( this, entity );
								data.Childs.AddRange( GetChilds( data.Id, entityList ) );
								list.Add( data );
						}

						return list;
				}
				public override void RenderControl( HtmlTextWriter writer )
				{
						writer.Write( string.Format( "<div id='{0}' class='{1}'>", this.ClientID, this.CssClass ) );
						base.RenderControl( writer );
						writer.Write( "</div>" );
				}

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						if ( this.Forum == null )
								return;

						ForumThreadEntity ft = Storage<ForumThreadEntity>.ReadFirst( new ForumThreadEntity.ReadById { ForumThreadId = this.Forum.ForumThreadId } );
						if ( ft == null ) return;
						if ( !this.IsVisibleForRole( ft ) ) return;

						//Incerement View Count
						ForumEntity.IncrementViewCountCommand cmd = new ForumEntity.IncrementViewCountCommand();
						cmd.ForumId = this.ForumId;
						Storage<ForumEntity>.Execute( cmd );

						if ( this.ShowForumHeader )
						{
								Control forulHeaderControl = CreateForumHeaderControl( ft );
								this.Controls.Add( forulHeaderControl );
						}

						//Title Fora
						HtmlGenericControl divForumTitle = new HtmlGenericControl( "div" );
						divForumTitle.Attributes.Add( "class", this.CssClass + "_title" );
						HyperLink hlForumTitle = new HyperLink();
						hlForumTitle.Text = this.Forum.Name;
						if ( !string.IsNullOrEmpty( this.Forum.Alias ) ) hlForumTitle.NavigateUrl = Page.ResolveUrl( this.Forum.Alias );
						else if ( !string.IsNullOrEmpty( this.DisplayUrlFormat ) )
								hlForumTitle.NavigateUrl = string.Format( Page.ResolveUrl( this.DisplayUrlFormat ), this.ForumId );
						divForumTitle.Controls.Add( hlForumTitle );
						//this.Controls.Add( divForumTitle );

						HtmlGenericControl divForumDescription = new HtmlGenericControl( "div" );
						divForumDescription.Attributes.Add( "class", this.CssClass + "_description" );
						Label lblDescription = new Label();
						lblDescription.Text = FormatTextToView( this.Forum.Description );
						divForumDescription.Controls.Add( lblDescription );
						//this.Controls.Add( divForumDescription );

						Table tbl = new Table(); //tbl.Attributes.Add("border", "1");
						TableRow row = new TableRow(); tbl.Rows.Add( row );
						TableCell cellProfile = new TableCell();
						cellProfile.RowSpan = 3;
						row.Cells.Add( cellProfile );
						//Title
						TableCell cell = new TableCell();
						cell.Controls.Add( divForumTitle ); row.Cells.Add( cell );
						//Created by and date
						row = new TableRow(); tbl.Rows.Add( row );
						cell = new TableCell();
						cell.Controls.Add( new LiteralControl( this.Forum.CreatedByAccountName ) );
						cell.Controls.Add( new LiteralControl( " | " ) );
						cell.Controls.Add( new LiteralControl( this.Forum.CreatedDate.ToString() ) ); 
						row.Cells.Add( cell );

						//Desription
						row = new TableRow(); tbl.Rows.Add( row );
						cell = new TableCell();
						cell.Controls.Add( divForumDescription ); row.Cells.Add( cell );
						this.Controls.Add(tbl);

						if ( !string.IsNullOrEmpty( this.AccountProfileItemPhoto ) )
						{
								cellProfile.CssClass = this.CssClass + "_item_photo";
								cellProfile.VerticalAlign = VerticalAlign.Top;

								Image photo = new Image();
								cellProfile.Controls.Add( photo );
								photo.DataBinding += ( sender, e ) =>
								{
										ForumPostsControl data = (ForumPostsControl)photo.NamingContainer;
										Image control = sender as Image;
										AccountProfileEntity ap = Storage<AccountProfileEntity>.ReadFirst( new AccountProfileEntity.ReadByAccountAndProfileName { AccountId = this.Forum.CreatedByAccountId, ProfileName = this.AccountProfileItemPhoto } );
										string imageUrl = ( ap != null && ap.Value != string.Empty ) ? ap.Value : null;

										if ( imageUrl != null ) control.ImageUrl = Page.ResolveUrl(imageUrl);
										else
										{
												control.Visible = false;
												( control.Parent as TableCell ).CssClass = this.CssClass + "_item_nophoto";
										}
								};
						}

						//BR
						this.Controls.Add( new LiteralControl( "<br/>" ) );

						//Iba prihlaseny pouzivatel
						HtmlGenericControl divReply = new HtmlGenericControl( "div" );
						HyperLink hlReply = new HyperLink();
						hlReply.CssClass = this.CssClass + "_item_reply";
						hlReply.Text = Resources.Controls.ForumPostsControl_Reply;
						hlReply.DataBinding += ( sender, e ) =>
						{
								HtmlGenericControl control = sender as HtmlGenericControl;
								hlReply.Attributes.Add( "onclick", string.Format( "showReplyForm('{0}', '{1}', null )",
										this.CommentFormID,
										this.ClientID ) );
								hlReply.Visible = this.Forum.Locked == false && Security.IsLogged( false );

						};
						divReply.Controls.Add( hlReply );
						this.Controls.Add( divReply );

						//BR
						this.Controls.Add( new LiteralControl( "<br/>" ) );

						//Template list
						this.divDataContainer = new HtmlGenericControl( "div" );

						ITemplate template = this.ForumPostsTemplate;
						ViewType viewType = ViewType.List;
						if ( this.rbViewTree.Checked ) viewType = ViewType.Tree;
						if ( template == null ) template = new DefaultForumPostTemplate( this, viewType, this.Forum.Locked );

						List<ForumPostEntity> list = Storage<ForumPostEntity>.Read( new ForumPostEntity.ReadByForumId { ForumId = this.ForumId } );
						List<ForumPostData> dataSource = BuildTreeData( list );

						foreach ( ForumPostData data in dataSource )
						{
								template.InstantiateIn( data );
								this.divDataContainer.Controls.Add( data );
						}

						this.Controls.Add( this.divDataContainer );
						this.DataBind();

						ClientScriptManager cs = this.Page.ClientScript;
						Type cstype = this.GetType();
						string urlInclude = cs.GetWebResourceUrl( typeof( ForumPostFormControl ), "CMS.Controls.Forum.ForumPosts.js" );
						cs.RegisterClientScriptInclude( cstype, "ForumPostsJs", urlInclude );
				}
				public string FormatTextToView( string content )
				{
						if ( ContentProcessor != null ) content = ContentProcessor( this, content );
						return content;
				}
				private bool IsVisibleForRole( ForumThreadEntity ft )
				{
						if ( string.IsNullOrEmpty( ft.VisibleForRole ) ) return true;
						else
						{
								if ( !Security.IsLogged( false ) ) return false;
								if ( Security.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) ) return true;

								foreach ( string role in Security.Account.RoleArray )
								{
										if ( ft.VisibleForRole.Contains( role ) )
												return true;
								}

								return false;
						}
				}
				#endregion
				protected virtual Control CreateForumHeaderControl( ForumThreadEntity ft )
				{
						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Attributes.Add( "class", this.CssClass + "_forumHeader" );

						Table table = new Table();
						TableRow row = new TableRow(); table.Rows.Add( row );

						Utilities.AliasUtilities aliasUtils = new Utilities.AliasUtilities();
						string alias = aliasUtils.Resolve( this.ForumContentUrl, this.Page );
						HyperLink hlObsahFora = new HyperLink();
						hlObsahFora.Text = Resources.Controls.ForumsControl_ColumnForumContent;
						hlObsahFora.NavigateUrl = alias;
						TableCell cell = new TableCell();
						cell.CssClass = this.CssClass + "_forumHeader_forumContentLink";
						cell.Wrap = false;
						cell.Controls.Add( hlObsahFora );
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = this.CssClass + "_forumHeader_linkSeparator";
						cell.Wrap = false;
						cell.Controls.Add( new LiteralControl( "»" ) );
						row.Cells.Add( cell );

						HyperLink hlForumThread = new HyperLink();
						hlForumThread.Text = ft.Name;
						hlForumThread.NavigateUrl = Page.ResolveUrl( ft.Alias );
						cell = new TableCell();
						cell.CssClass = this.CssClass + "_forumHeader_link";
						cell.Wrap = false;
						cell.Controls.Add( hlForumThread );
						row.Cells.Add( cell );

						//cell = new TableCell();
						//cell.CssClass = this.CssClass + "_forumHeader_linkSeparator";
						//cell.Wrap = false;
						//cell.Controls.Add( new LiteralControl( "»" ) );
						//row.Cells.Add( cell );

						this.rbViewTree = new RadioButton();
						this.rbViewTree.Text = Resources.Controls.ForumsControl_ViewAsTree_Text;
						this.rbViewTree.AutoPostBack = true;
						this.rbViewTree.GroupName = "postViewMode";
						this.rbViewTree.CheckedChanged += new EventHandler( OnViewCheckedChanged );
						this.rbViewList = new RadioButton();
						this.rbViewList.Text = Resources.Controls.ForumsControl_ViewAsList_Text;
						this.rbViewList.AutoPostBack = true;
						this.rbViewList.GroupName = "postViewMode";
						this.rbViewList.CheckedChanged += new EventHandler( OnViewCheckedChanged );
						cell = new TableCell();
						cell.CssClass = this.CssClass + "_forumHeader_viewType";
						cell.Wrap = false;
						cell.Width = Unit.Percentage( 100 );
						cell.HorizontalAlign = HorizontalAlign.Right;
						cell.VerticalAlign = VerticalAlign.Top;
						cell.Controls.Add( this.rbViewTree );
						cell.Controls.Add( this.rbViewList );
						row.Cells.Add( cell );

						div.Controls.Add( table );

						if ( !this.IsPostBack )
						{
								this.rbViewList.Checked = true;
						}
						return div;
				}

				void OnViewCheckedChanged( object sender, EventArgs e )
				{
						LoadData();
				}

				public override void DataBind()
				{
						this.ChildControlsCreated = true;
						base.DataBind();
				}

				/// <summary>
				/// Metoda naplni/reloadne kontrol datami
				/// </summary>
				public virtual void LoadData()
				{
						this.divDataContainer.Controls.Clear();
						ITemplate template = this.ForumPostsTemplate;
						ViewType viewType = ViewType.List;
						if ( this.rbViewTree.Checked ) viewType = ViewType.Tree;
						if ( template == null ) template = new DefaultForumPostTemplate( this, viewType, this.Forum.Locked );

						List<ForumPostEntity> list = Storage<ForumPostEntity>.Read( new ForumPostEntity.ReadByForumId { ForumId = this.ForumId } );
						List<ForumPostData> dataSource = BuildTreeData( list );

						foreach ( ForumPostData data in dataSource )
						{
								template.InstantiateIn( data );
								this.divDataContainer.Controls.Add( data );
						}

						DataBind();
				}

				internal sealed class DefaultForumPostTemplate: ITemplate
				{
						public string CssClass { get; set; }
						public ForumPostsControl Owner { get; set; }
						public string CommentFormID { get; set; }

						private ViewType viewType;
						private bool locked = false;

						public DefaultForumPostTemplate( ForumPostsControl owner, ViewType viewType, bool locked )
						{
								this.Owner = owner;
								this.viewType = viewType;
								this.locked = locked;
								this.CssClass = owner.CssClass;
								this.CommentFormID = owner.CommentFormID;
						}

						void ITemplate.InstantiateIn( Control container )
						{
								HtmlGenericControl divReplyFormContainer = new HtmlGenericControl( "div" );

								HtmlGenericControl div = new HtmlGenericControl( "div" );
								div.Attributes.Add( "class", this.CssClass + "_item" );

								Table table = new Table();
								//table.Attributes.Add( "border", "1" );
								TableRow row = new TableRow();

								if ( !string.IsNullOrEmpty( Owner.AccountProfileItemPhoto ) )
								{
										TableCell cellPhoto = new TableCell();
										cellPhoto.CssClass = this.CssClass + "_item_photo";
										cellPhoto.VerticalAlign = VerticalAlign.Top;
										cellPhoto.RowSpan = 6;
										row.Cells.Add( cellPhoto );

										Image photo = new Image();
										cellPhoto.Controls.Add( photo );
										photo.DataBinding += ( sender, e ) =>
										{
												ForumPostData data = (ForumPostData)photo.NamingContainer;
												Image control = sender as Image;
												if ( data.AccountPhoto != null ) control.ImageUrl = this.Owner.Page.ResolveUrl( data.AccountPhoto );
												else
												{
														control.Visible = false;
														( control.Parent as TableCell ).CssClass = this.CssClass + "_item_nophoto";
												}
										};
								}

								//Title
								TableCell cell = new TableCell();
								cell = new TableCell();
								cell.VerticalAlign = VerticalAlign.Top;
								cell.CssClass = this.CssClass + "_item_title";
								Label lblTitle = new Label();
								lblTitle.DataBinding += ( sender, e ) =>
								{
										ForumPostData data = (ForumPostData)lblTitle.NamingContainer;
										Label control = sender as Label;
										control.Text = data.Title;
								};
								cell.Controls.Add( lblTitle );
								row.Cells.Add( cell );

								//Carma
								cell = new TableCell();
								cell.RowSpan = 2;
								cell.HorizontalAlign = HorizontalAlign.Right;
								cell.VerticalAlign = VerticalAlign.Middle;
								cell.CssClass = this.CssClass + "_item_carma";
								Vote.CarmaControl carmaControl = new Vote.CarmaControl();
								carmaControl.CssClass = this.Owner.CssCarma;
								carmaControl.OnVote += ( objectId, rating ) =>
								{
										ForumPostEntity.IncrementVoteCommand cmd = new ForumPostEntity.IncrementVoteCommand();
										cmd.AccountId = Security.Account.Id;
										cmd.ForumPostId = objectId;
										cmd.Rating = rating;
										Storage<ForumPostEntity>.Execute( cmd );

										ForumPostEntity post = Storage<ForumPostEntity>.ReadFirst( new ForumPostEntity.ReadById { ForumPostId = objectId } );
										return post.RatingResult;
								};

								carmaControl.DataBinding += ( sender, e ) =>
								{
										ForumPostData data = (ForumPostData)lblTitle.NamingContainer;
										Vote.CarmaControl control = sender as Vote.CarmaControl;
										control.ObjectId = data.Id;
										control.ObjectTypeId = (int)ForumPostEntity.AccountVoteType;
										control.RatingResult = data.RatingResult;
								};
								carmaControl.Visible = Security.IsLogged( false );
								cell.Controls.Add( carmaControl );
								row.Cells.Add( cell );

								table.Rows.Add( row );

								#region Row 2
								//User name
								row = new TableRow();
								cell = new TableCell();
								Label lblUserName = new Label();
								lblUserName.CssClass = this.CssClass + "_item_user";
								lblUserName.DataBinding += ( sender, e ) =>
								{
										ForumPostData data = (ForumPostData)lblUserName.NamingContainer;
										Label control = sender as Label;
										control.Text = data.AccountName;
								};
								cell.Controls.Add( lblUserName );

								cell.Controls.Add( new LiteralControl( "|" ) );

								//DateTime
								Label lblDateTime = new Label();
								lblDateTime.CssClass = this.CssClass + "_item_date";
								lblDateTime.DataBinding += ( sender, e ) =>
								{
										ForumPostData data = (ForumPostData)lblDateTime.NamingContainer;
										Label control = sender as Label;
										control.Text = data.Date.ToString();
								};
								cell.Controls.Add( lblDateTime );
								row.Cells.Add( cell );

								table.Rows.Add( row );
								#endregion

								#region Row 3
								//Content
								row = new TableRow();
								cell = new TableCell();
								cell.CssClass = this.CssClass + "_item_content";
								cell.ColumnSpan = 2;
								Label lblContent = new Label();
								lblContent.DataBinding += ( sender, e ) =>
								{
										ForumPostData data = (ForumPostData)lblContent.NamingContainer;
										Label control = sender as Label;
										control.Text = this.Owner.FormatTextToView( data.Content );
								};
								cell.Controls.Add( lblContent );
								row.Cells.Add( cell );
								table.Rows.Add( row );
								#endregion

								#region Row 4
								//Attachments
								row = new TableRow();
								cell = new TableCell();
								cell.CssClass = this.CssClass + "_item_attachmnets";
								cell.ColumnSpan = 2;
								HtmlGenericControl divAttachments = new HtmlGenericControl( "div" );
								divAttachments.DataBinding += ( sender, e ) =>
								{
										ForumPostData data = (ForumPostData)divAttachments.NamingContainer;
										if ( data.Attachments.Count == 0 ) return;

										Table tableAtt = new Table();
										tableAtt.CssClass = this.CssClass + "_item_attachmnets_table";
										tableAtt.Attributes.Add( "border", "0px" );
										tableAtt.CellPadding = 0;
										tableAtt.CellSpacing = 0;
										TableRow rowAtt = new TableRow(); tableAtt.Rows.Add( rowAtt );
										//Left header
										string attachmentRowId = "attTableRow_" + data.Id.ToString();
										string collapseId = "collapse_" + data.Id.ToString();
										string expandId = "expand_" + data.Id.ToString();
										TableHeaderCell cellH = new TableHeaderCell(); cellH.CssClass = "th-left"; rowAtt.Cells.Add( cellH ); cellH.RowSpan = data.Attachments.Count + 1;
										cellH.VerticalAlign = VerticalAlign.Top;
										if ( this.Owner.ColapsibleAttachment )
										{
												HtmlGenericControl divColapseItem = new HtmlGenericControl( "div" );
												divColapseItem.Attributes.Add( "class", "th-left-collapse" );
												divColapseItem.Attributes.Add( "id", collapseId );
												divColapseItem.Attributes.Add( "onclick", string.Format( "hideControl('{0}', '{1}', '{2}')", attachmentRowId, expandId, collapseId ) );
												cellH.Controls.Add( divColapseItem );
												HtmlGenericControl divExpandItem = new HtmlGenericControl( "div" );
												divExpandItem.Attributes.Add( "class", "th-left-expand" );
												divExpandItem.Attributes.Add( "id", expandId );
												divExpandItem.Attributes.Add( "onclick", string.Format( "showControl('{0}', '{1}', '{2}')", attachmentRowId, expandId, collapseId ) );
												cellH.Controls.Add( divExpandItem );
										}

										//Top header
										cellH = new TableHeaderCell(); rowAtt.Cells.Add( cellH ); cellH.CssClass = "th-top";
										cellH.Controls.Add( new LiteralControl( Resources.Controls.ForumPostsControl_Attachments ) );

										Table tableAttItems = new Table();
										rowAtt = new TableRow(); tableAtt.Rows.Add( rowAtt ); rowAtt.Attributes.Add( "id", attachmentRowId );
										if ( this.Owner.ColapsibleAttachment ) rowAtt.Style.Add( "display", "none" );
										TableCell cellAtt = new TableCell(); rowAtt.Cells.Add( cellAtt );
										cellAtt.Controls.Add( tableAttItems );

										foreach ( ForumPostAttachmentEntity attachment in data.Attachments )
										{
												TableRow rowAttItem = new TableRow(); tableAttItems.Rows.Add( rowAttItem );
												TableCell cellAttItem = new TableCell(); rowAttItem.Cells.Add( cellAttItem );

												Label lblDescription = new Label();
												lblDescription.CssClass = this.CssClass + "_item_attachment_description_label";
												lblDescription.Text = Resources.Controls.ForumPostsControl_Attachment_Description;
												cellAttItem.Controls.Add( lblDescription );

												Label lblDescriptionText = new Label();
												lblDescriptionText.CssClass = this.CssClass + "_item_attachment_description_text";
												lblDescriptionText.Text = attachment.Description;
												cellAttItem.Controls.Add( lblDescriptionText );
												cellAttItem.Controls.Add( new LiteralControl( "<br/>" ) );

												HyperLink hlAttachment = new HyperLink();
												hlAttachment.Text = attachment.Name;
												hlAttachment.CssClass = this.CssClass + "_item_attachment_type_file";
												hlAttachment.NavigateUrl = attachment.Url;
												cellAttItem.Controls.Add( hlAttachment );

												if ( attachment.Type == ForumPostAttachmentEntity.AttachmentType.Image )
												{
														Image image = new Image();
														image.ImageUrl = attachment.Url;
														hlAttachment.CssClass = this.CssClass + "_item_attachment_type_image";
														hlAttachment.Target = "_blank";
														hlAttachment.Controls.Add( image );
												}

												Label lblSize = new Label();
												lblSize.CssClass = this.CssClass + "_item_attachment_size_label";
												lblSize.Text = Resources.Controls.ForumPostsControl_Attachment_Size;
												cellAttItem.Controls.Add( lblSize );

												Label lblSizeText = new Label();
												lblSizeText.CssClass = this.CssClass + "_item_attachment_size_text";
												lblSizeText.Text = string.Format( "{0} bytes", attachment.Size );
												cellAttItem.Controls.Add( lblSizeText );
												cellAttItem.Controls.Add( new LiteralControl( "<br/>" ) );
										}
										divAttachments.Controls.Add( tableAtt );


								};
								cell.Controls.Add( divAttachments );
								row.Cells.Add( cell );
								table.Rows.Add( row );
								#endregion

								#region Footer
								//Reply button
								row = new TableRow();
								cell = new TableCell();
								HyperLink hlReply = new HyperLink();
								hlReply.CssClass = this.CssClass + "_item_reply";
								hlReply.Text = Resources.Controls.ForumPostsControl_Reply;
								hlReply.DataBinding += ( sender, e ) =>
								{
										ForumPostData data = (ForumPostData)hlReply.NamingContainer;
										HtmlGenericControl control = sender as HtmlGenericControl;
										divReplyFormContainer.Attributes.Add( "id", string.Format( "formContainer_{0}", data.Id ) );
										hlReply.Attributes.Add( "onclick", string.Format( "showReplyForm('{0}', '{1}', {2} )",
												this.CommentFormID,
												divReplyFormContainer.ClientID,
												data.Id ) );

								};
								cell.Controls.Add( hlReply );
								hlReply.Visible = this.locked == false && this.viewType == ViewType.Tree && Security.IsLogged( false );
								if ( hlReply.Visible )
								{
										row.Cells.Add( cell );
										table.Rows.Add( row );
								}
								#endregion

								//Reply Form container
								row = new TableRow();
								cell = new TableCell();
								cell.ColumnSpan = 2;
								cell.Controls.Add( divReplyFormContainer );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								//Childs
								row = new TableRow();
								cell = new TableCell();
								cell.ColumnSpan = 2;
								HtmlGenericControl divChilds = new HtmlGenericControl( "div" );
								divChilds.Attributes.Add( "class", this.CssClass + "_item_children" );
								divChilds.DataBinding += ( sender, e ) =>
								{
										ForumPostData data = (ForumPostData)lblContent.NamingContainer;
										foreach ( ForumPostData child in data.Childs )
										{
												ITemplate template = new DefaultForumPostTemplate( this.Owner, this.viewType, this.locked );
												template.InstantiateIn( child );
												divChilds.Controls.Add( child );
										}
								};
								cell.Controls.Add( divChilds );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								div.Controls.Add( table );
								container.Controls.Add( div );
						}
				}
		}
}
