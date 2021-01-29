using EuronaImportFromTVD.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EuronaImportFromTVD {
    public partial class MainForm : Form {
        private List<Odberatel> odberateleIds;
        private List<int> objednavkyIds;
        public MainForm() {
            Trace.SetFileListener(System.IO.Path.ChangeExtension(Application.ExecutablePath, ".log"));
            InitializeComponent();

            this.odberateleIds = new List<Odberatel>();
            //this.odberateleIds.Add(1403);
            this.objednavkyIds = new List<int>();
            //this.objednavkyIds.Add(161204);

            //1403;$2y$10$FnGwmHWryKzOLykycNo.gOxJsIdpGotbZ5aKywxSaUgDBXzeBWar2

                /*
            string mySalt = BCrypt.Net.BCrypt.GenerateSalt();
            string myPassword = "Doe12345";
            string myHash = BCrypt.Net.BCrypt.HashPassword(myPassword, mySalt);
            
            bool doesPasswordMatch= BCrypt.Net.BCrypt.Verify(myPassword, "$2y$10$FnGwmHWryKzOLykycNo.gOxJsIdpGotbZ5aKywxSaUgDBXzeBWar2");
                */

/*            user doe@peterslavka.com
            pass Doe12345 => $2y$10$FnGwmHWryKzOLykycNo.gOxJsIdpGotbZ5aKywxSaUgDBXzeBWar2
            sedi aj podla https://bcrypt-generator.com/*/
        }

        private void btnImportData_Click(object sender, EventArgs e) {

            if (!string.IsNullOrEmpty(this.txtOdberatele.Text)) {
                string[] rows = this.txtOdberatele.Text.Split('\n');
                foreach (string row in rows) {
                    string[] odberatelData = row.Split(';');

                    Odberatel odberatel = new Odberatel(Convert.ToInt32(odberatelData[0]), odberatelData[1]);
                    this.odberateleIds.Add(odberatel);
                }
            }

            if (!string.IsNullOrEmpty(this.txtObjednavky.Text)) { 
            string[] ids = this.txtObjednavky.Text.Split('\n');
                foreach (string sId in ids) {
                    this.objednavkyIds.Add(Convert.ToInt32(sId));
                }
            }

            this.btnImportData.Enabled = false;
            Worker worker = new Worker(this.odberateleIds, this.objednavkyIds);
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.Completed += Worker_Completed;
            worker.RunAsync();
        }

        private void Worker_Completed(object sender, RunWorkerCompletedEventArgs args) {
            this.btnImportData.Enabled = true;
            if (args.Result == null) {
                this.lblStatus.Text = args.Error.Message;
                Trace.WriteLine(args.Error);
                return;
            }

            if (args.Error != null)
                this.lvStatus.Items.Add(args.Error.Message);
            else
                this.lvStatus.Items.Add("Hotovo!");

            this.lvStatus.EnsureVisible(this.lvStatus.Items.Count - 1);
            this.lblStatus.Text = args.Result.ToString();
        }

        private void Worker_ProgressChanged(object sender, ProgressEventArgs args) {
            this.lblStatus.Text = args.Message;

            if (args.Type == ProgressEventType.Step) {
                Trace.WriteLine(args.Message);
            } else {
                this.lvStatus.Items.Add(args.Message);
                Trace.WriteLine(args.Message);
            }

            this.lvStatus.EnsureVisible(this.lvStatus.Items.Count - 1);
        }
    }
}
