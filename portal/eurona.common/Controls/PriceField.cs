using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Eurona.Common.Controls {
    public class PriceField : BoundField {
        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex) {
            cell.Wrap = false;
            cell.Style.Add("text-align", "right");
            cell.HorizontalAlign = HorizontalAlign.Right;
            base.InitializeCell(cell, cellType, rowState, rowIndex);
        }

        protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState) {
            Label lblValue = new Label();
            lblValue.DataBinding += new EventHandler(lblValue_DataBinding);
            cell.Controls.Add(lblValue);
            //base.InitializeDataCell( cell, rowState );
        }

        void lblValue_DataBinding(object sender, EventArgs e) {
            Label control = sender as Label;
            object dataItem = DataBinder.GetDataItem(control.NamingContainer);

            string value = DataBinder.Eval(dataItem, this.DataField).ToString();
            string symbol = DataBinder.Eval(dataItem, this.CurrencySymbolDataField).ToString();
            control.Text = string.Format("{0} {1}", value, symbol);
            //throw new NotImplementedException();
        }

        //protected override string FormatDataValue( object dataValue, bool encode )
        //{
        //    //string symbol = string.Empty;

        //    //try
        //    //{
        //    //    if ( !string.IsNullOrEmpty( this.CurrencySymbolDataField ) )
        //    //    {
        //    //        if ( item.DataItem == null ) return string.Empty;
        //    //        PropertyInfo pi = item.DataItem.GetType().GetProperty( this.CurrencySymbolDataField );
        //    //        if ( pi != null ) symbol = pi.GetValue( item.DataItem, null ).ToString();
        //    //    }
        //    //}
        //    //catch
        //    //{
        //    //}
        //    //return Eurona.Common.Utilities.CultureUtilities.CurrencyInfo.ToString( Convert.ToDecimal( dataValue ), symbol );
        //    return SHP.Utilities.CultureUtilities.CurrencyInfo.ToString( Convert.ToDecimal( dataValue ), this.Control.Page.Session );
        //}

        public string CurrencySymbolDataField { get; set; }
    }

}
