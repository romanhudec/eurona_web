using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ForumTrackingEntity = CMS.Entities.ForumTracking;
using ForumEntity = CMS.Entities.Forum;
using ForumPostEntity = CMS.Entities.ForumPost;
using SettingsEntity = Eurona.Common.DAL.Entities.Settings;
using AccountEntity = Eurona.DAL.Entities.Account;
using System.Text;

namespace Eurona
{
    public partial class ForumPage : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.forumPostFormControl.OnPostSend += new CMS.Controls.Forum.ForumPostFormControl.PostSendEventHandler(OnPostSend);
            if (!string.IsNullOrEmpty(Request["Id"]))
                this.forumPostsControl.ForumId = Convert.ToInt32(Request["Id"]);

            if (!string.IsNullOrEmpty(Request["Id"]))
                this.forumPostFormControl.ForumId = Convert.ToInt32(Request["Id"]);

            this.forumPostsControl.CommentFormID = this.forumPostFormControl.ClientID;
            if (this.forumPostsControl.Forum == null || this.forumPostsControl.Forum.Locked)
                this.forumPostFormControl.Visible = false;
        }

        void OnPostSend(int accountId, int forumPostId)
        {
            this.SendTrackingEmail(this.forumPostsControl.Forum, accountId);
            this.SendAdminTrackingEmail(this.forumPostsControl.Forum, forumPostId, accountId);
            this.forumPostsControl.LoadData();
        }

        private void SendAdminTrackingEmail(ForumEntity forum, int forumPostId, int senderAccountId)
        {
            ForumPostEntity post = Storage<ForumPostEntity>.ReadFirst(new ForumPostEntity.ReadById { ForumPostId = forumPostId });
            if (post == null) return;
            SettingsEntity emailSettings = Storage<SettingsEntity>.ReadFirst(new SettingsEntity.ReadByCode { Code = "EMAIL_PRISPEVEK_DISKUSE" });
            if (emailSettings == null) return;
            if (String.IsNullOrEmpty(emailSettings.Value)) return;

            AccountEntity sender = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadById { AccountId = senderAccountId });

            CMS.EmailNotification email = new CMS.EmailNotification();
            email.To = emailSettings.Value;
            email.Subject = string.Format("Nový vložený příspěvek do diskuse : {0}", forum.Name);
            email.Message = string.Format(@"Dobry den<br/>uživatel {0} (login:{1}) vložil do diskuse {2} nový příspěvek:<br/><br/>{3}<br/>{4}<br/><br/>Automaticky genrovaný email",
                    post.AccountName, sender.Login, forum.Name, post.Title, post.Content);
            email.Notify(true);
        }

        private void SendTrackingEmail(ForumEntity forum, int senderAccountId)
        {
            string root = Utilities.Root(this.Request);
            root = root.EndsWith("/") ? root.Substring(0, root.Length - 1) : root;
            string forumUrl = root + Page.ResolveUrl(forum.Alias);

            List<ForumTrackingEntity> list = Storage<ForumTrackingEntity>.Read(new ForumTrackingEntity.ReadBy { ForumId = forum.Id });
            if (list.Count == 0) return;
            CMS.EmailNotification email = new CMS.EmailNotification();

            StringBuilder sbEmail = new StringBuilder();
            foreach (ForumTrackingEntity fte in list)
            {
                if (fte.AccountId == senderAccountId) continue;
                if (sbEmail.Length != 0) sbEmail.Append(";");
                sbEmail.Append(fte.Email);
            }
            email.Bcc = sbEmail.ToString();
            email.Subject = string.Format("Upozornenie na odpoveď v téme : {0}", forum.Name);
            email.Message = string.Format(@"Dobry den<br/>tento e-mail ste obdržal(a) pretože sledujete tému '{0}' na {1} Táto téma zaznamenala od vašej poslednej návštevy nový príspevok. Následujúcí odkaz môžete použiť k zobrazeniu nových príspevkov.<br/><br/> <a href='{2}'>{2}</a>",
                    forum.Name, root, forumUrl);
            email.Notify(true);
        }
    }
}
