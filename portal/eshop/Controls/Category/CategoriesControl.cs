using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Controls;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using CategoryEntity = SHP.Entities.Category;

namespace SHP.Controls.Category
{
		public class CategoriesControl: CmsControl
		{

				[ToolboxItem( false )]
				public class CategoriesDataList: WebControl, INamingContainer
				{
						private List<CategoryEntity> datasource = null;

						internal CategoriesDataList( string tag, CategoriesControl owner, List<CategoryEntity> datasource )
								: base( tag )
						{
								this.datasource = datasource;
								this.CssClass = owner.CssClass + "_container";
						}

						[Bindable( true )]
						public List<CategoryEntity> DataSource
						{
								get { return this.datasource; }
						}
				}

				public string DisplayUrlFormat { get; set; }
				private string tag = "div";
				public string Tag { get { return tag; } set { tag = value; } }

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "Categories template." )]
				[TemplateContainer( typeof( CategoriesDataList ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate CategoriesTemplate { get; set; }

				public object SelectedValue { get; set; }

				#region Protected overrides
				protected override void CreateChildControls()
				{
						Controls.Clear();		// clear out the control hierarchy

						ITemplate template = this.CategoriesTemplate;
						if ( template == null ) template = new DefaultTemplate( this );

						List<CategoryEntity> list = Storage<CategoryEntity>.Read();
						if ( list.Count == 0 )
						{
								this.Visible = false;
								return;
						}

						CategoriesDataList dataList = new CategoriesDataList( this.Tag, this, list );
						template.InstantiateIn( dataList );
						Controls.Add( dataList );

						this.DataBind();
				}
				#endregion

				internal sealed class DefaultTemplate: ITemplate
				{
						public string CssClass { get; set; }
						private string DisplayAliasUrlFormat { get; set; }
						public string DisplayUrlFormat { get; set; }
						public object SelectedValue { get; set; }
						private TreeNode selectedNode = null;


						public DefaultTemplate( CategoriesControl owner )
						{
								this.CssClass = owner.CssClass;
								this.DisplayAliasUrlFormat = "{0}?" + owner.BuildReturnUrlQueryParam();
								this.DisplayUrlFormat = owner.DisplayUrlFormat;

								this.SelectedValue = owner.SelectedValue;
						}

						void ITemplate.InstantiateIn( Control container )
						{
								CategoriesDataList dataList = (CategoriesDataList)container;

								TreeView tv = new TreeView();
								tv.CssClass = this.CssClass;
								tv.ShowLines = true;
								tv.ShowExpandCollapse = true;
								tv.NodeWrap = true;
								tv.SelectedNodeStyle.CssClass = this.CssClass + "_selectedItem";

								foreach ( CategoryEntity entity in dataList.DataSource )
								{
										if ( entity.ParentId.HasValue )
												continue;

										string url = container.ResolveUrl( entity.Alias );
										TreeNode node = new TreeNode( entity.Name, entity.Id.ToString(), string.Empty, url, string.Empty );
										PopulateChilds( node, dataList );
										node.Collapse();
										tv.Nodes.Add( node );

										if ( this.SelectedValue != null && node.Value == this.SelectedValue.ToString() )
												this.selectedNode = node;
								}
								container.Controls.Add( tv );

								//Expand and select selected node!
								if ( this.selectedNode != null )
								{
										TreeNode paren = selectedNode.Parent;
										while ( paren != null )
										{
												paren.Expand();
												paren = paren.Parent;
										}
										selectedNode.Select();

								}
						}

						private void PopulateChilds( TreeNode rootNode, CategoriesDataList dataList )
						{
								foreach ( CategoryEntity entity in dataList.DataSource )
								{
										if ( entity.ParentId != Convert.ToInt32( rootNode.Value ) )
												continue;

										string url = dataList.ResolveUrl( entity.Alias );
										TreeNode node = new TreeNode( entity.Name, entity.Id.ToString(), string.Empty, url, string.Empty );
										PopulateChilds( node, dataList );
										rootNode.ChildNodes.Add( node );
										node.Collapse();

										if ( this.SelectedValue != null && node.Value == this.SelectedValue.ToString() )
												this.selectedNode = node;
								}
						}

				}
		}
}
