using GlmSharp;
using Newtonsoft.Json;
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
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.RenderPassWindows;
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
        private LevelEditorDisplaySwapChain levelEditorSwapChain;
        public RenderPassModel renderPass { get; private set; } = new RenderPassModel();
        public MessengerModel RenderPassMessager { get; set; }

        IWindow window;
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;

            RenderPassMessager = new MessengerModel()
            {
                IsActive = true,
                richTextBox = richTextBox1,
                TextBoxName = richTextBox1.Name
            };
            GlobalMessenger.AddMessenger(RenderPassMessager);
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
            opts.Title = "Level Editor";
            opts.Size = new Vector2D<int>(1280, 720);

            window = Silk.NET.Windowing.Window.Create(opts);
            window.Initialize();

            scene = new Scene();
            scene.StartUp(window, richTextBox1);
            while (running)
            {
                //scene.Update(0);
                scene.DrawFrame();
                byte[] textureData = levelEditorSwapChain.BakeColorTexture(scene.silk3DRendererPass.renderedTexture);
                dataCollection.TryAdd(textureData);
            }
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

        private void UpdateBitmap(object sender, EventArgs e)
        {
            pictureBox1.Invoke(new Action(() => levelEditorSwapChain.PresentImage(pictureBox1)));
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            using (RenderPassBuilder renderPassBuilder = new RenderPassBuilder())
            {
                RenderPassMessager.IsActive = false;
                if (renderPassBuilder.ShowDialog() == DialogResult.OK)
                {
                    switch(renderPassBuilder.RenderedTextureAttachments.TextureType)
                    {
                        case RenderedTextureType.ColorRenderedTexture: renderPass.ColorAttachmentList.Add(renderPassBuilder.RenderedTextureAttachments); break;
                        case RenderedTextureType.DepthRenderedTexture: renderPass.DepthAttachmentList.Add(renderPassBuilder.RenderedTextureAttachments); break;
                        case RenderedTextureType.ResolveAttachmentTexture: renderPass.ResolveAttachmentList.Add(renderPassBuilder.RenderedTextureAttachments); break;
                        case RenderedTextureType.InputAttachmentTexture: renderPass.InputAttachmentList.Add(renderPassBuilder.RenderedTextureAttachments); break;
                    }
                    RenderPassMessager.IsActive = false;
                }
            }
        }

        private void SaveRenderPass_Click(object sender, EventArgs e)
        {
           var a = JsonConvert.SerializeObject(renderPass, Formatting.Indented);
            var ab = 32;
        }
    }
}