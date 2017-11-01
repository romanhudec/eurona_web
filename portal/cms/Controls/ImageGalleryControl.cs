using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

[assembly: WebResource("CMS.Controls.ImageGallery.lightbox.effects.js", "application/x-javascript")]
[assembly: WebResource("CMS.Controls.ImageGallery.lightbox.lightbox.js", "application/x-javascript")]
[assembly: WebResource("CMS.Controls.ImageGallery.lightbox.prototype.js", "application/x-javascript")]
[assembly: WebResource("CMS.Controls.ImageGallery.lightbox.scriptaculous.js?load=effects", "application/x-javascript")]
[assembly: WebResource("CMS.Controls.ImageGallery.lightbox.lightbox.css", "text/css")]
namespace CMS.Controls
{
	public class ImageGalleryControl : UserControl
	{
		/// <summary>
		/// Mod zobrazenia informacii galÈrie obr·zkov.
		/// </summary>
		public enum DisplayMode
		{
			Display,
			Edit
		}

		public delegate void ImageDeleteEventHandler(object sender, object id);
		public event ImageDeleteEventHandler ImageDelete;

		public delegate void ImageShiftEventHandler(object sender, object id);
		public event ImageShiftEventHandler ImageShiftRight;
		public event ImageShiftEventHandler ImageShiftLeft;

		public const int IMAGE_WIDTH = 100;
		public const int IMAGE_HEIGHT = 75;
		private const int IMAGES_IN_ROW = 3;

		private List<AditionalProperty> aditionalProperties = null;
		private GridView gridView = null;
		private DisplayMode mode = DisplayMode.Display;

		private Label lblWarning = null;

		private bool useMaxWidthAttribute = true;
		private bool useMaxHeightAttribute = true;

		#region Constucrors

		public ImageGalleryControl()
			: this(DisplayMode.Display)
		{
		}

		public ImageGalleryControl(DisplayMode mode)
		{
			this.mode = mode;
			this.ImageAttributes = new List<ImageAttribute>();
			this.HorizontalAlign = HorizontalAlign.Center;

			this.EnableViewState = this.Mode == DisplayMode.Edit;
		}

		#endregion

		#region Public properties
		public bool UseMaxWidthAttribute
		{
			get { return this.useMaxWidthAttribute; }
			set { this.useMaxWidthAttribute = value; }
		}
		public bool UseMaxHeightAttribute
		{
			get { return this.useMaxHeightAttribute; }
			set { this.useMaxHeightAttribute = value; }
		}
		public DisplayMode Mode
		{
			get { return this.mode; }
			set { this.mode = value; }
		}

		public HorizontalAlign HorizontalAlign { get; set; }

		public string CssClass { get; set; }
		public List<ImageAttribute> ImageAttributes { get; set; }

		/// <summary>
		/// DodatoËnÈ vlastnosti, torÈ sa maj˙ zobraziù pod obr·zkom.
		/// </summary>
		public List<AditionalProperty> AditionalProperties
		{
			get
			{
				if (this.aditionalProperties == null)
					this.aditionalProperties = new List<AditionalProperty>();
				return this.aditionalProperties;
			}
			set { this.aditionalProperties = value; }
		}

		public DataTable DataSource { get; set; }

		public string IdFieldName
		{
			get
			{
				object o = ViewState["IdFieldName"];
				return o != null ? o.ToString() : string.Empty;
			}
			set { ViewState["IdFieldName"] = value; }
		}

		public string ImageUrlFieldName
		{
			get
			{
				object o = ViewState["ImageUrlFieldName"];
				return o != null ? o.ToString() : string.Empty;
			}
			set { ViewState["ImageUrlFieldName"] = value; }
		}

		public string ImageUrlThumbnailFieldName
		{
			get
			{
				object o = ViewState["ImageUrlThumbnailFieldName"];
				return o != null ? o.ToString() : string.Empty;
			}
			set { ViewState["ImageUrlThumbnailFieldName"] = value; }
		}

		public string PositionFieldName
		{
			get
			{
				object o = ViewState["PositionFieldName"];
				return o != null ? o.ToString() : string.Empty;
			}
			set { ViewState["PositionFieldName"] = value; }
		}

		/// <summary>
		/// Text, ktor˝ sa vypÌöe ak obr·zok nie je k dispozÌcii.
		/// </summary>
		public string EmptyDataText
		{
			get
			{
				object o = ViewState["EmptyDataText"];
				return o != null ? o.ToString() : string.Empty;
			}
			set { ViewState["EmptyDataText"] = value; }
		}
		/// <summary>
		/// Text, ktor˝ sa vypÌöe ak obr·zok nie je k dispozÌcii.
		/// </summary>
		public string WarningText
		{
			get
			{
				object o = ViewState["warningText"];
				return o != null ? o.ToString() : string.Empty;
			}
			set { ViewState["warningText"] = value; }
		}

		public Unit Width
		{
			get
			{
				object o = ViewState["Width"];
				return o != null ? (Unit)o : Unit.Empty;
			}
			set { ViewState["Width"] = value; }
		}

		public int ImagesInRow
		{
			get
			{
				object o = ViewState["ImagesInRow"];
				return o != null ? (int)o : IMAGES_IN_ROW;
			}
			set { ViewState["ImagesInRow"] = value; }
		}

		#endregion

		#region Overrides
		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			HtmlGenericControl mainDiv = new HtmlGenericControl("div");
			mainDiv.Attributes.Add("class", this.CssClass);
			if (this.Width != Unit.Empty)
				mainDiv.Style.Add("width", this.Width.ToString());

			this.gridView = CreateGridControl(this.ImagesInRow);
			if (!string.IsNullOrEmpty(this.EmptyDataText))
				this.gridView.EmptyDataText = this.EmptyDataText;

			mainDiv.Controls.Add(this.gridView);

			//Upozornenie.
			this.lblWarning = new Label();
			this.lblWarning.ForeColor = System.Drawing.Color.Red;
			this.lblWarning.Text = this.WarningText;
			mainDiv.Controls.Add(this.lblWarning);

			this.Controls.Add(mainDiv);
			if (this.Page == null) return;

			#region Include LightBox javascripts
			if (this.Mode == DisplayMode.Display)
			{
				ClientScriptManager cs = this.Page.ClientScript;
				Type cstype = this.GetType();

				string urlInclude = cs.GetWebResourceUrl(cstype, "CMS.Controls.ImageGallery.lightbox.prototype.js");
				cs.RegisterClientScriptInclude(cstype, "lightbox.prototype.js", urlInclude);

				urlInclude = cs.GetWebResourceUrl(cstype, "CMS.Controls.ImageGallery.lightbox.effects.js");
				cs.RegisterClientScriptInclude(cstype, "lightbox.effects.js", urlInclude);

				urlInclude = cs.GetWebResourceUrl(cstype, "CMS.Controls.ImageGallery.lightbox.lightbox.js");
				cs.RegisterClientScriptInclude(cstype, "lightbox.lightbox.js", urlInclude);

				urlInclude = cs.GetWebResourceUrl(cstype, "CMS.Controls.ImageGallery.lightbox.scriptaculous.js?load=effects");
				cs.RegisterClientScriptInclude(cstype, "lightbox.scriptaculous.js", urlInclude);

				string urlCss = cs.GetWebResourceUrl(cstype, "CMS.Controls.ImageGallery.lightbox.lightbox.css");
				HtmlLink cssLink = new HtmlLink();
				cssLink.Href = urlCss;
				cssLink.Attributes.Add("rel", "stylesheet");
				cssLink.Attributes.Add("type", "text/css");
				this.Controls.Add(cssLink);
			}
			#endregion
		}

		protected override void Render(HtmlTextWriter writer)
		{
			string js = string.Format("<script language='javascript' type='text/javascript'>var fileLoadingImage = '{0}';var fileBottomNavCloseImage='{1}';</script>",
					Page.ResolveUrl("~/images/lightbox/loading.gif"), Page.ResolveUrl("~/images/lightbox/closelabel.gif"));
			ClientScriptManager cs = this.Page.ClientScript;
			Type cstype = this.Page.GetType();

			if (!cs.IsStartupScriptRegistered(cstype, "lightBoxImagesMemebrs"))
				cs.RegisterStartupScript(cstype, "lightBoxImagesMemebrs", js);

			base.Render(writer);
		}

		public override void DataBind()
		{
			//Zabezpeci, ze vsetky child controls budu vytvorene.
			EnsureChildControls();

			base.DataBind();

			this.gridView.DataSource = GetDataGridData(this.ImagesInRow);
			this.gridView.DataBind();

		}
		#endregion

		#region Private methods
		/// <summary>
		/// Vytvori DataGrid control s pozadovanymi stlpcami a 
		/// pripravenym bindingom stlpcou.
		/// </summary>
		private GridView CreateGridControl(int imagesInRow)
		{
			GridView dg = new GridView();
			dg.HorizontalAlign = this.HorizontalAlign;

			dg.GridLines = GridLines.None;
			dg.ShowHeader = false;
			dg.ShowFooter = false;
			dg.AutoGenerateColumns = false;
			dg.RowStyle.VerticalAlign = VerticalAlign.Top;

			dg.BorderStyle = BorderStyle.None;
			dg.BorderWidth = Unit.Pixel(0);

			for (int index = 0; index < imagesInRow; index++)
			{
				TemplateField tf = new TemplateField();
				ImageTemplate imageTemplate = new ImageTemplate(this.UseMaxWidthAttribute ? IMAGE_WIDTH : 0, this.UseMaxHeightAttribute ? IMAGE_HEIGHT : 0, this.Mode);
				imageTemplate.CssClass = this.CssClass;
				imageTemplate.ImageAttributes = this.ImageAttributes;

				imageTemplate.ValueField = string.Format("{0}_{1}", this.ImageUrlFieldName, index);
				imageTemplate.ValueThumbnailField = string.Format("{0}_{1}", this.ImageUrlThumbnailFieldName, index);
				imageTemplate.IDValueField = string.Format("{0}_{1}", this.IdFieldName, index);
				imageTemplate.IsFirstValueField = string.Format("IsFirst_{0}", index);
				imageTemplate.IsLastValueField = string.Format("IsLast_{0}", index);

				string valueField = string.Empty;
				//Transformacia na nazvy pre template control.
				foreach (AditionalProperty ap in this.AditionalProperties)
				{
					valueField = string.Format("{0}_{1}", ap.ValueFieldName, index);
					imageTemplate.AditionalProperties.Add(new AditionalProperty(ap.Text, valueField));
				}

				if (!string.IsNullOrEmpty(this.PositionFieldName))
				{
					valueField = string.Format("{0}_{1}", this.PositionFieldName, index);
					imageTemplate.PositionValueField = valueField;
				}

				imageTemplate.ItemDelete += (s, e) =>
				{
					if (ImageDelete != null)
						ImageDelete(s, e);
				};
				imageTemplate.ItemShiftLeft += (s, e) =>
				{
					if (ImageShiftLeft != null)
						ImageShiftLeft(s, e);
				};
				imageTemplate.ItemShiftRight += (s, e) =>
				{
					if (ImageShiftRight != null)
						ImageShiftRight(s, e);
				};
				tf.ItemTemplate = imageTemplate;

				dg.Columns.Add(tf);
			}

			return dg;
		}

		/// <summary>
		/// Vrati fifiltrovanÈ d·ta pre GridView.
		/// </summary>
		/// <returns></returns>
		private DataView GetDataGridData(int imagesInRow)
		{
			#region Create DataTable structure
			DataTable dt = new DataTable();
			for (int index = 0; index < imagesInRow; index++)
			{
				//ImageURL
				DataColumn dc1 = new DataColumn(string.Format("{0}_{1}", this.ImageUrlFieldName, index));
				dt.Columns.Add(dc1);

				//ImageUrlThumbnail
				DataColumn dc2 = new DataColumn(string.Format("{0}_{1}", this.ImageUrlThumbnailFieldName, index));
				dt.Columns.Add(dc2);

				//ID
				DataColumn dc3 = new DataColumn(string.Format("{0}_{1}", this.IdFieldName, index));
				dt.Columns.Add(dc3);

				//IsFirst
				DataColumn dc4 = new DataColumn(string.Format("IsFirst_{0}", index));
				dt.Columns.Add(dc4);

				//IsLast
				DataColumn dc5 = new DataColumn(string.Format("IsLast_{0}", index));
				dt.Columns.Add(dc5);

				//Dodatocne vlastnosti
				foreach (AditionalProperty ap in this.AditionalProperties)
				{
					DataColumn dc = new DataColumn(string.Format("{0}_{1}", ap.ValueFieldName, index));
					dt.Columns.Add(dc);
				}

				if (!string.IsNullOrEmpty(this.PositionFieldName))
				{
					DataColumn dcPoradoveCislo = new DataColumn(string.Format("{0}_{1}", this.PositionFieldName, index));
					dt.Columns.Add(dcPoradoveCislo);
				}
			}
			#endregion

			int spIndex = 1;
			int imagesProcssed = 1;
			DataRow dr = dt.NewRow();
			foreach (DataRow dtRow in this.DataSource.Rows)
			{
				dr[string.Format("{0}_{1}", this.ImageUrlFieldName, spIndex - 1)] = dtRow[this.ImageUrlFieldName];
				dr[string.Format("{0}_{1}", this.ImageUrlThumbnailFieldName, spIndex - 1)] = dtRow[this.ImageUrlThumbnailFieldName];
				dr[string.Format("{0}_{1}", this.IdFieldName, spIndex - 1)] = dtRow[this.IdFieldName];

				dr[string.Format("IsFirst_{0}", spIndex - 1)] = this.DataSource.Rows.IndexOf(dtRow) == 0;
				dr[string.Format("IsLast_{0}", spIndex - 1)] = this.DataSource.Rows.IndexOf(dtRow) == this.DataSource.Rows.Count - 1;

				//Dodatocne vlastnosti
				foreach (AditionalProperty ap in this.AditionalProperties)
					dr[string.Format("{0}_{1}", ap.ValueFieldName, spIndex - 1)] = dtRow[ap.ValueFieldName];

				//Poradie obrazku
				if (!string.IsNullOrEmpty(this.PositionFieldName))
					dr[string.Format("{0}_{1}", this.PositionFieldName, spIndex - 1)] = dtRow[this.PositionFieldName];

				if (spIndex == imagesInRow ||
					 imagesProcssed == this.DataSource.Rows.Count)
				{
					dt.Rows.Add(dr);
					dr = dt.NewRow();
					spIndex = 0;
				}

				spIndex++;
				imagesProcssed++;

			}

			return dt.DefaultView;
		}
		#endregion

		#region ImageTemplate implementation
		/// <summary>
		/// Template field na zobrazenie obrazku.
		/// </summary>
		public class ImageTemplate : ITemplate
		{
			public delegate void ItemDeleteEventHandler(object sender, object id);
			public event ItemDeleteEventHandler ItemDelete;

			public delegate void ItemShiftEventHandler(object sender, object id);
			public event ItemShiftEventHandler ItemShiftRight;
			public event ItemShiftEventHandler ItemShiftLeft;

			#region Private members

			private List<AditionalProperty> aditionalProperties = null;
			private DisplayMode mode = DisplayMode.Display;

			private int IMAGE_WIDTH = 0;
			private int IMAGE_HEIGHT = 0;
			#endregion

			public ImageTemplate(int IMAGE_WIDTH, int IMAGE_HEIGHT, DisplayMode mode)
			{
				this.IMAGE_WIDTH = IMAGE_WIDTH;
				this.IMAGE_HEIGHT = IMAGE_HEIGHT;

				this.mode = mode;
			}

			#region Public properties
			public string CssClass { get; set; }
			public List<ImageAttribute> ImageAttributes { get; set; }

			public DisplayMode Mode
			{
				get { return this.mode; }
			}

			public string IDValueField { get; set; }
			public string ValueField { get; set; }
			public string ValueThumbnailField { get; set; }
			public string PositionValueField { get; set; }

			public string IsFirstValueField { get; set; }
			public string IsLastValueField { get; set; }

			public List<AditionalProperty> AditionalProperties
			{
				get
				{
					if (this.aditionalProperties == null)
						this.aditionalProperties = new List<AditionalProperty>();
					return this.aditionalProperties;
				}
				set { this.aditionalProperties = value; }
			}
			#endregion

			/// <summary>
			/// MetÛda vytvorÌ samotn˝ control, ktor˝ zobrazuje obr·zok.
			/// </summary>
			private HtmlGenericControl CreateImageControl()
			{
				HtmlGenericControl imageControl = new HtmlGenericControl("a");

				if (this.ImageAttributes.Count != 0)
				{
					foreach (ImageAttribute attribute in this.ImageAttributes)
						imageControl.Attributes.Add(attribute.Key, attribute.Value);
				}

				Image image = new Image();
				if (this.IMAGE_WIDTH > 0)
				{
					image.Width = Unit.Pixel(this.IMAGE_WIDTH);
					image.Style.Add("max-width", this.IMAGE_WIDTH.ToString() + "px");
				}
				if (this.IMAGE_HEIGHT > 0) image.Style.Add("max-height", this.IMAGE_HEIGHT.ToString() + "px");

				image.DataBinding += new EventHandler(OnImageDataBinding);
				imageControl.Controls.Add(image);

				return imageControl;
			}

			#region ITemplate Members
			public void InstantiateIn(Control container)
			{
				container.DataBinding += new EventHandler(OnContainerDataBinding);
				HtmlGenericControl div = new HtmlGenericControl("div");
				if (!string.IsNullOrEmpty(this.CssClass))
					div.Attributes.Add("class", this.CssClass + "_item");

				TableRow row = null;
				TableCell cell = null;

				#region HEADER Poradove cislo fotky && button zmazat
				if (!string.IsNullOrEmpty(this.PositionValueField) && this.Mode == DisplayMode.Edit)
				{
					//Image & navigation
					Table tableHeader = new Table();
					if (!string.IsNullOrEmpty(this.CssClass))
						tableHeader.CssClass = this.CssClass + "_itemHeader";
					row = new TableRow();
					cell = new TableCell();

					//Poradove cislo
					cell.DataBinding += new EventHandler(OnPositionDataBinding);
					row.Cells.Add(cell);

					//Button odstranit
					cell = new TableCell();
					#region Delete button
					if (this.Mode == DisplayMode.Edit)
					{
						HtmlGenericControl img = new HtmlGenericControl("div");
						img.Attributes.Add("class", this.CssClass + "_itemDeleteLink");

						LinkButton btnDelete = new LinkButton();
						btnDelete.DataBinding += new EventHandler(OnButtonDeleteDataBinding);
						btnDelete.ToolTip = Resources.Controls.GridView_ToolTip_DeleteItem;
						btnDelete.Click += new EventHandler(OnDeleteImage);
						btnDelete.Controls.Add(img);

						cell.Controls.Add(btnDelete);
						cell.HorizontalAlign = HorizontalAlign.Right;
					}
					#endregion
					row.Cells.Add(cell);
					tableHeader.Rows.Add(row);
					div.Controls.Add(tableHeader);
				}
				#endregion

				#region IMAGE content
				if (this.AditionalProperties.Count != 0)
				{
					Table table = new Table();
					//table.Attributes.Add( "border", "1" );
					if (!string.IsNullOrEmpty(this.CssClass))
						table.CssClass = this.CssClass + "_imageItem";
					row = new TableRow();
					cell = new TableCell();

					#region Image control
					row = new TableRow();
					if (this.Mode == DisplayMode.Edit)
					{
						cell = new TableCell();
						HtmlGenericControl img = new HtmlGenericControl("div");
						img.Attributes.Add("class", this.CssClass + "_itemShiftLeft");

						LinkButton btnShiftLeft = new LinkButton();
						btnShiftLeft.DataBinding += new EventHandler(OnButtonShiftLeftDataBinding);
						btnShiftLeft.Click += new EventHandler(OnShiftImageLeft);
						btnShiftLeft.Controls.Add(img);

						cell.Controls.Add(btnShiftLeft);
						cell.HorizontalAlign = HorizontalAlign.Right;
						cell.VerticalAlign = VerticalAlign.Top;
						row.Cells.Add(cell);
					}

					cell = new TableCell();
					Control imageControl = CreateImageControl();
					cell.ColumnSpan = 2;
					cell.HorizontalAlign = HorizontalAlign.Center;
					cell.Controls.Add(imageControl);
					if (!string.IsNullOrEmpty(this.CssClass))
						cell.CssClass = this.CssClass + "_imageItemImage";
					row.Cells.Add(cell);

					if (this.Mode == DisplayMode.Edit)
					{
						cell = new TableCell();
						HtmlGenericControl img = new HtmlGenericControl("div");
						img.Attributes.Add("class", this.CssClass + "_itemShiftRight");

						LinkButton btnShiftRight = new LinkButton();
						btnShiftRight.DataBinding += new EventHandler(OnButtonShiftRightDataBinding);
						btnShiftRight.Click += new EventHandler(OnShiftImageRight);
						btnShiftRight.Controls.Add(img);

						cell.Controls.Add(btnShiftRight);
						cell.HorizontalAlign = HorizontalAlign.Right;
						cell.VerticalAlign = VerticalAlign.Top;
						row.Cells.Add(cell);
					}
					table.Rows.Add(row);
					#endregion

					#region Aditionals properties
					row = new TableRow();
					HtmlGenericControl ctrl = new HtmlGenericControl("div");
					ctrl.DataBinding += new EventHandler(OnAditionalPropertiesDataBinding);
					cell = new TableCell();
					if (!string.IsNullOrEmpty(this.CssClass))
						cell.CssClass = this.CssClass + "_imageItemAditionalProperties";
					cell.VerticalAlign = VerticalAlign.Top;
					cell.HorizontalAlign = HorizontalAlign.Center;
					cell.ColumnSpan = this.Mode == DisplayMode.Edit ? 4 : 2;
					cell.Controls.Add(ctrl);
					row.Cells.Add(cell);
					table.Rows.Add(row);
					#endregion

					div.Controls.Add(table);
				}
				else
				{
					#region Image control
					HtmlGenericControl imageControl = CreateImageControl();
					if (!string.IsNullOrEmpty(this.CssClass))
						imageControl.Attributes.Add("class", this.CssClass + "_imageItemImage");
					div.Controls.Add(imageControl);
					#endregion

				}
				#endregion

				container.Controls.Add(div);
			}
			#endregion

			#region Private handlers
			void OnPositionDataBinding(object sender, EventArgs e)
			{
				if (string.IsNullOrEmpty(this.PositionValueField))
					return;

				TableCell control = sender as TableCell;
				GridViewRow row = (GridViewRow)control.NamingContainer;

				string text = (row.DataItem as DataRowView)[this.PositionValueField].ToString();
				control.Text = text;
			}
			void OnAditionalPropertiesDataBinding(object sender, EventArgs e)
			{
				Control control = sender as Control;
				GridViewRow row = (GridViewRow)control.NamingContainer;

				foreach (AditionalProperty ap in this.AditionalProperties)
				{
					object value = (row.DataItem as DataRowView)[ap.ValueFieldName];
					string property = string.Format("{0}{1}{2}",
						ap.Text,
						string.IsNullOrEmpty(ap.Text) ? string.Empty : " : ",
						value);

					HtmlGenericControl div = new HtmlGenericControl("div");
					if (control.Controls.Count != 0)
						control.Controls.Add(new LiteralControl("<hr/>"));

					div.Controls.Add(new LiteralControl(property));
					control.Controls.Add(div);
				}
			}
			void OnContainerDataBinding(object sender, EventArgs e)
			{
				Control control = sender as Control;
				GridViewRow row = (GridViewRow)control.NamingContainer;

				if ((row.DataItem as DataRowView)[this.IDValueField] is DBNull)
				{
					control.Controls.Clear();
					control.Visible = false;
					return;
				}

			}
			void OnButtonDeleteDataBinding(object sender, EventArgs e)
			{
				Control control = sender as Control;
				GridViewRow row = (GridViewRow)control.NamingContainer;

				if ((row.DataItem as DataRowView)[this.IDValueField] is DBNull)
				{
					control.Controls.Clear();
					control.Visible = false;
					return;
				}

				(control as LinkButton).CommandArgument = (row.DataItem as DataRowView)[this.IDValueField].ToString();
			}

			void OnButtonShiftLeftDataBinding(object sender, EventArgs e)
			{
				LinkButton control = sender as LinkButton;
				GridViewRow row = (GridViewRow)control.NamingContainer;

				if ((row.DataItem as DataRowView)[this.IDValueField] is DBNull)
				{
					control.Controls.Clear();
					control.Visible = false;
					return;
				}

				//Ak je obrazok prvy do lava sa uz nebude dat posunut.
				if (Convert.ToBoolean((row.DataItem as DataRowView)[this.IsFirstValueField]) == true)
				{
					//control.Controls.Clear();
					control.Enabled = false;
					(control.Controls[0] as HtmlGenericControl).Attributes.Add("class", this.CssClass + "_itemShiftLeft_disabled");
					return;
				}

				control.CommandArgument = (row.DataItem as DataRowView)[this.IDValueField].ToString();
			}

			void OnButtonShiftRightDataBinding(object sender, EventArgs e)
			{
				LinkButton control = sender as LinkButton;
				GridViewRow row = (GridViewRow)control.NamingContainer;

				if ((row.DataItem as DataRowView)[this.IDValueField] is DBNull)
				{
					control.Controls.Clear();
					control.Visible = false;
					return;
				}

				//Ak je obrazok posledny do prava sa uz nebude dat posunut.
				if (Convert.ToBoolean((row.DataItem as DataRowView)[this.IsLastValueField]) == true)
				{
					//control.Controls.Clear();
					control.Enabled = false;
					(control.Controls[0] as HtmlGenericControl).Attributes.Add("class", this.CssClass + "_itemShiftRight_disabled");
					return;
				}

				control.CommandArgument = (row.DataItem as DataRowView)[this.IDValueField].ToString();
			}
			void OnImageDataBinding(object sender, EventArgs e)
			{
				if (string.IsNullOrEmpty(this.ValueField) || string.IsNullOrEmpty(this.ValueThumbnailField))
					return;

				Image control = sender as Image;
				GridViewRow row = (GridViewRow)control.NamingContainer;

				string imageUrl = (row.DataItem as DataRowView)[this.ValueField].ToString();
				string imageUrlThumbnail = (row.DataItem as DataRowView)[this.ValueThumbnailField].ToString();
				control.ImageUrl = imageUrlThumbnail.StartsWith("~") ? control.Page.ResolveUrl(imageUrlThumbnail) : imageUrlThumbnail;

				if (this.Mode == DisplayMode.Display && control.Parent is HtmlGenericControl)
				{
					HtmlGenericControl container = control.Parent as HtmlGenericControl;
					container.Attributes.Add("href", imageUrl);
					container.Attributes.Add("title", string.Empty);
				}
			}
			void OnDeleteImage(object sender, EventArgs e)
			{
				string itemId = (sender as LinkButton).CommandArgument;
				if (string.IsNullOrEmpty(itemId))
					return;

				if (ItemDelete != null)
					ItemDelete(this, itemId);
			}
			void OnShiftImageRight(object sender, EventArgs e)
			{
				string itemId = (sender as LinkButton).CommandArgument;
				if (string.IsNullOrEmpty(itemId))
					return;

				if (ItemShiftRight != null)
					ItemShiftRight(this, itemId);
			}
			void OnShiftImageLeft(object sender, EventArgs e)
			{
				string itemId = (sender as LinkButton).CommandArgument;
				if (string.IsNullOrEmpty(itemId))
					return;

				if (ItemShiftLeft != null)
					ItemShiftLeft(this, itemId);
			}
			#endregion

		}
		#endregion

		#region Image Attributes implementation
		public class ImageAttribute
		{
			private string key = string.Empty;
			private string value = string.Empty;
			public ImageAttribute(string key, string value)
			{
				this.key = key;
				this.value = value;
			}

			public string Key
			{
				get { return this.key; }
			}

			public string Value
			{
				get { return this.value; }
			}
		}
		#endregion

		#region AditionalProperty implementation
		public class AditionalProperty
		{
			private string text = string.Empty;
			private string valueFieldName = string.Empty;

			public AditionalProperty(string text, string valueFieldName)
			{
				this.text = text;
				this.valueFieldName = valueFieldName;
			}

			public string ValueFieldName
			{
				get { return this.valueFieldName; }
			}

			public string Text
			{
				get { return this.text; }
			}
		}
		#endregion

		#region Static helper methods
		private static System.Drawing.Bitmap BitmapResize(System.Drawing.Bitmap b, int nWidth, int nHeight, bool bBilinear)
		{
			System.Drawing.Bitmap bTemp = (System.Drawing.Bitmap)b.Clone();
			b = new System.Drawing.Bitmap(nWidth, nHeight, bTemp.PixelFormat);

			double nXFactor = (double)bTemp.Width / (double)nWidth;
			double nYFactor = (double)bTemp.Height / (double)nHeight;

			if (bBilinear)
			{
				/*
				double fraction_x, fraction_y, one_minus_x, one_minus_y;
				int ceil_x, ceil_y, floor_x, floor_y;
				System.Drawing.Color c1 = new System.Drawing.Color();
				System.Drawing.Color c2 = new System.Drawing.Color();
				System.Drawing.Color c3 = new System.Drawing.Color();
				System.Drawing.Color c4 = new System.Drawing.Color();
				byte red, green, blue, alpha;

				byte b1, b2;

				for ( int x = 0; x < b.Width; ++x )
						for ( int y = 0; y < b.Height; ++y )
						{
								// Setup

								floor_x = (int)Math.Floor( x * nXFactor );
								floor_y = (int)Math.Floor( y * nYFactor );
								ceil_x = floor_x + 1;
								if ( ceil_x >= bTemp.Width ) ceil_x = floor_x;
								ceil_y = floor_y + 1;
								if ( ceil_y >= bTemp.Height ) ceil_y = floor_y;
								fraction_x = x * nXFactor - floor_x;
								fraction_y = y * nYFactor - floor_y;
								one_minus_x = 1.0 - fraction_x;
								one_minus_y = 1.0 - fraction_y;

								c1 = bTemp.GetPixel( floor_x, floor_y );
								c2 = bTemp.GetPixel( ceil_x, floor_y );
								c3 = bTemp.GetPixel( floor_x, ceil_y );
								c4 = bTemp.GetPixel( ceil_x, ceil_y );

								// Blue
								b1 = (byte)( one_minus_x * c1.B + fraction_x * c2.B );

								b2 = (byte)( one_minus_x * c3.B + fraction_x * c4.B );

								blue = (byte)( one_minus_y * (double)( b1 ) + fraction_y * (double)( b2 ) );

								// Green
								b1 = (byte)( one_minus_x * c1.G + fraction_x * c2.G );

								b2 = (byte)( one_minus_x * c3.G + fraction_x * c4.G );

								green = (byte)( one_minus_y * (double)( b1 ) + fraction_y * (double)( b2 ) );

								// Red
								b1 = (byte)( one_minus_x * c1.R + fraction_x * c2.R );

								b2 = (byte)( one_minus_x * c3.R + fraction_x * c4.R );

								red = (byte)( one_minus_y * (double)( b1 ) + fraction_y * (double)( b2 ) );

								alpha = (byte)( (c4.R + c4.G + c4.B )/3 );

								b.SetPixel( x, y, System.Drawing.Color.FromArgb( alpha, red, green, blue ) );
						}
				 * */

				System.Drawing.Graphics graphicsImage = System.Drawing.Graphics.FromImage(b);

				graphicsImage.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
				graphicsImage.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
				graphicsImage.DrawImage(bTemp, 0, 0, b.Width, b.Height);
				graphicsImage.Dispose();
			}
			else
			{
				System.Drawing.Graphics graphicsImage = System.Drawing.Graphics.FromImage(b);

				graphicsImage.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
				graphicsImage.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
				graphicsImage.DrawImage(bTemp, 0, 0, b.Width, b.Height);
				graphicsImage.Dispose();
				//for ( int x = 0; x < b.Width; ++x )
				//    for ( int y = 0; y < b.Height; ++y )
				//        b.SetPixel( x, y, bTemp.GetPixel( (int)( Math.Floor( x * nXFactor ) ), (int)( Math.Floor( y * nYFactor ) ) ) );
			}
			return b;
		}

		public static MemoryStream GetImageStream(Stream inputStream, int maxWidth, int maxHeight, bool stretch)
		{
			System.Drawing.Bitmap b = null;
			//Resize obrazku.
			try
			{
				b = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream(inputStream);
			}
			catch
			{
				return null;
			}

			if (stretch)
				b = ImageGalleryControl.BitmapResize(b, maxWidth, maxHeight, true);
			else
			{
				bool recalculate = false;
				recalculate = (b.Width > maxWidth || b.Height > maxHeight);

				//Resize iba ak je obrazok vacsi ako maximalna povolena velkost.
				if (recalculate)
				{
					int width = maxWidth;
					int height = maxHeight;
					RecalculateImageSize(b, maxWidth, maxHeight, ref width, ref height);
					b = ImageGalleryControl.BitmapResize(b, width, height, true);
				}
			}

			MemoryStream str = new MemoryStream();
			try
			{
				b.Save(str, b.RawFormat);
			}
			catch
			{
				b.Save(str, System.Drawing.Imaging.ImageFormat.Jpeg);
			}

			return str;
		}

		public static MemoryStream GetImageStream(Stream inputStream)
		{
			//Resize obrazku.
			System.Drawing.Bitmap b = null;
			System.Drawing.Imaging.ImageFormat imf = System.Drawing.Imaging.ImageFormat.Png;
			try
			{
				b = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream(inputStream);
				imf = b.RawFormat;
			}
			catch
			{
				return null;
			}

			MemoryStream str = new MemoryStream();
			try
			{
				b.Save(str, imf);
			}
			catch
			{
				b.Save(str, System.Drawing.Imaging.ImageFormat.Jpeg);
			}

			return str;
		}

		public static bool ResizeImage(string fileName, string newFileName, int width, int height)
		{
			//Resize obrazku.
			System.Drawing.Bitmap b = null;
			System.Drawing.Imaging.ImageFormat imf = System.Drawing.Imaging.ImageFormat.Png;
			try
			{
				b = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(fileName);
				imf = b.RawFormat;
			}
			catch
			{
				return false;
			}
			b = ImageGalleryControl.BitmapResize(b, width, height, true);
			b.Save(newFileName, imf /*System.Drawing.Imaging.ImageFormat.Jpeg*/ );
			b.Dispose();

			return true; ;
		}

		/// <summary>
		/// PrepoËÌta v˝öku a öÌrku obr·zku v spr·vnom (povodnom) pomere str·n;
		/// </summary>
		private static void RecalculateImageSize(System.Drawing.Bitmap b, int maxWidth, int maxHeight, ref int width, ref int height)
		{
			RecalculateImageSize(b.Width, b.Height, maxWidth, maxHeight, ref width, ref height);
		}

		/// <summary>
		/// PrepoËÌta v˝öku a öÌrku obr·zku v spr·vnom (povodnom) pomere str·n;
		/// </summary>
		public static void RecalculateImageSize(int imageWidth, int imageHeight, int maxWidth, int maxHeight, ref int width, ref int height)
		{
			if (maxWidth > imageWidth && maxHeight > imageHeight)
			{
				width = imageWidth;
				height = imageHeight;
				return;
			}

			int imageW = imageWidth;
			int imageH = imageHeight;

			double wIndex = (double)imageWidth / (double)maxWidth;
			double hIndex = (double)imageHeight / (double)maxHeight;

			if (hIndex > wIndex)
			{
				height = maxHeight;
				width = (int)((imageW * height) / imageH);
			}
			else
			{
				width = maxWidth;
				height = (int)((imageH * width) / imageW);
			}
		}
		#endregion
	}
}
