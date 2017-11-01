using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderEntity = Eurona.DAL.Entities.Order;
namespace Eurona.PAY.CS {
    public partial class NOkPage : WebPage {
        int orderId = 0;
        //https://www.euronabycerny.com:443/pay/cs/nok.aspx?Ref=20170800064&Status=0&merchantvar1=1390190 
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["payment_order"] == null) orderId = 0;
            else orderId = Convert.ToInt32(Session["payment_order"]);

            if (orderId == 0) {
                this.btnBackToOrder.Visible = false;
                string stransactionId = Request.Form["Ref"];
                if( string.IsNullOrEmpty(stransactionId )){
                    stransactionId = Request.QueryString["Ref"];
                }
                OrderEntity order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadByFilter { OrderNumber = stransactionId });
                if (order != null) {
                    orderId = order.Id;
                    this.btnBackToOrder.Visible = true;
                } else {
                    Response.Write(Resources.EShopStrings.Pay_TransactionWasNotSuccessfully);
                    Response.End();
                }
            } else {
                Session["payment_order"] = null; ;
            }
        }

        protected void btnBackToOrder_Click(object sender, EventArgs e) {
            
            //Po neuspesnej platbe je nutne nechat objednavku v stave spracovava se, lebo uz je zaevidovana v eurosape!!!! 
            /*
            OrderEntity order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = orderId });
            if (order != null) {
                //Natavim priznak na caka na spracovanie aby bolo mozne objednavku znova uhradit kartou!!!
                order.OrderStatusCode = ((int)OrderEntity.OrderStatus.WaitingForProccess).ToString();
                Storage<OrderEntity>.Update(order);

            }
            */
            Response.Redirect(string.Format("~/user/advisor/newOrder.aspx?id={0}", orderId));
        }
    }
}