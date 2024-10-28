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
        private Vk vk = Vk.GetApi();
        private static Scene scene;
        private volatile bool running;
        private volatile bool levelEditorRunning;

        private Extent2D VulkanSwapChainResolution { get; set; }
        private Thread renderThread { get; set; }
        private System.Windows.Forms.Timer renderTimer { get; set; }
        public RenderPassBuildInfoModel renderPass { get; private set; } = new RenderPassBuildInfoModel();
        public MessengerModel RenderPassMessager { get; set; }

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;

            VulkanSwapChainResolution = new Extent2D() { Width = 1280, Height = 720 };
            Thread.CurrentThread.Name = "LevelEditor";
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
                IsBackground = true,
                Name = "VulkanRenderer"
            };
            renderThread.Start();
        }

        private void RenderLoop()
        {
            RenderPassMessager = new MessengerModel()
            {
                IsActive = true,
                richTextBox = richTextBox1,
                TextBoxName = richTextBox1.Name,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
            };
            GlobalMessenger.AddMessenger(RenderPassMessager);

            this.Invoke(new Action(() =>
            {
                VulkanRenderer.CreateVulkanRenderer(this.pictureBox1.Handle, VulkanSwapChainResolution);
            }));

            scene = new Scene();
            scene.StartUp();
            while (running)
            {
                scene.Update(0);
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