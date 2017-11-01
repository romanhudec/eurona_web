using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using ImageGalleryEntity = CMS.Entities.ImageGallery;
using ImageGalleryItemEntity = CMS.Entities.ImageGalleryItem;
using System.IO;
using System.Web.UI;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

namespace CMS.Controls.ImageGallery
{
		public class AdminImageGalleryItemsControl: CmsControl
		{
				private const int MAX_FREE_AUCTION_REPEAT = 2;

				private ImageGalleryControl imgGaleryCtrl = null;
				private ASPxMultipleFileUpload mfuPhotos = null;
				//private RadProgressManager radProgressManager = null;
				//private RadProgressArea radProgressArea = null;
				private Button btnSave = null;
				private Button btnCancel = null;

				private ImageGalleryEntity imageGalleryEntity = null;
				public AdminImageGalleryItemsControl()
				{
				}

				public int ImageGalleryId
				{
						get
						{
								object o = ViewState["ImageGalleryId"];
								return o != null ? (int)Convert.ToInt32( o ) : 0;
						}
						set { ViewState["ImageGalleryId"] = value; }
				}

				public int MaxImagesToUpload
				{
						get
						{
								object o = ViewState["MaxImagesToUpload"];
								return o != null ? (int)Convert.ToInt32( o ) : 100;
						}
						set { ViewState["MaxImagesToUpload"] = value; }
				}

				public ImageGalleryEntity ImageGallery
				{
						get
						{
								if ( this.imageGalleryEntity != null ) return this.imageGalleryEntity;
								this.imageGalleryEntity = Storage<ImageGalleryEntity>.ReadFirst( new ImageGalleryEntity.ReadById { ImageGalleryId = this.ImageGalleryId } );
								return this.imageGalleryEntity;
						}
				}

				public string RequestClassifierFunction { get; set; }
				public string CommentsFormatUrl { get; set; }

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						if ( string.IsNullOrEmpty( ConfigValue( "CMS:ImageGallery:StoragePath" ) ) )
								throw new InvalidOperationException( "Attribute 'CMS:ImageGallery:StoragePath' in web configuration file is missing!" );

						//Load ImageGalleryEntity item data
						if ( this.ImageGalleryId == 0 )
								return;

						if ( this.ImageGallery == null )
								return;

						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Attributes.Add( "class", this.CssClass );

						//Header
						HtmlGenericControl divHeader = new HtmlGenericControl( "div" );
						divHeader.Attributes.Add( "class", this.CssClass + "_header" );
						Label lblGalleryName = new Label();
						lblGalleryName.ID = "lblGalleryName";
						lblGalleryName.Text = this.ImageGallery.Name;
						divHeader.Controls.Add( lblGalleryName );
						div.Controls.Add( divHeader );

						// Create detail control
						Control detailControl = CreateDetailControl();
						if ( detailControl != null )
								div.Controls.Add( detailControl );

						this.Controls.Add( div );

						//this.radProgressManager = new RadProgressManager();
						//this.radProgressArea = new RadProgressArea();
						//this.Controls.Add( this.radProgressManager );
						//this.Controls.Add( this.radProgressArea );

						//Binding
						DataBindGalleryControl();
				}
				#endregion

				/// <summary>
				/// Vytvori Control
				/// </summary>
				private Control CreateDetailControl()
				{
						this.imgGaleryCtrl = new ImageGalleryControl( ImageGalleryControl.DisplayMode.Edit );
						this.imgGaleryCtrl.CssClass = this.CssClass + "_imageGallery";
						this.imgGaleryCtrl.ImageDelete += ( s, e ) =>
						{
								object id = ( e as object );
								if ( id == null )
										return;

								int itemId = Convert.ToInt32( id );
								ImageGalleryItemEntity item = Storage<ImageGalleryItemEntity>.ReadFirst( new ImageGalleryItemEntity.ReadById { ImageGalleryItemId = itemId } );
								if ( item == null )
										return;

								string imagePath = Server.MapPath( item.VirtualPath );
								string imageThumbnailPath = Server.MapPath( item.VirtualThumbnailPath );

								if ( File.Exists( imagePath ) ) File.Delete( imagePath );
								if ( File.Exists( imageThumbnailPath ) ) File.Delete( imageThumbnailPath );
								Storage<ImageGalleryItemEntity>.Delete( item );

								this.imgGaleryCtrl.DataSource = CreateImageGaleryDatasource( this.ImageGallery );
								this.imgGaleryCtrl.DataBind();
						};

						this.imgGaleryCtrl.ImageShiftLeft += ( s, e ) =>
						{
								object id = ( e as object );
								if ( id == null )
										return;

								int itemId = Convert.ToInt32( id );
								ImageGalleryItemEntity item = Storage<ImageGalleryItemEntity>.ReadFirst( new ImageGalleryItemEntity.ReadById { ImageGalleryItemId = itemId } );
								if ( item == null )
										return;

								List<ImageGalleryItemEntity> list = Storage<ImageGalleryItemEntity>.Read( new ImageGalleryItemEntity.ReadByImageGalleryId { ImageGalleryId = item.ImageGalleryId } );
								item = list.First( x => x.Id == itemId );
								int index = list.IndexOf( item );
								int prevIndex = index - 1;
								if ( prevIndex < 0 ) return;

								int pos = item.Position;
								item.Position = list[prevIndex].Position;
								Storage<ImageGalleryItemEntity>.Update( item );

								ImageGalleryItemEntity prevItem = list[prevIndex];
								prevItem.Position = pos;
								Storage<ImageGalleryItemEntity>.Update( prevItem );
								
								this.imgGaleryCtrl.DataSource = CreateImageGaleryDatasource( this.ImageGallery );
								this.imgGaleryCtrl.DataBind();
						};

						this.imgGaleryCtrl.ImageShiftRight += ( s, e ) =>
						{
								object id = ( e as object );
								if ( id == null )
										return;

								int itemId = Convert.ToInt32( id );
								ImageGalleryItemEntity item = Storage<ImageGalleryItemEntity>.ReadFirst( new ImageGalleryItemEntity.ReadById { ImageGalleryItemId = itemId } );
								if ( item == null )
										return;

								List<ImageGalleryItemEntity> list = Storage<ImageGalleryItemEntity>.Read( new ImageGalleryItemEntity.ReadByImageGalleryId { ImageGalleryId = item.ImageGalleryId } );
								item = list.First( x => x.Id == itemId );
								if ( item == null )
										return;
								int index = list.IndexOf( item );
								int nextIndex = index + 1;
								if ( nextIndex > list.Count - 1 ) return;

								int pos = item.Position;
								item.Position = list[nextIndex].Position;
								Storage<ImageGalleryItemEntity>.Update( item );

								ImageGalleryItemEntity nextItem = list[nextIndex];
								nextItem.Position = pos;
								Storage<ImageGalleryItemEntity>.Update( nextItem );

								this.imgGaleryCtrl.DataSource = CreateImageGaleryDatasource( this.ImageGallery );
								this.imgGaleryCtrl.DataBind();
						};

						this.mfuPhotos = new ASPxMultipleFileUpload();
						this.mfuPhotos.ID = "mfuPhotos";
						this.mfuPhotos.EnableDescription = true;
						this.mfuPhotos.MaxfilesToUpload = this.MaxImagesToUpload;
						List<ImageGalleryItemEntity> items = Storage<ImageGalleryItemEntity>.Read( new ImageGalleryItemEntity.ReadByImageGalleryId { ImageGalleryId = imageGalleryEntity.Id } );
						foreach ( ImageGalleryItemEntity item in items )
								this.mfuPhotos.ExcludePositions.Add( item.Position );

						this.btnSave = new Button();
						this.btnSave.CausesValidation = true;
						this.btnSave.Text = Resources.Controls.SaveButton_Text;
						this.btnSave.Click += new EventHandler( OnSave );
						this.btnCancel = new Button();
						this.btnCancel.CausesValidation = false;
						this.btnCancel.Text = Resources.Controls.CancelButton_Text;
						this.btnCancel.Click += new EventHandler( OnCancel );

						Table table = new Table();
						table.CssClass = this.CssClass + "_content";

						#region Add control to table
						TableRow row = null;
						TableCell cell = null;
						//Photos
						row = new TableRow();
						AddControlToRow( row, this.imgGaleryCtrl );
						table.Rows.Add( row );

						row = new TableRow();
						AddControlToRow( row, this.mfuPhotos );
						table.Rows.Add( row );

						//Save Cancel Buttons
						row = new TableRow();
						cell = new TableCell();
						cell.ColumnSpan = 2;
						cell.Controls.Add( this.btnSave );
						cell.Controls.Add( this.btnCancel );
						row.Cells.Add( cell );
						table.Rows.Add( row );
						#endregion

						return table;
				}

				private void DataBindGalleryControl()
				{
						this.imgGaleryCtrl.IdFieldName = "ImageGalleryId";
						this.imgGaleryCtrl.PositionFieldName = "Position";
						this.imgGaleryCtrl.ImageUrlFieldName = "ImageUrl";
						this.imgGaleryCtrl.ImageUrlThumbnailFieldName = "ImageUrlThumbnail";
						this.imgGaleryCtrl.AditionalProperties.Add( new ImageGalleryControl.AditionalProperty( string.Empty, "Description" ) );
						this.imgGaleryCtrl.DataSource = CreateImageGaleryDatasource( this.ImageGallery );

						if ( !IsPostBack )
								this.imgGaleryCtrl.DataBind();
				}

				private void AddControlToRow( TableRow row, Control control )
				{
						TableCell cell = new TableCell();
						if ( control == null )
								return;

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( control );
						row.Cells.Add( cell );
				}

				#region Public methods
				#region ImageGalleryEntity Photo methods

				/// <summary>
				/// Metóda vytvorí datasource pre ImageGalery control.
				/// </summary>
				private DataTable CreateImageGaleryDatasource( ImageGalleryEntity imageGalleryEntity )
				{
						DataTable dt = new DataTable( "image_galery" );

						DataColumn idColumn = new DataColumn( "ImageGalleryId", typeof( Int32 ) );
						DataColumn urlColumn = new DataColumn( "ImageUrl", typeof( string ) );
						DataColumn urlThumbnailColumn = new DataColumn( "ImageUrlThumbnail", typeof( string ) );
						DataColumn positionColumn = new DataColumn( "Position", typeof( Int32 ) );
						DataColumn descriptionColumn = new DataColumn( "Description", typeof( string ) );
						dt.Columns.AddRange( new DataColumn[] { idColumn, urlColumn, urlThumbnailColumn, positionColumn, descriptionColumn } );

						List<ImageGalleryItemEntity> items = Storage<ImageGalleryItemEntity>.Read( new ImageGalleryItemEntity.ReadByImageGalleryId { ImageGalleryId = imageGalleryEntity.Id } );
						foreach ( ImageGalleryItemEntity item in items )
								dt.Rows.Add( new object[] { item.Id, item.VirtualPath, item.VirtualThumbnailPath, item.Position, item.Description } );

						//Defaul sort by position
						dt.DefaultView.Sort = "Position ASC";

						return dt;
				}

				/// <summary>
				/// Metóda Uploadne/Nahradí fotografiu.
				/// V pripade, že už sa fotografia na tomto poradi nachádza, nahradí sa.
				/// </summary>
				private void UpdateGalleryPhotos( ImageGalleryEntity imageGalleryEntity, FileCollectionEventArgs mfuArgs )
				{
						if ( imageGalleryEntity == null || mfuArgs == null )
								return;

						if ( mfuArgs.PostedFilesInfo.Count == 0 )
								return;

						string galleryUrl = string.Format( "{0}{1}/", ConfigValue( "CMS:ImageGallery:StoragePath", false ), imageGalleryEntity.Id );
						string galleryThumbnailUrl = string.Format( "{0}{1}/", galleryUrl, "_t" );

						string galleryImagesPath = Server.MapPath( galleryUrl );
						string galleryImagesThumbnailPath = Server.MapPath( galleryThumbnailUrl );

						//Preview image path
						if ( !Directory.Exists( galleryImagesPath ) )
								Directory.CreateDirectory( galleryImagesPath );

						//Thumbnail image path
						if ( !Directory.Exists( galleryImagesThumbnailPath ) )
								Directory.CreateDirectory( galleryImagesThumbnailPath );

						RadProgressContext progress = RadProgressContext.Current;
						//int count = mfuArgs.PostedFilesInfo.Count;
						//int index = 0;
						foreach ( PostedFileInfo fi in mfuArgs.PostedFilesInfo )
						{
								string desc = fi.Description;

								string fileName = Path.GetFileName( fi.File.FileName );
								string filePath = Path.Combine( galleryImagesPath, fileName );
								string filePathThumbnail = Path.Combine( galleryImagesThumbnailPath, fileName );
								string filePathPrevie = Path.Combine( galleryImagesPath, fileName );

								MemoryStream ms = null;
								string wxh = this.ConfigValue( "CMS:ImageGallery:MaxPhotoSize" );
								if ( wxh.Length != 0 )
								{
										string[] size = wxh.ToLower().Split( 'x' );
										int maxW = 0;
										int maxH = 0;
										Int32.TryParse( size[0], out maxW );
										Int32.TryParse( size[1], out maxH );
										if ( maxW == 0 || maxH == 0 )
												throw new InvalidCastException( "Invalid config value for attribute 'CMS:ImageGallery:MaxPhotoSize' in web configoration file! Value mut be e.g. '1024x768'" );

										using ( Stream stream = fi.File.InputStream )
												ms = ImageGalleryControl.GetImageStream( stream, maxW, maxH, false );
								}
								else
								{
										using ( Stream stream = fi.File.InputStream )
												ms = ImageGalleryControl.GetImageStream( stream );
								}

								//Write previe photo
								using ( FileStream fs = File.OpenWrite( filePath ) )
								{
										ms.WriteTo( fs );
										fs.Flush();
										fs.Close();
								}

								ms.Flush();
								ms.Close();
  

								//Write Thumbnail photo
								int w = 0;
								int h = 0;
								using ( System.Drawing.Image img = System.Drawing.Image.FromFile( filePath ) )
								{
										w = img.Width;
										h = img.Height;
								}

								int newWidth = w;
								int newHeight = h;
								ImageGalleryControl.RecalculateImageSize( w, h, ImageGalleryControl.IMAGE_WIDTH * 2, ImageGalleryControl.IMAGE_HEIGHT * 2, ref newWidth, ref newHeight );
								ImageGalleryControl.ResizeImage( filePath, filePathThumbnail, newWidth, newHeight );

								ImageGalleryItemEntity.ReadByImageGalleryAndPosition by = new ImageGalleryItemEntity.ReadByImageGalleryAndPosition { ImageGalleryId = imageGalleryEntity.Id, Position = fi.Positon };
								ImageGalleryItemEntity item = Storage<ImageGalleryItemEntity>.ReadFirst( by );
								if ( item == null ) item = new ImageGalleryItemEntity();
								item.ImageGalleryId = imageGalleryEntity.Id;
								item.Position = fi.Positon;
								item.VirtualPath = galleryUrl + fileName;
								item.VirtualThumbnailPath = galleryThumbnailUrl + fileName;
								item.Description = fi.Description;

								if ( item.Id == 0 ) Storage<ImageGalleryItemEntity>.Create( item );
								else Storage<ImageGalleryItemEntity>.Update( item );

								//index++;
								//progress.PrimaryTotal = 1;
								//progress.PrimaryValue = 1;
								//progress.PrimaryPercent = 100;

								//progress.SecondaryTotal = count;
								//progress.SecondaryValue = index;
								//progress.SecondaryPercent = index;
								//progress.CurrentOperationText = fileName + " is being processed...";


								//Vytvorenie URL Aliasu pre komentare
								if ( !string.IsNullOrEmpty( this.CommentsFormatUrl ) && !string.IsNullOrEmpty(this.ImageGallery.Alias) )
								{
										string urlComment = string.Format( this.CommentsFormatUrl, item.Id );
										string aliasComment = string.Format( "{0}/{1}/{2}", this.ImageGallery.Alias, item.Id, Resources.Controls.Comment_AliasText );
										if ( !Utilities.AliasUtilities.CreateUrlAlias( this.Page, urlComment, string.Format( "image {0}", item.Id ), aliasComment ) )
												return;
								}
						}
				}
				#endregion

				#endregion
				void OnSave( object sender, EventArgs e )
				{
						//Update imageGalleryEntity photos
						UpdateGalleryPhotos( this.imageGalleryEntity, this.mfuPhotos.GetUploadEventArgs() );

						if ( !string.IsNullOrEmpty( this.ReturnUrl ) )
								Response.Redirect( this.ReturnUrl );
						else
						{
								this.imgGaleryCtrl.DataSource = CreateImageGaleryDatasource( this.imageGalleryEntity );
								this.imgGaleryCtrl.DataBind();
						}
				}

				void OnCancel( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( this.ReturnUrl ) )
								Response.Redirect( this.ReturnUrl );
				}
		}
}
