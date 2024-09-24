using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor
{
    public unsafe partial class Form1 : Form
    {
        private Thread renderThread;
        private volatile bool running;
        private BlockingCollection<byte[]> dataCollection = new BlockingCollection<byte[]>(boundedCapacity: 10);
        private System.Windows.Forms.Timer renderTimer;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load; // Register the Load event
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeRenderTimer(); // Initialize the timer after the form is loaded
            StartRenderer(); // Start the rendering thread
        }

        private void InitializeRenderTimer()
        {
            renderTimer = new System.Windows.Forms.Timer
            {
                Interval = 100 // Set timer interval for updates
            };
            renderTimer.Tick += UpdateBitmap;
            renderTimer.Start();
        }

        public void StartRenderer()
        {
            running = true;
            renderThread = new Thread(RenderLoop)
            {
                IsBackground = true // Ensures the thread won't prevent application exit
            };
            renderThread.Start();
        }

        private void RenderLoop()
        {
            Scene scene = new Scene();
            this.Invoke(new Action(() =>
            {
                VulkanRenderer.SetUpRenderer(this.Handle, pictureBox1);
                scene.StartUp();
            }));

            while (running)
            {
                scene.Update(0);
                byte[] textureData = GenerateColorByteData(pictureBox1.Width, pictureBox1.Height);
                dataCollection.TryAdd(textureData);
                scene.Draw();
            }
        }

        private void UpdateBitmap(object sender, EventArgs e)
        {
            byte[] textureData;
            // Try to take texture data from the collection safely
            if (dataCollection.TryTake(out textureData))
            {
                // Use Invoke to ensure we update the PictureBox on the UI thread
                if (pictureBox1.InvokeRequired)
                {
                    pictureBox1.Invoke(new Action(() => UpdateBitmapWithData(textureData)));
                }
                else
                {
                    UpdateBitmapWithData(textureData);
                }
            }
        }

        private void UpdateBitmapWithData(byte[] textureData)
        {

            using (Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb))
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        int index = (y * bitmap.Width + x) * 4; // Calculate index in the byte array
                        Color pixelColor = ByteArrayToColor(textureData, index);
                        bitmap.SetPixel(x, y, pixelColor); // Set the pixel color
                    }
                }

                // Dispose of the old image if it exists
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                }

                // Update the PictureBox image
                pictureBox1.Image = (Bitmap)bitmap.Clone();
                pictureBox1.Refresh();
            }

        }

        private byte[] GenerateColorByteData(int width, int height)
        {
            byte[] data = new byte[width * height * 4];
            Random rnd = new Random();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color randomColor = Color.FromArgb(255, rnd.Next(256), rnd.Next(256), rnd.Next(256));
                    int index = (y * width + x) * 4;
                    data[index + 0] = randomColor.A;
                    data[index + 1] = randomColor.R;
                    data[index + 2] = randomColor.G;
                    data[index + 3] = randomColor.B;
                }
            }

            return data;
        }

        private Color ByteArrayToColor(byte[] data, int index)
        {
            byte a = data[index + 0];
            byte r = data[index + 1];
            byte g = data[index + 2];
            byte b = data[index + 3];
            return Color.FromArgb(a, r, g, b);
        }

        public void StopRenderer()
        {
            running = false;
            if (renderThread != null && renderThread.IsAlive)
            {
                renderThread.Join();
            }
        }
    }
}