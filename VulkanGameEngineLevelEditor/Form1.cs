using GlmSharp;
using Silk.NET.Maths;
using Silk.NET.Vulkan;
using Silk.NET.Windowing;
using StbImageSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor
{
    public unsafe partial class Form1 : Form
    {
        Vk vk = Vk.GetApi();
        private SilkVulkanWindow windows;
        private Thread renderThread;
        private Thread levelEditerDisplayThread;
        private volatile bool running;
        private volatile bool levelEditorRunning;
        private BlockingCollection<byte[]> dataCollection = new BlockingCollection<byte[]>(boundedCapacity: 10);
        private BlockingCollection<Bitmap> levelEditorImage = new BlockingCollection<Bitmap>(boundedCapacity: 10);
        private System.Windows.Forms.Timer renderTimer;
        private static Scene scene;
        public bool isFramebufferResized = false;
        private Bitmap[] bitmapBuffer = new Bitmap[3];
        private uint NextTexture = 0;
        private LevelEditorDisplaySwapChain levelEditorSwapChain;

        IWindow window;
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //VulkanRenderer.CreateVulkanRenderer();
            //VulkanRenderer.window.FramebufferResize += OnFramebufferResize;

            //VulkanRenderer.window.Render += GameLoop;
            //VulkanRenderer.window.Run();
            //InitializeRenderTimer();
            levelEditorSwapChain = new LevelEditorDisplaySwapChain(new ivec2(pictureBox1.Width, pictureBox1.Height));

            StartLevelEditorRenderer();
            StartRenderer();
            InitializeRenderTimer();
        }

        void LoadVulkan()
        {
            //vulkanTutorial = new VulkanTutorial();
            //vulkanTutorial.Run();
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
            var opts = WindowOptions.DefaultVulkan;
            opts.Title = "Texture Demo";
            opts.Size = new Vector2D<int>((int)1280, (int)720);

            window = Silk.NET.Windowing.Window.Create(opts);
            window.Initialize();
           // window.FramebufferResize += OnFramebufferResize;
            //vulkanTutorial = new VulkanTutorial();
            //vulkanTutorial.Run(window);

            //  SilkVulkanRenderer.CreateVulkanRenderer(window, richTextBox1);

            scene = new Scene();
            scene.StartUp(window, richTextBox1);
            while (running)
            {
                //scene.Update(0);
                scene.DrawFrame();
                byte[] textureData = BakeColorTexture(scene.silk3DRendererPass.renderedColorTexture);
                dataCollection.TryAdd(textureData);
                // vulkanTutorial.DrawFrame();
                // byte[] textureData = BakeColorTexture(vulkanTutorial.texture);
                // dataCollection.TryAdd(textureData);
            }
        }

        public unsafe byte[] BakeColorTexture(Texture texture)
        {
            var pixel = new Pixel(0xFF, 0x00, 0x00, 0xFF);
            BakeTexture bakeTexture = new BakeTexture(new GlmSharp.ivec2(1280, 720));

            CommandBuffer commandBuffer = SilkVulkanRenderer.BeginSingleUseCommandBuffer();


            texture.UpdateImageLayout(commandBuffer, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal ,Silk.NET.Vulkan.ImageLayout.TransferSrcOptimal, ImageAspectFlags.ColorBit);
            bakeTexture.UpdateImageLayout(commandBuffer, Silk.NET.Vulkan.ImageLayout.Undefined, Silk.NET.Vulkan.ImageLayout.General, ImageAspectFlags.ColorBit);
            bakeTexture.UpdateImageLayout(commandBuffer, Silk.NET.Vulkan.ImageLayout.General, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal, ImageAspectFlags.ColorBit);

            ImageCopy copyImage = new ImageCopy();
            copyImage.SrcSubresource.AspectMask = ImageAspectFlags.ColorBit;
            copyImage.SrcSubresource.LayerCount = 1;

            copyImage.DstSubresource.AspectMask = ImageAspectFlags.ColorBit;
            copyImage.DstSubresource.LayerCount = 1;

            copyImage.DstOffset.X = 0;
            copyImage.DstOffset.Y = 0;
            copyImage.DstOffset.Z = 0;

            copyImage.Extent.Width = (uint)texture.Width;
            copyImage.Extent.Height = (uint)texture.Height;
            copyImage.Extent.Depth = 1;

            VKConst.vulkan.CmdCopyImage(commandBuffer, texture.Image, Silk.NET.Vulkan.ImageLayout.TransferSrcOptimal, bakeTexture.Image, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal, 1, &copyImage);

            bakeTexture.UpdateImageLayout(commandBuffer, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal, Silk.NET.Vulkan.ImageLayout.General, ImageAspectFlags.ColorBit);
            texture.UpdateImageLayout(commandBuffer, Silk.NET.Vulkan.ImageLayout.TransferSrcOptimal, Silk.NET.Vulkan.ImageLayout.PresentSrcKhr, ImageAspectFlags.ColorBit);
            SilkVulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);

            ImageSubresource subResource = new ImageSubresource { AspectMask = ImageAspectFlags.ColorBit, MipLevel = 0, ArrayLayer = 0 };
            SubresourceLayout subResourceLayout;
            VKConst.vulkan.GetImageSubresourceLayout(SilkVulkanRenderer.device, bakeTexture.Image, &subResource, &subResourceLayout);

            int pixelCount = bakeTexture.Width * bakeTexture.Height;
            byte[] pixelData = new byte[pixelCount * (int)bakeTexture.ColorChannels];

            IntPtr mappedMemory;
            VKConst.vulkan.MapMemory(SilkVulkanRenderer.device, bakeTexture.Memory, 0, Vk.WholeSize, 0, (void**)&mappedMemory);
            Marshal.Copy(mappedMemory, pixelData, 0, pixelCount * (int)bakeTexture.ColorChannels);
            VKConst.vulkan.UnmapMemory(SilkVulkanRenderer.device, bakeTexture.Memory);

            // Writing the pixel data correctly based on width, height, and channels
            fixed (byte* pixelPointer = pixelData)
            //{
            //    void* voidPointer = pixelPointer;
            //    StbImageWriteSharp.ImageWriter asd = new StbImageWriteSharp.ImageWriter();
            //    using (FileStream fileStream = new FileStream("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\texturerender234.bmp", FileMode.Create, FileAccess.Write))
            //    {
            //        asd.WriteBmp(voidPointer, Width, Height, StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, fileStream);
            //    }
            //}
            
            return pixelData;
        }
        public void StartLevelEditorRenderer()
        {
            running = true;
            levelEditerDisplayThread = new Thread(LevelEditorLoop)
            {
                IsBackground = true
            };
            levelEditerDisplayThread.Start();
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

        [DllImport("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DLL_stbi_write_bmp(string filename, int w, int h, int comp, void* data);

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
    }
}