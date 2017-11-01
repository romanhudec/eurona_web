﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona
{
		public partial class SearchResultPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( Request["keywords"] ) )
								this.searchEngineResultControl.SearchKeywords = CMS.Utilities.StringUtilities.RemoveDiacritics( Request["keywords"] );

				}
		}
}
