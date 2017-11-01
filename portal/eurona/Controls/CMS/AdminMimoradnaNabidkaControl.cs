using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMSEditor = CMS.Controls.RadEditor.RadEditor;
using MimoradnaNabidkaEntity = Eurona.DAL.Entities.MimoradnaNabidka;
//using CMS.Utilities;
using System.IO;
using CMS.Controls;

namespace Eurona.Controls
{
	public class AdminMimoradnaNabidkaControl : CmsControl
	{
		protected FileUpload iconUpload = null;
		protected Image icon = null;
		protected Button iconRemove = null;

		private TextBox txtTitle = null;
		private TextBox txtTeaser = null;
		private ASPxDatePicker dtpDate = null;
		private CMSEditor edtContent;
		private CMS.Controls.ASPxUrlAliasTextBox txtUrlAlis;

		private Button btnSave = null;
		private Button btnCancel = null;

		private MimoradnaNabidkaEntity mimoradnaNabidka = null;

		public AdminMimoradnaNabidkaControl()
		{
		}

		/// <summary>
		/// Ak je property null, komponenta pracuje v rezime New.
		/// </summary>
		public int? MimoradnaNabidkaId
		{
			get
			{
				object o = ViewState["MimoradnaNabidkaId"];
				return o != null ? (int?)Convert.ToInt32(o) : null;
			}
			set { ViewState["MimoradnaNabidkaId"] = value; }
		}

		/// <summary>
		/// ID Url Alis prefixu, ktory sa ma doplnat pre samotny alias.
		/// </summary>
		public int? UrlAliasPrefixId
		{
			get
			{
				object o = ViewState["UrlAliasPrefixId"];
				return o != null ? (int?)Convert.ToInt32(o) : null;
			}
			set { ViewState["UrlAliasPrefixId"] = value; }
		}

		public string DisplayUrlFormat { get; set; }

		#region Protected overrides
		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			Control pollControl = CreateDetailControl();
			if (pollControl != null)
				this.Controls.Add(pollControl);

			//Priradenie id fieldu z ktoreho sa generuje alias
			this.txtUrlAlis.FieldID = this.txtTitle.ClientID;
			this.txtUrlAlis.UrlAliasPrefixId = this.UrlAliasPrefixId;
			this.txtUrlAlis.OnGetUrlAliasPrefix += new ASPxUrlAliasTextBox.UrlAliasPrefixEventHandler(txtUrlAlis_OnGetUrlAliasPrefix);

			//Binding
			if (!this.MimoradnaNabidkaId.HasValue) this.mimoradnaNabidka = new Eurona.DAL.Entities.MimoradnaNabidka();
			else this.mimoradnaNabidka = Storage<MimoradnaNabidkaEntity>.ReadFirst(new MimoradnaNabidkaEntity.ReadById { MimoradnaNabidkaId = this.MimoradnaNabidkaId.Value });

			//Nastavenie zobrazenia icony
			if (string.IsNullOrEmpty(this.mimoradnaNabidka.Icon)) { this.iconRemove.Visible = false; this.icon.Visible = false; }
			else this.iconUpload.Visible = false;

			if (!IsPostBack)
			{
				this.icon.ImageUrl = this.mimoradnaNabidka.Icon != null ? Page.ResolveUrl(this.mimoradnaNabidka.Icon) : string.Empty;
				this.icon.Style.Add("max-width", ImageGalleryControl.IMAGE_WIDTH.ToString() + "px");
				this.icon.Style.Add("max-height", ImageGalleryControl.IMAGE_WIDTH.ToString() + "px");

				this.dtpDate.Value = this.mimoradnaNabidka.Date;
				this.txtTitle.Text = this.mimoradnaNabidka.Title;
				this.txtTeaser.Text = this.mimoradnaNabidka.Teaser;
				this.edtContent.Content = this.mimoradnaNabidka.Content;

				//Nastavenie controlsu pre UrlAlias
				this.txtUrlAlis.AutoCompletteAlias = !this.MimoradnaNabidkaId.HasValue;
				this.txtUrlAlis.Text = this.mimoradnaNabidka.Alias.StartsWith("~") ? this.mimoradnaNabidka.Alias.Remove(0, 1) : this.mimoradnaNabidka.Alias;
				this.DataBind();
			}

		}
		void txtUrlAlis_OnGetUrlAliasPrefix(string prefix, out string newPrefix)
		{
			newPrefix = "mimoradna-nabidka";
		}
		#endregion

		/// <summary>
		/// Vytvori Control Novinky
		/// </summary>
		private Control CreateDetailControl()
		{
			this.icon = new Image();
			this.icon.ID = "icon";
			this.iconUpload = new FileUpload();
			this.iconUpload.ID = "iconUpload";
			this.iconRemove = new Button();
			this.iconRemove.Text = global::CMS.Resources.Controls.AdminNewsControl__RemoveIcon;
			this.iconRemove.ID = "iconRemove";
			this.iconRemove.Click += (s, e) =>
			{
				if (this.mimoradnaNabidka == null) return;
				this.RemoveIcon(this.mimoradnaNabidka);
				this.icon.ImageUrl = string.Empty;

				//Nastavenie viditelnosti
				this.iconRemove.Visible = false;
				this.icon.Visible = false;
				this.iconUpload.Visible = true;
			};

			this.txtTitle = new TextBox();
			this.txtTitle.ID = "txtTitle";
			this.txtTitle.Width = Unit.Percentage(100);

			this.dtpDate = new ASPxDatePicker();
			this.dtpDate.ID = "dtpDate";

			this.txtTeaser = new TextBox();
			this.txtTeaser.ID = "txtTeaser";
			this.txtTeaser.TextMode = TextBoxMode.MultiLine;
			this.txtTeaser.Width = Unit.Percentage(100);
			this.txtTeaser.Height = Unit.Pixel(200);

			this.edtContent = new CMSEditor();
			this.edtContent.ID = "edtContent";

			this.txtUrlAlis = new ASPxUrlAliasTextBox();
			this.txtUrlAlis.ID = "txtUrlAlis";
			this.txtUrlAlis.Width = Unit.Percentage(100);

			this.btnSave = new Button();
			this.btnSave.CausesValidation = true;
			this.btnSave.Text = CMS.Resources.Controls.SaveButton_Text;
			this.btnSave.Click += new EventHandler(OnSave);
			this.btnCancel = new Button();
			this.btnCancel.CausesValidation = false;
			this.btnCancel.Text = CMS.Resources.Controls.CancelButton_Text;
			this.btnCancel.Click += new EventHandler(OnCancel);

			Table table = new Table();
			table.Width = this.Width;
			table.Height = this.Height;

			#region Icon
			TableRow row = new TableRow();
			TableCell cell = new TableCell();
			cell.CssClass = "form_label";
			cell.Text = CMS.Resources.Controls.AdminNewsControl_Icon;
			row.Cells.Add(cell);

			cell = new TableCell();
			cell.CssClass = "form_control";
			cell.VerticalAlign = VerticalAlign.Middle;
			cell.Controls.Add(this.icon);
			cell.Controls.Add(this.iconRemove);
			cell.Controls.Add(this.iconUpload);
			row.Cells.Add(cell);
			table.Rows.Add(row);
			#endregion

			table.Rows.Add(CreateTableRow(global::CMS.Resources.Controls.AdminNewsControl_Date, this.dtpDate, true));
			table.Rows.Add(CreateTableRow(global::CMS.Resources.Controls.AdminNewsControl_Title, this.txtTitle, true));
			table.Rows.Add(CreateTableRow(global::CMS.Resources.Controls.AdminNewsControl_Description, this.txtTeaser, false));
			table.Rows.Add(CreateTableRow(global::CMS.Resources.Controls.AdminNewsControl_Content, this.edtContent, false));
			table.Rows.Add(CreateTableRow(global::CMS.Resources.Controls.AdminNewsControl_UrlAlias, this.txtUrlAlis, true));

			//Save Cancel Buttons
			row = new TableRow();
			cell = new TableCell();
			cell.ColumnSpan = 2;
			cell.Controls.Add(this.btnSave);
			cell.Controls.Add(this.btnCancel);
			row.Cells.Add(cell);
			table.Rows.Add(row);

			return table;
		}

		private TableRow CreateTableRow(string labelText, Control control, bool required)
		{
			TableRow row = new TableRow();
			TableCell cell = new TableCell();
			cell.CssClass = required ? "form_label_required" : "form_label";
			cell.Text = labelText;
			row.Cells.Add(cell);

			cell = new TableCell();
			cell.CssClass = "form_control";
			cell.Controls.Add(control);
			if (required) cell.Controls.Add(base.CreateRequiredFieldValidatorControl(control.ID));
			row.Cells.Add(cell);

			return row;
		}

		protected void RemoveIcon(MimoradnaNabidkaEntity entity)
		{
			string filePath = Server.MapPath(entity.Icon);
			if (File.Exists(filePath)) File.Delete(filePath);

			entity.Icon = null;
			Storage<MimoradnaNabidkaEntity>.Update(entity);
		}

		protected string UploadIcon(MimoradnaNabidkaEntity entity)
		{
			if (!this.iconUpload.HasFile) return entity.Icon;

			string storagePath = CMS.Utilities.ConfigUtilities.GetEntityIconStoragePath(mimoradnaNabidka.GetType());
			string filePath = Server.MapPath(mimoradnaNabidka.Icon);
			if (File.Exists(filePath)) File.Delete(filePath);

			string storageDirectoty = Server.MapPath(storagePath);
			if (!Directory.Exists(storageDirectoty)) Directory.CreateDirectory(storageDirectoty);
			string dstFileName = string.Format("{0}.png", CMS.Utilities.StringUtilities.RemoveDiacritics(mimoradnaNabidka.Title));
			dstFileName = dstFileName.Replace(":", "-"); dstFileName = dstFileName.Replace("&", "");
			string dstFilePath = Path.Combine(storageDirectoty, dstFileName);

			//Zapis suboru
			Stream stream = this.iconUpload.PostedFile.InputStream;
			int len = (int)stream.Length;
			if (len == 0) return null;
			byte[] data = new byte[len];
			stream.Read(data, 0, len);
			stream.Close();
			using (FileStream fs = new FileStream(dstFilePath, FileMode.Create, FileAccess.Write))
			{
				fs.Write(data, 0, len);
			}

			return string.Format("{0}{1}", storagePath, dstFileName);
		}

		void OnSave(object sender, EventArgs e)
		{
			if (this.mimoradnaNabidka.Id != 0)
			{
				string icon = this.mimoradnaNabidka.Icon;
				Storage<MimoradnaNabidkaEntity>.Delete(this.mimoradnaNabidka);
				this.mimoradnaNabidka = new Eurona.DAL.Entities.MimoradnaNabidka();
				this.mimoradnaNabidka.Icon = icon;
			}

			this.mimoradnaNabidka.Date = Convert.ToDateTime(this.dtpDate.Value);
			this.mimoradnaNabidka.Title = this.txtTitle.Text;
			this.mimoradnaNabidka.Icon = this.UploadIcon(this.mimoradnaNabidka);
			this.mimoradnaNabidka.Teaser = this.txtTeaser.Text;
			this.mimoradnaNabidka.Content = this.edtContent.Content;
			this.mimoradnaNabidka.Locale = Security.Account.Locale;

			Storage<MimoradnaNabidkaEntity>.Create(this.mimoradnaNabidka);

			#region Vytvorenie URLAliasu
			string alias = this.txtUrlAlis.GetUrlAlias(string.Format("~/{0}", this.mimoradnaNabidka.Title));
			if (!CMS.Utilities.AliasUtilities.CreateUrlAlias<MimoradnaNabidkaEntity>(this.Page, this.DisplayUrlFormat, this.mimoradnaNabidka.Title, alias, this.mimoradnaNabidka, Storage<MimoradnaNabidkaEntity>.Instance))
				return;
			#endregion

			Response.Redirect(this.ReturnUrl);
		}

		void OnCancel(object sender, EventArgs e)
		{
			Response.Redirect(this.ReturnUrl);
		}
	}
}
