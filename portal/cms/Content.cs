using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS
{
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:Content runat=server></{0}:Content>")]
	public class Content : WebControl
	{
		private DropDownList ddl;
		private Button btn;

		//[Bindable(true)]
		//[Category("Appearance")]
		//[DefaultValue("")]
		//[Localizable(true)]
		//public string Text
		//{
		//        get
		//        {
		//                String s = (String)ViewState["Text"];
		//                return ((s == null) ? String.Empty : s);
		//        }
		//        set
		//        {
		//                ViewState["Text"] = value;
		//        }
		//}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string Text { get; set; }

		protected override void CreateChildControls()
		{
			ddl = new DropDownList();
			ddl.Items.Add(new ListItem { Text = "a", Value = "1" });
			ddl.Items.Add(new ListItem { Text = "b", Value = "2" });
			ddl.Items.Add(new ListItem { Text = "c", Value = "3" });
			this.Controls.Add(ddl);

			btn = new Button();
			btn.Text = "Hej";
			btn.Click += (s1, e1) => btn.Text = ddl.SelectedItem.Text;
			this.Controls.Add(btn);

			base.CreateChildControls();
		}

		/*
		protected override void RenderContents(HtmlTextWriter output)
		{
			output.Write(Text);
		}
		*/
	}
}
