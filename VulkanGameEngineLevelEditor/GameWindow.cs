using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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


        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();

        public GameWindow()
        {
            InitializeComponent();
            AllocConsole();

            this.Load += Form1_Load;
            this.KeyDown += KeyPress_Down;
            this.KeyUp += KeyPress_Up;
            // this.KeyDown += Form1_KeyDown;
            Console.WriteLine("asdfasdf");
            VulkanSwapChainResolution = new Extent2D() { Width = 1280, Height = 720 };
            Thread.CurrentThread.Name = "LevelEditor";
        }

        private void KeyPress_Down(object sender, KeyEventArgs e)
        {
            scene.Input(e);
        }

        private void KeyPress_Up(object sender, KeyEventArgs e)
        {
            scene.Input(e);
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
               // scene.KeyUpdate();
                scene.DrawFrame();
            }

            scene.Destroy();
        }

    }
}
