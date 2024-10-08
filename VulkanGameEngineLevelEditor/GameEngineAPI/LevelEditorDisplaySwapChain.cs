using GlmSharp;
using Silk.NET.Vulkan;
using StbImageSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
            DisplayImage = new Bitmap(imageSize.x, imageSize.y, PixelFormat.Format32bppArgb);
            for (int x = 0; x < BufferCount; x++)
            {
                BitMapBuffers.Add(new Bitmap(imageSize.x, imageSize.y, PixelFormat.Format32bppArgb));
            }
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

                currentBitmap.Save("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\texturerenderer.png", System.Drawing.Imaging.ImageFormat.Png);

                BitMapBuffers[CurrentBufferIndex] = currentBitmap;
                display.TryAdd(currentBitmap);
            }

            CurrentBufferIndex = (CurrentBufferIndex + 1) % BufferCount;
        }

        public void PresentImage(PictureBox picture)
        {
            if (display.TryTake(out Bitmap textureData))
            {
                // Lock not really needed for this operation as we're not modifying the shared resource.
                if (picture.Image != null)
                {
                    picture.Image.Dispose();
                }

                picture.Image = (Bitmap)textureData.Clone();
                picture.Refresh();
            }
        }

        private Color ByteArrayToColor(byte[] data, int index)
        {
            byte a = data[index + 3];
            byte r = data[index + 0];
            byte g = data[index + 1];
            byte b = data[index + 2];
            return Color.FromArgb(a, r, g, b);
        }
    }
}
