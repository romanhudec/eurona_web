using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ShpResources = SHP.Resources.Controls;
using ShpCultureUtilities = Eurona.Common.Utilities.CultureUtilities;
using SHP.Controls.Cart;

namespace Eurona.EShop
{
	/// <summary>
	/// Stranka zobrazi vsetky produkty, ktore su nejako zvyraznene (TOP, Akciove, Specialne, .... )
	/// </summary>
	public partial class ProductsPage : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack) this.productsControl.DataBind();
			this.productsControl.OnProductAddedToChart += new EventHandler(productsControl_OnProductAddedToChart);
		}

		void productsControl_OnProductAddedToChart(object sender, EventArgs e)
		{
			this.UpdateCartInfo();
		}

		protected void OnLoadProducts(object sender, EventArgs e)
		{
			string action = Request["id"];
			//TOP, AKCIOVE, NOVE, VYROBKY
			switch (action)
			{
				case "ds":
					this.Title = Resources.EShopStrings.Navigation_MenuItem_DarkoveSady;
					this.productsControl.Filter = new Eurona.Common.DAL.Entities.Product.ReadByFilter { DarkoveSety = true, SortBy= SHP.Entities.Product.SortBy.DarkovySet };
					break;
				case "top":
					this.Title = Resources.EShopStrings.Navigation_MenuItem_TopProducts;
					this.productsControl.Filter = new Eurona.Common.DAL.Entities.Product.ReadByFilter { TOPProducts = true };
					break;
				case "news":
					this.Title = Resources.EShopStrings.Navigation_MenuItem_NewProducts;
					this.productsControl.Filter = new Eurona.Common.DAL.Entities.Product.ReadByFilter { Novinka = true, SortBy = SHP.Entities.Product.SortBy.IdASC };
					break;
                case "vyprodej":
                    this.Title = Resources.EShopStrings.Navigation_MenuItem_Vyprodej;
                    this.productsControl.Filter = new Eurona.Common.DAL.Entities.Product.ReadByFilter { Vyprodej = true };
                    break;
			}
			//this.productsControl.Filter = new SHP.Entities.Product.ReadByFilter { Expression = Request["keywords"] };
		}

		/// <summary>
		/// Update informácie v nákupnom košiku.
		/// </summary>
		public void UpdateCartInfo()
		{
			(this.Master as Eurona.EShop.DefautMasterPage).UpdateCartInfo();
		}
	}
}
