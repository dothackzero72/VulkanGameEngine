using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor
{
    public partial class GameWindow : Form
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

        public GameWindow()
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
                scene.Update();
                scene.DrawFrame();
            }
        }

    }
}
