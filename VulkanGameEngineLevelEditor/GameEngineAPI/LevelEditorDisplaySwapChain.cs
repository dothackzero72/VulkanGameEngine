using GlmSharp;
using Silk.NET.Vulkan;
using StbImageSharp;
using StbImageWriteSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class LevelEditorDisplaySwapChain
    {
        const int BufferCount = SilkVulkanRenderer.MAX_FRAMES_IN_FLIGHT;
        public List<Bitmap> BitMapBuffers { get; protected set; } = new List<Bitmap>();
        public Bitmap DisplayImage { get; protected set; }
        public int CurrentBufferIndex { get; protected set; } = 0;
        private ivec2 SwapChainSize { get; set; } = new ivec2(0, 0);
        private readonly Object BufferLock = new Object();
        private BlockingCollection<Bitmap> display = new BlockingCollection<Bitmap>(boundedCapacity: 10);
        public LevelEditorDisplaySwapChain(ivec2 imageSize)
        {
            BitMapBuffers = new List<Bitmap>();
            CurrentBufferIndex = 0;
            SwapChainSize = imageSize;
            DisplayImage = new Bitmap(imageSize.x, imageSize.y, PixelFormat.Format32bppRgb);
            for (int x = 0; x < BufferCount; x++)
            {
                BitMapBuffers.Add(new Bitmap(imageSize.x, imageSize.y, PixelFormat.Format32bppRgb));
            }
        }

        public unsafe byte[] BakeColorTexture(Texture texture)
        {
            BakeTexture bakeTexture = new BakeTexture(new Pixel(0xFF, 0xFF, 0xFF, 0xFF), new GlmSharp.ivec2(1280, 720), Format.R8G8B8A8Unorm);

            CommandBuffer commandBuffer = SilkVulkanRenderer.BeginSingleUseCommandBuffer();
            texture.UpdateImageLayout(commandBuffer, Silk.NET.Vulkan.ImageLayout.PresentSrcKhr, Silk.NET.Vulkan.ImageLayout.TransferSrcOptimal, ImageAspectFlags.ColorBit);
            bakeTexture.UpdateImageLayout(commandBuffer, Silk.NET.Vulkan.ImageLayout.Undefined, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal, ImageAspectFlags.ColorBit);

            ImageCopy copyImage = new ImageCopy
            {
                SrcSubresource = { AspectMask = ImageAspectFlags.ColorBit, LayerCount = 1, MipLevel = 0 },
                DstSubresource = { AspectMask = ImageAspectFlags.ColorBit, LayerCount = 1, MipLevel = 0 },
                DstOffset = { X = 0, Y = 0, Z = 0 },
                Extent = { Width = (uint)texture.Width, Height = (uint)texture.Height, Depth = 1 }
            };

            VKConst.vulkan.CmdCopyImage(commandBuffer,
                texture.Image, Silk.NET.Vulkan.ImageLayout.TransferSrcOptimal,
                bakeTexture.Image, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal,
                1, &copyImage);
            bakeTexture.UpdateImageLayout(commandBuffer, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal, Silk.NET.Vulkan.ImageLayout.ShaderReadOnlyOptimal, ImageAspectFlags.ColorBit);

            SilkVulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
            ImageSubresource subResource = new ImageSubresource { AspectMask = ImageAspectFlags.ColorBit, MipLevel = 0, ArrayLayer = 0 };
            SubresourceLayout subResourceLayout;
            VKConst.vulkan.GetImageSubresourceLayout(SilkVulkanRenderer.device, bakeTexture.Image, &subResource, &subResourceLayout);

            int pixelCount = bakeTexture.Width * bakeTexture.Height;
            byte[] pixelData = new byte[pixelCount * 4];

            IntPtr mappedMemory;
            VKConst.vulkan.MapMemory(SilkVulkanRenderer.device, bakeTexture.Memory, 0, Vk.WholeSize, 0, (void**)&mappedMemory);
            Marshal.Copy(mappedMemory, pixelData, 0, pixelData.Length);
            VKConst.vulkan.UnmapMemory(SilkVulkanRenderer.device, bakeTexture.Memory);

            return ConvertB8G8R8A8ToARGB(pixelData);
        }

        public void UpdateBuffer(byte[] pixelData, Extent2D imageSize)
        {
            if (pixelData == null)
            {
                return;
            }

            lock (BufferLock)
            {
                Bitmap currentBitmap = new Bitmap((int)imageSize.Width, (int)imageSize.Height, PixelFormat.Format32bppRgb);
                BitmapData bitmapData = currentBitmap.LockBits(new System.Drawing.Rectangle(0, 0, currentBitmap.Width, currentBitmap.Height),
                                                                ImageLockMode.WriteOnly,
                                                                currentBitmap.PixelFormat);

                try
                {
                    Marshal.Copy(pixelData, 0, bitmapData.Scan0, pixelData.Length);
                }
                finally
                {
                    currentBitmap.UnlockBits(bitmapData);
                }

                BitMapBuffers[CurrentBufferIndex] = currentBitmap;
                display.TryAdd(currentBitmap);
            }
           

            CurrentBufferIndex = (CurrentBufferIndex + 1) % BufferCount;
        }

        public void PresentImage(PictureBox picture)
        {
            if (display.TryTake(out Bitmap textureData))
            {
                if (picture.Image != null)
                {
                    picture.Image.Dispose();
                }

                picture.Image = (Bitmap)textureData.Clone();
                picture.Refresh();
            }
        }

        private byte[] ConvertB8G8R8A8ToARGB(byte[] pixelData)
        {
            if (pixelData == null || pixelData.Length == 0 || pixelData.Length % 4 != 0)
            {
                throw new ArgumentException("Invalid pixel data array.");
            }

            int pixelCount = pixelData.Length / 4;
            byte[] argbData = new byte[pixelCount * 4];

            for (int i = 0; i < pixelCount; i++)
            {
                int srcIndex = i * 4;
                int destIndex = i * 4;

                argbData[destIndex + 0] = pixelData[srcIndex + 2];
                argbData[destIndex + 1] = pixelData[srcIndex + 1];
                argbData[destIndex + 2] = pixelData[srcIndex + 0];
                argbData[destIndex + 3] = pixelData[srcIndex + 3];
            }

            return argbData;
        }
    }
}
