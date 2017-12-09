using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace Blockly
{
    class BrowserHandler
    {
        private static BrowserHandler browserHandler;

        private BrowserHandler() { }

        public static BrowserHandler Instance
        {
            get
            {
                if (browserHandler == null)
                {
                    browserHandler = new BrowserHandler();
                }
                return browserHandler;
            }
        }

        private void SetBrowserFeatureControlKey(string feature, string appName, uint value)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(
                String.Concat(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\", feature),
                RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                key.SetValue(appName, (UInt32)value, RegistryValueKind.DWord);
            }
        }

        private void SetBrowserEmulationMode()
        {
            var fileName = System.IO.Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

            if (String.Compare(fileName, "devenv.exe", true) == 0 || String.Compare(fileName, "XDesProc.exe", true) == 0)
                return;
            UInt32 mode = 11001;
            SetBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION", fileName, mode);
        }

        public void SetBrowserSettings()
        {
            SetBrowserEmulationMode();
        }


    }
}
