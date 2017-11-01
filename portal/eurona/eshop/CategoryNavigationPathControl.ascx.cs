using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CategoryEntity = SHP.Entities.Category;
using System.Web.UI.HtmlControls;
using ShpProductEntity = Eurona.Common.DAL.Entities.Product;

namespace Eurona.EShop
{
    public partial class CategoryNavigationPathControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public int? CategoryId
        {
            get
            {
                object o = ViewState["CategoryId"];
                return o != null ? (int?)Convert.ToInt32(o) : null;
            }
            set { ViewState["CategoryId"] = value; }
        }

        public int? ProductId { get; set; }
        public string CssClass { get; set; }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes.Add("class", this.CssClass);

            if (this.CategoryId.HasValue)
                FillParentCategory(div, this.CategoryId.Value);

            //Fill Home
            CategoryEntity home = new CategoryEntity();
            home.Name = "Home";
            home.Alias = "~/home";
            Control navigationItem = CreateNavigationItem(home, true, false);
            if (navigationItem != null) div.Controls.AddAt(0, navigationItem);

            if (this.ProductId.HasValue)
            {
                ShpProductEntity product = Storage<ShpProductEntity>.ReadFirst(new ShpProductEntity.ReadById { ProductId = this.ProductId.Value });

                CategoryEntity productNavigation = new CategoryEntity();
                productNavigation.Name = product.Name;
                productNavigation.Alias = product.Alias;
                navigationItem = CreateNavigationItem(productNavigation, false, true);
                if (navigationItem != null) div.Controls.Add(navigationItem);

                ////Back button
                //LiteralControl ctrl = new LiteralControl( "<a href='javascript: history.go(-1)'><div class='navigation-back'><span>zpět</span></div></a>" );
                //div.Controls.Add( ctrl );
            }

            this.Controls.Add(div);

        }

        private void FillParentCategory(Control mainElement, int rootId)
        {
            CategoryEntity parent = Storage<CategoryEntity>.ReadFirst(new CategoryEntity.ReadById { CategoryId = rootId });
            if (parent != null)
            {
                Control navigationItem = CreateNavigationItem(parent, false, false);
                if (navigationItem != null) mainElement.Controls.AddAt(0, navigationItem);

                if (!parent.ParentId.HasValue)
                    return;

                FillParentCategory(mainElement, parent.ParentId.Value);
            }
        }

        private Control CreateNavigationItem(CategoryEntity category, bool isHome, bool isProduct)
        {
            string cssclass = string.Empty;
            if (isHome) cssclass = "navigation-item-home";
            else if (isProduct) cssclass = "navigation-item-product";
            else cssclass = "navigation-item-category";

            string url = this.Page.ResolveUrl(category.Alias);
            LiteralControl ctrl = new LiteralControl(string.Format("<div class='navigation-item-product'><span>{0}</span></div>", category.Name));
            if (!isProduct)
                ctrl = new LiteralControl(string.Format("<a href='{0}'><div class='{1}'><span>{2}</span></div></a>", url, cssclass, category.Name));

            return ctrl;
        }
    }
}