namespace CodeManager
{
    partial class FrmEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEdit));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnConfirm);
            this.panel1.Controls.Add(this.txtTitle);
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1041, 35);
            this.panel1.TabIndex = 0;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(932, 3);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(97, 29);
            this.btnConfirm.TabIndex = 4;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle.Location = new System.Drawing.Point(125, 3);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(801, 28);
            this.txtTitle.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(66, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(53, 18);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 35);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.txtDesc);
            this.splitContainer1.Panel1.Controls.Add(this.lblDesc);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.lblCode);
            this.splitContainer1.Panel2.Controls.Add(this.txtCode);
            this.splitContainer1.Size = new System.Drawing.Size(1041, 703);
            this.splitContainer1.SplitterDistance = 450;
            this.splitContainer1.TabIndex = 1;
            // 
            // txtDesc
            // 
            this.txtDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDesc.Location = new System.Drawing.Point(125, 6);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDesc.Size = new System.Drawing.Size(904, 454);
            this.txtDesc.TabIndex = 2;
            this.txtDesc.WordWrap = false;
            // 
            // lblDesc
            // 
            this.lblDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDesc.Location = new System.Drawing.Point(3, 6);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(116, 438);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = "Description";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCode
            // 
            this.lblCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCode.Location = new System.Drawing.Point(12, 3);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(107, 237);
            this.lblCode.TabIndex = 3;
            this.lblCode.Text = "Code";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCode
            // 
            this.txtCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCode.Location = new System.Drawing.Point(125, 3);
            this.txtCode.Multiline = true;
            this.txtCode.Name = "txtCode";
            this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCode.Size = new System.Drawing.Size(904, 234);
            this.txtCode.TabIndex = 3;
            this.txtCode.WordWrap = false;
            // 
            // FrmEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 738);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmEdit";
            this.ShowInTaskbar = false;
            this.Text = "FrmEdit";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.TextBox txtCode;
    }
}