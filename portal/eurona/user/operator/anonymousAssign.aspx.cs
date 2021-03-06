﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Common.DAL.Entities;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using Eurona.Controls.UserManagement;

namespace Eurona.User.Operator
{
	public partial class AnonymousAssignPage : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Security.IsLogged(true);
			LoadData(!IsPostBack);
		}

		private void LoadData(bool bind)
		{
			DateTime beforDate = DateTime.Now.AddMonths(-1);
			List<Organization> listNovacci = new List<OrganizationEntity>();
			if( this.cbShowAll.Checked )
				listNovacci = Storage<Organization>.Read(new Organization.ReadByAnonymous { AnonymousRegistration = true, Assigned = true, AssignedAndConfirmed = false });
			else
				listNovacci = Storage<Organization>.Read(new Organization.ReadByAnonymous { AnonymousRegistration = true, Assigned = true, AssignedAndConfirmed = false, CreatedAtYear = beforDate.Year, CreatedAtMonth = beforDate.Month });

			int index = 1;
			foreach (Organization org in listNovacci)
			{
				org.BankContactId = index;
				index++;
			}
			this.rpCekajiciNovacci.DataSource = listNovacci;
			if (bind) this.rpCekajiciNovacci.DataBind();
		}

		protected void OnCheckAllCheckedChanged(object sender, EventArgs e)
		{
			this.LoadData(true);
		}

		protected void OnItemDataBound(object Sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Button btn = (Button)e.Item.FindControl("btnPotvrdit");
				if (btn != null)
				{
					btn.OnClientClick = string.Format("return validatePrijemNovacka({0})", (e.Item.DataItem as Organization).Id);
				}
			}
		}

		protected void OnUlozit(object sender, EventArgs e)
		{
			Button btn = (sender as Button);
			if (string.IsNullOrEmpty(btn.CommandArgument)) return;

			int id = Convert.ToInt32(btn.CommandArgument);
			Organization org = Storage<Organization>.ReadFirst(new Organization.ReadById { OrganizationId = id });
			if (org == null) return;

			CheckBox cbAnonymousOvereniSluzeb = btn.Parent.FindControl("cbAnonymousOvereniSluzeb") as CheckBox;
			CheckBox cbAnonymousZmenaNaJineRegistracniCislo = btn.Parent.FindControl("cbAnonymousZmenaNaJineRegistracniCislo") as CheckBox;
			CheckBox cbAnonymousSouhlasStavajicihoPoradce = btn.Parent.FindControl("cbAnonymousSouhlasStavajicihoPoradce") as CheckBox;
			CheckBox cbAnonymousSouhlasNavrzenehoPoradce = btn.Parent.FindControl("cbAnonymousSouhlasNavrzenehoPoradce") as CheckBox;
			TextBox txtParentCode = btn.Parent.FindControl("txtAnonymousZmenaNaJineRegistracniCisloText") as TextBox;

			//Get Parent
			Organization parentOrg = null;
			if (cbAnonymousZmenaNaJineRegistracniCislo.Checked)
			{
				if (!string.IsNullOrEmpty(txtParentCode.Text)) parentOrg = Storage<Organization>.ReadFirst(new Organization.ReadByCode { Code = txtParentCode.Text });
				if (parentOrg == null)
				{
					string js = string.Format("alert('{0}');", "Nesprávne registrační číslo!");
					btn.Page.ClientScript.RegisterStartupScript(btn.Page.GetType(), "parentValidateOrganization", js, true);
					return;
				}
			}

			org.AnonymousOvereniSluzeb = cbAnonymousOvereniSluzeb.Checked;
			org.AnonymousZmenaNaJineRegistracniCislo = cbAnonymousZmenaNaJineRegistracniCislo.Checked;
			org.AnonymousZmenaNaJineRegistracniCisloText = parentOrg != null ? parentOrg.Code : string.Empty;
			org.AnonymousSouhlasStavajicihoPoradce = cbAnonymousSouhlasStavajicihoPoradce.Checked;
			org.AnonymousSouhlasNavrzenehoPoradce = cbAnonymousSouhlasNavrzenehoPoradce.Checked;
			Storage<Organization>.Update(org);

			LoadData(true);
		}

		protected string GetOrganizationNameByCode(string code)
		{
			Organization org = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByCode { Code = code });
			if (org == null) return string.Empty;

			return org.Name;
		}

		protected void OnZmenaNaJineRegCislo(object sender, EventArgs e)
		{
			CheckBox cb = (sender as CheckBox);
			TextBox txt = cb.Parent.FindControl("txtAnonymousZmenaNaJineRegistracniCisloText") as TextBox;
			if (cb.Checked)
			{
				txt.Enabled = true;
			}

			LoadData(false);
		}
	}
}