using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Admin.Controls;
using CMS.Entities.Classifiers;

namespace Eurona.Admin.Classifier
{
		public partial class SupportedLocalePage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						IconClassifierControl<SupportedLocale> ctrl = new IconClassifierControl<SupportedLocale>();
						if ( !string.IsNullOrEmpty( Request["Id"] ) )
								ctrl.Id = Convert.ToInt32( Request["Id"] );
						
						this.divControls.Controls.Add( ctrl );
				}
		}
}
