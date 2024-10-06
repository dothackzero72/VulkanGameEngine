
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Windowing;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public unsafe class SilkVulkanSwapChain
    {
        public Format colorFormat { get; private set; }
        public Extent2D swapchainExtent { get; set; }
        public SwapchainKHR swapChain { get; private set; }
        public Image[] images { get; private set; }
        public ImageView[] imageViews { get; private set; }
        public KhrSwapchain khrSwapchain { get; private set; }
        public uint ImageCount { get; private set; } = SilkVulkanRenderer.MAX_FRAMES_IN_FLIGHT;
        public SwapchainKHR CreateSwapChain(IWindow window, KhrSurface khrSurface, SurfaceKHR surfacekhrt)
        {

            SurfaceFormatKHR surfaceFormat = GetSurfaceFormat(GetSurfaceFormats(khrSurface, surfacekhrt));

            PresentModeKHR presentMode = GetPresentFormat(GetPresentFormats(khrSurface, surfacekhrt));
            SurfaceCapabilitiesKHR surfaceCapabilities = GetSurfaceCapabilitiesKHR(khrSurface, surfacekhrt);

            SurfaceTransformFlagsKHR preTransform = surfaceCapabilities.SupportedTransforms.HasFlag(SurfaceTransformFlagsKHR.IdentityBitKhr)
                                         ? SurfaceTransformFlagsKHR.IdentityBitKhr
                                         : surfaceCapabilities.CurrentTransform;

            CompositeAlphaFlagsKHR compositeAlpha =
                surfaceCapabilities.SupportedCompositeAlpha.HasFlag(CompositeAlphaFlagsKHR.PreMultipliedBitKhr) ? CompositeAlphaFlagsKHR.PreMultipliedBitKhr :
                surfaceCapabilities.SupportedCompositeAlpha.HasFlag(CompositeAlphaFlagsKHR.PostMultipliedBitKhr) ? CompositeAlphaFlagsKHR.PostMultipliedBitKhr :
                surfaceCapabilities.SupportedCompositeAlpha.HasFlag(CompositeAlphaFlagsKHR.InheritBitKhr) ? CompositeAlphaFlagsKHR.InheritBitKhr :
                CompositeAlphaFlagsKHR.OpaqueBitKhr;

            colorFormat = surfaceFormat.Format;
            swapchainExtent = new Extent2D();
            if (surfaceCapabilities.CurrentExtent.Width != uint.MaxValue)
            {
                swapchainExtent = surfaceCapabilities.CurrentExtent;
            }
            else
            {
                Extent2D actualExtent = new Extent2D()
                {
                    Width = (uint)SilkVulkanRenderer.window.FramebufferSize.X,
                    Height = (uint)SilkVulkanRenderer.window.FramebufferSize.Y
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
                    imageColorSpace: surfaceFormat.ColorSpace,
                    imageExtent: swapchainExtent,
                    imageArrayLayers: 1,
                    imageUsage: ImageUsageFlags.ColorAttachmentBit | ImageUsageFlags.TransferSrcBit,
                    imageSharingMode: SharingMode.Exclusive,
                    preTransform: preTransform,
                    compositeAlpha: compositeAlpha,
                    presentMode: presentMode,
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

            if (!SilkVulkanRenderer.vulkan.TryGetDeviceExtension(SilkVulkanRenderer.instance, SilkVulkanRenderer.device, out KhrSwapchain khrSwapChain))
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
                SilkVulkanRenderer.vulkan.CreateImageView(SilkVulkanRenderer.device, &viewInfo, null, out view);
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

        public SurfaceCapabilitiesKHR GetSurfaceCapabilitiesKHR(KhrSurface khrSurface, SurfaceKHR surfacekhrt)
        {
            Result result = khrSurface.GetPhysicalDeviceSurfaceCapabilities(SilkVulkanRenderer.physicalDevice, surfacekhrt, out SurfaceCapabilitiesKHR surfaceCapabilities);
            return surfaceCapabilities;
        }

        public PresentModeKHR[] GetPresentFormats(KhrSurface khrSurface, SurfaceKHR surfacekhrt)
        {
            uint presentModesCount = 0;
            Result result = khrSurface.GetPhysicalDeviceSurfacePresentModes(SilkVulkanRenderer.physicalDevice, surfacekhrt, &presentModesCount, null);
            PresentModeKHR[] presentModes = new PresentModeKHR[presentModesCount];
            result = khrSurface.GetPhysicalDeviceSurfacePresentModes(SilkVulkanRenderer.physicalDevice, surfacekhrt, &presentModesCount, presentModes);

            return presentModes;
        }

        public PresentModeKHR GetPresentFormat(PresentModeKHR[] availablePresentModes)
        {
            PresentModeKHR pickedMode = PresentModeKHR.FifoKhr;

            foreach (var availablePresentMode in availablePresentModes)
            {
                if (availablePresentMode == PresentModeKHR.MailboxKhr)
                {
                    return availablePresentMode;
                }

                if (availablePresentMode == PresentModeKHR.ImmediateKhr)
                {
                    pickedMode = PresentModeKHR.ImmediateKhr;
                }
            }

            return pickedMode;
        }

        public class SurfaceResult
        {
            public SurfaceKHR Surface { get; set; } = new SurfaceKHR();
            public KhrSurface KhrSurface { get; set; }
        }

        public SurfaceFormatKHR[] GetSurfaceFormats(KhrSurface khrSurface, SurfaceKHR surfacekhrt)
        {
            uint surfaceFormatCount = 0;
            Result result = khrSurface.GetPhysicalDeviceSurfaceFormats(SilkVulkanRenderer.physicalDevice, surfacekhrt, &surfaceFormatCount, null);
            SurfaceFormatKHR[] surfaceFormats = new SurfaceFormatKHR[surfaceFormatCount];
            khrSurface.GetPhysicalDeviceSurfaceFormats(SilkVulkanRenderer.physicalDevice, surfacekhrt, &surfaceFormatCount, surfaceFormats);

            return surfaceFormats;
        }

        public SurfaceFormatKHR GetSurfaceFormat(SurfaceFormatKHR[] formats)
        {
            SurfaceFormatKHR format = formats[0];
            Format[] requestedFormats = new Format[] { Format.B8G8R8A8Srgb, Format.R8G8B8A8Srgb, Format.B8G8R8Unorm, Format.R8G8B8Unorm };
            ColorSpaceKHR requestedColorSpace = ColorSpaceKHR.SpaceSrgbNonlinearKhr;
            for (int i = 0; i < requestedFormats.Length; i++)
            {
                Format requestedFormat = requestedFormats[i];

                if (formats.Any(f => f.Format == requestedFormat && f.ColorSpace == requestedColorSpace))
                {
                    format.Format = requestedFormat;
                    format.ColorSpace = requestedColorSpace;
                    break;
                }

            }
            return format;
        }
    }
}
