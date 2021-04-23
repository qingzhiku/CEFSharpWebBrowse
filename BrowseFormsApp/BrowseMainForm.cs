using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrowseForms
{
    public partial class BrowseMainForm : SkinMain
    {
        public BrowseMainForm()
        {
            MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            Size = new Size(Convert.ToInt32(MaximumSize.Width*0.8), Convert.ToInt32(MaximumSize.Height*0.8));
            
            InitializeComponent();

            RockerBrowseForms.CEFBrowse.CEFHelper.InitializeChromium();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        //#region 必须，移动窗体
        public Color TabItemSelectedBorderColor => Color.FromArgb(163, 163, 163);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawRectangle(new Pen(TabItemSelectedBorderColor), ClientRectangle.X, 
                ClientRectangle.Y, ClientRectangle.Width-1, ClientRectangle.Height-1);
        }



        //const int HTLEFT = 10;
        //const int HTRIGHT = 11;
        //const int HTTOP = 12;
        //const int HTTOPLEFT = 13;
        //const int HTTOPRIGHT = 14;
        //const int HTBOTTOM = 15;
        //const int HTBOTTOMLEFT = 0x10;
        //const int HTBOTTOMRIGHT = 17;
        //protected override void WndProc(ref Message m)
        //{
        //    switch (m.Msg)
        //    {
        //        case 0x0084:
        //            base.WndProc(ref m);
        //            Point vPoint = new Point((int)m.LParam & 0xFFFF,
        //                (int)m.LParam >> 16 & 0xFFFF);
        //            vPoint = PointToClient(vPoint);
        //            if (vPoint.X <= 5)
        //                if (vPoint.Y <= 5)
        //                    m.Result = (IntPtr)HTTOPLEFT;
        //                else if (vPoint.Y >= ClientSize.Height - 5)
        //                    m.Result = (IntPtr)HTBOTTOMLEFT;
        //                else m.Result = (IntPtr)HTLEFT;
        //            else if (vPoint.X >= ClientSize.Width - 5)
        //                if (vPoint.Y <= 5)
        //                    m.Result = (IntPtr)HTTOPRIGHT;
        //                else if (vPoint.Y >= ClientSize.Height - 5)
        //                    m.Result = (IntPtr)HTBOTTOMRIGHT;
        //                else m.Result = (IntPtr)HTRIGHT;
        //            else if (vPoint.Y <= 5)
        //                m.Result = (IntPtr)HTTOP;
        //            else if (vPoint.Y >= ClientSize.Height - 5)
        //                m.Result = (IntPtr)HTBOTTOM;
        //            break;
        //        case 0x0201://鼠标左键按下的消息
        //            m.Msg = 0x00A1;//更改消息为非客户区按下鼠标
        //            m.LParam = IntPtr.Zero;//默认值
        //            m.WParam = new IntPtr(2);//鼠标放在标题栏内
        //            base.WndProc(ref m);
        //            break;
        //        default:
        //            base.WndProc(ref m);
        //            break;
        //    }

        //}

        //#endregion

    }
}
