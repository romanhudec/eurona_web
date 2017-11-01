using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Resources;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;

[assembly: WebResource( "CMS.Controls.ASPxDatePicker.js", "application/x-javascript" )]
namespace CMS.Controls
{
		/// <summary>
		/// Autor: Ing. Roman Hudec
		/// </summary>
		[ToolboxData( "<{0}:ASPxDatePicker runat=server></{0}:ASPxDatePicker>" )]
		[ControlValueProperty( "Value" ), ValidationProperty( "Value" ), ParseChildren( true, "Value" )]
		public class ASPxDatePicker: WebControl
		{
				#region Public properties
				public string Text
				{
						get { return this.Value == null ? string.Empty : ( (DateTime)this.Value ).ToShortDateString(); }
						set { this.Value = value; }
				}


				/// <summary>
				/// Vráti hodnotu ako object;
				/// </summary>
				[Bindable( true )]
				[Localizable( true )]
				public object Value
				{
						get
						{
								if ( Page.IsPostBack )
								{
										//Nacitanie ViewState z form. (Vykonava sa v tedy ak este nie je ViewState inicializovany)
										if ( this.Page.Request.Form.Count != 0 )
										{
												object tmpValue = this.TryParseDateTime( this.Page.Request.Form[this.ClientID] );
												if ( tmpValue != null ) return tmpValue;
										}
								}
								return this.ViewState["Value"];
						}
						set
						{
								if ( value == null ) this.ViewState["Value"] = value;
								else this.ViewState["Value"] = this.TryParseDateTime( value );
						}
				}

				/// <summary>
				/// Vrati True ak nie je dátum vyplnený.
				/// </summary>
				public bool IsNullDate
				{
						get { return ( this.Value == null ); }
				}

				private DateTime? TryParseDateTime( object dateTime )
				{
						if ( dateTime == null ) return null;
						DateTime? outDate = null;
						try
						{
								outDate = Convert.ToDateTime( dateTime );
								
								//Kontrola datumu
								if ( outDate.HasValue && ( outDate.Value.Year <= 1754 || outDate.Value.Year >= 9999 ) )
										outDate = null;
						}
						catch
						{
								outDate = null;
						}

						return outDate;
				}

				#endregion

				public ASPxDatePicker()
				{
						this.Width = Unit.Pixel( 100 );
				}

				#region Overridet methods
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						ClientScriptManager cs = this.Page.ClientScript;
						Type cstype = this.GetType();
						string urlInclude = cs.GetWebResourceUrl( cstype, "CMS.Controls.ASPxDatePicker.js" );
						cs.RegisterClientScriptInclude( cstype, "ASPxDatePickerJs", urlInclude );
				}

				//protected override void OnInit( EventArgs e )
				//{
				//    //Nacitam hodnotu z VIEWSTATE pre input.
				//    if ( this.Page.Request.Form.Count != 0 )
				//        this.tmpValue = this.Page.Request.Form[this.ClientID];

				//    base.OnInit( e );

				//}

				//protected override void OnLoad( EventArgs e )
				//{
				//    //Nacitam hodnotu z VIEWSTATE pre input.
				//    if ( this.Page.Request.Form.Count != 0 )
				//        this.tmpValue = this.Page.Request.Form[this.ClientID];

				//    base.OnLoad( e );

				//}

				/// <summary>
				/// Vyrenderovanie samotného controlsu.
				/// </summary>
				protected override void Render( HtmlTextWriter writer )
				{
						if ( !this.Visible )
								return;

						ClientScriptManager cs = this.Page.ClientScript;
						Type cstype = this.Page.GetType();

						if ( !this.Enabled )
						{
								writer.Write( string.Format( "<input type='text' class='dateValue' name='{0}' id='{0}' style='width: {1};' value='{2}' title='{3}' disabled >",
										this.ClientID, this.Width, this.Text, this.ToolTip ) );
								return;
						}

						if ( !cs.IsStartupScriptRegistered( cstype, "ASPxDatePicker" ) )
						{
								System.Text.StringBuilder sTemp = new StringBuilder( 512 );
								sTemp.AppendLine( "<div id='theDatePicker' name='theDatePicker' class='ASPxDatePicker' style='position:absolute;display:none;' ></div>" );
								sTemp.AppendLine( "<div id='theMonthPicker' name='theMonthPicker' class='ASPxDatePicker' style='position:absolute;display:none;' ></div>" );
								sTemp.AppendLine( "<div id='theYearPicker' name='theYearPicker' class='ASPxDatePicker' style='position:absolute;display:none;' ></div>" );
								sTemp.AppendLine( "<div id='theMoreYearsPicker' name='theMoreYearsPicker' class='ASPxDatePicker' style='position:absolute;display:none;' ></div>" );
								sTemp.AppendLine( "<script language='JavaScript'>var oDatePicker = new DatePicker ('theDatePicker', 'theMonthPicker', 'theYearPicker', 'theMoreYearsPicker' );</script>" );
								cs.RegisterStartupScript( cstype, "ASPxDatePicker", sTemp.ToString() );
						}

						//Date input
						writer.Write( string.Format( "<input type='text' class='dateValue' name='{0}' id='{0}' maxlength='12' style='width:{1};' value='{2}' title='{3}' onChange=\"CheckDatePickerDate('{0}')\">",
								this.ClientID, this.Width, this.Text, this.ToolTip ) );

						//Date calendar image.
						string calendarButtonId = string.Format( "calendarBtn_{0}", this.ID );
						string imagesPath = ConfigurationManager.AppSettings["CMS:ASPxDatePicker:ImagePath"];
						if ( imagesPath.StartsWith( "~" ) ) imagesPath = Page.ResolveUrl( imagesPath );
						writer.Write( string.Format( "<img name='{0}' id='{0}' src='{1}calendar.png' class='calendarImage' onclick=\"DatePickerShow( '{2}', '{0}' , '{4}', '{5}' ,'{1}',{3});\" title='' >",
								calendarButtonId, imagesPath, this.ClientID, GetLocalizedLabels(),
								System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern,
								System.Globalization.DateTimeFormatInfo.CurrentInfo.DateSeparator ) );
				}
				#endregion

				/// <summary>
				/// Vráti lokalizované názvy ako vstup do js funkcie.
				/// </summary>
				private string GetLocalizedLabels()
				{
						System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;

						#region Nazvy dni v tyzdni
						string[] dayNames = ci.DateTimeFormat.ShortestDayNames;
						StringBuilder sbDays = new StringBuilder( 128 );
						for ( int i = 1; i < dayNames.Length; i++ )
						{
								string separator = sbDays.Length == 0 ? string.Empty : "-";
								sbDays.AppendFormat( "{0}{1}", separator, dayNames[i] );
						}
						sbDays.AppendFormat( "-{0}", dayNames[0] );
						#endregion

						#region Nazvy mesiacov
						string[] monthNames = ci.DateTimeFormat.MonthNames;
						StringBuilder sbMonths = new StringBuilder( 128 );
						foreach ( string month in monthNames )
						{
								string separator = sbMonths.Length == 0 ? string.Empty : "-";
								sbMonths.AppendFormat( "{0}{1}", separator, month );
						}
						#endregion

						System.Text.StringBuilder labels = new StringBuilder( 512 );
						string transDest = string.Empty;

						labels.AppendFormat( "'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'",
											sbMonths.ToString(), Resources.ASPxDatePicker.Close, sbDays.ToString(), Resources.ASPxDatePicker.Month,
											Resources.ASPxDatePicker.PreviousMont, Resources.ASPxDatePicker.NextMont, Resources.ASPxDatePicker.Year,
											Resources.ASPxDatePicker.Today );

						return labels.ToString();
				}

		}
}
