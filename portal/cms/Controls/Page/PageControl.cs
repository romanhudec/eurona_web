using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMSEditor = CMS.Controls.RadEditor.RadEditor;
using PageEntity = CMS.Entities.Page;
using RoleEntity = CMS.Entities.Role;
using System;
using System.Web.UI.HtmlControls;
using CMS.Utilities;
using Telerik.Web.UI;
using System.Collections.Generic;
using SupportedLocaleEntity = CMS.Entities.Classifiers.SupportedLocale;
using MasterPageEntity = CMS.Entities.MasterPage;
using UrlAliasEntity = CMS.Entities.UrlAlias;

namespace CMS.Controls.Page {
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:Page runat=server></{0}:Page>")]
    public class PageControl : ContentEditorCmsControl {
        internal enum PageMode {
            Display = 1,
            Edit = 2,
            Tool = 3
        }

        public delegate string ContentProcessorHandler(PageControl sender, string content);
        public event ContentProcessorHandler ContentProcessor;

        private Button save;
        private Button cancel;
        private CMSEditor editor;
        private PageEntity page;
        private ContentEditorToolbarControl editorToolBar;
        private HtmlGenericControl editorContent;

        private HtmlGenericControl toolModeContainer;
        private HtmlGenericControl editModeContainer;

        Telerik.Web.UI.RadWindow rw = null;
        Telerik.Web.UI.RadWindowManager rwm = null;

        public PageControl() {
        }

        #region Public properties

        private PageEntity PageEntity {
            get {
                if (page != null) return page;
                if (!string.IsNullOrEmpty(this.PageName))
                    page = Storage<PageEntity>.ReadFirst(new PageEntity.ReadForCurrentAccount { Name = PageName, Locale = this.PageLocale });
                if (page == null) {
                    if (!String.IsNullOrEmpty(NotFoundUrlFormat))
                        Response.Redirect(String.Format(NotFoundUrlFormat, PageName));
                }

                return page;
            }
        }

        /// <summary>
        /// Titulok stránky
        /// </summary>
        public string Title {
            get { return this.PageEntity.Title; }
        }

        /// <summary>
        /// Názov stránky
        /// </summary>
        public string PageName {
            get { return GetState<string>("PageName"); }
            set { SetState<String>("PageName", value); }
        }

        /// <summary>
        /// Locale stránky
        /// </summary>
        public string PageLocale {
            get { return Request["locale"]; }
        }

        /// <summary>
        /// Url, ktorá sa zobrazí, ak sa stránka nenájde.
        /// </summary>
        public string NotFoundUrlFormat { get; set; }

        /// <summary>
        /// Url PopUpEditor, ak je definovana Url editora, zobrazi sa ako popUp window.
        /// </summary>
        public string PopUpEditorUrlFormat { get; set; }

        /// <summary>
        /// Vráti true, ak je stránka v editačnom režime.
        /// </summary>
        public new bool IsEditing {
            get { return this.Mode == PageMode.Edit; }
        }
        /// <summary>
        /// Mód v ktoróm je stránka zobrazena. ( Display, Edit, Tool )
        /// </summary>
        private PageMode Mode {
            get { return GetState<PageMode>("PageMode"); }
            set { SetState<PageMode>("PageMode", value); }
        }
        #endregion

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            if (!IsPostBack) {
                this.Mode = PageMode.Display;
                if (base.CanManageControl())
                    this.Mode = string.IsNullOrEmpty(Request["edit"]) ? PageMode.Tool : PageMode.Edit;
            }
        }

        protected override void CreateChildControls() {
            if (this.PageEntity == null) return;
            //Musi tu byt, lebo prepisujem ContentEditorCmsControl a toolbar je tu umiestneny inde !!!
            this.Controls.Clear();

            //Get content data
            string content = this.PageEntity.Content;
            if (ContentProcessor != null) content = ContentProcessor(this, content);

            //Pridanie div pre tool rezim
            this.toolModeContainer = new HtmlGenericControl("div");
            this.editorToolBar = CreateEditorToolBar();
            if (this.editorToolBar != null)
                this.toolModeContainer.Controls.Add(this.editorToolBar);
            this.editorContent = new HtmlGenericControl("div");
            this.editorContent.Attributes.Add("class", this.CssEditorContent);
            this.editorContent.InnerHtml = string.Format("<div style='margin:5px;'>{0}</div>", content);
            this.toolModeContainer.Controls.Add(this.editorContent);

            //Pridanie div pre editacny rezim
            this.editModeContainer = new HtmlGenericControl("div");
            this.save = new Button();
            this.save.CausesValidation = false;
            this.save.ID = "save";
            this.save.Text = Resources.Controls.SaveButton_Text;
            this.save.Click += OnSave;

            this.cancel = new Button();
            this.cancel.CausesValidation = false;
            this.cancel.ID = "cancel";
            this.cancel.Text = Resources.Controls.CancelButton_Text;
            this.cancel.Click += OnCancel;

            this.editor = new CMSEditor();
            this.editor.ID = "editor";
            if (!IsPostBack || !IsPostBackControl("editor")) this.editor.Content = PageEntity.Content;
            this.editModeContainer.Controls.Add(this.editor);
            this.editModeContainer.Controls.Add(this.save);
            this.editModeContainer.Controls.Add(this.cancel);
            this.Controls.Add(this.toolModeContainer);
            this.Controls.Add(this.editModeContainer);

            this.toolModeContainer.Visible = true;
            this.editModeContainer.Visible = true;

            this.editorToolBar.Attributes.Add("onmouseover", "document.getElementById('" + this.editorToolBar.ClientID + "').nextSibling.className = '" + this.CssEditorContent + "_selected'");
            this.editorToolBar.Attributes.Add("onmouseout", "document.getElementById('" + this.editorToolBar.ClientID + "').nextSibling.className = '" + this.CssEditorContent + "'");

            if (!string.IsNullOrEmpty(this.PopUpEditorUrlFormat)) {
                this.rw = new Telerik.Web.UI.RadWindow();
                this.rw.ReloadOnShow = true;
                this.rw.DestroyOnClose = false;
                this.rw.ShowContentDuringLoad = false;
                this.rw.VisibleOnPageLoad = false;
                this.rw.Title = this.Page.Title;
                this.rw.ID = "radWindow";
                this.rw.Modal = true;
                this.rw.VisibleStatusbar = false;
                this.rw.Behaviors = Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize;
                this.rw.OnClientClose = @"function(sender, e){var arg = e.get_argument(); 
								if(arg) location.reload(true);}";

                this.rwm = new Telerik.Web.UI.RadWindowManager();
                this.rwm.ID = "radWindowManager";
                this.rwm.Windows.Add(this.rw);

                this.Controls.Add(rwm);
            }

        }

        void OnSave(object sender, EventArgs e) {
            PageEntity.Content = editor.Content;
            PageEntity.ContentKeywords = StringUtilities.RemoveDiacritics(editor.Text);
            Storage<PageEntity>.Update(PageEntity);

            this.editorContent.InnerHtml = string.Format("<div style='margin:5px;'>{0}</div>", PageEntity.Content);
            if (!string.IsNullOrEmpty(this.ReturnUrl))
                Response.Redirect(Page.ResolveUrl(this.ReturnUrl));
        }
        void OnCancel(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(this.ReturnUrl))
                Response.Redirect(Page.ResolveUrl(this.ReturnUrl));
        }

        protected override void OnPreRender(EventArgs e) {
            base.OnPreRender(e);
            if (String.IsNullOrEmpty(PageName) || PageEntity == null)
                return;

            if (IsPostBack && (IsPostBackControl("save") || IsPostBackControl("cancel")) && this.Mode == PageMode.Edit)
                this.Mode = PageMode.Tool;

            if (!CanManageControl()) {
                this.Mode = PageMode.Display;
            }

            if (this.Mode == PageMode.Edit) {
                //do editora sa priradi aktualne ulozeny content
                this.editor.Content = PageEntity.Content;
                this.editModeContainer.Visible = true;
                this.toolModeContainer.Visible = false;
            } else if (this.Mode == PageMode.Tool) {
                this.editModeContainer.Visible = false;
                this.toolModeContainer.Visible = true;
            } else if (this.Mode == PageMode.Display) {
                this.editModeContainer.Visible = false;
                this.toolModeContainer.Visible = false;
            }
        }

        protected override void Render(HtmlTextWriter writer) {
            base.Render(writer);

            if (String.IsNullOrEmpty(PageName) || PageEntity == null) {
                writer.Write("Page not found! Page={0}", PageName);
                return;
            }

            if (!base.CanManageControl()) {
                this.Mode = PageMode.Display;
            }

            if (this.Mode == PageMode.Display) {
                //Get content data
                string content = PageEntity.Content;
                if (ContentProcessor != null) content = ContentProcessor(this, content);

                writer.Write(content);
            }
        }

        public override ContentEditorToolbarControl CreateEditorToolBar() {
            //Editor toolbar
            ContentEditorToolbarControl editorToolBar = new ContentEditorToolbarControl();
            editorToolBar.ID = "pageEditToolbarControl";
            editorToolBar.CssClass = this.CssEditorToolBar;
            string navigateUrl = string.Empty;

            //Create toolbar items
            if (!string.IsNullOrEmpty(this.ManageUrl)) {
                string paramSeparator = this.ManageUrl.IndexOf("?") > 0 ? "&" : "?";
                navigateUrl = Page.ResolveUrl(this.ManageUrl) + paramSeparator + this.BuildReturnUrlQueryParam();
                ContentEditorToolbarButton btnManage = new ContentEditorToolbarButton(Resources.Controls.PageEditToolBar_ManageButtonText, this.ConfigValue("CMS:ManageButtonImage"), navigateUrl);
                editorToolBar.Items.Add(btnManage);
            }

            if (!string.IsNullOrEmpty(this.NewUrl)) {
                string paramSeparator = this.NewUrl.IndexOf("?") > 0 ? "&" : "?";
                navigateUrl = Page.ResolveUrl(this.NewUrl) + paramSeparator + this.BuildReturnUrlQueryParam();
                ContentEditorToolbarButton btnNew = new ContentEditorToolbarButton(Resources.Controls.PageEditToolBar_NewButtonText, this.ConfigValue("CMS:NewButtonImage"), navigateUrl);
                editorToolBar.Items.Add(btnNew);
            }

            ContentEditorToolbarButton btnEdit = new ContentEditorToolbarButton(Resources.Controls.PageEditToolBar_EditButtonText, this.ConfigValue("CMS:EditButtonImage"), null);
            if (string.IsNullOrEmpty(this.PopUpEditorUrlFormat))
                btnEdit.Click += (s, e) => { this.Mode = PageMode.Edit; };
            else {
                string url = Page.ResolveUrl(string.Format(this.PopUpEditorUrlFormat, this.page.Id));
                btnEdit.OnClientClick = string.Format(@"var oWnd = radopen('{0}', 'radWindow');
								oWnd.setSize(document.documentElement.clientWidth-200, document.documentElement.clientHeight-50);
								oWnd.center();return false;", url);
            }
            editorToolBar.Items.Add(btnEdit);

            //Supported languages
            List<SupportedLocaleEntity> list = Storage<SupportedLocaleEntity>.Read();
            if (list.Count != 0) {
                RadComboBox ddlSupportedLocale = new RadComboBox();
                ddlSupportedLocale.AutoPostBack = true;
                ddlSupportedLocale.CausesValidation = false;
                ddlSupportedLocale.MarkFirstMatch = false;
                ddlSupportedLocale.EnableLoadOnDemand = false;
                ddlSupportedLocale.EnableTextSelection = false;
                ddlSupportedLocale.AutoCompleteSeparator = "";
                ddlSupportedLocale.ID = "ddlSupportedLocale";
                foreach (SupportedLocaleEntity locale in list) {
                    RadComboBoxItem item = new RadComboBoxItem(locale.Name, locale.Code);
                    if (!string.IsNullOrEmpty(locale.Icon)) item.ImageUrl = Page.ResolveUrl(locale.Icon);
                    ddlSupportedLocale.Items.Add(item);
                }
                editorToolBar.Items.Add(ddlSupportedLocale);
                ddlSupportedLocale.SelectedIndexChanged += (s, e) => {
                            PageEntity localedPage = null;
                            UrlAliasEntity urlAliasEntity = null;

                            //Page with childs
                            if (PageEntity.ParentId.HasValue) {
                                PageEntity parentPage = Storage<PageEntity>.ReadFirst(new PageEntity.ReadById { PageId = PageEntity.ParentId.Value });
                                localedPage = Storage<PageEntity>.ReadFirst(new PageEntity.ReadByName { Name = parentPage.Name, Locale = ddlSupportedLocale.SelectedValue });
                                if (localedPage == null) {
                                    urlAliasEntity = Storage<UrlAliasEntity>.ReadFirst(new UrlAliasEntity.ReadById { UrlAliasId = parentPage.UrlAliasId.Value });

                                    parentPage.Locale = ddlSupportedLocale.SelectedValue;
                                    Storage<PageEntity>.Create(parentPage);

                                    urlAliasEntity.Locale = ddlSupportedLocale.SelectedValue;
                                    Storage<UrlAliasEntity>.Create(urlAliasEntity);
                                }
                                localedPage = Storage<PageEntity>.ReadFirst(new PageEntity.ReadByName { Name = parentPage.Name, Locale = ddlSupportedLocale.SelectedValue });

                                urlAliasEntity = Storage<UrlAliasEntity>.ReadFirst(new UrlAliasEntity.ReadById { UrlAliasId = localedPage.UrlAliasId.Value });
                                Response.Redirect(urlAliasEntity.Url + (urlAliasEntity.Url.Contains("?") ? "&" : "?") + "locale=" + ddlSupportedLocale.SelectedValue);
                                return;
                            }

                            //Content pages without childs
                            localedPage = Storage<PageEntity>.ReadFirst(new PageEntity.ReadByName { Name = PageEntity.Name, Locale = ddlSupportedLocale.SelectedValue });
                            if (localedPage == null) {
                                urlAliasEntity = Storage<UrlAliasEntity>.ReadFirst(new UrlAliasEntity.ReadById { UrlAliasId = PageEntity.UrlAliasId.Value });
                                urlAliasEntity.Locale = ddlSupportedLocale.SelectedValue;
                                urlAliasEntity = Storage<UrlAliasEntity>.Create(urlAliasEntity);

                                PageEntity.Locale = ddlSupportedLocale.SelectedValue;
                                PageEntity.UrlAliasId = urlAliasEntity.Id;
                                Storage<PageEntity>.Create(PageEntity);

                            }

                            string url = PageEntity.Alias;
                            if (PageEntity.UrlAliasId.HasValue) {
                                urlAliasEntity = Storage<UrlAliasEntity>.ReadFirst(new UrlAliasEntity.ReadById { UrlAliasId = PageEntity.UrlAliasId.Value });
                                url = urlAliasEntity.Url;
                            }
                            Response.Redirect(url + (url.Contains("?") ? "&" : "?") + "locale=" + ddlSupportedLocale.SelectedValue);
                        };

                if (!IsPostBack) {
                    string locale = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
                    ddlSupportedLocale.SelectedValue = string.IsNullOrEmpty(this.PageLocale) ? locale : this.PageLocale;
                    ddlSupportedLocale.DataBind();
                }
            }

            if (editorToolBar.Items.Count == 0)
                return null;

            return editorToolBar;
        }
    }
}
