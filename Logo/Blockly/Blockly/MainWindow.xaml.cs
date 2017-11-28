using Antlr4.Runtime;
using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Xml.Linq;

namespace Blockly
{
    public partial class MainWindow : Window
    {
        private struct ValidationResult
        {
            public bool Success { get; private set; }
            public TinyScriptParser.ProgramContext ProgramContext { get; private set; }
            public TinyScriptVisitor.ITypeData TypeData { get; private set; }

            public ValidationResult(bool success, TinyScriptParser.ProgramContext programContext, TinyScriptVisitor.ITypeData typeData)
            {
                Success = success;
                ProgramContext = programContext;
                TypeData = typeData;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            BrowserHandler.Instance.SetBrowserSettings();
            browser.Navigate(GetPath());
            browser.ObjectForScripting = new ScriptInterface(this);
        }

        public string GetPath()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\..\..\Blockly_Offline\blockly\index.html"));
            return newPath;
        }

        private void DisplayBlocks(XElement root)
        {
            string xmlString = root.ToString().Replace('\r', ' ').Replace('\n', ' ');
            string script = $"var xml = Blockly.Xml.textToDom('{ xmlString }'); Blockly.Xml.domToWorkspace(xml, workspace);";
            browser.InvokeScript("execScript", new Object[] { script, "JavaScript" });
        }

        private void ToXmlButton_Click(object sender, RoutedEventArgs e)
        {
            var script = "var xml = Blockly.Xml.workspaceToDom(workspace);var xml_text = Blockly.Xml.domToPrettyText(xml); alert(xml_text);";
            browser.InvokeScript("execScript", new Object[] { script, "JavaScript" });
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
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
            TinyScriptVisitor.ITypeData data;
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

        private void CompileButton_Click(object sender, RoutedEventArgs e)
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            browser.InvokeScript("saveBlocks");
            var xml = browser.InvokeScript("eval", new object[] { "generatedXml" });
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Save Blocks",
                Filter = "Blocks | *.xml",
                DefaultExt = "xml",
                FileName = "Blocks"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(Path.GetFullPath(saveFileDialog.FileName), xml.ToString());
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfiledialog = new OpenFileDialog
            {
                Filter = "Blocks | *.xml"
            };
            if (openfiledialog.ShowDialog() == true)
            {
                string readText = File.ReadAllText(Path.GetFullPath(openfiledialog.FileName));
                browser.InvokeScript("loadBlocks", readText);
            }
        }

        private void CodeSaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveCode(textBox.Text);
        }

        private void SaveCode(string code)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Save Code",
                Filter = "Text Files | *.txt",
                DefaultExt = "txt",
                FileName = "Code"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(Path.GetFullPath(saveFileDialog.FileName), code);
            }
        }

        private void CodeLoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfiledialog = new OpenFileDialog
            {
                Filter = "Text Files | *.txt"
            };
            if (openfiledialog.ShowDialog() == true)
            {
                textBox.Text = File.ReadAllText(Path.GetFullPath(openfiledialog.FileName));
            }
        }

        private void GenCButton_Click(object sender, RoutedEventArgs e)
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
            cw.SetTextBox(cCode);
        }
    }
}
