using Silk.NET.Vulkan;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public unsafe class SilkVulkanDebug
    {
        private static RichTextBox _logTextBox;

        // Constructor to set the RichTextBox for logging
        public SilkVulkanDebug(RichTextBox logTextBox)
        {
            _logTextBox = logTextBox;
        }

        private static string GetMessageFromPointer(byte* messagePtr)
        {
            int length = 0;
            while (messagePtr[length] != 0)
            {
                length++;
            }

            byte[] messageBytes = new byte[length];
            Marshal.Copy(new IntPtr(messagePtr), messageBytes, 0, length);
            return System.Text.Encoding.ASCII.GetString(messageBytes);
        }

        public static DebugUtilsMessengerCreateInfoEXT MakeDebugUtilsMessengerCreateInfoEXT(RichTextBox logTextBox)
        {
            _logTextBox = logTextBox;
            DebugUtilsMessengerCreateInfoEXT createInfo = new
            (
                messageSeverity:
                    DebugUtilsMessageSeverityFlagsEXT.VerboseBitExt |
                    DebugUtilsMessageSeverityFlagsEXT.InfoBitExt |
                    DebugUtilsMessageSeverityFlagsEXT.WarningBitExt |
                    DebugUtilsMessageSeverityFlagsEXT.ErrorBitExt,
                messageType:
                    DebugUtilsMessageTypeFlagsEXT.GeneralBitExt |
                    DebugUtilsMessageTypeFlagsEXT.ValidationBitExt |
                    DebugUtilsMessageTypeFlagsEXT.PerformanceBitExt,
                pfnUserCallback: new PfnDebugUtilsMessengerCallbackEXT(MessageCallback)
            );

            return createInfo;
        }

        //public uint MessageCallback(DebugUtilsMessageSeverityFlagsEXT severity, DebugUtilsMessageTypeFlagsEXT messageType, DebugUtilsMessengerCallbackDataEXT* callbackData, void* userData)
        //{
        //    string message = GetMessageFromPointer(callbackData->PMessage);
        //    string formattedMessage = $"Vulkan Message [Severity: {severity}, Type: {messageType}]: {message}";

        //    // Log messages to the RichTextBox
        //    if (_logTextBox.InvokeRequired)
        //    {
        //        _logTextBox.Invoke(new Action(() => LogMessage(formattedMessage, severity)));
        //    }
        //    else
        //    {
        //        LogMessage(formattedMessage, severity);
        //    }

        //    return Vk.False;
        //}
        public static uint MessageCallback(DebugUtilsMessageSeverityFlagsEXT severity, DebugUtilsMessageTypeFlagsEXT messageType, DebugUtilsMessengerCallbackDataEXT* callbackData, void* userData)
        {
            string message = GetMessageFromPointer(callbackData->PMessage);
            string formattedMessage = $"Vulkan Message [Severity: {severity}, Type: {messageType}]: {message}";

            if (_logTextBox != null)
            {
                if (_logTextBox.InvokeRequired)
                {
                    _logTextBox.Invoke(new Action(() => LogMessage(formattedMessage, severity)));
                }
            }

            return Vk.False;
        }

        private static void LogMessage(string formattedMessage, DebugUtilsMessageSeverityFlagsEXT severity)
        {
            switch (severity)
            {
                case DebugUtilsMessageSeverityFlagsEXT.VerboseBitExt:
                    _logTextBox.AppendText($"VERBOSE: {formattedMessage}{Environment.NewLine}");
                    break;
                case DebugUtilsMessageSeverityFlagsEXT.InfoBitExt:
                    _logTextBox.AppendText($"INFO: {formattedMessage}{Environment.NewLine}");
                    break;
                case DebugUtilsMessageSeverityFlagsEXT.WarningBitExt:
                    _logTextBox.AppendText($"WARNING: {formattedMessage}{Environment.NewLine}");
                    break;
                case DebugUtilsMessageSeverityFlagsEXT.ErrorBitExt:
                    _logTextBox.AppendText($"ERROR: {formattedMessage}{Environment.NewLine}");
                    break;
                default:
                    _logTextBox.AppendText($"UNKNOWN SEVERITY: {formattedMessage}{Environment.NewLine}");
                    break;
            }

            _logTextBox.ScrollToCaret(); // Scroll to the bottom if new text is added
        }
    }
}