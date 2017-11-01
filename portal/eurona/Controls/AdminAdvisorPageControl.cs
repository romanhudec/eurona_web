using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdvisorPageEntity = Eurona.DAL.Entities.AdvisorPage;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using UrlAliasEntity = CMS.Entities.UrlAlias;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using RoleEntity = CMS.Entities.Role;
using MasterPageEntity = CMS.Entities.MasterPage;
using CMS.Controls;

namespace Eurona.Controls
{
    public class AdminAdvisorPageControl : CmsControl
    {
        private TextBox tbOrganizationCode;
        private TextBox tbTitle;
        private TextBox tbName;
        private DropDownList ddlMasterPage;
        private CheckBox cbBlocked;
        private Button btnSave;
        private Button btnCancel;

        private bool isNew = false;
        private AdvisorPageEntity pageEntity = null;

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

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            isNew = Request["Id"] == null;
            if (isNew) pageEntity = new AdvisorPageEntity();
            else
            {
                int pageId = Convert.ToInt32(Request["id"]);
                pageEntity = Storage<AdvisorPageEntity>.ReadFirst(new AdvisorPageEntity.ReadById { AdvisorPageId = pageId });
            }

            Table table = new Table();
            table.Width = this.Width;

            
            TableRow trOrgCode = new TableRow();
            trOrgCode.Cells.Add(new TableCell
            {
                Text = "Číslo poradce",
                CssClass = "form_label_required"
            });
            trOrgCode.Cells.Add(CreateOrganizationCodeInput());
            table.Rows.Add(trOrgCode);

            TableRow trOrder = new TableRow();
            trOrder.Cells.Add(new TableCell
            {
                Text = CMS.Resources.Controls.AdminPageControl_LabelTitle,
                CssClass = "form_label_required"
            });
            trOrder.Cells.Add(CreateTitleInput());
            table.Rows.Add(trOrder);

            TableRow trName = new TableRow();
            trName.Cells.Add(new TableCell
            {
                Text = CMS.Resources.Controls.AdminPageControl_LabelName,
                CssClass = "form_label_required",
            });
            trName.Cells.Add(CreateNameInput());
            table.Rows.Add(trName);

            TableRow trMasterPage = new TableRow();
            trMasterPage.Cells.Add(new TableCell
            {
                Text = CMS.Resources.Controls.AdminPageControl_LabelMasterPage,
                CssClass = "form_label_required",
            });
            trMasterPage.Cells.Add(CreateMasterPageList());
            table.Rows.Add(trMasterPage);


            TableRow trBlocked = new TableRow();
            trBlocked.Cells.Add(new TableCell
            {
                Text = "Blokace",
                CssClass = "form_label_required",
            });
            trBlocked.Cells.Add(CreateBlockedInput());
            table.Rows.Add(trBlocked);

            CreateSaveButton();
            CreateCancelButton();

            TableRow trButtons = new TableRow();
            TableCell tdButtons = new TableCell();
            tdButtons.ColumnSpan = 2;
            tdButtons.Controls.Add(btnSave);
            tdButtons.Controls.Add(btnCancel);
            trButtons.Cells.Add(tdButtons);
            table.Rows.Add(trButtons);

            Controls.Add(table);

            if (!isNew)
            {
                this.tbOrganizationCode.Enabled = false;
            }
            this.ddlMasterPage.Enabled = false;
        }

        private TableCell CreateOrganizationCodeInput()
        {
            TableCell cell = new TableCell();
            cell.Attributes.Add("class", "form_control");
            tbOrganizationCode = new TextBox
            {
                ID = "tbOrganizationCode",
                Text = pageEntity.OrganizationCode,
                Width = Unit.Percentage(80)
            };
            cell.Controls.Add(tbOrganizationCode);
            cell.Controls.Add(CreateRequiredFieldValidatorControl(tbOrganizationCode.ID));
            return cell;
        }

        private TableCell CreateTitleInput()
        {
            TableCell cell = new TableCell();
            cell.Attributes.Add("class", "form_control");
            tbTitle = new TextBox
            {
                ID = "tbTitle",
                Text = pageEntity.Title,
                Width = Unit.Percentage(80)
            };
            cell.Controls.Add(tbTitle);
            cell.Controls.Add(CreateRequiredFieldValidatorControl(tbTitle.ID));
            return cell;
        }

        private TableCell CreateNameInput()
        {
            TableCell cell = new TableCell();
            cell.Attributes.Add("class", "form_control");
            tbName = new TextBox
            {
                ID = "tbName",
                Text = pageEntity.Name,
                Width = Unit.Percentage(80),
                Enabled = pageEntity.Id >= 0 //U systemovych stranok sa tento udaj editovat neda
            };
            cell.Controls.Add(tbName);
            cell.Controls.Add(CreateRequiredFieldValidatorControl(tbName.ID));
            return cell;
        }

        private TableCell CreateMasterPageList()
        {
            List<MasterPageEntity> masterPages = Storage<MasterPageEntity>.Read();
            masterPages = masterPages.OrderBy(p => p.Id).ToList();
            TableCell cell = new TableCell();
            ddlMasterPage = new DropDownList();
            ddlMasterPage.ID = "ddlMasterPage";
            ddlMasterPage.Width = Unit.Percentage(80);
            ddlMasterPage.DataSource = masterPages;
            ddlMasterPage.DataTextField = "Name";
            ddlMasterPage.DataValueField = "Id";
            ddlMasterPage.DataBind();
            cell.Controls.Add(ddlMasterPage);
            cell.Controls.Add(CreateRequiredFieldValidatorControl(ddlMasterPage.ID));
            ddlMasterPage.SelectedValue = pageEntity.MasterPageId.ToString();
            ddlMasterPage.Enabled = pageEntity.Id >= 0; //U systemovych stranok sa tento udaj editovat neda
            return cell;
        }

        private TableCell CreateBlockedInput()
        {
            TableCell cell = new TableCell();
            cell.Attributes.Add("class", "form_control");
            cbBlocked = new CheckBox()
            {
                ID = "cbBlocked",
                Text = "Blokace",
                Width = Unit.Percentage(80),
                Checked = pageEntity.Blocked
            };
            cell.Controls.Add(cbBlocked);
            return cell;
        }

        private void CreateCancelButton()
        {
            btnCancel = new Button
            {
                Text = CMS.Resources.Controls.CancelButton_Text,
                CausesValidation = false
            };
            btnCancel.Click += (s1, e1) => Response.Redirect(this.ReturnUrl);
        }

        private void CreateSaveButton()
        {
            btnSave = new Button
            {
                Text = CMS.Resources.Controls.SaveButton_Text
            };
            btnSave.Click += (s1, e1) =>
            {
                UrlAliasEntity urlAlias = null;
                if (isNew)
                {
                    OrganizationEntity organization = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByCode { Code = tbOrganizationCode.Text });
                    if (organization == null)
                    {
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Poradce podle čísla nebyl nalezen!');", true);
                        return;
                    }
                    if (organization.AccountId.HasValue == false)
                    {
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Poradce podle čísla nebyl nalezen!');", true);
                        return;
                    }

                    AdvisorPageEntity existPage = Storage<AdvisorPageEntity>.ReadFirst(new AdvisorPageEntity.ReadByAdvisorAccountId { AdvisorAccountId = organization.AccountId.Value });
                    if (existPage != null)
                    {
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Tento poradce již má osobní stránku!');", true);
                        return;
                    }

                    pageEntity.AdvisorAccountId = organization.AccountId.Value;
                    pageEntity.Title = tbTitle.Text;
                    pageEntity.Name = tbName.Text;
                    pageEntity.MasterPageId = Convert.ToInt32(ddlMasterPage.SelectedValue);
                    pageEntity.Blocked = cbBlocked.Checked;
                    if (pageEntity.RoleId == 0) pageEntity.RoleId = null;

                    urlAlias = new UrlAliasEntity();
                    urlAlias.Alias = "~/" + organization.Id.ToString();
                    urlAlias.Url = string.Format("~/advisorPage.aspx?aid={0}", organization.AccountId.Value);
                    urlAlias.Name = pageEntity.Title;
                    Storage<UrlAliasEntity>.Create(urlAlias);
                    pageEntity.UrlAliasId = urlAlias.Id;
                    pageEntity = Storage<AdvisorPageEntity>.Create(pageEntity);
                }
                else
                {
                    pageEntity.Blocked = cbBlocked.Checked;
                    pageEntity.Title = tbTitle.Text;
                    pageEntity.Name = tbName.Text;
                    Storage<AdvisorPageEntity>.Update(pageEntity);
                }

                Response.Redirect(this.ReturnUrl);
            };
        }

    }
}
