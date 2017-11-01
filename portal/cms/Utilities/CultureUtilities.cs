using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Globalization;

namespace CMS.Utilities
{
		public static class CultureUtilities
		{
				public static string TwoLetter
				{
						get
						{
								return Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
						}
				}

				public static class CurrencyInfo
				{
						private static NumberFormatInfo nfi = null;
						private static NumberFormatInfo NumberFormatInfo
						{
								get
								{
										if ( nfi != null ) return nfi;

										string separator = ConfigurationManager.AppSettings["CultureInfo:CurrencyGroupSeparator"];
										if ( string.IsNullOrEmpty( separator ) ) return null;

										nfi = new CultureInfo( CultureInfo.CurrentUICulture.LCID, false ).NumberFormat;
										nfi.NumberGroupSeparator = separator;
										return nfi;
								}
						}

						public static string ToString( decimal? value )
						{
								if ( !value.HasValue )
										return string.Empty;

								string format = ConfigurationManager.AppSettings["CultureInfo:CurrencyFormatString"];
								string symbol = ConfigurationManager.AppSettings["CultureInfo:CurrencySymbol"];
								if ( string.IsNullOrEmpty( format ) || CurrencyInfo.NumberFormatInfo == null )
										return value.ToString();

								if ( symbol == null ) symbol = string.Empty;

								return string.Format( CurrencyInfo.NumberFormatInfo, format, value ) + symbol;
						}
				}
		}
}
