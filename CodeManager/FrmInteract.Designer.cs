namespace CodeManager
{
    partial class FrmInteract
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmInteract));
            this.txtHistory = new System.Windows.Forms.TextBox();
            this.txtCmd = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtHistory
            // 
            this.txtHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHistory.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtHistory.Font = new System.Drawing.Font("宋体", 10F);
            this.txtHistory.Location = new System.Drawing.Point(12, 12);
            this.txtHistory.Multiline = true;
            this.txtHistory.Name = "txtHistory";
            this.txtHistory.ReadOnly = true;
            this.txtHistory.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtHistory.Size = new System.Drawing.Size(903, 403);
            this.txtHistory.TabIndex = 3;
            this.txtHistory.WordWrap = false;
            // 
            // txtCmd
            // 
            this.txtCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCmd.Location = new System.Drawing.Point(12, 421);
            this.txtCmd.Name = "txtCmd";
            this.txtCmd.Size = new System.Drawing.Size(903, 28);
            this.txtCmd.TabIndex = 2;
            this.txtCmd.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtCmd_PreviewKeyDown);
            // 
            // FrmInteract
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 461);
            this.ControlBox = false;
            this.Controls.Add(this.txtCmd);
            this.Controls.Add(this.txtHistory);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmInteract";
            this.ShowInTaskbar = false;
            this.Text = "Aeobro Manager";
            this.Load += new System.EventHandler(this.FrmInteract_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtHistory;
        private System.Windows.Forms.TextBox txtCmd;
    }
}