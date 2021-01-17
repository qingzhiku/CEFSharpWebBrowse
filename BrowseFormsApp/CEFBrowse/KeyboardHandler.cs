using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RockerBrowseForms.CEFBrowse
{
    internal class KeyboardHandler : IKeyboardHandler
    {
        public bool OnKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            if (type == KeyType.KeyUp && Enum.IsDefined(typeof(Keys), windowsKeyCode))
            {
                var key = (Keys)windowsKeyCode;
                switch (key)
                {
                    case Keys.F12:
                        browser.ShowDevTools();
                        break;

                    case Keys.F5:

                        if (modifiers == CefEventFlags.ControlDown)
                        {
                            //MessageBox.Show("ctrl+f5");
                            browser.Reload(true); //强制忽略缓存

                        }
                        else
                        {
                            //MessageBox.Show("f5");
                            browser.Reload();
                        }
                        break;
                }
            }
           
            #region 手动设置
            //const int VK_SHIFT = 0xA;
            //const int VK_CONTROL = 0x11;
            //const int VK_MENU = 0x12; // alt

            //const int VK_LEFT = 0x25;
            //const int VK_RIGHT = 0x27;

            //const int VK_F1 = 0x70;
            //const int VK_F2 = 0x71;
            //const int VK_F3 = 0x72;
            //const int VK_F4 = 0x73;
            //const int VK_F5 = 0x74;
            //const int VK_F6 = 0x75;
            //const int VK_F7 = 0x76;
            //const int VK_F8 = 0x77;
            //const int VK_F9 = 0x78;
            //const int VK_F10 = 0x79;
            //const int VK_F11 = 0x7A;
            //const int VK_F12 = 0x7B;

            //switch (modifiers)
            //{
            //    case CefEventFlags.None:
            //        switch (windowsKeyCode)
            //        {
            //            case VK_F5:
            //                browser.Reload(); //刷新
            //                break;
            //            case VK_F12:
            //                browser.ShowDevTools(); //开发工具
            //                break;
            //        }
            //        break;
            //    case CefEventFlags.CapsLockOn:
            //        break;
            //    case CefEventFlags.ShiftDown:
            //        break;
            //    case CefEventFlags.ControlDown:
            //        break;
            //    case CefEventFlags.AltDown:
            //        switch (windowsKeyCode)
            //        {
            //            case (VK_LEFT):
            //                if (browser.CanGoBack)
            //                    browser.GoBack();
            //                break;
            //            case (VK_RIGHT):
            //                if (browser.CanGoForward)
            //                    browser.GoForward();
            //                break;
            //        }
            //        break;
            //    case CefEventFlags.LeftMouseButton:
            //        break;
            //    case CefEventFlags.MiddleMouseButton:
            //        break;
            //    case CefEventFlags.RightMouseButton:
            //        break;
            //    case CefEventFlags.CommandDown:
            //        break;
            //    case CefEventFlags.NumLockOn:
            //        break;
            //    case CefEventFlags.IsKeyPad:
            //        break;
            //    case CefEventFlags.IsLeft:
            //        break;
            //    case CefEventFlags.IsRight:
            //        break;
            //    case CefEventFlags.AltGrDown:
            //        break;
            //    default:
            //        break;
            //} 
            #endregion

            return false;
        }

        public bool OnPreKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            

            
            return false;
        }
    }
}
