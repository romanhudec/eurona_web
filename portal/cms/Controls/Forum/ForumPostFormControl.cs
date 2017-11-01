using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ForumEntity = CMS.Entities.Forum;
using ForumPostEntity = CMS.Entities.ForumPost;
using ForumTrackingEntity = CMS.Entities.ForumTracking;

[assembly: WebResource( "CMS.Controls.Forum.ForumPostForm.js", "application/x-javascript", PerformSubstitution = true )]
namespace CMS.Controls.Forum
{
		public class ForumPostFormControl: CmsControl
		{
				public delegate void PostSendEventHandler( int accountId, int forumPostId );
				public event PostSendEventHandler OnPostSend;

				private HiddenField hfParentId = null;
				private TextBox txtTitle = null;
				private TextBox txtContent = null;
				private CheckBox cbTrackThisForum = null;
				private ASPxMultipleFileUpload mfuFiles = null;
				private Button btnSend = null;

				public ForumPostFormControl()
				{
				}

				/// <summary>
				/// ID Parent prispevku
				/// </summary>
				public int? ParentId
				{
						get
						{
								object o = ViewState["ParentId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["ParentId"] = value; }
				}

				/// <summary>
				/// ID Fora
				/// </summary>
				public int ForumId
				{
						get
						{
								object o = ViewState["ForumId"];
								return o != null ? Convert.ToInt32( o ) : 0;
						}
						set { ViewState["ForumId"] = value; }
				}

				/// <summary>
				/// Url, na ktoru sa redirectne po odoslani postu
				/// </summary>
				public string RedirectUrl { get; set; }

				/// <summary>
				/// Hidden field, kde je ulozene ID nadradeneho prispevku. Naplna sa v js (ForumPostForm.js)
				/// </summary>
				public string HiddenFieldParenId { get; set; }

				/// <summary>
				/// Maximalny pocet priloh
				/// </summary>
				public int MaxfilesToUpload { get; set; }

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

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						if ( !Security.IsLogged( false ) )
						{
								this.Visible = false;
								return;
						}

						if ( this.Forum == null ) return;

						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Attributes.Add( "class", this.CssClass );
						div.Attributes.Add( "id", this.ClientID );

						Control control = CreateDetailControl( this.Forum );
						if ( control != null )
								div.Controls.Add( control );

						this.Controls.Add( div );

						this.hfParentId = new HiddenField();
						this.hfParentId.ID = this.HiddenFieldParenId;
						this.Controls.Add( this.hfParentId );

						div.Attributes.Add( "HiddenFieldParenId", this.hfParentId.ClientID );

						ClientScriptManager cs = this.Page.ClientScript;
						Type cstype = this.GetType();
						string urlInclude = cs.GetWebResourceUrl( typeof( ForumPostFormControl ), "CMS.Controls.Forum.ForumPostForm.js" );
						cs.RegisterClientScriptInclude( cstype, "ForumPostFormJs", urlInclude );

				}
				#endregion

				/// <summary>
				/// Vytvori Control Clanku
				/// </summary>
				private Control CreateDetailControl( ForumEntity forum )
				{
						this.txtTitle = new TextBox();
						this.txtTitle.ID = "txtTitle";
						this.txtTitle.Width = Unit.Percentage( 90 );

						this.txtContent = new TextBox();
						this.txtContent.ID = "txtContent";
						this.txtContent.TextMode = TextBoxMode.MultiLine;
						this.txtContent.Width = Unit.Percentage( 90 );
						this.txtContent.Height = Unit.Pixel( 200 );

						this.cbTrackThisForum = new CheckBox();
						this.cbTrackThisForum.ID = "cbTrackThisForum";
						this.cbTrackThisForum.Text = Resources.Controls.ForumPostFormControl_TrackingThisForum;

						this.mfuFiles = new ASPxMultipleFileUpload();
						this.mfuFiles.ID = "mfuFiles";
						this.mfuFiles.MaxfilesToUpload = this.MaxfilesToUpload;
						this.mfuFiles.EnableDescription = true;

						this.btnSend = new Button();
						this.btnSend.CausesValidation = true;
						this.btnSend.Text = Resources.Controls.ForumPostFormControl_SendPostButton_Text;
						this.btnSend.Click += new EventHandler( OnSendPost );

						Table table = new Table();
                        table.CssClass = this.CssClass + "_table";
						//table.Attributes.Add( "border", "1" );
						table.Width = this.Width;
						table.Height = this.Height;

						table.Rows.Add( CreateTableRow( Resources.Controls.ForumPostFormControl_NewPost, false, this.CssClass + "_header" ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.ForumPostFormControl_Subject, false, this.CssClass + "_title" ) );
						table.Rows.Add( CreateTableRow( this.txtTitle, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.ForumPostFormControl_Content, true, this.CssClass + "_content" ) );
						table.Rows.Add( CreateTableRow( this.txtContent, true ) );

						//Settings
						table.Rows.Add( CreateTableRow( Resources.Controls.ForumPostFormControl_Settings, false, this.CssClass + "_settings" ) );
						table.Rows.Add( CreateTableRow( this.cbTrackThisForum, false ) );

						//Prilohy
						table.Rows.Add( CreateTableRow( Resources.Controls.ForumPostFormControl_Attachments, false, this.CssClass + "_attachments" ) );
						table.Rows.Add( CreateTableRow( this.mfuFiles, false ) );

						//Save Cancel Buttons
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.HorizontalAlign = HorizontalAlign.Left;
						cell.Controls.Add( this.btnSend );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						if ( !this.IsPostBack )
						{
								this.txtTitle.Text = string.Format( "Re:{0}", forum.Name );
						}

						return table;
				}

				private TableRow CreateTableRow( string labelText, bool required, string cssClas )
				{
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = cssClas;
						cell.Text = labelText;
						row.Cells.Add( cell );
						return row;
				}

				private TableRow CreateTableRow( Control control, bool required )
				{
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( control );
						cell.VerticalAlign = VerticalAlign.Top;
						if ( required ) cell.Controls.Add( base.CreateRequiredFieldValidatorControl( control.ID ) );
						row.Cells.Add( cell );

						return row;
				}

				//#region Attachment methods

				///// <summary>
				///// Metóda vymaže existujúci obrázok na danej pozíci z file systému.
				///// </summary>
				//private void RemoveExistingAttachmentByPosition( string forumAttPath, int position )
				//{
				//    //Delete image
				//    DirectoryInfo di = new DirectoryInfo( forumAttPath );
				//    FileInfo[] fileInfos = di.GetFiles( string.Format( "{0:0#}_*.*", position ) );
				//    foreach ( FileInfo fileInfo in fileInfos )
				//        fileInfo.Delete();
				//}
				///// <summary>
				///// Metóda Uploadne/Nahradí fotografiu produktu pre daný produkt.
				///// V pripade, že už sa fotografia na tomto poradi nachádza, nahradí sa.
				///// </summary>
				//private void UpdatePostAttachments( ForumPostEntity forumPost, FileCollectionEventArgs mfuArgs )
				//{
				//    if ( forumPost == null || mfuArgs == null )
				//        return;

				//    if ( mfuArgs.PostedFilesInfo.Count == 0 )
				//        return;

				//    string forumVirtualAttPath = ConfigValue( "CMS:Forum:Post:StoragePath" );
				//    if ( !forumVirtualAttPath.EndsWith( "/" ) ) forumVirtualAttPath += "/";
				//    forumVirtualAttPath += forumPost.ForumId.ToString() + "/" + forumPost.Id.ToString();
				//    string forumAttPath = Server.MapPath( forumVirtualAttPath );

				//    if ( !Directory.Exists( forumAttPath ) )
				//        Directory.CreateDirectory( forumAttPath );

				//    foreach ( PostedFileInfo fi in mfuArgs.PostedFilesInfo )
				//    {
				//        string desc = fi.Description;

				//        //Delete existing Product photo on position from file system.
				//        RemoveExistingAttachmentByPosition( forumAttPath, fi.Positon );

				//        string fileName = string.Format( "{0:0#}_{1}", fi.Positon, Path.GetFileName( fi.File.FileName ) );
				//        string filePath = Path.Combine( forumAttPath, fileName );

				//        //Read input stream
				//        Stream stream = fi.File.InputStream;
				//        int len = (int)stream.Length;
				//        if ( len == 0 ) return;
				//        byte[] data = new byte[len];
				//        stream.Read( data, 0, len );
				//        stream.Flush();
				//        stream.Close();

				//        //Write new product photo.
				//        using ( FileStream fs = new FileStream( filePath, FileMode.Create, FileAccess.Write ) )
				//            fs.Write( data, 0, len );

				//        ForumPostAttachmentEntity attachment = new ForumPostAttachmentEntity();
				//        attachment.ForumPostId = forumPost.Id;
				//        attachment.Url = forumVirtualAttPath + "/" + fileName;
				//        attachment.Name = Path.GetFileName( fi.File.FileName );
				//        attachment.Description = fi.Description;
				//        attachment.Type = IsImageFile( attachment.Name ) ? ForumPostAttachmentEntity.AttachmentType.Image : ForumPostAttachmentEntity.AttachmentType.File;
				//        attachment.Size = len;
				//        attachment.Order = fi.Positon;
				//        Storage<ForumPostAttachmentEntity>.Create( attachment );
				//    }
				//}
				//private bool IsImageFile( string file )
				//{
				//    string extension = Path.GetExtension( file );
				//    extension = extension.ToUpper();
				//    if ( extension == ".JPG" || extension == ".PNG" || extension == ".GIF" || extension == ".BMP" ) return true;
				//    return false;
				//}

				//#endregion
				private ForumPostEntity CreateEntityFromUI()
				{
						int? parentId = this.ParentId;
						if ( !string.IsNullOrEmpty( this.hfParentId.Value ) )
								parentId = Convert.ToInt32( this.hfParentId.Value );

						ForumPostEntity forumPost = new ForumPostEntity();
						forumPost.AccountId = Security.Account.Id;
						forumPost.ForumId = this.ForumId;
						forumPost.ParentId = parentId;
						forumPost.Title = this.txtTitle.Text;
						forumPost.Content = Server.HtmlEncode(this.txtContent.Text);
						forumPost.Date = DateTime.Now;
						forumPost.IPAddress = this.Page.Request.UserHostAddress;
						return forumPost;
				}

				public virtual void CreatePost( ForumPostEntity forumPost )
				{
						forumPost = Storage<ForumPostEntity>.Create( forumPost );
						if ( this.cbTrackThisForum.Checked )
						{
								ForumTrackingEntity fte = Storage<ForumTrackingEntity>.ReadFirst( new ForumTrackingEntity.ReadBy { ForumId = forumPost.ForumId, AccountId = forumPost.AccountId } );
								if ( fte == null )
								{
										fte = new ForumTrackingEntity();
										fte.ForumId = forumPost.ForumId;
										fte.AccountId = forumPost.AccountId;
										Storage<ForumTrackingEntity>.Create( fte );
								}
						}

						ForumPostAttachmentHelper.UpdatePostAttachments( this.Page, forumPost, this.mfuFiles.GetUploadEventArgs() );
				}

				void OnSendPost( object sender, EventArgs e )
				{
						if ( !Security.IsLogged( true ) )
								return;

						ForumPostEntity forumPost = CreateEntityFromUI();
						CreatePost( forumPost );
						this.txtContent.Text = string.Empty;

						if ( this.OnPostSend != null ) OnPostSend( forumPost.AccountId, forumPost.Id );

						if ( string.IsNullOrEmpty( this.RedirectUrl ) )
								return;

						Response.Redirect( Page.ResolveUrl( this.RedirectUrl ) );
				}
		}
}
