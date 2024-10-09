using GlmSharp;
using Silk.NET.Maths;
using Silk.NET.Vulkan;
using Silk.NET.Windowing;
using StbImageSharp;
using StbImageWriteSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
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

            levelEditorSwapChain = new LevelEditorDisplaySwapChain(new ivec2(1280, 720));

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
            opts.Size = new Vector2D<int>(1280, 720);

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
                byte[] textureData = BakeColorTexture(scene.silk3DRendererPass.renderedTexture);
                dataCollection.TryAdd(textureData);
                // vulkanTutorial.DrawFrame();
                // byte[] textureData = BakeColorTexture(vulkanTutorial.texture);
                // dataCollection.TryAdd(textureData);
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

            using (FileStream fileStream = new FileStream("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\texturerenderer.bmp", FileMode.Create, FileAccess.Write))
            {
                StbImageWriteSharp.ImageWriter imageWriter = new ImageWriter();
                imageWriter.WriteBmp(pixelData, bakeTexture.Width, bakeTexture.Height, StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, fileStream);
            }

            return ConvertB8G8R8A8ToARGB(pixelData);
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
                int srcIndex = i * 4;          // B8G8R8A8: 0(B) 1(G) 2(R) 3(A)
                int destIndex = i * 4;         // ARGB: 0(A) 1(R) 2(G) 3(B)

                argbData[destIndex + 0] = pixelData[srcIndex + 0]; // Alpha
                argbData[destIndex + 1] = pixelData[srcIndex + 1]; // Red
                argbData[destIndex + 2] = pixelData[srcIndex + 2]; // Green
                argbData[destIndex + 3] = pixelData[srcIndex + 3]; // Blue
            }

            return argbData;
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
                    levelEditorSwapChain.UpdateBuffer(textureData, SilkVulkanRenderer.swapChain.swapchainExtent);
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            using (RenderPassBuilder popup = new RenderPassBuilder())
            {
                DialogResult result = popup.ShowDialog(); // Open Modal
                if (result == DialogResult.OK)
                {
                    MessageBox.Show("Popup closed.");
                }
            }
        }
    }
}