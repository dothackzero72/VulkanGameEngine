using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Silk.NET.Vulkan;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngine.Systems;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor
{
    public partial class GameWindow : Form
    {
        private Vk vk = Vk.GetApi();
        private volatile bool running;
        private volatile bool levelEditorRunning;
        private Stopwatch stopwatch = new Stopwatch();
        private Extent2D VulkanSwapChainResolution { get; set; }
        private Thread renderThread { get; set; }
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

            VulkanSwapChainResolution = new Extent2D() { Width = 1280, Height = 720 };
            Thread.CurrentThread.Name = "LevelEditor";
        }

        private void KeyPress_Down(object sender, KeyEventArgs e)
        {
   
        }

        private void KeyPress_Up(object sender, KeyEventArgs e)
        {
        
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
                GameSystem.StartUp(this.Handle, this.pictureBox1.Handle);
            }));

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            double lastTime = 0.0;
            while (running)
            {
                double currentTime = stopwatch.Elapsed.TotalSeconds;
                double deltaTime = currentTime - lastTime;
                lastTime = currentTime;

                GameSystem.Update((float)deltaTime);
                GameSystem.Draw((float)deltaTime);
            }

            GameSystem.Destroy();
        }
    }
}
