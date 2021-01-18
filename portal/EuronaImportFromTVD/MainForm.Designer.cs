
namespace EuronaImportFromTVD {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btnImportData = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lvStatus = new System.Windows.Forms.ListView();
            this.chStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtOdberatele = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtObjednavky = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnImportData
            // 
            this.btnImportData.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnImportData.Location = new System.Drawing.Point(477, 9);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(214, 40);
            this.btnImportData.TabIndex = 35;
            this.btnImportData.Text = "Import Odebatele & Objednavky from TVD";
            this.btnImportData.UseVisualStyleBackColor = true;
            this.btnImportData.Click += new System.EventHandler(this.btnImportData_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblStatus.Location = new System.Drawing.Point(0, 699);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(699, 25);
            this.lblStatus.TabIndex = 34;
            this.lblStatus.Text = "...";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lvStatus
            // 
            this.lvStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvStatus.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chStatus});
            this.lvStatus.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvStatus.HideSelection = false;
            this.lvStatus.Location = new System.Drawing.Point(12, 6);
            this.lvStatus.Name = "lvStatus";
            this.lvStatus.Size = new System.Drawing.Size(675, 410);
            this.lvStatus.TabIndex = 36;
            this.lvStatus.UseCompatibleStateImageBehavior = false;
            this.lvStatus.View = System.Windows.Forms.View.Details;
            // 
            // chStatus
            // 
            this.chStatus.Text = "Status";
            this.chStatus.Width = 1000;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lvStatus);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 220);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(699, 479);
            this.panel1.TabIndex = 37;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(699, 220);
            this.panel2.TabIndex = 38;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 37);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(10);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(10);
            this.splitContainer1.Size = new System.Drawing.Size(699, 183);
            this.splitContainer1.SplitterDistance = 366;
            this.splitContainer1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtOdberatele);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(346, 163);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Odberatele";
            // 
            // txtOdberatele
            // 
            this.txtOdberatele.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOdberatele.Location = new System.Drawing.Point(5, 18);
            this.txtOdberatele.Multiline = true;
            this.txtOdberatele.Name = "txtOdberatele";
            this.txtOdberatele.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOdberatele.Size = new System.Drawing.Size(336, 140);
            this.txtOdberatele.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtObjednavky);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(10, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox2.Size = new System.Drawing.Size(309, 163);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Objednavky";
            // 
            // txtObjednavky
            // 
            this.txtObjednavky.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtObjednavky.Location = new System.Drawing.Point(5, 18);
            this.txtObjednavky.Multiline = true;
            this.txtObjednavky.Name = "txtObjednavky";
            this.txtObjednavky.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObjednavky.Size = new System.Drawing.Size(299, 140);
            this.txtObjednavky.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lblTitle);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(699, 37);
            this.panel4.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblTitle.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(699, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Import odberatelu a objednavek z databaze TVD";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnImportData);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 642);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(699, 57);
            this.panel3.TabIndex = 39;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 724);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblStatus);
            this.Name = "MainForm";
            this.Text = "Import odberatelu a objednavek z databaze TVD";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnImportData;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ListView lvStatus;
        private System.Windows.Forms.ColumnHeader chStatus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtOdberatele;
        private System.Windows.Forms.TextBox txtObjednavky;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

