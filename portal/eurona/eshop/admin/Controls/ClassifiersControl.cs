using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Controls;
using CMS.MSSQL;
using EmptyCMS.Entities.Classifiers;
using System.Web.UI.HtmlControls;
using CMS;
using CMS.Entities.Classifiers;

namespace EmptyCMS.Admin.Controls
{
		public class ClassifiersControl<T>: CMS.Controls.CmsControl where T : ClassifierBase, new()
		{
				protected const string DELETE_COMMAND = "DELETE_ITEM";
				protected const string EDIT_COMMAND = "EDIT_ITEM";

				protected HyperLink hlNewItem;
				protected GridView dataGrid = null;

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "ClassifiersControl-SortDirection", SortDirection.Ascending ); }
						set { SetSession<SortDirection>( "ClassifiersControl-SortDirection", value ); }
				}
				public string SortExpression
				{
						get { return GetSession<string>( "ClassifiersControl-SortExpression" ); }
						set { SetSession<string>( "ClassifiersControl-SortExpression", value ); }
				}

				public ClassifiersControl()
				{
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						Control clsControl = CreateClassifiersControl();
						if ( clsControl != null ) this.Controls.Add( clsControl );
						else return;

						//Binding
						this.dataGrid.PagerTemplate = null;
						this.dataGrid.DataSource = GetDataGridData();

						if ( !IsPostBack )
						{
								this.dataGrid.DataKeyNames = new string[] { "Id" };
								this.dataGrid.DataBind();
						}

				}
				#endregion


				public IStorage<T> ClassifierStorage
				{
						get { return new WebStorage<T>().Access(); }
				}

				protected T ReadFirst( object criteria )
				{
						return ClassifierStorage.Read( criteria )[0];
				}

				public string NewUrl { get; set; }
				public string EditUrlFormat { get; set; }

				/// <summary>
				/// Vytvori Control zoznamu poloziek ciselnika
				/// </summary>
				protected virtual Control CreateClassifiersControl()
				{
						HtmlGenericControl div = new HtmlGenericControl( "div" );

						//DataGrid
						this.dataGrid = CreateGridControl();
						div.Controls.Add( this.dataGrid );

						if ( !string.IsNullOrEmpty( this.NewUrl ) )
						{
								this.hlNewItem = new HyperLink();
								this.hlNewItem.CssClass = CssClass + "_newItem";
								this.hlNewItem.Text = Resources.Strings.ClassifiersControl_AddItem;
								this.hlNewItem.NavigateUrl = Page.ResolveUrl( string.Format( "{0}{1}{2}", this.NewUrl,
										( this.NewUrl.Contains( "?" ) ? "&" : "?" ),
										base.BuildReturnUrlQueryParam() ) );
								div.Controls.Add( hlNewItem );
						}

						return div;
				}

				/// <summary>
				/// Vytvori DataGrid control s pozadovanymi stlpcami a 
				/// pripravenym bindingom stlpcou.
				/// </summary>
				protected virtual GridView CreateGridControl()
				{
						GridView dg = new GridView();
						dg.GridLines = GridLines.None;
						dg.CssClass = CssClass;
						dg.RowStyle.CssClass = CssClass + "_rowStyle";
						dg.FooterStyle.CssClass = CssClass + "_footerStyle";
						dg.PagerStyle.CssClass = CssClass + "_pagerStyle";
						dg.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
						dg.HeaderStyle.CssClass = CssClass + "_headerStyle";
						dg.EditRowStyle.CssClass = CssClass + "_editRowStyle";
						dg.AlternatingRowStyle.CssClass = CssClass + "_alternatingRowStyle";

						dg.AllowPaging = true;
						dg.PageSize = 25;
						dg.PagerSettings.Mode = PagerButtons.NumericFirstLast;
						dg.PagerSettings.PageButtonCount = 10;
						dg.PageIndexChanging += OnPageIndexChanging;

						dg.AutoGenerateColumns = false;
						dg.AllowSorting = true;
						dg.Sorting += OnSorting;
						dg.RowCommand += OnRowCommand;

						BoundField bf = new BoundField();
						bf.DataField = "Name";
						bf.HeaderText = Resources.Strings.ClassifiersControl_ColumnName;
						bf.SortExpression = "Name";
						dg.Columns.Add( bf );

						bf = new BoundField();
						bf.DataField = "Notes";
						bf.HeaderText = Resources.Strings.ClassifiersControl_ColumnNotes;
						bf.SortExpression = "Notes";
						dg.Columns.Add( bf );

						CMSButtonField btnEdit = new CMSButtonField();
						btnEdit.HeaderStyle.Width = Unit.Pixel( 16 );
						btnEdit.ImageUrl = ConfigValue( "CMS:EditButtonImage" );
						btnEdit.ToolTip = Resources.Strings.GridView_ToolTip_EditItem;
						btnEdit.ButtonType = ButtonType.Image;
						btnEdit.CommandName = EDIT_COMMAND;
						dg.Columns.Add( btnEdit );

						CMSButtonField btnDelete = new CMSButtonField();
						btnDelete.HeaderStyle.Width = Unit.Pixel( 16 );
						btnDelete.ImageUrl = ConfigValue( "CMS:DeleteButtonImage" );
						btnDelete.ToolTip = Resources.Strings.GridView_ToolTip_DeleteItem;
						btnDelete.ButtonType = ButtonType.Image;
						btnDelete.OnClientClick = string.Format( "javascript:return confirm('{0}')", Resources.Strings.DeleteItemQuestion );
						btnDelete.CommandName = DELETE_COMMAND;
						dg.Columns.Add( btnDelete );

						return dg;
				}

				protected virtual List<T> GetDataGridData()
				{
						List<T> list = ClassifierStorage.Read( null );

						SortDirection previous = SortDirection;
						string sortExpression = String.IsNullOrEmpty( SortExpression ) ? "Name" : SortExpression;
						var ordered = list.AsQueryable().OrderBy( sortExpression + " " + ( previous == SortDirection.Ascending ? "ascending" : "descending" ) );
						return ordered.ToList();
				}

				#region Event handlers
				protected void OnSorting( object sender, GridViewSortEventArgs e )
				{
						List<T> list = dataGrid.DataSource as List<T>;
						SortDirection previous = SortDirection;
						var ordered = list.AsQueryable().OrderBy( e.SortExpression + " " + ( previous == SortDirection.Ascending ? "descending" : "ascending" ) );
						dataGrid.DataSource = ordered.ToList();
						dataGrid.DataKeyNames = new string[] { "Id" };
						dataGrid.DataBind();
						SortDirection = previous == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
				}
				/// <summary>
				/// Implementacia strankovania zaznamov v gride.
				/// </summary>
				protected  void OnPageIndexChanging( object sender, GridViewPageEventArgs e )
				{
						this.dataGrid.PageIndex = e.NewPageIndex;

						this.dataGrid.DataSource = GetDataGridData();
						this.dataGrid.DataKeyNames = new string[] { "Id" };
						this.dataGrid.DataBind();
				}

				protected void OnRowCommand( object sender, GridViewCommandEventArgs e )
				{
						if ( e.CommandArgument == null )
								return;

						//Ak to je neznamy command, nebu se spracovavat tu.
						if ( e.CommandName != EDIT_COMMAND &&
								e.CommandName != DELETE_COMMAND )
								return;

						int rowIndex = Convert.ToInt32( e.CommandArgument );
						if ( rowIndex == -1 )
								return;

						int id = Convert.ToInt32( ( sender as GridView ).DataKeys[rowIndex].Value );

						if ( e.CommandName == EDIT_COMMAND )
						{
								string url = Page.ResolveUrl( string.Format( EditUrlFormat, id ) + "&" + base.BuildReturnUrlQueryParam() );
								Response.Redirect( url );
								return;
						}

						if ( e.CommandName == DELETE_COMMAND )
						{
								ClassifierBase classifier = (ClassifierBase)ReadFirst( new ClassifierBase.ReadById { Id = id } );
								ClassifierStorage.Delete( classifier as T );

								this.dataGrid.DataSource = GetDataGridData();
								this.dataGrid.DataKeyNames = new string[] { "ID" };
								this.dataGrid.DataBind();
						}
				}
				#endregion
		}
}
