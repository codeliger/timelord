namespace timelord
{
    partial class frmMain
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.mnuTimesheet = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTimesheetOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTimesheetNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTimesheetClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInvoice = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInvoiceCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.dgvTimesheet = new System.Windows.Forms.DataGridView();
            this.txtTaskName = new System.Windows.Forms.TextBox();
            this.btnTaskToggle = new System.Windows.Forms.Button();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.lblTaskDuration = new System.Windows.Forms.Label();
            this.btnTaskClear = new System.Windows.Forms.Button();
            this.btnTaskSave = new System.Windows.Forms.Button();
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
            this.mnuTimesheetOpen.Size = new System.Drawing.Size(103, 22);
            this.mnuTimesheetOpen.Text = "Open";
            this.mnuTimesheetOpen.Click += new System.EventHandler(this.mnuTimesheetOpen_Click);
            // 
            // mnuTimesheetNew
            // 
            this.mnuTimesheetNew.Name = "mnuTimesheetNew";
            this.mnuTimesheetNew.Size = new System.Drawing.Size(103, 22);
            this.mnuTimesheetNew.Text = "New";
            this.mnuTimesheetNew.Click += new System.EventHandler(this.mnuTimesheetNew_Click);
            // 
            // mnuTimesheetClose
            // 
            this.mnuTimesheetClose.Enabled = false;
            this.mnuTimesheetClose.Name = "mnuTimesheetClose";
            this.mnuTimesheetClose.Size = new System.Drawing.Size(103, 22);
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
            this.dgvTimesheet.AllowUserToAddRows = false;
            this.dgvTimesheet.AllowUserToDeleteRows = false;
            this.dgvTimesheet.AllowUserToResizeColumns = false;
            this.dgvTimesheet.AllowUserToResizeRows = false;
            this.dgvTimesheet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTimesheet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTimesheet.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTimesheet.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvTimesheet.Location = new System.Drawing.Point(0, 67);
            this.dgvTimesheet.Name = "dgvTimesheet";
            this.dgvTimesheet.RowHeadersVisible = false;
            this.dgvTimesheet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTimesheet.Size = new System.Drawing.Size(923, 571);
            this.dgvTimesheet.TabIndex = 1;
            // 
            // txtTaskName
            // 
            this.txtTaskName.AcceptsReturn = true;
            this.txtTaskName.Enabled = false;
            this.txtTaskName.Location = new System.Drawing.Point(80, 41);
            this.txtTaskName.Name = "txtTaskName";
            this.txtTaskName.Size = new System.Drawing.Size(271, 20);
            this.txtTaskName.TabIndex = 2;
            // 
            // btnTaskStart
            // 
            this.btnTaskToggle.Enabled = false;
            this.btnTaskToggle.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnTaskToggle.Location = new System.Drawing.Point(440, 39);
            this.btnTaskToggle.Name = "btnTaskStart";
            this.btnTaskToggle.Size = new System.Drawing.Size(75, 23);
            this.btnTaskToggle.TabIndex = 3;
            this.btnTaskToggle.Text = "Start";
            this.btnTaskToggle.UseVisualStyleBackColor = true;
            this.btnTaskToggle.Click += new System.EventHandler(this.btnTaskToggle_Click);
            // 
            // lblTaskName
            // 
            this.lblTaskName.AutoSize = true;
            this.lblTaskName.Enabled = false;
            this.lblTaskName.Location = new System.Drawing.Point(12, 44);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(62, 13);
            this.lblTaskName.TabIndex = 4;
            this.lblTaskName.Text = "Task Name";
            // 
            // lblTaskDuration
            // 
            this.lblTaskDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaskDuration.Location = new System.Drawing.Point(357, 41);
            this.lblTaskDuration.Name = "lblTaskDuration";
            this.lblTaskDuration.Size = new System.Drawing.Size(77, 20);
            this.lblTaskDuration.TabIndex = 5;
            this.lblTaskDuration.Text = "00:00:00";
            // 
            // btnTaskClear
            // 
            this.btnTaskClear.Enabled = false;
            this.btnTaskClear.Location = new System.Drawing.Point(836, 39);
            this.btnTaskClear.Name = "btnTaskClear";
            this.btnTaskClear.Size = new System.Drawing.Size(75, 23);
            this.btnTaskClear.TabIndex = 6;
            this.btnTaskClear.Text = "Clear";
            this.btnTaskClear.UseVisualStyleBackColor = true;
            this.btnTaskClear.Click += new System.EventHandler(this.btnTaskClear_Click);
            // 
            // btnTaskSave
            // 
            this.btnTaskSave.Enabled = false;
            this.btnTaskSave.Location = new System.Drawing.Point(521, 39);
            this.btnTaskSave.Name = "btnTaskSave";
            this.btnTaskSave.Size = new System.Drawing.Size(75, 23);
            this.btnTaskSave.TabIndex = 7;
            this.btnTaskSave.Text = "Save";
            this.btnTaskSave.UseVisualStyleBackColor = true;
            this.btnTaskSave.Click += new System.EventHandler(this.btnTaskSave_Click);
            // 
            // frmMain
            // 
            this.AcceptButton = this.btnTaskToggle;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 639);
            this.Controls.Add(this.btnTaskSave);
            this.Controls.Add(this.btnTaskClear);
            this.Controls.Add(this.lblTaskDuration);
            this.Controls.Add(this.lblTaskName);
            this.Controls.Add(this.btnTaskToggle);
            this.Controls.Add(this.txtTaskName);
            this.Controls.Add(this.dgvTimesheet);
            this.Controls.Add(this.msMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.msMain;
            this.Name = "frmMain";
            this.ShowIcon = false;
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
        private System.Windows.Forms.Button btnTaskToggle;
        private System.Windows.Forms.Label lblTaskName;
        private System.Windows.Forms.ToolStripMenuItem mnuTimesheetClose;
        private System.Windows.Forms.Label lblTaskDuration;
        private System.Windows.Forms.Button btnTaskClear;
        private System.Windows.Forms.Button btnTaskSave;
    }
}

