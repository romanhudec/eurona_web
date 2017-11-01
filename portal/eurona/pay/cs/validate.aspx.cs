using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderEntity = Eurona.DAL.Entities.Order;

namespace Eurona.PAY.CS
{
	public partial class ValidatePage : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			//ProxyPay provede kontrolu dat, poté odešle tato data na URL specifikované obchodním
			//partnerem (Validation Post). Obchodní partner zkontroluje, zda data souhlasí s původně
			//odeslanými údaji. Pokud ano, odpoví HTML řetězcem:
			//<html><head></head><body>[OK]</body></html>

			int orderId = 0;
			if (!Int32.TryParse(Request["merchantvar1"], out orderId)) Failed(0);

			OrderEntity order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = orderId });
			Transaction tran = Transaction.CreateTransaction(order, this);

            if (tran.Merchantref != Request["merchantref"]) Failed(orderId);
            if (tran.MerchantId != Request["merchantid"]) Failed(orderId);
            if (tran.Amount != Request["amountcents"]) Failed(orderId);
			if (Request["exponent"] != "2" /*exponent meny*/) Failed(orderId);
			if (tran.Currency != Request["currencycode"]) Failed(orderId);
			if (Request["password"] != "apjGPpj451op200muj92jvopPQm") Failed(orderId);

			// Kontrola OK
            Success(orderId);
		}

		private void Success(int orderId)
		{
            Session["payment_order"] = orderId;
#if __LOCAL_BANK_PAY
			Response.Redirect(ResolveUrl("~/pay/cs/ok.aspx"));
#else
						Response.Write( "<html><head></head><body>[OK]</body></html>" );
						Response.End();
#endif
		}

		private void Failed(int orderId)
		{
            Session["payment_order"] = orderId;
#if __LOCAL_BANK_PAY
			Response.Redirect(ResolveUrl("~/pay/cs/nok.aspx"));
#else
						Response.Write( "<html><head></head><body>[FAILED]</body></html>" );
						Response.End();
#endif
		}
	}
}