using GlmSharp;
using StbImageSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
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

        public LevelEditorDisplaySwapChain(ivec2 imageSize)
        {
            BitMapBuffers = new List<Bitmap>();
            CurrentBufferIndex = 0;
            SwapChainSize = imageSize;
            for (int x = 0; x < BufferCount; x++)
            {
                BitMapBuffers.Add(new Bitmap(imageSize.x, imageSize.y, PixelFormat.Format32bppArgb));
            }
        }

        public void UpdateBuffer(byte[] pixelData)
        {
            if (pixelData == null)
            {
                return;
            }

            Bitmap currentBitmap = BitMapBuffers[CurrentBufferIndex];
            for (int y = 0; y < BitMapBuffers[CurrentBufferIndex].Height; y++)
            {
                for (int x = 0; x < BitMapBuffers[CurrentBufferIndex].Width; x++)
                {
                    int index = (y * BitMapBuffers[CurrentBufferIndex].Width + x) * 4;
                    Color pixelColor = ByteArrayToColor(pixelData, index);
                    BitMapBuffers[CurrentBufferIndex].SetPixel(x, y, pixelColor);
                }
            }

            lock (BufferLock)
            {
                DisplayImage = currentBitmap;
                BitMapBuffers[CurrentBufferIndex] = currentBitmap;
            }
            CurrentBufferIndex = (CurrentBufferIndex + 1) % BufferCount;
        }

        public void PresentImage(PictureBox picture)
        {
            if (DisplayImage == null)
            {
                return;
            }

            if (picture.Image != null)
            {
                picture.Image.Dispose();
            }

            picture.Image = (Bitmap)DisplayImage.Clone();
            picture.Refresh();
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
