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
using System.Diagnostics;
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

        private static Scene scene = new Scene();
        private volatile bool running;
        private volatile bool levelEditorRunning;
        private Stopwatch stopwatch = new Stopwatch();
        private Thread renderThread { get; set; }
        public RenderPassBuildInfoModel renderPass { get; private set; } = new RenderPassBuildInfoModel();
        public MessengerModel RenderPassMessager { get; set; }

        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();

        public Form1()
        {
            InitializeComponent();
            AllocConsole();

            this.Load += Form1_Load;

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
                VulkanRenderer.CreateVulkanRenderer(pictureBox1.Handle);
            }));

            stopwatch.Start();
            scene.StartUp();
            while (running)
            {
                float deltaTime = (float)stopwatch.Elapsed.TotalSeconds;
                stopwatch.Restart();
                scene.Update(deltaTime);
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

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void SaveLevel_Click(object sender, EventArgs e)
        {

        }
    }
}