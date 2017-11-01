using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using PageEntity = CMS.Entities.Page;
using CMS.Entities;
using Eurona.Controls;
using System.Text.RegularExpressions;

namespace Eurona.EShop {
    public partial class GenericPageFB : WebPage {
        private PageEntity pageEntity;
        private PageEntity PageEntity {
            get {
                if (pageEntity != null) return pageEntity;
                string sid = Request["id"];
                if (!String.IsNullOrEmpty(sid)) {
                    int id = -1;
                    if (Int32.TryParse(sid, out id))
                        pageEntity = Storage<PageEntity>.ReadFirst(new PageEntity.ReadById { PageId = id });
                } else {
                    string name = Server.UrlDecode(Request["name"]);
                    pageEntity = Storage<PageEntity>.ReadFirst(new PageEntity.ReadByName { Name = name });
                }
                return pageEntity;
            }
        }

        public string Root {
            get {
                string root = CMS.Utilities.ServerUtilities.Root(this.Page.Request);
                if (root.EndsWith("/")) root = root.Remove(root.Length - 1, 1);
                return root;
            }
        }

        protected void Page_PreInit(object sender, EventArgs e) {
            if (this.PageEntity != null) {
                this.Page.MasterPageFile = this.PageEntity.MasterPage.Url;
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            if (this.PageEntity != null) {
                this.Title = this.PageEntity.Title;
                genericPage.PageName = this.PageEntity.Name;

                this.btnFacebook.Visible = Security.IsLogged(false);
            }
        }

        protected void OnSendToFacebook(object sender, EventArgs e) {
            //Zaevidovanie bonusovych kreditov
            string imageUrl = this.SearchFirstImageInHTML();
            if (imageUrl == null) return;
            if (Security.IsLogged(false))
                BonusovyKreditUzivateleHelper.ZaevidujKredit(Security.Account.Id, DAL.Entities.Classifiers.BonusovyKreditTyp.ShareAkcniNabidkyFacebook, null, this.PageEntity.Name);

            Response.Redirect("http://www.facebook.com/sharer.php?u=" + imageUrl);
        }

        protected string SearchFirstImageInHTML() {

            string htmlData = this.PageEntity.Content;
            List<string> logicalBlocks = NormalizeExternalHTMLToBlocks(htmlData);
            foreach (string dataBlock in logicalBlocks) {
                CMS.Entities.SearchEngineBase seb = new CMS.Entities.SearchEngineBase();

                string fTitle = string.Empty;
                string fContent = string.Empty;

                //Get search titles
                MatchCollection match = Regex.Matches(dataBlock, @"(SRC="".*?"")", RegexOptions.Singleline);
                if (match.Count != 0) {
                    string htmlValue = match[0].Groups[1].Value;
                    htmlValue = htmlValue.Replace("SRC=", string.Empty);
                    htmlValue = htmlValue.Replace("\"", string.Empty);
                    htmlValue = htmlValue.Replace("'", string.Empty);

                    return this.Root + htmlValue.ToLower();
                }
            }
            return null;
        }

        private List<string> NormalizeExternalHTMLToBlocks(string htmlData) {
            List<string> result = new List<string>();
            htmlData = htmlData.ToUpper();
            string data = Regex.Replace(htmlData, "\r\n", "");

            MatchCollection match = Regex.Matches(data, @"(<IMG.*?>.*?</IMG>)", RegexOptions.Singleline);
            foreach (Match m in match) {
                string htmlValue = m.Groups[1].Value;
                result.Add(htmlValue);
            }
            match = Regex.Matches(data, @"(<IMG.*?/>)", RegexOptions.Singleline);
            foreach (Match m in match) {
                string htmlValue = m.Groups[1].Value;
                result.Add(htmlValue);
            }
            return result;

        }
    }
}
