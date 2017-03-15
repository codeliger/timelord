namespace timelord
{
    partial class frmInvoice
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
            this.cbIdentity = new System.Windows.Forms.ComboBox();
            this.lblIdentity = new System.Windows.Forms.Label();
            this.lblClient = new System.Windows.Forms.Label();
            this.cbClient = new System.Windows.Forms.ComboBox();
            this.btnIdentityEdit = new System.Windows.Forms.Button();
            this.btnClientEdit = new System.Windows.Forms.Button();
            this.btnIdentityNew = new System.Windows.Forms.Button();
            this.btnClientNew = new System.Windows.Forms.Button();
            this.dgvInvoice = new System.Windows.Forms.DataGridView();
            this.btnCreate = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnPath = new System.Windows.Forms.Button();
            this.btnIdentityDelete = new System.Windows.Forms.Button();
            this.btnClientDelete = new System.Windows.Forms.Button();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.duration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoice)).BeginInit();
            this.SuspendLayout();
            // 
            // cbIdentity
            // 
            this.cbIdentity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIdentity.FormattingEnabled = true;
            this.cbIdentity.Location = new System.Drawing.Point(84, 17);
            this.cbIdentity.Name = "cbIdentity";
            this.cbIdentity.Size = new System.Drawing.Size(181, 21);
            this.cbIdentity.TabIndex = 0;
            // 
            // lblIdentity
            // 
            this.lblIdentity.AutoSize = true;
            this.lblIdentity.Location = new System.Drawing.Point(12, 20);
            this.lblIdentity.Name = "lblIdentity";
            this.lblIdentity.Size = new System.Drawing.Size(66, 13);
            this.lblIdentity.TabIndex = 1;
            this.lblIdentity.Text = "Your Identity";
            // 
            // lblClient
            // 
            this.lblClient.AutoSize = true;
            this.lblClient.Location = new System.Drawing.Point(12, 51);
            this.lblClient.Name = "lblClient";
            this.lblClient.Size = new System.Drawing.Size(70, 13);
            this.lblClient.TabIndex = 2;
            this.lblClient.Text = "Client Identity";
            // 
            // cbClient
            // 
            this.cbClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClient.FormattingEnabled = true;
            this.cbClient.Location = new System.Drawing.Point(84, 47);
            this.cbClient.Name = "cbClient";
            this.cbClient.Size = new System.Drawing.Size(181, 21);
            this.cbClient.TabIndex = 3;
            // 
            // btnIdentityEdit
            // 
            this.btnIdentityEdit.Location = new System.Drawing.Point(271, 15);
            this.btnIdentityEdit.Name = "btnIdentityEdit";
            this.btnIdentityEdit.Size = new System.Drawing.Size(75, 23);
            this.btnIdentityEdit.TabIndex = 4;
            this.btnIdentityEdit.Text = "Edit";
            this.btnIdentityEdit.UseVisualStyleBackColor = true;
            this.btnIdentityEdit.Click += new System.EventHandler(this.btnIdentityEdit_Click);
            // 
            // btnClientEdit
            // 
            this.btnClientEdit.Location = new System.Drawing.Point(272, 45);
            this.btnClientEdit.Name = "btnClientEdit";
            this.btnClientEdit.Size = new System.Drawing.Size(75, 23);
            this.btnClientEdit.TabIndex = 5;
            this.btnClientEdit.Text = "Edit";
            this.btnClientEdit.UseVisualStyleBackColor = true;
            this.btnClientEdit.Click += new System.EventHandler(this.btnClientEdit_Click);
            // 
            // btnIdentityNew
            // 
            this.btnIdentityNew.Location = new System.Drawing.Point(352, 15);
            this.btnIdentityNew.Name = "btnIdentityNew";
            this.btnIdentityNew.Size = new System.Drawing.Size(75, 23);
            this.btnIdentityNew.TabIndex = 6;
            this.btnIdentityNew.Text = "New";
            this.btnIdentityNew.UseVisualStyleBackColor = true;
            this.btnIdentityNew.Click += new System.EventHandler(this.btnIdentityNew_Click);
            // 
            // btnClientNew
            // 
            this.btnClientNew.Location = new System.Drawing.Point(353, 44);
            this.btnClientNew.Name = "btnClientNew";
            this.btnClientNew.Size = new System.Drawing.Size(75, 23);
            this.btnClientNew.TabIndex = 7;
            this.btnClientNew.Text = "New";
            this.btnClientNew.UseVisualStyleBackColor = true;
            this.btnClientNew.Click += new System.EventHandler(this.btnClientNew_Click);
            // 
            // dgvInvoice
            // 
            this.dgvInvoice.AllowUserToAddRows = false;
            this.dgvInvoice.AllowUserToDeleteRows = false;
            this.dgvInvoice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInvoice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.description,
            this.cost,
            this.duration});
            this.dgvInvoice.Location = new System.Drawing.Point(15, 74);
            this.dgvInvoice.Name = "dgvInvoice";
            this.dgvInvoice.Size = new System.Drawing.Size(493, 355);
            this.dgvInvoice.TabIndex = 8;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(406, 433);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(102, 23);
            this.btnCreate.TabIndex = 9;
            this.btnCreate.Text = "Create Invoice";
            this.btnCreate.UseVisualStyleBackColor = true;
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(123, 435);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(277, 20);
            this.txtPath.TabIndex = 10;
            // 
            // btnPath
            // 
            this.btnPath.Location = new System.Drawing.Point(15, 433);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(102, 23);
            this.btnPath.TabIndex = 11;
            this.btnPath.Text = "Choose Location";
            this.btnPath.UseVisualStyleBackColor = true;
            // 
            // btnIdentityDelete
            // 
            this.btnIdentityDelete.Location = new System.Drawing.Point(433, 15);
            this.btnIdentityDelete.Name = "btnIdentityDelete";
            this.btnIdentityDelete.Size = new System.Drawing.Size(75, 23);
            this.btnIdentityDelete.TabIndex = 12;
            this.btnIdentityDelete.Text = "Delete";
            this.btnIdentityDelete.UseVisualStyleBackColor = true;
            this.btnIdentityDelete.Click += new System.EventHandler(this.btnIdentityDelete_Click);
            // 
            // btnClientDelete
            // 
            this.btnClientDelete.Location = new System.Drawing.Point(433, 44);
            this.btnClientDelete.Name = "btnClientDelete";
            this.btnClientDelete.Size = new System.Drawing.Size(75, 23);
            this.btnClientDelete.TabIndex = 13;
            this.btnClientDelete.Text = "Delete";
            this.btnClientDelete.UseVisualStyleBackColor = true;
            this.btnClientDelete.Click += new System.EventHandler(this.btnClientDelete_Click);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.Visible = false;
            // 
            // description
            // 
            this.description.DataPropertyName = "description";
            this.description.HeaderText = "Description";
            this.description.Name = "description";
            // 
            // cost
            // 
            this.cost.DataPropertyName = "rate";
            this.cost.HeaderText = "Hourly Cost";
            this.cost.Name = "cost";
            // 
            // duration
            // 
            this.duration.DataPropertyName = "duration";
            this.duration.HeaderText = "Duration";
            this.duration.Name = "duration";
            // 
            // frmInvoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 463);
            this.Controls.Add(this.btnClientDelete);
            this.Controls.Add(this.btnIdentityDelete);
            this.Controls.Add(this.btnPath);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.dgvInvoice);
            this.Controls.Add(this.btnClientNew);
            this.Controls.Add(this.btnIdentityNew);
            this.Controls.Add(this.btnClientEdit);
            this.Controls.Add(this.btnIdentityEdit);
            this.Controls.Add(this.cbClient);
            this.Controls.Add(this.lblClient);
            this.Controls.Add(this.lblIdentity);
            this.Controls.Add(this.cbIdentity);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmInvoice";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Timelord Invoice";
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbIdentity;
        private System.Windows.Forms.Label lblIdentity;
        private System.Windows.Forms.Label lblClient;
        private System.Windows.Forms.ComboBox cbClient;
        private System.Windows.Forms.Button btnIdentityEdit;
        private System.Windows.Forms.Button btnClientEdit;
        private System.Windows.Forms.Button btnIdentityNew;
        private System.Windows.Forms.Button btnClientNew;
        private System.Windows.Forms.DataGridView dgvInvoice;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnPath;
        private System.Windows.Forms.Button btnIdentityDelete;
        private System.Windows.Forms.Button btnClientDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost;
        private System.Windows.Forms.DataGridViewTextBoxColumn duration;
    }
}