using GlmSharp;
using StbImageSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using static VulkanGameEngineLevelEditor.VulkanAPI;

namespace VulkanGameEngineLevelEditor
{
    public unsafe partial class Form1 : Form
    {
        private Thread renderThread;
        private Thread levelEditerDisplayThread;
        private volatile bool running;
        private volatile bool levelEditorRunning;
        private BlockingCollection<byte[]> dataCollection = new BlockingCollection<byte[]>(boundedCapacity: 10);
        private BlockingCollection<Bitmap> levelEditorImage = new BlockingCollection<Bitmap>(boundedCapacity: 10);
        private System.Windows.Forms.Timer renderTimer;
        private static Scene scene;
        private Bitmap[] bitmapBuffer = new Bitmap[3];
        private uint NextTexture = 0;
        private LevelEditorDisplaySwapChain levelEditorSwapChain;
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeRenderTimer();
            levelEditorSwapChain = new LevelEditorDisplaySwapChain(new ivec2(pictureBox1.Width, pictureBox1.Height));
            StartRenderer();
            StartLevelEditorRenderer();
        }

        private void InitializeRenderTimer()
        {
            renderTimer = new System.Windows.Forms.Timer
            {
                Interval = 100
            };
            renderTimer.Tick += UpdateBitmap;
            renderTimer.Start();
        }

        public void StartRenderer()
        {
            running = true;
            renderThread = new Thread(RenderLoop)
            {
                IsBackground = true
            };
            renderThread.Start();
        }

        public void StartLevelEditorRenderer()
        {
            running = true;
            renderThread = new Thread(LevelEditorLoop)
            {
                IsBackground = true
            };
            renderThread.Start();
        }

        private void LevelEditorLoop()
        {
            while (running)
            {
                if (dataCollection.TryTake(out byte[] textureData))
                {
                    levelEditorSwapChain.UpdateBuffer(textureData);
                }
            }
        }

        private void RenderLoop()
        {
             scene = new Scene();
            this.Invoke(new Action(() =>
            {
                VulkanRenderer.SetUpRenderer(this.Handle, pictureBox1);
                scene.StartUp();
            }));

            while (running)
            {
                scene.Update(0);
                byte[] textureData = BakeColorTexture("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/asdfa.bmp", scene.texture);
                dataCollection.TryAdd(textureData);
                scene.Draw();
                Thread.Sleep(16);
            }
        }

        [DllImport("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DLL_stbi_write_bmp(string filename, int w, int h, int comp, void* data);


        public unsafe byte[] BakeColorTexture(string filename, Texture texture)
        {
            var pixel = new Pixel(0xFF, 0x00, 0x00, 0xFF);
            BakeTexture bakeTexture = new BakeTexture(pixel, new GlmSharp.ivec2(1280, 720), VkFormat.VK_FORMAT_R8G8B8A8_UNORM);

            VkCommandBuffer commandBuffer = VulkanRenderer.BeginCommandBuffer();

            bakeTexture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
            texture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL);

            VkImageCopy copyImage = new VkImageCopy();
            copyImage.srcSubresource.aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT;
            copyImage.srcSubresource.layerCount = 1;

            copyImage.dstSubresource.aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT;
            copyImage.dstSubresource.layerCount = 1;

            copyImage.dstOffset.X = 0;
            copyImage.dstOffset.Y = 0;
            copyImage.dstOffset.Z = 0;

            copyImage.extent.Width = (uint)texture.Width;
            copyImage.extent.Height = (uint)texture.Height;
            copyImage.extent.Depth = 1;

            vkCmdCopyImage(commandBuffer, texture.Image, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL, bakeTexture.Image, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &copyImage);

            bakeTexture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_GENERAL);
            texture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR);
            VulkanRenderer.EndCommandBuffer(commandBuffer);

            VkImageSubresource subResource = new VkImageSubresource { aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT, mipLevel = 0, arrayLayer = 0 };
            VkSubresourceLayout subResourceLayout;
            vkGetImageSubresourceLayout(VulkanRenderer.Device, bakeTexture.Image, &subResource, &subResourceLayout);

            int pixelCount = bakeTexture.Width * bakeTexture.Height;
            byte[] pixelData = new byte[pixelCount * (int)bakeTexture.ColorChannels];

            IntPtr mappedMemory;
            VulkanAPI.vkMapMemory(VulkanRenderer.Device, bakeTexture.Memory, 0, VulkanConsts.VK_WHOLE_SIZE, 0, (void**)&mappedMemory);
            Marshal.Copy(mappedMemory, pixelData, 0, pixelCount * (int)bakeTexture.ColorChannels);
            VulkanAPI.vkUnmapMemory(VulkanRenderer.Device, bakeTexture.Memory);

            return pixelData;
        }
        public static void WriteImage(string filePath, byte[] imageData, int width, int height, int channels)
        {
                int result = DLL_stbi_write_bmp(filePath, width, height, channels, (void*)imageData.ToArray()[0]);
                if (result == 0)
                {
                    Console.WriteLine("Failed to write image.");
                }
                else
                {
                    Console.WriteLine("Image written successfully.");
                }
        }

        private void UpdateBitmap(object sender, EventArgs e)
        {
            pictureBox1.Invoke(new Action(() => levelEditorSwapChain.PresentImage(pictureBox1)));
        }
        //private void UpdateBitmapWithData(byte[] textureData)
        //{
        //    if (textureData == null)
        //    {
        //        return;
        //    }

        //    using (Bitmap bitmap = new Bitmap(scene.testRenderPass2D.texture.Width, scene.testRenderPass2D.texture.Height, PixelFormat.Format32bppArgb))
        //    {
        //        for (int y = 0; y < bitmap.Height; y++)
        //        {
        //            for (int x = 0; x < bitmap.Width; x++)
        //            {
        //                int index = (y * bitmap.Width + x) * 4;
        //                Color pixelColor = ByteArrayToColor(textureData, index);
        //                bitmap.SetPixel(x, y, pixelColor);
        //            }
        //        }

        //        if (pictureBox1.Image != null)
        //        {
        //            pictureBox1.Image.Dispose();
        //        }

        //        pictureBox1.Image = (Bitmap)bitmap.Clone();
        //        pictureBox1.Refresh();
        //    }
        //}

        public void StopRenderer()
        {
            running = false;
            if (renderThread != null && renderThread.IsAlive)
            {
                renderThread.Join();
            }
        }
    }
}