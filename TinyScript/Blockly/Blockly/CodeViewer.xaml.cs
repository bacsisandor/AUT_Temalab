using Microsoft.Win32;
using System.Windows;
using System.Windows.Shapes;
using System.IO;

namespace Blockly
{
    /// <summary>
    /// Interaction logic for CodeViewer.xaml
    /// </summary>
    public partial class CodeViewer : Window
    {
        public CodeViewer()
        {
            InitializeComponent();
        }

        public void setTextBox(string text)
        {
            textBox.Text = text;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save Code";
            saveFileDialog.Filter = "C files | *.c";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.FileName = "Code";
            if (saveFileDialog.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(System.IO.Path.GetFullPath(saveFileDialog.FileName), textBox.Text);
            }
        }
    }
}
