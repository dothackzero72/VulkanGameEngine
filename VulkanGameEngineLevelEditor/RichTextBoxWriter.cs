using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace VulkanGameEngineLevelEditor
{
    public class RichTextBoxWriter : TextWriter
    {
        private readonly RichTextBox _richTextBox;
        public override Encoding Encoding => Encoding.UTF8;

        public RichTextBoxWriter(RichTextBox richTextBox)
        {
            _richTextBox = richTextBox;
        }

        public override void Write(char value)
        {
            if (_richTextBox.InvokeRequired)
            {
                _richTextBox.Invoke(new Action(() => Write(value)));
            }
            else
            {
                _richTextBox.AppendText(value.ToString());
                _richTextBox.ScrollToCaret();
            }
        }

        public override void Write(string value)
        {
            if (_richTextBox.InvokeRequired)
            {
                _richTextBox.Invoke(new Action(() => Write(value)));
            }
            else
            {
                _richTextBox.AppendText(value);
                _richTextBox.ScrollToCaret();
            }
        }

        public override void WriteLine(string value)
        {
            Write(value + Environment.NewLine);
        }
    }
}
