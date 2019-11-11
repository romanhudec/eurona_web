using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProductEntity = Eurona.Common.DAL.Entities.Product;
using RoleEntity = CMS.Entities.Role;
using System.Web.UI.WebControls;
using CMS.Controls;
using System.Web.UI;
using Eurona.Common;
using System.IO;


namespace Eurona.Controls.Product {
    public class AdminProductControl : CmsControl {
        private TextBox txtMaximalniPocetVBaleni = null;
        private TextBox txtMinimalniPocetVBaleni = null;
        private TextBox txtVamiNejviceNakupovane = null;
        private TextBox txtDarkovySet = null;
        private TextBox txtInternalStorageCount = null;
        private ASPxDatePicker dtpLimitDate = null;
        private TextBox txtLimitTime = null;
        private CheckBox cbBSRProdukt = null;
        private TextBox txtOrder = null;

        private Table ctrlDocuments;
        private Button btnSave = null;
        private Button btnCancel = null;

        private ASPxMultipleFileUpload mfuDocuments;

        private ProductEntity product = null;

        public AdminProductControl() {
        }

        /// <summary>
        /// Ak je property null, komponenta pracuje v rezime New.
        /// </summary>
        public int? ProductId {
            get {
                object o = ViewState["ProductId"];
                return o != null ? (int?)Convert.ToInt32(o) : null;
            }
            set { ViewState["ProductId"] = value; }
        }

        #region Protected overrides
        protected override void CreateChildControls() {
            base.CreateChildControls();

            Control productControl = CreateDetailControl();
            if (productControl != null)
                this.Controls.Add(productControl);

            this.product = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = this.ProductId.Value });

            //Binding
            if (!IsPostBack) {
                this.txtMinimalniPocetVBaleni.Text = this.product.MinimalniPocetVBaleni.ToString();
                this.txtMaximalniPocetVBaleni.Text = this.product.MaximalniPocetVBaleni.ToString();
                this.txtVamiNejviceNakupovane.Text = this.product.VamiNejviceNakupovane.ToString();
                this.txtDarkovySet.Text = this.product.DarkovySet.ToString();
                this.txtInternalStorageCount.Text = this.product.InternalStorageCount.ToString();
                this.dtpLimitDate.Value = this.product.LimitDate;
                if (this.product.LimitDate.HasValue)
                    this.txtLimitTime.Text = string.Format("{0}:{1}", this.product.LimitDate.Value.Hour, this.product.LimitDate.Value.Minute);
                this.cbBSRProdukt.Checked = this.product.BSR;
                if( this.product.Order.HasValue )
                    this.txtOrder.Text = this.product.Order.Value.ToString();
            }

            LoadAttachments();
            
        }
        #endregion

        private void LoadAttachments() {
            //Documents
            string productAttachmentsVirtualPath = ConfigValue("SHP:DocumentGallery:Product:StoragePath");
            string productAttachmentsPath = Path.Combine(Server.MapPath(productAttachmentsVirtualPath), product.Id.ToString());
            if (!Directory.Exists(productAttachmentsPath))
                return;
            string[] files = Directory.GetFiles(productAttachmentsPath);
            int index = 0;
            this.ctrlDocuments.Rows.Clear();
            foreach (string file in files) {
                string fileName = Path.GetFileName(file);
                string url = string.Format("{0}{1}/{2}", productAttachmentsVirtualPath, product.Id, fileName);
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.VerticalAlign = VerticalAlign.Middle;
                cell.HorizontalAlign = HorizontalAlign.Left;
                cell.Controls.Add(new HyperLink { NavigateUrl = Page.ResolveUrl(url), Text = fileName });
                row.Cells.Add(cell);

                cell = new TableCell();
                ImageButton btnDelete = new ImageButton { ImageUrl = Page.ResolveUrl("~/images/delete.png"), AlternateText="Smazat" };
                btnDelete.ID = "btnDelete_" + index.ToString();
                btnDelete.CommandArgument = file;
                btnDelete.Click += onAttachmentDelete;
                cell.Controls.Add(btnDelete);
                cell.VerticalAlign = VerticalAlign.Middle;
                cell.HorizontalAlign = HorizontalAlign.Right;
                row.Cells.Add(cell);
                this.ctrlDocuments.Rows.Add(row);
                index++;
            }
        }

        private void onAttachmentDelete(Object sender, EventArgs e) {
            ImageButton btnDelete = sender as ImageButton;
            File.Delete(btnDelete.CommandArgument);
            LoadAttachments();
        }
        /// <summary>
        /// Vytvori Control Clanku
        /// </summary>
        private Control CreateDetailControl() {
            this.txtMinimalniPocetVBaleni = new TextBox();
            this.txtMinimalniPocetVBaleni.ID = "txtMinimalniPocetVBaleni";
            this.txtMinimalniPocetVBaleni.Width = Unit.Pixel(100);

            this.txtMaximalniPocetVBaleni = new TextBox();
            this.txtMaximalniPocetVBaleni.ID = "txtMaximalniPocetVBaleni";
            this.txtMaximalniPocetVBaleni.Width = Unit.Pixel(100);

            this.txtVamiNejviceNakupovane = new TextBox();
            this.txtVamiNejviceNakupovane.ID = "txtVamiNejviceNakupovane";
            this.txtVamiNejviceNakupovane.Width = Unit.Pixel(100);

            this.txtDarkovySet = new TextBox();
            this.txtDarkovySet.ID = "txtDarkovySet";
            this.txtDarkovySet.Width = Unit.Pixel(100);

            this.txtInternalStorageCount = new TextBox();
            this.txtInternalStorageCount.ID = "txtInternalStorageCount";
            this.txtInternalStorageCount.Width = Unit.Pixel(100);

            this.dtpLimitDate = new ASPxDatePicker();
            this.dtpLimitDate.ID = "dtpLimitDate";

            this.txtLimitTime = new TextBox();
            this.txtLimitTime.ID = "txtLimitTime";
            this.txtLimitTime.Width = Unit.Pixel(100);

            this.cbBSRProdukt = new CheckBox();
            this.cbBSRProdukt.ID = "cbBSRProdukt";
            this.cbBSRProdukt.Width = Unit.Pixel(100);

            this.txtOrder = new TextBox();
            this.txtOrder.ID = "txtOrder";
            this.txtOrder.Width = Unit.Pixel(100);

            this.btnSave = new Button();
            this.btnSave.CausesValidation = true;
            this.btnSave.Text = CMS.Resources.Controls.SaveButton_Text;
            this.btnSave.Click += new EventHandler(OnSave);
            this.btnCancel = new Button();
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Text = CMS.Resources.Controls.CancelButton_Text;
            this.btnCancel.Click += new EventHandler(OnCancel);

            Table table = new Table();
            table.Width = this.Width;
            table.Height = this.Height;

            table.Rows.Add(CreateTableRow("Maximální objednatelný počet : ", this.txtMaximalniPocetVBaleni, false, ValidationDataType.Integer));
            table.Rows.Add(CreateTableRow("Minimální objednatelný počet : ", this.txtMinimalniPocetVBaleni, false, ValidationDataType.Integer));
            table.Rows.Add(CreateTableRow("Vámi nejvíce nakupované (pořadí) : ", this.txtVamiNejviceNakupovane, false, ValidationDataType.Integer));
            table.Rows.Add(CreateTableRow("Dárkový set (pořadí) : ", this.txtDarkovySet, false, ValidationDataType.Integer));
            table.Rows.Add(CreateTableRow("Interní stav skladu : ", this.txtInternalStorageCount, false, ValidationDataType.Integer, "(Pokud je hodnota -1, není interní stav skladu u tohoto produktu kontrolován!)"));
            table.Rows.Add(CreateTableRow("Datum odpočtu : ", this.dtpLimitDate, false, ValidationDataType.Date));
            table.Rows.Add(CreateTableRow("Čas odpočtu : ", this.txtLimitTime, false, null));
            table.Rows.Add(CreateTableRow("BSR Produkt : ", this.cbBSRProdukt, false, null));
            table.Rows.Add(CreateTableRow("Pořadí pro zobrazení : ", this.txtOrder, false, ValidationDataType.Integer));

            TableRow r = new TableRow();
            TableCell c = new TableCell();
            c.Controls.Add( new LiteralControl("<h3>Přílohy produktu</h3>"));
            r.Cells.Add(c);
            table.Rows.Add(r);
            this.mfuDocuments = new ASPxMultipleFileUpload();
            this.mfuDocuments.ID = "mfuDocuments";
            this.mfuDocuments.MaxfilesToUpload = 10;

            this.ctrlDocuments = CreateDocuments();
            table.Rows.Add(CreateTableRow("Přílohy : ", ctrlDocuments, false, null));
            table.Rows.Add(CreateTableRow("", this.mfuDocuments, false, null));

            //Save Cancel Buttons
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.ColumnSpan = 2;
            cell.Controls.Add(this.btnSave);
            cell.Controls.Add(this.btnCancel);
            row.Cells.Add(cell);
            table.Rows.Add(row);
            
            return table;
        }

        private Table CreateDocuments() {
            Table table = new Table();
            table.CellPadding = 3;
            table.CellSpacing = 0;
            //table.Attributes.Add("border", "1");
            return table;
        }

        private TableRow CreateTableRow(string labelText, Control control, bool required, ValidationDataType? valDataType, String description = null) {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.CssClass = required ? "form_label_required" : "form_label";
            cell.Text = labelText;
            row.Cells.Add(cell);

            if (control == null)
                return row;

            cell = new TableCell();
            cell.CssClass = "form_control";
            cell.Controls.Add(control);
            if (description != null) {
                cell.Controls.Add(new LiteralControl("<span style='color:#878787;padding-left:10px;font-style:italic;'>" + description + "</span>"));
            }

            if (required) cell.Controls.Add(base.CreateRequiredFieldValidatorControl(control.ID));

            switch (valDataType) {
                case ValidationDataType.Integer:
                    cell.Controls.Add(base.CreateNumberValidatorControl(control.ID));
                    break;
                case ValidationDataType.Double:
                    cell.Controls.Add(base.CreateDoubleValidatorControl(control.ID));
                    break;
            }

            row.Cells.Add(cell);
            return row;
        }

        private void RemoveExistingProductDocumentsByPosition(string productDocumentPath, int position) {
            //Delete image
            DirectoryInfo di = new DirectoryInfo(productDocumentPath);
            FileInfo[] fileInfos = di.GetFiles(string.Format("{0:0#}_*.*", position));
            foreach (FileInfo fileInfo in fileInfos)
                fileInfo.Delete();
        }

        private void UpdateProductDocuments(ProductEntity product, FileCollectionEventArgs mfuArgs) {
            if (product == null || mfuArgs == null)
                return;

            if (mfuArgs.PostedFilesInfo.Count == 0)
                return;

            string productImagesPath = Path.Combine(Server.MapPath(ConfigValue("SHP:DocumentGallery:Product:StoragePath")), product.Id.ToString());
            if (!Directory.Exists(productImagesPath))
                Directory.CreateDirectory(productImagesPath);

            foreach (PostedFileInfo fi in mfuArgs.PostedFilesInfo) {
                string desc = fi.Description;

                //Delete existing Product photo on position from file system.
                RemoveExistingProductDocumentsByPosition(productImagesPath, fi.Positon);

                string fileName = string.Format("{0:0#}_{1}", fi.Positon, Path.GetFileName(fi.File.FileName));
                string filePath = Path.Combine(productImagesPath, fileName);

                //Read input stream
                Stream stream = fi.File.InputStream;
                int len = (int)stream.Length;
                if (len == 0) return;
                byte[] data = new byte[len];
                stream.Read(data, 0, len);
                stream.Flush();
                stream.Close();

                //Write new product photo.
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    fs.Write(data, 0, len);
            }
        }


        void OnSave(object sender, EventArgs e) {
            int? pocet = null;
            int? pocetMin = null;
            int? poradiVNN = null;
            int? poradiDS = null;
            int internalStorageCount = -1;
            int? order = null;
            int tmp = 0;

            if (Int32.TryParse(this.txtMaximalniPocetVBaleni.Text, out tmp))
                pocet = tmp;

            if (Int32.TryParse(this.txtMinimalniPocetVBaleni.Text, out tmp))
                pocetMin = tmp;

            if (Int32.TryParse(this.txtVamiNejviceNakupovane.Text, out tmp))
                poradiVNN = tmp;

            if (Int32.TryParse(this.txtDarkovySet.Text, out tmp))
                poradiDS = tmp;

            if (Int32.TryParse(this.txtInternalStorageCount.Text, out tmp))
                internalStorageCount = tmp;

            if (Int32.TryParse(this.txtOrder.Text, out tmp))
                order = tmp;

            DateTime? limitDate = null;
            if (this.dtpLimitDate.Value != null) {
                limitDate = (DateTime)this.dtpLimitDate.Value;
                string[] times = this.txtLimitTime.Text.Split(':');
                if (times.Length == 2) {
                    int hour = 0;
                    int minute = 0;
                    Int32.TryParse(times[0], out hour);
                    Int32.TryParse(times[1], out minute);
                    DateTime date = new DateTime(limitDate.Value.Year, limitDate.Value.Month, limitDate.Value.Day, hour, minute, 0);
                    limitDate = date;
                } else
                    limitDate = null;
            }

            this.product.MinimalniPocetVBaleni = pocetMin;
            this.product.MaximalniPocetVBaleni = pocet;
            this.product.VamiNejviceNakupovane = poradiVNN;
            this.product.DarkovySet = poradiDS;
            this.product.InternalStorageCount = internalStorageCount;
            this.product.LimitDate = limitDate;
            this.product.BSR = this.cbBSRProdukt.Checked;
            this.product.Order = order;

            Storage<ProductEntity>.Update(this.product);

            //Update productDosuments
            UpdateProductDocuments(this.product, this.mfuDocuments.GetUploadEventArgs());


            Response.Redirect(this.ReturnUrl);
        }

        void OnCancel(object sender, EventArgs e) {
            Response.Redirect(this.ReturnUrl);
        }
    }
}
