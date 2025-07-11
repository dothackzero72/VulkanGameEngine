using Silk.NET.Vulkan;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace VulkanGameEngineLevelEditor.Models
{
    public class MessengerModel
    {
        public string TextBoxName { get; set; }
        public RichTextBox richTextBox { get; set; }
        public int ThreadId { get; set; }
        public bool IsActive { get; set; }

        public void LogMessage(string formattedMessage, DebugUtilsMessageSeverityFlagsEXT severity)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke(new Action(() => LogMessage(formattedMessage, severity)));
                return;
            }

            Color color;
            string prefix;
            switch (severity)
            {
                case DebugUtilsMessageSeverityFlagsEXT.VerboseBitExt:
                    color = Color.Blue;
                    prefix = "VERBOSE: ";
                    break;
                case DebugUtilsMessageSeverityFlagsEXT.InfoBitExt:
                    color = Color.Green;
                    prefix = "INFO: ";
                    break;
                case DebugUtilsMessageSeverityFlagsEXT.WarningBitExt:
                    color = Color.Orange;
                    prefix = "WARNING: ";
                    break;
                case DebugUtilsMessageSeverityFlagsEXT.ErrorBitExt:
                    color = Color.Red;
                    prefix = "ERROR: ";
                    break;
                default:
                    color = Color.Green;
                    prefix = "UNKNOWN: ";
                    break;
            }

            string modifiedMessage = ModifyMessage(formattedMessage, severity);
            richTextBox.SelectionStart = richTextBox.TextLength;
            richTextBox.SelectionLength = 0;
            richTextBox.SelectionColor = color;
            richTextBox.AppendText(prefix);
            richTextBox.SelectionColor = Color.White;
            richTextBox.AppendText(modifiedMessage + Environment.NewLine);
            richTextBox.SelectionColor = Color.Black;
            richTextBox.ScrollToCaret();
        }

        private string ModifyMessage(string message, DebugUtilsMessageSeverityFlagsEXT severity)
        {
            return message.Trim();
        }
    }
}
