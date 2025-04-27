using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace VulkanGameEngineLevelEditor
{
    public class TextBoxWriter : TextWriter
    {
        private readonly TextBox _textBox;
        public override Encoding Encoding => Encoding.UTF8;

        public TextBoxWriter(TextBox textBox)
        {
            _textBox = textBox;
        }

        public override void Write(char value)
        {
            if (_textBox.InvokeRequired)
            {
                _textBox.Invoke(new Action(() => Write(value)));
            }
            else
            {
                _textBox.AppendText(value.ToString());
                _textBox.ScrollToCaret();
            }
        }

        public override void Write(string value)
        {
            if (_textBox.InvokeRequired)
            {
                _textBox.Invoke(new Action(() => Write(value)));
            }
            else
            {
                _textBox.AppendText(value);
                _textBox.ScrollToCaret();
            }
        }

        public override void WriteLine(string value)
        {
            Write(value + Environment.NewLine);
        }
    }
}
