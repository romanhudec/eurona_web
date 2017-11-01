using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using CMS.Controls;
using CMS.Entities;
using Eurona.DAL.Entities;
using CMSEditor = CMS.Controls.RadEditor.RadEditor;
using System.Web.UI.HtmlControls;
using System;
using System.Web;
using System.IO;

namespace Eurona.Controls {
    public class AccountProfileControlEdit : CmsControl {
        private List<ProfileItemControl> ItemControls { get; set; }
        private Button btnSave;
        private Button btnCancel;

        internal class ProfileItemControl : CmsControl {
            private Profile profile = null;
            private int accountId;
            public ProfileItemControl(Profile profile, int accountId) {
                this.profile = profile;
                this.accountId = accountId;
            }

            protected override void CreateChildControls() {
                base.CreateChildControls();
                Control ctrl = CreateProfileControl(this.profile);
                if (ctrl != null)
                    this.Controls.Add(ctrl);

                if (!this.Page.IsPostBack) {
                    this.Bind(this.accountId);
                    this.DataBind();
                }
            }

            public Profile Profile {
                get { return this.profile; }
            }

            /// <summary>
            /// Vráti hodnotu ProfileItemControl.
            /// </summary>
            public object Value {
                get {
                    EnsureChildControls();
                    switch ((ProfileType)profile.Type) {
                        case ProfileType.Text:
                            return (this.Controls[0] as TextBox).Text;
                        case ProfileType.Picture:
                            return (this.Controls[0].Controls[2] as FileUpload).PostedFile;
                        case ProfileType.HtmlText:
                            return (this.Controls[0] as CMSEditor).Content;
                    }

                    return null;
                }
            }

            private Control CreateProfileControl(Profile profile) {
                string id = profile.Name.Normalize().Replace(' ', '_');

                Control ctrl = null;
                switch ((ProfileType)profile.Type) {
                    case ProfileType.Text:
                        ctrl = new TextBox() { ID = id, Width = Unit.Pixel(200) };
                        break;
                    case ProfileType.Picture:
                        ctrl = new HtmlGenericControl("div");
                        ctrl.Controls.Add(new Image() { ID = id + "_image" });
                        ctrl.Controls.Add(new LiteralControl("<br />"));
                        ctrl.Controls.Add(new FileUpload() { ID = id + "_fu" });
                        break;
                    case ProfileType.HtmlText:
                        ctrl = new CMSEditor() { ID = id, Height = Unit.Pixel(500) };
                        break;
                }

                return ctrl;
            }

            private void Bind(int accountId) {
                AccountProfile ap = Storage<AccountProfile>.ReadFirst(new AccountProfile.ReadByAccountAndProfile { AccountId = accountId, ProfileId = this.Profile.Id });
                if (ap == null) {
                    if ((ProfileType)profile.Type == ProfileType.Picture) this.Controls[0].Controls[0].Visible = false;
                    return;
                }

                if (string.IsNullOrEmpty(ap.Value) && (ProfileType)profile.Type == ProfileType.Picture) {
                    this.Controls[0].Controls[0].Visible = false;
                    return;
                }

                switch ((ProfileType)profile.Type) {
                    case ProfileType.Text:
                        (this.Controls[0] as TextBox).Text = ap.Value;
                        break;
                    case ProfileType.Picture:
                        (this.Controls[0].Controls[0] as Image).ImageUrl = this.Page.ResolveUrl(ap.Value);
                        break;
                    case ProfileType.HtmlText:
                        (this.Controls[0] as CMSEditor).Content = ap.Value;
                        break;
                }
            }
        }

        public int AccountId {
            get {
                object o = ViewState["AccountId"];
                return o != null ? (int)Convert.ToInt32(o) : 0;
            }
            set { ViewState["AccountId"] = value; }
        }

        #region Protected overrides
        protected override void CreateChildControls() {
            base.CreateChildControls();

            List<Profile> profileItems = Storage<Profile>.Read();
            if (profileItems == null)
                return;

            this.ItemControls = new List<ProfileItemControl>();
            Table table = new Table();
            TableRow row = null;
            TableCell cell = null;
            foreach (Profile profile in profileItems) {
                row = new TableRow();
                cell = new TableCell();
                cell.Controls.Add(new LiteralControl(string.Format("{0} :", profile.Name)));
                cell.CssClass = "form_label";
                row.Cells.Add(cell);

                //Control
                ProfileItemControl ctrl = new ProfileItemControl(profile, this.AccountId);
                if (ctrl == null) continue;
                cell = new TableCell();
                cell.Controls.Add(ctrl);
                cell.CssClass = "form_control";
                row.Cells.Add(cell);

                table.Rows.Add(row);

                this.ItemControls.Add(ctrl);
            }

            CreateCancelButton();
            CreateSaveButton();
            row = new TableRow();
            cell = new TableCell();
            cell.ColumnSpan = 2;
            cell.Controls.Add(this.btnSave);
            cell.Controls.Add(this.btnCancel);
            row.Cells.Add(cell);

            table.Rows.Add(row);

            this.Controls.Add(table);
        }
        #endregion

        private void CreateCancelButton() {
            btnCancel = new Button {
                Text = Resources.Strings.CancelButton_Text,
                CausesValidation = false
            };
            btnCancel.Click += (s1, e1) => Response.Redirect(this.ReturnUrl);
        }

        private void CreateSaveButton() {
            btnSave = new Button {
                Text = Resources.Strings.SaveButton_Text
            };
            btnSave.Click += (s1, e1) => {

                foreach (ProfileItemControl ctrl in this.ItemControls) {
                    AccountProfile ap = Storage<AccountProfile>.ReadFirst(new AccountProfile.ReadByAccountAndProfile { AccountId = this.AccountId, ProfileId = ctrl.Profile.Id });
                    bool isNew = ap == null;
                    if (ap == null) {
                        ap = new AccountProfile();
                        ap.AccountId = this.AccountId;
                        ap.ProfileId = ctrl.Profile.Id;
                    }

                    object value = ctrl.Value;
                    if (value == null && (ProfileType)ctrl.Profile.Type != ProfileType.Picture) ap.Value = string.Empty;
                    if (value is string) ap.Value = value.ToString();
                    if (value is HttpPostedFile) {
                        HttpPostedFile pf = (value as HttpPostedFile);
                        if (pf.ContentLength != 0) {
                            string imgVirtualPath = string.Format("~/userfiles/{0}", Path.GetFileName(pf.FileName));
                            string imgPsychycalPath = Server.MapPath(imgVirtualPath);
                            string imgPsychycalPathTmp = imgPsychycalPath + ".tmp";

                            if (File.Exists(imgPsychycalPath)) File.Delete(imgPsychycalPath);
                            if (File.Exists(imgPsychycalPathTmp)) File.Delete(imgPsychycalPathTmp);

                            pf.SaveAs(imgPsychycalPath);
                            File.Copy(imgPsychycalPath, imgPsychycalPathTmp, true);

                            System.Drawing.Image img = System.Drawing.Image.FromFile(imgPsychycalPath);
                            int w = img.Width;
                            int h = img.Height;
                            img.Dispose();
                            ImageGalleryControl.RecalculateImageSize(w, h, 120, 120, ref w, ref h);
                            ImageGalleryControl.ResizeImage(imgPsychycalPathTmp, imgPsychycalPath, w, h);

                            ap.Value = imgVirtualPath;
                        }

                    }

                    //Save account profile
                    if (isNew) Storage<AccountProfile>.Create(ap);
                    else Storage<AccountProfile>.Update(ap);
                }

                if (string.IsNullOrEmpty(this.ReturnUrl)) return;
                Response.Redirect(this.ReturnUrl);

            };
        }
    }
}
