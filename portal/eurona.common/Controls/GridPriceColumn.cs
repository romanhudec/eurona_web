using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Reflection;
using System.ComponentModel;

namespace Eurona.Common.Controls
{
		public class GridPriceColumn: GridBoundColumn
		{
				protected override string FormatDataValue( object dataValue, GridItem item )
				{
						string symbol = string.Empty;

						if ( dataValue == null || dataValue == DBNull.Value )
								return this.EmptyDataText;

						try
						{
								if ( !string.IsNullOrEmpty( this.CurrencySymbolDataField ) )
								{
										if ( item.DataItem == null ) return string.Empty;
										PropertyInfo pi = item.DataItem.GetType().GetProperty( this.CurrencySymbolDataField );
										if ( pi != null ) symbol = pi.GetValue( item.DataItem, null ).ToString();
								}
						}
						catch
						{
						}
						return Eurona.Common.Utilities.CultureUtilities.CurrencyInfo.ToString( Convert.ToDecimal( dataValue ), symbol );
				}

				public string CurrencySymbolDataField { get; set; }
		}
}
