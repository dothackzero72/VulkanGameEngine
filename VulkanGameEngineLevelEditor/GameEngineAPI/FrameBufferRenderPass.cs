using GlmSharp;
using Newtonsoft.Json;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VulkanGameEngineGameObjectScripts.Vulkan;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class FrameBufferRenderPass
    {
        Vk vk = Vk.GetApi();

        ivec2 RenderPassResolution;
        SampleCountFlags SampleCount;

        VkRenderPass renderPass;
        VkPipeline pipeline;
        VkPipelineLayout pipelineLayout;
        VkPipelineCache pipelineCache;
        VkDescriptorPool descriptorPool;

        ListPtr<VkDescriptorSetLayout> descriptorSetLayoutList;
        ListPtr<VkDescriptorSet> descriptorSetList;
        ListPtr<VkCommandBuffer> commandBufferList = new ListPtr<VkCommandBuffer>();
        ListPtr<VkFramebuffer> frameBufferList;

        public List<RenderedTexture> RenderedColorTextureList { get; private set; } = new List<RenderedTexture>();
        public DepthTexture depthTexture { get; private set; } = new DepthTexture();

        public FrameBufferRenderPass()
        {

        }

        public void BuildRenderPass(String jsonFile, Texture texture)
        {
        //    SaveRenderPass();
          //  SavePipeline();

            RenderPassResolution = new ivec2((int)VulkanRenderer.SwapChain.SwapChainResolution.width, (int)VulkanRenderer.SwapChain.SwapChainResolution.height);
            SampleCount = SampleCountFlags.Count1Bit;

            //ListPtr<Texture> textureList = new ListPtr<Texture> { texture };
            //ListPtr<DepthTexture> depthTextureList = new ListPtr<DepthTexture>();

            RenderPassBuildInfoModel model = new RenderPassBuildInfoModel(jsonFile);

            renderPass = CreateRenderPass();
            frameBufferList = CreateFramebuffer();
            //GameEngineImport.DLL_RenderPass_BuildFrameBuffer(VulkanRenderer.device, renderPass, model, frameBufferList.Ptr, textureList.Ptr, depthTextureList.Ptr, VulkanRenderer.SwapChain.imageViews.Ptr, frameBufferList.UCount, textureList.UCount, RenderPassResolution);
            BuildRenderPipeline(texture);
            VulkanRenderer.CreateCommandBuffers(commandBufferList);

            // ListPtr<RenderedTexture> textureList = new ListPtr<Texture>((RenderedTexture)texture);
            // ListPtr<DepthTexture> depthTexture = new ListPtr<DepthTexture>();
            // List<Texture> texture4 = new List<Texture>();

            //// frameBufferList = new ListPtr<VkFramebuffer>(VulkanRenderer.SwapChain.ImageCount);

            

            // renderPass = CreateRenderPass();
            // GameEngineImport.DLL_RenderPass_BuildFrameBuffer(VulkanRenderer.device, renderPass, model, frameBufferList.Ptr, textureList.Ptr, depthTexture.Ptr, VulkanRenderer.SwapChain.imageViews.Ptr, frameBufferList.UCount, textureList.UCount, RenderPassResolution);

            // BuildRenderPipeline(texture);
            // VulkanRenderer.CreateCommandBuffers(commandBufferList);

            // //fixed (RenderedTexture* renderedColorTextureListPtr = RenderedColorTextureList)
            // //fixed (VkFramebuffer* frameBufferListPtr = FrameBufferList)
            // //fixed(VkImageView* swapChainImageView = VulkanRenderer.SwapChain.imageViews)
            // //{
            // //    GCHandle handle = GCHandle.Alloc(depthTexture, GCHandleType.Pinned);
            // //    DepthTexture* depthTexturePtr = (DepthTexture*)handle.AddrOfPinnedObject();


            // //    handle.Free();
            // //}
        }

        public VkRenderPass CreateRenderPass()
        {
            VkRenderPass tempRenderPass;
            List<VkAttachmentDescription> attachmentDescriptionList = new List<VkAttachmentDescription>()
            {
                new VkAttachmentDescription
                {
                   format = VkFormat.VK_FORMAT_R8G8B8A8_UNORM,
                    samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                    loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR,
                    storeOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_STORE,
                    stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE,
                    stencilStoreOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE,
                    initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED,
                    finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR
                }
            };

            VkImageLayout initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            VkImageLayout finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR;
            uint sdf = (uint)VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL;
            int wer = (int)VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL;


            List<VkAttachmentReference> colorRefsList = new List<VkAttachmentReference>()
            {
                new VkAttachmentReference
                {
                    attachment = 0,
                    layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
                }
            };

            List<VkAttachmentReference> multiSampleReferenceList = new List<VkAttachmentReference>();
            List<VkAttachmentReference> depthReference = new List<VkAttachmentReference>();

            List<VkSubpassDescription> subpassDescriptionList = new List<VkSubpassDescription>();
            fixed (VkAttachmentReference* colorRefs = colorRefsList.ToArray())
            {
                subpassDescriptionList.Add(
                    new VkSubpassDescription
                    {
                        pipelineBindPoint = VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS,
                        colorAttachmentCount = (uint)colorRefsList.Count,
                        pColorAttachments = colorRefs,
                        pResolveAttachments = null,
                        pDepthStencilAttachment = null
                    });
            }

            List<VkSubpassDependency> subpassDependencyList = new List<VkSubpassDependency>()
            {
                new VkSubpassDependency
                {
                    srcSubpass = uint.MaxValue,
                    dstSubpass = 0,
                    srcStageMask = VkPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                    dstStageMask = VkPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                    srcAccessMask = 0,
                    dstAccessMask = VkAccessFlagBits.VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT,
                }
            };

            fixed (VkAttachmentDescription* attachments = attachmentDescriptionList.ToArray())
            fixed (VkSubpassDescription* description = subpassDescriptionList.ToArray())
            fixed (VkSubpassDependency* dependency = subpassDependencyList.ToArray())
            {
                var renderPassCreateInfo = new VkRenderPassCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
                    pNext = null,
                    flags = 0,
                    attachmentCount = (uint)attachmentDescriptionList.Count(),
                    pAttachments = attachments,
                    subpassCount = (uint)subpassDescriptionList.Count(),
                    pSubpasses = description,
                    dependencyCount = (uint)subpassDependencyList.Count(),
                    pDependencies = dependency
                };

                VkFunc.vkCreateRenderPass(VulkanRenderer.device, &renderPassCreateInfo, null, &tempRenderPass);
                renderPass = tempRenderPass;
            }

            return renderPass;
        }

        public new ListPtr<VkFramebuffer> CreateFramebuffer()
        {
            ListPtr<VkFramebuffer> frameBufferList = new ListPtr<VkFramebuffer>(VulkanRenderer.SwapChain.ImageCount);
            for (int x = 0; x < (int)VulkanRenderer.SwapChain.ImageCount; x++)
            {
                var attachment = VulkanRenderer.SwapChain.imageViews[x];
                VkFramebufferCreateInfo framebufferInfo = new VkFramebufferCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
                    renderPass = renderPass,
                    attachmentCount = 1,
                    pAttachments = &attachment,
                    width = VulkanRenderer.SwapChain.SwapChainResolution.width,
                    height = VulkanRenderer.SwapChain.SwapChainResolution.height,
                    layers = 1
                };

                var frameBuffer = frameBufferList[x];
                VkFunc.vkCreateFramebuffer(VulkanRenderer.device, &framebufferInfo, null, &frameBuffer);
                frameBufferList[x] = frameBuffer;
            }

            return frameBufferList;
        }

        public unsafe void BuildRenderPipeline(Texture texture)
        {
            string jsonContent = File.ReadAllText("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Pipelines\\FrameBufferPipeline.json");
            RenderPipelineDLL nativeModel = JsonConvert.DeserializeObject<RenderPipelineModel>(jsonContent).ToDLL();

            uint descriptorSetCount = nativeModel.LayoutBindingListCount;
            descriptorSetLayoutList = new ListPtr<VkDescriptorSetLayout>(descriptorSetCount);
            descriptorSetList = new ListPtr<VkDescriptorSet>(descriptorSetCount);

            ListPtr<VkVertexInputBindingDescription> vertexBindingDescription = new ListPtr<VkVertexInputBindingDescription>();
            ListPtr<VkVertexInputAttributeDescription> vertexAttributeDescription = new ListPtr<VkVertexInputAttributeDescription>();
            
            var meshProperties = JsonPipeline.GetMeshPropertiesBuffer<NullVertex>(new List<Mesh<NullVertex>>());
            var textureProperties = new ListPtr<VkDescriptorImageInfo> { texture.GetTextureBuffer() };
            var materialProperties = JsonPipeline.GetMaterialPropertiesBuffer(new List<Material>());
            var vertexProperties = new ListPtr<VkDescriptorBufferInfo>();
            var indexProperties = new ListPtr<VkDescriptorBufferInfo>();
            var transformProperties = new ListPtr<VkDescriptorBufferInfo>();

            GPUIncludes includes = new GPUIncludes
            {
                vertexProperties = vertexProperties.Ptr,
                indexProperties = indexProperties.Ptr,
                transformProperties = transformProperties.Ptr,
                meshProperties = meshProperties.Ptr,
                texturePropertiesList = textureProperties.Ptr,
                materialProperties = materialProperties.Ptr,
                vertexPropertiesCount = (uint)vertexProperties.Count,
                indexPropertiesCount = (uint)indexProperties.Count,
                transformPropertiesCount = (uint)transformProperties.Count,
                meshPropertiesCount = (uint)meshProperties.Count,
                texturePropertiesListCount = (uint)textureProperties.Count,
                materialPropertiesCount = (uint)materialProperties.Count
            };

            descriptorPool = GameEngineImport.DLL_Pipeline_CreateDescriptorPool(VulkanRenderer.device, nativeModel, &includes);
            GameEngineImport.DLL_Pipeline_CreateDescriptorSetLayout(VulkanRenderer.device, nativeModel, includes, descriptorSetLayoutList.Ptr, descriptorSetCount);
            GameEngineImport.DLL_Pipeline_AllocateDescriptorSets(VulkanRenderer.device, descriptorPool, descriptorSetLayoutList.Ptr, descriptorSetList.Ptr, descriptorSetCount);
            GameEngineImport.DLL_Pipeline_UpdateDescriptorSets(VulkanRenderer.device, descriptorSetList.Ptr, nativeModel, includes, descriptorSetCount);
            GameEngineImport.DLL_Pipeline_CreatePipelineLayout(VulkanRenderer.device, descriptorSetLayoutList.Ptr, 0, out VkPipelineLayout pipelineLayoutPtr, descriptorSetCount);
            GameEngineImport.DLL_Pipeline_CreatePipeline(VulkanRenderer.device, renderPass, pipelineLayoutPtr, pipelineCache, nativeModel, vertexBindingDescription.Ptr, vertexAttributeDescription.Ptr, out VkPipeline pipelinePtr, 0, 0);

            pipelineLayout = pipelineLayoutPtr;
            pipeline = pipelinePtr;
        }

        public unsafe VkCommandBuffer Draw()
        {
            var commandIndex = VulkanRenderer.CommandIndex;
            var imageIndex = VulkanRenderer.ImageIndex;
            var commandBuffer = commandBufferList[(int)commandIndex];

            VkClearValue* clearValues = stackalloc[]
            {
                new VkClearValue(new VkClearColorValue(1, 1, 0, 1))
            };

            VkViewport viewport = new VkViewport
            {
                x = 0.0f,
                y = 0.0f,
                width = VulkanRenderer.SwapChain.SwapChainResolution.width,
                height = VulkanRenderer.SwapChain.SwapChainResolution.height,
                minDepth = 0.0f,
                maxDepth = 1.0f
            };

            VkRect2D scissor = new VkRect2D
            {
                offset = new VkOffset2D { x = 0, y = 0 },
                extent = VulkanRenderer.SwapChain.SwapChainResolution
            };

            VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
                renderPass = renderPass,
                renderArea = new VkRect2D(new VkOffset2D(0, 0), VulkanRenderer.SwapChain.SwapChainResolution),
                clearValueCount = 1,
                framebuffer = frameBufferList[(int)imageIndex],
                pClearValues = clearValues,
                pNext = IntPtr.Zero
            };

            VkCommandBufferBeginInfo commandInfo = new VkCommandBufferBeginInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
                flags = VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
            };

            VkFunc.vkBeginCommandBuffer(commandBuffer, &commandInfo);
            VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
            VkFunc.vkCmdSetViewport(commandBuffer, 0, 1, &viewport);
            VkFunc.vkCmdSetScissor(commandBuffer, 0, 1, &scissor);
            VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline);
            VkFunc.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipelineLayout, 0, 1, descriptorSetList.Ptr, 0, null);
            VkFunc.vkCmdDraw(commandBuffer, 6, 1, 0, 0);
            VkFunc.vkCmdEndRenderPass(commandBuffer);
            VkFunc.vkEndCommandBuffer(commandBuffer);

            return commandBuffer;

        }

        private void SaveRenderPass()
        {
            RenderPassBuildInfoModel modelInfo = new RenderPassBuildInfoModel()
            {
                //SubpassDependencyList = new List<VkSubpassDependencyModel>()
                //{
                //      new VkSubpassDependencyModel
                //    {
                //        srcSubpass = uint.MaxValue,
                //        dstSubpass = 0,
                //        srcStageMask = VkPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                //        dstStageMask = VkPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                //        srcAccessMask = 0,
                //        dstAccessMask = VkAccessFlagBits.VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT,
                //    },
                //},
                _name = "Default2DRenderPass"
            };

            string jsonString = JsonConvert.SerializeObject(modelInfo, Formatting.Indented);

            string finalfilePath = @"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\FrameBufferRenderPass.json";
            File.WriteAllText(finalfilePath, jsonString);
        }

        public void SavePipeline()
        {
            var jsonObj = new RenderPipelineModel
            {
                _name = "DefaultPipeline",
                VertexShader = "C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Shaders\\Shader2DVert.spv",
                FragmentShader = "C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Shaders\\Shader2DFrag.spv",
                PipelineColorBlendAttachmentStateList = new List<VkPipelineColorBlendAttachmentState>()
                {
                     new VkPipelineColorBlendAttachmentState
                            {
                                blendEnable = true,
                                srcColorBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_SRC_ALPHA,
                                dstColorBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ONE_MINUS_SRC_ALPHA,
                                colorBlendOp = VkBlendOp.VK_BLEND_OP_ADD,
                                srcAlphaBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ONE,
                                dstAlphaBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO,
                                alphaBlendOp = VkBlendOp.VK_BLEND_OP_ADD,
                                colorWriteMask = VkColorComponentFlagBits.VK_COLOR_COMPONENT_R_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_G_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_B_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_A_BIT
                            }
                },
                PipelineDepthStencilStateCreateInfo = new VkPipelineDepthStencilStateCreateInfoModel()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO,
                    depthTestEnable = Vk.True,
                    depthWriteEnable = Vk.True,
                    depthCompareOp = VkCompareOp.VK_COMPARE_OP_LESS_OR_EQUAL,
                    depthBoundsTestEnable = Vk.False,
                    stencilTestEnable = Vk.False
                },
                PipelineMultisampleStateCreateInfo = new VkPipelineMultisampleStateCreateInfoModel()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO,
                    rasterizationSamples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT
                },
                PipelineRasterizationStateCreateInfo = new VkPipelineRasterizationStateCreateInfoModel()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO,
                    depthClampEnable = Vk.False,
                    rasterizerDiscardEnable = Vk.False,
                    polygonMode = VkPolygonMode.VK_POLYGON_MODE_FILL,
                    cullMode = VkCullModeFlagBits.VK_CULL_MODE_NONE,
                    frontFace = VkFrontFace.VK_FRONT_FACE_COUNTER_CLOCKWISE,
                    depthBiasEnable = Vk.False,
                    depthBiasConstantFactor = 0.0f,
                    depthBiasClamp = 0.0f,
                    depthBiasSlopeFactor = 0.0f,
                    lineWidth = 1.0f
                },
                ScissorList = new List<VkRect2D>(),
                ViewportList = new List<VkViewport>(),
                PipelineColorBlendStateCreateInfoModel = new VkPipelineColorBlendStateCreateInfoModel()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO,
                    logicOpEnable = Vk.False,
                    logicOp = VkLogicOp.VK_LOGIC_OP_NO_OP,
                    attachmentCount = 1,
                    pNext = IntPtr.Zero,
                },
                PipelineInputAssemblyStateCreateInfo = new VkPipelineInputAssemblyStateCreateInfoModel()
                {
                    topology = VkPrimitiveTopology.VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST
                },
                PipelineDescriptorModelsList = new List<PipelineDescriptorModel>()
                {

                },
                LayoutBindingList = new List<VkDescriptorSetLayoutBindingModel>()
                {
 
                }
            };

            string finalfilePath = @"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\Pipelines\FrameBufferPipeline.json";
            try
            {
                string jsonString = JsonConvert.SerializeObject(jsonObj, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                Directory.CreateDirectory(Path.GetDirectoryName(finalfilePath)); // Ensure directory exists
                File.WriteAllText(finalfilePath, jsonString);
                Console.WriteLine("Pipeline saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save pipeline: {ex.Message}");
                throw;
            }
        }
    }
}