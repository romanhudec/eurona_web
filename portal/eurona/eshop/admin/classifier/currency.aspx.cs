using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SHP.Controls.Classifiers;
using SHP.Entities.Classifiers;

namespace Eurona.EShop.Admin.Classifier
{
		public partial class CurrencyPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						CurrencyControl ctrl = new CurrencyControl();
						if ( !string.IsNullOrEmpty( Request["Id"] ) )
								ctrl.Id = Convert.ToInt32( Request["Id"] );

						this.divControls.Controls.Add( ctrl );
				}
		}
}
