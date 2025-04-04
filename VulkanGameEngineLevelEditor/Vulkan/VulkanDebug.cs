using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public unsafe class VulkanDebug
    {
        public MessageBox Message { get; private set; }
        public VulkanDebug()
        {
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

        public static uint MessageCallback(DebugUtilsMessageSeverityFlagsEXT severity, DebugUtilsMessageTypeFlagsEXT messageType, DebugUtilsMessengerCallbackDataEXT* callbackData, void* userData)
        {
            string message = GetMessageFromPointer(callbackData->PMessage);
            string formattedMessage = $"Vulkan Message [Severity: {severity}, Type: {messageType}]: {message}";
            switch (severity)
            {
                case DebugUtilsMessageSeverityFlagsEXT.VerboseBitExt:
                    Console.WriteLine($"VERBOSE: {formattedMessage}");
                    break;
                case DebugUtilsMessageSeverityFlagsEXT.InfoBitExt:
                    Console.WriteLine($"INFO: {formattedMessage}");
                    break;
                case DebugUtilsMessageSeverityFlagsEXT.WarningBitExt:
                    Console.Error.WriteLine($"WARNING: {formattedMessage}");
                    break;
                case DebugUtilsMessageSeverityFlagsEXT.ErrorBitExt:
                    Console.Error.WriteLine($"ERROR: {formattedMessage}");
                    break;
                default:
                    Console.Error.WriteLine($"UNKNOWN SEVERITY: {formattedMessage}");
                    break;
            }
            return Vk.False;
        }
    }
}