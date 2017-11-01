using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Controls;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ProductsEntity = Eurona.Common.DAL.Entities.Product;
using System.IO;
using Eurona.Common.Controls.Cart;

namespace Eurona.Common.Controls.Product
{
	public class ProductsControl : CmsControl
	{
		//private ProductsFilterControl filterControl = null;
		private ListView listView = null;
		private Label lblResultCount = null;

		public ProductsControl()
		{
			this.RepeatColumns = 1;
			this.AllowPaging = true;
			this.PageSize = 20;
		}

		public string DisplayUrlFormat { get; set; }
		public int RepeatColumns { get; set; }
		public bool AllowPaging { get; set; }
		public int PageSize { get; set; }

		public int CategoryId
		{
			get
			{
				object o = ViewState["CategoryId"];
				return o != null ? (int)Convert.ToInt32(o) : 0;
			}
			set { ViewState["CategoryId"] = value; }
		}

		public ProductsEntity.ReadByFilter Filter { get; set; }

		[Browsable(false)]
		[DefaultValue(null)]
		[Description("Products Layout template.")]
		[TemplateContainer(typeof(ListViewDataItem))]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual ITemplate ProductsLayoutTemplate { get; set; }

		[Browsable(false)]
		[DefaultValue(null)]
		[Description("Products Group template.")]
		[TemplateContainer(typeof(ListViewDataItem))]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual ITemplate ProductsGroupTemplate { get; set; }

		[Browsable(false)]
		[DefaultValue(null)]
		[Description("Products Item template.")]
		[TemplateContainer(typeof(ListViewDataItem))]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual ITemplate ProductsItemTemplate { get; set; }

		#region Protected overrides
		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			HtmlGenericControl div = new HtmlGenericControl("div");
			div.Attributes.Add("class", this.CssClass);

			this.lblResultCount = new Label();
			this.lblResultCount.CssClass = this.CssClass + "_resultMessage";

			//Select template
			ITemplate layoutTemplate = this.ProductsLayoutTemplate;
			if (layoutTemplate == null) layoutTemplate = new DefaultProductsLayoutTemplate(this);

			//Select template
			ITemplate groupTemplate = this.ProductsGroupTemplate;
			if (groupTemplate == null) groupTemplate = new DefaultProductsGroupTemplate();

			//Select template
			ITemplate itemTemplate = this.ProductsItemTemplate;
			if (itemTemplate == null) itemTemplate = new DefaultProductsItemTemplate(this);

			this.listView = new ListView();
			this.listView.DataKeyNames = new string[] { "Id" };
			this.listView.GroupItemCount = this.RepeatColumns;
			this.listView.LayoutTemplate = layoutTemplate;
			this.listView.GroupTemplate = groupTemplate;
			this.listView.ItemTemplate = itemTemplate;
			this.listView.PreRender += (s, e) =>
			{
				this.listView.DataBind();
			};
			div.Controls.Add(lblResultCount);
			div.Controls.Add(listView);

			this.Controls.Add(div);

			//Binding
			List<ProductsEntity> dataSource = this.GetDataListData(this.Filter);
			this.listView.DataSource = dataSource;
			this.lblResultCount.Visible = false;
			if (dataSource.Count == 0)
			{
				this.lblResultCount.Text = global::SHP.Resources.Controls.ProductsControl_NoProductsFound_Message;
				this.lblResultCount.Visible = true;
			}
		}

		public void OnFilter(Eurona.Common.DAL.Entities.Product.ReadByFilter filter)
		{
			//Binding
			EnsureChildControls();
			List<ProductsEntity> dataSource = this.GetDataListData(filter);
			listView.DataSource = dataSource;
			this.listView.DataBind();

			this.lblResultCount.Visible = false;
			if (dataSource.Count == 0)
			{
				this.lblResultCount.Text = global::SHP.Resources.Controls.ProductsControl_NoProductsFound_Message;
				this.lblResultCount.Visible = true;
			}
		}
		#endregion

		private List<ProductsEntity> GetDataListData(ProductsEntity.ReadByFilter filter)
		{
			List<ProductsEntity> list = new List<ProductsEntity>();

			if (this.CategoryId != 0)
			{
				ProductsEntity.ReadAllInCategory allInCategory = new ProductsEntity.ReadAllInCategory { CategoryId = this.CategoryId };
				if (filter != null)
					allInCategory.ByFilter = filter;

				list = Storage<ProductsEntity>.Read(allInCategory);
			}
			else if (filter != null)
				list = Storage<ProductsEntity>.Read(filter);
			else
				list = Storage<ProductsEntity>.Read();

			return list;
		}

		#region Templates
		private class DefaultProductsLayoutTemplate : ITemplate
		{
			private ProductsControl owner = null;
			public DefaultProductsLayoutTemplate(ProductsControl owner)
			{
				this.owner = owner;
			}
			#region ITemplate Members
			public void InstantiateIn(Control container)
			{
				HtmlGenericControl table = new HtmlGenericControl("table");
				table.Attributes.Add("class", owner.CssClass + "_list");

				//Item placeHolder
				PlaceHolder ph = new PlaceHolder();
				ph.ID = "groupPlaceholder";
				table.Controls.Add(ph);
				container.Controls.Add(table);

				if (this.owner.AllowPaging)
				{
					HtmlGenericControl div = new HtmlGenericControl("div");
					div.Attributes.Add("class", owner.CssClass + "_dataPager");

					//Pager
					DataPager dataPager = new DataPager();
					dataPager.ID = "dataPager";
					dataPager.PageSize = this.owner.PageSize;
					dataPager.Fields.Add(new NumericPagerField() { ButtonCount = 10 });
					dataPager.PagedControlID = container.ID;
					div.Controls.Add(dataPager);

					container.Controls.Add(div);
				}
			}
			#endregion
		}
		private class DefaultProductsGroupTemplate : ITemplate
		{
			#region ITemplate Members

			public DefaultProductsGroupTemplate()
			{
			}

			public void InstantiateIn(Control container)
			{
				HtmlGenericControl tr = new HtmlGenericControl("tr");

				//Item placeHolder
				PlaceHolder ph = new PlaceHolder();
				ph.ID = "itemPlaceHolder";
				tr.Controls.Add(ph);
				container.Controls.Add(tr);
			}

			#endregion
		}

		private class DefaultProductsItemTemplate : ITemplate
		{
			#region Public properties
			private ProductsControl owner = null;
			public string CssClass { get; set; }
			private string DisplayAliasUrlFormat { get; set; }
			private string DisplayUrlFormat { get; set; }

			#endregion

			#region ITemplate Members

			public DefaultProductsItemTemplate(ProductsControl owner)
			{
				this.owner = owner;
				this.CssClass = owner.CssClass + "_item";
				this.DisplayAliasUrlFormat = "{0}?" + owner.BuildReturnUrlQueryParam();
				this.DisplayUrlFormat = owner.DisplayUrlFormat + "&" + owner.BuildReturnUrlQueryParam();
			}

			public void InstantiateIn(Control container)
			{
				container.Controls.Add(new LiteralControl("Implement in Eurona OR CL!"));
			}
			#endregion
		}
		#endregion
	}
}
