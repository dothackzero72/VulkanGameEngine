using AutoMapper.Features;
using GlmSharp;
using Newtonsoft.Json;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanCS;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Systems;


namespace VulkanGameEngineLevelEditor.Systems
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct RenderPipelineJsonLoader
    {
        public String VertexShader { get; set; }
        public String FragmentShader { get; set; }
        public size_t DescriptorSetCount { get; set; }
        public size_t DescriptorSetLayoutCount { get; set; }
        public VertexTypeEnum VertexType { get; set; }
        public ListPtr<VkViewport> ViewportList { get; set; } = new ListPtr<VkViewport>();
        public ListPtr<VkRect2D> ScissorList { get; set; } = new ListPtr<VkRect2D>();
        public ListPtr<VkPipelineColorBlendAttachmentState> PipelineColorBlendAttachmentStateList { get; set; } = new ListPtr<VkPipelineColorBlendAttachmentState>();
        public VkPipelineColorBlendStateCreateInfoModel PipelineColorBlendStateCreateInfoModel { get; set; } = new VkPipelineColorBlendStateCreateInfoModel();
        public VkPipelineRasterizationStateCreateInfoModel PipelineRasterizationStateCreateInfo { get; set; } = new VkPipelineRasterizationStateCreateInfoModel();
        public VkPipelineMultisampleStateCreateInfoModel PipelineMultisampleStateCreateInfo { get; set; } = new VkPipelineMultisampleStateCreateInfoModel();
        public VkPipelineDepthStencilStateCreateInfoModel PipelineDepthStencilStateCreateInfo { get; set; }
        public VkPipelineInputAssemblyStateCreateInfoModel PipelineInputAssemblyStateCreateInfo { get; set; } = new VkPipelineInputAssemblyStateCreateInfoModel();
        public ListPtr<VkDescriptorSetLayoutBindingModel> LayoutBindingList { get; set; } = new ListPtr<VkDescriptorSetLayoutBindingModel>();
        public ListPtr<PipelineDescriptorModel> PipelineDescriptorModelsList { get; set; } = new ListPtr<PipelineDescriptorModel>();
        public ListPtr<VkVertexInputBindingDescription> VertexInputBindingDescriptionList { get; set; } = new ListPtr<VkVertexInputBindingDescription>();
        public ListPtr<VkVertexInputAttributeDescription> VertexInputAttributeDescriptionList { get; set; } = new ListPtr<VkVertexInputAttributeDescription>();
        public ListPtr<VkClearValue> ClearValueList { get; set; } = new ListPtr<VkClearValue>();

        public RenderPipelineJsonLoader()
        {
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct RenderPipelineLoader
    {
        public string VertexShaderPath;
        public string FragmentShaderPath;
        public VertexTypeEnum VertexType;
        public size_t ViewportListCount;
        public size_t ScissorListCount;
        public size_t DescriptorSetCount;
        public size_t DescriptorSetLayoutCount;
        public size_t LayoutBindingListCount;
        public size_t PipelineDescriptorModelsListCount;
        public size_t PipelineColorBlendAttachmentStateListCount;
        public size_t VertexInputBindingDescriptionListCount;
        public size_t VertexInputAttributeDescriptionListCount;
        public VkViewport* ViewportList;
        public VkRect2D* ScissorList;
        public VkPipelineColorBlendStateCreateInfo PipelineColorBlendStateCreateInfoModel;
        public VkPipelineRasterizationStateCreateInfo PipelineRasterizationStateCreateInfo;
        public VkPipelineMultisampleStateCreateInfo PipelineMultisampleStateCreateInfo;
        public VkPipelineDepthStencilStateCreateInfo PipelineDepthStencilStateCreateInfo;
        public VkPipelineInputAssemblyStateCreateInfo PipelineInputAssemblyStateCreateInfo;
        public VkDescriptorSetLayoutBinding* LayoutBindingList;
        public PipelineDescriptorModel* PipelineDescriptorModelsList;
        public VkPipelineColorBlendAttachmentState* PipelineColorBlendAttachmentStateList;
        public VkVertexInputBindingDescription* VertexInputBindingDescriptionList;
        public VkVertexInputAttributeDescription* VertexInputAttributeDescriptionList;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct TextureJsonLoader
    {
        public string TextureFilePath { get; set; }
        public Guid TextureId { get; set; }
        public VkFormat TextureByteFormat { get; set; }
        public VkImageAspectFlagBits ImageType { get; set; }
        public TextureTypeEnum TextureType { get; set; }
        public bool UseMipMaps { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct GraphicsRenderer
    {
        public VkInstance Instance { get; set; }
        public VkDevice Device { get; set; }
        public VkPhysicalDevice PhysicalDevice { get; set; }
        public VkSurfaceKHR Surface { get; set; }
        public VkCommandPool CommandPool { get; set; }
        public VkDebugUtilsMessengerEXT DebugMessenger { get; set; }

        public VkFence* InFlightFences { get; set; }
        public VkSemaphore* AcquireImageSemaphores { get; set; }
        public VkSemaphore* PresentImageSemaphores { get; set; }
        public VkImage* SwapChainImages { get; set; }
        public VkImageView* SwapChainImageViews { get; set; }
        public VkExtent2D SwapChainResolution { get; set; }
        public VkSwapchainKHR Swapchain { get; set; }

        public size_t SwapChainImageCount { get; set; }
        public size_t ImageIndex { get; set; }
        public size_t CommandIndex { get; set; }
        public uint GraphicsFamily { get; set; }
        public uint PresentFamily { get; set; }

        public VkQueue GraphicsQueue { get; set; }
        public VkQueue PresentQueue { get; set; }
        public VkFormat Format { get; set; }
        public VkColorSpaceKHR ColorSpace { get; set; }
        public VkPresentModeKHR PresentMode { get; set; }

        public bool RebuildRendererFlag { get; set; }
    }


    public unsafe struct RenderPipelineDLL
    {

        public String VertexShader { get; set; }
        public String FragmentShader { get; set; }
        public size_t DescriptorSetCount { get; set; }
        public size_t DescriptorSetLayoutCount { get; set; }
        public VertexTypeEnum VertexType { get; set; }
        public List<VkViewport> ViewportList { get; set; } = new List<VkViewport>();
        public List<VkRect2D> ScissorList { get; set; } = new List<VkRect2D>();
        public List<VkPipelineColorBlendAttachmentState> PipelineColorBlendAttachmentStateList { get; set; } = new List<VkPipelineColorBlendAttachmentState>();
        public VkPipelineColorBlendStateCreateInfoModel PipelineColorBlendStateCreateInfoModel { get; set; } = new VkPipelineColorBlendStateCreateInfoModel();
        public VkPipelineRasterizationStateCreateInfoModel PipelineRasterizationStateCreateInfo { get; set; } = new VkPipelineRasterizationStateCreateInfoModel();
        public VkPipelineMultisampleStateCreateInfoModel PipelineMultisampleStateCreateInfo { get; set; } = new VkPipelineMultisampleStateCreateInfoModel();
        public VkPipelineDepthStencilStateCreateInfoModel PipelineDepthStencilStateCreateInfo { get; set; }
        public VkPipelineInputAssemblyStateCreateInfoModel PipelineInputAssemblyStateCreateInfo { get; set; } = new VkPipelineInputAssemblyStateCreateInfoModel();
        public List<VkDescriptorSetLayoutBindingModel> LayoutBindingList { get; set; } = new List<VkDescriptorSetLayoutBindingModel>();
        //  public List<PipelineDescriptorModel> PipelineDescriptorModelsList { get; set; } = new List<PipelineDescriptorModel>();
        public List<VkVertexInputBindingDescription> VertexInputBindingDescriptionList { get; set; } = new List<VkVertexInputBindingDescription>();
        public List<VkVertexInputAttributeDescription> VertexInputAttributeDescriptionList { get; set; } = new List<VkVertexInputAttributeDescription>();
        public List<VkClearValue> ClearValueList { get; set; } = new List<VkClearValue>();
        public RenderPipelineDLL()
        {
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct VulkanPipeline
    {
        public uint RenderPipelineId { get; set; } = 0;
        public size_t DescriptorSetLayoutCount { get; set; } = 0;
        public size_t DescriptorSetCount { get; set; } = 0;
        public VkDescriptorPool DescriptorPool { get; set; } = VulkanCSConst.VK_NULL_HANDLE;
        public VkDescriptorSetLayout* DescriptorSetLayoutList { get; set; } = null;
        public VkDescriptorSet* DescriptorSetList { get; set; } = null;
        public VkPipeline Pipeline { get; set; } = VulkanCSConst.VK_NULL_HANDLE;
        public VkPipelineLayout PipelineLayout { get; set; } = VulkanCSConst.VK_NULL_HANDLE;
        public VkPipelineCache PipelineCache { get; set; } = VulkanCSConst.VK_NULL_HANDLE;
        public VulkanPipeline()
        {
        }

    };

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct VulkanRenderPass
    {

        public Guid RenderPassId { get; set; } = Guid.Empty;
        public VkSampleCountFlagBits SampleCount { get; set; } = VkSampleCountFlagBits.VK_SAMPLE_COUNT_FLAG_BITS_MAX_ENUM;
        public VkRect2D RenderArea { get; set; } = new VkRect2D();
        public VkRenderPass RenderPass { get; set; } = new VkRenderPass();
        public VkFramebuffer* FrameBufferList { get; set; } = null;
        public VkClearValue* ClearValueList { get; set; } = null;
        public size_t FrameBufferCount { get; set; } = 0;
        public size_t ClearValueCount { get; set; } = 0;
        public VkCommandBuffer CommandBuffer { get; set; }
        public bool UseFrameBufferResolution { get; set; }
        public VulkanRenderPass()
        {
        }
    };

    //public unsafe struct ImGuiRenderer
    //{
    //    public VkRenderPass RenderPass = VK_NULL_HANDLE;
    //    public VkDescriptorPool ImGuiDescriptorPool = VK_NULL_HANDLE;
    //    public VkCommandBuffer ImGuiCommandBuffer = VK_NULL_HANDLE;
    //    public ListPtr<VkFramebuffer> SwapChainFramebuffers;
    //};

    public unsafe static class RenderSystem
    {
        public static GraphicsRenderer renderer { get; set; }
        public static Dictionary<Guid, VulkanRenderPass> RenderPassList { get; set; } = new Dictionary<Guid, VulkanRenderPass>();
        public static Dictionary<Guid, ListPtr<VulkanPipeline>> RenderPipelineMap { get; set; } = new Dictionary<Guid, ListPtr<VulkanPipeline>>();
        public static VkCommandBufferBeginInfo CommandBufferBeginInfo { get; set; } = new VkCommandBufferBeginInfo();
        public static bool RebuildRendererFlag { get; set; }

        public static void CreateVulkanRenderer(WindowType windowType, IntPtr window, IntPtr renderAreaHandle)
        {
            renderer = Renderer_RendererSetUp(windowType, renderAreaHandle.ToPointer());
        }

        public static void Update(float deltaTime)
        {
            if (RebuildRendererFlag)
            {
                uint width = renderer.SwapChainResolution.width;
                uint height = renderer.SwapChainResolution.height;
                RecreateSwapchain();
                RebuildRendererFlag = false;
            }
        }

        public static void RecreateSwapchain()
        {
            /*  int width = 0;
              int height = 0;

              vkDeviceWaitIdle(*Device.get());

              vulkanWindow->GetFrameBufferSize(vulkanWindow, &width, &height);
              renderer.DestroySwapChainImageView();
              renderer.DestroySwapChain();
              renderer.SetUpSwapChain();

              RenderPassID id;
              id.id = 2;

              RenderPassList[id].RecreateSwapchain(width, height);*/
        }

        public static VkCommandBuffer RenderFrameBuffer(Guid renderPassId)
        {
             VulkanRenderPass renderPass = RenderPassList[renderPassId];
             VulkanPipeline pipeline = RenderPipelineMap[renderPassId][0];
             VkCommandBuffer commandBuffer = renderPass.CommandBuffer;

            VkRenderPassBeginInfo renderPassBeginInfo = new VkRenderPassBeginInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
                renderPass = renderPass.RenderPass,
                framebuffer = renderPass.FrameBufferList[(uint)renderer.ImageIndex],
                renderArea = renderPass.RenderArea,
                clearValueCount = renderPass.ClearValueCount,
                pClearValues = renderPass.ClearValueList
            };

            var beginCommandbufferindo = CommandBufferBeginInfo;
            VkFunc.vkResetCommandBuffer(commandBuffer, 0);
            VkFunc.vkBeginCommandBuffer(commandBuffer, &beginCommandbufferindo);
            VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassBeginInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
            VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline.Pipeline);
            VkFunc.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline.PipelineLayout, 0, (uint)pipeline.DescriptorSetCount, pipeline.DescriptorSetList, 0, null);
            VkFunc.vkCmdDraw(commandBuffer, 6, 1, 0, 0);
            VkFunc.vkCmdEndRenderPass(commandBuffer);
            VkFunc.vkEndCommandBuffer(commandBuffer);
            return commandBuffer;
        }

        public static VkCommandBuffer RenderLevel(Guid renderPassId, Guid levelId, float deltaTime, SceneDataBuffer sceneDataBuffer)
        {
            VulkanRenderPass renderPass = RenderPassList[renderPassId];
            VulkanPipeline spritePipeline = RenderPipelineMap[renderPassId][0];
            VulkanPipeline levelPipeline = RenderPipelineMap[renderPassId][1];
            ListPtr<VkClearValue> clearColorValues = new ListPtr<VkClearValue>(renderPass.ClearValueList, renderPass.ClearValueCount);
            VkCommandBuffer commandBuffer = renderPass.CommandBuffer;

            VkRenderPassBeginInfo renderPassBeginInfo = new VkRenderPassBeginInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
                renderPass = renderPass.RenderPass,
                framebuffer = renderPass.FrameBufferList[(int)renderer.ImageIndex],
                renderArea = renderPass.RenderArea,
                clearValueCount = clearColorValues.Count,
                pClearValues = clearColorValues.Ptr
            };

            var beginCommandbufferinfo = CommandBufferBeginInfo;
            VkFunc.vkResetCommandBuffer(commandBuffer, 0);
            VkFunc.vkBeginCommandBuffer(commandBuffer, &beginCommandbufferinfo);
            VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassBeginInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
            VkFunc.vkCmdEndRenderPass(commandBuffer);
            VkFunc.vkEndCommandBuffer(commandBuffer);
            return commandBuffer;
        }

        public static Guid LoadRenderPass(Guid levelId, string jsonPath, ivec2 renderPassResolution)
        {
            string jsonContent = File.ReadAllText(jsonPath);
            RenderPassBuildInfoModel model = JsonConvert.DeserializeObject<RenderPassBuildInfoModel>(jsonContent);

            VkExtent2D extent2D = new VkExtent2D
            {
                width = (uint)renderPassResolution.x,
                height = (uint)renderPassResolution.y
            };

            size_t renderedTextureCount = model.RenderedTextureInfoModelList.Where(x => x.TextureType == RenderedTextureType.ColorRenderedTexture).Count();
            ListPtr<Texture> renderedTextureListPtr = new ListPtr<Texture>(renderedTextureCount);
            VulkanRenderPass vulkanRenderPass = VulkanRenderPass_CreateVulkanRenderPass(RenderSystem.renderer, jsonPath, ref extent2D, sizeof(SceneDataBuffer), renderedTextureListPtr.Ptr, ref renderedTextureCount, out Texture depthTexture);
            RenderPassList[vulkanRenderPass.RenderPassId] = vulkanRenderPass;

            TextureSystem.RenderedTextureList[vulkanRenderPass.RenderPassId] = new ListPtr<Texture>();
            for (int x = 0; x < renderedTextureCount; x++)
            {
                TextureSystem.RenderedTextureList[vulkanRenderPass.RenderPassId].Add(renderedTextureListPtr[x]);
            }
            if (depthTexture.textureView != VulkanCSConst.VK_NULL_HANDLE)
            {
                TextureSystem.DepthTextureList[vulkanRenderPass.RenderPassId] = depthTexture;
            }

            ListPtr<VulkanPipeline> vulkanPipelineList = new ListPtr<VulkanPipeline>();
            foreach (var pipelineId in model.RenderPipelineList)
            {
                string fulRenderPassPath = Path.GetFullPath("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\pipelines\\" + pipelineId);

                ListPtr<VkDescriptorBufferInfo> vertexPropertiesList = RenderSystem.GetVertexPropertiesBuffer();
                ListPtr<VkDescriptorBufferInfo> indexPropertiesList = RenderSystem.GetIndexPropertiesBuffer();
                ListPtr<VkDescriptorBufferInfo> transformPropertiesList = RenderSystem.GetGameObjectTransformBuffer();
                ListPtr<VkDescriptorBufferInfo> meshPropertiesList = RenderSystem.GetMeshPropertiesBuffer(levelId);
                //  ListPtr<VkDescriptorBufferInfo> levelLayerMeshPropertiesList = Vector<VkDescriptorBufferInfo>(includes.LevelLayerMeshProperties, includes.LevelLayerMeshProperties + includes.LevelLayerMeshPropertiesCount);
                ListPtr<VkDescriptorImageInfo> texturePropertiesList = RenderSystem.GetTexturePropertiesBuffer(vulkanRenderPass.RenderPassId, null);
                ListPtr<VkDescriptorBufferInfo> materialPropertiesList = MaterialSystem.GetMaterialPropertiesBuffer();
               
                GPUIncludes include = new GPUIncludes
                {
                    vertexPropertiesList = vertexPropertiesList.Ptr,
                    indexPropertiesList = indexPropertiesList.Ptr,
                    transformPropertiesList = transformPropertiesList.Ptr,
                    meshPropertiesList = meshPropertiesList.Ptr,
                    texturePropertiesList = texturePropertiesList.Ptr,
                    materialPropertiesList = materialPropertiesList.Ptr,
                    vertexPropertiesListCount = vertexPropertiesList.Count,
                    indexPropertiesListCount = indexPropertiesList.Count,
                    transformPropertiesListCount = transformPropertiesList.Count,
                    meshPropertiesListCount = meshPropertiesList.Count,
                    texturePropertiesListCount = texturePropertiesList.Count,
                    materialPropertiesListCount = materialPropertiesList.Count
                };

                var pipeLineId = (uint)RenderPipelineMap.Count;
                var renderPassId = vulkanRenderPass.RenderPassId;
                VulkanPipeline vulkanPipelineDLL = VulkanPipeline_CreateRenderPipeline(renderer.Device, ref renderPassId, pipeLineId, fulRenderPassPath, RenderPassList[vulkanRenderPass.RenderPassId].RenderPass, (uint)sizeof(SceneDataBuffer), ref renderPassResolution, include);
                vulkanPipelineList.Add(vulkanPipelineDLL);
            }
            RenderPipelineMap[vulkanRenderPass.RenderPassId] = vulkanPipelineList;
            return vulkanRenderPass.RenderPassId;
        }

        public static Guid LoadRenderPass(Guid levelId, string jsonPath, Texture inputTexture, ivec2 renderPassResolution)
        {
            string jsonContent = File.ReadAllText(jsonPath);
            RenderPassBuildInfoModel model = JsonConvert.DeserializeObject<RenderPassBuildInfoModel>(jsonContent);

            VkExtent2D extent2D = new VkExtent2D
            {
                width = (uint)renderPassResolution.x,
                height = (uint)renderPassResolution.y
            };

            size_t renderedTextureCount = model.RenderedTextureInfoModelList.Where(x => x.TextureType == RenderedTextureType.ColorRenderedTexture).Count();
            ListPtr<Texture> renderedTextureListPtr = new ListPtr<Texture>(renderedTextureCount);
            VulkanRenderPass vulkanRenderPass = VulkanRenderPass_CreateVulkanRenderPass(RenderSystem.renderer, jsonPath, ref extent2D, sizeof(SceneDataBuffer), renderedTextureListPtr.Ptr, ref renderedTextureCount, out Texture depthTexture);
            RenderPassList[vulkanRenderPass.RenderPassId] = vulkanRenderPass;

            TextureSystem.RenderedTextureList[vulkanRenderPass.RenderPassId] = new ListPtr<Texture>();
            for (int x = 0; x < renderedTextureCount; x++)
            {
                TextureSystem.RenderedTextureList[vulkanRenderPass.RenderPassId].Add(renderedTextureListPtr[x]);
            }
            if (depthTexture.textureView != VulkanCSConst.VK_NULL_HANDLE)
            {
                TextureSystem.DepthTextureList[vulkanRenderPass.RenderPassId] = depthTexture;
            }

            ListPtr<VulkanPipeline> vulkanPipelineList = new ListPtr<VulkanPipeline>();
            foreach (var pipelineId in model.RenderPipelineList)
            {
                string fulRenderPassPath = Path.GetFullPath("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\pipelines\\" + pipelineId);

                ListPtr<VkDescriptorBufferInfo> vertexPropertiesList = RenderSystem.GetVertexPropertiesBuffer();
                ListPtr<VkDescriptorBufferInfo> indexPropertiesList = RenderSystem.GetIndexPropertiesBuffer();
                ListPtr<VkDescriptorBufferInfo> transformPropertiesList = RenderSystem.GetGameObjectTransformBuffer();
                ListPtr<VkDescriptorBufferInfo> meshPropertiesList = RenderSystem.GetMeshPropertiesBuffer(levelId);
                //  ListPtr<VkDescriptorBufferInfo> levelLayerMeshPropertiesList = Vector<VkDescriptorBufferInfo>(includes.LevelLayerMeshProperties, includes.LevelLayerMeshProperties + includes.LevelLayerMeshPropertiesCount);
                ListPtr<VkDescriptorImageInfo> texturePropertiesList = RenderSystem.GetTexturePropertiesBuffer(vulkanRenderPass.RenderPassId, &inputTexture);
                ListPtr<VkDescriptorBufferInfo> materialPropertiesList = MaterialSystem.GetMaterialPropertiesBuffer();

                GPUIncludes include = new GPUIncludes
                {
                    vertexPropertiesList = vertexPropertiesList.Ptr,
                    indexPropertiesList = indexPropertiesList.Ptr,
                    transformPropertiesList = transformPropertiesList.Ptr,
                    meshPropertiesList = meshPropertiesList.Ptr,
                    texturePropertiesList = texturePropertiesList.Ptr,
                    materialPropertiesList = materialPropertiesList.Ptr,
                    vertexPropertiesListCount = vertexPropertiesList.Count,
                    indexPropertiesListCount = indexPropertiesList.Count,
                    transformPropertiesListCount = transformPropertiesList.Count,
                    meshPropertiesListCount = meshPropertiesList.Count,
                    texturePropertiesListCount = texturePropertiesList.Count,
                    materialPropertiesListCount = materialPropertiesList.Count
                };

                var pipeLineId = (uint)RenderPipelineMap.Count;
                var renderPassId = vulkanRenderPass.RenderPassId;
                VulkanPipeline vulkanPipelineDLL = VulkanPipeline_CreateRenderPipeline(renderer.Device, ref renderPassId, pipeLineId, fulRenderPassPath, RenderPassList[vulkanRenderPass.RenderPassId].RenderPass, (uint)sizeof(SceneDataBuffer), ref renderPassResolution, include);
                vulkanPipelineList.Add(vulkanPipelineDLL);
            }
            RenderPipelineMap[vulkanRenderPass.RenderPassId] = vulkanPipelineList;
            return vulkanRenderPass.RenderPassId;
        }

        public static VkResult StartFrame()
        {

           var imageIndex = renderer.ImageIndex;
            var commandIndex = renderer.CommandIndex;
            var rebuildRendererFlag = renderer.RebuildRendererFlag;
            var result = Renderer_StartFrame(renderer.Device, renderer.Swapchain, renderer.InFlightFences, renderer.AcquireImageSemaphores, &imageIndex, &commandIndex, &rebuildRendererFlag);

            var rendererTemp = renderer;
            rendererTemp.ImageIndex = imageIndex;
            rendererTemp.CommandIndex = commandIndex;
            rendererTemp.RebuildRendererFlag = rebuildRendererFlag;
            renderer = rendererTemp;
            return result;
        }

        public static unsafe VkResult EndFrame(ListPtr<VkCommandBuffer> commandBufferSubmitList)
        {
            VkSwapchainKHR swapChain = renderer.Swapchain;
            VkSemaphore* acquireImageSemaphoreList = renderer.AcquireImageSemaphores;
            VkSemaphore* presentImageSemaphoreList = renderer.PresentImageSemaphores;
            VkFence* fenceList = renderer.InFlightFences;
            VkQueue graphicsQueue = renderer.GraphicsQueue;
            VkQueue presentQueue = renderer.PresentQueue;
            size_t commandIndex = renderer.CommandIndex;
            size_t imageIndex = renderer.ImageIndex;
            VkCommandBuffer* pCommandBufferSubmitList = commandBufferSubmitList.Ptr;
            size_t commandBufferCount = commandBufferSubmitList.Count;
            bool rebuildRendererFlag = renderer.RebuildRendererFlag;
            var result = Renderer_EndFrame(swapChain, acquireImageSemaphoreList, presentImageSemaphoreList, fenceList, graphicsQueue, presentQueue, commandIndex, imageIndex, pCommandBufferSubmitList, commandBufferCount, &rebuildRendererFlag);
            return result;
        }

        public static ListPtr<VkDescriptorBufferInfo> GetVertexPropertiesBuffer()
        {
            //Vector<MeshStruct> meshList;
            //meshList.reserve(meshSystem.SpriteMeshList.size());
            //std::transform(meshSystem.SpriteMeshList.begin(), meshSystem.SpriteMeshList.end(),
            //    std::back_inserter(meshList),
            //    [](const auto& pair) { return pair.second; });


            ListPtr<VkDescriptorBufferInfo> vertexPropertiesBuffer = new ListPtr<VkDescriptorBufferInfo>();
            //if (meshList.empty())
            //{
            //    vertexPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
            //        {
            //            .buffer = VK_NULL_HANDLE,
            //            .offset = 0,
            //            .range = VK_WHOLE_SIZE
            //        });
            //}
            //else
            //{
            //    for (auto& mesh : meshList)
            //    {
            //        const VulkanBufferStruct& vertexProperties = bufferSystem.VulkanBuffer[mesh.MeshVertexBufferId];
            //        vertexPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
            //            {
            //                .buffer = vertexProperties.Buffer,
            //                .offset = 0,
            //                .range = VK_WHOLE_SIZE
            //            });
            //    }
            //}

            return vertexPropertiesBuffer;
        }

        public static ListPtr<VkDescriptorBufferInfo> GetIndexPropertiesBuffer()
        {
            //Vector<MeshStruct> meshList;
            //meshList.reserve(meshSystem.SpriteMeshList.size());
            //std::transform(meshSystem.SpriteMeshList.begin(), meshSystem.SpriteMeshList.end(),
            //    std::back_inserter(meshList),
            //    [](const auto& pair) { return pair.second; });

            ListPtr<VkDescriptorBufferInfo> indexPropertiesBuffer = new ListPtr<VkDescriptorBufferInfo>();
            //if (meshList.empty())
            //{
            //    indexPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
            //        {
            //            .buffer = VK_NULL_HANDLE,
            //            .offset = 0,
            //            .range = VK_WHOLE_SIZE
            //        });
            //}
            //else
            //{
            //    for (auto& mesh : meshList)
            //    {
            //        const VulkanBufferStruct& indexProperties = bufferSystem.VulkanBuffer[mesh.MeshIndexBufferId];
            //        indexPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
            //            {
            //                .buffer = indexProperties.Buffer,
            //                .offset = 0,
            //                .range = VK_WHOLE_SIZE
            //            });
            //    }
            //}
            return indexPropertiesBuffer;
        }

        public static ListPtr<VkDescriptorBufferInfo> GetGameObjectTransformBuffer()
        {
            //Vector<MeshStruct> meshList;
            //meshList.reserve(meshSystem.SpriteMeshList.size());
            //std::transform(meshSystem.SpriteMeshList.begin(), meshSystem.SpriteMeshList.end(),
            //    std::back_inserter(meshList),
            //    [](const auto& pair) { return pair.second; });

            ListPtr<VkDescriptorBufferInfo> transformPropertiesBuffer = new ListPtr<VkDescriptorBufferInfo>();
            //if (meshList.empty())
            //{
            //    transformPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
            //        {
            //            .buffer = VK_NULL_HANDLE,
            //            .offset = 0,
            //            .range = VK_WHOLE_SIZE
            //        });
            //}
            //else
            //{
            //    for (auto& mesh : meshList)
            //    {
            //        const VulkanBufferStruct& transformBuffer = bufferSystem.VulkanBuffer[mesh.MeshTransformBufferId];
            //        transformPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
            //            {
            //                .buffer = transformBuffer.Buffer,
            //                .offset = 0,
            //                .range = VK_WHOLE_SIZE
            //            });
            //    }
            //}

            return transformPropertiesBuffer;
        }

        public static ListPtr<VkDescriptorBufferInfo> GetMeshPropertiesBuffer(Guid levelLayerId)
        {
            ListPtr<VkDescriptorBufferInfo> meshPropertiesBuffer = new ListPtr<VkDescriptorBufferInfo>();

                meshPropertiesBuffer.Add(new VkDescriptorBufferInfo
                    {
                        buffer = VulkanCSConst.VK_NULL_HANDLE,
                        offset = 0,
                        range = VulkanCSConst.VK_WHOLE_SIZE
                });
    
            return meshPropertiesBuffer;
        }


        public unsafe static ListPtr<VkDescriptorImageInfo> GetTexturePropertiesBuffer(Guid renderPassId, Texture* renderedTexture)
        {
            ListPtr<VkDescriptorImageInfo> texturePropertiesBuffer = new ListPtr<VkDescriptorImageInfo>();
            VkSamplerCreateInfo NullSamplerInfo = new VkSamplerCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
                magFilter = VkFilter.VK_FILTER_NEAREST,
                minFilter = VkFilter.VK_FILTER_NEAREST,
                mipmapMode = VkSamplerMipmapMode.VK_SAMPLER_MIPMAP_MODE_LINEAR,
                addressModeU = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
                addressModeV = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
                addressModeW = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
                mipLodBias = 0,
                anisotropyEnable = true,
                maxAnisotropy = 16.0f,
                compareEnable = false,
                compareOp = VkCompareOp.VK_COMPARE_OP_ALWAYS,
                minLod = 0,
                maxLod = 0,
                borderColor = VkBorderColor.VK_BORDER_COLOR_INT_OPAQUE_BLACK,
                unnormalizedCoordinates = false,
            };

            VkFunc.vkCreateSampler(renderer.Device, &NullSamplerInfo, null, out VkSampler nullSampler);
            VkDescriptorImageInfo nullBuffer = new VkDescriptorImageInfo()
            {
                sampler = nullSampler,
                imageView = VulkanCSConst.VK_NULL_HANDLE,
                imageLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED,
            };
            texturePropertiesBuffer.Add(nullBuffer);
            texturePropertiesBuffer.Add(nullBuffer);
            return texturePropertiesBuffer;
        }

        public static VkCommandBuffer BeginSingleTimeCommands()
        {
            return Renderer_BeginSingleTimeCommands(RenderSystem.renderer.Device, RenderSystem.renderer.CommandPool);
        }

        public static VkCommandBuffer BeginSingleTimeCommands(VkCommandPool commandPool)
        {
            return Renderer_BeginSingleTimeCommands(RenderSystem.renderer.Device, RenderSystem.renderer.CommandPool);
        }

        public static VkResult EndSingleTimeCommands(VkCommandBuffer commandBuffer)
        {
            return Renderer_EndSingleTimeCommands(RenderSystem.renderer.Device, RenderSystem.renderer.CommandPool, RenderSystem.renderer.GraphicsQueue, commandBuffer);
        }

        public static VkResult EndSingleTimeCommands(VkCommandBuffer commandBuffer, VkCommandPool commandPool)
        {
            return Renderer_EndSingleTimeCommands(RenderSystem.renderer.Device, commandPool, RenderSystem.renderer.GraphicsQueue, commandBuffer);
        }

        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern GraphicsRenderer Renderer_RendererSetUp(WindowType windowType, void* windowHandle);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult Renderer_StartFrame(VkDevice device, VkSwapchainKHR swapChain, VkFence* fenceList, VkSemaphore* acquireImageSemaphoreList, size_t* pImageIndex, size_t* pCommandIndex, bool* pRebuildRendererFlag);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult Renderer_EndFrame(VkSwapchainKHR swapChain, VkSemaphore* acquireImageSemaphoreList, VkSemaphore* presentImageSemaphoreList, VkFence* fenceList, VkQueue graphicsQueue, VkQueue presentQueue, size_t commandIndex, size_t imageIndex, VkCommandBuffer* pCommandBufferSubmitList, size_t commandBufferCount, bool* rebuildRendererFlag);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkCommandBuffer Renderer_BeginSingleTimeCommands(VkDevice device, VkCommandPool commandPool);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult Renderer_EndSingleTimeCommands(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkCommandBuffer commandBuffer);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VulkanRenderPass VulkanRenderPass_CreateVulkanRenderPass(GraphicsRenderer renderer, [MarshalAs(UnmanagedType.LPStr)] string renderPassLoader, ref VkExtent2D renderPassResolution, int ConstBuffer, Texture* renderedTextureListPtr, ref size_t renderedTextureCount, out Texture depthTexture);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void VulkanRenderPass_DestroyRenderPass(GraphicsRenderer renderer, VulkanRenderPass renderPass);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VulkanPipeline VulkanPipeline_CreateRenderPipeline(IntPtr device, ref Guid renderPassId, uint renderPipelineId, [MarshalAs(UnmanagedType.LPStr)] string pipelineJson, IntPtr renderPass, uint constBufferSize, ref ivec2 renderPassResolution, GPUIncludes includes);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void VulkanPipeline_Destroy(VkDevice device, VulkanPipeline vulkanPipelineDLL);
    }
}
