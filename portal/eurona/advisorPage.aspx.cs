using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using PageEntity = Eurona.DAL.Entities.AdvisorPage;
using CMS.Entities;

namespace Eurona
{
    public partial class AdvisorGenericPage : WebPage
    {
        private PageEntity pageEntity;
        private PageEntity PageEntity
        {
            get
            {
                if (pageEntity != null) return pageEntity;
                string sid = Request["id"];
                if (!String.IsNullOrEmpty(sid))
                {
                    int id = -1;
                    if (Int32.TryParse(sid, out id))
                        pageEntity = Storage<PageEntity>.ReadFirst(new PageEntity.ReadById { AdvisorPageId = id });
                }
                else
                {
                    string aid = Request["aid"];
                    if (!String.IsNullOrEmpty(aid))
                    {
                        int accountId = -1;
                        if (Int32.TryParse(aid, out accountId))
                            pageEntity = Storage<PageEntity>.ReadFirst(new PageEntity.ReadByAdvisorAccountId { AdvisorAccountId = accountId });
                    }
                }
                return pageEntity;
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (this.PageEntity != null)
            {
                this.Page.MasterPageFile = this.PageEntity.MasterPage.Url;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.PageEntity != null)
            {
                this.Title = this.PageEntity.Title;
                genericPage.PageName = this.PageEntity.Name;
                genericPage.AdvisorAccountId = this.PageEntity.AdvisorAccountId;
                aThisPage.Text = this.PageEntity.Title;

                /*
                if (this.PageEntity.Blocked)
                {
                    Response.Redirect("~/");
                }
                 * */
            }
        }
    }
}
