using Eurona.PAY.CSOB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderEntity = Eurona.DAL.Entities.Order;

namespace Eurona.pay.csob {
    public partial class testPay : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            string orderNumber = Request.QueryString["OrderNumber"];
            if (string.IsNullOrEmpty(orderNumber)) {
                orderNumber = "20181000004";
            }
            OrderEntity order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadByFilter { OrderNumber = orderNumber });
            Transaction payTransaction = Transaction.CreateTransaction(order, this);
            PaymentInitResponse paymentInitResponse = payTransaction.InitPayment(this);
            Response.Write(paymentInitResponse);
        }
    }
}