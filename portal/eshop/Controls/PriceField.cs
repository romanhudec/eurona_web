using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace SHP.Controls {
    public class PriceField : BoundField {
        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex) {
            cell.Wrap = false;
            cell.Style.Add("text-align", "right");
            cell.HorizontalAlign = HorizontalAlign.Right;
            base.InitializeCell(cell, cellType, rowState, rowIndex);
        }
        protected override string FormatDataValue(object dataValue, bool encode) {
            return Utilities.CultureUtilities.CurrencyInfo.ToString(Convert.ToDecimal(dataValue), this.Control.Page.Session);
        }
    }

    public class HyperlinkPriceField : HyperLinkField {
        protected override string FormatDataTextValue(object dataTextValue) {
            if (dataTextValue == null || dataTextValue.ToString() == string.Empty)
                return string.Empty;

            return Utilities.CultureUtilities.CurrencyInfo.ToString(Convert.ToDecimal(dataTextValue), this.Control.Page.Session);
        }
    }
}
