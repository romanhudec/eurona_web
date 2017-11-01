using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShpResources = SHP.Resources.Controls;
using ShpCultureUtilities = Eurona.Common.Utilities.CultureUtilities;
using ProductsEntity = Eurona.Common.DAL.Entities.Product;
using System.IO;
using Eurona.Common.Controls.Cart;
using Eurona.Common.DAL.Entities;
using Eurona.Common;

namespace Eurona.EShop
{
	public partial class ProductsControl : System.Web.UI.UserControl
	{
		public event EventHandler OnLoadProducts;
		public event EventHandler OnProductAddedToChart;
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		public int CategoryId
		{
			get { return this.productsControl.CategoryId; }
			set { this.productsControl.CategoryId = value; }
		}

		public ProductsEntity.ReadByFilter Filter
		{
			get { return this.productsControl.Filter; }
			set { this.productsControl.Filter = value; }
		}

		public string GetShpResourceString(string key)
		{
			return ShpResources.ResourceManager.GetString(key);
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			if (OnLoadProducts != null)
				OnLoadProducts(this, null);
		}

		public string RenderImage(int productId)
		{
			string noImageImg = string.Format("<img src='{0}' style='border:0px none #fff;'>", this.ResolveUrl("~/images/noimage.png"));

			string storageUrl = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:ImageGallery:Product:StoragePath", this.Page);

			string storagePath = string.Format("{0}{1}/", storageUrl, productId);
			string productImagesPath = this.Server.MapPath(storagePath);

			if (!Directory.Exists(productImagesPath))
				return noImageImg;

			DirectoryInfo di = new DirectoryInfo(productImagesPath);
			FileInfo[] fileInfos = di.GetFiles("*.*");

			if (fileInfos.Length == 0)
				return noImageImg;
            
            //Sort files by name
            Comparison<FileInfo> comparison = new Comparison<FileInfo>(delegate(FileInfo a, FileInfo b)
            {
                return String.Compare(a.Name, b.Name);
            });
            Array.Sort(fileInfos, comparison);

			string urlThumbnail = storagePath + "_t/" + fileInfos[0].Name;
			string img = string.Format("<img src='{0}' style='border:0px none #fff;max-height:150px;max-width:100px;'>", this.ResolveUrl(urlThumbnail));

			return img;
		}

		protected void OnAddCart(object sender, EventArgs e)
		{
			Button btn = (sender as Button);
			if (string.IsNullOrEmpty(btn.CommandArgument))
				return;
			int quantity = 1;
			int productId = 0;

			TextBox txtQuantity = (TextBox)btn.Parent.Controls[2];
			if (!Int32.TryParse(txtQuantity.Text, out quantity)) quantity = 1;

			Int32.TryParse(btn.CommandArgument, out productId);

			ProductsEntity product = Storage<ProductsEntity>.ReadFirst(new ProductsEntity.ReadById { ProductId = productId });
			if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(product.Code, product, quantity, this))
				return;
			if (!EuronaCartHelper.AddProductToCart(this.Page, productId, quantity, this))
				return;

			//Alert s informaciou o pridani do nakupneho kosika
			string js = string.Format("alert('{0}');", string.Format(ShpResources.AdminProductControl_ProductWasAddedToCart_Message, btn.CommandName, quantity));
			this.Page.ClientScript.RegisterStartupScript(this.GetType(), "addProductToCart", js, true);

			if (OnProductAddedToChart != null)
				OnProductAddedToChart(this, e);
		}

		protected void OnDetail(object sender, EventArgs e)
		{
			LinkButton btn = (sender as LinkButton);
			if (string.IsNullOrEmpty(btn.CommandArgument))
				return;

			Response.Redirect(Page.ResolveUrl(btn.CommandArgument));
		}
	}
}