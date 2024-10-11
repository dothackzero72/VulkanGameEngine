
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Windowing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Silk.NET.SDL;
using Silk.NET.Core.Native;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public unsafe class SilkVulkanSwapChain
    {
        Vk vk = Vk.GetApi();
        public Format colorFormat { get; private set; }
        public Extent2D swapchainExtent { get; set; }
        public SwapchainKHR swapChain { get; private set; }
        public Image[] images { get; private set; }
        public ImageView[] imageViews { get; private set; }
        public KhrSwapchain khrSwapchain { get; private set; }
        public uint ImageCount { get; private set; } = SilkVulkanRenderer.MAX_FRAMES_IN_FLIGHT;

        [DllImport("vulkan-1.dll")]
        public static extern Result vkGetPhysicalDeviceSurfaceCapabilitiesKHR(PhysicalDevice physicalDevice, SurfaceKHR surface, out SurfaceCapabilitiesKHR pSurfaceCapabilities);
    public SwapchainKHR CreateSwapChain(IntPtr window)
        {
            List<SurfaceFormatKHR> surfaceFormat = GetSurfaceFormatsKHR(SilkVulkanRenderer.physicalDevice).ToList();
            List<PresentModeKHR> presentMode = GetSurfacePresentModesKHR(SilkVulkanRenderer.physicalDevice).ToList();
            SurfaceCapabilitiesKHR surfaceCapabilities = GetSurfaceCapabilitiesKHR(SilkVulkanRenderer.physicalDevice);

            SurfaceTransformFlagsKHR preTransform = surfaceCapabilities.SupportedTransforms.HasFlag(SurfaceTransformFlagsKHR.IdentityBitKhr)
                                         ? SurfaceTransformFlagsKHR.IdentityBitKhr
                                         : surfaceCapabilities.CurrentTransform;

            CompositeAlphaFlagsKHR compositeAlpha =
                surfaceCapabilities.SupportedCompositeAlpha.HasFlag(CompositeAlphaFlagsKHR.PreMultipliedBitKhr) ? CompositeAlphaFlagsKHR.PreMultipliedBitKhr :
                surfaceCapabilities.SupportedCompositeAlpha.HasFlag(CompositeAlphaFlagsKHR.PostMultipliedBitKhr) ? CompositeAlphaFlagsKHR.PostMultipliedBitKhr :
                surfaceCapabilities.SupportedCompositeAlpha.HasFlag(CompositeAlphaFlagsKHR.InheritBitKhr) ? CompositeAlphaFlagsKHR.InheritBitKhr :
                CompositeAlphaFlagsKHR.OpaqueBitKhr;

            colorFormat = surfaceFormat[0].Format;
            swapchainExtent = new Extent2D();
            if (surfaceCapabilities.CurrentExtent.Width != uint.MaxValue)
            {
                swapchainExtent = surfaceCapabilities.CurrentExtent;
            }
            else
            {
                Extent2D actualExtent = new Extent2D()
                {
                    Width = (uint)swapchainExtent.Width,
                    Height = (uint)swapchainExtent.Height
                };

                Extent2D actualExtent2D = new Extent2D();
                actualExtent2D.Width = actualExtent.Width;
                actualExtent2D.Height = actualExtent.Height;
                swapchainExtent = actualExtent2D;
            }

            SwapchainCreateInfoKHR createInfo = new SwapchainCreateInfoKHR
                (
                    surface: SilkVulkanRenderer.surface,
                    minImageCount: surfaceCapabilities.MinImageCount,
                    imageFormat: colorFormat,
                    imageColorSpace: surfaceFormat[0].ColorSpace,
                    imageExtent: swapchainExtent,
                    imageArrayLayers: 1,
                    imageUsage: ImageUsageFlags.ColorAttachmentBit | ImageUsageFlags.TransferSrcBit,
                    imageSharingMode: SharingMode.Exclusive,
                    preTransform: preTransform,
                    compositeAlpha: compositeAlpha,
                    presentMode: presentMode[0],
                    oldSwapchain: new SwapchainKHR(null),
                    clipped: true
                );

            if (SilkVulkanRenderer.GraphicsFamily != SilkVulkanRenderer.PresentFamily)
            {
                uint* queueFamilyIndices = stackalloc uint[] { SilkVulkanRenderer.GraphicsFamily, SilkVulkanRenderer.PresentFamily };
                createInfo.ImageSharingMode = SharingMode.Concurrent;
                createInfo.QueueFamilyIndexCount = 2;
                createInfo.PQueueFamilyIndices = queueFamilyIndices;
            }

            if (!SilkVulkanRenderer.vk.TryGetDeviceExtension(SilkVulkanRenderer.instance, SilkVulkanRenderer.device, out KhrSwapchain khrSwapChain))
            {
                throw new NotSupportedException("KHR_swapchain extension not found.");
            }
            khrSwapchain = khrSwapChain;

            var Swapchain = new SwapchainKHR();
            var result = khrSwapchain.CreateSwapchain(SilkVulkanRenderer.device, &createInfo, null, out Swapchain);
            if (result != Result.Success)
            {
                throw new Exception("failed to create swap chain!");
            }
            swapChain = Swapchain;

            images = GetImagesKHR(swapChain, khrSwapChain);
            imageViews = new ImageView[images.Length];
            for (int x = 0; x < images.Length; x++)
            {
                ImageViewCreateInfo viewInfo = new(
                    image: images[x],
                    viewType: ImageViewType.Type2D,
                    format: colorFormat,
                    subresourceRange: new(ImageAspectFlags.ColorBit, 0, 1, 0, 1)
                );

                ImageView view = new ImageView();
                SilkVulkanRenderer.vk.CreateImageView(SilkVulkanRenderer.device, &viewInfo, null, out view);
                imageViews[x] = view;
            }

            return swapChain;
        }

        public Image[] GetImagesKHR(SwapchainKHR swapchain2, KhrSwapchain swapC)
        {
            uint imageCount = 0;
            swapC.GetSwapchainImages(SilkVulkanRenderer.device, swapchain2, &imageCount, null);
            Image[] swapChainImages = new Image[imageCount];
            swapC.GetSwapchainImages(SilkVulkanRenderer.device, swapchain2, &imageCount, swapChainImages);
            return swapChainImages;
        }

        public SurfaceCapabilitiesKHR GetSurfaceCapabilitiesKHR(PhysicalDevice physicalDevice)
        {
            Result result = vkGetPhysicalDeviceSurfaceCapabilitiesKHR(physicalDevice, SilkVulkanRenderer.surface, out SurfaceCapabilitiesKHR surfaceCapabilities);
            return surfaceCapabilities;
        }

        [DllImport("vulkan-1.dll")]
        public static extern int vkGetPhysicalDeviceSurfaceFormatsKHR(PhysicalDevice physicalDevice, SurfaceKHR surface, ref uint pSurfaceFormatCount, IntPtr pSurfaceFormats);
        public static SurfaceFormatKHR[] GetSurfaceFormatsKHR(PhysicalDevice physicalDevice)
        {
            uint formatCount = 0;
            vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, SilkVulkanRenderer.surface, ref formatCount, IntPtr.Zero);

            var formats = new SurfaceFormatKHR[formatCount];
            int structureSize = Marshal.SizeOf<SurfaceFormatKHR>();
            IntPtr formatPtr = Marshal.AllocHGlobal(structureSize * (int)formatCount);
            try
            {
                vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, SilkVulkanRenderer.surface, ref formatCount, formatPtr);
                for (int x = 0; x < formatCount; x++)
                {
                    formats[x] = Marshal.PtrToStructure<SurfaceFormatKHR>(formatPtr + x * Marshal.SizeOf<SurfaceFormatKHR>());
                    Console.WriteLine($"Format: {formats[x].Format}, Color Space: {formats[x].ColorSpace}");
                }
            }
            finally
            {
                Marshal.FreeHGlobal(formatPtr);
            }
            return formats;
        }


        [DllImport("vulkan-1.dll")]
        public static extern int vkGetPhysicalDeviceSurfacePresentModesKHR(PhysicalDevice physicalDevice, SurfaceKHR surface, ref uint pPresentModeCount, IntPtr pPresentModes);
        public static PresentModeKHR[] GetSurfacePresentModesKHR(PhysicalDevice physicalDevice)
        {
            uint presentModeCount = 0;
            vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, SilkVulkanRenderer.surface, ref presentModeCount, IntPtr.Zero);

            var presentModes = new PresentModeKHR[presentModeCount];
            IntPtr presentModePtr = Marshal.AllocHGlobal((int)presentModeCount * sizeof(int));
            try
            {
                vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, SilkVulkanRenderer.surface, ref presentModeCount, presentModePtr);
                for (int x = 0; x < presentModeCount; x++)
                {
                    presentModes[x] = (PresentModeKHR)Marshal.ReadInt32(presentModePtr + x * sizeof(int));
                    Console.WriteLine($"Present Mode: {presentModes[x]}");
                }
            }
            finally
            {
                Marshal.FreeHGlobal(presentModePtr);
            }
            return presentModes;
        }
    }
}
