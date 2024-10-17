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
            Thread currentThread = Thread.CurrentThread;
            string currentThreadInfo = $"Thread ID: {currentThread.ManagedThreadId}, Name: {currentThread.Name}";

            string message = GetMessageFromPointer(callbackData->PMessage);
            string formattedMessage = $"Vulkan Message [Severity: {severity}, Type: {messageType}] [Thread: {currentThreadInfo}]: {message}";

            foreach (var messager in GlobalMessenger.messenger)
            {
                if (messager != null &&
                    messager.IsActive)

                {
                    if (messager.ThreadId == currentThread.ManagedThreadId)
                    {
                        if (messager.richTextBox.InvokeRequired)
                        {
                            messager.richTextBox.Invoke(new Action(() => messager.LogMessage(formattedMessage, severity)));
                        }
                    }
                }
            }


            Console.WriteLine(currentThreadInfo);
            switch (severity)
            {
                case DebugUtilsMessageSeverityFlagsEXT.VerboseBitExt:
                    Console.WriteLine($"VERBOSE: {formattedMessage}");
                    break;
                case DebugUtilsMessageSeverityFlagsEXT.InfoBitExt:
                    Console.WriteLine($"INFO: {formattedMessage}");
                    break;
                case DebugUtilsMessageSeverityFlagsEXT.WarningBitExt:
                    Console.WriteLine($"WARNING: {formattedMessage}");
                    break;
                case DebugUtilsMessageSeverityFlagsEXT.ErrorBitExt:
                    Console.WriteLine($"ERROR: {formattedMessage}");
                    break;
                default:
                    Console.WriteLine($"UNKNOWN SEVERITY: {formattedMessage}");
                    break;
            }
            return Vk.False;
        }
    }
}