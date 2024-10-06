using GlmSharp;
using Silk.NET.Core.Native;
using Silk.NET.Maths;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Tests;
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
        Silk3DRendererPass silk3DRendererPass;

        public ExtDebugUtils debugUtils;
        static readonly long startTime = DateTime.Now.Ticks;
        const int MAX_FRAMES_IN_FLIGHT = 2;
        bool isFramebufferResized = false;
        IWindow window;

        CommandPool commandPool;
        PipelineLayout pipelineLayout;

        DescriptorSet descriptorSets;
      


        [StructLayout(LayoutKind.Sequential)]
        struct UniformBufferObject
        {
            public Matrix4X4<float> model;
            public Matrix4X4<float> view;
            public Matrix4X4<float> proj;

        }

        readonly Vertex[] vertices = new Vertex[]
        {
            new Vertex(new (-0.5f, -0.5f, 0.0f), new (1.0f, 0.0f, 0.0f), new (1.0f, 0.0f)),
            new Vertex(new (0.5f, -0.5f, 0.0f), new (0.0f, 1.0f, 0.0f), new (0.0f, 0.0f)),
            new Vertex(new (0.5f, 0.5f, 0.0f), new (0.0f, 0.0f, 1.0f), new (0.0f, 1.0f)),
            new Vertex(new (-0.5f, 0.5f, 0.0f), new (1.0f, 1.0f, 1.0f), new (1.0f, 1.0f)),

            new Vertex(new (-0.5f, -0.5f, -0.5f), new (1.0f, 0.0f, 0.0f), new (1.0f, 0.0f)),
            new Vertex(new (0.5f, -0.5f, -0.5f), new (0.0f, 1.0f, 0.0f), new (0.0f, 0.0f)),
            new Vertex(new (0.5f, 0.5f, -0.5f), new (0.0f, 0.0f, 1.0f), new (0.0f, 1.0f)),
            new Vertex(new (-0.5f, 0.5f, -0.5f), new (1.0f, 1.0f, 1.0f), new (1.0f, 1.0f))
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
            SilkVulkanRenderer.CreateInstance(_richTextBox);
            SilkVulkanRenderer.CreateSurface(window);
            SilkVulkanRenderer.CreatePhysicalDevice(SilkVulkanRenderer.khrSurface);
            SilkVulkanRenderer.CreateDevice();
            SilkVulkanRenderer.CreateDeviceQueue();
            SilkVulkanRenderer.swapChain.CreateSwapChain(window, SilkVulkanRenderer.khrSurface, SilkVulkanRenderer.surface);
            commandPool = SilkVulkanRenderer.CreateCommandPool();
            SilkVulkanRenderer.CreateSemaphores();

            silk3DRendererPass = new Silk3DRendererPass();
            silk3DRendererPass.Create3dRenderPass();

          
            descriptorSets = silk3DRendererPass.allocateDescriptorSets(silk3DRendererPass.descriptorpool);
            silk3DRendererPass.UpdateDescriptorSet(descriptorSets);

           
        }

        public CommandBuffer Draw()
        {
            var commandIndex = SilkVulkanRenderer.CommandIndex;
            var imageIndex = SilkVulkanRenderer.ImageIndex;
            var commandBuffer = silk3DRendererPass.commandBufferList[commandIndex];
            List<CommandBuffer> commandBufferList = new List<CommandBuffer>();
            ClearValue* clearValues = stackalloc[]
{
                new ClearValue(new ClearColorValue(1, 0, 0, 1)),
                new ClearValue(null, new ClearDepthStencilValue(1.0f, 0))
            };

            RenderPassBeginInfo renderPassInfo = new
            (
                renderPass: silk3DRendererPass.renderPass,
                framebuffer: silk3DRendererPass.FrameBufferList[imageIndex],
                clearValueCount: 2,
                pClearValues: clearValues,
                renderArea: new(new Offset2D(0, 0), SilkVulkanRenderer.swapChain.swapchainExtent)
            );

            var viewport = new Viewport(0.0f, 0.0f, SilkVulkanRenderer.swapChain.swapchainExtent.Width, SilkVulkanRenderer.swapChain.swapchainExtent.Height, 0.0f, 1.0f);
            var scissor = new Rect2D(new Offset2D(0, 0), SilkVulkanRenderer.swapChain.swapchainExtent);

            var descSet = descriptorSets;
            var vertexbuffer = silk3DRendererPass.vertexBuffer.Buffer;
            var commandInfo = new CommandBufferBeginInfo(flags: 0);


            VKConst.vulkan.BeginCommandBuffer(commandBuffer, &commandInfo);
            VKConst.vulkan.CmdBeginRenderPass(commandBuffer, &renderPassInfo, SubpassContents.Inline);
            VKConst.vulkan.CmdBindPipeline(commandBuffer, PipelineBindPoint.Graphics, silk3DRendererPass.shaderpipeline);
            VKConst.vulkan.CmdBindVertexBuffers(commandBuffer, 0, 1, &vertexbuffer, 0);
            VKConst.vulkan.CmdBindIndexBuffer(commandBuffer, silk3DRendererPass.indexBuffer.Buffer, 0, IndexType.Uint16);
            VKConst.vulkan.CmdBindDescriptorSets(commandBuffer, PipelineBindPoint.Graphics, silk3DRendererPass.shaderpipelineLayout, 0, 1, descSet, 0, null);
            VKConst.vulkan.CmdSetViewport(commandBuffer, 0, 1, &viewport);
            VKConst.vulkan.CmdSetScissor(commandBuffer, 0, 1, &scissor);
            VKConst.vulkan.CmdDrawIndexed(commandBuffer, (uint)indices.Length, 1, 0, 0, 0);
            VKConst.vulkan.CmdEndRenderPass(commandBuffer);
            VKConst.vulkan.EndCommandBuffer(commandBuffer);

            return commandBuffer;
        }
        public void DrawFrame()
        {
            silk3DRendererPass.UpdateUniformBuffer(startTime);

            List<CommandBuffer> commandBufferList = new List<CommandBuffer>();
            SilkVulkanRenderer.StartFrame();
            commandBufferList.Add(Draw());
            SilkVulkanRenderer.EndFrame(commandBufferList);
        }

    

    }
}

