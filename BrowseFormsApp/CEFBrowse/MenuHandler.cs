using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RockerBrowseForms.CEFBrowse
{
    /// <summary>
    /// 禁用右键菜单
    /// </summary>
    internal class MenuHandler : IContextMenuHandler
    {

        public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            model.Clear();
            //主要修改代码在此处;如果需要完完全全重新添加菜单项,首先执行model.Clear()清空菜单列表即可.
            //需要自定义菜单项的,可以在这里添加按钮;
            //if (model.Count > 0)
            //{
            //    model.AddSeparator();//添加分隔符;
            //}
            model.AddItem(CefMenuCommand.Back, "返回");
            model.AddItem(CefMenuCommand.Forward, "前进");
            model.AddItem(CefMenuCommand.Reload, "刷新");
            model.AddItem(CefMenuCommand.SelectAll, "全选");
            model.AddSeparator();
            model.AddItem(CefMenuCommand.Print, "打印");

            model.AddSeparator();

            model.AddItem(CefMenuCommand.ViewSource, "查看源码");
            model.AddItem((CefMenuCommand)26501, "检查");

            


        }

        public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame,IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            //命令的执行,点击菜单做什么事写在这里.
            if (commandId == (CefMenuCommand)26501)
            {
                browser.GetHost().ShowDevTools(inspectElementAtX: parameters.XCoord,inspectElementAtY:parameters.YCoord);
                return true;
            }
            if (commandId == (CefMenuCommand)26502)
            {
                browser.GetHost().CloseDevTools();
                return true;
            }
            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
            var webBrowser = (ChromiumWebBrowser)browserControl;
            Action setContextAction = delegate ()
            {
                webBrowser.ContextMenu = null;
            };
            webBrowser.Invoke(setContextAction);
        }

        public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser,IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            var webBrowser = (ChromiumWebBrowser)browserControl;

            model.SetEnabled(CefMenuCommand.Back, webBrowser.CanGoBack);
            model.SetEnabled(CefMenuCommand.Forward, webBrowser.CanGoForward);
            model.SetEnabled(CefMenuCommand.SelectAll, webBrowser.CanSelect);

            //var menuItems = GetMenuItems(model).ToList();

            //menuItems.ForEach(m =>
            //{
            //    if(m.Item3 == CefMenuCommand.Back)
            //    {
            //        if (!webBrowser.CanGoBack)
            //        {

            //        }
            //    }
            //});

            #region MyRegion

            //webBrowser.Invoke(new Action(() =>
            //{
            //    foreach (var item in menuItems)
            //    {
            //        switch (item.Item2)
            //        {
            //            case CefMenuCommand.Back:
            //                {
            //                    //browser.GoBack();
            //                    break;
            //                }
            //            case CefMenuCommand.Forward:
            //                {
            //                    //browser.GoForward();
            //                    break;
            //                }
            //            case CefMenuCommand.Cut:
            //                {
            //                    //browser.FocusedFrame.Cut();
            //                    break;
            //                }
            //            case CefMenuCommand.Copy:
            //                {
            //                    //browser.FocusedFrame.Copy();
            //                    break;
            //                }
            //            case CefMenuCommand.Paste:
            //                {
            //                    //browser.FocusedFrame.Paste();
            //                    break;
            //                }
            //            case CefMenuCommand.Print:
            //                {
            //                    //browser.GetHost().Print();
            //                    break;
            //                }
            //            case CefMenuCommand.ViewSource:
            //                {
            //                    //browser.FocusedFrame.ViewSource();
            //                    break;
            //                }
            //            case CefMenuCommand.Undo:
            //                {
            //                    //browser.FocusedFrame.Undo();
            //                    break;
            //                }
            //            case CefMenuCommand.StopLoad:
            //                {
            //                    //browser.StopLoad();
            //                    break;
            //                }
            //            case CefMenuCommand.SelectAll:
            //                {
            //                    //browser.FocusedFrame.SelectAll();
            //                    break;
            //                }
            //            case CefMenuCommand.Redo:
            //                {
            //                    //browser.FocusedFrame.Redo();
            //                    break;
            //                }
            //            case CefMenuCommand.Find:
            //                {
            //                    //browser.GetHost().Find(0, parameters.SelectionText, true, false, false);
            //                    break;
            //                }
            //            case CefMenuCommand.AddToDictionary:
            //                {
            //                    //browser.GetHost().AddWordToDictionary(parameters.MisspelledWord);
            //                    break;
            //                }
            //            case CefMenuCommand.Reload:
            //                {
            //                    //browser.Reload();
            //                    break;
            //                }
            //            case CefMenuCommand.ReloadNoCache:
            //                {
            //                    //browser.Reload(ignoreCache: true);
            //                    break;
            //                }
            //            case (CefMenuCommand)26501:
            //                {
            //                    //browser.GetHost().ShowDevTools();
            //                    break;
            //                }
            //            case (CefMenuCommand)26502:
            //                {
            //                    //browser.GetHost().CloseDevTools();
            //                    break;
            //                }
            //        }
            //    }
            //}));

            #endregion

            return false; // 弹出
        }

        //下面这个官网Example的Fun,读取已有菜单项列表时候,实现的IEnumerable,如果不需要,
        // 完全可以注释掉;不属于IContextMenuHandler接口规定的
        private static IEnumerable<Tuple<int,string, CefMenuCommand, bool>> GetMenuItems(IMenuModel model)
        {
            for (var i = 0; i < model.Count; i++)
            {
                var header = model.GetLabelAt(i);
                var commandId = model.GetCommandIdAt(i);
                var isEnabled = model.IsEnabledAt(i);
                yield return new Tuple<int,string, CefMenuCommand, bool>(i,header, commandId, isEnabled);
            }
        }

    }
}
