using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Resources;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

[assembly: WebResource( "CMS.Controls.ASPxMultipleFileUpload.js", "application/x-javascript" )]
namespace CMS.Controls
{
		/// <summary>
		/// Autor: Ing. Roman Hudec
		/// </summary>
		[DefaultProperty( "Value" )]
		[ToolboxData( "<{0}:ASPxMultipleFileUpload runat=server></{0}:ASPxMultipleFileUpload>" )]
		public class ASPxMultipleFileUpload: UserControl
		{
				public delegate void MultipleFileUploadClick( object sender, FileCollectionEventArgs e );
				public event MultipleFileUploadClick OnUpload;

				#region Private members
				private HtmlGenericControl divFiles = null;
				private DropDownList ddlPozicia = null;
				private HtmlInputFile htmlInputFile = null;

				private HtmlGenericControl divDescription = null;
				private TextBox txtDescription = null;

				private Table tableFiles = null;

				private HtmlInputButton htmlInputBtnAdd = null;
				private Button btnUpload = null;
				#endregion

				public ASPxMultipleFileUpload()
				{
						this.ExcludePositions = new List<int>();
				}

				#region Public properties

				/// <summary>
				/// Maximálny počet obrázkov, ktore je možne uploudovať.
				/// </summary>
				public int MaxfilesToUpload
				{
						get
						{
								object o = ViewState["MaxfilesToUpload"];
								return o != null ? Convert.ToInt32( o ) : 0;
						}
						set { ViewState["MaxfilesToUpload"] = value; }
				}

				/// <summary>
				/// Zoznam pozicií, ktore sa nebudu zobrazovať v ddl zozname pozic.
				/// </summary>
				public List<int> ExcludePositions{ get; set;}

				/// <summary>
				/// Nastavuje či sa bude evidovat k súboru aj popis.
				/// </summary>
				[DefaultValue( false )]
				public bool EnableDescription
				{
						get
						{
								object o = ViewState["EnableDescription"];
								return o != null ? Convert.ToBoolean( o ) : false;
						}
						set { ViewState["EnableDescription"] = value; }
				}

				/// <summary>
				/// Nastavuje či sa bude zobrazovat tlačidlo na upload súboru.
				/// </summary>
				[DefaultValue( false )]
				public bool VisibleUploadButton
				{
						get
						{
								object o = ViewState["VisibleUploadButton"];
								return o != null ? Convert.ToBoolean( o ) : false;
						}
						set { ViewState["VisibleUploadButton"] = value; }
				}

				public Unit Width
				{
						get
						{
								object o = ViewState["Width"];
								return o != null ? (Unit)o : Unit.Empty;
						}
						set { ViewState["Width"] = value; }
				}


				#endregion

				public FileCollectionEventArgs GetUploadEventArgs()
				{
						return new FileCollectionEventArgs( this.ClientID, this.Request );
				}

				#region Overridet methods

				protected override void CreateChildControls()
				{
						HtmlGenericControl divMultipleFileUpload = new HtmlGenericControl( "div" );
						divMultipleFileUpload.ID = "aspxMultipleFileUpload";
						divMultipleFileUpload.Attributes.Add( "class", "aspxMfu" );
						if ( this.Width != Unit.Empty )
								divMultipleFileUpload.Style.Add( "width", this.Width.ToString() );

						//Pridanie DDL a divFiles do Tabulky.
						Table tableMultipleFileUpload = new Table();
						//tableMultipleFileUpload.Attributes.Add("border", "1");
						tableMultipleFileUpload.Width = Unit.Percentage( 100 );

						#region FileInput a Description
						this.ddlPozicia = new DropDownList();
						this.ddlPozicia.ID = "selectPosition";
						for ( int pozicia = 1; pozicia <= this.MaxfilesToUpload; pozicia++ )
						{
								if ( this.ExcludePositions.Contains( pozicia ) ) continue;
								this.ddlPozicia.Items.Add( pozicia.ToString() );
						}

						this.htmlInputFile = new HtmlInputFile();
						this.htmlInputFile.ID = "inputFile_0";
						this.htmlInputFile.Attributes.Add( "class", "inpuFile" );
						this.htmlInputFile.Name = "inputFile_";
						//Input file
						this.divFiles = new HtmlGenericControl( "span" );
						this.divFiles.ID = "divFiles";
						this.divFiles.Controls.Add( htmlInputFile );

						TableRow row = new TableRow();
						//Cell dropDownList - pozicia.
						TableCell cell = new TableCell();
						cell.ColumnSpan = 2;
						cell.Width = Unit.Percentage( 100 );
						cell.Controls.Add( this.ddlPozicia );
						cell.Controls.Add( this.divFiles );
						row.Cells.Add( cell );

						tableMultipleFileUpload.Rows.Add( row );

						//Description
						if ( this.EnableDescription )
						{
								this.divDescription = new HtmlGenericControl( "div" );
								this.divDescription.ID = "divDescription";
								divMultipleFileUpload.Controls.Add( divDescription );

								this.txtDescription = new TextBox();
								this.txtDescription.ID = "inputDescription_0";
								this.txtDescription.Attributes.Add( "name", "inputDescription_" );
								this.txtDescription.CssClass = "description";
								this.txtDescription.TextMode = TextBoxMode.MultiLine;
								this.divDescription.Controls.Add( txtDescription );

								row = new TableRow();
								cell = new TableCell();
								cell.ColumnSpan = 2;
								cell.Controls.Add( this.divDescription );
								row.Cells.Add( cell );
								tableMultipleFileUpload.Rows.Add( row );
						}
						#endregion

						#region Table Pridat button
						row = new TableRow();
						cell = new TableCell();
						cell.ColumnSpan = 2;
						cell.HorizontalAlign = HorizontalAlign.Right;

						this.htmlInputBtnAdd = new HtmlInputButton();
						this.htmlInputBtnAdd.ID = "btnAdd";
						this.htmlInputBtnAdd.Value = Resources.ASPxMultipleFileUpload.AddButtonCaption;
						this.htmlInputBtnAdd.Attributes.Add( "onclick", string.Format( "o{0}.Add('{1}');", this.ID, Resources.ASPxMultipleFileUpload.ChooceAtOneFile ) );

						cell.Controls.Add( htmlInputBtnAdd );
						row.Cells.Add( cell );
						tableMultipleFileUpload.Rows.Add( row );

						#endregion

						#region Table s polozkami
						//Table na zobrazenie poloziek!!
						tableFiles = new Table();
						tableFiles.CellPadding = 0;
						tableFiles.CellSpacing = 0;
						tableFiles.ID = "tableFiles";
						tableFiles.CssClass = "tableFiles";

						row = new TableHeaderRow();
						cell = new TableHeaderCell();
						cell.Text = Resources.ASPxMultipleFileUpload.Position;
						cell.CssClass = "header";
						row.Cells.Add( cell );

						cell = new TableHeaderCell();
						cell.Text = Resources.ASPxMultipleFileUpload.File;
						cell.CssClass = "header";
						if ( !this.EnableDescription )
								cell.Style.Add( "width", "100%" );
						row.Cells.Add( cell );

						if ( this.EnableDescription )
						{
								cell = new TableHeaderCell();
								cell.Text = Resources.ASPxMultipleFileUpload.Description;
								cell.CssClass = "header";
								cell.Style.Add( "width", "100%" );
								row.Cells.Add( cell );
						}

						cell = new TableHeaderCell();
						cell.Text = Resources.ASPxMultipleFileUpload.Action;
						cell.CssClass = "header";
						row.Cells.Add( cell );

						tableFiles.Rows.Add( row );

						row = new TableRow();
						cell = new TableCell();
						cell.ColumnSpan = 2;
						cell.Controls.Add( tableFiles );
						row.Cells.Add( cell );
						tableMultipleFileUpload.Rows.Add( row );
						#endregion

						#region Button Upload
						if ( this.VisibleUploadButton )
						{
								row = new TableRow();
								cell = new TableCell();
								cell.ColumnSpan = 2;
								cell.HorizontalAlign = HorizontalAlign.Right;

								this.btnUpload = new Button();
								this.btnUpload.Text = Resources.ASPxMultipleFileUpload.UploadButtonCaption;
								this.btnUpload.OnClientClick = string.Format( "javascript:return o{0}.DisableTop();", this.ID );
								this.btnUpload.Click += new EventHandler( btnUpload_Click );

								cell.Controls.Add( this.btnUpload );
								row.Cells.Add( cell );
								tableMultipleFileUpload.Rows.Add( row );
						}

						#endregion

						divMultipleFileUpload.Controls.Add( tableMultipleFileUpload );
						this.Controls.Add( divMultipleFileUpload );

						//HtmlInputButton
						base.CreateChildControls();

						ClientScriptManager cs = this.Page.ClientScript;
						Type cstype = this.GetType();
						string urlInclude = cs.GetWebResourceUrl( cstype, "CMS.Controls.ASPxMultipleFileUpload.js" );
						cs.RegisterClientScriptInclude( cstype, "ASPxMultipleFileUploadJs", urlInclude );
				}

				/// <summary>
				/// Vyrenderovanie samotného controlsu.
				/// </summary>
				protected override void Render( HtmlTextWriter writer )
				{
						System.Text.StringBuilder sTemp = null;
						ClientScriptManager cs = this.Page.ClientScript;
						Type cstype = this.Page.GetType();

						string SCRIPT_NAME = "ASPxMultipleFileUpload" + this.ID;

						//string urlInclude = cs.GetWebResourceUrl( cstype, "CMS.Controls.ASPxMultipleFileUpload.js" );
						//cs.RegisterClientScriptInclude( cstype, "ASPxMultipleFileUpload", urlInclude );

						if ( !cs.IsStartupScriptRegistered( cstype, SCRIPT_NAME ) )
						{
								sTemp = new StringBuilder( 512 );
								sTemp.AppendFormat( "<script type=\"text/javascript\">var o" + this.ID + " = new MultipleFileUpload('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, '{7}', '{8}');</script>",
									this.ClientID,
									this.ClientID + "_" + this.ddlPozicia.ID,
									this.ClientID + "_" + this.divFiles.ID,
									this.EnableDescription ? this.ClientID + "_" + this.divDescription.ID : string.Empty,
									this.ClientID + "_" + this.tableFiles.ID,
									this.ClientID + "_" + this.htmlInputBtnAdd.ID,
									this.MaxfilesToUpload,
									Resources.ASPxMultipleFileUpload.DeleteButtonCaption,
									Resources.ASPxMultipleFileUpload.EditButtonCaption );
								cs.RegisterStartupScript( cstype, SCRIPT_NAME, sTemp.ToString() );
						}

						base.Render( writer );

				}

				void btnUpload_Click( object sender, EventArgs e )
				{
						if ( OnUpload != null )
								OnUpload( this, GetUploadEventArgs() );
				}
				#endregion
		}

		public class FileCollectionEventArgs: EventArgs
		{
				private HttpRequest httpRequest = null;
				private List<PostedFileInfo> postedFilesInfo = null;
				private string multipleFileUploadId = string.Empty;

				public FileCollectionEventArgs( string multipleFileUploadId, HttpRequest oHttpRequest )
				{
						this.multipleFileUploadId = multipleFileUploadId;
						this.httpRequest = oHttpRequest;
				}

				public List<PostedFileInfo> PostedFilesInfo
				{
						get
						{
								if ( this.postedFilesInfo == null || this.postedFilesInfo.Count == 0 )
								{
										this.postedFilesInfo = new List<PostedFileInfo>();

										foreach ( string inputTagName in httpRequest.Files )
										{
												HttpPostedFile postedFile = httpRequest.Files[inputTagName];
												if ( postedFile.ContentLength <= 0 )
														continue;

												//Parse nazov komponentu, kde je ulozene aj poradie.
												if ( inputTagName.Length <= this.multipleFileUploadId.Length )
														continue;
												string[] parsedInputTagName = inputTagName.Remove( 0, this.multipleFileUploadId.Length ).Split( '_' );
												if ( parsedInputTagName.Length != 2 )
														continue;

												if ( parsedInputTagName[1] == string.Empty )
														continue;

												//Get poradie fuboru.
												int position = Convert.ToInt32( parsedInputTagName[1] );

												//Get hodnotu description pre InputTagName
												string description = string.Empty;
												description = httpRequest.Form.Get( string.Format( "{0}$inputDescription_{1}", this.multipleFileUploadId, position ) );

												//Ulozenie do kolekcie PostedFileInfo.
												PostedFileInfo pfi = new PostedFileInfo( position, postedFile, description );
												postedFilesInfo.Add( pfi );
										}
								}
								return this.postedFilesInfo;
						}
				}

				public HttpFileCollection PostedFiles
				{
						get { return httpRequest.Files; }
				}
		}

		public class PostedFileInfo
		{
				private int position = 0;
				private HttpPostedFile file = null;
				private string description = null;

				public PostedFileInfo( int position, HttpPostedFile file, string description )
				{
						this.position = position;
						this.file = file;
						this.description = description;
				}

				public int Positon
				{
						get { return this.position; }
				}

				public HttpPostedFile File
				{
						get { return this.file; }
				}

				public string Description
				{
						get { return this.description; }
				}
		}
}
