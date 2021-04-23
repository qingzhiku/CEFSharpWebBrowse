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
    // 包含
    public class BrowseTabPage : TabPage
    {
        public Uri Uri { get; set; }
        public Image Icon { get; set; }

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

              BrowseNav nav = new BrowseNav();
              nav.Dock = DockStyle.Fill;
              // nav.Height = 40;
              nav.BackColor = tabcontrol.TabItemSelectedColor;
              
              Padding = new Padding(0,0,9,0);

              Controls.Add(nav);
            }
        }

    }
}
