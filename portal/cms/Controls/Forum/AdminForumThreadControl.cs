using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ForumThreadEntity = CMS.Entities.ForumThread;
using CMSEditor = CMS.Controls.RadEditor.RadEditor;
using RoleEntity = CMS.Entities.Role;
using System.Text;
using CMS.Utilities;
using System.IO;

namespace CMS.Controls.Forum
{
	public class AdminForumThreadControl : CmsControl
	{
		protected FileUpload iconUpload = null;
		protected Image icon = null;
		protected Button iconRemove = null;

		private TextBox txtName = null;
		private TextBox txtDescription = null;
		private CheckBoxList cblVisibleForRole = null;
		private CheckBoxList cblEditableForRole = null;
		private CheckBox cbLocked = null;
		private CMS.Controls.ASPxUrlAliasTextBox txtUrlAlis;

		private Button btnSave = null;
		private Button btnCancel = null;

		private ForumThreadEntity forumThread = null;

		public AdminForumThreadControl()
		{
		}

		/// <summary>
		/// Ak je property null, komponenta pracuje v rezime New.
		/// </summary>
		public int? ForumThreadId
		{
			get
			{
				object o = ViewState["ForumThreadId"];
				return o != null ? (int?)Convert.ToInt32(o) : null;
			}
			set { ViewState["ForumThreadId"] = value; }
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
		public bool ShowRoleManager { get; set; }

		#region Protected overrides
		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			Control articleControl = CreateDetailControl();
			if (articleControl != null)
				this.Controls.Add(articleControl);

			//Priradenie id fieldu z ktoreho sa generuje alias
			this.txtUrlAlis.FieldID = this.txtName.ClientID;
			this.txtUrlAlis.UrlAliasPrefixId = this.UrlAliasPrefixId;

			//Binding
			if (!this.ForumThreadId.HasValue) this.forumThread = new CMS.Entities.ForumThread();
			else this.forumThread = Storage<ForumThreadEntity>.ReadFirst(new ForumThreadEntity.ReadById { ForumThreadId = this.ForumThreadId.Value });

			//Nastavenie zobrazenia icony
			if (string.IsNullOrEmpty(this.forumThread.Icon)) { this.iconRemove.Visible = false; this.icon.Visible = false; }
			else this.iconUpload.Visible = false;

			if (!IsPostBack)
			{
				this.icon.ImageUrl = this.forumThread.Icon != null ? Page.ResolveUrl(this.forumThread.Icon) : string.Empty;
				this.icon.Style.Add("max-width", ImageGalleryControl.IMAGE_WIDTH.ToString() + "px");
				this.icon.Style.Add("max-height", ImageGalleryControl.IMAGE_WIDTH.ToString() + "px");

				//Role
				List<RoleEntity> rolesV = Storage<RoleEntity>.Read();
				rolesV = rolesV.FindAll(x => x.Name.ToUpper() != CMS.Entities.Role.ADMINISTRATOR.ToUpper());
				rolesV = rolesV.OrderBy(p => p.Name).ToList();
				this.cblVisibleForRole.DataSource = rolesV;
				this.cblVisibleForRole.RepeatColumns = 3;
				this.cblVisibleForRole.DataTextField = "Name";
				this.cblVisibleForRole.DataValueField = "Id";
				this.cblVisibleForRole.DataBind();

				List<RoleEntity> rolesE = Storage<RoleEntity>.Read();
				rolesE = rolesE.FindAll(x => x.Name.ToUpper() != CMS.Entities.Role.ADMINISTRATOR.ToUpper());
				rolesE = rolesE.OrderBy(p => p.Name).ToList();
				this.cblEditableForRole.DataSource = rolesE;
				this.cblEditableForRole.RepeatColumns = 3;
				this.cblEditableForRole.DataTextField = "Name";
				this.cblEditableForRole.DataValueField = "Id";
				this.cblEditableForRole.DataBind();

				string[] visbleForRole = this.forumThread.VisibleForRole.Split(';');
				foreach (ListItem li in this.cblVisibleForRole.Items)
					if (visbleForRole.Contains(li.Text)) li.Selected = true;

				string[] editableForRole = this.forumThread.EditableForRole.Split(';');
				foreach (ListItem li in this.cblEditableForRole.Items)
					if (editableForRole.Contains(li.Text)) li.Selected = true;

				this.txtName.Text = this.forumThread.Name;
				this.txtDescription.Text = this.forumThread.Description;
				this.cbLocked.Checked = this.forumThread.Locked;

				//Nastavenie controlsu pre UrlAlias
				this.txtUrlAlis.AutoCompletteAlias = !this.ForumThreadId.HasValue;
				this.txtUrlAlis.Text = this.forumThread.Alias.StartsWith("~") ? this.forumThread.Alias.Remove(0, 1) : this.forumThread.Alias;
			}
		}
		#endregion

		/// <summary>
		/// Vytvori Control Clanku
		/// </summary>
		private Control CreateDetailControl()
		{
			this.icon = new Image();
			this.icon.ID = "icon";
			this.iconUpload = new FileUpload();
			this.iconUpload.ID = "iconUpload";
			this.iconRemove = new Button();
			this.iconRemove.CssClass = this.CssClass + "_remove";
			this.iconRemove.Text = Resources.Controls.AdminForumThreadControl_RemoveIcon;
			this.iconRemove.ID = "iconRemove";
			this.iconRemove.Click += (s, e) =>
			{
				if (this.forumThread == null) return;
				this.RemoveIcon(this.forumThread);
				this.icon.ImageUrl = string.Empty;

				//Nastavenie viditelnosti
				this.iconRemove.Visible = false;
				this.icon.Visible = false;
				this.iconUpload.Visible = true;
			};

			this.txtName = new TextBox();
			this.txtName.ID = "txtName";
			this.txtName.Width = Unit.Percentage(100);

			this.cblVisibleForRole = new CheckBoxList();
			this.cblVisibleForRole.ID = "cblVisibleForRole";
			this.cblVisibleForRole.Width = Unit.Percentage(100);

			this.cblEditableForRole = new CheckBoxList();
			this.cblEditableForRole.ID = "cblEditableForRole";
			this.cblEditableForRole.Width = Unit.Percentage(100);

			this.txtDescription = new TextBox();
			this.txtDescription.ID = "txtDescription";
			this.txtDescription.TextMode = TextBoxMode.MultiLine;
			this.txtDescription.Width = Unit.Percentage(100);
			this.txtDescription.Height = Unit.Pixel(100);

			this.cbLocked = new CheckBox();
			this.cbLocked.ID = "cbLocked";
			this.cbLocked.Checked = true;

			this.txtUrlAlis = new ASPxUrlAliasTextBox();
			this.txtUrlAlis.ID = "txtUrlAlis";
			this.txtUrlAlis.Width = Unit.Percentage(100);

			this.btnSave = new Button();
			this.btnSave.CausesValidation = true;
			this.btnSave.Text = Resources.Controls.SaveButton_Text;
			this.btnSave.Click += new EventHandler(OnSave);
			this.btnCancel = new Button();
			this.btnCancel.CausesValidation = false;
			this.btnCancel.Text = Resources.Controls.CancelButton_Text;
			this.btnCancel.Click += new EventHandler(OnCancel);

			Table table = new Table();
			table.CssClass = this.CssClass + "_table";
			table.Width = this.Width;
			table.Height = this.Height;

			#region Icon
			TableRow row = new TableRow();
			TableCell cell = new TableCell();
			cell.CssClass = "form_label";
			cell.Text = Resources.Controls.AdminForumThreadControl_Icon;
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

			table.Rows.Add(CreateTableRow(Resources.Controls.AdminForumThreadControl_Name, this.txtName, true));
			table.Rows.Add(CreateTableRow(Resources.Controls.AdminForumThreadControl_Description, this.txtDescription, false));
			table.Rows.Add(CreateTableRow(Resources.Controls.AdminForumThreadControl_Locked, this.cbLocked, false));
			if (this.ShowRoleManager)
			{
				table.Rows.Add(CreateTableRow(Resources.Controls.AdminForumThreadControl_VisibleForRole, this.cblVisibleForRole, false));
				table.Rows.Add(CreateTableRow(string.Empty, new LiteralControl(), false));
				table.Rows.Add(CreateTableRow(Resources.Controls.AdminForumThreadControl_EditableForRole, this.cblEditableForRole, false));
			}
			table.Rows.Add(CreateTableRow(Resources.Controls.AdminForumThreadControl_UrlAlias, this.txtUrlAlis, true));

			//Save Cancel Buttons
			row = new TableRow();
			this.iconRemove.CssClass = this.CssClass + "_table_buttons";
			cell = new TableCell();
			cell.ColumnSpan = 2;
			cell.Controls.Add(this.btnSave);
			cell.Controls.Add(new LiteralControl
			{
				Text = ""
			});
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

		protected void RemoveIcon(ForumThreadEntity entity)
		{
			string filePath = Server.MapPath(entity.Icon);
			if (File.Exists(filePath)) File.Delete(filePath);

			entity.Icon = null;
			Storage<ForumThreadEntity>.Update(entity);
		}

		protected string UploadIcon(ForumThreadEntity entity)
		{
			if (!this.iconUpload.HasFile) return entity.Icon;

			string storagePath = CMS.Utilities.ConfigUtilities.GetEntityIconStoragePath(forumThread.GetType());
			string filePath = Server.MapPath(forumThread.Icon);
			if (File.Exists(filePath)) File.Delete(filePath);

			string storageDirectoty = Server.MapPath(storagePath);
			if (!Directory.Exists(storageDirectoty)) Directory.CreateDirectory(storageDirectoty);
			string dstFileName = string.Format("{0}.png", CMS.Utilities.StringUtilities.RemoveDiacritics(forumThread.Name));
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
			this.forumThread.Name = this.txtName.Text;
			this.forumThread.Icon = this.UploadIcon(this.forumThread);
			this.forumThread.Description = this.txtDescription.Text;
			this.forumThread.Locale = Security.Account.Locale;
			this.forumThread.Locked = this.cbLocked.Checked;

			//VisibleForRole
			StringBuilder visibleForRole = new StringBuilder();
			foreach (ListItem li in this.cblVisibleForRole.Items)
				if (li.Selected) visibleForRole.AppendFormat("{0};", li.Text);
			this.forumThread.VisibleForRole = visibleForRole.ToString();

			//EditableForRole
			StringBuilder editableForRole = new StringBuilder();
			foreach (ListItem li in this.cblEditableForRole.Items)
				if (li.Selected) editableForRole.AppendFormat("{0};", li.Text);
			this.forumThread.EditableForRole = editableForRole.ToString();

			if (!this.ForumThreadId.HasValue) Storage<ForumThreadEntity>.Create(this.forumThread);
			else Storage<ForumThreadEntity>.Update(this.forumThread);

			#region Vytvorenie URLAliasu
			string alias = this.txtUrlAlis.GetUrlAlias(string.Format("~/{0}", this.forumThread.Name));
			if (!Utilities.AliasUtilities.CreateUrlAlias<ForumThreadEntity>(this.Page, this.DisplayUrlFormat, this.forumThread.Name, alias, this.forumThread))
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
