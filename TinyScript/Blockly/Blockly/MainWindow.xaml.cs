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
        [System.Runtime.InteropServices.ComVisibleAttribute(true)]
        public class ScriptInterface
        {
            private MainWindow _w;
            public ScriptInterface(MainWindow w)
            {
                _w = w;
            }

            public void onChanged()
            {
                if (_w.autogenCheckBox.IsChecked == true)
                {
                    _w.browser.InvokeScript("showCode");
                    var generatedCode = _w.browser.InvokeScript("eval", new object[] { "generatedCode" });
                    _w.textBox.Text = generatedCode.ToString();
                }
                
            }
        }

        private struct ValidationResult
        {
            public bool Success { get; private set; }
            public TinyScriptParser.ProgramContext ProgramContext { get; private set; }
            public TinyScriptVisitor.TypeData TypeData { get; private set; }

            public ValidationResult(bool success, TinyScriptParser.ProgramContext programContext, TinyScriptVisitor.TypeData typeData)
            {
                Success = success;
                ProgramContext = programContext;
                TypeData = typeData;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            SetBrowserEmulationMode(); // Changing webbrowser to IE11
            string path = Assembly.GetExecutingAssembly().Location;
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\..\..\Blockly_Offline\blockly\demos\tinyscript\index.html"));
            browser.Navigate(newPath);
            browser.ObjectForScripting = new ScriptInterface(this);
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

        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
            browser.InvokeScript("showCode");
            var generatedCode = browser.InvokeScript("eval", new object[] { "generatedCode" });
            textBox.Text = generatedCode.ToString();
        }

        private ValidationResult Validate()
        {
            ErrorListener error = new ErrorListener();
            var inputStream = new AntlrInputStream(textBox.Text);
            var lexer = new TinyScriptLexer(inputStream);
            lexer.AddErrorListener(error);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new TinyScriptParser(tokenStream);
            parser.AddErrorListener(error);
            var visitor = new TinyScriptVisitor();
            TinyScriptParser.ProgramContext context;
            TinyScriptVisitor.TypeData data;
            try
            {
                context = parser.program();
                data = visitor.Analyze(context);
            }
            catch (SyntaxErrorException ex)
            {
                ex.Display();
                return new ValidationResult(false, null, null);
            }
            return new ValidationResult(true, context, data);
        }

        private void compileButton_Click(object sender, RoutedEventArgs e)
        {
            ValidationResult result = Validate();
            if (!result.Success)
            {
                return;
            }
            browser.InvokeScript("clearWorkspace");
            TinyScriptXMLVisitor visitor = new TinyScriptXMLVisitor(result.TypeData);
            DisplayBlocks(visitor.Visit(result.ProgramContext));
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            browser.InvokeScript("saveBlocks");
            var xml = browser.InvokeScript("eval", new object[] { "generatedXml" });
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save Blocks";
            saveFileDialog.Filter = "Blocks | *.xml";
            saveFileDialog.DefaultExt = "xml";
            saveFileDialog.FileName = "Blocks";
            if (saveFileDialog.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(Path.GetFullPath(saveFileDialog.FileName), xml.ToString());
            }
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfiledialog = new OpenFileDialog();
            openfiledialog.Filter = "Blocks | *.xml";
            if (openfiledialog.ShowDialog() == true)
            {
                string readText = File.ReadAllText(Path.GetFullPath(openfiledialog.FileName));
                browser.InvokeScript("loadBlocks", readText);
            }
        }

        private void codeSaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveCode(textBox.Text);
        }

        private void SaveCode(string code)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save Code";
            saveFileDialog.Filter = "Text Files | *.txt";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.FileName = "Code";
            if (saveFileDialog.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(Path.GetFullPath(saveFileDialog.FileName), code);
            }
        }

        private void codeLoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfiledialog = new OpenFileDialog();
            openfiledialog.Filter = "Text Files | *.txt";
            if (openfiledialog.ShowDialog() == true)
            {
                textBox.Text = File.ReadAllText(Path.GetFullPath(openfiledialog.FileName));
            }
        }

        private void genCButton_Click(object sender, RoutedEventArgs e)
        {
            ValidationResult result = Validate();
            if (!result.Success)
            {
                return;
            }
            TinyScriptCVisitor visitor = new TinyScriptCVisitor(result.TypeData);
            string cCode = visitor.Visit(result.ProgramContext);
            CodeViewer cw = new CodeViewer();
            cw.Show();
            cw.setTextBox(cCode);
        }
    }
}
