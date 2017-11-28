namespace Blockly
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class ScriptInterface
    {
        private MainWindow _w;
        public ScriptInterface(MainWindow w)
        {
            _w = w;
        }

        public void OnChanged()
        {
            if (_w.autogenCheckBox.IsChecked == true)
            {
                _w.browser.InvokeScript("showCode");
                var generatedCode = _w.browser.InvokeScript("eval", new object[] { "generatedCode" });
                _w.textBox.Text = generatedCode.ToString();
            }

        }
    }
}
