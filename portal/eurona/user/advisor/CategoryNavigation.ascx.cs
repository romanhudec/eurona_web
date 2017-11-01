using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CategoryEntity = SHP.Entities.Category;
using ASPMenu = System.Web.UI.WebControls.Menu;
using System.Web.UI.HtmlControls;

namespace Eurona.User.Advisor {
    public partial class CategoryNavigation : CMS.Controls.CmsControl {
        //private static int CATEGORY_CERNYCOSMETICS_ID = 3003;
        private List<CategoryEntity> list = null;
        private List<CategoryEntity> listCL = null;
        protected void Page_Load(object sender, EventArgs e) {
            this.list = Storage<CategoryEntity>.Read();
            this.listCL = Storage<CategoryEntity>.Read(new CategoryEntity.ReadByInstance { InstanceId = (int)Eurona.Common.Application.InstanceType.CernyForLife });

            /*
            CategoryEntity cc = new CategoryEntity { Id = CATEGORY_CERNYCOSMETICS_ID, Name = "Cerny Cosmetics" };
            cc.Icon = "~/images/kategorie/cc.png";
            cc.Order = 98;
            this.list.Add(cc);
            */

            /*
            //Zakomentovat 2.12.2016
            CategoryEntity ce = new CategoryEntity { Id = 0, Name = "Cerny for Life" };
            ce.Icon = "~/images/kategorie/cl.png";
            ce.Order = 99;
            this.list.Add(ce);
            */

            Comparison<CategoryEntity> comparison = new Comparison<CategoryEntity>(delegate(CategoryEntity a, CategoryEntity b) {
                if (!a.Order.HasValue) a.Order = 0;
                if (!b.Order.HasValue) b.Order = 0;
                return a.Order == b.Order ? 0 : (Math.Max((decimal)a.Order, (decimal)b.Order) == a.Order ? 1 : -1);

            });
            this.list.Sort(comparison);
        }

        public bool RemoveLastSeparator { get; set; }
        public Orientation Orientation { get; set; }
        public object SelectedValue { get; set; }

        protected override void CreateChildControls() {
            base.CreateChildControls();
            #region Create Menu control
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
            this.aspMenu.StaticSubMenuIndent = Unit.Pixel(0);
            this.aspMenu.CssClass = this.CssClass;

            this.aspMenu.StaticMenuStyle.HorizontalPadding = Unit.Pixel(0);
            this.aspMenu.StaticMenuItemStyle.HorizontalPadding = Unit.Pixel(0);

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

            foreach (CategoryEntity category in this.list) {
                if (category.ParentId.HasValue) continue;

                if (category.Id != 0)
                    category.Icon = "~/images/kategorie/" + category.Id.ToString() + ".png";
                string imagePath = Server.MapPath(Page.ResolveUrl(category.Icon));
                if (System.IO.File.Exists(imagePath))
                    category.Name = string.Empty;
                else
                    category.Icon = string.Empty;

                MenuItem mi = new MenuItem(category.Name, category.Id.ToString(), Page.ResolveUrl(category.Icon), Page.ResolveUrl(category.Alias), string.Empty);
                if (!string.IsNullOrEmpty(MenuItemSeparatorImageUrl))
                    mi.SeparatorImageUrl = Page.ResolveUrl(MenuItemSeparatorImageUrl);
                this.aspMenu.Items.Add(mi);

                List<CategoryEntity> subCategories = Storage<CategoryEntity>.Read(new CategoryEntity.ReadByParentId { ParentId = category.Id });
                foreach (CategoryEntity menuItem in subCategories) {
                    if (menuItem.Name.ToUpper() == "CÉ") {
                        menuItem.Icon = "~/images/kategorie/cc.png";
                        menuItem.Name = "";
                    }
                    if (menuItem.Name.ToUpper() == "CH") {
                        menuItem.Icon = "~/images/kategorie/ch.png";
                        menuItem.Name = "";
                    }
                    MenuItem miChild = new MenuItem(menuItem.Name, menuItem.Id.ToString(), Page.ResolveUrl(menuItem.Icon), Page.ResolveUrl(menuItem.Alias), string.Empty);
                    mi.ChildItems.Add(miChild);
                }
            }

            #region CL kategories
            ////Zakomentovat 02.12.2016
            //MenuItem miCl = this.aspMenu.Items[this.aspMenu.Items.Count - 2];
            //foreach (CategoryEntity categoryCL in this.listCL) {
            //    if (categoryCL.ParentId.HasValue) continue;
            //    if (categoryCL.Id != 3042/*"CL LIVIENNE"*/ /*&& categoryCL.Id != 3003/*"CC"*/ && categoryCL.Id != 3005/*"CH"*/ &&
            //        categoryCL.Id != 3007/*"CÉ"*/ && categoryCL.Id != 3016/*"CA"*/) continue;

            //    if (categoryCL.Id != 0)
            //        categoryCL.Icon = "~/images/kategorie/cl/" + categoryCL.Id.ToString() + ".png";
            //    string imagePath = Server.MapPath(Page.ResolveUrl(categoryCL.Icon));
            //    if (System.IO.File.Exists(imagePath))
            //        categoryCL.Name = string.Empty;
            //    else
            //        categoryCL.Icon = string.Empty;

            //    MenuItem miChildCL = new MenuItem(categoryCL.Name, categoryCL.Id.ToString(), Page.ResolveUrl(categoryCL.Icon), Page.ResolveUrl(categoryCL.Alias), string.Empty);
            //    miCl.ChildItems.Add(miChildCL);

            //    List<CategoryEntity> subCategoriesCL = Storage<CategoryEntity>.Read(new CategoryEntity.ReadByParentId { ParentId = categoryCL.Id, InstanceId = categoryCL.InstanceId });
            //    foreach (CategoryEntity menuItemSubCL in subCategoriesCL) {
            //        MenuItem miChildSubCL = new MenuItem(menuItemSubCL.Name, menuItemSubCL.Id.ToString(), Page.ResolveUrl(menuItemSubCL.Icon), Page.ResolveUrl(menuItemSubCL.Alias), string.Empty);
            //        miChildCL.ChildItems.Add(miChildSubCL);
            //    }
            //}
            #endregion

            #region Cerny Cosmetics
            ////MenuItem miClCC = this.aspMenu.Items[this.aspMenu.Items.Count - 3];//02.12.2016 nastavit -2
            //MenuItem miClCC = this.aspMenu.Items[this.aspMenu.Items.Count - 2];
            //List<CategoryEntity> subCategoriesCLCC = Storage<CategoryEntity>.Read(new CategoryEntity.ReadByParentId { ParentId = CATEGORY_CERNYCOSMETICS_ID, InstanceId = 3 });
            //foreach (CategoryEntity menuItemSubCL in subCategoriesCLCC) {
            //    MenuItem miChildSubCL = new MenuItem(menuItemSubCL.Name, menuItemSubCL.Id.ToString(), Page.ResolveUrl(menuItemSubCL.Icon), Page.ResolveUrl(menuItemSubCL.Alias), string.Empty);
            //    miClCC.ChildItems.Add(miChildSubCL);
            //}

            ////New 1.12.2016 - odstranene CL a pridane do CC
            //foreach (CategoryEntity categoryCL in this.listCL) {
            //    if (categoryCL.ParentId.HasValue) continue;
            //    if (categoryCL.Id != 3042/*"CL LIVIENNE"*/ /*&& categoryCL.Id != 3003/*"CC"*/ && categoryCL.Id != 3005/*"CH"*/ &&
            //        categoryCL.Id != 3007/*"CÉ"*/ && categoryCL.Id != 3016/*"CA"*/) continue;

            //    if (categoryCL.Id != 0)
            //        categoryCL.Icon = "~/images/kategorie/cl/" + categoryCL.Id.ToString() + ".png";
            //    string imagePath = Server.MapPath(Page.ResolveUrl(categoryCL.Icon));
            //    if (System.IO.File.Exists(imagePath))
            //        categoryCL.Name = string.Empty;
            //    else
            //        categoryCL.Icon = string.Empty;


            //    MenuItem miChildCL = new MenuItem(categoryCL.Name, categoryCL.Id.ToString(), Page.ResolveUrl(categoryCL.Icon), Page.ResolveUrl(categoryCL.Alias), string.Empty);
            //    miClCC.ChildItems.Add(miChildCL);

            //    List<CategoryEntity> subCategoriesCL = Storage<CategoryEntity>.Read(new CategoryEntity.ReadByParentId { ParentId = categoryCL.Id, InstanceId = categoryCL.InstanceId });
            //    foreach (CategoryEntity menuItemSubCL in subCategoriesCL) {
            //        MenuItem miChildSubCL = new MenuItem(menuItemSubCL.Name, menuItemSubCL.Id.ToString(), Page.ResolveUrl(menuItemSubCL.Icon), Page.ResolveUrl(menuItemSubCL.Alias), string.Empty);
            //        miChildCL.ChildItems.Add(miChildSubCL);
            //    }
            //}
            #endregion

            if (RemoveLastSeparator == true && this.aspMenu.Items.Count != 0) {
                MenuItem mi = this.aspMenu.Items[this.aspMenu.Items.Count - 1];
                if (!string.IsNullOrEmpty(mi.SeparatorImageUrl)) mi.SeparatorImageUrl = string.Empty;
            }

        }

        public string RenderStyle() {

            EnsureChildControls();

            string style = "<style type=\"text/css\">\r\n";
            int index = 0;
            foreach (MenuItem mi in this.aspMenu.Items) {
                string imageName = (mi.Value == "0" ? "cl" : mi.Value) + ".png";
                string imageS = Page.ResolveUrl("~/images/kategorie/selected_" + imageName);
                string imageN = Page.ResolveUrl("~/images/kategorie/normal_" + imageName);
                /*
                string imageS = "../images/kategorie/selected_" + imageName;
                string imageN = "../images/kategorie/normal_" + imageName;
                 * */
                style += "#" + this.aspMenu.ClientID + "n" + index.ToString() + " a{background-image:url(" + imageN + ")!important;background-repeat:no-repeat;width:auto;}\r\n";
                style += "#" + this.aspMenu.ClientID + "n" + index.ToString() + ":hover a{background-image:url(" + imageS + ")!important;background-repeat:no-repeat;width:auto;}\r\n";
                if (mi.Value == "0"/*cl*/ ) {
                    style += "#" + this.aspMenu.ClientID + "n" + index.ToString() + " a{width:100%!important;text-align:left!important;}\r\n";
                    style += "#" + this.aspMenu.ClientID + "n" + index.ToString() + " {width:100%!important;}\r\n";
                }
                index++;
            }
            style += "</style>";
            return style;
        }

        public string StaticTopSeparatorImageUrl { get; set; }
        public string StaticBottomSeparatorImageUrl { get; set; }
        public string StaticPopOutImageUrl { get; set; }
        public string MenuItemSeparatorImageUrl { get; set; }
        public bool StaticEnableDefaultPopOutImage { get; set; }
        public bool DynamicEnableDefaultPopOutImage { get; set; }
    }
}