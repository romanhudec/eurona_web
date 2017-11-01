using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;

namespace Eurona.pay.test_post
{
	public partial class transaction : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			string merchantref = this.Request.Form["merchantref"];
			string merchantid = this.Request.Form["merchantid"];
			string amountcents = this.Request.Form["amount"];
			string exponent = "2";
			string currencycode = this.Request.Form["currency"];
			string password = "apjGPpj451op200muj92jvopPQm";
			string merchantvar1 = this.Request.Form["merchantvar1"];

			string url = string.Format("~/pay/cs/confirm.aspx?merchantref={0}&merchantid={1}&amountcents={2}&exponent={3}&currencycode={4}&password={5}&merchantvar1={6}",
							merchantref, merchantid, amountcents, exponent, currencycode, password, merchantvar1);
			Response.Redirect(url);

			//int orderId = 0;
			//if ( !Int32.TryParse( Request["merchantvar1"], out orderId ) ) Failed();

			//OrderEntity order = Storage<OrderEntity>.ReadFirst( new OrderEntity.ReadById { OrderId = orderId } );
			//Transaction tran = Transaction.CreateTransaction( order, this );

			//if ( tran.Merchantref != Request["merchantref"] ) Failed();
			//if ( tran.MerchantId != Request["merchantid"] ) Failed();
			//if ( tran.Amount != Request["amountcents"] ) Failed();
			//if ( Request["exponent"] != "2" /*exponent meny*/) Failed();
			//if ( tran.Currency != Request["currencycode"] ) Failed();
			//if ( Request["password"] != "apjGPpj451op200muj92jvopPQm" ) Failed();
		}
	}
}