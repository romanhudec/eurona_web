using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.PAY.CS
{
	public partial class OkPage : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			string stransactionId = Request.Form["Ref"];
			Response.Write(Resources.EShopStrings.Pay_TransactionWasSuccessfully);
			Response.End();
		}
	}
}