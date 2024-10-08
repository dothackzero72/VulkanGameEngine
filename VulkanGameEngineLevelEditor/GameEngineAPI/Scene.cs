using GlmSharp;
using Silk.NET.Core.Native;
using Silk.NET.Maths;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
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
using PixelFormat = System.Drawing.Imaging.PixelFormat;
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
        public SilkFrameBufferRenderPass frameBuffer;
        public Silk3DRendererPass silk3DRendererPass;
        public Texture texture;
        public ExtDebugUtils debugUtils;
        static readonly long startTime = DateTime.Now.Ticks;
        const int MAX_FRAMES_IN_FLIGHT = 2;
        bool isFramebufferResized = false;
        IWindow window;

        CommandPool commandPool;
      


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


            texture = new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\awesomeface.png", Format.R8G8B8A8Unorm, TextureTypeEnum.kType_DiffuseTextureMap);

            silk3DRendererPass = new Silk3DRendererPass();
            silk3DRendererPass.Create3dRenderPass();

            frameBuffer = new SilkFrameBufferRenderPass(SilkVulkanRenderer.device, silk3DRendererPass.renderedColorTexture);
        }

        public void DrawFrame()
        {
            silk3DRendererPass.UpdateUniformBuffer(startTime);

            List<CommandBuffer> commandBufferList = new List<CommandBuffer>();
            SilkVulkanRenderer.StartFrame();
            commandBufferList.Add(silk3DRendererPass.Draw());
            commandBufferList.Add(frameBuffer.Draw());
            SilkVulkanRenderer.EndFrame(commandBufferList);
        }
    }
}

