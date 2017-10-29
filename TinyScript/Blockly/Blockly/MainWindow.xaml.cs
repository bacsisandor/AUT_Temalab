using Antlr4.Runtime;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Xml.Linq;

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
            SetBrowserEmulationMode(); // Changing webbrowser to IE11
            string path = Assembly.GetExecutingAssembly().Location;
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\..\..\Blockly_Offline\blockly\demos\tinyscript\index.html"));
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

        private void DisplayBlocks(XElement root)
        {
            string xmlString = root.ToString().Replace('\r', ' ').Replace('\n', ' ');
            string script = $"var xml = Blockly.Xml.textToDom('{ xmlString }'); Blockly.Xml.domToWorkspace(xml, workspace);";
            browser.InvokeScript("execScript", new Object[] { script, "JavaScript" });
        }

        private void toXmlButton_Click(object sender, RoutedEventArgs e)
        {
 
            var script = "var xml = Blockly.Xml.workspaceToDom(workspace);var xml_text = Blockly.Xml.domToPrettyText(xml); alert(xml_text);";

            browser.InvokeScript("execScript", new Object[] { script, "JavaScript" });
        }

        private void injectButton_Click(object sender, RoutedEventArgs e)
        {

            /*XDocument doc = new XDocument();
            doc.Add(new XElement("block", new XAttribute("type", "pen_up")));


            //string xmlString = doc.ToString();

            string xmlString = "<block type=\"pen_up\"> <next> <block type=\"pen_up\" /> </next> </block>";

            var script = "var xml = Blockly.Xml.textToDom('<xml>";
            script += xmlString;
            script += "</xml>'); Blockly.Xml.domToWorkspace(xml, workspace);";

            browser.InvokeScript("clearWorkspace");

           // var script = "var xml = Blockly.Xml.textToDom('<xml><block type=\"repeat\"  x=\"10\" y=\"10\"><field name=\"repeat_number\">4</field><statement name=\"repeat\"><block type=\"move_forward\"><value name=\"forward_pixels\"><block type=\"math_number\"><field name=\"NUM\">90</field></block></value><next><block type=\"turn_right\"><value name=\"turn_right\"><block type= \"math_number\"><field name=\"NUM\">90</field></block></value></block></next></block></statement></block></xml>'); Blockly.Xml.domToWorkspace(xml, workspace);";

            browser.InvokeScript("execScript", new Object[] { script, "JavaScript" });
            */
        }

        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
            browser.InvokeScript("showCode");
            var generatedCode = browser.InvokeScript("eval", new object[] { "generatedCode" });
            textBox.Text = generatedCode.ToString();
        }

        private void compileButton_Click(object sender, RoutedEventArgs e)
        {
            var code = textBox.Text;
            ErrorListener error = new ErrorListener();
            var inputStream = new AntlrInputStream(code);
            var lexer = new TinyScriptLexer(inputStream);
            lexer.AddErrorListener(error);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new TinyScriptParser(tokenStream);
            parser.AddErrorListener(error);
            var visitor = new TinyScriptVisitor();
            browser.InvokeScript("clearWorkspace");
            try
            {
                DisplayBlocks(visitor.Visit(parser.program()));
            }
            catch (SyntaxErrorException ex)
            {
                ex.Display();
            }
        }
    }
}
