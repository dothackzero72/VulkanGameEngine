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

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class LevelEditorDisplaySwapChain
    {
        const int BufferCount = VulkanRenderer.MAX_FRAMES_IN_FLIGHT;
        public List<Bitmap> BitMapBuffers { get; protected set; } = new List<Bitmap>();
        public int CurrentBufferIndex { get; protected set; } = 0;
        private ivec2 SwapChainSize { get; set; } = new ivec2(0,0);
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
            lock (BufferLock)
            {
                Bitmap currentBitmap = BitMapBuffers[CurrentBufferIndex];
                // BitmapData bitmapData = currentBitmap.LockBits(new Rectangle(0, 0, SwapChainSize.x, SwapChainSize.y), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                for (int y = 0; y < BitMapBuffers[CurrentBufferIndex].Height; y++)
                {
                    for (int x = 0; x < BitMapBuffers[CurrentBufferIndex].Width; x++)
                    {
                        int index = (y * BitMapBuffers[CurrentBufferIndex].Width + x) * 4;
                        Color pixelColor = ByteArrayToColor(pixelData, index);
                        BitMapBuffers[CurrentBufferIndex].SetPixel(x, y, pixelColor);
                    }
                }

                //currentBitmap.UnlockBits(bitmapData);

                CurrentBufferIndex = (CurrentBufferIndex + 1) % BufferCount;
            }
        }

        public void PresentImage(PictureBox picture)
        {
            lock (BufferLock)
            {
                if (picture.Image != null)
                {
                    picture.Image.Dispose();
                }

                picture.Image = (Bitmap)BitMapBuffers[CurrentBufferIndex].Clone();
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
