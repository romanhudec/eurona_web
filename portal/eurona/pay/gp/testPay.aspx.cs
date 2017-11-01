using Eurona.PAY.GP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderEntity = Eurona.DAL.Entities.Order;

namespace Eurona.pay.gp {
    public partial class testPay : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            string orderNumber = Request.QueryString["OrderNumber"];
            //http://localhost:53255/pay/gp/testPay.aspx?OrderNumber=20121201896
            OrderEntity order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadByFilter { OrderNumber = orderNumber });
            Transaction payTransaction = Transaction.CreateTransaction(order, this);
            payTransaction.MakePayment(this);
        }
    }
}