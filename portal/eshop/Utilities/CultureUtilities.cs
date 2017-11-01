using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Globalization;
using System.Web.UI;
using System.Web.SessionState;

namespace SHP.Utilities
{
	public static class CultureUtilities
	{

		public static class CurrencyInfo
		{
			private static NumberFormatInfo nfi = null;
			private static NumberFormatInfo NumberFormatInfo
			{
				get
				{
					if (nfi != null) return nfi;

					string separator = ConfigurationManager.AppSettings["CultureInfo:CurrencyGroupSeparator"];
					if (string.IsNullOrEmpty(separator)) return null;

					nfi = new CultureInfo(CultureInfo.CurrentUICulture.LCID, false).NumberFormat;
					nfi.NumberGroupSeparator = separator;
					return nfi;
				}
			}

			public static string ToString(decimal? value, HttpSessionState session)
			{
				if (!value.HasValue)
					return string.Empty;

				object currencyRate = session == null ? null : session["SHP:Currency:Rate"];
				object currencySymbol = session == null ? null : session["SHP:Currency:Symbol"];

				string format = ConfigurationManager.AppSettings["CultureInfo:CurrencyFormatString"];
				string symbol = ConfigurationManager.AppSettings["CultureInfo:CurrencySymbol"];
				if (string.IsNullOrEmpty(format) || CurrencyInfo.NumberFormatInfo == null)
					return value.ToString();

				if (currencyRate != null && Convert.ToDecimal(currencyRate) != 1)
					value = value * Convert.ToDecimal(currencyRate);

				if (currencySymbol != null && Convert.ToString(currencySymbol).Length != 0)
					symbol = currencySymbol.ToString();

				symbol = symbol == null ? string.Empty : symbol;
				return string.Format(CurrencyInfo.NumberFormatInfo, format, value) + symbol;
			}

			public static string ToString(decimal? value, int currencyId)
			{
				if (!value.HasValue)
					return string.Empty;

				Entities.Classifiers.Currency currency = Storage<Entities.Classifiers.Currency>.ReadFirst(new Entities.Classifiers.Currency.ReadById { Id = currencyId });
				if (currency == null) return string.Empty;

				object currencyRate = currency.Rate;
				object currencySymbol = currency.Symbol;

				string format = ConfigurationManager.AppSettings["CultureInfo:CurrencyFormatString"];
				string symbol = ConfigurationManager.AppSettings["CultureInfo:CurrencySymbol"];
				if (string.IsNullOrEmpty(format) || CurrencyInfo.NumberFormatInfo == null)
					return value.ToString();

				if (currencyRate != null && Convert.ToDecimal(currencyRate) != 1)
					value = value * Convert.ToDecimal(currencyRate);

				if (currencySymbol != null && Convert.ToString(currencySymbol).Length != 0)
					symbol = currencySymbol.ToString();

				symbol = symbol == null ? string.Empty : symbol;
				return string.Format(CurrencyInfo.NumberFormatInfo, format, value) + symbol;
			}
		}
	}
}
