using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CMS.Controls {
    /// <summary>
    /// Round panel.
    /// </summary>
    public class RoundPanel : Panel {
        public RoundPanel() {
            FillHeader = true;
        }

        protected override void Render(HtmlTextWriter writer) {
            //if (!this.HasContentControls()) {
            //        Visible = false;
            //        return;
            //}

            // TODO: add custom rendering code here.
            // writer.Write("Output HTML");

            bool showHeader = !string.IsNullOrEmpty(this.Text);
            string cssHeadPostFix = showHeader && this.FillHeader ? "Head" : string.Empty;

            string widthAttr = this.Width.IsEmpty ? string.Empty : string.Format("width:{0};", this.Width);
            string heightAttr = this.Height.IsEmpty ? string.Empty : string.Format("height:{0};", this.Height);
            string divStyleAttribute = string.Empty;
            if (!string.IsNullOrEmpty(widthAttr) || !string.IsNullOrEmpty(heightAttr))
                divStyleAttribute = string.Format("style='{0}{1}'", widthAttr, heightAttr);

            writer.Write(string.Format("<div id='{0}' class='" + this.CssClass + "' {1}>", this.ClientID, divStyleAttribute));
            writer.Write("<table class='" + this.CssClass + "_main' cellspacing='0' cellpadding='0' >");

            //Top
            writer.Write("<tr>");
            writer.Write("<td class='" + this.CssClass + "_topLeft" + cssHeadPostFix + "' style='white-space:nowrap;' ></td>");

            if (!showHeader) writer.Write("<td class='" + this.CssClass + "_top' style='white-space:nowrap;' ></td>");
            else if (this.FillHeader) writer.Write("<td rowspan='2' class='" + this.CssClass + "_headFill'>" + this.Text + "</td>");
            else writer.Write("<td rowspan='2' class='" + this.CssClass + "_headNoFill'>" + this.Text + "</td>");

            writer.Write("<td class='" + this.CssClass + "_topRight" + cssHeadPostFix + "' style='white-space:nowrap;' ></td>");
            writer.Write("</tr>");

            if (showHeader) {
                string cssLeft = this.CssClass + (this.FillHeader ? "_headFillLeft" : "_headNoFillLeft");
                string cssRight = this.CssClass + (this.FillHeader ? "_headFillRight" : "_headNoFillRight");

                writer.Write("<tr>");
                writer.Write("<td class='" + cssLeft + "'></td>");

                writer.Write("<td class='" + cssRight + "'></td>");
                writer.Write("</tr>");
            }

            writer.Write("<tr>");
            writer.Write("<td class='" + this.CssClass + "_left' style='white-space:nowrap;' ></td>");

            //Content
            writer.Write("<td class='" + this.CssClass + "_content'>");
            writer.Write("<div>");
            RenderContents(writer);
            writer.Write("</div>");
            writer.Write("</td>");

            writer.Write("<td class='" + this.CssClass + "_right' style='white-space:nowrap;' ></td>");
            writer.Write("</tr>");

            //Bottom
            writer.Write("<tr>");
            writer.Write("<td class='" + this.CssClass + "_bottomLeft' style='white-space:nowrap;' ></td>");
            writer.Write("<td class='" + this.CssClass + "_bottom' style='white-space:nowrap;' ></td>");
            writer.Write("<td class='" + this.CssClass + "_bottomRight' style='white-space:nowrap;' ></td>");
            writer.Write("</tr>");
            writer.Write("</table>");

            writer.Write("</div>");
        }

        [WebBrowsable(true)]
        public string Text { get; set; }

        [WebBrowsable(true)]
        public bool FillHeader { get; set; }

        //private bool HasContentControls()
        //{
        //        if (this.IsLiteralContent())
        //                return false;

        //        foreach (Control ctrl in this.Controls) {
        //                if ((ctrl is LiteralControl))
        //                        continue;

        //                if (ctrl.Visible == true)
        //                        return true;
        //        }

        //        return false;
        //}
    }
}
