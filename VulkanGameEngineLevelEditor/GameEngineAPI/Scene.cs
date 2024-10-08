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
        static readonly long startTime = DateTime.Now.Ticks;
        bool isFramebufferResized = false;
        IWindow window;
        CommandPool commandPool;
        CommandBuffer commandBuffer;
        public Texture texture;

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
            texture = new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\awesomeface.png", Format.R8G8B8A8Unorm, TextureTypeEnum.kType_DiffuseTextureMap);
        }

        public void StartUp(IWindow windows)
        {
            window = windows;
            InitWindow(windows);
            InitializeVulkan();
            CommandBufferAllocateInfo commandBufferAllocateInfo = new CommandBufferAllocateInfo()
            {
                SType = StructureType.CommandBufferAllocateInfo,
                CommandPool = SilkVulkanRenderer.commandPool,
                Level = CommandBufferLevel.Primary,
                CommandBufferCount = 1
            };
            vk.AllocateCommandBuffers(SilkVulkanRenderer.device, in commandBufferAllocateInfo, out commandBuffer);
            texture = new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\awesomeface.png", Format.R8G8B8A8Unorm, TextureTypeEnum.kType_DiffuseTextureMap);
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

        public void InitializeVulkan(RichTextBox _richTextBox)
        {
            SilkVulkanRenderer.CreateVulkanRenderer(window,_richTextBox);

            silk3DRendererPass = new Silk3DRendererPass();
            silk3DRendererPass.Create3dRenderPass();
        }
        public void InitializeVulkan()
        {
            SilkVulkanRenderer.CreateVulkanRenderer(window);

            silk3DRendererPass = new Silk3DRendererPass();
            silk3DRendererPass.Create3dRenderPass();
        }
        public void DrawFrame()
        {
            silk3DRendererPass.UpdateUniformBuffer(startTime);

            List<CommandBuffer> commandBufferList = new List<CommandBuffer>();
            SilkVulkanRenderer.StartFrame();
            commandBufferList.Add(silk3DRendererPass.Draw());
            SilkVulkanRenderer.EndFrame(commandBufferList);
        }

    }
}

