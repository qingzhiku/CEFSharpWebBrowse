using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CefSharp;
using CefSharp.WinForms;

namespace RockerBrowseForms.CEFBrowse
{
    public class CEFHelper
    {
        //初始化浏览器并启动
        public static void InitializeChromium()
        {
            string currentPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string cachePath = currentPath + @"\Cache";

            CefSettings settings = new CefSettings();
            settings.BrowserSubprocessPath = Path.Combine(currentPath, Environment.Is64BitProcess ? "x64" : "x86", "CefSharp.BrowserSubprocess.exe");
            settings.IgnoreCertificateErrors = true; //忽略证书错误问题
            //settings.CefCommandLineArgs.Add("--ignore-urlfetcher-cert-requests", "1");
            //settings.CefCommandLineArgs.Add("--ignore-certificate-errors", "1");
            settings.UserAgent = string.Format("Mozilla/5.0 (Windows NT {0}.{1}; Win{2}; x{3}) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/{4} Safari/537.36 Edg/87.0.664.66",
                Environment.OSVersion.Version.Major,
                Environment.OSVersion.Version.Minor,
                Environment.Is64BitOperatingSystem ? "64" : "86",
                Environment.Is64BitProcess ? "64" : "86",
                Cef.CefSharpVersion);
            settings.Locale = "zh-CN,zh;q=0.8";
            settings.AcceptLanguageList = Application.CurrentCulture.Name; // "zh-CN";
            settings.PersistSessionCookies = true;//保存回话
            settings.CachePath = cachePath; //缓存文件保存目录：默认情况下，CEF使用内存缓存来保存缓存的数据，例如 要保留cookie，您需要指定一个缓存路径
            settings.LocalesDirPath = currentPath + @"\localeDir";
            settings.UserDataPath = currentPath + @"\userData";
            settings.LogFile = currentPath + @"\LogData";

            //flash
            settings.CefCommandLineArgs.Add("ppapi-flash-path", currentPath + "\\Plugins\\pepflash\\pepflashplayer.dll");
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;//将c#对象注册为 js对象
            Cef.EnableHighDPISupport();

            //用于多线程访问控件
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        public static void BindCommonHandler(ChromiumWebBrowser webBrowser) {
            webBrowser.LifeSpanHandler = new CefLifeSpanHandler();
            webBrowser.KeyboardHandler = new KeyboardHandler();
            webBrowser.MenuHandler = new MenuHandler();
        }


    }
}
