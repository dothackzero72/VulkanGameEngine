using GlmSharp;
using StbImageSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor
{
    public unsafe partial class Form1 : Form
    {
        private Thread renderThread;
        private volatile bool running;
        private BlockingCollection<byte[]> dataCollection = new BlockingCollection<byte[]>(boundedCapacity: 10);
        private System.Windows.Forms.Timer renderTimer;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeRenderTimer();
            StartRenderer();
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

        private void RenderLoop()
        {
            Scene scene = new Scene();
            this.Invoke(new Action(() =>
            {
                VulkanRenderer.SetUpRenderer(this.Handle, pictureBox1);
                scene.StartUp();
            }));

            while (running)
            {
                scene.Update(0);
                byte[] textureData = UpdateBitmapData(scene.testRenderPass2D.texture);
                dataCollection.TryAdd(textureData);
                scene.Draw();
                Thread.Sleep(16);
            }
        }

        [DllImport("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DLL_stbi_write_bmp(string filename, int w, int h, int comp, IntPtr data);
        //public static extern int stbi_write_png(string filename, int w, int h, int comp, IntPtr data, int stride_in_bytes);
        public static void WriteImage(string filePath, byte[] imageData, int width, int height)
        {


            // Calculate the size of the image data
            int comp = 4; // For RGBA

            // Convert byte array to IntPtr
            IntPtr dataPtr = Marshal.AllocHGlobal(imageData.Length);
            Marshal.Copy(imageData, 0, dataPtr, imageData.Length);

            try
            {
                // Write the PNG image
                int result = DLL_stbi_write_bmp(filePath, width, height, comp, dataPtr);
                if (result == 0)
                {
                    Console.WriteLine("Failed to write image.");
                }
                else
                {
                    Console.WriteLine("Image written successfully.");
                }
            }
            finally
            {
                // Free the allocated memory
                Marshal.FreeHGlobal(dataPtr);
            }
        }

        public byte[] UpdateBitmapData(Texture texture)
        {
            var piexl = new Pixel(0x00, 0x00, 0x00, 0xFF);
            BakeTexture bakeTexture = new(piexl, new GlmSharp.ivec2(this.Width, this.Height), VkFormat.VK_FORMAT_R8G8B8A8_UNORM);

            VkCommandBufferAllocateInfo allocInfo;
            allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
            allocInfo.level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY;
            allocInfo.commandPool = VulkanRenderer.CommandPool;
            allocInfo.commandBufferCount = 1;

            VkCommandBuffer commandBuffer = new VkCommandBuffer();
            VulkanAPI.vkAllocateCommandBuffers(VulkanRenderer.Device, &allocInfo, &commandBuffer);

            VkCommandBufferBeginInfo beginInfo;
            beginInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO;
            beginInfo.flags = VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT;

            VulkanAPI.vkBeginCommandBuffer(commandBuffer, &beginInfo);

            bakeTexture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
            texture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL);

            VkImageCopy copyImage;
            copyImage.srcSubresource.aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT;
            copyImage.srcSubresource.layerCount = 1;
            copyImage.dstSubresource.aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT;
            copyImage.dstSubresource.layerCount = 1;
            copyImage.dstOffset = new VkOffset3D { X = 0, Y = 0, Z = 0 };
            copyImage.extent.Width = (uint)texture.Width;
            copyImage.extent.Height = (uint)texture.Height;
            copyImage.extent.Depth = 1;
            VulkanAPI.vkCmdCopyImage(commandBuffer, texture.Image, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL, bakeTexture.Image, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &copyImage);

            bakeTexture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_GENERAL);
            texture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);

            VulkanAPI.vkEndCommandBuffer(commandBuffer);

            byte* data;
            VulkanAPI.vkMapMemory(VulkanRenderer.Device, bakeTexture.Memory, 0, VulkanConsts.VK_WHOLE_SIZE, 0, (void**)&data);

            int pixelCount = bakeTexture.Width * bakeTexture.Height;
            byte[] pixelData = new byte[pixelCount * (int)bakeTexture.ColorChannels];
            byte[] sourcePixelData = new byte[pixelCount * (int)bakeTexture.ColorChannels];
            for (int y = 0; y < bakeTexture.Height; y++)
            {
                for (int x = 0; x < bakeTexture.Width; x++)
                {
                    int index = (y * bakeTexture.Width + x) * (int)bakeTexture.ColorChannels;
                    sourcePixelData[index + 0] = data[index + 0]; // Alpha
                    sourcePixelData[index + 1] = data[index + 1]; // Red
                    sourcePixelData[index + 2] = data[index + 2]; // Green
                    sourcePixelData[index + 3] = data[index + 3]; // Blue

                    pixelData[index + 0] = data[index + 3]; // Alpha
                    pixelData[index + 1] = data[index + 0]; // Red
                    pixelData[index + 2] = data[index + 1]; // Green
                    pixelData[index + 3] = data[index + 2]; // Blue
                }
            }
            WriteImage2("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/asdfa.bmp", pixelData, (int)texture.ColorChannels);

            //unsafe
            //{
            //    for (int y = 0; y < bakeTexture.Height; y++)
            //    {
            //        for (int x = 0; x < bakeTexture.Width; x++)
            //        {
            //            int index = (y * bakeTexture.Width + x) * (int)bakeTexture.ColorChannels;

            //            byte b = data[index + 0]; // blue
            //            byte g = data[index + 1]; // green
            //            byte r = data[index + 2]; // red
            //            byte a = data[index + 3]; // alpha

            //            // Reorder to ARGB
            //            pixelData[index + 0] = a; // Alpha
            //            pixelData[index + 1] = r; // Red
            //            pixelData[index + 2] = g; // Green
            //            pixelData[index + 3] = b; // Blue
            //        }
            //    }
            //}
           
            VulkanAPI.vkUnmapMemory(VulkanRenderer.Device, bakeTexture.Memory);

            return pixelData;
        }

        public void WriteImage2(string filePath, byte[] imageData, int colorChannels)
        {
            IntPtr dataPtr = Marshal.AllocHGlobal(imageData.Length);
            Marshal.Copy(imageData, 0, dataPtr, imageData.Length);
            try
            {
                int result = DLL_stbi_write_bmp(filePath, this.Width, this.Height, colorChannels, dataPtr);
                if (result == 0)
                {
                    Console.WriteLine("Failed to write image.");
                }
                else
                {
                    Console.WriteLine("Image written successfully.");
                }
            }
            finally
            {
                Marshal.FreeHGlobal(dataPtr);
            }
        }

        private void UpdateBitmap(object sender, EventArgs e)
        {
            byte[] textureData;
            if (dataCollection.TryTake(out textureData))
            {
                if (pictureBox1.InvokeRequired)
                {
                    pictureBox1.Invoke(new Action(() => UpdateBitmapWithData(textureData)));
                }
                else
                {
                    UpdateBitmapWithData(textureData);
                }
            }
        }


        private void UpdateBitmapWithData(byte[] textureData)
        {
            if(textureData == null)
            {
                return;
            }

            using (Bitmap bitmap = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height, PixelFormat.Format32bppArgb))
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        int index = (y * bitmap.Width + x) * 4;
                        Color pixelColor = ByteArrayToColor(textureData, index);
                        bitmap.SetPixel(x, y, pixelColor);
                    }
                }

                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                }

                pictureBox1.Image = (Bitmap)bitmap.Clone();
                pictureBox1.Refresh();
            }
        }

        private byte[] GenerateColorByteData(int width, int height)
        {
            byte[] data = new byte[width * height * 4];
            Random rnd = new Random();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color randomColor = Color.FromArgb(255, rnd.Next(256), rnd.Next(256), rnd.Next(256));
                    int index = (y * width + x) * 4;
                    data[index + 0] = randomColor.A;
                    data[index + 1] = randomColor.R;
                    data[index + 2] = randomColor.G;
                    data[index + 3] = randomColor.B;
                }
            }

            return data;
        }

        private Color ByteArrayToColor(byte[] data, int index)
        {
            byte a = data[index + 0];
            byte r = data[index + 1];
            byte g = data[index + 2];
            byte b = data[index + 3];
            return Color.FromArgb(a, r, g, b);
        }

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