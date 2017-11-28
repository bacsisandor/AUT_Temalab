using Microsoft.Win32;
using System.Windows;
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

        public void SetTextBox(string text)
        {
            textBox.Text = text;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Save Code",
                Filter = "C++ files | *.cpp",
                DefaultExt = "txt",
                FileName = "Code"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(System.IO.Path.GetFullPath(saveFileDialog.FileName), textBox.Text);
            }
        }
    }
}
