using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SHP.Controls {
    public class GridPriceColumn : GridBoundColumn {
        //public String CurrencySymbolField {
        //    get; set;
        //}
        protected override string FormatDataValue(object dataValue, GridItem item) {
            //if( string.IsNullOrEmpty(this.CurrencySymbolField )){
            //    return Utilities.CultureUtilities.CurrencyInfo.ToString(Convert.ToDecimal(dataValue), item.Page.Session);
            //}else{
            //    try {
            //        object symbolObj = item.DataItem.GetType().GetProperty(this.CurrencySymbolField).GetValue(item.DataItem, null);
            //        if (symbolObj != null) string.Format("{0}{1}", Convert.ToDecimal(dataValue), symbolObj.ToString());
            //    }catch(Exception e){
            //    }
            //    return Utilities.CultureUtilities.CurrencyInfo.ToString(Convert.ToDecimal(dataValue), item.Page.Session);
            //}

            return Utilities.CultureUtilities.CurrencyInfo.ToString(Convert.ToDecimal(dataValue), item.Page.Session);
            
        }
    }
    public class GridHyperlinkPriceColumn : GridHyperLinkColumn {
        protected override string FormatDataTextValue(object dataTextValue) {
            if (dataTextValue == null || dataTextValue.ToString() == string.Empty)
                return string.Empty;

            return Utilities.CultureUtilities.CurrencyInfo.ToString(Convert.ToDecimal(dataTextValue), this.Owner.Page.Session);
        }
    }


}
