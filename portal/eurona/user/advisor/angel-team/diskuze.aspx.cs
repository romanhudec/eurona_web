using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ForumThreadEntity = CMS.Entities.ForumThread;
using System.Text;
using Eurona.DAL.Entities;
using Eurona.Common.DAL.Entities;

namespace Eurona.User.Advisor.AngelTeam
{
	public partial class DiskuzePage : WebPage
	{
		public const string DisplayUrlFormat = "~/user/advisor/forumThread.aspx?id={0}";
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(Request["Id"]))
			{
				int objectId = Convert.ToInt32(Request["Id"]);
				Account account = Storage<Account>.ReadFirst(new Account.ReadById { AccountId = objectId });
				if (account == null) return;
				Organization organization = Storage<Organization>.ReadFirst(new Organization.ReadByAccountId { AccountId = account.Id });
				if (organization == null) return;


				ForumThreadEntity thread = Storage<ForumThreadEntity>.ReadFirst(new ForumThreadEntity.ReadByObjectId { ObjectId = objectId });
				if (thread == null)
				{
					thread = new ForumThreadEntity();
					thread.ObjectId = objectId;
					thread.Name = string.Format("{0}-{1}", organization.Code, organization.Name);
					thread.Icon = "";
					thread.Description = string.Format("Vlákno uživaltele {0}", organization.Code);
					thread.Locale = account.Locale;
					thread = Storage<ForumThreadEntity>.Create(thread);

					#region Vytvorenie URLAliasu
					string alias = string.Format("~/forum/{0}", CMS.Utilities.AliasUtilities.GetAliasString(thread.Name));
					if (!CMS.Utilities.AliasUtilities.CreateUrlAlias<ForumThreadEntity>(this.Page, DisplayUrlFormat, thread.Name, alias, thread, Storage<ForumThreadEntity>.Instance))
						return;
					#endregion
				}
				this.forumsControl.ForumThreadId = thread.Id;
			}
		}
	}
}