using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VulkanGameEngineLevelEditor
{
    public static class GameEngineAPI
    {
        public static void CreateVulkanSurface(IntPtr handle)
        {
            var surfaceCreateInfo = new VkWin32SurfaceCreateInfoKHR
            {
                sType = 0,
                hwnd = handle,
                hinstance = System.Diagnostics.Process.GetCurrentProcess().MainModule.BaseAddress
            };

            VkSurfaceKHR surface = UIntPtr.Zero;
            if (VulkanAPI.vkCreateWin32SurfaceKHR(VulkanInstance.Instance, ref surfaceCreateInfo, IntPtr.Zero, out surface) != 0)
            {
                MessageBox.Show("Failed to create Vulkan surface.");
                return;
            }
            VulkanInstance.Surface = surface;
        }
    }
}
