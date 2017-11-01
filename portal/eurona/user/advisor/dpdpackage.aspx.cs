using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Eurona.User.Advisor
{
		public partial class DPDPackage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( Request["typ"] == "0" ) lblTitle.Text = "DPD Sledování zásilky";
						else lblTitle.Text = "DPD Online čas doručení";
				}
				protected void btnPresmeruj_Click( object sender, EventArgs e )
				{
						if ( Request["typ"] == "0" )
						{
								string url = "http://extranet.dpd.de/cgi-bin/delistrack?typ=1&lang=cz&pknr=";
								StringBuilder sb = new StringBuilder();
								sb.Append( "<script language=javascript>\n" );
								sb.AppendFormat( "window.open('{0}{1}','_blank')", url, txtBalikCislo.Text );
								sb.Append( "</script>\n" );
								ClientScript.RegisterClientScriptBlock( this.Page.GetType(), "OpenInNew", sb.ToString() );
						}
						else
						{
								string url = "http://public.dpd.cz/delivery/index.aspx?parcel=";
								StringBuilder sb = new StringBuilder();
								sb.Append( "<script language=javascript>\n" );
								sb.AppendFormat( "window.open('{0}{1}','_blank')", url, txtBalikCislo.Text );
								sb.Append( "</script>\n" );
								ClientScript.RegisterClientScriptBlock( this.Page.GetType(), "OpenInNew", sb.ToString() );
						}
				}
		}
}