using Newtonsoft.Json;
using Silk.NET.Vulkan;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngine.Systems;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.RenderPassWindows;

namespace VulkanGameEngineLevelEditor
{
    public unsafe partial class LevelEditorForm : Form
    {
        private volatile bool running;
        private volatile bool levelEditorRunning;
        private Stopwatch stopwatch = new Stopwatch();
        private RichTextBoxWriter textBoxWriter;
        private Thread renderThread { get; set; }
        public RenderPassBuildInfoModel renderPass { get; private set; } = new RenderPassBuildInfoModel();
        private MessengerModel _messenger;
        private GCHandle _callbackHandle;

        [DllImport("kernel32.dll")] static extern bool AllocConsole();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void LogVulkanMessageDelegate(string message, int severity);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.Cdecl)] public static extern void SetRichTextBoxHandle(IntPtr hwnd);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.Cdecl)] public static extern void SetLogVulkanMessageCallback(LogVulkanMessageDelegate callback);

        public LevelEditorForm()
        {
            InitializeComponent();

            this.Load += Form1_Load;

            Thread.CurrentThread.Name = "LevelEditor";

            textBoxWriter = new RichTextBoxWriter(richTextBox2);

            //RenderPassMessager = new MessengerModel()
            //{
            //    IsActive = true,
            //    richTextBox = richTextBox2,
            //    TextBoxName = richTextBox2.Name,
            //    ThreadId = Thread.CurrentThread.ManagedThreadId,
            //};
            _messenger = new MessengerModel
            {
                richTextBox = richTextBox2,
                TextBoxName = richTextBox2.Name,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                IsActive = true
            };

            GlobalMessenger.AddMessenger(_messenger);

            LogVulkanMessageDelegate callback = LogVulkanMessage;
            _callbackHandle = GCHandle.Alloc(callback); 
            SetLogVulkanMessageCallback(callback);

            // Buttons
            var saveButton = new System.Windows.Forms.Button { Text = "Save", Location = new Point(13, 10), Size = new Size(80, 30) };
            var undoButton = new System.Windows.Forms.Button { Text = "Undo", Location = new Point(100, 10), Size = new Size(80, 30) };
            var redoButton = new System.Windows.Forms.Button { Text = "Redo", Location = new Point(190, 10), Size = new Size(80, 30) };
            var addPipelineButton = new System.Windows.Forms.Button { Text = "Add Pipeline", Location = new Point(290, 10), Size = new Size(80, 30) };
            saveButton.Click += (s, e) => objectDataGridView1.SaveToJson();
            undoButton.Click += (s, e) => objectDataGridView1.Undo();
            redoButton.Click += (s, e) => objectDataGridView1.Redo();
            addPipelineButton.Click += (s, e) => levelEditorTreeView1.AddRenderPipeline();

            var renderPassBuildInfo = new RenderPassBuildInfoModel();
            levelEditorTreeView1.RootObject = renderPassBuildInfo;
            levelEditorTreeView1.SelectionChanged += obj =>
            {
                if (obj != null)
                {
                    objectDataGridView1.SelectedObject = obj;
                }
            };

            // Add controls to form
            Controls.AddRange(new Control[] { levelEditorTreeView1, objectDataGridView1, saveButton, undoButton, redoButton, addPipelineButton });

            // Load data (example)
            try
            {
                objectDataGridView1.LoadFromJson<RenderPassBuildInfoModel>("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\RenderPass\\LevelShader2DRenderPass.json");
                levelEditorTreeView1.RootObject = objectDataGridView1.SelectedObject; // Sync initial state
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load JSON: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Resize event to handle form resizing
            this.Resize += (s, e) =>
            {
                levelEditorTreeView1.Size = new Size(270, this.ClientSize.Height - 51);
                objectDataGridView1.Size = new Size(this.ClientSize.Width - 303, this.ClientSize.Height - 51);
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartRenderer();
        }
        public static void LogVulkanMessage(string message, int severity)
        {
            GlobalMessenger.LogMessage(message, (DebugUtilsMessageSeverityFlagsEXT)severity);
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
            this.Invoke(new Action(() =>
            {
                GameSystem.StartUp(this.pictureBox1.Handle.ToPointer(), this.richTextBox2.Handle.ToPointer());
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //using (RendererEditorForm renderPassBuilder = new RendererEditorForm())
            //{
            //    RenderPassMessager.IsActive = false;
            //    if (renderPassBuilder.ShowDialog() == DialogResult.OK)
            //    {
            //        RenderPassMessager.IsActive = false;
            //    }
            //}
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