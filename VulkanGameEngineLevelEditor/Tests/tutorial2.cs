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
        uint currentFrame = 0;
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
        VkCommandBuffer[] commandBuffers;

        RenderPass renderPass;

        DescriptorSetLayout descriptorSetLayout;
        PipelineLayout pipelineLayout;
        Pipeline graphicsPipeline;

        Semaphore[] imageAvailableSemaphores;
        Semaphore[] renderFinishedSemaphores;
        Fence[] inFlightFences;

        VulkanBuffer<Vertex> vertexBuffer;
        VulkanBuffer<ushort> indexBuffer;
        BufferData[] uniformBuffers;
        string[] extensions;
        public GameEngineAPI.Texture texture;

        DepthBufferData depthBuffer;

        DescriptorPool descriptorPool;
        VkDescriptorSet[] descriptorSets;

        public string[] validationLayers = { "VK_LAYER_KHRONOS_validation" };

        string[] requiredExtensions;
        readonly string[] instanceExtensions = { ExtDebugUtils.ExtensionName };
        readonly string[] deviceExtensions = { KhrSwapchain.ExtensionName };

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

        public void Run(IWindow windows)
        {
            InitWindow(windows);
            InitializeVulkan();
        }

        public void WaitIdle()
        {
            vk.DeviceWaitIdle(device);
        }

        public void WaitForFences(Fence fence, ulong timeout)
        {
            var result = vk.WaitForFences(device, 1, fence, true, timeout);

            if (result != Result.Success)
            {
                //  ResultException.Throw(result, "Error waiting for fence");
            }
        }

        public void ResetFences(Fence fence)
        {
            var result = vk.ResetFences(device, 1, fence);

            if (result != Result.Success)
            {
                // ResultException.Throw(result, "Error resetting fence");
            }
        }


        public void InitializeVulkan()
        {
            CreateInstance();
            CreateLogicalDevice();

            CreateSwapChain();
            CreateRenderPass();
            CreateDescriptorSetLayout();
            CreateGraphicsPipeline();
            CreateCommandPool();

            CreateDepthResources();
            CreateFramebuffers();
            CreateTextureImage();
            CreateVertexBuffer();
            CreateIndexBuffer();
            CreateUniformBuffers();
            CreateDescriptorPool();
            CreateDescriptorSets();

            CreateCommandBuffer();
            CreateSyncObjects();
        }

        public bool GetSurfaceSupportKHR(PhysicalDevice tempDevice, uint queueFamilyIndex)
        {
            Result result = khrSurface.GetPhysicalDeviceSurfaceSupport(tempDevice, queueFamilyIndex, surface, out Bool32 presentSupport);
            if (result != Result.Success)
            {
                //  ResultException.Throw(result, "Error getting supported surfaces by physical device");
            }

            return presentSupport;
        }

        public QueueFamilyIndices FindGraphicsAndPresentQueueFamilyIndex(PhysicalDevice tempDevice)
        {
            QueueFamilyIndices queueFamilyIndices = new();

            uint queueFamilyCount = 0;
            SU.GetQueueFamilyProperties(tempDevice, &queueFamilyCount, null);

            QueueFamilyProperties[] queueFamilies = new QueueFamilyProperties[queueFamilyCount];
            SU.GetQueueFamilyProperties(tempDevice, &queueFamilyCount, queueFamilies);

            uint i = 0;
            foreach (var queueFamily in queueFamilies)
            {
                if (queueFamily.QueueFlags.HasFlag(QueueFlags.QueueGraphicsBit))
                {
                    queueFamilyIndices.GraphicsFamily = i;
                }

                bool isPresentSupport = GetSurfaceSupportKHR(tempDevice, i);

                if (isPresentSupport)
                {
                    queueFamilyIndices.PresentFamily = i;
                }

                if (queueFamilyIndices.IsComplete)
                {
                    break;
                }

                i++;
            }

            return queueFamilyIndices;
        }

        void CreateInstance()
        {
            instance = SilkVulkanRenderer.CreateInstance();

            var asdf = SilkVulkanRenderer.CreateSurface(window);
            surface = asdf.Surface;
            khrSurface = asdf.KhrSurface;
            extent = SilkVulkanRenderer.swapChain.swapchainExtent;
            physicalDevice = SilkVulkanRenderer.CreatePhysicalDevice(khrSurface);
        }

        public void GetQueueFamilyProperties(PhysicalDevice physicalDevice, uint* queueFamilyPropertyCount, Span<QueueFamilyProperties> queueFamilyProperties)
        {
            vk.GetPhysicalDeviceQueueFamilyProperties(physicalDevice, queueFamilyPropertyCount, queueFamilyProperties);
        }

        void CreateLogicalDevice()
        {
            device = SilkVulkanRenderer.CreateDevice();
            var ques = SilkVulkanRenderer.CreateDeviceQueue();

            graphicsQueue = ques.graphics;
            presentQueue = ques.present;
        }

        void CreateSwapChain()
        {
            if (!vk.TryGetDeviceExtension(instance, device, out khrSwapchain))
            {
                throw new NotSupportedException("KHR_swapchain extension not found.");
            }

            swapChain = SilkVulkanRenderer.swapChain.CreateSwapChain(window, khrSurface, surface);
        }

        void CreateGraphicsPipeline()
        {
            ShaderModule vertShaderModule = SU.CreateShaderModule(device, "vertshader.spv");
            ShaderModule fragShaderModule = SU.CreateShaderModule(device, "fragshader.spv");

            DescriptorSetLayout descriptorSetLayoutPtr = descriptorSetLayout;

            PipelineLayoutCreateInfo pipelineLayoutInfo = new
            (
                setLayoutCount: 1,
                pSetLayouts: &descriptorSetLayoutPtr,
                pushConstantRangeCount: 0,
                pPushConstantRanges: null
            );
            vk.CreatePipelineLayout(device, &pipelineLayoutInfo, null, out PipelineLayout pipelinelayout);
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

            vk.CreateGraphicsPipelines(device, new PipelineCache(null), 1, &graphicsPipelineCreateInfo, null, out Pipeline pipeline);
            graphicsPipeline = pipeline;

            SilkMarshal.Free((nint)pipelineShaderStageCreateInfos[0].PName);
            SilkMarshal.Free((nint)pipelineShaderStageCreateInfos[1].PName);

            Mem.FreeArray(vertexInputAttributeDescriptions);

            //vertShaderModule.Dispose();
            // fragShaderModule.Dispose();
        }

        void CreateRenderPass()
        {
            renderPass = SU.MakeRenderPass(device, SilkVulkanRenderer.swapChain.colorFormat,
                SU.PickDepthFormat(physicalDevice));
        }

        void CreateFramebuffers()
        {
            swapChainFramebuffers = SU.MakeFramebuffers(device, renderPass, SilkVulkanRenderer.swapChain.imageViews, depthBuffer.imageView,
                SilkVulkanRenderer.swapChain.swapchainExtent);
        }

        void CreateCommandPool()
        {
            commandPool = SilkVulkanRenderer.CreateCommandPool();
        }

        void CreateDepthResources()
        {
            Format depthFormat = SU.PickDepthFormat(physicalDevice);

            depthBuffer = new DepthBufferData(physicalDevice, device, depthFormat, SilkVulkanRenderer.swapChain.swapchainExtent);

            SU.OneTimeSubmit(device, commandPool, graphicsQueue,
                commandBuffer => SU.SetImageLayout(commandBuffer, depthBuffer.image, depthFormat,
                ImageLayout.Undefined, ImageLayout.DepthStencilAttachmentOptimal));

        }

        void CreateTextureImage()
        {
            texture = new GameEngineAPI.Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\VulkanGameEngineLevelEditor\\bin\\Debug\\awesomeface.png", Format.R8G8B8A8Srgb, TextureTypeEnum.kType_DiffuseTextureMap);
        }

        public void PresentKHR(Queue queue, KhrSwapchain khrSwapchain, in PresentInfoKHR presentInfo)
        {
            khrSwapchain.QueuePresent(queue, in presentInfo);
        }

        public void Submit(Queue queue, SubmitInfo submit, Fence fence)
        {
            var result = vk.QueueSubmit(queue, 1, submit, fence);

            if (result != Result.Success)
            {
                //ResultException.Throw(result, "Error submitting queue");
            }
        }

        void CreateVertexBuffer()
        {
            GCHandle handle = GCHandle.Alloc(vertices, GCHandleType.Pinned);
            IntPtr pointer = handle.AddrOfPinnedObject();
            vertexBuffer = new VulkanBuffer<Vertex>((void*)pointer, (uint)vertices.Count(), MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, true);
        }

        void CreateIndexBuffer()
        {
            GCHandle handle = GCHandle.Alloc(indices, GCHandleType.Pinned);
            IntPtr pointer = handle.AddrOfPinnedObject();
            indexBuffer = new VulkanBuffer<ushort>((void*)pointer, (uint)indices.Count(), MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, true);
        }

        void CreateUniformBuffers()
        {
            uint bufferSize = (uint)sizeof(UniformBufferObject);

            uniformBuffers = new BufferData[MAX_FRAMES_IN_FLIGHT];

            for (int i = 0; i < MAX_FRAMES_IN_FLIGHT; i++)
            {
                uniformBuffers[i] = new BufferData(physicalDevice, device, bufferSize,
                    BufferUsageFlags.BufferUsageUniformBufferBit);
            }
        }

        void CreateDescriptorSetLayout()
        {
            descriptorSetLayout = SU.MakeDescriptorSetLayout(device, new[]{
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
            descriptorSets = new VkDescriptorSets(device, allocInfo).ToArray();

            for (int i = 0; i < MAX_FRAMES_IN_FLIGHT; i++)
            {
                SU.UpdateDescriptorSets(device, descriptorSets[i], new[]{
                    (DescriptorType.UniformBuffer,uniformBuffers[i].buffer,(ulong)sizeof(UniformBufferObject), (VkBufferView)null)},
                    texture);
            }
        }

        void CreateCommandBuffer()
        {
            CommandBufferAllocateInfo allocInfo = new
            (
            commandPool: commandPool,
                level: CommandBufferLevel.Primary,
                commandBufferCount: MAX_FRAMES_IN_FLIGHT
            );

            commandBuffers = new VkCommandBuffers(device, allocInfo).ToArray();
        }

        void RecordCommandBuffer(VkCommandBuffer commandBuffer, uint imageIndex)
        {
            commandBuffer.Begin(new(flags: 0));

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

            commandBuffer.BeginRenderPass(renderPassInfo, SubpassContents.Inline);
            commandBuffer.BindPipeline(PipelineBindPoint.Graphics, graphicsPipeline);
            commandBuffer.BindVertexBuffers(vertexBuffer._bufferHandle, 0);
            commandBuffer.BindIndexBuffer(indexBuffer._bufferHandle, 0, IndexType.Uint16);
            commandBuffer.BindDescriptorSets(PipelineBindPoint.Graphics, pipelineLayout, descriptorSets[(int)currentFrame]);
            commandBuffer.SetViewport(new Viewport(0.0f, 0.0f, extent.Width, extent.Height, 0.0f, 1.0f));
            commandBuffer.SetScissor(new Rect2D(new Offset2D(0, 0), extent));
            commandBuffer.DrawIndexed((uint)indices.Length, 1, 0, 0, 0);
            commandBuffer.EndRenderPass();
            commandBuffer.End();
        }

        void CreateSyncObjects()
        {
            imageAvailableSemaphores = new Semaphore[MAX_FRAMES_IN_FLIGHT];
            renderFinishedSemaphores = new Semaphore[MAX_FRAMES_IN_FLIGHT];
            inFlightFences = new Fence[MAX_FRAMES_IN_FLIGHT];

            for (int i = 0; i < MAX_FRAMES_IN_FLIGHT; i++)
            {
                var info = new SemaphoreCreateInfo(flags: 0);
                var fenceInfo = new FenceCreateInfo(flags: FenceCreateFlags.FenceCreateSignaledBit);
                vk.CreateSemaphore(device, &info, null, out Semaphore imageAvailable);
                imageAvailableSemaphores[i] = imageAvailable;

                vk.CreateSemaphore(device, &info, null, out Semaphore renderFinished);
                renderFinishedSemaphores[i] = renderFinished;

                vk.CreateFence(device, &fenceInfo, null, out Fence fence);
                inFlightFences[i] = fence;
            }
        }

        public (uint imageIndex, Result result) AquireNextImage(ulong timeout, in Semaphore semaphore, Fence fence)
        {
            uint imageIndex = 0;


            var result = khrSwapchain.AcquireNextImage(device, swapChain, timeout, semaphore, fence,
                ref imageIndex);

            return (imageIndex, result);
        }

        public void DrawFrame()
        {
            WaitForFences(inFlightFences[currentFrame], ulong.MaxValue);

            (uint imageIndex, Result result) = AquireNextImage(ulong.MaxValue,
                imageAvailableSemaphores[currentFrame], new Fence(null));

            if (result == Result.ErrorOutOfDateKhr || result == Result.SuboptimalKhr || isFramebufferResized)
            {
                isFramebufferResized = false;
                RecreateSwapChain();
                return;
            }
            else if (result != Result.Success)
            {
                throw new Exception("failed to acquire swap chain image!");
            }

            ResetFences(inFlightFences[currentFrame]);

            commandBuffers[currentFrame].Reset(0);

            RecordCommandBuffer(commandBuffers[currentFrame], imageIndex);

            UpdateUniformBuffer(imageIndex);

            Semaphore waitSemaphore = imageAvailableSemaphores[currentFrame];
            PipelineStageFlags waitStages = PipelineStageFlags.PipelineStageColorAttachmentOutputBit;
            Semaphore signalSemaphore = renderFinishedSemaphores[currentFrame];

            CommandBuffer commandBuffer = commandBuffers[currentFrame];

            SubmitInfo submitInfo = new
            (
                waitSemaphoreCount: 1,
                pWaitSemaphores: &waitSemaphore,
                pWaitDstStageMask: &waitStages,
                commandBufferCount: 1,
                pCommandBuffers: &commandBuffer,
                signalSemaphoreCount: 1,
                pSignalSemaphores: &signalSemaphore
            );

            Submit(graphicsQueue, submitInfo, inFlightFences[currentFrame]);

            currentFrame = (currentFrame + 1) % MAX_FRAMES_IN_FLIGHT;

            SwapchainKHR swapChain = SilkVulkanRenderer.swapChain.swapChain;

            PresentInfoKHR presentInfo = new
            (
                waitSemaphoreCount: 1,
                pWaitSemaphores: &signalSemaphore,
                swapchainCount: 1,
                pSwapchains: &swapChain,
                pImageIndices: &imageIndex,
                pResults: null
            );

            PresentKHR(graphicsQueue, khrSwapchain, presentInfo);
        }

        void UpdateUniformBuffer(uint currentImage)
        {
            float secondsPassed = (float)TimeSpan.FromTicks(DateTime.Now.Ticks - startTime).TotalSeconds;

            UniformBufferObject ubo = new();

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

            uniformBuffers[currentFrame].Upload(ubo);

        }

        void RecreateSwapChain()
        {
            while (window.Size.X == 0 || window.Size.Y == 0)
            {
                window.DoEvents();
            }

            WaitIdle();

            CleanupSwapChain();

            CreateSwapChain();
            CreateRenderPass();
            CreateGraphicsPipeline();
            CreateDepthResources();
            CreateFramebuffers();
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

        void Cleanup()
        {
            for (int i = 0; i < MAX_FRAMES_IN_FLIGHT; i++)
            {
                //imageAvailableSemaphores[i].Dispose();
                //renderFinishedSemaphores[i].Dispose();
                //inFlightFences[i].Dispose();             
            }

            //commandPool.Dispose();

            CleanupSwapChain();

            for (int i = 0; i < MAX_FRAMES_IN_FLIGHT; i++)
            {
                uniformBuffers[i].Dispose();
            }

            //textureData.Dispose();

            //descriptorPool.Dispose();
            //descriptorSetLayout.Dispose();

            indexBuffer.Dispose();
            vertexBuffer.Dispose();

            // device.Dispose();

            //surfaceData.Dispose();
            // debugMessenger.Dispose();
            // instance.Dispose();

            window.Close();
            window.Dispose();
        }
    }
}

