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
        private Extent2D VulkanSwapChainResolution { get; set; }
        private Thread renderThread;
        private Thread levelEditerDisplayThread;
        private volatile bool running;
        private volatile bool levelEditorRunning;
        private BlockingCollection<byte[]> dataCollection = new BlockingCollection<byte[]>(boundedCapacity: 10);
        private BlockingCollection<Bitmap> levelEditorImage = new BlockingCollection<Bitmap>(boundedCapacity: 10);
        private System.Windows.Forms.Timer renderTimer;
        private static Scene scene;
        public bool isFramebufferResized = false;
        public RenderPassModel renderPass { get; private set; } = new RenderPassModel();
        public MessengerModel RenderPassMessager { get; set; }

        IWindow window;
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;

            VulkanSwapChainResolution = new Extent2D() { Width = 1280, Height = 720 };
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
            StartRenderer();
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
            this.Invoke(new Action(() =>
            {
                VulkanRenderer.CreateVulkanRenderer(this.pictureBox1.Handle, VulkanSwapChainResolution);
            }));

            scene = new Scene();
            scene.StartUp();
            while (running)
            {
                scene.Update();
                scene.DrawFrame();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            using (RenderPassBuilder renderPassBuilder = new RenderPassBuilder())
            {
                RenderPassMessager.IsActive = false;
                if (renderPassBuilder.ShowDialog() == DialogResult.OK)
                {
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