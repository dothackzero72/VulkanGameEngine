using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public unsafe class SilkVulkanDebug
    {
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

        public static DebugUtilsMessengerCreateInfoEXT MakeDebugUtilsMessengerCreateInfoEXT()
        {
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
