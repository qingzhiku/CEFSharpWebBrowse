using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RockerBrowseForms
{
    public partial class BrowseNav : UserControl
    {
        public BrowseNav()
        {
            InitializeComponent();

            CEFBrowse.CEFHelper.BindCommonHandler(webBrowser);
            webBrowser.BackColor = Color.White;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e); 
            webBrowser.Load("http://www.baidu.com");
        }

        private void enterbtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(uritxtbox.Text))
                return;

            webBrowser.Load(uritxtbox.Text);
                
        }

        private void backbtn_Click(object sender, EventArgs e)
        {
            if (!webBrowser.CanGoBack)
                return;
            webBrowser.GetBrowser().GoBack();
        }

        private void forwardbtn_Click(object sender, EventArgs e)
        {
            if (!webBrowser.CanGoForward)
                return;
            webBrowser.GetBrowser().GoForward();
        }

        private void refreshbtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(uritxtbox.Text))
                return;

            webBrowser.Load(uritxtbox.Text);
        }

        private void webBrowser_AddressChanged(object sender, CefSharp.AddressChangedEventArgs e)
        {
            if (!(Parent is BrowseForms.BrowseTabPage page))
                return;

            uritxtbox.Text = e.Address;
        }

        private void webBrowser_FrameLoadStart(object sender, CefSharp.FrameLoadStartEventArgs e)
        {
            
        }

        private void webBrowser_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
        }

        private void webBrowser_TitleChanged(object sender, CefSharp.TitleChangedEventArgs e)
        {
            if (!(Parent is BrowseForms.BrowseTabPage page))
                return;

            if (!(Parent.Parent is BrowseForms.BrowseTabControl control))
                return;

            page.Text = e.Title;
            control.Invalidate();
        }

        private void uritxtbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)13)
                return;

            if (string.IsNullOrWhiteSpace(uritxtbox.Text))
                return;

            webBrowser.Load(uritxtbox.Text);
        }

        private void webBrowser_StatusMessage(object sender, CefSharp.StatusMessageEventArgs e)
        {

            if (webBrowser.CanGoBack)
            {
                backbtn.Enabled = true;
                backbtn.ForeColor = SystemColors.ControlText;
            }
            else
            {
                backbtn.ForeColor = SystemColors.ControlLight;
                backbtn.Enabled = false;
            }

            if (webBrowser.CanGoForward)
            {
                forwardbtn.Enabled = true;
                forwardbtn.ForeColor = SystemColors.ControlText;
            }
            else
            {
                forwardbtn.ForeColor = SystemColors.ControlLightLight;
                forwardbtn.Enabled = false;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!(Parent.Parent is BrowseForms.BrowseTabControl control))
                return;

            e.Graphics.DrawLine(new Pen(control.TabItemBorderColor), 0, webBrowser.Top-1, Width, webBrowser.Top-1);
        }


    }
}
