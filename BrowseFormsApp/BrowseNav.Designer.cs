
namespace RockerBrowseForms
{
    partial class BrowseNav
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.uritxtbox = new System.Windows.Forms.TextBox();
            this.backbtn = new System.Windows.Forms.Button();
            this.forwardbtn = new System.Windows.Forms.Button();
            this.refreshbtn = new System.Windows.Forms.Button();
            this.enterbtn = new System.Windows.Forms.Button();
            this.webBrowser = new CefSharp.WinForms.ChromiumWebBrowser();
            this.SuspendLayout();
            // 
            // uritxtbox
            // 
            this.uritxtbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uritxtbox.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.uritxtbox.Location = new System.Drawing.Point(99, 5);
            this.uritxtbox.Name = "uritxtbox";
            this.uritxtbox.Size = new System.Drawing.Size(372, 29);
            this.uritxtbox.TabIndex = 0;
            this.uritxtbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.uritxtbox_KeyPress);
            // 
            // backbtn
            // 
            this.backbtn.Enabled = false;
            this.backbtn.FlatAppearance.BorderSize = 0;
            this.backbtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.backbtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.backbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backbtn.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.backbtn.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.backbtn.Location = new System.Drawing.Point(10, 4);
            this.backbtn.Name = "backbtn";
            this.backbtn.Size = new System.Drawing.Size(20, 20);
            this.backbtn.TabIndex = 1;
            this.backbtn.Text = "←";
            this.backbtn.UseVisualStyleBackColor = true;
            this.backbtn.Click += new System.EventHandler(this.backbtn_Click);
            // 
            // forwardbtn
            // 
            this.forwardbtn.Enabled = false;
            this.forwardbtn.FlatAppearance.BorderSize = 0;
            this.forwardbtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.forwardbtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.forwardbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.forwardbtn.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.forwardbtn.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.forwardbtn.Location = new System.Drawing.Point(40, 4);
            this.forwardbtn.Name = "forwardbtn";
            this.forwardbtn.Size = new System.Drawing.Size(20, 20);
            this.forwardbtn.TabIndex = 2;
            this.forwardbtn.Text = "→";
            this.forwardbtn.UseVisualStyleBackColor = true;
            this.forwardbtn.Click += new System.EventHandler(this.forwardbtn_Click);
            // 
            // refreshbtn
            // 
            this.refreshbtn.FlatAppearance.BorderSize = 0;
            this.refreshbtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.refreshbtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.refreshbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshbtn.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.refreshbtn.Location = new System.Drawing.Point(70, 4);
            this.refreshbtn.Name = "refreshbtn";
            this.refreshbtn.Size = new System.Drawing.Size(20, 20);
            this.refreshbtn.TabIndex = 3;
            this.refreshbtn.Text = "↻";
            this.refreshbtn.UseVisualStyleBackColor = true;
            this.refreshbtn.Click += new System.EventHandler(this.refreshbtn_Click);
            // 
            // enterbtn
            // 
            this.enterbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.enterbtn.Location = new System.Drawing.Point(483, 3);
            this.enterbtn.Name = "enterbtn";
            this.enterbtn.Size = new System.Drawing.Size(37, 20);
            this.enterbtn.TabIndex = 5;
            this.enterbtn.Text = "确认";
            this.enterbtn.UseVisualStyleBackColor = true;
            this.enterbtn.Click += new System.EventHandler(this.enterbtn_Click);
            // 
            // webBrowser
            // 
            this.webBrowser.ActivateBrowserOnCreation = true;
            this.webBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser.Location = new System.Drawing.Point(2, 28);
            this.webBrowser.Margin = new System.Windows.Forms.Padding(2);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(634, 300);
            this.webBrowser.TabIndex = 6;
            this.webBrowser.FrameLoadStart += new System.EventHandler<CefSharp.FrameLoadStartEventArgs>(this.webBrowser_FrameLoadStart);
            this.webBrowser.FrameLoadEnd += new System.EventHandler<CefSharp.FrameLoadEndEventArgs>(this.webBrowser_FrameLoadEnd);
            this.webBrowser.StatusMessage += new System.EventHandler<CefSharp.StatusMessageEventArgs>(this.webBrowser_StatusMessage);
            this.webBrowser.AddressChanged += new System.EventHandler<CefSharp.AddressChangedEventArgs>(this.webBrowser_AddressChanged);
            this.webBrowser.TitleChanged += new System.EventHandler<CefSharp.TitleChangedEventArgs>(this.webBrowser_TitleChanged);
            // 
            // BrowseNav
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.enterbtn);
            this.Controls.Add(this.refreshbtn);
            this.Controls.Add(this.forwardbtn);
            this.Controls.Add(this.backbtn);
            this.Controls.Add(this.uritxtbox);
            this.Name = "BrowseNav";
            this.Size = new System.Drawing.Size(636, 330);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox uritxtbox;
        private System.Windows.Forms.Button backbtn;
        private System.Windows.Forms.Button forwardbtn;
        private System.Windows.Forms.Button refreshbtn;
        private System.Windows.Forms.Button enterbtn;
        private CefSharp.WinForms.ChromiumWebBrowser webBrowser;
    }
}
