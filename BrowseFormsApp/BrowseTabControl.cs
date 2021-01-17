using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Design;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.Design;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

namespace BrowseForms
{
    [ToolboxItem(typeof(BrowseToolboxItem))]
    public class BrowseTabControl : TabControl
    {
        // tabitem
        const int CLOSE_SIZE = 16;
        const int ADD_SIZE = 15;
        const int ICON_SIZE = 16;
        const int TabItem_Horization_Padding = 9;
        private Size _itemSize = Size.Empty;

        // tabpage
        const int Right_MIN_WIDTH = 200;
        const int LEFT_FIXED_WIDTH = 8;
        const int TOP_FIXED_HEIGHT = 9;
        const int TAB_MAX_WIDTH = 230;

        // mouse
        bool mousecanmoveDown = false;
        MouseEventArgs mouseEventArgs = null;

        // index
        int _lastDisplayTabIndex = -1;

        // MessageFilter
        IMessageFilter messageFilter = null;

        #region user32
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private const int VM_NCLBUTTONDOWN = 0XA1;//定义鼠标左键按下
        private const int HTCAPTION = 2; 
        #endregion

        #region ICON_IMAGE

        private Bitmap _add_Btn_Normal = null;
        private Bitmap _add_Btn_Hover = null;
        private Bitmap _add_Btn_Down = null;
        private Bitmap _close_Btn_Normal = null;
        private Bitmap _close_Btn_Hover = null;
        private Bitmap _close_Btn_Down = null;
        private string _defalticonstring = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAHqSURBVDhPfVO/ixpREHZdf6x3kuQwueYgncJdMJBiLQNpbGy8xkZLEe0sr7PyDxAsRQT/ARtbq6vsAwoHVmoU8ScGq53MN/t24x53fvCYb755O29m3lsfQERB2Ol0emVZVhj8cDh8g3Vix+PR5FgE/CLW6/XHYrFI4XCYdrtdgiUKhUKExNvt1rR3vYPhcPhjMBhkmJJagPB+v28lk0nqdrtFEYn8sAKUXq1WKZfLUb1eJ7/f70kAv9FoUKlUgrZpt9vPEiDSpWQ4AAsaL9dHC/D3+/1nxHgOd9DZ/wnLLd34uLfrSqUiJ45Go6doNCocpyLmVJNIJGg+n9+CBwIBKpfLnNMesC8SicgmcCcBBrfZbB7BdV0XbbVafYDuaZGzGJqmiYDSzoL2hv/c4weDQVRoX6v08go4DVfHBwSwoE0mE0OCCqxrQs5bUNYp24d+HQ2+srLcBIVCwd2QSqXcDfBh0aJpmn/H4/E9ZgCtVqvZD42z6MiEQKvV+pXNZikWi7kJ8CLB0+k0dTqd70qn2Wz2wN/Zj8klCovFIgq7XC4zSM7D/SIBBr+br4p6wVf2iZ/xb36yL3yVf1iSCmDj8Tj1ej3LMAxqNpv01tA9yOfzbgvK0ul0wo8l4KpCil6G0wraEOEViEj/B7wRQP4+IfrNAAAAAElFTkSuQmCC";
        private string _add_btn_normal_string = "iVBORw0KGgoAAAANSUhEUgAAAB0AAAAdCAYAAABWk2cPAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAA7SURBVEhLYxgFo2AUDF9gbGz8H8qkHxi1lKZg+FkKMpxUDNVKfUBTw3GBUUtpCkaOpaNgFIwCOgEGBgAJwyfZ62FyDwAAAABJRU5ErkJggg==";
        private string _systembuttonmin = "iVBORw0KGgoAAAANSUhEUgAAADcAAAAlCAYAAADmxXHWAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAkSURBVGhD7cOxCQAACAOw/v+0/tBNSSABAAAAHpgiAAAAcFiy53gM9D32TvMAAAAASUVORK5CYII=";
        private string _systembuttonnormal = "iVBORw0KGgoAAAANSUhEUgAAADcAAAAlCAYAAADmxXHWAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAA8SURBVGhD7c+xCQAwDANB7790Qjq3TiXDHajXFwAALHA+tsb0rLgU4hpxKcQ14lKIa8SleGenAwCAQFUX3wcv0c4l1bYAAAAASUVORK5CYII=";
        private string _systembuttonmax = "iVBORw0KGgoAAAANSUhEUgAAADcAAAAlCAYAAADmxXHWAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAABNSURBVGhD7dGxCsAgEETB+/+fTkwh2KndbZiBxfYeFgAAhHguFuf06Ki49Ud2+8w3wumx4roRN4jrRtzw+7i5GDdxcdYf2Q0AAJqregE8Uz7CCaaQ+wAAAABJRU5ErkJggg==";
        private string _systembuttonclose = "iVBORw0KGgoAAAANSUhEUgAAAC0AAAAdCAYAAAA+YOU3AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAABRSURBVFhH7dAxCgAwCARB///phEAEiyBW4Q52mmssFgMAAFXrbmdy810XJRmcXnHSwalGWgSnE2sVfNhF11iL8FekdHgXJxk+iZL+OAAAbiI2crgT7XoeAAEAAAAASUVORK5CYII=";
        private Bitmap _systembuttonmin_Normal = null;
        private Bitmap _systembuttonmin_Hover = null;
        private Bitmap _systembuttonmin_Down = null;
        private Bitmap _systembuttonnormal_Normal = null;
        private Bitmap _systembuttonnormal_Hover = null;
        private Bitmap _systembuttonnormal_Down = null;
        private Bitmap _systembuttonclose_Normal = null;
        private Bitmap _systembuttonclose_Hover = null;
        private Bitmap _systembuttonclose_Down = null;
        private Bitmap _systembuttonmax_Normal = null;
        private Bitmap _systembuttonmax_Hover = null;
        private Bitmap _systembuttonmax_Down = null;

        //public Bitmap Add_Btn_Normal => _add_Btn_Normal ?? (_add_Btn_Normal = TabHelper.GetBitmapFromGraphicePath(TabHelper.CreateAddGraphicsPath(new Rectangle(0, 0, 8, 8)), new Size(ADD_SIZE, ADD_SIZE)));
        public Bitmap Add_Btn_Normal => _add_Btn_Normal ?? (_add_Btn_Normal = (Bitmap)TabHelper.ConvertBase64ToImage(_add_btn_normal_string));
        //public Bitmap Add_Btn_Hover => _add_Btn_Hover ?? (_add_Btn_Hover = TabHelper.GetBitmapFromGraphicePath(TabHelper.CreateAddGraphicsPath(new Rectangle(0, 0, 8, 8)), new Size(ADD_SIZE, ADD_SIZE), 20));
        public Bitmap Add_Btn_Hover => _add_Btn_Hover ?? (_add_Btn_Hover = TabHelper.IamgeFillBackColor(TabHelper.ConvertBase64ToImage(_add_btn_normal_string), Color.FromArgb(40, Color.Black), false));
        //public Bitmap Add_Btn_Down => _add_Btn_Down ?? (_add_Btn_Down = TabHelper.GetBitmapFromGraphicePath(TabHelper.CreateAddGraphicsPath(new Rectangle(0, 0, 8, 8)), new Size(ADD_SIZE, ADD_SIZE), 40));
        public Bitmap Add_Btn_Down => _add_Btn_Down ?? (_add_Btn_Down = TabHelper.IamgeFillBackColor(TabHelper.ConvertBase64ToImage(_add_btn_normal_string), Color.FromArgb(80, Color.Black), true));
        public Bitmap Close_Btn_Normal => _close_Btn_Normal ?? (_close_Btn_Normal = TabHelper.GetBitmapFromGraphicePath(TabHelper.CreateCloseGraphicsPath(new Rectangle(0, 0, 8, 8)), new Size(CLOSE_SIZE, CLOSE_SIZE)));
        public Bitmap Close_Btn_Hover => _close_Btn_Hover ?? (_close_Btn_Hover = TabHelper.GetBitmapFromGraphicePath(TabHelper.CreateCloseGraphicsPath(new Rectangle(0, 0, 8, 8)), new Size(CLOSE_SIZE, CLOSE_SIZE), 20));
        public Bitmap Close_Btn_Down => _close_Btn_Down ?? (_close_Btn_Down = TabHelper.GetBitmapFromGraphicePath(TabHelper.CreateCloseGraphicsPath(new Rectangle(0, 0, 8, 8)), new Size(CLOSE_SIZE, CLOSE_SIZE), 40));
        public Image DefaltICON => TabHelper.ConvertBase64ToImage(_defalticonstring);
        public Bitmap SystemButtonMinBox_Normal => _systembuttonmin_Normal ?? (_systembuttonmin_Normal = TabHelper.IamgeFillBackColor(TabHelper.ConvertBase64ToImage(_systembuttonmin), Color.Transparent,false));
        public Bitmap SystemButtonMinBox_Hover =>    _systembuttonmin_Hover?? (_systembuttonmin_Hover= TabHelper.IamgeFillBackColor(TabHelper.ConvertBase64ToImage(_systembuttonmin), Color.FromArgb(40,Color.Black),false));
        public Bitmap SystemButtonMinBox_Down =>    _systembuttonmin_Down ?? (_systembuttonmin_Down = TabHelper.IamgeFillBackColor(TabHelper.ConvertBase64ToImage(_systembuttonmin), Color.FromArgb(80, Color.Black),true));
        public Bitmap SystemButtonMaxBox_Normal => _systembuttonmax_Normal ?? (_systembuttonmax_Normal = TabHelper.IamgeFillBackColor(TabHelper.ConvertBase64ToImage(_systembuttonmax), Color.Transparent,false));
        public Bitmap SystemButtonMaxBox_Hover =>    _systembuttonmax_Hover?? (_systembuttonmax_Hover= TabHelper.IamgeFillBackColor(TabHelper.ConvertBase64ToImage(_systembuttonmax), Color.FromArgb(40,Color.Black),false));
        public Bitmap SystemButtonMaxBox_Down => _systembuttonmax_Down ?? (_systembuttonmax_Down = TabHelper.IamgeFillBackColor(TabHelper.ConvertBase64ToImage(_systembuttonmax), Color.FromArgb(80, Color.Black),true));
        public Bitmap SystemButtonNormalBox_Normal => _systembuttonnormal_Normal ?? (_systembuttonnormal_Normal = TabHelper.IamgeFillBackColor(TabHelper.ConvertBase64ToImage(_systembuttonnormal), Color.Transparent,false));
        public Bitmap SystemButtonNormalBox_Hover =>    _systembuttonnormal_Hover?? (_systembuttonnormal_Hover= TabHelper.IamgeFillBackColor(TabHelper.ConvertBase64ToImage(_systembuttonnormal), Color.FromArgb(40,Color.Black),false));
        public Bitmap SystemButtonNormalBox_Down => _systembuttonnormal_Down ?? (_systembuttonnormal_Down = TabHelper.IamgeFillBackColor(TabHelper.ConvertBase64ToImage(_systembuttonnormal), Color.FromArgb(80, Color.Black),true));
        public Bitmap SystemButtonCloseBox_Normal => _systembuttonclose_Normal ?? (_systembuttonclose_Normal = TabHelper.IamgeFillBackColor(TabHelper.ConvertBase64ToImage(_systembuttonclose), Color.Transparent,false));
        public Bitmap SystemButtonCloseBox_Hover =>    _systembuttonclose_Hover?? (_systembuttonclose_Hover= TabHelper.IamgeFillBackColor(TabHelper.ConvertBase64ToImage(_systembuttonclose), Color.FromArgb(180,Color.Red),false));
        public Bitmap SystemButtonCloseBox_Down => _systembuttonclose_Down ?? (_systembuttonclose_Down = TabHelper.IamgeFillBackColor(TabHelper.ConvertBase64ToImage(_systembuttonclose), Color.FromArgb(255, Color.Red),true));


        private Color _tabHeaderBackgroundColor = Color.FromArgb(231, 231, 231);
        private Color _tabItemNormalColor = Color.FromArgb(220, 220, 220);
        private Color _tabItemHoverColor = Color.FromArgb(235, 235, 235);
        private Color _tabItemSelectedColor = Color.FromArgb(248, 248, 248);
        private Color _tabItemSelectedBorderColor = Color.FromArgb(163, 163, 163);
        private Color _tabItemBorderColor = Color.FromArgb(194, 194, 194);

        public Color TabHeaderBackgroundColor { get => _tabHeaderBackgroundColor; set => _tabHeaderBackgroundColor = value; }
        public Color TabItemNormalColor { get => _tabItemNormalColor; set => _tabItemNormalColor = value; }
        public Color TabItemHoverColor { get => _tabItemHoverColor; set => _tabItemHoverColor = value; }
        public Color TabItemSelectedColor { get => _tabItemSelectedColor; set => _tabItemSelectedColor = value; }
        public Color TabItemSelectedBorderColor { get => _tabItemSelectedBorderColor; set => _tabItemSelectedBorderColor = value; }
        public Color TabItemBorderColor { get => _tabItemBorderColor; set => _tabItemBorderColor = value; }

        #endregion

        public int LastDisplayTabIndex => _lastDisplayTabIndex;

        /// <summary> 
        /// 解决系统TabControl多余边距问题 
        /// </summary> 
        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle rect = base.DisplayRectangle;
                return new Rectangle(rect.Left - 3, rect.Top - 1, rect.Width + 8, rect.Height + 8);
            }
        }

        public virtual Rectangle TabRectangle
        {
            get
            {
                return new Rectangle(ClientRectangle.Location, new Size(ClientSize.Width, ItemSize.Height));
            }
        }

        public virtual Rectangle SystemButtonBounds => new Rectangle(Width - 39 * 3 - 3, 0, 39 * 3, 27); 

        public new Size ItemSize
        {
            get
            {
                return _itemSize;
            }
            set
            {
                _itemSize = value;
                if (base.ItemSize.Height != _itemSize.Height)
                    base.ItemSize = new Size(0, _itemSize.Height);
            }
        }

        public BrowseTabControl() : base()
        {
            SizeMode = TabSizeMode.Fixed;
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            Font = new Font("微软雅黑", 9F);
            ItemSize = new Size(230, 45);
            messageFilter = new BrowseMessageFilter(this);
            Application.AddMessageFilter(messageFilter);
        }

        protected override void OnCreateControl()
        {
            SetStyle(
                ControlStyles.DoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();

            base.OnCreateControl();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if(Parent != null)
            {
                Parent.BackColor = TabHeaderBackgroundColor;
            }
        }

        protected override void OnSelecting(TabControlCancelEventArgs e)
        { 
           //if(mouseDown)
           // {
                //e.Cancel = true;
            //}
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            _lastDisplayTabIndex++;
            base.OnControlAdded(e);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            _lastDisplayTabIndex--;
            base.OnControlRemoved(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;//画图质量
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            
            // 重置选项卡大小
            SetTabItemSize();

            //创建缓冲 Graphics对象，区域
            using (BufferedGraphics bufferedGraphics = BufferedGraphicsManager.Current.Allocate(e.Graphics, ClientRectangle))
            {
                // 绘制全部区域背景
                OnDrawTabItemBackground(bufferedGraphics.Graphics, ClientRectangle);

                OnDrawSystemButton(new PaintEventArgs(bufferedGraphics.Graphics, SystemButtonBounds));

                var mousepoint = PointToClient(MousePosition);
                int mousehovertabindex = -1;

                int totalwidth = ClientRectangle.Width - Right_MIN_WIDTH - LEFT_FIXED_WIDTH;
                int addwidth = ADD_SIZE + TabItem_Horization_Padding * 2;
                int tabtotalwidth = totalwidth - addwidth;
                int mintabwidth = TabItem_Horization_Padding * 2 + CLOSE_SIZE;
                int mintabwidth_maxcount = (int)Math.Floor(tabtotalwidth * 1d / mintabwidth);

                int displaymaxindex = Math.Min(TabCount, mintabwidth_maxcount);

                for (int index = 0; index < displaymaxindex; index++)
                {
                    if (SelectedIndex == index)
                        continue;

                    var itemrect = GetTabRect(index);
                    if (itemrect.Contains(mousepoint))
                    {
                        mousehovertabindex = index;
                        continue;
                    }

                    OnDrawTabItem(new DrawItemEventArgs(bufferedGraphics.Graphics, Font, itemrect, index, DrawItemState.None));
                }

                if (mousehovertabindex > -1 && mousehovertabindex != SelectedIndex)
                {
                    OnDrawTabItem(new DrawItemEventArgs(bufferedGraphics.Graphics, Font, GetTabRect(mousehovertabindex), mousehovertabindex, DrawItemState.HotLight));
                }

                if (SelectedIndex < displaymaxindex)
                    OnDrawSelectedTabItem(new DrawItemEventArgs(bufferedGraphics.Graphics, Font, GetTabRect(SelectedIndex), SelectedIndex, DrawItemState.Selected));

                OnDrawAddTabItem(new DrawItemEventArgs(bufferedGraphics.Graphics, Font, GetAddRect(), -1, DrawItemState.None));


                //获取缓冲 Graphics对象，区域
                bufferedGraphics.Render(e.Graphics);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            mousecanmoveDown = true;
            mouseEventArgs = e;

            int totalwidth = ClientRectangle.Width - Right_MIN_WIDTH - LEFT_FIXED_WIDTH;
            int addwidth = ADD_SIZE + TabItem_Horization_Padding * 2;
            int tabtotalwidth = totalwidth - addwidth;
            int mintabwidth = TabItem_Horization_Padding * 2 + CLOSE_SIZE;
            int mintabwidth_maxcount = (int)Math.Floor(tabtotalwidth * 1d / mintabwidth);

            for (int index = 0; index < Math.Min(TabCount, mintabwidth_maxcount); index++)
            {
                var rect = GetTabRect(index);
                if (rect.Contains(e.Location))
                {
                    
                    var closerect = GetTabCloseRect(rect,SelectedIndex == index ? DrawItemState.Selected:DrawItemState.None);
                    if (!closerect.Contains(e.Location))
                    {
                        SelectedIndex = index;
                        if(closerect.Width<=1) 
                            mouseEventArgs = null;
                    }
                    mousecanmoveDown = false;
                    break;
                }
            }

            if(GetAddRect().Contains(e.Location) || SystemButtonBounds.Contains(e.Location))
                mousecanmoveDown = false;

            Invalidate();

            if (e.Button == MouseButtons.Left && mousecanmoveDown && mouseEventArgs != null && FindForm() != null)
            {
                ReleaseCapture();
                SendMessage((IntPtr)FindForm().Handle, VM_NCLBUTTONDOWN, HTCAPTION, 0);

                mousecanmoveDown = false;
                mouseEventArgs = null;
            }

        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            var form = FindForm();

            if (form != null && e.Button == MouseButtons.Left)
            {
                if (GetAddRect().Contains(e.Location))
                    return;
                if (SystemButtonBounds.Contains(e.Location))
                    return;

                int totalwidth = ClientRectangle.Width - Right_MIN_WIDTH - LEFT_FIXED_WIDTH;
                int addwidth = ADD_SIZE + TabItem_Horization_Padding * 2;
                int tabtotalwidth = totalwidth - addwidth;
                int mintabwidth = TabItem_Horization_Padding * 2 + CLOSE_SIZE;
                int mintabwidth_maxcount = (int)Math.Floor(tabtotalwidth * 1d / mintabwidth);

                for (int index = 0; index < Math.Min(TabCount, mintabwidth_maxcount); index++)
                {
                    if (GetTabRect(index).Contains(e.Location))
                        return;
                }


                if (form.WindowState == FormWindowState.Maximized)
                    form.WindowState = FormWindowState.Normal;
                else
                {
                    form.WindowState = FormWindowState.Maximized;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            //TabPages[0].Text = e.Location.ToString();

            if (mouseEventArgs == null)
            {
                Invalidate();
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                // 关闭按钮
                bool isclose = false;
                int totalwidth = ClientRectangle.Width - Right_MIN_WIDTH - LEFT_FIXED_WIDTH;
                int addwidth = ADD_SIZE + TabItem_Horization_Padding * 2;
                int tabtotalwidth = totalwidth - addwidth;
                int mintabwidth = TabItem_Horization_Padding * 2 + CLOSE_SIZE;
                int mintabwidth_maxcount = (int)Math.Floor(tabtotalwidth * 1d / mintabwidth);

                for (int index = 0; index < Math.Min(TabCount, mintabwidth_maxcount); index++)
                {
                    var closerect = GetTabCloseRect(GetTabRect(index), SelectedIndex == index ? DrawItemState.Selected : DrawItemState.None);
                    if (closerect.Contains(e.Location) && closerect.Contains(e.Location))
                    {
                        if (TabCount > 1)
                            TabPages.RemoveAt(index);
                        isclose = true;
                        break;
                    }
                }

                // 添加浏览器标签
                if (!isclose)
                {
                    Rectangle addrect = GetAddRect();
                    if (addrect.Contains(mouseEventArgs.Location) && addrect.Contains(e.Location))
                    {
                        TabPages.Add(new BrowseTabPage("新建标签页"));
                        SelectedIndex = TabCount - 1;
                        isclose = true;
                    }
                }

                if (!isclose)
                {
                    if (SystemButtonBounds.Contains(mouseEventArgs.Location)  && SystemButtonBounds.Contains(e.Location))
                    {
                        SystemButtonType sbtype = SystemButtonType.None;
                        var sbrect = GetSystemButton(SystemButtonType.CloseBox);

                        if (sbrect.Contains(mouseEventArgs.Location) && sbrect.Contains(e.Location))
                        {
                            sbtype = SystemButtonType.CloseBox;
                        }
                        if (sbtype == SystemButtonType.None)
                        {
                            sbrect = GetSystemButton(SystemButtonType.MaxBox);
                            if (sbrect.Contains(mouseEventArgs.Location) && sbrect.Contains(e.Location))
                            {
                                sbtype = SystemButtonType.MaxBox;
                            }
                        }
                        if (sbtype == SystemButtonType.None)
                        {
                            sbrect = GetSystemButton(SystemButtonType.MinBox);
                            if (sbrect.Contains(mouseEventArgs.Location) && sbrect.Contains(e.Location))
                            {
                                sbtype = SystemButtonType.MinBox;
                            }
                        }

                        switch (sbtype)
                        {
                            case SystemButtonType.CloseBox:
                                Application.Exit();
                                break;
                            case SystemButtonType.MaxBox:
                                if(FindForm().WindowState == FormWindowState.Maximized)
                                {
                                    FindForm().WindowState = FormWindowState.Normal;
                                }else if(FindForm().WindowState == FormWindowState.Normal)
                                {
                                    FindForm().WindowState = FormWindowState.Maximized;
                                }
                                break;
                            case SystemButtonType.MinBox:
                                FindForm().WindowState = FormWindowState.Minimized;
                                break;
                        }

                    }

                    
                }

            }


            mousecanmoveDown = false;
            mouseEventArgs = null;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //var form = FindForm();
            //this.FindForm().Text = e.Location.ToString();

            //if(e.Button == MouseButtons.Left && mousecanmoveDown && mouseEventArgs != null && form != null)
            //{
            //    if(form.WindowState == FormWindowState.Maximized)
            //    {
            //        form.WindowState = FormWindowState.Normal;
            //    }


            //    var space = new Point(e.X - mouseEventArgs.X,e.Y-mouseEventArgs.Y);
            //    form.Location = new Point(form.Location.X + space.X, form.Location.Y + space.Y);
            //    return;
            //}

            base.OnMouseMove(e);
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mousecanmoveDown = false;
            mouseEventArgs = null;
            Invalidate();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            var form = FindForm();
            var spaceh = form?.WindowState ?? FormWindowState.Normal;
            if(spaceh == FormWindowState.Maximized)
            {
                ItemSize = new Size(ItemSize.Width, 40);
            }
            else
            {
                ItemSize = new Size(ItemSize.Width, 45);
            }
            base.OnSizeChanged(e);

        }
        protected virtual void OnDrawTabItem(DrawItemEventArgs e)
        {
            var mousepoint = PointToClient(MousePosition);
            // 背景色
            //e.Graphics.FillRectangle(new SolidBrush(TabItemNormalColor), e.Bounds);
            e.Graphics.FillPath(new SolidBrush(TabItemNormalColor), TabHelper.CreateTabitemBackGraphicsPath(e.Bounds, TabBorderStyle.Outset, TabBorderStyle.Outset));
            if (e.Index != 0 && e.State == DrawItemState.None)
                e.Graphics.DrawLine(new Pen(TabItemSelectedBorderColor), e.Bounds.Left, e.Bounds.Top + 8, e.Bounds.Left, e.Bounds.Bottom - 8);
            if (e.State == DrawItemState.HotLight)
            {
                e.Graphics.FillPath(new SolidBrush(TabItemHoverColor), TabHelper.CreateTabitemBackGraphicsPath(e.Bounds, TabBorderStyle.Outset, TabBorderStyle.Outset));
                e.Graphics.DrawPath(new Pen(TabItemHoverColor), TabHelper.CreateTabitemBackGraphicsPath(e.Bounds, TabBorderStyle.Outset, TabBorderStyle.Outset,false)); 
                e.Graphics.DrawLine(new Pen(TabItemSelectedBorderColor), e.Bounds.Left - 8, e.Bounds.Bottom, e.Bounds.Right + 8, e.Bounds.Bottom);
            }

            //e.Graphics.DrawRectangle(Pens.Red, e.Bounds);
            //e.Graphics.DrawPath(Pens.Red, TabHelper.CreateTabitemBackGraphicsPath(e.Bounds,TabBorderStyle.Outset,TabBorderStyle.Outset));

            // 绘制文字
            var textrect = GetTabTextRect(e.Bounds);
            StringFormat sf = new StringFormat();
            sf.Trimming = StringTrimming.None;
            sf.FormatFlags = StringFormatFlags.NoWrap;

            var textrectf = new RectangleF(textrect.X, textrect.Y, textrect.Width, textrect.Height);
            
            if (textrect.Width > 2)
            {
                LinearGradientBrush linearGradientBrush = new LinearGradientBrush(textrectf, Color.Transparent, Color.Transparent, LinearGradientMode.Horizontal);
                ColorBlend colorBlend = new ColorBlend();
                colorBlend.Colors = new[] { Color.Black, Color.Black, Color.Transparent };
                colorBlend.Positions = new[] { 0.0f, 0.85f, 1.0f };
                linearGradientBrush.InterpolationColors = colorBlend;

                e.Graphics.DrawString(TabPages[e.Index].Text, Font, linearGradientBrush /*Brushes.Black*/, textrect, sf);
            }
                
            //e.Graphics.DrawRectangle(Pens.Blue, textrect);

            // 绘制关闭按钮 
            var closerect = GetTabCloseRect(e.Bounds);
            if (mouseEventArgs != null)
            {
                if (closerect.Contains(mouseEventArgs.Location) && closerect.Contains(mousepoint) && mouseEventArgs.Button == MouseButtons.Left)
                {
                    e.Graphics.DrawImage(Close_Btn_Down, closerect);
                }
                else
                {
                    e.Graphics.DrawImage(Close_Btn_Normal, closerect);
                }
            }
            else
            {
                if (closerect.Contains(mousepoint))
                    e.Graphics.DrawImage(Close_Btn_Hover, closerect);
                else
                    e.Graphics.DrawImage(Close_Btn_Normal, closerect);
            }

            //var textrect = GetTabTextRect(e.Bounds);

            var iconrect = GetTabICONRect(e.Bounds);
            e.Graphics.DrawImage(DefaltICON, iconrect);

        }

        protected virtual void OnDrawSelectedTabItem(DrawItemEventArgs e)
        {
            var mousepoint = PointToClient(MousePosition);
            // 背景色
            var backgraphicspath = TabHelper.CreateTabitemBackGraphicsPath(e.Bounds, TabBorderStyle.Outset, TabBorderStyle.Outset);
            //e.Graphics.FillRectangle(new SolidBrush(TabItemSelectedColor), e.Bounds);
            e.Graphics.FillPath(new SolidBrush(TabItemSelectedColor), backgraphicspath);
            e.Graphics.DrawPath(new Pen(TabItemSelectedColor), backgraphicspath);
            e.Graphics.DrawPath(new Pen(TabItemSelectedBorderColor), TabHelper.CreateTabitemBackGraphicsPath(e.Bounds, TabBorderStyle.Outset, TabBorderStyle.Outset,false));

            //e.Graphics.DrawRectangle(Pens.Red, e.Bounds);

            // 绘制文字
            var textrect = GetTabTextRect(e.Bounds, DrawItemState.Selected);
            StringFormat sf = new StringFormat();
            sf.Trimming = StringTrimming.None;
            sf.FormatFlags = StringFormatFlags.NoWrap;

            if (textrect.Width > 1)
            {
                var textrectf = new RectangleF(textrect.X, textrect.Y, textrect.Width, textrect.Height);
                LinearGradientBrush linearGradientBrush = new LinearGradientBrush(textrectf, Color.Transparent, Color.Transparent, LinearGradientMode.Horizontal);
                ColorBlend colorBlend = new ColorBlend();
                colorBlend.Colors = new[] { Color.Black, Color.Black, Color.Transparent };
                colorBlend.Positions = new[] { 0.0f, 0.85f, 1.0f };
                linearGradientBrush.InterpolationColors = colorBlend;

                e.Graphics.DrawString(TabPages[e.Index].Text, Font, linearGradientBrush, textrect, sf);
            }
            //e.Graphics.DrawRectangle(Pens.Blue, textrect);

            //关闭按钮
            var closerect = GetTabCloseRect(e.Bounds,DrawItemState.Selected);
            if (mouseEventArgs != null)
            {
                if (closerect.Contains(mouseEventArgs.Location) && closerect.Contains(mousepoint) && mouseEventArgs.Button == MouseButtons.Left)
                {
                    e.Graphics.DrawImage(Close_Btn_Down, closerect);
                }
                else
                {
                    e.Graphics.DrawImage(Close_Btn_Normal, closerect);
                }
            }
            else
            {
                if (closerect.Contains(mousepoint))
                    e.Graphics.DrawImage(Close_Btn_Hover, closerect);
                else
                    e.Graphics.DrawImage(Close_Btn_Normal, closerect);
            }

            var iconrect = GetTabICONRect(e.Bounds,DrawItemState.Selected);
            e.Graphics.DrawImage(DefaltICON, iconrect);

        }

        protected virtual void OnDrawAddTabItem(DrawItemEventArgs e)
        {
            //e.Graphics.DrawRectangle(Pens.Red, e.Bounds);
            var mousepoint = PointToClient(MousePosition);

            if (mouseEventArgs != null)
            {
                if (e.Bounds.Contains(mouseEventArgs.Location) && e.Bounds.Contains(mousepoint) && mouseEventArgs.Button == MouseButtons.Left)
                {
                    e.Graphics.DrawImage(Add_Btn_Down, e.Bounds);
                }
                else
                {
                    e.Graphics.DrawImage(Add_Btn_Normal, e.Bounds);
                }
            }
            else
            {
                if (e.Bounds.Contains(mousepoint))
                    e.Graphics.DrawImage(Add_Btn_Hover, e.Bounds);
                else
                    e.Graphics.DrawImage(Add_Btn_Normal, e.Bounds);
            }

        }

        protected virtual void OnDrawSystemButton(PaintEventArgs args)
        {
            var form = FindForm();
            if (form == null) return;
            var windowstate = form.WindowState;

            var mousepoint = PointToClient(MousePosition);
            var minr = GetSystemButton(SystemButtonType.MinBox);
            var minmouser = new Rectangle(minr.X, 5, minr.Width, minr.Height - 5);
            var maxr = GetSystemButton(SystemButtonType.MaxBox);
            var maxmouser = new Rectangle(maxr.X, 5, maxr.Width, maxr.Height - 5);
            var closer = GetSystemButton(SystemButtonType.CloseBox);
            var closemouser = new Rectangle(closer.X, 5, closer.Width-5, closer.Height - 5);

            //args.Graphics.DrawRectangle(Pens.Red, GetSystemButton(SystemButtonType.CloseBox));
            //args.Graphics.DrawRectangle(Pens.Red, GetSystemButton(SystemButtonType.MaxBox));
            //args.Graphics.DrawRectangle(Pens.Red, GetSystemButton(SystemButtonType.MinBox));

            if (mouseEventArgs != null)
            {
                if (minr.Contains(mouseEventArgs.Location) && minmouser.Contains(mousepoint) && mouseEventArgs.Button == MouseButtons.Left)
                {
                    args.Graphics.DrawImage(SystemButtonMinBox_Down, minr);
                }
                else
                {
                    args.Graphics.DrawImage(SystemButtonMinBox_Normal, minr);
                }

                if (maxr.Contains(mouseEventArgs.Location) && maxmouser.Contains(mousepoint) && mouseEventArgs.Button == MouseButtons.Left)
                {
                    args.Graphics.DrawImage(windowstate == FormWindowState.Maximized?SystemButtonMaxBox_Down:SystemButtonNormalBox_Down, maxr);
                }
                else
                {
                    args.Graphics.DrawImage(windowstate == FormWindowState.Maximized ? SystemButtonMaxBox_Normal: SystemButtonNormalBox_Normal, maxr);
                }

                if (closer.Contains(mouseEventArgs.Location) && closemouser.Contains(mousepoint) && mouseEventArgs.Button == MouseButtons.Left)
                {
                    args.Graphics.DrawImage(SystemButtonCloseBox_Down, closer);
                }
                else
                {
                    args.Graphics.DrawImage(SystemButtonCloseBox_Normal, closer);
                }
            }
            else
            {
                if (minmouser.Contains(mousepoint))
                {
                    args.Graphics.DrawImage(SystemButtonMinBox_Hover, minr);
                }
                else
                {
                    args.Graphics.DrawImage(SystemButtonMinBox_Normal, minr);
                }

                if (maxmouser.Contains(mousepoint))
                {
                    args.Graphics.DrawImage(windowstate == FormWindowState.Maximized ? SystemButtonMaxBox_Hover:SystemButtonNormalBox_Hover, maxr);
                }
                else
                {
                    args.Graphics.DrawImage(windowstate == FormWindowState.Maximized ? SystemButtonMaxBox_Normal : SystemButtonNormalBox_Normal, maxr);
                }

                if (closemouser.Contains(mousepoint))
                {
                    args.Graphics.DrawImage(SystemButtonCloseBox_Hover, closer);
                }
                else
                {
                    args.Graphics.DrawImage(SystemButtonCloseBox_Normal, closer);
                }

            }

        }

        protected virtual void OnDrawTabItemBackground(Graphics g, Rectangle rect)
        {
            //填充背景
            g.FillRectangle(new SolidBrush(TabHeaderBackgroundColor), rect);
            g.DrawLine(new Pen(TabItemSelectedBorderColor), rect.X, ItemSize.Height+2, rect.Right, ItemSize.Height+2);
            
        }

        protected virtual void SetTabItemSize()
        {
            ItemSize = new Size(TAB_MAX_WIDTH, base.ItemSize.Height);
            if (TabCount <= 0) return;

            int totalwidth = ClientRectangle.Width - Right_MIN_WIDTH - LEFT_FIXED_WIDTH;
            int addwidth = ADD_SIZE + TabItem_Horization_Padding * 2;
            int tabtotalwidth = totalwidth - addwidth;
            int currenttotalwidth = ItemSize.Width * TabCount;
            int mintabwidth = TabItem_Horization_Padding * 2 + CLOSE_SIZE;
            int maxtabwidth_mincount = (int)Math.Floor(tabtotalwidth * 1d / TAB_MAX_WIDTH);
            int mintabwidth_maxcount = (int)Math.Floor(tabtotalwidth * 1d / mintabwidth);

            int itemw = ItemSize.Width;
            if(currenttotalwidth >= tabtotalwidth)
            {
                if (TabCount >= mintabwidth_maxcount)
                {
                    itemw = (int)Math.Floor(tabtotalwidth / (float)mintabwidth_maxcount);
                }
                else if(TabCount > maxtabwidth_mincount)
                {
                    itemw = (int)Math.Floor(tabtotalwidth / (float)TabCount);
                }
            }


            if (itemw != ItemSize.Width)
                ItemSize = new Size(itemw, ItemSize.Height);
        }

        public new Rectangle GetTabRect(int index)
        {
            var form = FindForm();
            var fwstate = form?.WindowState ?? FormWindowState.Normal;
            var top = fwstate == FormWindowState.Maximized ? 0 : TOP_FIXED_HEIGHT;

            if (index < 0 || index >= TabCount) return Rectangle.Empty;
            Rectangle bounds = new Rectangle(0, 0, ItemSize.Width, ItemSize.Height - top + 2);
            bounds.Offset(LEFT_FIXED_WIDTH + ItemSize.Width * index, top);
            return bounds;
        }

        protected virtual Rectangle GetAddRect()
        {
            int totalwidth = ClientRectangle.Width - Right_MIN_WIDTH - LEFT_FIXED_WIDTH;
            int addwidth = ADD_SIZE + TabItem_Horization_Padding * 2;
            int tabtotalwidth = totalwidth - addwidth;
            int mintabwidth = TabItem_Horization_Padding * 2 + CLOSE_SIZE;
            int maxtabwidth_mincount = (int)Math.Floor(tabtotalwidth * 1d / TAB_MAX_WIDTH);
            int mintabwidth_maxcount = (int)Math.Floor(tabtotalwidth * 1d / mintabwidth);
            int remainder = totalwidth % TAB_MAX_WIDTH;

            int left = ClientRectangle.Width - Right_MIN_WIDTH - addwidth;
            if (TabCount <= maxtabwidth_mincount)
            {
                left = TAB_MAX_WIDTH * TabCount + LEFT_FIXED_WIDTH;
            }

            var form = FindForm();
            var fwstate = form?.WindowState ?? FormWindowState.Normal;
            var top = fwstate == FormWindowState.Maximized ? 0 : TOP_FIXED_HEIGHT;
            if (ItemSize.Height - ADD_SIZE > 0)
            {
                top += (ItemSize.Height - ADD_SIZE) / 2 - (fwstate == FormWindowState.Maximized ? 0 :2);
            }

            return new Rectangle(left+ TabItem_Horization_Padding, top- TabItem_Horization_Padding/2, ADD_SIZE+ TabItem_Horization_Padding, ADD_SIZE+ TabItem_Horization_Padding);
        }

        protected virtual Rectangle GetTabCloseRect(Rectangle tabRect, DrawItemState state = DrawItemState.None)
        {
            int fontsize = (int)(Font.Size * 3);
            int iconw = ICON_SIZE;
            int closew = CLOSE_SIZE;
            int paddw = TabItem_Horization_Padding * 2;

            var rect = new Rectangle(tabRect.Right - CLOSE_SIZE,
                tabRect.Top + (tabRect.Height - ADD_SIZE) / 2,
                CLOSE_SIZE, CLOSE_SIZE);

            rect.Offset(-TabItem_Horization_Padding, 0);

            if (state == DrawItemState.None)
            {
                if (tabRect.Width <= paddw + iconw + fontsize)
                {
                    rect = Rectangle.Empty;
                }
            }

            return rect;
        }

        protected virtual Rectangle GetTabICONRect(Rectangle tabRect, DrawItemState state = DrawItemState.None)
        {
            int fontsize = (int)(Font.Size * 3);
            int iconw = ICON_SIZE;
            int closew = CLOSE_SIZE;
            int paddw = TabItem_Horization_Padding * 3;

            var rect = new Rectangle(tabRect.Left,
                tabRect.Top + (tabRect.Height - ICON_SIZE) / 2,
                 ICON_SIZE, ICON_SIZE);

            rect.Offset(TabItem_Horization_Padding, 0);

            if(state == DrawItemState.Selected)
            {
                if (tabRect.Width <= paddw + closew + iconw)
                {
                    rect = Rectangle.Empty;
                }
            }

            return rect;
        }

        protected virtual Rectangle GetTabTextRect(Rectangle tabRect, DrawItemState state = DrawItemState.None)
        {
            var iconrect = GetTabICONRect(tabRect, state);
            var closerect = GetTabCloseRect(tabRect, state);

            int remainedw = (closerect.Left <= tabRect.Left ? tabRect.Right:closerect.Left) - Math.Max(tabRect.Left,iconrect.Right);
            int textw = remainedw - TabItem_Horization_Padding * 2;

            var rect = new Rectangle(
                Math.Max(tabRect.Left, iconrect.Right),
                tabRect.Top + (tabRect.Height - FontHeight) / 2,
                 textw,
                FontHeight);

            rect.Offset(TabItem_Horization_Padding, 0);
            
            return rect;
        }

        // 使用FormWindowState表示关闭 最大化 最小化三个按钮
        protected virtual Rectangle GetSystemButton(SystemButtonType buttonType)
        {
            Rectangle rect = new Rectangle(0, 0, 39, 27);
            switch (buttonType)
            {
                case SystemButtonType.CloseBox: // 关闭按钮
                    rect.Offset(Width - rect.Width, 0);
                    break;
                case SystemButtonType.MaxBox:
                    rect.Offset(Width - rect.Width*2 , 0);
                    break;
                case SystemButtonType.MinBox:
                    rect.Offset(Width - rect.Width*3 , 0);
                    break;
            }

            return rect;
        }

        protected override void Dispose(bool disposing)
        {
            if (messageFilter != null)
                Application.RemoveMessageFilter(messageFilter);
            base.Dispose(disposing);
        }

        public class BrowseMessageFilter : IMessageFilter
        {
            BrowseTabControl control = null;
            public BrowseMessageFilter(BrowseTabControl tabControl)
            {
                control = tabControl;
            }

            public bool PreFilterMessage(ref Message m)
            {
                bool flag = false;
                var point = control.PointToClient(MousePosition);
                var treck = new Rectangle(control.TabRectangle.X, control.TabRectangle.Y - 2, control.TabRectangle.Width, control.TabRectangle.Height + 10);

                switch (m.Msg)
                {
                    case 0x0200://鼠标移动
                        if (treck.Contains(point))
                        {
                            control.OnMouseMove(new MouseEventArgs(MouseButtons, 1, point.X, point.Y, 0));

                            //if(control.ClientRectangle.X - point.X  < 2 || control.ClientRectangle.Right - point.X < 2 ||
                            //    control.ClientRectangle.Y - point.Y < 2 || control.ClientRectangle.Bottom - point.Y < 2)
                            //{
                            //    control.OnMouseLeave(EventArgs.Empty);
                            //}

                            return true;
                        }
                        else if(!control.ClientRectangle.Contains(point))
                        {
                            control.OnMouseLeave(EventArgs.Empty);
                        }
                        break;
                    case 0x201://鼠标左键Down
                        if (control.TabRectangle.Contains(point))
                        {
                            control.OnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, point.X, point.Y, 0));
                            return true;
                        }
                        break;
                    case 0x0202://鼠标左键UP
                        if (control.TabRectangle.Contains(point))
                        {
                            control.OnMouseUp(new MouseEventArgs(MouseButtons.Left, 1, point.X, point.Y, 0));
                            return true;
                        }
                        else
                        {
                            control.OnMouseUp(new MouseEventArgs(MouseButtons.Left, 1, point.X, point.Y, 0));
                        }
                        break;
                    case 0x0203://鼠标左键双击LDoubleClick
                        if (control.TabRectangle.Contains(point))
                        {
                            control.OnMouseDoubleClick(new MouseEventArgs(MouseButtons.Left, 1, point.X, point.Y, 0));
                            return true;
                        }
                        break;
                    case 0x0204://鼠标右键Down
                        if (control.TabRectangle.Contains(point))
                        {
                            control.OnMouseDown(new MouseEventArgs(MouseButtons.Right, 1, point.X, point.Y, 0));
                            return true;
                        }
                        break;
                    case 0x0205://鼠标右键UP
                        if (control.TabRectangle.Contains(point))
                        {
                            control.OnMouseUp(new MouseEventArgs(MouseButtons.Right, 1, point.X, point.Y, 0));
                            return true;
                        }
                        break;
                    case 0x0206://双击鼠标右键RDoubleClick
                        if (control.TabRectangle.Contains(point))
                        {
                            control.OnMouseDoubleClick(new MouseEventArgs(MouseButtons.Right, 1, point.X, point.Y, 0));
                            return true;
                        }
                        break;
                    case 0x0207://鼠标中键Down
                        if (control.TabRectangle.Contains(point))
                        {
                            control.OnMouseDown(new MouseEventArgs(MouseButtons.Middle, 1, point.X, point.Y, 0));
                            return true;
                        }
                        break;
                    case 0x0208://鼠标中键UP
                        if (control.TabRectangle.Contains(point))
                        {
                            control.OnMouseUp(new MouseEventArgs(MouseButtons.Middle, 1, point.X, point.Y, 0));
                            return true;
                        }
                        break;
                    case 0x0209://鼠标中键双击MDoubleDown
                        if (control.TabRectangle.Contains(point))
                        {
                            control.OnMouseDoubleClick(new MouseEventArgs(MouseButtons.Middle, 1, point.X, point.Y, 0));
                            return true;
                        }
                        break;
                }

                return flag;
            }


        }



        internal class TabHelper
        {
            public static GraphicsPath CreateAddGraphicsPath(Rectangle rect)
            {
                GraphicsPath path = new GraphicsPath();
                path.AddLine(rect.Width / 2, rect.Top, rect.Width / 2, rect.Bottom);
                path.CloseFigure();
                path.AddLine(rect.Left, rect.Height / 2, rect.Right, rect.Height / 2);
                path.CloseFigure();
                return path;
            }

            public static GraphicsPath CreateCloseGraphicsPath(Rectangle rect)
            {
                GraphicsPath path = new GraphicsPath();
                path.AddLine(rect.Left, rect.Top, rect.Right, rect.Bottom);
                path.CloseFigure();
                path.AddLine(rect.Left, rect.Bottom, rect.Right, rect.Top);
                path.CloseFigure();
                return path;
            }

            public static GraphicsPath CreateTabitemBackGraphicsPath(Rectangle rect, TabBorderStyle leftBorderStyle = TabBorderStyle.None, 
                TabBorderStyle rightBorderStyle = TabBorderStyle.None,bool isclosebottomFigure = true, int radius=8)
            {
                GraphicsPath path = new GraphicsPath();

                //path.StartFigure();
                switch (leftBorderStyle)
                {
                    case TabBorderStyle.None:
                        if (isclosebottomFigure)
                            path.AddLine(new Point(rect.Right, rect.Bottom), new Point(rect.Left, rect.Bottom));
                        path.AddLine(new Point(rect.Left, rect.Bottom), new Point(rect.X, rect.Y + radius));
                        break;
                    case TabBorderStyle.Outset:
                        if (isclosebottomFigure)
                            path.AddLine(new Point(rect.Right + radius, rect.Bottom), new Point(rect.Left, rect.Bottom));
                        path.AddArc(new Rectangle(new Point(rect.X - 2 * radius, rect.Bottom - 2 * radius), new Size(2 * radius, 2 * radius)), 90, -90);//左下角
                        path.AddLine(new Point(rect.X, rect.Bottom - radius), new Point(rect.X, rect.Y + radius));
                        break;
                    case TabBorderStyle.Inset:
                        if (isclosebottomFigure)
                            path.AddLine(new Point(rect.Right - radius, rect.Bottom), new Point(rect.X + radius, rect.Bottom));
                        path.AddArc(new Rectangle(new Point(rect.X, rect.Bottom - 2 * radius), new Size(2 * radius, 2 * radius)), 90, 90);//左下角
                        path.AddLine(new Point(rect.X, rect.Bottom - radius), new Point(rect.X, rect.Y + radius));
                        break;
                }

                //path.StartFigure();
                path.AddArc(new Rectangle(new Point(rect.X, rect.Y), new Size(2 * radius, 2 * radius)), 180, 90); // 左上角
                path.AddLine(new Point(rect.X + radius, rect.Y), new Point(rect.Right - radius, rect.Y));
                path.AddArc(new Rectangle(new Point(rect.Right - 2 * radius, rect.Y), new Size(2 * radius, 2 * radius)), 270, 90); // 右上角
                path.AddLine(new Point(rect.Right, rect.Y + radius), new Point(rect.Right, rect.Bottom - radius));

                switch (rightBorderStyle)
                {
                    case TabBorderStyle.None:
                        path.AddLine(new Point(rect.Right, rect.Bottom - radius), new Point(rect.Right, rect.Bottom));
                        break;
                    case TabBorderStyle.Outset:
                        path.AddArc(new Rectangle(new Point(rect.Right, rect.Bottom - 2* radius),new Size(2 * radius, 2 * radius)),180,-90);
                        break;
                    case TabBorderStyle.Inset:
                        path.AddArc(new Rectangle(new Point(rect.Right, rect.Y + radius), new Size(2 * radius, 2 * radius)), 0, 90);
                        break;
                }
                
                //path.CloseFigure();

                //path.AddArc(new Rectangle(new Point(rect.Right - 2 * radius, rect.Bottom - 2 * radius), new Size(2 * radius, 2 * radius)), 0, 90);//右下角
                //path.AddLine(new Point(rect.Right - radius, rect.Bottom), new Point(rect.X + radius, rect.Bottom));
                //path.AddArc(new Rectangle(new Point(rect.X, rect.Bottom - 2 * radius), new Size(2 * radius, 2 * radius)), 90, 90);//左下角
                //path.AddLine(new Point(rect.X, rect.Bottom - radius), new Point(rect.X, rect.Y + radius));
                //path.CloseFigure();
                path.FillMode = FillMode.Alternate;

                return path;
            }

            public static GraphicsPath CreateArcGraphicsPath(Rectangle rect, int radius)
            {
                // 指定图形路径， 有一系列 直线/曲线 组成
                GraphicsPath path = new GraphicsPath();
                path.StartFigure();
                path.AddArc(new Rectangle(new Point(rect.X, rect.Y), new Size(2 * radius, 2 * radius)), 180, 90);
                path.AddLine(new Point(rect.X + radius, rect.Y), new Point(rect.Right - radius, rect.Y));
                path.AddArc(new Rectangle(new Point(rect.Right - 2 * radius, rect.Y), new Size(2 * radius, 2 * radius)), 270, 90);
                path.AddLine(new Point(rect.Right, rect.Y + radius), new Point(rect.Right, rect.Bottom - radius));
                path.AddArc(new Rectangle(new Point(rect.Right - 2 * radius, rect.Bottom - 2 * radius), new Size(2 * radius, 2 * radius)), 0, 90);
                path.AddLine(new Point(rect.Right - radius, rect.Bottom), new Point(rect.X + radius, rect.Bottom));
                path.AddArc(new Rectangle(new Point(rect.X, rect.Bottom - 2 * radius), new Size(2 * radius, 2 * radius)), 90, 90);
                path.AddLine(new Point(rect.X, rect.Bottom - radius), new Point(rect.X, rect.Y + radius));
                path.CloseFigure();

                return path;
            }

            public static Bitmap GetBitmapFromGraphicePath(GraphicsPath gpath, Size imagesize, int backgroundAlpha = 0)
            {
                Bitmap bitmap = new Bitmap(imagesize.Width, imagesize.Height, PixelFormat.Format32bppPArgb);

                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.FillPath(new SolidBrush(Color.FromArgb(backgroundAlpha, Color.Black)), TabHelper.CreateArcGraphicsPath(new Rectangle(0, 0, imagesize.Width, imagesize.Height), 2));

                    graphics.TranslateTransform(Math.Max((imagesize.Width - gpath.GetBounds().Width) / 2,0), Math.Max((imagesize.Height - gpath.GetBounds().Height) / 2,0));
                    using (GraphicsPath path = gpath)
                    {
                        using (Pen pen = new Pen(new SolidBrush(Color.FromArgb(250, Color.Black)), 1.55f))
                        {
                            pen.StartCap = LineCap.Round;
                            pen.EndCap = LineCap.Round;
                            pen.Alignment = PenAlignment.Center;
                            graphics.DrawPath(pen, path);
                        }
                    }
                    graphics.ResetTransform();
                }
                
                return bitmap;
            }

            public static Image ConvertBase64ToImage(string base64String)
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    return Image.FromStream(ms, true);
                }
            }

            public static string ConvertImageToBase64(Image file)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    file.Save(memoryStream, file.RawFormat);
                    byte[] imageBytes = memoryStream.ToArray();
                    return Convert.ToBase64String(imageBytes);
                }
            }

            public static Bitmap IamgeFillBackColor(Image image, Color backcolor,bool isinvert)
            {
                var bitmap = new Bitmap(image.Width, image.Height);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;//画图质量
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.Clear(backcolor);
                    g.DrawImage(isinvert?ConvertToInvert(image): image, new Point(0, 0));
                }

                return bitmap;
            }

            public static Bitmap ConvertToInvert(Image img)
            {
                // 读入欲转换的图片并转成为 WritableBitmap www.it165.net
               Bitmap bitmap = new Bitmap(img);
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        // 取得每一个 pixel
                        var pixel = bitmap.GetPixel(x, y);

                        // 负片效果 将其反转
                        Color newColor = Color.FromArgb(pixel.A, 255 - pixel.R, 255 - pixel.G, 255 - pixel.B);

                        bitmap.SetPixel(x, y, newColor);

                    }
                }
                // 显示结果
                return bitmap;
            }

        }

        internal enum TabBorderStyle
        {
            None = 1,
            Inset = 2,
            Outset = 4
        }

        public enum SystemButtonType
        {
            None,
            CloseBox,
            MaxBox,
            MinBox
        }

    }

    [Serializable]
    public class BrowseToolboxItem : ToolboxItem
    {
        public BrowseToolboxItem(Type toolType) : base(toolType)
        {
        }

        public BrowseToolboxItem(SerializationInfo info, StreamingContext context)
        {
            Deserialize(info, context);
        }

        protected override IComponent[] CreateComponentsCore(IDesignerHost host, IDictionary defaultValues)
        {
            IComponent[] comps = base.CreateComponentsCore(host, defaultValues);

            //if (comps.FirstOrDefault() is BrowseTabControl bt)
            //{
            //    if (bt.TabPages.Count > 1)
            //    {
            //        for (int index = 1; index < bt.TabPages.Count; index++)
            //        {
            //            bt.TabPages.RemoveAt(index);
            //        }
            //    }
            //}

            return comps;
        }

    }
}
