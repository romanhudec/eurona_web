using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Controls;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using CategoryEntity = SHP.Entities.Category;

namespace SHP.Controls.Category
{
		public class CategoryControl : CmsControl
		{
				private CategoryPathControl categoryPathControl = null;

				public CategoryControl()
				{
						this.categoryPathControl = new CategoryPathControl();
				}

				public int CategoryId
				{
						get
						{
								object o = ViewState["CategoryId"];
								return o != null ? (int)Convert.ToInt32( o ) : 0;
						}
						set { ViewState["CategoryId"] = value; }
				}

				public string Title
				{
						get
						{
								this.categoryPathControl.CategoryId = this.CategoryId;
								return this.categoryPathControl.Path;
						}
				}
		}
}
