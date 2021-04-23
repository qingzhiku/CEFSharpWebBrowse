using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RockerBrowseForms;

namespace BrowseForms
{
    // 包含BrowseNavBarControl控件的TabPage
    public class BrowseTabPage : TabPage
    {
        public Static Uri DefaultUri = "http://www.baidu.com";
        public Uri Uri { get; set; }
        public Image Icon { get; set; }
        // 记录选项页下的BrowseNavBarControl
        public BrowseNavBarControl BrowseNavBar {get;private set;}

        public BrowseTabPage()
        {
            BackColor = Color.White;
        }
        
        public BrowseTabPage(string title) : base(title)
        {
            BackColor = Color.White;
        }

        public BrowseTabPage(string title,string url) : this(title)
        {
            Uri = new Uri(url);
        }
        
        public BrowseTabPage(string title, string url, string iconurl) : this(title, url)
        {
            Icon = new Bitmap(iconurl);
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            
            if(Parent is BrowseTabControl tabControl){
              // var tabcontrol = Parent as BrowseTabControl;
              // Panel panel = new Panel();
              // panel.Dock = DockStyle.Top;
              // panel.Height = 40;
              // panel.BackColor = tabcontrol.TabItemSelectedColor;

              //Controls.Add(panel);
              if(BrowseNavBar == null){
                  BrowseNavBarControl = new BrowseNavBarControl();
                  Controls.Clear();
              }
              
              BrowseNavBar.Dock = DockStyle.Fill;
              // bnav.Height = 40;
              BrowseNavBar.BackColor = tabControl.TabItemSelectedColor;

              Controls.Add(BrowseNavBar);
            }
        }

    }
}
