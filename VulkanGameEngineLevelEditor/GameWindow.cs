using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Silk.NET.Vulkan;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor
{
    public partial class GameWindow : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        private Vk vk = Vk.GetApi();
        private static Scene scene;
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
            _serviceProvider = ServiceConfig.ConfigureServices();
            _mapper = _serviceProvider.GetRequiredService<IMapper>();

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
                GameSystem.StartUp(this.Handle, this.pictureBox1.Handle);
            }));

            scene = new Scene();
            stopwatch.Start();
            scene.StartUp();
            while (running)
            {
                float deltaTime = (float)stopwatch.Elapsed.TotalSeconds;
                stopwatch.Restart();
                scene.DrawFrame();
            }

            scene.Destroy();
        }

    }
}
