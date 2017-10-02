﻿using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;


namespace Blockly
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetBrowserEmulationMode();
            string path = Assembly.GetExecutingAssembly().Location;
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\..\..\Blockly_Offline\blockly\demos\logo\index.html"));
            browser.Navigate(newPath);
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

        public void SetBrowserEmulationMode()
        {
            var fileName = System.IO.Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

            if (String.Compare(fileName, "devenv.exe", true) == 0 || String.Compare(fileName, "XDesProc.exe", true) == 0)
                return;
            UInt32 mode = 11001;
            SetBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION", fileName, mode);
        }

        private void toXmlButton_Click(object sender, RoutedEventArgs e)
        {
            var script = "var xml = Blockly.Xml.workspaceToDom(workspace);var xml_text = Blockly.Xml.domToPrettyText(xml); document.write(xml_text);";

            browser.InvokeScript("execScript", new Object[] { script, "JavaScript" });
        }

        private void injectButton_Click(object sender, RoutedEventArgs e)
        {
            var script = "var xml = Blockly.Xml.textToDom('<xml><block type=\"repeat\"  x=\"10\" y=\"10\"><field name=\"repeat_number\">4</field><statement name=\"repeat\"><block type=\"move_forward\"><value name=\"forward_pixels\"><block type=\"math_number\"><field name=\"NUM\">90</field></block></value><next><block type=\"turn_right\"><value name=\"turn_right\"><block type= \"math_number\"><field name=\"NUM\">90</field></block></value></block></next></block></statement></block></xml>'); Blockly.Xml.domToWorkspace(xml, workspace);";

            browser.InvokeScript("execScript", new Object[] { script, "JavaScript" });
        }
    }
}