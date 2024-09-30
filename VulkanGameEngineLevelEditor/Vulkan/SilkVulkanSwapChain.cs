
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public unsafe class VulkanSwapChain
    {
        public Format colorFormat { get; private set; }
        public Extent2D swapchainExtent { get; set; }
        public SwapchainKHR swapChain { get; private set; }
        public Image[] images { get; private set; }
        public ImageView[] imageViews { get; private set; }
        public KhrSwapchain khrSwapchain { get; private set; }
        public uint ImageCount { get; private set; } = VulkanRenderer.MAX_FRAMES_IN_FLIGHT;
        public void CreateSwapChain()
        {
            SurfaceFormatKHR surfaceFormat = GetSurfaceFormat(GetSurfaceFormats());
            PresentModeKHR presentMode = GetPresentFormat(GetPresentFormats());
            SurfaceCapabilitiesKHR surfaceCapabilities = GetSurfaceCapabilitiesKHR();

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
                    Width = (uint)VulkanRenderer.window.FramebufferSize.X,
                    Height = (uint)VulkanRenderer.window.FramebufferSize.Y
                };

                Extent2D actualExtent2D = new Extent2D();
                actualExtent2D.Width = actualExtent.Width;
                actualExtent2D.Height = actualExtent.Height;
                swapchainExtent = actualExtent2D;
            }

            SwapchainCreateInfoKHR createInfo = new SwapchainCreateInfoKHR
                (
                    surface: VulkanRenderer.surface,
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

            if (VulkanRenderer.GraphicsFamily != VulkanRenderer.PresentFamily)
            {
                uint* queueFamilyIndices = stackalloc uint[] { VulkanRenderer.GraphicsFamily, VulkanRenderer.PresentFamily };
                createInfo.ImageSharingMode = SharingMode.Concurrent;
                createInfo.QueueFamilyIndexCount = 2;
                createInfo.PQueueFamilyIndices = queueFamilyIndices;
            }

            if (!VulkanRenderer.vulkan.TryGetDeviceExtension(VulkanRenderer.instance, VulkanRenderer.device, out KhrSwapchain khrSwapChain))
            {
                throw new NotSupportedException("KHR_swapchain extension not found.");
            }
            khrSwapchain = khrSwapChain;

            var Swapchain = new SwapchainKHR();
            if (khrSwapchain.CreateSwapchain(VulkanRenderer.device, &createInfo, null, out Swapchain) != Result.Success)
            {
                throw new Exception("failed to create swap chain!");
            }
            swapChain = Swapchain;

            images = GetImagesKHR();
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
                VulkanRenderer.vulkan.CreateImageView(VulkanRenderer.device, &viewInfo, null, out view);
                imageViews[x] = view;
            }
        }

        public Image[] GetImagesKHR()
        {
            uint imageCount = 0;
            khrSwapchain.GetSwapchainImages(VulkanRenderer.device, swapChain, &imageCount, null);
            Image[] swapChainImages = new Image[imageCount];
            khrSwapchain.GetSwapchainImages(VulkanRenderer.device, swapChain, &imageCount, swapChainImages);
            return swapChainImages;
        }

        private SurfaceCapabilitiesKHR GetSurfaceCapabilitiesKHR()
        {
            Result result = VulkanRenderer.khrSurface.GetPhysicalDeviceSurfaceCapabilities(VulkanRenderer.physicalDevice, VulkanRenderer.surface, out SurfaceCapabilitiesKHR surfaceCapabilities);
            return surfaceCapabilities;
        }

        private PresentModeKHR[] GetPresentFormats()
        {
            uint presentModesCount = 0;
            Result result = VulkanRenderer.khrSurface.GetPhysicalDeviceSurfacePresentModes(VulkanRenderer.physicalDevice, VulkanRenderer.surface, &presentModesCount, null);
            PresentModeKHR[] presentModes = new PresentModeKHR[presentModesCount];
            result = VulkanRenderer.khrSurface.GetPhysicalDeviceSurfacePresentModes(VulkanRenderer.physicalDevice, VulkanRenderer.surface, &presentModesCount, presentModes);

            return presentModes;
        }

        private PresentModeKHR GetPresentFormat(PresentModeKHR[] availablePresentModes)
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

        private SurfaceFormatKHR[] GetSurfaceFormats()
        {
            uint surfaceFormatCount = 0;
            Result result = VulkanRenderer.khrSurface.GetPhysicalDeviceSurfaceFormats(VulkanRenderer.physicalDevice, VulkanRenderer.surface, &surfaceFormatCount, null);
            SurfaceFormatKHR[] surfaceFormats = new SurfaceFormatKHR[surfaceFormatCount];
            VulkanRenderer.khrSurface.GetPhysicalDeviceSurfaceFormats(VulkanRenderer.physicalDevice, VulkanRenderer.surface, &surfaceFormatCount, surfaceFormats);

            return surfaceFormats;
        }

        private SurfaceFormatKHR GetSurfaceFormat(SurfaceFormatKHR[] formats)
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
