using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Maths;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Linq;
using Semaphore = Silk.NET.Vulkan.Semaphore;
using VulkanGameEngineLevelEditor.Vulkan;
using StbImageSharp;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using Silk.NET.SDL;
using System.Buffers;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Windows.Forms;
using Image = Silk.NET.Vulkan.Image;

namespace VulkanGameEngineLevelEditor.Tests
{
    unsafe class VulkanTutorial
    {
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

        Instance instance;
        DebugUtilsMessengerEXT debugMessenger;

        SwapchainKHR swapChain;

        PhysicalDevice physicalDevice;
        Device device;

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
        public GameEngineAPI.Texture texture;

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
            instance = SilkVulkanRenderer.CreateInstance(_richTextBox);

            var asdf = SilkVulkanRenderer.CreateSurface(window);
            surface = asdf.Surface;
            khrSurface = asdf.KhrSurface;
            extent = SilkVulkanRenderer.swapChain.swapchainExtent;
            physicalDevice = SilkVulkanRenderer.CreatePhysicalDevice(khrSurface);

            device = SilkVulkanRenderer.CreateDevice();
            var ques = SilkVulkanRenderer.CreateDeviceQueue();

            graphicsQueue = ques.graphics;
            presentQueue = ques.present;

            if (!vk.TryGetDeviceExtension(instance, device, out khrSwapchain))
            {
                throw new NotSupportedException("KHR_swapchain extension not found.");
            }
            swapChain = SilkVulkanRenderer.swapChain.CreateSwapChain(window, khrSurface, surface);

            CreateRenderPass();
            CreateDescriptorSetLayout();
            CreateGraphicsPipeline();
            commandPool = SilkVulkanRenderer.CreateCommandPool();

            CreateDepthResources();
            CreateFramebuffers();
            texture = new GameEngineAPI.Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\VulkanGameEngineLevelEditor\\bin\\Debug\\awesomeface.png", Format.R8G8B8A8Srgb, TextureTypeEnum.kType_DiffuseTextureMap);


                GCHandle vhandle = GCHandle.Alloc(vertices, GCHandleType.Pinned);
                IntPtr vpointer = vhandle.AddrOfPinnedObject();
                vertexBuffer = new VulkanBuffer<Vertex>((void*)vpointer, (uint)vertices.Count(), BufferUsageFlags.BufferUsageTransferSrcBit | BufferUsageFlags.BufferUsageTransferDstBit, MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, true);
            
                GCHandle fhandle = GCHandle.Alloc(indices, GCHandleType.Pinned);
                IntPtr fpointer = fhandle.AddrOfPinnedObject();
                indexBuffer = new VulkanBuffer<ushort>((void*)fpointer, (uint)indices.Count(), BufferUsageFlags.BufferUsageTransferSrcBit | BufferUsageFlags.BufferUsageTransferDstBit, MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, true);

            CreateUniformBuffers();
            CreateDescriptorPool();
            CreateDescriptorSets();

            commandBuffers = new CommandBuffer[MAX_FRAMES_IN_FLIGHT];
            SilkVulkanRenderer.CreateCommandBuffers(commandBuffers);

            var timing = SilkVulkanRenderer.CreateSemaphores();
            imageAvailableSemaphores = timing.acquire;
            renderFinishedSemaphores = timing.present;
            inFlightFences = timing.fences;
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
                                    Silk.NET.Vulkan.ImageLayout colorFinalLayout = Silk.NET.Vulkan.ImageLayout.PresentSrcKhr)
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
                InitialLayout = Silk.NET.Vulkan.ImageLayout.Undefined,
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
                    InitialLayout = Silk.NET.Vulkan.ImageLayout.Undefined,
                    FinalLayout = Silk.NET.Vulkan.ImageLayout.DepthStencilAttachmentOptimal,
                });

            }
            AttachmentReference colorAttachment = new(0, Silk.NET.Vulkan.ImageLayout.ColorAttachmentOptimal);
            AttachmentReference depthAttachment = new(1, Silk.NET.Vulkan.ImageLayout.DepthStencilAttachmentOptimal);

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

        void CreateFramebuffers()
        {
            swapChainFramebuffers = SU.MakeFramebuffers(device, renderPass, SilkVulkanRenderer.swapChain.imageViews, depthBuffer.imageView,
                SilkVulkanRenderer.swapChain.swapchainExtent);
        }

        void CreateDepthResources()
        {
            Format depthFormat = PickDepthFormat(SilkVulkanRenderer.physicalDevice);

            depthBuffer = new DepthBufferData(SilkVulkanRenderer.physicalDevice, SilkVulkanRenderer.device, depthFormat, SilkVulkanRenderer.swapChain.swapchainExtent);

            OneTimeSubmit(SilkVulkanRenderer.device, commandPool, graphicsQueue,
                commandBuffer => SetImageLayout(commandBuffer, depthBuffer.image, depthFormat,
                Silk.NET.Vulkan.ImageLayout.Undefined, Silk.NET.Vulkan.ImageLayout.DepthStencilAttachmentOptimal));

        }


        public static void SetImageLayout(VkCommandBuffer commandBuffer, Image image, Format format, Silk.NET.Vulkan.ImageLayout oldImageLayout, Silk.NET.Vulkan.ImageLayout newImageLayout)
        {
            AccessFlags sourceAccessMask = 0;

            switch (oldImageLayout)
            {
                case Silk.NET.Vulkan.ImageLayout.TransferDstOptimal:
                    {
                        sourceAccessMask = AccessFlags.AccessTransferWriteBit;
                        break;
                    }
                case Silk.NET.Vulkan.ImageLayout.Preinitialized:
                    {
                        sourceAccessMask = AccessFlags.AccessHostWriteBit;
                        break;
                    }
                case Silk.NET.Vulkan.ImageLayout.ColorAttachmentOptimal:
                    {
                        sourceAccessMask = AccessFlags.AccessColorAttachmentWriteBit;
                        break;
                    }
                case Silk.NET.Vulkan.ImageLayout.General:  // sourceAccessMask is empty
                case Silk.NET.Vulkan.ImageLayout.Undefined:
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
                case Silk.NET.Vulkan.ImageLayout.General:
                case Silk.NET.Vulkan.ImageLayout.Preinitialized:
                    {
                        sourceStage = PipelineStageFlags.PipelineStageHostBit;
                        break;
                    }
                case Silk.NET.Vulkan.ImageLayout.TransferDstOptimal:
                    {
                        sourceStage = PipelineStageFlags.PipelineStageTransferBit;
                        break;
                    }
                case Silk.NET.Vulkan.ImageLayout.Undefined:
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
                case Silk.NET.Vulkan.ImageLayout.ColorAttachmentOptimal:
                    {
                        destinationAccessMask = AccessFlags.AccessColorAttachmentWriteBit;
                        break;
                    }
                case Silk.NET.Vulkan.ImageLayout.DepthStencilAttachmentOptimal:
                    {
                        destinationAccessMask = AccessFlags.AccessDepthStencilAttachmentReadBit | AccessFlags.AccessDepthStencilAttachmentWriteBit;
                        break;
                    }
                case Silk.NET.Vulkan.ImageLayout.General:  // empty destinationAccessMask
                case Silk.NET.Vulkan.ImageLayout.PresentSrcKhr:
                    {
                        break;
                    }
                case Silk.NET.Vulkan.ImageLayout.ShaderReadOnlyOptimal:
                    {
                        destinationAccessMask = AccessFlags.AccessShaderReadBit;
                        break;
                    }
                case Silk.NET.Vulkan.ImageLayout.TransferSrcOptimal:
                    {
                        destinationAccessMask = AccessFlags.AccessTransferReadBit;
                        break;
                    }
                case Silk.NET.Vulkan.ImageLayout.TransferDstOptimal:
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
                case Silk.NET.Vulkan.ImageLayout.ColorAttachmentOptimal:
                    {
                        destinationStage = PipelineStageFlags.PipelineStageColorAttachmentOutputBit;
                        break;
                    }
                case Silk.NET.Vulkan.ImageLayout.DepthStencilAttachmentOptimal:
                    {
                        destinationStage = PipelineStageFlags.PipelineStageEarlyFragmentTestsBit;
                        break;
                    }
                case Silk.NET.Vulkan.ImageLayout.General:
                    {
                        destinationStage = PipelineStageFlags.PipelineStageHostBit;
                        break;
                    }
                case Silk.NET.Vulkan.ImageLayout.PresentSrcKhr:
                    {
                        destinationStage = PipelineStageFlags.PipelineStageBottomOfPipeBit;
                        break;
                    }
                case Silk.NET.Vulkan.ImageLayout.ShaderReadOnlyOptimal:
                    {
                        destinationStage = PipelineStageFlags.PipelineStageFragmentShaderBit;
                        break;
                    }
                case Silk.NET.Vulkan.ImageLayout.TransferDstOptimal:
                case Silk.NET.Vulkan.ImageLayout.TransferSrcOptimal:
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

            if (newImageLayout == Silk.NET.Vulkan.ImageLayout.DepthStencilAttachmentOptimal)
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

        void CreateUniformBuffers()
        {
            uint bufferSize = (uint)sizeof(UniformBufferObject);

            ubosize = (uint)sizeof(UniformBufferObject);
            ubousage = BufferUsageFlags.BufferUsageUniformBufferBit;
            ubopropertyFlags = MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit;


            var bufferInfo = new BufferCreateInfo(size: ubosize, usage: ubousage);
             vk.CreateBuffer(device, in bufferInfo, null, out Silk.NET.Vulkan.Buffer ubobufferss);
            ubobuffers = ubobufferss;

            vk.GetBufferMemoryRequirements(device, ubobuffers, out MemoryRequirements memoryRequirements);
            uint memoryTypeIndex = FindMemoryType(GetMemoryProperties(physicalDevice), memoryRequirements.MemoryTypeBits, ubopropertyFlags);

            MemoryAllocateInfo allocInfo = new
            (
                allocationSize: memoryRequirements.Size,
                memoryTypeIndex: memoryTypeIndex
            );

             bufferMemory = new VkDeviceMemory(device, allocInfo);
            vk.BindBufferMemory(device, ubobuffers, bufferMemory, 0);

         
            //GCHandle uhandle = GCHandle.Alloc(ubo, GCHandleType.Pinned);
            //IntPtr upointer = uhandle.AddrOfPinnedObject();
            //uniformBuffers = new VulkanBuffer<UniformBufferObject>((void*)upointer, 1, BufferUsageFlags.BufferUsageTransferSrcBit | BufferUsageFlags.BufferUsageTransferDstBit, MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, false);

        }

        public PhysicalDeviceMemoryProperties GetMemoryProperties(PhysicalDevice physicalDevice)
        {
            vk.GetPhysicalDeviceMemoryProperties(physicalDevice, out PhysicalDeviceMemoryProperties memProperties);

            return memProperties;
        }

        void CreateDescriptorSetLayout()
        {
            descriptorSetLayout = SU.MakeDescriptorSetLayout(SilkVulkanRenderer.device, new[]{
                (DescriptorType.UniformBuffer, 1, ShaderStageFlags.ShaderStageVertexBit),
                (DescriptorType.CombinedImageSampler, 1, ShaderStageFlags.ShaderStageFragmentBit)});


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

            descriptorPool = SU.MakeDescriptorPool(device, poolSizes);

        }
        public FormatProperties GetFormatProperties(Format format)
        {
            vk.GetPhysicalDeviceFormatProperties(SilkVulkanRenderer.physicalDevice, format,
                out FormatProperties props);

            return props;
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
                ImageLayout = Silk.NET.Vulkan.ImageLayout.ReadOnlyOptimal
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
                renderPass: renderPass,
                framebuffer: swapChainFramebuffers[imageIndex],
                clearValueCount: 2,
                pClearValues: clearValues,
                renderArea: new(new Offset2D(0, 0), SilkVulkanRenderer.swapChain.swapchainExtent)
            );

            var viewport = new Viewport(0.0f, 0.0f, extent.Width, extent.Height, 0.0f, 1.0f);
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
            depthBuffer.Dispose();

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

