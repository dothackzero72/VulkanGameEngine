using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

            if (ThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                switch (severity)
                {
                    case DebugUtilsMessageSeverityFlagsEXT.VerboseBitExt:
                        richTextBox.AppendText($"VERBOSE: {formattedMessage}{Environment.NewLine}");
                        break;
                    case DebugUtilsMessageSeverityFlagsEXT.InfoBitExt:
                        richTextBox.AppendText($"INFO: {formattedMessage}{Environment.NewLine}");
                        break;
                    case DebugUtilsMessageSeverityFlagsEXT.WarningBitExt:
                        richTextBox.AppendText($"WARNING: {formattedMessage}{Environment.NewLine}");
                        break;
                    case DebugUtilsMessageSeverityFlagsEXT.ErrorBitExt:
                        richTextBox.AppendText($"ERROR: {formattedMessage}{Environment.NewLine}");
                        break;
                    default:
                        richTextBox.AppendText($"UNKNOWN SEVERITY: {formattedMessage}{Environment.NewLine}");
                        break;
                }

                richTextBox.ScrollToCaret();
            }
        }
    }

}
