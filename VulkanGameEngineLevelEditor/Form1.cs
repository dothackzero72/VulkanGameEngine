using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.RenderPassWindows;
using VulkanGameEngineLevelEditor.Systems;

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
                RenderSystem.CreateVulkanRenderer(this.Handle, pictureBox1.Handle);
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