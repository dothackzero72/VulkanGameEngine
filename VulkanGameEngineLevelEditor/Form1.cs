using StbImageSharp;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeRenderTimer();
            StartRenderer();
        }

        private void InitializeRenderTimer()
        {
            renderTimer = new System.Windows.Forms.Timer
            {
                Interval = 100
            };
            renderTimer.Tick += UpdateBitmap;
            renderTimer.Start();
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
            Scene scene = new Scene();
            this.Invoke(new Action(() =>
            {
                VulkanRenderer.SetUpRenderer(this.Handle, pictureBox1);
                scene.StartUp();
            }));

            while (running)
            {
                scene.Update(0);
                byte[] textureData = Texture.UpdateBitmapData(scene.texture);
                dataCollection.TryAdd(textureData);
              //  scene.Draw();
                Thread.Sleep(16);
            }
        }

     

        //public static byte[] DisplayImageFromStb(string filePath)
        //{
        //    // Step 1: Load Image Data with StbImageSharp
        //    using (var stream = File.OpenRead(filePath))
        //    {
        //        ImageResult image = ImageResult.FromStream(stream);
        //        int width = image.Width;
        //        int height = image.Height;
        //        ColorComponents channels = image.Comp;

        //        // Prepare the output byte array to hold ARGB format
        //        byte[] imageData = new byte[width * height * 4]; // 4 bytes for ARGB per pixel

        //        // Convert from RGB(A) to ARGB
        //        for (int y = 0; y < height; y++)
        //        {
        //            for (int x = 0; x < width; x++)
        //            {
        //                int srcIndex = (y * width + x) * (int)channels; // source index in imageData
        //                int destIndex = (y * width + x) * 4; // destination index in ARGB imageData

        //                // Handle different channel combinations
        //                //if (channels == ColorComponents.RedGreenBlue)
        //                //{
        //                    // Assuming no alpha, set Alpha to 255.
        //                    imageData[destIndex + 0] = 255; // A
        //                    imageData[destIndex + 1] = image.Data[srcIndex + 0]; // R
        //                    imageData[destIndex + 2] = image.Data[srcIndex + 1]; // G
        //                    imageData[destIndex + 3] = image.Data[srcIndex + 2]; // B
        //                //}
        //                //else if (channels == ColorComponents.RedGreenBlueAlpha)
        //                //{
        //                //    imageData[destIndex + 0] = image.Data[srcIndex + 3]; // A
        //                //    imageData[destIndex + 1] = image.Data[srcIndex + 0]; // R
        //                //    imageData[destIndex + 2] = image.Data[srcIndex + 1]; // G
        //                //    imageData[destIndex + 3] = image.Data[srcIndex + 2]; // B
        //                //}
        //                // You can add more cases for grayscale or other formats if needed
        //            }
        //        }

        //        return imageData;
        //    }
        //}

        private void UpdateBitmap(object sender, EventArgs e)
        {
            

            byte[] textureData;
            if (dataCollection.TryTake(out textureData))
            {
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
            if(textureData == null)
            {
                return;
            }

            using (Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb))
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        int index = (y * bitmap.Width + x) * 4;
                        Color pixelColor = ByteArrayToColor(textureData, index);
                        bitmap.SetPixel(x, y, pixelColor);
                    }
                }

                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                }

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