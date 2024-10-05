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
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Tests;
using VulkanGameEngineLevelEditor.Vulkan;
namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MeshProperitiesStruct
    {
        uint MeshIndex = 0;
        uint MaterialIndex = 0;
        mat4 MeshTransform ;

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
        public Texture texture;
      //  private SilkFrameBufferRenderPass renderPass;
        //private Mesh2D mesh;

        Vk vk = Vk.GetApi();
        public ExtDebugUtils debugUtils;
        static readonly long startTime = DateTime.Now.Ticks;
        KhrSwapchain khrSwapchain;
        const int MAX_FRAMES_IN_FLIGHT = 2;
        KhrSurface khrSurface;
        bool isFramebufferResized = false;
        SurfaceKHR surface;
        Extent2D extent;
        IWindow window;


        Queue graphicsQueue;
        Queue presentQueue;

        Framebuffer[] swapChainFramebuffers;

        CommandPool commandPool;
        CommandBuffer[] commandBuffers;

        RenderPass renderPass;

        DescriptorSetLayout descriptorSetLayout;
        PipelineLayout pipelineLayout;
        Pipeline graphicsPipeline;

        Semaphore[] imageAvailableSemaphores;
        Semaphore[] renderFinishedSemaphores;
        Fence[] inFlightFences;

        UniformBufferObject ubo = new();
        VulkanBuffer<Vertex> vertexBuffer;
        VulkanBuffer<ushort> indexBuffer;
        VulkanBuffer<UniformBufferObject> uniformBuffers;
        string[] extensions;


        DepthBufferData depthBuffer;

        DescriptorPool descriptorPool;
        DescriptorSet descriptorSets;

        public string[] validationLayers = { "VK_LAYER_KHRONOS_validation" };

        string[] requiredExtensions;
        readonly string[] instanceExtensions = { ExtDebugUtils.ExtensionName };
        readonly string[] deviceExtensions = { KhrSwapchain.ExtensionName };

        Silk.NET.Vulkan.Buffer ubobuffers;
        VkDeviceMemory bufferMemory;
        ulong ubosize;
        BufferUsageFlags ubousage;
        MemoryPropertyFlags ubopropertyFlags;
        bool ubodisposedValue;

        private SceneDataBuffer sceneProperties;


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
            texture = new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\VulkanGameEngineLevelEditor\\bin\\Debug\\awesomeface.png", Format.R8G8B8A8Srgb, TextureTypeEnum.kType_DiffuseTextureMap);
            // mesh = new Mesh2D();
           // renderPass = new SilkFrameBufferRenderPass();
        }

        public void StartUp()
        {
           texture = new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\VulkanGameEngineLevelEditor\\bin\\Debug\\awesomeface.png", Format.R8G8B8A8Srgb, TextureTypeEnum.kType_DiffuseTextureMap);
            //mesh = new Mesh2D();
          // renderPass = new SilkFrameBufferRenderPass();
           // testRenderPass2D = new TestRenderPass();
            BuildRenderPasses();
        }

        public void Update(float deltaTime)
        {
            if (SilkVulkanRenderer.RebuildRendererFlag)
            {
                UpdateRenderPasses();
            }
        }

        public void BuildRenderPasses()
        {
            CreateRenderPass();
            CreateDescriptorSetLayout();
            CreateGraphicsPipeline();
            commandPool = SilkVulkanRenderer.CreateCommandPool();

            CreateDepthResources();
            CreateFramebuffers();
            texture = new GameEngineAPI.Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\VulkanGameEngineLevelEditor\\bin\\Debug\\awesomeface.png", Format.R8G8B8A8Srgb, TextureTypeEnum.kType_DiffuseTextureMap);

            for (int x = 0; x < SilkVulkanRenderer.MAX_FRAMES_IN_FLIGHT; x++)
            {
                var commandBuffer = SilkVulkanRenderer.BeginSingleUseCommandBuffer();
                TransitionImageLayout(SilkVulkanRenderer.swapChain.images[x], ImageLayout.Undefined, ImageLayout.ColorAttachmentOptimal, commandBuffer);
                TransitionImageLayout(SilkVulkanRenderer.swapChain.images[x], ImageLayout.ColorAttachmentOptimal, ImageLayout.PresentSrcKhr, commandBuffer);
                SilkVulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
            }

            const BufferUsageFlags MeshBufferUsageSettings = BufferUsageFlags.VertexBufferBit |
    BufferUsageFlags.IndexBufferBit |
     BufferUsageFlags.UniformBufferBit |
    BufferUsageFlags.BufferUsageTransferSrcBit |
     BufferUsageFlags.BufferUsageTransferDstBit;

            const MemoryPropertyFlags MeshBufferPropertySettings = MemoryPropertyFlags.MemoryPropertyHostVisibleBit |
                MemoryPropertyFlags.MemoryPropertyHostCoherentBit;

            GCHandle vhandle = GCHandle.Alloc(vertices, GCHandleType.Pinned);
            IntPtr vpointer = vhandle.AddrOfPinnedObject();
            vertexBuffer = new VulkanBuffer<Vertex>((void*)vpointer, (uint)vertices.Count(), MeshBufferUsageSettings, MeshBufferPropertySettings, true);

            GCHandle fhandle = GCHandle.Alloc(indices, GCHandleType.Pinned);
            IntPtr fpointer = fhandle.AddrOfPinnedObject();
            indexBuffer = new VulkanBuffer<ushort>((void*)fpointer, (uint)indices.Count(), MeshBufferUsageSettings, MeshBufferPropertySettings, true);

            CreateUniformBuffers();
            CreateDescriptorPool();
            CreateDescriptorSets();

            commandBuffers = new CommandBuffer[MAX_FRAMES_IN_FLIGHT];
            SilkVulkanRenderer.CreateCommandBuffers(commandBuffers);

            var timing = SilkVulkanRenderer.CreateSemaphores();
            imageAvailableSemaphores = timing.acquire;
            renderFinishedSemaphores = timing.present;
            inFlightFences = timing.fences;
            // renderPass2D.BuildRenderPass(mesh);
            //  testRenderPass2D.BuildRenderPass(texture);
            //renderPass.BuildRenderPass(texture);
        }
        public void UpdateRenderPasses()
        {
        }

        public void Draw()
        {
            UpdateUniformBuffer();

            List<CommandBuffer> commandBufferList = new List<CommandBuffer>();
            SilkVulkanRenderer.StartFrame();
            //renderPass2D.Draw(mesh);
            //testRenderPass2D.Draw();
            commandBufferList.Add(DrawStruff());
            SilkVulkanRenderer.EndFrame(commandBufferList);
            //BakeColorTexture("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/asdfa.bmp", texture);
        }

        public FormatProperties GetFormatProperties(Format format)
        {
            vk.GetPhysicalDeviceFormatProperties(SilkVulkanRenderer.physicalDevice, format,
                out FormatProperties props);

            return props;
        }

        private void TransitionImageLayout(Image image,
                                    ImageLayout oldLayout,
                                    ImageLayout newLayout,
                                    CommandBuffer commandBuffer)
        {
            var barrier = new ImageMemoryBarrier
            {
                SType = StructureType.MemoryBarrier,
                OldLayout = oldLayout,
                NewLayout = newLayout,
                SrcQueueFamilyIndex = Vk.QueueFamilyIgnored,
                DstQueueFamilyIndex = Vk.QueueFamilyIgnored,
                Image = image,
                SubresourceRange = new ImageSubresourceRange
                {
                    AspectMask = ImageAspectFlags.ColorBit,
                    BaseMipLevel = 0,
                    LevelCount = 1,
                    BaseArrayLayer = 0,
                    LayerCount = 1
                }
            };

            PipelineStageFlags sourceStage;
            PipelineStageFlags destinationStage;

            if (oldLayout == ImageLayout.Undefined && newLayout == ImageLayout.ColorAttachmentOptimal)
            {
                barrier.SrcAccessMask = 0;
                barrier.DstAccessMask = AccessFlags.ColorAttachmentWriteBit;

                sourceStage = PipelineStageFlags.TopOfPipeBit;
                destinationStage = PipelineStageFlags.ColorAttachmentOutputBit;
            }
            else if (oldLayout == ImageLayout.ColorAttachmentOptimal && newLayout == ImageLayout.PresentSrcKhr)
            {
                barrier.SrcAccessMask = AccessFlags.ColorAttachmentWriteBit;
                barrier.DstAccessMask = 0;

                sourceStage = PipelineStageFlags.ColorAttachmentOutputBit;
                destinationStage = PipelineStageFlags.BottomOfPipeBit;
            }
            else
            {
                throw new ArgumentException("Unsupported layout transition!");
            }

            vk.CmdPipelineBarrier(commandBuffer, sourceStage, destinationStage, 0, 0, null, 0, null, 1, &barrier);
        }

        public CommandBuffer DrawStruff()
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
                renderPass: renderPass,
                framebuffer: swapChainFramebuffers[imageIndex],
                clearValueCount: 2,
                pClearValues: clearValues,
                renderArea: new(new Offset2D(0, 0), SilkVulkanRenderer.swapChain.swapchainExtent)
            );

            var viewport = new Viewport(0.0f, 0.0f, SilkVulkanRenderer.swapChain.swapchainExtent.Width, SilkVulkanRenderer.swapChain.swapchainExtent.Height, 0.0f, 1.0f);
            var scissor = new Rect2D(new Offset2D(0, 0), extent);

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

        void CreateRenderPass()
        {
            renderPass = MakeRenderPass(SilkVulkanRenderer.device, SilkVulkanRenderer.swapChain.colorFormat,
                PickDepthFormat(SilkVulkanRenderer.physicalDevice));
        }

        public Format PickDepthFormat(PhysicalDevice physicalDevice)
        {
            Format[] candidates = new[] { Format.D32Sfloat, Format.D32SfloatS8Uint, Format.D24UnormS8Uint };
            foreach (Format format in candidates)
            {
                FormatProperties props = GetFormatProperties(format);

                if (props.OptimalTilingFeatures.HasFlag(FormatFeatureFlags.FormatFeatureDepthStencilAttachmentBit))
                {
                    return format;
                }
            }
            throw new Exception("failed to find supported format!");
        }

        public RenderPass MakeRenderPass(Device device,
                                    Format colorFormat,
                                    Format depthFormat,
                                    AttachmentLoadOp loadOp = AttachmentLoadOp.Clear,
                                    ImageLayout colorFinalLayout = ImageLayout.PresentSrcKhr)
        {
            List<AttachmentDescription> attachmentDescriptions = new();

            Debug.Assert(colorFormat != Format.Undefined);

            attachmentDescriptions.Add(new()
            {
                Format = colorFormat,
                Samples = SampleCountFlags.SampleCount1Bit,
                LoadOp = loadOp,
                StoreOp = AttachmentStoreOp.Store,
                StencilLoadOp = AttachmentLoadOp.DontCare,
                StencilStoreOp = AttachmentStoreOp.DontCare,
                InitialLayout = ImageLayout.Undefined,
                FinalLayout = colorFinalLayout,
            });

            if (depthFormat != Format.Undefined)
            {
                attachmentDescriptions.Add(new()
                {
                    Format = depthFormat,
                    Samples = SampleCountFlags.SampleCount1Bit,
                    LoadOp = loadOp,
                    StoreOp = AttachmentStoreOp.DontCare,
                    StencilLoadOp = AttachmentLoadOp.DontCare,
                    StencilStoreOp = AttachmentStoreOp.DontCare,
                    InitialLayout = ImageLayout.Undefined,
                    FinalLayout = ImageLayout.DepthStencilAttachmentOptimal,
                });

            }
            AttachmentReference colorAttachment = new(0, ImageLayout.ColorAttachmentOptimal);
            AttachmentReference depthAttachment = new(1, ImageLayout.DepthStencilAttachmentOptimal);

            SubpassDescription subpassDescription = new()
            {
                PipelineBindPoint = PipelineBindPoint.Graphics,
                ColorAttachmentCount = 1,
                PColorAttachments = &colorAttachment,
                PDepthStencilAttachment = (depthFormat != Format.Undefined) ? &depthAttachment : null
            };

            AttachmentDescription* attachments = (AttachmentDescription*)Mem.AllocArray<AttachmentDescription>(attachmentDescriptions.Count);

            for (int i = 0; i < attachmentDescriptions.Count; i++)
            {
                attachments[i] = attachmentDescriptions[i];
            }

            RenderPassCreateInfo renderPassCreateInfo = new
            (
                attachmentCount: (uint)attachmentDescriptions.Count,
                pAttachments: attachments,
                subpassCount: 1,
                pSubpasses: &subpassDescription
            );


            vk.CreateRenderPass(device, renderPassCreateInfo, null, out RenderPass renderPass);

            Mem.FreeArray(attachments);

            return renderPass;
        }

        uint FindMemoryType(PhysicalDeviceMemoryProperties memoryProperties, uint typeBits, MemoryPropertyFlags requirementsMask)
        {
            for (int i = 0; i < memoryProperties.MemoryTypeCount; i++)
            {
                if ((typeBits & (i << 1)) != 0 && memoryProperties.MemoryTypes[i].PropertyFlags.HasFlag(requirementsMask))
                {
                    return (uint)i;

                }
            }

            throw new Exception("failed to find suitable memory type!");
        }


        void CreateGraphicsPipeline()
        {
            ShaderModule vertShaderModule = SU.CreateShaderModule(SilkVulkanRenderer.device, "vertshader.spv");
            ShaderModule fragShaderModule = SU.CreateShaderModule(SilkVulkanRenderer.device, "fragshader.spv");

            DescriptorSetLayout descriptorSetLayoutPtr = descriptorSetLayout;

            PipelineLayoutCreateInfo pipelineLayoutInfo = new
            (
                setLayoutCount: 1,
                pSetLayouts: &descriptorSetLayoutPtr,
                pushConstantRangeCount: 0,
                pPushConstantRanges: null
            );
            vk.CreatePipelineLayout(SilkVulkanRenderer.device, &pipelineLayoutInfo, null, out PipelineLayout pipelinelayout);
            pipelineLayout = pipelinelayout;

            var specializationInfo = new SpecializationInfo(null);
            PipelineShaderStageCreateInfo* pipelineShaderStageCreateInfos = stackalloc[]
          {
                new PipelineShaderStageCreateInfo(
                    stage: ShaderStageFlags.ShaderStageVertexBit,
                    module: vertShaderModule,
                    pName: (byte*)SilkMarshal.StringToPtr("main"),
                    pSpecializationInfo: &specializationInfo),

                new PipelineShaderStageCreateInfo(
                    stage: ShaderStageFlags.ShaderStageFragmentBit,
                    module: fragShaderModule,
                    pName: (byte*)SilkMarshal.StringToPtr("main"),
                    pSpecializationInfo: &specializationInfo)
            };

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
                pStages: pipelineShaderStageCreateInfos,
                pVertexInputState: &pipelineVertexInputStateCreateInfo,
                pInputAssemblyState: &pipelineInputAssemblyStateCreateInfo,
                pViewportState: &pipelineViewportStateCreateInfo,
                pRasterizationState: &pipelineRasterizationStateCreateInfo,
                pMultisampleState: &pipelineMultisampleStateCreateInfo,
                pDepthStencilState: &pipelineDepthStencilStateCreateInfo,
                pColorBlendState: &pipelineColorBlendStateCreateInfo,
                pDynamicState: &pipelineDynamicStateCreateInfo,
                layout: pipelineLayout,
                renderPass: renderPass
            );

            vk.CreateGraphicsPipelines(SilkVulkanRenderer.device, new PipelineCache(null), 1, &graphicsPipelineCreateInfo, null, out Pipeline pipeline);
            graphicsPipeline = pipeline;

            SilkMarshal.Free((nint)pipelineShaderStageCreateInfos[0].PName);
            SilkMarshal.Free((nint)pipelineShaderStageCreateInfos[1].PName);

            Mem.FreeArray(vertexInputAttributeDescriptions);

            //vertShaderModule.Dispose();
            // fragShaderModule.Dispose();
        }

        void CreateDescriptorSetLayout()
        {
            descriptorSetLayout = SU.MakeDescriptorSetLayout(SilkVulkanRenderer.device, new[]{
                (DescriptorType.UniformBuffer, 1, ShaderStageFlags.ShaderStageVertexBit),
                (DescriptorType.CombinedImageSampler, 1, ShaderStageFlags.ShaderStageFragmentBit)});


        }

        public void OneTimeSubmit(Device device, CommandPool commandPool, Queue queue, Action<VkCommandBuffer> func)
        {
            VkCommandBuffers commandBuffers = new(device,
                new(commandPool: commandPool, level: CommandBufferLevel.Primary, commandBufferCount: 1));
            var commandBuffer = commandBuffers.FirstOrDefault();

            OneTimeSubmit(commandBuffer, SilkVulkanRenderer.graphicsQueue, func);

            commandBuffer.Dispose();
        }

        public void OneTimeSubmit(VkCommandBuffer commandBuffer, Queue queue, Action<VkCommandBuffer> func)
        {
            commandBuffer.Begin(new(flags: CommandBufferUsageFlags.CommandBufferUsageOneTimeSubmitBit));
            func(commandBuffer);
            commandBuffer.End();

            CommandBuffer buffer = commandBuffer;

            SubmitInfo submitInfo = new(commandBufferCount: 1, pCommandBuffers: &buffer);
            Submit(queue, submitInfo, null);
            WaitIdle(queue);
        }

        public void Submit(Queue queue, SubmitInfo submit, void* fence)
        {
            var result = vk.QueueSubmit(queue, 1, submit, new Fence(null));

            if (result != Result.Success)
            {
                //ResultException.Throw(result, "Error submitting queue");
            }
        }
        public void WaitIdle(Queue queue)
        {
            vk.QueueWaitIdle(queue);
        }

        void CreateDepthResources()
        {
            Format depthFormat = PickDepthFormat(SilkVulkanRenderer.physicalDevice);

            depthBuffer = new DepthBufferData(SilkVulkanRenderer.physicalDevice, SilkVulkanRenderer.device, depthFormat, SilkVulkanRenderer.swapChain.swapchainExtent);

            OneTimeSubmit(SilkVulkanRenderer.device, commandPool, graphicsQueue,
                commandBuffer => SetImageLayout(commandBuffer, depthBuffer.image, depthFormat,
                ImageLayout.Undefined, ImageLayout.DepthStencilAttachmentOptimal));

        }

        public static void SetImageLayout(VkCommandBuffer commandBuffer, Image image, Format format, ImageLayout oldImageLayout, ImageLayout newImageLayout)
        {
            AccessFlags sourceAccessMask = 0;

            switch (oldImageLayout)
            {
                case ImageLayout.TransferDstOptimal:
                    {
                        sourceAccessMask = AccessFlags.AccessTransferWriteBit;
                        break;
                    }
                case ImageLayout.Preinitialized:
                    {
                        sourceAccessMask = AccessFlags.AccessHostWriteBit;
                        break;
                    }
                case ImageLayout.ColorAttachmentOptimal:
                    {
                        sourceAccessMask = AccessFlags.AccessColorAttachmentWriteBit;
                        break;
                    }
                case ImageLayout.General:  // sourceAccessMask is empty
                case ImageLayout.Undefined:
                    {
                        break;
                    }
                default:
                    {
                        Debug.Assert(false);
                        break;
                    }
            }

            PipelineStageFlags sourceStage = 0;

            switch (oldImageLayout)
            {
                case ImageLayout.General:
                case ImageLayout.Preinitialized:
                    {
                        sourceStage = PipelineStageFlags.PipelineStageHostBit;
                        break;
                    }
                case ImageLayout.TransferDstOptimal:
                    {
                        sourceStage = PipelineStageFlags.PipelineStageTransferBit;
                        break;
                    }
                case ImageLayout.Undefined:
                    {
                        sourceStage = PipelineStageFlags.PipelineStageTopOfPipeBit;
                        break;
                    }
                default:
                    {
                        Debug.Assert(false);
                        break;
                    }
            }

            AccessFlags destinationAccessMask = 0;

            switch (newImageLayout)
            {
                case ImageLayout.ColorAttachmentOptimal:
                    {
                        destinationAccessMask = AccessFlags.AccessColorAttachmentWriteBit;
                        break;
                    }
                case ImageLayout.DepthStencilAttachmentOptimal:
                    {
                        destinationAccessMask = AccessFlags.AccessDepthStencilAttachmentReadBit | AccessFlags.AccessDepthStencilAttachmentWriteBit;
                        break;
                    }
                case ImageLayout.General:  // empty destinationAccessMask
                case ImageLayout.PresentSrcKhr:
                    {
                        break;
                    }
                case ImageLayout.ShaderReadOnlyOptimal:
                    {
                        destinationAccessMask = AccessFlags.AccessShaderReadBit;
                        break;
                    }
                case ImageLayout.TransferSrcOptimal:
                    {
                        destinationAccessMask = AccessFlags.AccessTransferReadBit;
                        break;
                    }
                case ImageLayout.TransferDstOptimal:
                    {
                        destinationAccessMask = AccessFlags.AccessTransferWriteBit;
                        break;
                    }
                default:
                    {
                        Debug.Assert(false);
                        break;
                    }
            }

            PipelineStageFlags destinationStage = 0;

            switch (newImageLayout)
            {
                case ImageLayout.ColorAttachmentOptimal:
                    {
                        destinationStage = PipelineStageFlags.PipelineStageColorAttachmentOutputBit;
                        break;
                    }
                case ImageLayout.DepthStencilAttachmentOptimal:
                    {
                        destinationStage = PipelineStageFlags.PipelineStageEarlyFragmentTestsBit;
                        break;
                    }
                case ImageLayout.General:
                    {
                        destinationStage = PipelineStageFlags.PipelineStageHostBit;
                        break;
                    }
                case ImageLayout.PresentSrcKhr:
                    {
                        destinationStage = PipelineStageFlags.PipelineStageBottomOfPipeBit;
                        break;
                    }
                case ImageLayout.ShaderReadOnlyOptimal:
                    {
                        destinationStage = PipelineStageFlags.PipelineStageFragmentShaderBit;
                        break;
                    }
                case ImageLayout.TransferDstOptimal:
                case ImageLayout.TransferSrcOptimal:
                    {
                        destinationStage = PipelineStageFlags.PipelineStageTransferBit;
                        break;
                    }
                default:
                    {
                        Debug.Assert(false);
                        break;
                    }
            }

            ImageAspectFlags aspectMask = ImageAspectFlags.ImageAspectColorBit;

            if (newImageLayout == ImageLayout.DepthStencilAttachmentOptimal)
            {
                aspectMask = ImageAspectFlags.ImageAspectDepthBit;

                if (format == Format.D32SfloatS8Uint || format == Format.D24UnormS8Uint)
                {
                    aspectMask |= ImageAspectFlags.ImageAspectStencilBit;
                }
            }

            ImageSubresourceRange imageSubresourceRange = new(aspectMask, 0, 1, 0, 1);
            ImageMemoryBarrier imageMemoryBarrier = new
            (
                srcAccessMask: sourceAccessMask,
                dstAccessMask: destinationAccessMask,
                oldLayout: oldImageLayout,
                newLayout: newImageLayout,
                srcQueueFamilyIndex: Vk.QueueFamilyIgnored,
                dstQueueFamilyIndex: Vk.QueueFamilyIgnored,
                image: image,
                subresourceRange: imageSubresourceRange
            );

            var imageMemoryBarriers = new ReadOnlySpan<ImageMemoryBarrier>(new[] { imageMemoryBarrier });

            commandBuffer.PipelineBarrier(sourceStage, destinationStage, 0, null, null, imageMemoryBarriers);
        }

        void CreateFramebuffers()
        {
            swapChainFramebuffers = SU.MakeFramebuffers(SilkVulkanRenderer.device, renderPass, SilkVulkanRenderer.swapChain.imageViews, depthBuffer.imageView,
                SilkVulkanRenderer.swapChain.swapchainExtent);
        }

        void CreateDescriptorPool()
        {



            DescriptorPoolSize[] poolSizes = new DescriptorPoolSize[]
            {
                new DescriptorPoolSize
                {
                    Type = DescriptorType.UniformBuffer,
                    DescriptorCount = MAX_FRAMES_IN_FLIGHT
                },

                new DescriptorPoolSize
                {
                    Type = DescriptorType.CombinedImageSampler,
                    DescriptorCount = MAX_FRAMES_IN_FLIGHT
                }
            };

            descriptorPool = SU.MakeDescriptorPool(SilkVulkanRenderer.device, poolSizes);

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
            uint memoryTypeIndex = FindMemoryType(GetMemoryProperties(SilkVulkanRenderer.physicalDevice), memoryRequirements.MemoryTypeBits, ubopropertyFlags);

            MemoryAllocateInfo allocInfo = new
            (
                allocationSize: memoryRequirements.Size,
                memoryTypeIndex: memoryTypeIndex
            );

            bufferMemory = new VkDeviceMemory(SilkVulkanRenderer.device, allocInfo);
            vk.BindBufferMemory(SilkVulkanRenderer.device, ubobuffers, bufferMemory, 0);


            //GCHandle uhandle = GCHandle.Alloc(ubo, GCHandleType.Pinned);
            //IntPtr upointer = uhandle.AddrOfPinnedObject();
            //uniformBuffers = new VulkanBuffer<UniformBufferObject>((void*)upointer, 1, BufferUsageFlags.BufferUsageTransferSrcBit | BufferUsageFlags.BufferUsageTransferDstBit, MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, false);

        }

        public PhysicalDeviceMemoryProperties GetMemoryProperties(PhysicalDevice physicalDevice)
        {
            vk.GetPhysicalDeviceMemoryProperties(physicalDevice, out PhysicalDeviceMemoryProperties memProperties);

            return memProperties;
        }

        void CreateDescriptorSets()
        {
            DescriptorSetLayout* layouts = stackalloc DescriptorSetLayout[MAX_FRAMES_IN_FLIGHT];

            for (int i = 0; i < MAX_FRAMES_IN_FLIGHT; i++)
            {
                layouts[i] = descriptorSetLayout;
            }

            DescriptorSetAllocateInfo allocInfo = new
            (
                descriptorPool: descriptorPool,
                descriptorSetCount: MAX_FRAMES_IN_FLIGHT,
                pSetLayouts: layouts
            );
            SilkVulkanRenderer.vulkan.AllocateDescriptorSets(SilkVulkanRenderer.device, &allocInfo, out DescriptorSet descriptorSet);
            descriptorSets = descriptorSet;

            var colorDescriptorImage = new DescriptorImageInfo
            {
                Sampler = texture.Sampler,
                ImageView = texture.View,
                ImageLayout = ImageLayout.ReadOnlyOptimal
            };


            DescriptorBufferInfo uniformBuffer = new DescriptorBufferInfo
            {
                Buffer = ubobuffers,
                Offset = 0,
                Range = Vk.WholeSize
            };

            List<WriteDescriptorSet> descriptorSetList = new List<WriteDescriptorSet>();
            for (uint x = 0; x < SilkVulkanRenderer.swapChain.ImageCount; x++)
            {
                WriteDescriptorSet descriptorSetWrite = new WriteDescriptorSet
                {
                    SType = StructureType.WriteDescriptorSet,
                    DstSet = descriptorSet,
                    DstBinding = 0,
                    DstArrayElement = 0,
                    DescriptorCount = 1,
                    DescriptorType = DescriptorType.UniformBuffer,
                    PImageInfo = null,
                    PBufferInfo = &uniformBuffer,
                    PTexelBufferView = null
                };

                WriteDescriptorSet descriptorSetWrite2 = new WriteDescriptorSet
                {
                    SType = StructureType.WriteDescriptorSet,
                    DstSet = descriptorSet,
                    DstBinding = 1,
                    DstArrayElement = 0,
                    DescriptorCount = 1,
                    DescriptorType = DescriptorType.CombinedImageSampler,
                    PImageInfo = &colorDescriptorImage,
                    PBufferInfo = null,
                    PTexelBufferView = null
                };

                descriptorSetList.Add(descriptorSetWrite);
                descriptorSetList.Add(descriptorSetWrite2);
            }


            fixed (WriteDescriptorSet* ptr = descriptorSetList.ToArray())
            {
                SilkVulkanRenderer.vulkan.UpdateDescriptorSets(SilkVulkanRenderer.device, (uint)descriptorSetList.UCount(), ptr, 0, null);
            }
        }


        //[DllImport("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int DLL_stbi_write_bmp(string filename, int w, int h, int comp, void* data);
        //public unsafe void BakeColorTexture(string filename, Texture texture)
        //{
        //    //std::shared_ptr<Texture2D> BakeTexture = std::make_shared<Texture2D>(Texture2D(Pixel(255, 0, 0), glm::vec2(1280,720), VkFormat::VK_FORMAT_R8G8B8A8_UNORM, TextureTypeEnum::kTextureAtlus));
        //    var pixel = new Pixel(0xFF, 0x00, 0x00, 0xFF);

        //    BakeTexture bakeTexture = new BakeTexture(pixel, new GlmSharp.ivec2(texture.Width, texture.Width), VkFormat.VK_FORMAT_R8G8B8A8_UNORM);

        //    VkCommandBuffer commandBuffer = VulkanRenderer.BeginCommandBuffer();

        //    bakeTexture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
        //    texture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL);

        //    VkImageCopy copyImage = new VkImageCopy();
        //    copyImage.srcSubresource.aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT;
        //    copyImage.srcSubresource.layerCount = 1;

        //    copyImage.dstSubresource.aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT;
        //    copyImage.dstSubresource.layerCount = 1;

        //    copyImage.dstOffset.X = 0;
        //    copyImage.dstOffset.Y = 0;
        //    copyImage.dstOffset.Z = 0;

        //    copyImage.extent.Width = (uint)texture.Width;
        //    copyImage.extent.Height = (uint)texture.Height;
        //    copyImage.extent.Depth = 1;

        //    vkCmdCopyImage(commandBuffer, texture.Image, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL, bakeTexture.Image, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &copyImage);

        //    bakeTexture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_GENERAL);
        //    texture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR);
        //    VulkanRenderer.EndCommandBuffer(commandBuffer);

        //    VkImageSubresource subResource = new VkImageSubresource { aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT, mipLevel = 0, arrayLayer = 0 };
        //    VkSubresourceLayout subResourceLayout;
        //    vkGetImageSubresourceLayout(VulkanRenderer.Device, bakeTexture.Image, &subResource, &subResourceLayout);

        //    void* data;
        //    vkMapMemory(VulkanRenderer.Device, bakeTexture.Memory, 0, VulkanConsts.VK_WHOLE_SIZE, 0, (void**)&data);

        //    DLL_stbi_write_bmp(filename, bakeTexture.Width, bakeTexture.Height, 4, data);
        //}


        public void Destroy()
        {

        }
    }
}