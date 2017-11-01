using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderEntity = Eurona.DAL.Entities.Order;

namespace Eurona.Operator {
    public partial class OrdersPage : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
        }
        protected override void CreateChildControls() {
            base.CreateChildControls();
            this.adminOrdersControl.OnGridViewDataBinded += adminOrdersControl_OnGridViewDataBinded;
            this.adminOrdersControl.OnGridViewPageChanged += adminOrdersControl_OnGridViewPageChanged;
        }

        void adminOrdersControl_OnGridViewPageChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e) {
           this.cbSelecUnselectAll.Checked = false;
        }
        void adminOrdersControl_OnGridViewDataBinded(object sender, EventArgs e) {
            if (this.cbSelecUnselectAll.Checked) {
                this.adminOrdersControl.SelecAll(false);
            } else {
                this.adminOrdersControl.UnselecAll(false);
            }
        }

        protected void OnSelectUnselectAll(object sender, EventArgs e) {
            if (this.cbSelecUnselectAll.Checked) {
                this.adminOrdersControl.SelecAll(false);
            } else {
                this.adminOrdersControl.UnselecAll(false);
            }
        }

        protected void btnDeleteSelectedOrders_Click(object sender, EventArgs e) {
            List<OrderEntity>  orders = this.adminOrdersControl.GetSelectedOrders();
            foreach (OrderEntity order in orders) {
                //Objednavky pre pridruzenie k tejto objednavke - zrusi sa pridruzenie
                OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
                filter.ParentId = order.Id;
                List<OrderEntity> list = Storage<OrderEntity>.Read(filter);
                foreach (OrderEntity childOrder in list) {
                    childOrder.ParentId = null;
                    childOrder.AssociationAccountId = null;
                    childOrder.AssociationRequestStatus = (int)OrderEntity.AssociationStatus.None;
                    Storage<OrderEntity>.Update(childOrder);
                }
                // vymazanie danej objednavky
                Storage<OrderEntity>.Delete(order);
            }

            this.adminOrdersControl.GridViewDataBind(true);

        }
    }
}
