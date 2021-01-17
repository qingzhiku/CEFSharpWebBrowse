namespace BrowseForms
{
    partial class BrowseMainForm
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
            this.browseTabControl1 = new BrowseForms.BrowseTabControl();
            this.browseTabPage1 = new BrowseTabPage();
            this.browseTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // browseTabControl1
            // 
            this.browseTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browseTabControl1.Controls.Add(this.browseTabPage1);
            this.browseTabControl1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.browseTabControl1.ItemSize = new System.Drawing.Size(119, 45);
            this.browseTabControl1.Location = new System.Drawing.Point(1, 1);
            this.browseTabControl1.Name = "browseTabControl1";
            this.browseTabControl1.SelectedIndex = 0;
            this.browseTabControl1.Size = new System.Drawing.Size(480, 300);
            this.browseTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.browseTabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.browseTabPage1.Location = new System.Drawing.Point(1, 48);
            this.browseTabPage1.Name = "tabPage1";
            this.browseTabPage1.Padding = new System.Windows.Forms.Padding(0);
            this.browseTabPage1.Size = new System.Drawing.Size(480, 255);
            this.browseTabPage1.TabIndex = 0;
            this.browseTabPage1.Text = "新建标签页";
            this.browseTabPage1.UseVisualStyleBackColor = true;
            // 
            // BrowseMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 302);
            this.Controls.Add(this.browseTabControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "BrowseMainForm";
            this.Padding = new System.Windows.Forms.Padding(1, 1, 1, 2);
            this.Text = "cefsharp浏览器";
            this.browseTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BrowseTabControl browseTabControl1;
        private BrowseForms.BrowseTabPage browseTabPage1;
    }
}