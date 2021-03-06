﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Admin.Controls;
using CMS.Entities.Classifiers;

namespace Eurona.Admin.Classifier
{
		public partial class UrlAliasPrefixPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						ClassifierControl<UrlAliasPrefix> ctrl = new ClassifierControl<UrlAliasPrefix>();
						if ( !string.IsNullOrEmpty( Request["Id"] ) )
								ctrl.Id = Convert.ToInt32( Request["Id"] );
						
						this.divControls.Controls.Add( ctrl );
				}
		}
}
