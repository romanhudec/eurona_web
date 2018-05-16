using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CartEntity = Eurona.Common.DAL.Entities.Cart;
using CartProductEntity = Eurona.Common.DAL.Entities.CartProduct;
using OrderEntity = Eurona.DAL.Entities.Order;
using ShipmentEntity = SHP.Entities.Classifiers.Shipment;
using PaymentEntity = SHP.Entities.Classifiers.Payment;
using AccountEntity = Eurona.DAL.Entities.Account;
using ProductEntity = Eurona.Common.DAL.Entities.Product;
using Eurona.Common.Controls.Cart;
using System.ComponentModel;
using Eurona.Common.DAL.Entities;
using CMS.Controls;
using SHP.Controls;
using Eurona.Controls;

namespace Eurona.User.Anonymous
{
	public partial class CartControl : CMS.Controls.CmsControl
	{
		private const string DELETE_COMMAND = "DELETE_ITEM";

		public delegate bool OnProccessOrderBeforeSaveHandler(out string result);
		public delegate bool OnProccessOrderAfterSaveHandler(int orderId, out string invoiceUrl, out string result);

		public event OnProccessOrderBeforeSaveHandler OnProccessOrderBeforeSave;
		public event OnProccessOrderAfterSaveHandler OnProccessOrderAfterSave;

		public event EventHandler OnCartItemsChanged;

		private GridViewEx dataGrid = null;

		private CartEntity cartEntity = null;
		public CartEntity CartEntity
		{
			get
			{
				if (this.cartEntity != null) return this.cartEntity;
				this.cartEntity = EuronaCartHelper.GetAccountCart(this.Page);
				return this.cartEntity;
			}
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			this.dataGrid = CreateGridControl();
			this.Controls.Add(this.dataGrid);

			//Binding
			GridViewDataBind(!IsPostBack);
		}

		private void GridViewDataBind(bool bind)
		{
			List<CartProductEntity> list = new List<CartProductEntity>();
			if (this.CartEntity != null)
				list = Storage<CartProductEntity>.Read(new CartProductEntity.ReadByCart { CartId = this.CartEntity.Id });

			this.dataGrid.PagerTemplate = null;
			dataGrid.DataSource = list;
			if (bind)
			{
				dataGrid.DataKeyNames = new string[] { "Id" };
				dataGrid.DataBind();
			}
		}


		private GridViewEx CreateGridControl()
		{
			GridViewEx grid = new GridViewEx();
			grid.ShowWhenEmpty = true;
			grid.EnableViewState = true;
			grid.GridLines = GridLines.None;
			grid.RowDataBound += new GridViewRowEventHandler(grid_RowDataBound);

			grid.CssClass = CssClass;
			grid.RowStyle.CssClass = CssClass + "_rowStyle";
			grid.FooterStyle.CssClass = CssClass + "_footerStyle";
			grid.PagerStyle.CssClass = CssClass + "_pagerStyle";
			grid.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
			grid.HeaderStyle.CssClass = CssClass + "_headerStyle";
			grid.EditRowStyle.CssClass = CssClass + "_editRowStyle";
			grid.AlternatingRowStyle.CssClass = CssClass + "_alternatingRowStyle";
			grid.ShowHeader = true;
			grid.ShowFooter = true;

			grid.AllowPaging = false;
			grid.AutoGenerateColumns = false;

			Eurona.Common.Controls.CartProductQuantityItemTemplate template = new Eurona.Common.Controls.CartProductQuantityItemTemplate();
			template.OnRefresh += (id, quantity) =>
			{
				if (quantity != 0)
				{
					CartProductEntity cartProduct = Storage<CartProductEntity>.ReadFirst(new CartProductEntity.ReadById { CartProductId = id });
					ProductEntity p = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = cartProduct.ProductId });
					if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(p.Code, p, quantity, false, this, false))
						return;
					Eurona.Common.EuronaUserMarzeInfo umi = EuronaCartHelper.UpdateCartProduct(this.Page, cartProduct.CartId, cartProduct.ProductId, quantity);
					this.cartEntity = null;
					if (OnCartItemsChanged != null) OnCartItemsChanged(this, null);
				}

				GridViewDataBind(true);
			};
			grid.Columns.Add(new HyperLinkField
			{
				DataTextField = "ProductCode",
				HeaderText = "Kód",
				SortExpression = "ProductCode",
			});
			grid.Columns.Add(new HyperLinkField
			{
				DataTextField = "",
				HeaderText = "",
			});
			grid.Columns.Add(new HyperLinkField
			{
				DataTextField = "ProductName",
				HeaderText = SHP.Resources.Controls.CartControl_ColumnName,
				SortExpression = "ProductName",
			});
			grid.Columns.Add(new TemplateField
			{
				ItemTemplate = template,
				HeaderText = SHP.Resources.Controls.CartControl_ColumnQuantity,
				SortExpression = "Quantity",
			});
			grid.Columns.Add(new BoundField
			{
				DataField = "BodyCelkem",
				HeaderText = "Body",
				SortExpression = "BodyCelkem",
			});
			grid.Columns.Add(new PriceField
			{
				DataField = "KatalogPriceWVATTotal",
				HeaderText = "Katalogová cena celkem",
				SortExpression = "KatalogPriceWVATTotal",
			});
			grid.Columns.Add(new PriceField
			{
				DataField = "PriceTotalWVAT",
				HeaderText = SHP.Resources.Controls.CartControl_ColumnPriceTotalWithVAT,
				SortExpression = "PriceTotalWVAT",
			});

			CMSButtonField btnDelete = new CMSButtonField();
			btnDelete.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
			btnDelete.ImageUrl = ConfigValue("CMS:DeleteButtonImage");
			btnDelete.ToolTip = SHP.Resources.Controls.GridView_ToolTip_DeleteItem;
			btnDelete.ButtonType = ButtonType.Image;
			btnDelete.OnClientClick = string.Format("javascript:return confirm('{0}')", SHP.Resources.Controls.DeleteItemQuestion);
			btnDelete.CommandName = DELETE_COMMAND;
			grid.Columns.Add(btnDelete);

			grid.RowCommand += OnRowCommand;

			return grid;
		}

		void grid_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				int columnNameIndex = 0;
				HyperLink hl = (e.Row.Cells[columnNameIndex].Controls[0] as HyperLink);
				CartProductEntity cp = (e.Row.DataItem as CartProductEntity);

				if (cp.CerpatBonusoveKredity == true)
				{
					HyperLink hlBK = (e.Row.Cells[1].Controls[0] as HyperLink);
					hlBK.NavigateUrl = Page.ResolveUrl("~/user/advisor/bonusoveKredity.aspx");
					hlBK.ToolTip = "Čerpání bonusových kreditů";
					Image img = new Image();
					img.ImageUrl = Page.ResolveUrl("~/images/gift.png");
					hlBK.Controls.Add(img);
				}

				if (!string.IsNullOrEmpty(cp.Alias))
					hl.NavigateUrl = Page.ResolveUrl(cp.Alias + "?&" + base.BuildReturnUrlQueryParam());
				return;
			}

			if (e.Row.RowType == DataControlRowType.Footer)
			{
				//Cena celkom/Cena celkom s DPH
				string price = string.Empty;
				price = string.Format("Body celkem : {0}&nbsp;&nbsp;&nbsp;&nbsp;Cena celkem : {1}", this.CartEntity.BodyKatalogTotal,
				SHP.Utilities.CultureUtilities.CurrencyInfo.ToString(this.CartEntity.PriceTotalWVAT, this.Session));

				e.Row.Cells[0].Text = SHP.Resources.Controls.CartControl_ColumnPriceTotal;
				e.Row.Cells.RemoveAt(e.Row.Cells.Count - 1);
				e.Row.Cells.RemoveAt(e.Row.Cells.Count - 1);
				e.Row.Cells.RemoveAt(e.Row.Cells.Count - 1);
				int lastCellIndex = e.Row.Cells.Count - 1;
				e.Row.Cells[lastCellIndex].Text = price;
				e.Row.Cells[lastCellIndex].ColumnSpan = 4;
				e.Row.Cells[lastCellIndex].HorizontalAlign = HorizontalAlign.Right;
				e.Row.Font.Bold = true;

				return;
			}
		}

		void OnRowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName == DELETE_COMMAND) OnDeleteCommand(sender, e);
		}
		private void OnDeleteCommand(object sender, GridViewCommandEventArgs e)
		{
			int rowIndex = Convert.ToInt32(e.CommandArgument);
			int cartProductId = Convert.ToInt32((sender as GridView).DataKeys[rowIndex].Value);

			EuronaCartHelper.RemoveProductFromCart(cartProductId);

			//Recalculate Cart
			EuronaCartHelper.RecalculateCart(this.Page, this.CartEntity.Id);
			this.cartEntity = null;

			GridViewDataBind(true);
			if (OnCartItemsChanged != null) OnCartItemsChanged(this, null);
		}

		protected class GridViewEx : GridView
		{
			protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)
			{
				int numRows = base.CreateChildControls(dataSource, dataBinding);

				//no data rows created, create empty table if enabled
				if (numRows == 0 && ShowWhenEmpty)
				{
					//create table
					Table table = new Table();
					table.ID = this.ID;

					//convert the exisiting columns into an array and initialize
					DataControlField[] fields = new DataControlField[this.Columns.Count];
					this.Columns.CopyTo(fields, 0);

					if (this.ShowHeader)
					{
						//create a new header row
						GridViewRow headerRow = base.CreateRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);

						this.InitializeRow(headerRow, fields);
						table.Rows.Add(headerRow);
					}

					//create the empty row
					GridViewRow emptyRow = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);

					TableCell cell = new TableCell();
					cell.ColumnSpan = this.Columns.Count;
					cell.Width = Unit.Percentage(100);
					if (!String.IsNullOrEmpty(EmptyDataText))
						cell.Controls.Add(new LiteralControl(EmptyDataText));

					if (this.EmptyDataTemplate != null)
						EmptyDataTemplate.InstantiateIn(cell);

					emptyRow.Cells.Add(cell);
					table.Rows.Add(emptyRow);

					if (this.ShowFooter)
					{
						//create footer row
						GridViewRow footerRow = base.CreateRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal);

						this.InitializeRow(footerRow, fields);
						table.Rows.Add(footerRow);
					}

					this.Controls.Clear();
					this.Controls.Add(table);
				}

				return numRows;
			}

			[Bindable(BindableSupport.No)]
			public bool ShowWhenEmpty
			{
				get
				{
					if (ViewState["ShowWhenEmpty"] == null)
						ViewState["ShowWhenEmpty"] = false;

					return (bool)ViewState["ShowWhenEmpty"];
				}
				set { ViewState["ShowWhenEmpty"] = value; }
			}
		}

		/// <summary>
		/// Vytvorenie objednávky pre daného pouzivatela
		/// </summary>
		/// <param name="orderAccount"></param>
		public void CreateOrder(int orderAccountId, bool redirect, string finishUrlFormat)
		{
			this.cartEntity = null;
            if (this.CartEntity == null) return;

			//Prepocitanie kosika a nastavenie spravnej marze na kosiku.
			EuronaCartHelper.RecalculateCart(this.Page, this.CartEntity.Id);

			//Prepocet pre spravneho pouzivatela
			EuronaCartHelper.UpdateIntensaProductInCart(this.Page, this.CartEntity.Id, orderAccountId);

			//Nanovo potiahnutie kosika
			this.cartEntity = Storage<Cart>.ReadFirst(new Cart.ReadById { CartId = this.CartEntity.Id });
			//Nastavenie spravneho majitela
			this.cartEntity.AccountId = orderAccountId;

			//Vykonanie prepoctu v TVD
			//Ak sa z eurosapu vrati chyba -> objednavku nemozno vytvorit.
			bool bSuccess = false;
            int? currencyId = null;
#if !__DEBUG_VERSION_WITHOUTTVD
			string message = Eurona.Controls.CartOrderHelper.RecalculateTVDCart(this.Page, /*this.updatePanel*/null, null, this.CartEntity, out currencyId, out bSuccess);
#else
			bSuccess = true;
#endif
			if (!bSuccess) return;
			OrderEntity order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadByCart { CartId = this.CartEntity.Id });
			if (order != null)
			{
				//Update Order
				order.OrderDate = Eurona.Common.Application.CurrentOrderDate;
				order.AccountId = orderAccountId;
				order.ShipmentCode = this.cartEntity.ShipmentCode;
				order.PaymentCode = this.cartEntity.PaymentCode;
				order.DeliveryAddressId = this.cartEntity.DeliveryAddressId;
				order.InvoiceAddressId = this.cartEntity.InvoiceAddressId;
				order.Price = this.cartEntity.PriceTotal.Value;
				order.PriceWVAT = this.cartEntity.PriceTotalWVAT.Value;
				order = Storage<OrderEntity>.Update(order);

				if (string.IsNullOrEmpty(finishUrlFormat))
					return;
				
				if (!this.cartEntity.Closed.HasValue)
				{
					//Close Cart
					this.cartEntity.Closed = DateTime.Now;
					this.cartEntity.AccountId = orderAccountId;
					this.cartEntity.SessionId = null;
					this.cartEntity = Storage<CartEntity>.Update(this.cartEntity);
				}

				Response.Redirect(Page.ResolveUrl(string.Format(finishUrlFormat, order.Id)));
				return;
			}
			try
			{
				#region Before Proccesing Event
				if (OnProccessOrderBeforeSave != null)
				{
					string result = string.Empty;
					if (!OnProccessOrderBeforeSave(out result))
					{
						this.Controls.Add(new LiteralControl(result));
						return;
					}
				}
				#endregion

                //Nastavenie dopravneho
                CartOrderHelper.RecalculateDopravne(this.cartEntity, this.cartEntity.ShipmentCode);

				//Create Order
				order = new OrderEntity();
				order.OrderDate = Eurona.Common.Application.CurrentOrderDate;
				order.CartId = this.cartEntity.Id;
				order.AccountId = orderAccountId;
				order.ShipmentCode = this.cartEntity.ShipmentCode;
				order.PaymentCode = this.cartEntity.PaymentCode;
				order.DeliveryAddressId = this.cartEntity.DeliveryAddressId;
				order.InvoiceAddressId = this.cartEntity.InvoiceAddressId;
				order.Price = this.cartEntity.PriceTotal.Value;
				order.PriceWVAT = this.cartEntity.PriceTotalWVAT.Value;
				order.CreatedByAccountId = Security.Account.Id;
                if (currencyId.HasValue)
                    order.CurrencyId = currencyId.Value;

				order = Storage<OrderEntity>.Create(order);

				Organization org = Storage<Organization>.ReadFirst(new Organization.ReadByAccountId { AccountId = order.AccountId });
				order.DeliveryAddress.City = org.CorrespondenceAddress.City;
				order.DeliveryAddress.Email = org.ContactEmail;
				order.DeliveryAddress.State = org.CorrespondenceAddress.State;
				order.DeliveryAddress.Street = org.CorrespondenceAddress.Street;
				order.DeliveryAddress.Zip = org.CorrespondenceAddress.Zip;
				order.DeliveryAddress.Phone = string.IsNullOrEmpty(org.ContactPhone) ? org.ContactMobile : org.ContactPhone;
				order.DeliveryAddress.Organization = org.Name;

				string[] name = org.Name.Split(' ');
				if (name.Length >= 2)
				{
					order.DeliveryAddress.FirstName = name[0];
					order.DeliveryAddress.LastName = name[1];
				}
				Storage<SHP.Entities.Address>.Update(order.DeliveryAddress);

				//Naviazanie objednavok na pridruzenie
				int associationCount = 0;
				List<OrderEntity> ordersToAssociate = Storage<OrderEntity>.Read(new OrderEntity.ReadByFilter { AssociationAccountId = order.AccountId, AssociationRequestStatus = (int)OrderEntity.AssociationStatus.Accepted });
				foreach (OrderEntity orderToAssociate in ordersToAssociate)
				{
					if (orderToAssociate.ParentId.HasValue) continue;
					orderToAssociate.ParentId = order.Id;
					Storage<OrderEntity>.Update(orderToAssociate);
					associationCount++;
				}

				//Close Cart
				//Kosik sa zavrie az po potbrdeni objednavky
				//this.cartEntity.Closed = DateTime.Now;
				this.cartEntity.AccountId = orderAccountId;
				this.cartEntity.SessionId = null;
				this.cartEntity = Storage<CartEntity>.Update(this.cartEntity);

				#region After Proccesing Event
				if (OnProccessOrderAfterSave != null)
				{
					string result = string.Empty;
					string invoiceUrl = string.Empty;
					if (!OnProccessOrderAfterSave(order.Id, out invoiceUrl, out result))
					{
						//Unodo close cart !!
						Storage<OrderEntity>.Delete(order);
                        CartOrderHelper.DeleteTVDOrderWithCart(order);
						this.cartEntity.Closed = null;
						Storage<CartEntity>.Update(this.cartEntity);

						this.Controls.Add(new LiteralControl(result));
						return;
					}
					else if (!string.IsNullOrEmpty(invoiceUrl))
					{
						order.InvoiceUrl = invoiceUrl;
						order = Storage<OrderEntity>.Update(order);
					}
				}
				#endregion

				if (string.IsNullOrEmpty(finishUrlFormat))
					return;

				if (associationCount != 0)
				{
					string js = string.Format("alert('K Vaši objednávce jsou přidruženy {0} objednávky jiného poradce!');window.location.href='{1}';", associationCount, Page.ResolveUrl(string.Format(finishUrlFormat, order.Id)));
					this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "alert", js, true);
				}
				else
				{
					if (redirect) Response.Redirect(Page.ResolveUrl(string.Format(finishUrlFormat, order.Id)));
				}
			}
			finally { }
		}
	}
}