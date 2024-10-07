using GlmSharp;
using Silk.NET.Core.Native;
using Silk.NET.Maths;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Windowing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Vulkan;
using ImageLayout = Silk.NET.Vulkan.ImageLayout;
namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MeshProperitiesStruct
    {
        uint MeshIndex = 0;
        uint MaterialIndex = 0;
        mat4 MeshTransform;

        public MeshProperitiesStruct()
        {
            MeshTransform = new mat4();
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct SceneDataBuffer
    {
        public mat4 Projection;
        public mat4 View;
        public vec3 CameraPosition;

        public SceneDataBuffer()
        {
            Projection = new mat4();
            View = new mat4();
            CameraPosition = new vec3(0.0f);
        }
    };



    public unsafe class Scene
    {
        Vk vk = Vk.GetApi();
        public Silk3DRendererPass silk3DRendererPass;
        public SilkFrameBufferRenderPass framebufferRenderPass;
        public ExtDebugUtils debugUtils;
        static readonly long startTime = DateTime.Now.Ticks;
        const int MAX_FRAMES_IN_FLIGHT = 2;
        bool isFramebufferResized = false;
        IWindow window;
        public Bitmap DisplayImage { get; protected set; }
        private readonly Object BufferLock = new Object();
        CommandPool commandPool;
        CommandBuffer commandBuffer;


          [StructLayout(LayoutKind.Sequential)]
        struct UniformBufferObject
        {
            public Matrix4X4<float> model;
            public Matrix4X4<float> view;
            public Matrix4X4<float> proj;

        }

        readonly Vertex3D[] vertices = new Vertex3D[]
        {
            new Vertex3D(new (-0.5f, -0.5f, 0.0f), new (1.0f, 0.0f, 0.0f), new (1.0f, 0.0f)),
            new Vertex3D(new (0.5f, -0.5f, 0.0f), new (0.0f, 1.0f, 0.0f), new (0.0f, 0.0f)),
            new Vertex3D(new (0.5f, 0.5f, 0.0f), new (0.0f, 0.0f, 1.0f), new (0.0f, 1.0f)),
            new Vertex3D(new (-0.5f, 0.5f, 0.0f), new (1.0f, 1.0f, 1.0f), new (1.0f, 1.0f)),

            new Vertex3D(new (-0.5f, -0.5f, -0.5f), new (1.0f, 0.0f, 0.0f), new (1.0f, 0.0f)),
            new Vertex3D(new (0.5f, -0.5f, -0.5f), new (0.0f, 1.0f, 0.0f), new (0.0f, 0.0f)),
            new Vertex3D(new (0.5f, 0.5f, -0.5f), new (0.0f, 0.0f, 1.0f), new (0.0f, 1.0f)),
            new Vertex3D(new (-0.5f, 0.5f, -0.5f), new (1.0f, 1.0f, 1.0f), new (1.0f, 1.0f))
        };

        readonly ushort[] indices = new ushort[]
        {
            0, 1, 2, 2, 3, 0,
            4, 5, 6, 6, 7, 4
        };

        public Scene()
        {

        }

        public void StartUp(IWindow windows, RichTextBox _richTextBox)
        {
            window = windows;
            InitWindow(windows);
            InitializeVulkan(_richTextBox);
            CommandBufferAllocateInfo commandBufferAllocateInfo = new CommandBufferAllocateInfo()
            {
                SType = StructureType.CommandBufferAllocateInfo,
                CommandPool = SilkVulkanRenderer.commandPool,
                Level = CommandBufferLevel.Primary,
                CommandBufferCount = 1
            };
            vk.AllocateCommandBuffers(SilkVulkanRenderer.device, in commandBufferAllocateInfo, out commandBuffer);
        }



        void InitWindow(IWindow windows)
        {
            window = windows;
            SilkVulkanRenderer.CreateWindow(windows);
        }

        void OnFramebufferResize(Vector2D<int> obj)
        {
            isFramebufferResized = true;
        }

        public void Run(IWindow windows, RichTextBox _richTextBox)
        {
            InitWindow(windows);
            InitializeVulkan(_richTextBox);
        }


        public void InitializeVulkan(RichTextBox _richTextBox)
        {
            SilkVulkanRenderer.CreateVulkanRenderer(window,_richTextBox);


            silk3DRendererPass = new Silk3DRendererPass();
            silk3DRendererPass.Create3dRenderPass();
            framebufferRenderPass = new SilkFrameBufferRenderPass();
            framebufferRenderPass.BuildRenderPass(silk3DRendererPass.texture);
        }

        public unsafe IntPtr ConvertByteArrayToVoidPointer(byte[] byteArray)
        {
            // Pin the byte array in memory to prevent the garbage collector from moving it
            fixed (byte* bytePtr = byteArray)
            {
                return (IntPtr)bytePtr; // Cast to IntPtr to return as void*
            }
        }


        public void DrawFrame()
        {
            silk3DRendererPass.UpdateUniformBuffer(startTime);

            List<CommandBuffer> commandBufferList = new List<CommandBuffer>();
            SilkVulkanRenderer.StartFrame();
            commandBufferList.Add(silk3DRendererPass.Draw());
            commandBufferList.Add(framebufferRenderPass.Draw());
            SilkVulkanRenderer.EndFrame(commandBufferList);

            //BakeColorTexture(silk3DRendererPass.renderedColorTexture, out BakeTexture bakeTexture);

            //ImageSubresource subResource = new ImageSubresource { AspectMask = ImageAspectFlags.ColorBit, MipLevel = 0, ArrayLayer = 0 };
            //SubresourceLayout subResourceLayout;
            //VKConst.vulkan.GetImageSubresourceLayout(SilkVulkanRenderer.device, bakeTexture.Image, &subResource, &subResourceLayout);

            //int pixelCount = bakeTexture.Width * bakeTexture.Height;
            //byte[] pixelData = new byte[pixelCount * (int)bakeTexture.ColorChannels];

            //IntPtr mappedMemory = IntPtr.Zero;
            //var result = VKConst.vulkan.MapMemory(SilkVulkanRenderer.device, bakeTexture.Memory, 0, Vk.WholeSize, 0, (void**)&mappedMemory);

            //if (result != Result.Success)
            //{
            //    throw new Exception($"Failed to map memory: {result}");
            //}

            //try
            //{
            //    Marshal.Copy(mappedMemory, pixelData, 0, pixelCount * (int)bakeTexture.ColorChannels);
            //}
            //catch (Exception ex)
            //{
            //    VKConst.vulkan.UnmapMemory(SilkVulkanRenderer.device, bakeTexture.Memory);
            //    throw new Exception("Error copying mapped memory: " + ex.Message);
            //}
            //VKConst.vulkan.UnmapMemory(SilkVulkanRenderer.device, bakeTexture.Memory);

            ////using (FileStream fileStream = new FileStream("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\texturerenderer.bmp", FileMode.Create))
            ////{
            ////    WriteBitmapFile(bakeTexture, pixelData, fileStream);
            ////}

            //Pixel[] pixelArray = new Pixel[bakeTexture.Width * bakeTexture.Height];

            //// Fill the array with some pixel values
            //for (int y = 0; y < bakeTexture.Height; y++)
            //{
            //    for (int x = 0; x < bakeTexture.Width; x++)
            //    {
            //        // Just as an example, we'll fill the pixels with a gradient
            //        byte r = (byte)(x * 255 / (bakeTexture.Width - 1)); // Red gradient
            //        byte g = (byte)(y * 255 / (bakeTexture.Height - 1)); // Green gradient
            //        byte b = 128; // Constant Blue
            //        byte a = 255; // Full opacity

            //        pixelArray[y * bakeTexture.Width + x] = new Pixel(r, g, b, a);
            //    }
            //}

            //fixed (Pixel* pixelPointer = pixelArray)
            //{
            //    //void* voidPointer = pixelPointer;
            //    //StbImageWriteSharp.ImageWriter asd = new StbImageWriteSharp.ImageWriter();
            //    //using (FileStream fileStream = new FileStream("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\texturerender234.bmp", FileMode.Create, FileAccess.Write))
            //    //{
            //    //    asd.WriteBmp(voidPointer, bakeTexture.Width, bakeTexture.Height, StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, fileStream);
            //    //}
            //    var ds = ConvertPixelsToByteArray(pixelArray);
            //    UpdateBuffer(ds);
            //}
        }

        public CommandBuffer BakeColorTexture(RenderedTexture texture, out BakeTexture bakeTexture)
        {
            var pixel = new Pixel(0xFF, 0x00, 0x00, 0xFF);
            bakeTexture = new BakeTexture(pixel, new GlmSharp.ivec2(1280, 720), Format.R8G8B8A8Unorm);

            var commandInfo = new CommandBufferBeginInfo(flags: 0);
            var commandBuffer = SilkVulkanRenderer.BeginSingleUseCommandBuffer();

            // Explicitly set the image layouts before copying
            bakeTexture.UpdateImageLayout(commandBuffer, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal);
            texture.UpdateImageLayout(commandBuffer, Silk.NET.Vulkan.ImageLayout.TransferSrcOptimal);

            ImageCopy copyImage = new ImageCopy
            {
                SrcSubresource = { AspectMask = ImageAspectFlags.ColorBit, MipLevel = 0, BaseArrayLayer = 0, LayerCount = 1 },
                DstSubresource = { AspectMask = ImageAspectFlags.ColorBit, MipLevel = 0, BaseArrayLayer = 0, LayerCount = 1 },
                DstOffset = new Offset3D { X = 0, Y = 0, Z = 0 },
                Extent = new Extent3D { Width = (uint)texture.Width, Height = (uint)texture.Height, Depth = 1 }
            };
            VKConst.vulkan.CmdCopyImage(commandBuffer, texture.Image, Silk.NET.Vulkan.ImageLayout.TransferSrcOptimal, bakeTexture.Image, Silk.NET.Vulkan.ImageLayout.TransferDstOptimal, 1, &copyImage);

            bakeTexture.UpdateImageLayout(commandBuffer, Silk.NET.Vulkan.ImageLayout.General);
            texture.UpdateImageLayout(commandBuffer, Silk.NET.Vulkan.ImageLayout.PresentSrcKhr);

            SilkVulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);

            return commandBuffer;
        }

        private void WriteBitmapFile(BakeTexture bakeTexture, byte[] pixelData, FileStream fileStream)
        {
            // Ensure the pixel data is in RGB format:
            int bitmapSize = bakeTexture.Width * bakeTexture.Height;
            byte[] bmpData = new byte[bitmapSize * 3]; // For RGB

            // Convert from RGBA to RGB
            for (int i = 0; i < bitmapSize; i++)
            {
                bmpData[i * 3] = pixelData[i * 4];     // R
                bmpData[i * 3 + 1] = pixelData[i * 4 + 1]; // G
                bmpData[i * 3 + 2] = pixelData[i * 4 + 2]; // B
            }

            // Write BMP using StbImageWrite
            //StbImageWriteSharp.ImageWriter iw = new StbImageWriteSharp.ImageWriter();
            //fixed (byte* bmpDataPtr = bmpData) // Pin the data in memory
            //{
            //    iw.WriteBmp((void*)bmpDataPtr, bakeTexture.Width, bakeTexture.Height, StbImageWriteSharp.ColorComponents.RedGreenBlue, fileStream);
            //}
        }

        public void UpdateBuffer(byte[] pixelData)
        {
            if (pixelData == null)
            {
                return;
            }

            Bitmap currentBitmap = new Bitmap((int)SilkVulkanRenderer.swapChain.swapchainExtent.Width, (int)SilkVulkanRenderer.swapChain.swapchainExtent.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (int y = 0; y < currentBitmap.Height; y++)
            {
                for (int x = 0; x < currentBitmap.Width; x++)
                {
                    int index = (y * currentBitmap.Width + x) * 4;
                    System.Drawing.Color pixelColor = ByteArrayToColor(pixelData, index);
                    currentBitmap.SetPixel(x, y, pixelColor);
                }
            }

            currentBitmap.Save("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\texturerenderer.bmp", System.Drawing.Imaging.ImageFormat.Png);
            lock (BufferLock)
            {
                DisplayImage = currentBitmap;
            }
        }

        public void PresentImage(PictureBox picture)
        {
            if (DisplayImage == null)
            {
                return;
            }

            if (picture.Image != null)
            {
                picture.Image.Dispose();
            }

            picture.Image = (Bitmap)DisplayImage.Clone();
            picture.Refresh();
        }

        [DllImport("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DLL_stbi_write_bmp(string filename, int w, int h, int comp, void* data);
        public static void WriteImage(string filePath, byte[] imageData, int width, int height, int channels)
        {
            int result = DLL_stbi_write_bmp(filePath, width, height, channels, (void*)imageData.ToArray()[0]);
            if (result == 0)
            {
                Console.WriteLine("Failed to write image.");
            }
            else
            {
                Console.WriteLine("Image written successfully.");
            }
        }

        private System.Drawing.Color ByteArrayToColor(byte[] data, int index)
        {
            byte a = data[index + 3];
            byte r = data[index + 0];
            byte g = data[index + 1];
            byte b = data[index + 2];
            return System.Drawing.Color.FromArgb(a, r, g, b);
        }

        public  byte[] ConvertPixelsToByteArray(Pixel[] pixels)
        {
            // Each pixel has 4 components (RGBA)
            int byteCount = pixels.Length * 4; // 4 bytes per pixel
            byte[] byteArray = new byte[byteCount];

            // Populate the byte array
            for (int i = 0; i < pixels.Length; i++)
            {
                byteArray[i * 4] = pixels[i].Red;     // R
                byteArray[i * 4 + 1] = pixels[i].Green; // G
                byteArray[i * 4 + 2] = pixels[i].Blue; // B
                byteArray[i * 4 + 3] = pixels[i].Alpha; // A
            }

            return byteArray;
        }
    }
}

