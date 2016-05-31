namespace timelord
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.mnuTimesheet = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTimesheetOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTimesheetNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTimesheetClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInvoice = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInvoiceCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.dgvTimesheet = new System.Windows.Forms.DataGridView();
            this.txtTaskName = new System.Windows.Forms.TextBox();
            this.btnTaskStart = new System.Windows.Forms.Button();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.msMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTimesheet)).BeginInit();
            this.SuspendLayout();
            // 
            // msMain
            // 
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTimesheet,
            this.mnuInvoice});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Size = new System.Drawing.Size(923, 24);
            this.msMain.TabIndex = 0;
            this.msMain.Text = "msMain";
            // 
            // mnuTimesheet
            // 
            this.mnuTimesheet.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTimesheetOpen,
            this.mnuTimesheetNew,
            this.mnuTimesheetClose});
            this.mnuTimesheet.Name = "mnuTimesheet";
            this.mnuTimesheet.Size = new System.Drawing.Size(74, 20);
            this.mnuTimesheet.Text = "Timesheet";
            // 
            // mnuTimesheetOpen
            // 
            this.mnuTimesheetOpen.Name = "mnuTimesheetOpen";
            this.mnuTimesheetOpen.Size = new System.Drawing.Size(152, 22);
            this.mnuTimesheetOpen.Text = "Open";
            this.mnuTimesheetOpen.Click += new System.EventHandler(this.mnuTimesheetOpen_Click);
            // 
            // mnuTimesheetNew
            // 
            this.mnuTimesheetNew.Name = "mnuTimesheetNew";
            this.mnuTimesheetNew.Size = new System.Drawing.Size(152, 22);
            this.mnuTimesheetNew.Text = "New";
            this.mnuTimesheetNew.Click += new System.EventHandler(this.mnuTimesheetNew_Click);
            // 
            // mnuTimesheetClose
            // 
            this.mnuTimesheetClose.Enabled = false;
            this.mnuTimesheetClose.Name = "mnuTimesheetClose";
            this.mnuTimesheetClose.Size = new System.Drawing.Size(152, 22);
            this.mnuTimesheetClose.Text = "Close";
            this.mnuTimesheetClose.Click += new System.EventHandler(this.mnuTimesheetClose_Click);
            // 
            // mnuInvoice
            // 
            this.mnuInvoice.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuInvoiceCreate});
            this.mnuInvoice.Enabled = false;
            this.mnuInvoice.Name = "mnuInvoice";
            this.mnuInvoice.Size = new System.Drawing.Size(57, 20);
            this.mnuInvoice.Text = "Invoice";
            // 
            // mnuInvoiceCreate
            // 
            this.mnuInvoiceCreate.Name = "mnuInvoiceCreate";
            this.mnuInvoiceCreate.Size = new System.Drawing.Size(108, 22);
            this.mnuInvoiceCreate.Text = "Create";
            // 
            // dgvTimesheet
            // 
            this.dgvTimesheet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTimesheet.Location = new System.Drawing.Point(0, 67);
            this.dgvTimesheet.Name = "dgvTimesheet";
            this.dgvTimesheet.Size = new System.Drawing.Size(923, 571);
            this.dgvTimesheet.TabIndex = 1;
            // 
            // txtTaskName
            // 
            this.txtTaskName.AcceptsReturn = true;
            this.txtTaskName.Enabled = false;
            this.txtTaskName.Location = new System.Drawing.Point(130, 41);
            this.txtTaskName.Name = "txtTaskName";
            this.txtTaskName.Size = new System.Drawing.Size(271, 20);
            this.txtTaskName.TabIndex = 2;
            // 
            // btnTaskStart
            // 
            this.btnTaskStart.Enabled = false;
            this.btnTaskStart.Location = new System.Drawing.Point(407, 39);
            this.btnTaskStart.Name = "btnTaskStart";
            this.btnTaskStart.Size = new System.Drawing.Size(75, 23);
            this.btnTaskStart.TabIndex = 3;
            this.btnTaskStart.Text = "Start";
            this.btnTaskStart.UseVisualStyleBackColor = true;
            // 
            // lblTaskName
            // 
            this.lblTaskName.AutoSize = true;
            this.lblTaskName.Enabled = false;
            this.lblTaskName.Location = new System.Drawing.Point(62, 44);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(62, 13);
            this.lblTaskName.TabIndex = 4;
            this.lblTaskName.Text = "Task Name";
            // 
            // MainWindow
            // 
            this.AcceptButton = this.btnTaskStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 639);
            this.Controls.Add(this.lblTaskName);
            this.Controls.Add(this.btnTaskStart);
            this.Controls.Add(this.txtTaskName);
            this.Controls.Add(this.dgvTimesheet);
            this.Controls.Add(this.msMain);
            this.MainMenuStrip = this.msMain;
            this.Name = "MainWindow";
            this.Text = "Timelord";
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTimesheet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem mnuTimesheet;
        private System.Windows.Forms.ToolStripMenuItem mnuInvoice;
        private System.Windows.Forms.ToolStripMenuItem mnuInvoiceCreate;
        private System.Windows.Forms.ToolStripMenuItem mnuTimesheetOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuTimesheetNew;
        private System.Windows.Forms.DataGridView dgvTimesheet;
        private System.Windows.Forms.TextBox txtTaskName;
        private System.Windows.Forms.Button btnTaskStart;
        private System.Windows.Forms.Label lblTaskName;
        private System.Windows.Forms.ToolStripMenuItem mnuTimesheetClose;
    }
}

