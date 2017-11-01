using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MenuEntity = CMS.Entities.NavigationMenu;
using MenuItemEntity = CMS.Entities.NavigationMenuItem;
using PageEntity = CMS.Entities.Page;
using ASPMenu = System.Web.UI.WebControls.Menu;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.ComponentModel;

namespace CMS.Controls.Menu
{
		public class NavigationMenuControl: CmsControl
		{
				private ASPMenu aspMenu = null;
				private List<MenuItem> customMenuItems = null;

				public NavigationMenuControl()
				{
						this.customMenuItems = new List<MenuItem>();
				}

				/// <summary>
				/// Kod menu, podla ktoreho sa nacita prislusne navigacne menu.
				/// </summary>
				public string Code { get; set; }

				[DefaultValue( false )]
				public bool RemoveLastSeparator { get; set; }

				[DefaultValue( Orientation.Horizontal )]
				public Orientation Orientation { get; set; }

				protected override void CreateChildControls()
				{
						base.CreateChildControls();
						#region Create Menu control
						this.aspMenu = new ASPMenu();
						this.aspMenu.ID = "aspNavigationMenu";
						this.aspMenu.EnableViewState = false;
						this.aspMenu.Orientation = this.Orientation;
						this.aspMenu.StaticDisplayLevels = 1;
						this.aspMenu.MaximumDynamicDisplayLevels = 3;
						this.aspMenu.StaticSubMenuIndent = 0;
						this.aspMenu.DynamicHorizontalOffset = 0;
						this.aspMenu.DynamicVerticalOffset = 0;
						this.aspMenu.StaticPopOutImageUrl = this.StaticPopOutImageUrl;
						this.aspMenu.StaticPopOutImageTextFormatString = "";
						this.aspMenu.SkipLinkText = "";
						this.aspMenu.StaticSubMenuIndent = Unit.Pixel( 0 );
						this.aspMenu.CssClass = this.CssClass;

						this.aspMenu.StaticMenuStyle.HorizontalPadding = Unit.Pixel( 0 );
						this.aspMenu.StaticMenuItemStyle.HorizontalPadding = Unit.Pixel( 0 );

						this.aspMenu.StaticEnableDefaultPopOutImage = StaticEnableDefaultPopOutImage;
						this.aspMenu.DynamicEnableDefaultPopOutImage = DynamicEnableDefaultPopOutImage;
						this.aspMenu.StaticTopSeparatorImageUrl = this.StaticTopSeparatorImageUrl;
						this.aspMenu.StaticBottomSeparatorImageUrl = this.StaticBottomSeparatorImageUrl;

						this.aspMenu.StaticMenuItemStyle.Font.Size = FontUnit.Larger;

						this.aspMenu.StaticMenuItemStyle.CssClass = this.CssClass + "_StaticMenuItemStyle";
						this.aspMenu.StaticMenuStyle.CssClass = this.CssClass + "_StaticMenuStyle";
						this.aspMenu.StaticSelectedStyle.CssClass = this.CssClass + "_StaticSelectedStyle";
						this.aspMenu.StaticHoverStyle.CssClass = this.CssClass + "_StaticHoverStyle";

						this.aspMenu.DynamicMenuStyle.CssClass = this.CssClass + "_DynamicMenuStyle";
						this.aspMenu.DynamicMenuItemStyle.CssClass = this.CssClass + "_DynamicMenuItemStyle";
						this.aspMenu.DynamicSelectedStyle.CssClass = this.CssClass + "_DynamicSelectedStyle";
						this.aspMenu.DynamicHoverStyle.CssClass = this.CssClass + "_DynamicHoverStyle";
						#endregion

						List<MenuEntity> mainMenuList = Storage<MenuEntity>.Read( new MenuEntity.ReadForCurrentAccount { Code = this.Code } );
						foreach ( MenuEntity mainMenuItem in mainMenuList )
						{
								MenuItem mi = new MenuItem( mainMenuItem.Name, mainMenuItem.Id.ToString(), Page.ResolveUrl( mainMenuItem.Icon ), Page.ResolveUrl( mainMenuItem.Alias ), string.Empty );
								if ( !string.IsNullOrEmpty( MenuItemSeparatorImageUrl ) )
										mi.SeparatorImageUrl = Page.ResolveUrl( MenuItemSeparatorImageUrl );
								this.aspMenu.Items.Add( mi );

								//Create child menu items
								List<MenuItemEntity> menuItemList = Storage<MenuItemEntity>.Read( new MenuItemEntity.ReadForCurrentAccount() { NavigationMenuId = mainMenuItem.Id } );
								foreach ( MenuItemEntity menuItem in menuItemList )
								{
										MenuItem miChild = new MenuItem( menuItem.Name, menuItem.Id.ToString(), Page.ResolveUrl( menuItem.Icon ), Page.ResolveUrl( menuItem.Alias ), string.Empty );
										mi.ChildItems.Add( miChild );
								}
						}

						this.customMenuItems.ForEach( m => this.aspMenu.Items.Add( m ) );
						if ( RemoveLastSeparator == true && this.aspMenu.Items.Count != 0 )
						{
								MenuItem mi = this.aspMenu.Items[this.aspMenu.Items.Count - 1];
								if ( !string.IsNullOrEmpty( mi.SeparatorImageUrl ) ) mi.SeparatorImageUrl = string.Empty;
						}

						this.Controls.Add( this.aspMenu );
				}

				/// <summary>
				///  Gets top level menu item by index.
				/// </summary>
				public MenuItem this[int index]
				{
						get { EnsureChildControls(); return this.aspMenu.Items[index]; }
				}
				public int MenuItemsCount
				{
						get { EnsureChildControls(); return this.aspMenu.Items.Count; }
				}
				/// <summary>
				/// Odstranenie menu polozky
				/// </summary>
				public void RemoveAt( int index )
				{
						EnsureChildControls(); this.aspMenu.Items.RemoveAt( index );
				}
				/// <summary>
				/// Pridanie menu polozky
				/// </summary>
				public void AddAt( int index, MenuItem item )
				{
						EnsureChildControls(); this.aspMenu.Items.AddAt( index, item );
				}

				/// <summary>
				/// Add custom menu.
				/// </summary>
				public void AddMenuItem( MenuItem mi )
				{
						this.customMenuItems.Add( mi );
				}

				public string StaticTopSeparatorImageUrl { get; set; }
				public string StaticBottomSeparatorImageUrl { get; set; }
				public string StaticPopOutImageUrl { get; set; }
				public string MenuItemSeparatorImageUrl { get; set; }
				public bool StaticEnableDefaultPopOutImage { get; set; }
				public bool DynamicEnableDefaultPopOutImage { get; set; }
		}
}
