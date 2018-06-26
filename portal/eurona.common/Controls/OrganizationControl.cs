using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CMS.Controls;
using CMS.Controls.UserManagement;
using Eurona.Common.DAL.Entities;
using CMS;
using System.Data;
using System.Configuration;

namespace Eurona.Common.Controls.UserManagement
{
	public class OrganizationControl : CmsControl
	{
        public event EventHandler OnChildControlsCreated; 
		#region Private members
		private Label lblCode = null;
		public TextBox txtId1 = null;
        public TextBox txtId2 = null;
        public TextBox txtId3 = null;

        public TextBox txtName = null;
        public TextBox txtWeb = null;
        public TextBox txtNotes = null;
        public CheckBox cbVATPayment = null;
        //public CheckBox cbTopManager = null;
		//private DropDownList ddlParent = null;
        public TextBox txtParent = null;

        public TextBox txtContactEmail = null;
        public TextBox txtContactPhone = null;
        public TextBox txtContactMobil = null;

        //public TextBox txtFAX = null;
        //public TextBox txtSkype = null;
        //public TextBox txtICQ = null;
        public ASPxDatePicker dtpContactBirthDay = null;
        //public TextBox txtContactCardId = null;
        //public TextBox txtContactWorkPhone = null;
        public DropDownList ddlRegion = null;
        public DropDownList ddlPF = null;
        public DropDownList ddlStatut = null;
        public TextBox txtPredmetCinnosti = null;

        public PersonControl contactPerson = null;

        public AddressControl registeredAddress = null;
        public CheckBox cbCorrespondenceAddressAsRegistered = null;
        public AddressControl correspondenceAddress = null;
        public CheckBox cbInvoicingAddressAsRegistered = null;
        public AddressControl invoicingAddress = null;
        public BankContactControl bankContact = null;

		protected Button btnSave = null;
		private Button btnCancel = null;

		private Organization organization = null;
		private bool isNew = false;

		#endregion

		public string CancelButtonText { get; set; }
		public string SaveButtonText { get; set; }

		public EventHandler SaveCompleted = null;
		public EventHandler Canceled = null;

		public OrganizationControl()
		{
		}

		public ControlSettings Settings
		{
			get
			{
				object o = ViewState["Settings"];
				return o != null ? (ControlSettings)o : null;
			}
			set { ViewState["Settings"] = value; }
		}

		#region Public properties
		/// <summary>
		/// Property je nutne naplnat pri zobrazovani a editacii organizacie.
		/// </summary>
		public int? OrganizationId
		{
			get
			{
				object o = ViewState["OrganizationId"];
				return o != null ? (int?)Convert.ToInt32(o) : null;
			}
			set { ViewState["OrganizationId"] = value; }
		}

		/// <summary>
		/// Property je nutne naplnat pri vytvarani novej organizacie
		/// ak su to osobne udaje registrovaneho pouzivatela.
		/// </summary>
		public int? AccountId
		{
			get
			{
				object o = ViewState["AccountId"];
				return o != null ? (int?)Convert.ToInt32(o) : null;
			}
			set { ViewState["AccountId"] = value; }
		}

		public string PreddefinedName
		{
			get
			{
				object o = ViewState["PreddefinedName"];
				return o != null ? o.ToString() : null;
			}
			set { ViewState["PreddefinedName"] = value; }
		}
		public string PreddefinedEmail
		{
			get
			{
				object o = ViewState["PreddefinedEmail"];
				return o != null ? o.ToString() : null;
			}
			set { ViewState["PreddefinedEmail"] = value; }
		}
		public int? PreddefinedParentTVD_Id
		{
			get
			{
				object o = ViewState["PreddefinedParentTVD_Id"];
				return o != null ? (int?)Convert.ToInt32(o) : null;
			}
			set { ViewState["PreddefinedParentTVD_Id"] = value; }
		}


		/// <summary>
		/// Extended mod pre zadavanie telefonneho cisla podla locale daneho pouzivatela.
		/// </summary>
		public bool ExtendedPhoneMode
		{
			get
			{
				object o = ViewState["ExtendedPhoneMode"];
				return o != null ? (bool)Convert.ToBoolean(o) : false;
			}
			set { ViewState["ExtendedPhoneMode"] = value; }
		}

		#endregion


		public string AutocompleteCityUrl
		{
			get
			{
				object o = ViewState["AutocompleteCityUrl"];
				return o != null ? o.ToString() : null;
			}
			set { ViewState["AutocompleteCityUrl"] = value; }
		}

		#region Protected overrides
		protected override void CreateChildControls()
		{
			//View State je povoleny iba v editacnom rezime
			this.EnableViewState = this.IsEditing;

			//Vytvorenie settings ak neexistuje
			if (this.Settings == null)
				this.Settings = new ControlSettings();

			base.CreateChildControls();

			Control detailControl = CreateDetailControl();
			if (detailControl != null)
			{
				this.Controls.Add(detailControl);
			}

			//Binding
			this.isNew = this.OrganizationId == null;
			if (!this.OrganizationId.HasValue)
			{
				this.organization = new Organization();
				if (!string.IsNullOrEmpty(this.PreddefinedName))
					this.organization.Name = this.PreddefinedName;
				if (!string.IsNullOrEmpty(this.PreddefinedEmail))
					this.organization.ContactEmail = this.PreddefinedEmail;

				this.organization.ParentId = this.PreddefinedParentTVD_Id;
			}
			else
				this.organization = Storage<Organization>.ReadFirst(new Organization.ReadById { OrganizationId = this.OrganizationId.Value });


			//List<Organization> listOrg = Storage<Organization>.Read();
			//if ( Security.IsLogged( false ) )
			//{
			//    if ( Security.IsInRole( Eurona.Common.DAL.Entities.Role.ADMINISTRATOR ) || Security.IsInRole( Eurona.Common.DAL.Entities.Role.OPERATOR ) )
			//        listOrg.Insert( 0, new Organization { Id = 0, TVD_Id = 0 } );
			//}

			//this.ddlParent.DataSource = listOrg;
			//this.ddlParent.DataTextField = "Name";
			//this.ddlParent.DataValueField = "TVD_Id";

			//this.cbTopManager.Checked = Convert.ToBoolean(this.organization.TopManager);

			if (!IsPostBack)
			{
				//this.ddlParent.DataBind();
				this.ddlPF.DataBind();
				this.ddlStatut.DataBind();
				this.ddlRegion.DataBind();

				this.lblCode.Text = this.organization.Code;
				this.txtId1.Text = this.organization.Id1;
				this.txtId2.Text = this.organization.Id2;
				this.txtId3.Text = this.organization.Id3;

				this.cbVATPayment.Checked = this.organization.VATPayment;
				if (this.organization.ParentId.HasValue)
				{
					Organization parentOrg = Storage<Organization>.ReadFirst(new Organization.ReadByTVDId { TVD_Id = this.organization.ParentId.Value });
					if (parentOrg != null)
						this.txtParent.Text = parentOrg.Code;
				}
				//if ( this.organization.ParentId.HasValue ) this.ddlParent.SelectedValue = this.organization.ParentId.Value.ToString();

				this.txtName.Text = this.organization.Name;
				this.txtWeb.Text = this.organization.Web;
				this.txtNotes.Text = this.organization.Notes;

				this.txtContactEmail.Text = this.organization.ContactEmail;
				this.txtContactMobil.Text = this.organization.ContactMobile;
				this.txtContactPhone.Text = this.organization.ContactPhone;

                //this.txtFAX.Text = this.organization.FAX;
                //this.txtSkype.Text = this.organization.Skype;
                //this.txtICQ.Text = this.organization.ICQ;
				this.dtpContactBirthDay.Value = this.organization.ContactBirthDay;
                //this.txtContactCardId.Text = this.organization.ContactCardId;
                //this.txtContactWorkPhone.Text = this.organization.ContactWorkPhone;
				this.txtPredmetCinnosti.Text = this.organization.PredmetCinnosti;

				if (this.ddlRegion.Items.FindByValue(this.organization.RegionCode) != null)
					this.ddlRegion.SelectedValue = this.organization.RegionCode;

				if (this.ddlPF.Items.FindByValue(this.organization.PF) != null)
					this.ddlPF.SelectedValue = this.organization.PF;

				if (this.ddlStatut.Items.FindByValue(this.organization.Statut) != null)
					this.ddlStatut.SelectedValue = this.organization.Statut;

				this.registeredAddress.AddressId = this.organization.RegisteredAddressId;
				this.correspondenceAddress.AddressId = this.organization.CorrespondenceAddressId;
				this.invoicingAddress.AddressId = this.organization.InvoicingAddressId;
				this.bankContact.BankContactId = this.organization.BankContactId;

				this.contactPerson.PersonId = this.organization.ContactPersonId;

				#region Disable Send button and js validation
				StringBuilder sb = new StringBuilder();
				sb.Append("if (typeof(Page_ClientValidate) == 'function') { ");
				sb.Append("var oldPage_IsValid = Page_IsValid; var oldPage_BlockSubmit = Page_BlockSubmit;");
				sb.Append("if (Page_ClientValidate('" + btnSave.ValidationGroup + "') == false) {");
				sb.Append(" Page_IsValid = oldPage_IsValid; Page_BlockSubmit = oldPage_BlockSubmit; return false; }} ");

				//change button text and disable it
				sb.AppendFormat("this.value = '{0}...';", this.btnSave.Text);
				sb.Append("this.disabled = true;");
				sb.Append(Page.ClientScript.GetPostBackEventReference(this.btnSave, null) + ";");
				sb.Append("return true;");
				string submit_button_onclick_js = sb.ToString();
				btnSave.Attributes.Add("onclick", submit_button_onclick_js);
				#endregion

				this.DataBind();
			}

            if (OnChildControlsCreated != null)
                OnChildControlsCreated(this, new EventArgs());
		}
		#endregion

		#region Public Styles Properties
		public string CssRoundPanel { get; set; }
		#endregion

		#region Private methods & handlers
		/// <summary>
		/// Vytvori Control Adresy
		/// </summary>
		private Control CreateDetailControl()
		{
			this.lblCode = new Label();
			this.lblCode.ID = "lblCode";
			this.lblCode.Width = Unit.Pixel(100);

			this.txtId1 = new TextBox();
			this.txtId1.ID = "txtId1";
			this.txtId1.Width = Unit.Pixel(100);

			this.txtId2 = new TextBox();
			this.txtId2.ID = "txtId2";
			this.txtId2.Width = Unit.Pixel(100);

			this.txtId3 = new TextBox();
			this.txtId3.ID = "txtId3";
			this.txtId3.Width = Unit.Pixel(100);

			this.txtName = new TextBox();
			this.txtName.ID = "txtName";
			this.txtName.Width = Unit.Pixel(200);

			this.txtWeb = new TextBox();
			this.txtWeb.ID = "txtWeb";
			this.txtWeb.Width = Unit.Pixel(200);

			this.txtNotes = new TextBox();
			this.txtNotes.ID = "txtNotes";
			this.txtNotes.Width = Unit.Percentage(100);

			this.cbVATPayment = new CheckBox();
			this.cbVATPayment.ID = "cbVATPayment";
			this.cbVATPayment.Width = Unit.Percentage(100);
			this.cbVATPayment.Enabled = false;
			this.cbVATPayment.Text = Resources.Controls.OrganizationControl_VATPayment_Description;
			this.cbVATPayment.TextAlign = TextAlign.Right;

			//this.cbTopManager = new CheckBox();
			//this.cbTopManager.ID = "cbTopManager";
			//this.cbTopManager.Width = Unit.Percentage(100);

			//this.ddlParent = new DropDownList();
			//this.ddlParent.ID = "ddlParent";
			//this.ddlParent.Width = Unit.Percentage( 100 );
			this.txtParent = new TextBox();
			this.txtParent.ID = "txtParent";
			this.txtParent.Width = Unit.Percentage(100);

			this.txtContactEmail = new TextBox();
			this.txtContactEmail.ID = "txtContactEmail";
			this.txtContactEmail.Width = Unit.Percentage(100);

			this.txtContactPhone = new TextBox();
			this.txtContactPhone.ID = "txtContactPhone";
			this.txtContactPhone.Width = Unit.Percentage(100);

			this.txtContactMobil = new TextBox();
			this.txtContactMobil.ID = "txtContactMobil";
			this.txtContactMobil.Width = Unit.Percentage(100);

            //this.txtFAX = new TextBox();
            //this.txtFAX.ID = "txtFAX";
            //this.txtFAX.Width = Unit.Percentage(100);

            //this.txtSkype = new TextBox();
            //this.txtSkype.ID = "txtSkype";
            //this.txtSkype.Width = Unit.Percentage(100);

            //this.txtICQ = new TextBox();
            //this.txtICQ.ID = "txtICQ";
            //this.txtICQ.Width = Unit.Percentage(100);

			this.dtpContactBirthDay = new ASPxDatePicker();
			this.dtpContactBirthDay.ID = "dtpContactBirthDay";
			this.dtpContactBirthDay.Width = Unit.Percentage(100);

            //this.txtContactCardId = new TextBox();
            //this.txtContactCardId.ID = "txtContactCardId";
            //this.txtContactCardId.Width = Unit.Percentage(100);

            //this.txtContactWorkPhone = new TextBox();
            //this.txtContactWorkPhone.ID = "txtContactWorkPhone";
            //this.txtContactWorkPhone.Width = Unit.Percentage(100);

			this.ddlRegion = new DropDownList();
			this.ddlRegion.ID = "ddlRegion";
			this.ddlRegion.Width = Unit.Percentage(100);
			this.ddlRegion.Items.AddRange(new Hepler().GetRegions().ToArray());
            ListItem itemEmpty = new ListItem(string.Empty, string.Empty);
            this.ddlRegion.Items.Insert(0, itemEmpty);

			this.ddlPF = new DropDownList();
			this.ddlPF.ID = "ddlPF";
			this.ddlPF.Width = Unit.Percentage(100);
			this.ddlPF.Items.Add(new ListItem { Value = "F", Text = Resources.Controls.OrganizationControl_PF_F });
			this.ddlPF.Items.Add(new ListItem { Value = "P", Text = Resources.Controls.OrganizationControl_PF_P });

			this.ddlStatut = new DropDownList();
			this.ddlStatut.ID = "ddlStatut";
			this.ddlStatut.Width = Unit.Percentage(100);
			this.ddlStatut.Items.Add(new ListItem { Value = "NRZ", Text = "NRZ" });
			this.ddlStatut.Items.Add(new ListItem { Value = "NRP", Text = "NRP" });

			this.txtPredmetCinnosti = new TextBox();
			this.txtPredmetCinnosti.ID = "txtPredmetCinnosti";
			this.txtPredmetCinnosti.Width = Unit.Percentage(100);

			this.registeredAddress = new AddressControl();
			this.registeredAddress.ID = "registeredAddress";
			this.registeredAddress.IsEditing = this.IsEditing;
			this.registeredAddress.Settings = this.Settings.RegisteredAddressSettings;
			this.registeredAddress.AutocompleteCityUrl = this.AutocompleteCityUrl;

			this.cbCorrespondenceAddressAsRegistered = new CheckBox();
			this.cbCorrespondenceAddressAsRegistered.ID = "cbCorrespondenceAddressAsRegistered";
			this.cbCorrespondenceAddressAsRegistered.Text = CMS.Resources.Controls.OrganizationControl_EqualWithRegisteredAdress;
			this.cbCorrespondenceAddressAsRegistered.AutoPostBack = true;
			this.cbCorrespondenceAddressAsRegistered.Font.Bold = true;
			this.cbCorrespondenceAddressAsRegistered.CheckedChanged += new EventHandler(cbCorrespondenceAddressAsRegistered_CheckedChanged);
			this.cbCorrespondenceAddressAsRegistered.CssClass = "checkBox";
			this.cbCorrespondenceAddressAsRegistered.CausesValidation = false;

			this.correspondenceAddress = new AddressControl();
			this.correspondenceAddress.ID = "correspondenceAddress";
			this.correspondenceAddress.IsEditing = this.IsEditing;
			this.correspondenceAddress.Settings = this.Settings.CorrespondenceAddressSettings;
			this.correspondenceAddress.AutocompleteCityUrl = this.AutocompleteCityUrl;

			this.cbInvoicingAddressAsRegistered = new CheckBox();
			this.cbInvoicingAddressAsRegistered.ID = "cbInvoicingAddressAsRegistered";
			this.cbInvoicingAddressAsRegistered.Text = CMS.Resources.Controls.OrganizationControl_EqualWithRegisteredAdress;
			this.cbInvoicingAddressAsRegistered.AutoPostBack = true;
			this.cbInvoicingAddressAsRegistered.Font.Bold = true;
			this.cbInvoicingAddressAsRegistered.CheckedChanged += new EventHandler(cbInvoicingAddressAsRegistered_CheckedChanged);
			this.cbInvoicingAddressAsRegistered.CssClass = "checkBox";
			this.cbInvoicingAddressAsRegistered.CausesValidation = false;

			this.invoicingAddress = new AddressControl();
			this.invoicingAddress.ID = "invoicingAddress";
			this.invoicingAddress.IsEditing = this.IsEditing;
			this.invoicingAddress.Settings = this.Settings.InvoicingAddressSettings;
			this.invoicingAddress.AutocompleteCityUrl = this.AutocompleteCityUrl;

			this.bankContact = new BankContactControl();
			this.bankContact.ID = "bankContact";
			this.bankContact.IsEditing = this.IsEditing;
			this.bankContact.Settings = this.Settings.BankContactSettings;

			this.contactPerson = new PersonControl();
			this.contactPerson.ID = "contactPerson";
			//this.contactPerson.CssRoundPanel = this.CssRoundPanel;
			this.contactPerson.IsEditing = this.IsEditing;
			this.contactPerson.HideSaveCancel = true;
			this.contactPerson.Settings = this.Settings.ContactPersonSettings;
			this.contactPerson.ExtendedPhoneMode = this.ExtendedPhoneMode;

			this.btnSave = new Button();
			this.btnSave.CausesValidation = true;
			this.btnSave.Text = String.IsNullOrEmpty(SaveButtonText) ? CMS.Resources.Controls.SaveButton_Text : SaveButtonText;
			this.btnSave.Click += new EventHandler(OnSave);
			this.btnCancel = new Button();
			this.btnCancel.CausesValidation = false;
			this.btnCancel.Text = String.IsNullOrEmpty(CancelButtonText) ? CMS.Resources.Controls.CancelButton_Text : CancelButtonText;
			this.btnCancel.Click += new EventHandler(OnCancel);

			Table mainTable = new Table();
			mainTable.Width = this.Width;
			mainTable.Height = this.Height;

			#region Row 1
			Table table = new Table();
			table.Width = Unit.Percentage(100);
			//Code
			TableRow row = null;
			row = new TableRow();
			AddControlToRow(row, Eurona.Common.Resources.Controls.OrganizationControl_Code, this.lblCode, 0, false);
			table.Rows.Add(row);
			this.lblCode.Enabled = false;

			//Id1
			row = null;
			if (this.Settings.Visibility.Id1)
			{
				row = new TableRow();
				AddControlToRow(row, CMS.Resources.Controls.OrganizationControl_Id1, this.txtId1, 0, this.Settings.Require.Id1);
				table.Rows.Add(row);
			}

			//Id2
			if (this.Settings.Visibility.Id2)
			{
				row = new TableRow();
				AddControlToRow(row, CMS.Resources.Controls.OrganizationControl_Id2, this.txtId2, 0, this.Settings.Require.Id2);
				table.Rows.Add(row);
			}

			//Id3
			if (this.Settings.Visibility.Id3)
			{
				row = new TableRow();
				AddControlToRow(row, CMS.Resources.Controls.OrganizationControl_Id3, this.txtId3, 0, this.Settings.Require.Id3);
				table.Rows.Add(row);
			}

			//Name
			if (this.Settings.Visibility.Name)
			{
				row = new TableRow();
				AddControlToRow(row, CMS.Resources.Controls.OrganizationControl_Name, this.txtName, 0, this.Settings.Require.Name);
				table.Rows.Add(row);
			}
            /*
			//Web
			if (this.Settings.Visibility.Web)
			{
				row = new TableRow();
				AddControlToRow(row, CMS.Resources.Controls.OrganizationControl_Web, this.txtWeb, 0, this.Settings.Require.Web);
				table.Rows.Add(row);
			}

			//FAX
			if (this.Settings.Visibility.FAX)
			{
				row = new TableRow();
				AddControlToRow(row, Resources.Controls.OrganizationControl_FAX, this.txtFAX, 0, this.Settings.Require.FAX);
				table.Rows.Add(row);
			}
			//Skype
			if (this.Settings.Visibility.Skype)
			{
				row = new TableRow();
				AddControlToRow(row, Resources.Controls.OrganizationControl_Skype, this.txtSkype, 0, this.Settings.Require.Skype);
				table.Rows.Add(row);
			}
			//ICQ
			if (this.Settings.Visibility.ICQ)
			{
				row = new TableRow();
				AddControlToRow(row, Resources.Controls.OrganizationControl_ICQ, this.txtICQ, 0, this.Settings.Require.ICQ);
				table.Rows.Add(row);
			}
			//ContactCardId
			if (this.Settings.Visibility.ContactCardId)
			{
				row = new TableRow();
				AddControlToRow(row, Resources.Controls.PersonControl_CardId, this.txtContactCardId, 0, this.Settings.Require.ContactCardId);
				table.Rows.Add(row);
			}
            */
			//ContactBirthDay
			if (this.Settings.Visibility.ContactBirthDay)
			{
				row = new TableRow();
				AddControlToRow(row, Resources.Controls.PersonControl_BirthDay, this.dtpContactBirthDay, 0, this.Settings.Require.ContactBirthDay);
				table.Rows.Add(row);
			}
			//Email
			if (this.Settings.Visibility.ContactEmail)
			{
				row = new TableRow();
				AddControlToRow(row, CMS.Resources.Controls.PersonControl_Email, this.txtContactEmail, 0, this.Settings.Require.ContactEmail);
				table.Rows.Add(row);
				row.Cells[row.Cells.Count - 1].Controls.Add(CreateEmailValidatorControl(txtContactEmail.ID));
			}
			//Phone
			if (this.Settings.Visibility.ContactPhone)
			{
				row = new TableRow();
				AddControlToRow(row, CMS.Resources.Controls.PersonControl_Phone, this.txtContactPhone, 0, this.Settings.Require.ContactPhone);
				table.Rows.Add(row);
			}
			//Mobil
			if (this.Settings.Visibility.ContactMobil)
			{
				row = new TableRow();
				AddControlToRow(row, CMS.Resources.Controls.PersonControl_Mobile, this.txtContactMobil, 0, this.Settings.Require.ContactMobil);
				table.Rows.Add(row);
				RegularExpressionValidator rev = CreatePhoneValidatorControl(txtContactMobil.ID);
				rev.ValidationExpression = @"^\+[0-9\s]{0,15}$";
				rev.ErrorMessage = "+XXXXXXXXXXXX";
				row.Cells[row.Cells.Count - 1].Controls.Add(rev);
			}

            ////ContactWorkPhone
            //if (this.Settings.Visibility.ContactWorkPhone)
            //{
            //    row = new TableRow();
            //    AddControlToRow(row, Resources.Controls.PersonControl_WorkPhone, this.txtContactWorkPhone, 0, this.Settings.Require.ContactWorkPhone);
            //    table.Rows.Add(row);
            //}

			//PF
			if (this.Settings.Visibility.PF)
			{
				row = new TableRow();
				AddControlToRow(row, Resources.Controls.PersonControl_PF, this.ddlPF, 0, this.Settings.Require.PF);
				table.Rows.Add(row);
			}
			//Statut
			if (this.Settings.Visibility.Statut)
			{
				row = new TableRow();
				AddControlToRow(row, "Statut :", this.ddlStatut, 0, this.Settings.Require.Statut);
				table.Rows.Add(row);
			}
			//PF
			if (this.Settings.Visibility.RegionCode)
			{
				row = new TableRow();
				AddControlToRow(row, Resources.Controls.PersonControl_RegionCode, this.ddlRegion, 0, this.Settings.Require.RegionCode);
				table.Rows.Add(row);
			}
			//PredmetCinnosti
			if (this.Settings.Visibility.PredmetCinnosti)
			{
				row = new TableRow();
				AddControlToRow(row, "Předmět činnosti :", this.txtPredmetCinnosti, 0, this.Settings.Require.PredmetCinnosti);
				table.Rows.Add(row);
			}
			//Notes
			if (this.Settings.Visibility.Notes)
			{
				row = new TableRow();
				AddControlToRow(row, CMS.Resources.Controls.OrganizationControl_Notes, this.txtNotes, 0, this.Settings.Require.Notes);
				table.Rows.Add(row);
			}

			//VATPayment
			row = new TableRow();
			AddControlToRow(row, Eurona.Common.Resources.Controls.OrganizationControl_VATPayment, this.cbVATPayment, 0, false, true);
			this.cbVATPayment.Enabled = false;
			table.Rows.Add(row);
			if (Security.IsLogged(false))
			{
				if (Security.IsInRole(Eurona.Common.DAL.Entities.Role.ADMINISTRATOR) || Security.IsInRole(Eurona.Common.DAL.Entities.Role.OPERATOR))
					this.cbVATPayment.Enabled = true;
			}

            /*
			//TopManager           
			row = new TableRow();
			AddControlToRow(row, Eurona.Common.Resources.Controls.OrganizationControl_TopManager, this.cbTopManager, 0, false);
			this.cbTopManager.Enabled = false;
			if (Security.IsLogged(false))
			{
				if (Security.IsInRole(Eurona.Common.DAL.Entities.Role.ADMINISTRATOR) || Security.IsInRole(Eurona.Common.DAL.Entities.Role.OPERATOR))
					this.cbTopManager.Enabled = true;
			}
			table.Rows.Add(row);
            */

			//ParentId
			row = new TableRow();
			AddControlToRow(row, Eurona.Common.Resources.Controls.OrganizationControl_Parent, this.txtParent, 0, this.Settings.Require.Parent);
			table.Rows.Add(row);

			//Bank contact
			if (this.Settings.Visibility.BankContact)
			{
				row = new TableRow();
				TableCell cell = new TableCell();
				cell.Width = Unit.Percentage(50);
				cell.Controls.Add(table);
				row.Cells.Add(cell);

				cell = new TableCell();
				cell.VerticalAlign = VerticalAlign.Top;
				cell.Controls.Add(CreateRoundPanel(CMS.Resources.Controls.OrganizationControl_BankContactTitle,	new Control[] { this.bankContact }, true));
				row.Cells.Add(cell);
				mainTable.Rows.Add(row);

                //cell.Controls.Add(new LiteralControl("<div style='vertical-align:bottom; margin: 330px 5px 20px 40px;'><span style='font-weight:bold;color:#eb0a5b;'>V případě změny ŽL, adresy sídla, IČO, DIČ, názvu v ŽL, kontaktujte obchodní oddělení.</span></div>"));
			}

			#endregion

			#region Row 2
			row = new TableRow();
			if (this.Settings.Visibility.RegisteredAddress)
			{
				//TableCell cell = new TableCell();
				//cell.Width = Unit.Percentage( 50 );
				//cell.Controls.Add( table );
				//row.Cells.Add( cell );

				//Registered Address
				TableCell cell = new TableCell();
				cell.Width = Unit.Percentage(50);
				RoundPanel rp = CreateRoundPanel(CMS.Resources.Controls.OrganizationControl_RegisteredAddressTitle,
						new Control[] { this.registeredAddress }, false);

				rp.Style.Add("background-image", "none!Important");
				cell.Controls.Add(rp);
				row.Cells.Add(cell);

				mainTable.Rows.Add(row);
			}
			//Correspondence Address
			if (this.Settings.Visibility.CorrespondenceAddress)
			{
				TableCell cell = new TableCell();
				RoundPanel rp = CreateRoundPanel(CMS.Resources.Controls.OrganizationControl_CorrespondenceAddressTitle,
						new Control[] { this.IsEditing && this.Settings.Visibility.RegisteredAddress ? this.cbCorrespondenceAddressAsRegistered : null, this.correspondenceAddress },
						false);
				rp.Style.Add("background-image", "none!Important");
				cell.Controls.Add(rp);
				row.Cells.Add(cell);
			}

			mainTable.Rows.Add(row);
			#endregion

			#region Row 3
			row = new TableRow();
			//Invoicing Address
			if (this.Settings.Visibility.InvoicingAddress)
			{
				TableCell cell = new TableCell();
				cell.Controls.Add(CreateRoundPanel(CMS.Resources.Controls.OrganizationControl_InvoicingAddressTitle,
						new Control[] { this.IsEditing && this.Settings.Visibility.RegisteredAddress ? this.cbInvoicingAddressAsRegistered : null, this.invoicingAddress },
						false));
				row.Cells.Add(cell);
			}

			//Contact person
			if (this.Settings.Visibility.ContactPerson)
			{
				TableCell cell = new TableCell();
				cell.VerticalAlign = VerticalAlign.Top;
				cell.Controls.Add(CreateRoundPanel(CMS.Resources.Controls.OrganizationControl_ContactPersonTitle,
						new Control[] { this.contactPerson }, true));
				row.Cells.Add(cell);
			}
			if (row.Cells.Count != 0) mainTable.Rows.Add(row);
			#endregion

			if (this.IsEditing)
			{
				//Save Cancel Buttons
				row = new TableRow();
				TableCell cell = new TableCell();
				cell.ColumnSpan = 2;
				cell.Controls.Add(this.btnSave);
				cell.Controls.Add(this.btnCancel);
				row.Cells.Add(cell);
				mainTable.Rows.Add(row);
			}

			return mainTable;
		}

		private void AddControlToRow(TableRow row, string labelText, Control control, int controlColspan, bool required)
		{
			(control as WebControl).Enabled = this.IsEditing;
            if (!(control as WebControl).Enabled)
            {
                (control as WebControl).BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            }

			TableCell cell = new TableCell();
			cell.CssClass = required && this.IsEditing ? "form_label_required" : "form_label";
			cell.Text = labelText;
			row.Cells.Add(cell);

			cell = new TableCell();
			cell.CssClass = "form_control";
			cell.Controls.Add(control);
			if (controlColspan != 0)
				cell.ColumnSpan = controlColspan;
			if (required && this.IsEditing) cell.Controls.Add(base.CreateRequiredFieldValidatorControl(control.ID));
			row.Cells.Add(cell);
		}

		private void AddControlToRow(TableRow row, string labelText, Control control, int controlColspan, bool required, bool bWrapControl)
		{
			(control as WebControl).Enabled = this.IsEditing;
            if (!(control as WebControl).Enabled)
            {
                (control as WebControl).BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            }

			TableCell cell = new TableCell();
			cell.CssClass = required && this.IsEditing ? "form_label_required" : "form_label";
			cell.Text = labelText;
			row.Cells.Add(cell);

			cell = new TableCell();
			cell.CssClass = bWrapControl ? "form_control_wrap" : "form_control";
			cell.Controls.Add(control);
			if (controlColspan != 0)
				cell.ColumnSpan = controlColspan;
			if (required && this.IsEditing) cell.Controls.Add(base.CreateRequiredFieldValidatorControl(control.ID));
			row.Cells.Add(cell);
		}

		private RoundPanel CreateRoundPanel(string title, Control[] controls, bool fillHeader)
		{
			RoundPanel rp = new RoundPanel();
			rp.CssClass = this.CssRoundPanel;
			rp.Text = title;
			rp.FillHeader = fillHeader;

			foreach (Control control in controls)
			{
				if (control == null) continue;
				rp.Controls.Add(control);
			}
			return rp;
		}

		void cbCorrespondenceAddressAsRegistered_CheckedChanged(object sender, EventArgs e)
		{
			if (this.cbCorrespondenceAddressAsRegistered.Checked)
			{
				CMS.Entities.Address current = new CMS.Entities.Address();
				current = this.registeredAddress.UpdateEntityFromUI(current);
				this.correspondenceAddress.UpdateUIFromEntity(current);
			}
		}

		void cbInvoicingAddressAsRegistered_CheckedChanged(object sender, EventArgs e)
		{
			if (this.cbInvoicingAddressAsRegistered.Checked)
			{
				CMS.Entities.Address current = new CMS.Entities.Address();
				current = this.registeredAddress.UpdateEntityFromUI(current);
				this.invoicingAddress.UpdateUIFromEntity(current);
			}
		}
		#endregion

		#region Public methods
		private void UpdateEntityFromUI(Organization organization)
		{
			organization.Code = this.lblCode.Text;

			organization.Id1 = this.txtId1.Text;
			organization.Id2 = this.txtId2.Text;
			organization.Id3 = this.txtId3.Text;

			organization.Name = this.txtName.Text;
			organization.Web = this.txtWeb.Text;
			organization.Notes = this.txtNotes.Text;

			organization.VATPayment = this.cbVATPayment.Checked;
			//organization.TopManager = this.cbTopManager.Checked ? 1 : 0;

			Organization parentOrg = Storage<Organization>.ReadFirst(new Organization.ReadByCode { Code = this.txtParent.Text });
			if (parentOrg != null) organization.ParentId = parentOrg.TVD_Id;
			else organization.ParentId = null;

			organization.ContactEmail = this.txtContactEmail.Text;
			organization.ContactPhone = this.txtContactPhone.Text;
			organization.ContactMobile = this.txtContactMobil.Text;

            //organization.FAX = this.txtFAX.Text;
            //organization.Skype = this.txtSkype.Text;
            //organization.ICQ = this.txtICQ.Text;
			organization.ContactBirthDay = (DateTime?)this.dtpContactBirthDay.Value;
            //organization.ContactCardId = this.txtContactCardId.Text;
            //organization.ContactWorkPhone = this.txtContactWorkPhone.Text;
			organization.RegionCode = this.ddlRegion.Text;
			organization.PF = this.ddlPF.Text;

			organization.Statut = this.ddlStatut.Text;
			organization.PredmetCinnosti = this.txtPredmetCinnosti.Text;
			//if ( !string.IsNullOrEmpty( this.txtId1.Text ) )
			//    organization.Statut = "NRP";
			//else
			//    organization.Statut = "NRZ";
		}

		public virtual Organization UpdateOrganization(Organization organization)
		{
			UpdateEntityFromUI(organization);
			Storage<Organization>.Update(organization);

			if (this.Settings.Visibility.RegisteredAddress)
				this.registeredAddress.UpdateAddress(organization.RegisteredAddress);

			if (this.Settings.Visibility.CorrespondenceAddress)
				this.correspondenceAddress.UpdateAddress(organization.CorrespondenceAddress);

			if (this.Settings.Visibility.InvoicingAddress)
				this.invoicingAddress.UpdateAddress(organization.InvoicingAddress);

			if (this.Settings.Visibility.BankContact)
				this.bankContact.UpdateBankContact(organization.BankContact);

			if (this.Settings.Visibility.ContactPerson)
				this.contactPerson.UpdatePerson(organization.ContactPerson);

			return organization;
		}

		public virtual Organization CreateOrganization(Organization organization)
		{
			if (this.AccountId.HasValue)
				organization.AccountId = this.AccountId.Value;

			UpdateEntityFromUI(organization);
			organization = Storage<Organization>.Create(organization);
			this.OrganizationId = organization.Id;
			this.isNew = false;

			if (this.Settings.Visibility.RegisteredAddress)
				this.registeredAddress.UpdateAddress(organization.RegisteredAddress);

			if (this.Settings.Visibility.CorrespondenceAddress)
				this.correspondenceAddress.UpdateAddress(organization.CorrespondenceAddress);

			if (this.Settings.Visibility.InvoicingAddress)
				this.invoicingAddress.UpdateAddress(organization.InvoicingAddress);

			if (this.Settings.Visibility.BankContact)
				this.bankContact.UpdateBankContact(organization.BankContact);

			if (this.Settings.Visibility.ContactPerson)
				this.contactPerson.UpdatePerson(organization.ContactPerson);

			return organization;
		}
		#endregion

		#region Event handlers
		void OnSave(object sender, EventArgs e)
		{
			if (this.isNew) CreateOrganization(this.organization);
			else UpdateOrganization(this.organization);

			if (!organization.ParentId.HasValue && organization.TopManager == 0)
			{
				string js = string.Format("alert('{0}');", "Musíte vybrat nadřízeného anebo označit uživatele jako TOP manažéra!");
				this.btnSave.Page.ClientScript.RegisterStartupScript(this.btnSave.Page.GetType(), "addValidateOrganization", js, true);
				return;
			}

			//Validate PSČ
			string message = Eurona.Common.PSCHelper.ValidatePSCByPSC(organization.RegisteredAddress.Zip, organization.RegisteredAddress.City, organization.RegisteredAddress.State);
			if (message != string.Empty)
			{
				string js = string.Format("alert('Sídlo : {0}');", message);
				this.btnSave.Page.ClientScript.RegisterStartupScript(this.btnSave.Page.GetType(), "addValidateOrganization", js, true);
				return;
			}

			message = Eurona.Common.PSCHelper.ValidatePSCByPSC(organization.CorrespondenceAddress.Zip, organization.CorrespondenceAddress.City, organization.CorrespondenceAddress.State);
			if (message != string.Empty)
			{
				string js = string.Format("alert('Korespondenční adresa : {0}');", message);
				this.btnSave.Page.ClientScript.RegisterStartupScript(this.btnSave.Page.GetType(), "addValidateOrganization", js, true);
				return;
			}

			if (!SyncTVDUser(this.organization)) return;

			if (SaveCompleted != null)
				SaveCompleted(this, EventArgs.Empty);

			if (!string.IsNullOrEmpty(this.ReturnUrl))
				Response.Redirect(this.ReturnUrl);
		}

		void OnCancel(object sender, EventArgs e)
		{
			if (Canceled != null)
				Canceled(this, EventArgs.Empty);

			if (!string.IsNullOrEmpty(this.ReturnUrl))
				Response.Redirect(this.ReturnUrl);
		}

		protected virtual bool SyncTVDUser(Organization organization)
		{
			return true;
		}
		#endregion

		#region Settings classes
		[Serializable()]
		public class ControlSettings
		{
			public ControlSettings()
			{
				this.Visibility = new Visibility();
				this.Require = new Require();

				this.RegisteredAddressSettings = new AddressControl.ControlSettings();
				this.CorrespondenceAddressSettings = new AddressControl.ControlSettings();
				this.InvoicingAddressSettings = new AddressControl.ControlSettings();
				this.BankContactSettings = new BankContactControl.ControlSettings();
				this.ContactPersonSettings = new PersonControl.ControlSettings();
			}

			public Visibility Visibility { get; set; }
			public Require Require { get; set; }
			public AddressControl.ControlSettings RegisteredAddressSettings { get; set; }
			public AddressControl.ControlSettings CorrespondenceAddressSettings { get; set; }
			public AddressControl.ControlSettings InvoicingAddressSettings { get; set; }
			public BankContactControl.ControlSettings BankContactSettings { get; set; }
			public PersonControl.ControlSettings ContactPersonSettings { get; set; }

		}

		[Serializable()]
		public class Visibility
		{
			public Visibility()
			{
				bool defaultValue = true;
				this.Id1 = defaultValue;
				this.Id2 = defaultValue;
				this.Id3 = defaultValue;
				this.Name = defaultValue;
				this.Web = defaultValue;
				this.Notes = defaultValue;
				this.ContactEmail = defaultValue;
				this.ContactPhone = defaultValue;
				this.ContactMobil = defaultValue;

				this.FAX = defaultValue;
				this.Skype = defaultValue;
				this.ICQ = defaultValue;
				this.ContactBirthDay = defaultValue;
				this.ContactCardId = defaultValue;
				this.ContactWorkPhone = defaultValue;
				this.PF = defaultValue;
				this.Statut = defaultValue;
				this.RegionCode = defaultValue;
				this.PredmetCinnosti = defaultValue;

				this.RegisteredAddress = defaultValue;
				this.CorrespondenceAddress = defaultValue;
				this.InvoicingAddress = defaultValue;
				this.BankContact = defaultValue;
				this.ContactPerson = defaultValue;
			}

			public bool Id1 { get; set; }
			public bool Id2 { get; set; }
			public bool Id3 { get; set; }
			public bool Name { get; set; }
			public bool Web { get; set; }
			public bool Notes { get; set; }
			public bool ContactEmail { get; set; }
			public bool ContactPhone { get; set; }
			public bool ContactMobil { get; set; }

			public bool FAX { get; set; }
			public bool Skype { get; set; }
			public bool ICQ { get; set; }
			public bool ContactBirthDay { get; set; }
			public bool ContactCardId { get; set; }
			public bool ContactWorkPhone { get; set; }
			public bool PF { get; set; }
			public bool Statut { get; set; }
			public bool RegionCode { get; set; }
			public bool PredmetCinnosti { get; set; }

			public bool RegisteredAddress { get; set; }
			public bool CorrespondenceAddress { get; set; }
			public bool InvoicingAddress { get; set; }
			public bool BankContact { get; set; }
			public bool ContactPerson { get; set; }
		}

		[Serializable()]
		public class Require
		{
			public Require()
			{
				bool defaultValue = false;
				this.Id1 = defaultValue;
				this.Id2 = defaultValue;
				this.Id3 = defaultValue;
				this.Web = defaultValue;
				this.Name = true;
				this.Notes = defaultValue;
				this.ContactEmail = defaultValue;
				this.ContactPhone = defaultValue;
				this.ContactMobil = defaultValue;

				this.FAX = defaultValue;
				this.Skype = defaultValue;
				this.ICQ = defaultValue;
				this.ContactBirthDay = defaultValue;
				this.ContactCardId = defaultValue;
				this.ContactWorkPhone = defaultValue;
				this.PF = defaultValue;
				this.Statut = defaultValue;
				this.PredmetCinnosti = defaultValue;
				this.RegionCode = defaultValue;
				this.Parent = true;
			}

			public bool Id1 { get; set; }
			public bool Id2 { get; set; }
			public bool Id3 { get; set; }
			public bool Name { get; set; }
			public bool Web { get; set; }
			public bool Notes { get; set; }
			public bool ContactEmail { get; set; }
			public bool ContactPhone { get; set; }
			public bool ContactMobil { get; set; }

			public bool FAX { get; set; }
			public bool Skype { get; set; }
			public bool ICQ { get; set; }
			public bool ContactBirthDay { get; set; }
			public bool ContactCardId { get; set; }
			public bool ContactWorkPhone { get; set; }
			public bool PF { get; set; }
			public bool Statut { get; set; }
			public bool PredmetCinnosti { get; set; }
			public bool RegionCode { get; set; }
			public bool Parent { get; set; }
		}


		public class Hepler
		{
			public virtual List<ListItem> GetRegions(string state="")
			{
				List<ListItem> list = new List<ListItem>();
                ListItem li;
                li = new ListItem { Value = "NEVIM", Text = "Neznámá oblast" }; list.Add(li);//NEVIM;Neznámá oblast  
                if (state == "" || state == "CZ") {
                    li = new ListItem { Value = "1 jč", Text = "Jihočeský" }; list.Add(li);//1 jč ;Jihočeský                
                    li = new ListItem { Value = "10 pl", Text = "Plzeňský" }; list.Add(li);//10 pl;Plzeňský                 
                    li = new ListItem { Value = "11 sč", Text = "Středočeský" }; list.Add(li);//11 sč;Středočeský              
                    li = new ListItem { Value = "12 ús", Text = "Ústecký" }; list.Add(li);//12 ús;Ústecký                  
                    li = new ListItem { Value = "13 zl", Text = "Zlínský" }; list.Add(li);//13 zl;Zllínský      
                    li = new ListItem { Value = "14 pr", Text = "Hlavní město Praha" }; list.Add(li);//14 pr	Hlavní město Praha 
                    li = new ListItem { Value = "2 jm", Text = "Jihomoravský" }; list.Add(li);//2 jm ;Jihomoravský             
                    li = new ListItem { Value = "3 kv", Text = "Karlovarský" }; list.Add(li);//3 kv ;Karlovarský              
                    li = new ListItem { Value = "4 vys", Text = "Vysočina" }; list.Add(li);//4 vys;Vysočina                 
                    li = new ListItem { Value = "5 kh", Text = "Královehradecký" }; list.Add(li);//5 kh ;Královehradecký          
                    li = new ListItem { Value = "6 lb", Text = "Liberecký" }; list.Add(li);//6 lb ;Liberecký                
                    li = new ListItem { Value = "7 ms", Text = "Moravskoslezský" }; list.Add(li);//7 ms ;Moravskoslezský          
                    li = new ListItem { Value = "8 ol", Text = "Olomoucký" }; list.Add(li);//8 ol ;Olomoucký                
                    li = new ListItem { Value = "9 pc", Text = "Pardubický" }; list.Add(li);//9 pc ;Pardubický           
                }    
                if (state == "" || state == "PL") {
                    li = new ListItem { Value = "P dls", Text = "Dolnoslaskie (50 - 54)" }; list.Add(li);//P dls;Dolnoslaskie (50 - 54)   
                    li = new ListItem { Value = "P k-p", Text = "Kujawsko-Pomor(85)" }; list.Add(li);//P k-p;Kujawsko-Pomor(85)       
                    li = new ListItem { Value = "P l", Text = "Lodzkie (90-94)" }; list.Add(li);//P l  ;Lodzkie (90-94)          
                    li = new ListItem { Value = "P lbe", Text = "Lubelskie (20)" }; list.Add(li);//P lbe;Lubelskie (20)           
                    li = new ListItem { Value = "P maz", Text = "Mazowieckie (00-04,26)" }; list.Add(li);//P maz;Mazowieckie (00-04,26)   
                    li = new ListItem { Value = "P ml", Text = "Malopolskie (30-31)" }; list.Add(li);//P ml ;Malopolskie (30-31)      
                    li = new ListItem { Value = "P op", Text = "Opolskie (45-47,49)" }; list.Add(li);//P op ;Opolskie (45-47,49)      
                    li = new ListItem { Value = "P pdk", Text = "Podkarpackie (35)" }; list.Add(li);//P pdk;Podkarpackie (35)        
                    li = new ListItem { Value = "P pdl", Text = "Podlaskie (15)" }; list.Add(li);//P pdl;Podlaskie (15)           
                    li = new ListItem { Value = "P pom", Text = "Pomorskie (80-81)" }; list.Add(li);//P pom;Pomorskie (80-81)        
                    li = new ListItem { Value = "P sl", Text = "Slaskie (40-44,47) " }; list.Add(li);//P sl ;Slaskie (40-44,47)       
                    li = new ListItem { Value = "P swt", Text = "Swietokrzyskie (25)" }; list.Add(li);//P swt;Swietokrzyskie (25)      
                    li = new ListItem { Value = "P w-m", Text = "Warminsko-mazu (10-11)" }; list.Add(li);//P w-m;Warminsko-mazu (10-11)   
                    li = new ListItem { Value = "P wlk", Text = "Wielkopolskie (60-61)" }; list.Add(li);//P wlk;Wielkopolskie (60-61)    
                    li = new ListItem { Value = "P zp", Text = "Zachodniopomo (70-75)" }; list.Add(li);//P zp ;Zachodniopomo (70-75)    
                    li = new ListItem { Value = "Plbu", Text = "Lubuskie (65)" }; list.Add(li);//Plbu ;Lubuskie (65)        
                }
                if (state == "" || state == "SK") {
                    li = new ListItem { Value = "SK", Text = "Bratislavský" }; list.Add(li);//SK   ;Bratislavský             
                    li = new ListItem { Value = "SK b", Text = "Banskobystrický" }; list.Add(li);//SK b ;Banskobystrický          
                    li = new ListItem { Value = "SK ko", Text = "Košický" }; list.Add(li);//SK ko;Košický                  
                    li = new ListItem { Value = "SK ni", Text = "Nitranský" }; list.Add(li);//SK ni;Nitranský                
                    li = new ListItem { Value = "SK pr", Text = "Prešovský" }; list.Add(li);//SK pr;Prešovský                
                    li = new ListItem { Value = "SK ta", Text = "Trnavský" }; list.Add(li);//SK ta;Trnavský                 
                    li = new ListItem { Value = "SK tr", Text = "Trenčianský" }; list.Add(li);//SK tr;Trenčianský              
                    li = new ListItem { Value = "SK ži", Text = "Žilinský" }; list.Add(li);//SK ži;Žilinský   
                }

				return list;
			}
		}
		#endregion
	}
}
