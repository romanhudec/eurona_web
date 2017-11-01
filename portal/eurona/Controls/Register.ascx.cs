using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Utilities;
using Eurona.DAL.Entities;
using System.Data;
using Telerik.Web.UI;
using Eurona.Common.DAL.Entities;
using System.Text;

namespace Eurona.Controls
{
	public partial class Register : System.Web.UI.UserControl
	{
		public delegate void ContinueHandler(object sender, Account account);
		public event ContinueHandler Continue;

		protected void Page_Load(object sender, EventArgs e)
		{
			//Predplnenie Host name ako login
			if (!IsPostBack)
			{
				if (Session[Eurona.User.Host.HostSecurity.HostNameSessionName] != null)
				{
					string hostName = Session[Eurona.User.Host.HostSecurity.HostNameSessionName].ToString();
					if (!string.IsNullOrEmpty(hostName))
						this.txtLogin.Text = hostName;
				}
			}

			this.advisorRow.Visible = false;
			if (this.IsHost)
			{
				this.advisorRow.Visible = true;
				this.ddlAdvisor.EnableLoadOnDemand = false;
				List<Organization> list = Storage<Organization>.Read();
				this.ddlAdvisor.DataSource = list;
				this.ddlAdvisor.DataBind();
			}

			this.hlObchodniPodminky.NavigateUrl = "~/userfiles/SMLUVNÍ PODMÍNKY CZ.pdf";
			string locale = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
			if (locale == "pl") this.hlObchodniPodminky.NavigateUrl = "~/userfiles/WARUNKI UMOWY PL.pdf";

			this.capcha.ErrorMessage = CMS.Resources.Controls.ForgotPasswordControl_Capcha_ErrorMessage;
			this.capcha.CaptchaTextBoxLabel = CMS.Resources.Controls.ForgotPasswordControl_Capcha_TextBoxLabel;
			this.capcha.Width = Unit.Percentage(100);
			this.capcha.EnableRefreshImage = true;
			this.capcha.CaptchaLinkButtonText = CMS.Resources.Controls.ForgotPasswordControl_Capcha_LinkButtonText;

			//#region js on click vytvaram objednavku
			//StringBuilder sb = new StringBuilder();
			//sb.AppendFormat( "this.value = '{0} ...';", Resources.Strings.RegisterControl_ContinueButton );
			//sb.Append( "this.disabled = true;" );
			//sb.Append( Page.ClientScript.GetPostBackEventReference( this.btnContinue, null ) + ";" );

			//string submit_button_onclick_js = sb.ToString();
			//this.btnContinue.Attributes.Add( "onclick", submit_button_onclick_js );
			//#endregion

			#region Disable Send button and js validation
			StringBuilder sb = new StringBuilder();
			sb.Append("if (typeof(Page_ClientValidate) == 'function') { ");
			sb.Append("var oldPage_IsValid = Page_IsValid; var oldPage_BlockSubmit = Page_BlockSubmit;");
			sb.Append("if (Page_ClientValidate('" + btnContinue.ValidationGroup + "') == false) {");
			sb.Append(" Page_IsValid = oldPage_IsValid; Page_BlockSubmit = oldPage_BlockSubmit; return false; }} ");

			//change button text and disable it
			sb.AppendFormat("this.value = '{0}...';", this.btnContinue.Text);
			sb.Append("this.disabled = true;");
			sb.Append(Page.ClientScript.GetPostBackEventReference(this.btnContinue, null) + ";");
			sb.Append("return true;");
			string submit_button_onclick_js = sb.ToString();
			btnContinue.Attributes.Add("onclick", submit_button_onclick_js);
			#endregion
		}

		protected void OnDdlAdvisor_DataBound(object sender, EventArgs e)
		{
			//set the initial footer label
			((Literal)ddlAdvisor.Footer.FindControl("RadComboItemsCount")).Text = Convert.ToString(ddlAdvisor.Items.Count);
		}

		protected void OnDdlAdvisor_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
		{
			List<Organization> list = Storage<Organization>.Read();
			this.ddlAdvisor.DataSource = list;
			this.ddlAdvisor.DataBind();
		}
		protected void OnDdlAdvisor_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
		{
			//set the Text and Value property of every item
			//here you can set any other properties like Enabled, ToolTip, Visible, etc.
			e.Item.Text = ((Organization)e.Item.DataItem).Name;
			e.Item.Value = ((Organization)e.Item.DataItem).AccountId.ToString();
		}


		/// <summary>
		/// Indikuje či sa na daný účet budú evidovať faktúry.
		/// </summary>
		public bool IsHost
		{
			get { return Convert.ToBoolean(ViewState["IsHost"]); }
			set
			{
				ViewState["IsHost"] = value;
				this.advisorRow.Visible = value;
			}
		}

		protected void OnContinueClick(object sender, EventArgs e)
		{
			this.capcha.Validate();
			if (!this.capcha.IsValid) return;

			if (AccountExists(txtLogin.Text, txtEmail.Text))
			{
				lblAlreadyExists.Visible = true;
				return;
			}

			this.btnContinue.Enabled = false;

			Account account = new Account();
			account.Locale = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
			account.Login = txtLogin.Text;
			account.Email = txtEmail.Text;
			account.Password = Cryptographer.MD5Hash(txtPassword.Text);
			account.Enabled = false;
			account = Storage<Account>.Create(account);
			Session[account.Login] = txtPassword.Text;

			int advisorId = 0;
			Int32.TryParse(this.ddlAdvisor.SelectedValue, out advisorId);
			if (advisorId != 0)
			{
				account.AccountExtension = new AccountExt();
				account.AccountExtension.AccountId = account.Id;
				account.AccountExtension.AdvisorId = advisorId;
				Storage<AccountExt>.Create(account.AccountExtension);
			}
			if (Continue != null) Continue(this, account);
		}

		private bool AccountExists(string login, string email)
		{
			List<Account> exists = Storage<Account>.Read(new Account.ReadByLogin { Login = login });
			if (exists != null && exists.Count == 0)
				exists = Storage<Account>.Read(new Account.ReadByEmail { Email = email });

			return exists != null && exists.Count > 0;
		}
	}
}