namespace Proxmark_Tool
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
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpMain = new System.Windows.Forms.TabPage();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.tvMain = new System.Windows.Forms.TreeView();
            this.scMain_2 = new System.Windows.Forms.SplitContainer();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.tvSettings = new DropDownTreeView.DropDownTreeView();
            this.rtbMain = new System.Windows.Forms.RichTextBox();
            this.tcMain.SuspendLayout();
            this.tpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scMain_2)).BeginInit();
            this.scMain_2.Panel2.SuspendLayout();
            this.scMain_2.SuspendLayout();
            this.tpSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpMain);
            this.tcMain.Controls.Add(this.tpSettings);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(584, 332);
            this.tcMain.TabIndex = 1;
            // 
            // tpMain
            // 
            this.tpMain.Controls.Add(this.scMain);
            this.tpMain.Location = new System.Drawing.Point(4, 22);
            this.tpMain.Name = "tpMain";
            this.tpMain.Padding = new System.Windows.Forms.Padding(3);
            this.tpMain.Size = new System.Drawing.Size(576, 306);
            this.tpMain.TabIndex = 0;
            this.tpMain.Text = "Proxmark III";
            this.tpMain.UseVisualStyleBackColor = true;
            // 
            // scMain
            // 
            this.scMain.BackColor = System.Drawing.SystemColors.Control;
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Location = new System.Drawing.Point(3, 3);
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.scMain.Panel1.Controls.Add(this.tvMain);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.BackColor = System.Drawing.SystemColors.Window;
            this.scMain.Panel2.Controls.Add(this.scMain_2);
            this.scMain.Size = new System.Drawing.Size(570, 300);
            this.scMain.SplitterDistance = 190;
            this.scMain.TabIndex = 3;
            // 
            // tvMain
            // 
            this.tvMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvMain.Location = new System.Drawing.Point(0, 0);
            this.tvMain.Name = "tvMain";
            this.tvMain.Size = new System.Drawing.Size(190, 300);
            this.tvMain.TabIndex = 1;
            this.tvMain.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvMain_NodeMouseClick);
            // 
            // scMain_2
            // 
            this.scMain_2.BackColor = System.Drawing.SystemColors.Control;
            this.scMain_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain_2.Location = new System.Drawing.Point(0, 0);
            this.scMain_2.Name = "scMain_2";
            // 
            // scMain_2.Panel1
            // 
            this.scMain_2.Panel1.BackColor = System.Drawing.SystemColors.Window;
            // 
            // scMain_2.Panel2
            // 
            this.scMain_2.Panel2.BackColor = System.Drawing.SystemColors.Window;
            this.scMain_2.Panel2.Controls.Add(this.rtbMain);
            this.scMain_2.Size = new System.Drawing.Size(376, 300);
            this.scMain_2.SplitterDistance = 125;
            this.scMain_2.TabIndex = 0;
            // 
            // tpSettings
            // 
            this.tpSettings.Controls.Add(this.tvSettings);
            this.tpSettings.Location = new System.Drawing.Point(4, 22);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpSettings.Size = new System.Drawing.Size(576, 306);
            this.tpSettings.TabIndex = 1;
            this.tpSettings.Text = "Settings";
            this.tpSettings.UseVisualStyleBackColor = true;
            // 
            // tvSettings
            // 
            this.tvSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSettings.Location = new System.Drawing.Point(3, 3);
            this.tvSettings.Name = "tvSettings";
            this.tvSettings.Size = new System.Drawing.Size(570, 300);
            this.tvSettings.TabIndex = 0;
            // 
            // rtbMain
            // 
            this.rtbMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbMain.Location = new System.Drawing.Point(0, 0);
            this.rtbMain.Name = "rtbMain";
            this.rtbMain.Size = new System.Drawing.Size(247, 300);
            this.rtbMain.TabIndex = 0;
            this.rtbMain.Text = "";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 332);
            this.Controls.Add(this.tcMain);
            this.KeyPreview = true;
            this.Name = "frmMain";
            this.ShowIcon = false;
            this.Text = "Proxmark Tool";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.tcMain.ResumeLayout(false);
            this.tpMain.ResumeLayout(false);
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.scMain_2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain_2)).EndInit();
            this.scMain_2.ResumeLayout(false);
            this.tpSettings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpSettings;
        //private System.Windows.Forms.TreeView tvSettings;

        private DropDownTreeView.DropDownTreeView tvSettings;
        private System.Windows.Forms.TabPage tpMain;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.TreeView tvMain;
        private System.Windows.Forms.SplitContainer scMain_2;
        private System.Windows.Forms.RichTextBox rtbMain;
    }
}

