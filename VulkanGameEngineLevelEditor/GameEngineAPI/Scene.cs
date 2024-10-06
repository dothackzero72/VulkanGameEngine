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

    [StructLayout(LayoutKind.Sequential)]
    struct UniformBufferObject
    {
        public Matrix4X4<float> model;
        public Matrix4X4<float> view;
        public Matrix4X4<float> proj;

    }

    public unsafe class Scene
    {
        Vk vk = Vk.GetApi();
        Silk3DRendererPass silk3DRendererPass;

        public ExtDebugUtils debugUtils;
        static readonly long startTime = DateTime.Now.Ticks;
        const int MAX_FRAMES_IN_FLIGHT = 2;
        bool isFramebufferResized = false;
        IWindow window;

        Framebuffer[] swapChainFramebuffers;

        CommandPool commandPool;
        CommandBuffer[] commandBuffers;
        DescriptorSetLayout descriptorSetLayout;
        PipelineLayout pipelineLayout;
        Pipeline graphicsPipeline;

        UniformBufferObject ubo;
        VulkanBuffer<Vertex> vertexBuffer;
        VulkanBuffer<ushort> indexBuffer;
        VulkanBuffer<UniformBufferObject> uniformBuffers;

        public GameEngineAPI.Texture texture;

        DepthTexture depthTexture;

        DescriptorPool descriptorPool;
        DescriptorSet descriptorSets;
      
        Silk.NET.Vulkan.Buffer ubobuffers;
        VkDeviceMemory bufferMemory;
        ulong ubosize;
        BufferUsageFlags ubousage;
        MemoryPropertyFlags ubopropertyFlags;
        bool ubodisposedValue;

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

            descriptorSetLayout = silk3DRendererPass.CreateDescriptorSetLayout();
            CreateGraphicsPipeline();
            depthTexture = new DepthTexture(new ivec2((int)SilkVulkanRenderer.swapChain.swapchainExtent.Width, (int)SilkVulkanRenderer.swapChain.swapchainExtent.Height));
            CreateFramebuffers();
            texture = new GameEngineAPI.Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\VulkanGameEngineLevelEditor\\bin\\Debug\\awesomeface.png", Format.R8G8B8A8Srgb, TextureTypeEnum.kType_DiffuseTextureMap);


            GCHandle vhandle = GCHandle.Alloc(vertices, GCHandleType.Pinned);
            IntPtr vpointer = vhandle.AddrOfPinnedObject();
            vertexBuffer = new VulkanBuffer<Vertex>((void*)vpointer, (uint)vertices.Count(), BufferUsageFlags.BufferUsageTransferSrcBit | BufferUsageFlags.BufferUsageTransferDstBit, MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, true);

            GCHandle fhandle = GCHandle.Alloc(indices, GCHandleType.Pinned);
            IntPtr fpointer = fhandle.AddrOfPinnedObject();
            indexBuffer = new VulkanBuffer<ushort>((void*)fpointer, (uint)indices.Count(), BufferUsageFlags.BufferUsageTransferSrcBit | BufferUsageFlags.BufferUsageTransferDstBit, MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, true);

            CreateUniformBuffers();
            descriptorPool = silk3DRendererPass.CreateDescriptorPoolBinding();
            descriptorSets = silk3DRendererPass.allocateDescriptorSets(descriptorPool, descriptorSetLayout);
            silk3DRendererPass.UpdateDescriptorSet(descriptorSets, texture, ubobuffers);

            commandBuffers = new CommandBuffer[MAX_FRAMES_IN_FLIGHT];
            SilkVulkanRenderer.CreateCommandBuffers(commandBuffers);
           
        }


        void CreateGraphicsPipeline()
        {
            PipelineShaderStageCreateInfo* shadermoduleList = stackalloc[]
            {
                SilkVulkanRenderer.CreateShader("vertshader.spv",  ShaderStageFlags.VertexBit),
                SilkVulkanRenderer.CreateShader("fragshader.spv", ShaderStageFlags.FragmentBit)
            };
            pipelineLayout = silk3DRendererPass.CreatePipelineLayout(descriptorSetLayout);

            PipelineVertexInputStateCreateInfo pipelineVertexInputStateCreateInfo = new(flags: 0);
            VertexInputBindingDescription vertexInputBindingDescription = new(0, Vertex.GetStride());

            VertexInputAttributeDescription* vertexInputAttributeDescriptions = null;

            if (Vertex.GetStride() > 0)
            {
                vertexInputAttributeDescriptions = (VertexInputAttributeDescription*)Mem.AllocArray<VertexInputAttributeDescription>(Vertex.GetAttributeFormatsAndOffsets().Length);
                for (uint i = 0; i < Vertex.GetAttributeFormatsAndOffsets().Length; i++)
                {
                    vertexInputAttributeDescriptions[i] = new
                    (
                        location: i,
                        binding: 0,
                        format: Vertex.GetAttributeFormatsAndOffsets()[i].Item1,
                        offset: Vertex.GetAttributeFormatsAndOffsets()[i].Item2
                    );
                }

                pipelineVertexInputStateCreateInfo.VertexBindingDescriptionCount = 1;
                pipelineVertexInputStateCreateInfo.PVertexBindingDescriptions = &vertexInputBindingDescription;

                pipelineVertexInputStateCreateInfo.VertexAttributeDescriptionCount = (uint)Vertex.GetAttributeFormatsAndOffsets().Length;
                pipelineVertexInputStateCreateInfo.PVertexAttributeDescriptions = vertexInputAttributeDescriptions;
            }

            PipelineInputAssemblyStateCreateInfo pipelineInputAssemblyStateCreateInfo = new(topology: PrimitiveTopology.TriangleList);

            PipelineViewportStateCreateInfo pipelineViewportStateCreateInfo = new
            (
                viewportCount: 1,
                pViewports: null,
                scissorCount: 1,
                pScissors: null
            );

            PipelineRasterizationStateCreateInfo pipelineRasterizationStateCreateInfo = new
            (
                depthClampEnable: false,
                rasterizerDiscardEnable: false,
                polygonMode: PolygonMode.Fill,
                cullMode: CullModeFlags.CullModeBackBit,
                frontFace: FrontFace.CounterClockwise,
                depthBiasEnable: false,
                depthBiasConstantFactor: 0.0f,
                depthBiasClamp: 0.0f,
                depthBiasSlopeFactor: 0.0f,
                lineWidth: 1.0f
            );

            PipelineMultisampleStateCreateInfo pipelineMultisampleStateCreateInfo = new(rasterizationSamples: SampleCountFlags.SampleCount1Bit);

            StencilOpState stencilOpState = new(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep, CompareOp.Always);

            PipelineDepthStencilStateCreateInfo pipelineDepthStencilStateCreateInfo = new
            (
                depthTestEnable: true,
                depthWriteEnable: true,
                depthCompareOp: CompareOp.LessOrEqual,
                depthBoundsTestEnable: false,
                stencilTestEnable: false,
                front: stencilOpState,
                back: stencilOpState
            );

            ColorComponentFlags colorComponentFlags =
                ColorComponentFlags.ColorComponentRBit |
                ColorComponentFlags.ColorComponentGBit |
                ColorComponentFlags.ColorComponentBBit |
                ColorComponentFlags.ColorComponentABit;

            PipelineColorBlendAttachmentState pipelineColorBlendAttachmentState = new(
                false,
                Silk.NET.Vulkan.BlendFactor.Zero,
                Silk.NET.Vulkan.BlendFactor.Zero,
                BlendOp.Add,
                Silk.NET.Vulkan.BlendFactor.Zero,
                Silk.NET.Vulkan.BlendFactor.Zero,
                BlendOp.Add,
                colorComponentFlags);

            PipelineColorBlendStateCreateInfo pipelineColorBlendStateCreateInfo = new
            (
                logicOpEnable: false,
                logicOp: LogicOp.NoOp,
                attachmentCount: 1,
                pAttachments: &pipelineColorBlendAttachmentState
            );

            pipelineColorBlendStateCreateInfo.BlendConstants[0] = 0.0f;
            pipelineColorBlendStateCreateInfo.BlendConstants[1] = 0.0f;
            pipelineColorBlendStateCreateInfo.BlendConstants[2] = 0.0f;
            pipelineColorBlendStateCreateInfo.BlendConstants[3] = 0.0f;

            DynamicState* dynamicStates = stackalloc[] { DynamicState.Viewport, DynamicState.Scissor };

            PipelineDynamicStateCreateInfo pipelineDynamicStateCreateInfo = new
            (
                dynamicStateCount: 2,
                pDynamicStates: dynamicStates
            );

            GraphicsPipelineCreateInfo graphicsPipelineCreateInfo = new
            (
                stageCount: 2,
                pStages: shadermoduleList,
                pVertexInputState: &pipelineVertexInputStateCreateInfo,
                pInputAssemblyState: &pipelineInputAssemblyStateCreateInfo,
                pViewportState: &pipelineViewportStateCreateInfo,
                pRasterizationState: &pipelineRasterizationStateCreateInfo,
                pMultisampleState: &pipelineMultisampleStateCreateInfo,
                pDepthStencilState: &pipelineDepthStencilStateCreateInfo,
                pColorBlendState: &pipelineColorBlendStateCreateInfo,
                pDynamicState: &pipelineDynamicStateCreateInfo,
                layout: pipelineLayout,
                renderPass: silk3DRendererPass.renderPass
            );

            vk.CreateGraphicsPipelines(SilkVulkanRenderer.device, new PipelineCache(null), 1, &graphicsPipelineCreateInfo, null, out Pipeline pipeline);
            graphicsPipeline = pipeline;

            //SilkMarshal.Free((nint)pipelineShaderStageCreateInfos[0].PName);
            //SilkMarshal.Free((nint)pipelineShaderStageCreateInfos[1].PName);

            Mem.FreeArray(vertexInputAttributeDescriptions);

            //vertShaderModule.Dispose();
            // fragShaderModule.Dispose();
        }

        void CreateFramebuffers()
        {
            swapChainFramebuffers = silk3DRendererPass.CreateFramebuffer(depthTexture);
            swapChainFramebuffers = SU.MakeFramebuffers(SilkVulkanRenderer.device, silk3DRendererPass.renderPass, SilkVulkanRenderer.swapChain.imageViews, depthTexture.View,
                SilkVulkanRenderer.swapChain.swapchainExtent);
        }

        void CreateUniformBuffers()
        {
            uint bufferSize = (uint)sizeof(UniformBufferObject);

            ubosize = (uint)sizeof(UniformBufferObject);
            ubousage = BufferUsageFlags.BufferUsageUniformBufferBit;
            ubopropertyFlags = MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit;


            var bufferInfo = new BufferCreateInfo(size: ubosize, usage: ubousage);
            vk.CreateBuffer(SilkVulkanRenderer.device, in bufferInfo, null, out Silk.NET.Vulkan.Buffer ubobufferss);
            ubobuffers = ubobufferss;

            vk.GetBufferMemoryRequirements(SilkVulkanRenderer.device, ubobuffers, out MemoryRequirements memoryRequirements);
            uint memoryTypeIndex = SilkVulkanRenderer.GetMemoryType(memoryRequirements.MemoryTypeBits, ubopropertyFlags);

            MemoryAllocateInfo allocInfo = new
            (
                allocationSize: memoryRequirements.Size,
                memoryTypeIndex: memoryTypeIndex
            );

            bufferMemory = new VkDeviceMemory(SilkVulkanRenderer.device, allocInfo);
            vk.BindBufferMemory(SilkVulkanRenderer.device, ubobuffers, bufferMemory, 0);


            GCHandle uhandle = GCHandle.Alloc(ubo, GCHandleType.Pinned);
            IntPtr upointer = uhandle.AddrOfPinnedObject();
            uniformBuffers = new VulkanBuffer<UniformBufferObject>((void*)upointer, 1, BufferUsageFlags.BufferUsageTransferSrcBit | BufferUsageFlags.BufferUsageTransferDstBit, MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, false);

        }

        public CommandBuffer Draw()
        {
            var commandIndex = SilkVulkanRenderer.CommandIndex;
            var imageIndex = SilkVulkanRenderer.ImageIndex;
            var commandBuffer = commandBuffers[commandIndex];
            List<CommandBuffer> commandBufferList = new List<CommandBuffer>();
            ClearValue* clearValues = stackalloc[]
{
                new ClearValue(new ClearColorValue(1, 0, 0, 1)),
                new ClearValue(null, new ClearDepthStencilValue(1.0f, 0))
            };

            RenderPassBeginInfo renderPassInfo = new
            (
                renderPass: silk3DRendererPass.renderPass,
                framebuffer: swapChainFramebuffers[imageIndex],
                clearValueCount: 2,
                pClearValues: clearValues,
                renderArea: new(new Offset2D(0, 0), SilkVulkanRenderer.swapChain.swapchainExtent)
            );

            var viewport = new Viewport(0.0f, 0.0f, SilkVulkanRenderer.swapChain.swapchainExtent.Width, SilkVulkanRenderer.swapChain.swapchainExtent.Height, 0.0f, 1.0f);
            var scissor = new Rect2D(new Offset2D(0, 0), SilkVulkanRenderer.swapChain.swapchainExtent);

            var descSet = descriptorSets;
            var vertexbuffer = vertexBuffer.Buffer;
            var commandInfo = new CommandBufferBeginInfo(flags: 0);


            VKConst.vulkan.BeginCommandBuffer(commandBuffer, &commandInfo);
            VKConst.vulkan.CmdBeginRenderPass(commandBuffer, &renderPassInfo, SubpassContents.Inline);
            VKConst.vulkan.CmdBindPipeline(commandBuffer, PipelineBindPoint.Graphics, graphicsPipeline);
            VKConst.vulkan.CmdBindVertexBuffers(commandBuffer, 0, 1, &vertexbuffer, 0);
            VKConst.vulkan.CmdBindIndexBuffer(commandBuffer, indexBuffer.Buffer, 0, IndexType.Uint16);
            VKConst.vulkan.CmdBindDescriptorSets(commandBuffer, PipelineBindPoint.Graphics, pipelineLayout, 0, 1, descSet, 0, null);
            VKConst.vulkan.CmdSetViewport(commandBuffer, 0, 1, &viewport);
            VKConst.vulkan.CmdSetScissor(commandBuffer, 0, 1, &scissor);
            VKConst.vulkan.CmdDrawIndexed(commandBuffer, (uint)indices.Length, 1, 0, 0, 0);
            VKConst.vulkan.CmdEndRenderPass(commandBuffer);
            VKConst.vulkan.EndCommandBuffer(commandBuffer);

            return commandBuffer;
        }
        public void DrawFrame()
        {
            UpdateUniformBuffer();

            List<CommandBuffer> commandBufferList = new List<CommandBuffer>();
            SilkVulkanRenderer.StartFrame();
            commandBufferList.Add(Draw());
            SilkVulkanRenderer.EndFrame(commandBufferList);
        }

        void UpdateUniformBuffer()
        {
            float secondsPassed = (float)TimeSpan.FromTicks(DateTime.Now.Ticks - startTime).TotalSeconds;



            ubo.model = Matrix4X4.CreateFromAxisAngle(
                new Vector3D<float>(0, 0, 1),
                secondsPassed * Scalar.DegreesToRadians(90.0f));

            ubo.view = Matrix4X4.CreateLookAt(
                new Vector3D<float>(2.0f, 2.0f, 2.0f),
                new Vector3D<float>(0.0f, 0.0f, 0.0f),
                new Vector3D<float>(0.0f, 0.0f, -0.1f));

            ubo.proj = Matrix4X4.CreatePerspectiveFieldOfView(
                Scalar.DegreesToRadians(45.0f),
                SilkVulkanRenderer.swapChain.swapchainExtent.Width / (float)SilkVulkanRenderer.swapChain.swapchainExtent.Height,
                0.1f,
                10.0f);

            ubo.proj.M11 *= -1;
            uint dataSize = (uint)Marshal.SizeOf(ubo);

            Debug.Assert(ubopropertyFlags.HasFlag(MemoryPropertyFlags.MemoryPropertyHostCoherentBit | MemoryPropertyFlags.MemoryPropertyHostVisibleBit));
            Debug.Assert(dataSize <= ubosize);

            void* dataPtr = Unsafe.AsPointer(ref ubo);

            void* dstPtr = bufferMemory.MapMemory(0, dataSize);


            System.Buffer.MemoryCopy(dataPtr, dstPtr, ubosize, dataSize);

            bufferMemory.UnmapMemory();
            //GCHandle vhandle = GCHandle.Alloc(ubo, GCHandleType.Pinned);
            //IntPtr vpointer = vhandle.AddrOfPinnedObject();
            //uniformBuffers.UpdateBufferData(vpointer);
        }


        void CleanupSwapChain()
        {
          //  depthBuffer.Dispose();

            foreach (var framebuffer in swapChainFramebuffers)
            {
                // framebuffer.Dispose();              
            }

            // graphicsPipeline.Dispose();
            //pipelineLayout.Dispose();       
            // renderPass.Dispose();

            //   swapChainData.Clear(device);      
        }
    }
}

