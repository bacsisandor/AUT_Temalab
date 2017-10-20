using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Blockly
{
    public class SyntaxErrorException : Exception
    {
        public SyntaxErrorException(int line, int charPositionInLine, string msg) : base(msg + "\nat line " + line + ", char " + charPositionInLine) { }

        public void Display()
        {
            MessageBox.Show(Message, "Syntax error");
        }
    }
}
