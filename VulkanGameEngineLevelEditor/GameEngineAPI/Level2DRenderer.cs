using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using VulkanGameEngineGameObjectScripts.Vulkan;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.Components;
using VulkanGameEngineLevelEditor.Vulkan;
using Silk.NET.Vulkan;
using VulkanGameEngineLevelEditor.Models;
using System.Text;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public struct VkPipelineSet
    {
        public VkDescriptorPool descriptorPool;
        public ListPtr<VkDescriptorSetLayout> descriptorSetLayoutList = new ListPtr<VkDescriptorSetLayout>();
        public ListPtr<VkDescriptorSet> descriptorSetList = new ListPtr<VkDescriptorSet>(); // Public for SpriteBatchLayer
        public VkPipeline pipeline; // Public for SpriteBatchLayer
        public VkPipelineLayout pipelineLayout; // Public for SpriteBatchLayer
        public VkPipelineCache pipelineCache; // Optional, left as VK_NULL_HANDLE

        public VkPipelineSet()
        {
        }
    }

    public unsafe class Level2DRenderer
    {
        private VkDevice device => VulkanRenderer.device;
        private VkRenderPass renderPass;
        private ListPtr<VkFramebuffer> FrameBufferList;
        private ListPtr<VkCommandBuffer> commandBufferList;
        public List<SpriteBatchLayer> SpriteLayerList { get; private set; } = new List<SpriteBatchLayer>();
        public List<GameObject> GameObjectList { get; private set; } = new List<GameObject>();
        public List<Texture> TextureList { get; private set; } = new List<Texture>();
        public List<Material> MaterialList { get; private set; } = new List<Material>();
        public RenderedTexture texture { get; private set; }
        public DepthTexture depthTexture { get; private set; }

        // Pipeline-related fields
        private VkDescriptorPool descriptorPool;
        private ListPtr<VkDescriptorSetLayout> descriptorSetLayoutList = new ListPtr<VkDescriptorSetLayout>();
        public ListPtr<VkDescriptorSet> descriptorSetList = new ListPtr<VkDescriptorSet>();
        public VkPipeline pipeline;
        public VkPipelineLayout pipelineLayout;
        private VkPipelineCache pipelineCache = VulkanConst.VK_NULL_HANDLE;

        private ivec2 RenderPassResolution;

        public Level2DRenderer(string json, ivec2 renderPassResolution)
        {
            RenderPassBuildInfoModel model = new RenderPassBuildInfoModel(json);
            foreach (var item in model.RenderedTextureInfoModelList)
            {
                item.ImageCreateInfo.extent = new VkExtent3DModel
                {
                    width = (uint)renderPassResolution.x,
                    height = (uint)renderPassResolution.y,
                    depth = 1
                };
            }

            texture = new RenderedTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, model.RenderedTextureInfoModelList[0].ImageCreateInfo.Convert(), model.RenderedTextureInfoModelList[0].SamplerCreateInfo.Convert());
            depthTexture = new DepthTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_DEPTH_BIT, model.RenderedTextureInfoModelList[1].ImageCreateInfo.Convert(), model.RenderedTextureInfoModelList[1].SamplerCreateInfo.Convert());

            RenderPassResolution = renderPassResolution;
            FrameBufferList = new ListPtr<VkFramebuffer>(3);
            commandBufferList = new ListPtr<VkCommandBuffer>(3);
            VulkanRenderer.CreateCommandBuffers(commandBufferList);
            CreateHardcodedRenderPass();
            CreateHardcodedFramebuffers();
            StartLevelRenderer();
        }

        private void CreateHardcodedRenderPass()
        {
            VkAttachmentDescription colorAttachment = new VkAttachmentDescription
            {
                format = VkFormat.VK_FORMAT_R8G8B8A8_UNORM,
                samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR,
                storeOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_STORE,
                stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE,
                stencilStoreOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE,
                initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED,
                finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR
            };

            VkAttachmentDescription depthAttachment = new VkAttachmentDescription
            {
                format = VkFormat.VK_FORMAT_D32_SFLOAT,
                samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR,
                storeOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE,
                stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE,
                stencilStoreOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE,
                initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED,
                finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_DEPTH_STENCIL_ATTACHMENT_OPTIMAL
            };

            VkAttachmentReference colorAttachmentRef = new VkAttachmentReference
            {
                attachment = 0,
                layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
            };

            VkAttachmentReference depthAttachmentRef = new VkAttachmentReference
            {
                attachment = 1,
                layout = VkImageLayout.VK_IMAGE_LAYOUT_DEPTH_STENCIL_ATTACHMENT_OPTIMAL
            };

            VkSubpassDescription subpass = new VkSubpassDescription
            {
                pipelineBindPoint = VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS,
                colorAttachmentCount = 1,
                pColorAttachments = &colorAttachmentRef,
                pDepthStencilAttachment = &depthAttachmentRef
            };

            VkSubpassDependency dependency = new VkSubpassDependency
            {
                srcSubpass = VulkanConst.VK_SUBPASS_EXTERNAL,
                dstSubpass = 0,
                srcStageMask = VkPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                dstStageMask = VkPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                srcAccessMask = 0,
                dstAccessMask = VkAccessFlagBits.VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT
            };

            VkAttachmentDescription[] attachments = new[] { colorAttachment, depthAttachment };
            GCHandle attachmentsHandle = GCHandle.Alloc(attachments, GCHandleType.Pinned);
            VkRenderPassCreateInfo renderPassInfo = new VkRenderPassCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
                attachmentCount = 2,
                pAttachments = (VkAttachmentDescription*)attachmentsHandle.AddrOfPinnedObject(),
                subpassCount = 1,
                pSubpasses = &subpass,
                dependencyCount = 1,
                pDependencies = &dependency
            };

            VkFunc.vkCreateRenderPass(device, &renderPassInfo, null, out VkRenderPass renderPass2);
            renderPass = renderPass2;
            attachmentsHandle.Free();
        }

        private void CreateHardcodedFramebuffers()
        {
            for (int i = 0; i < VulkanRenderer.SwapChain.ImageCount; i++) // 3 images
            {
                nint[] attachments = new nint[]
                {
                    texture.View,
                    depthTexture.View
                };

                GCHandle attachmentsHandle = GCHandle.Alloc(attachments, GCHandleType.Pinned);
                VkFramebufferCreateInfo fbInfo = new VkFramebufferCreateInfo
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
                    renderPass = renderPass,
                    attachmentCount = 2,
                    pAttachments = (nint*)attachmentsHandle.AddrOfPinnedObject(),
                    width = (uint)RenderPassResolution.x,
                    height = (uint)RenderPassResolution.y,
                    layers = 1
                };

                VkFunc.vkCreateFramebuffer(device, &fbInfo, null, out VkFramebuffer fb);
                FrameBufferList.Add(fb);
                attachmentsHandle.Free();
            }
        }

        public void StartLevelRenderer()
        {
            TextureList.Add(new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\MegaMan_diffuse.bmp", VkFormat.VK_FORMAT_R8G8B8A8_SRGB, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_DiffuseTextureMap, false));
            TransitionTextureLayout(TextureList[0]);
            MaterialList.Add(new Material("Material1"));
            MaterialList.Last().SetAlbedoMap(TextureList[0]);

            ivec2 size = new ivec2(32);
            SpriteSheet spriteSheet = new SpriteSheet(MaterialList[0], size, 0);

            AddGameObject("Obj1", new List<ComponentTypeEnum> { ComponentTypeEnum.kTransform2DComponent, ComponentTypeEnum.kSpriteComponent }, spriteSheet, new vec2(960.0f, 540.0f));
            AddGameObject("Obj2", new List<ComponentTypeEnum> { ComponentTypeEnum.kTransform2DComponent, ComponentTypeEnum.kSpriteComponent }, spriteSheet, new vec2(300.0f, 20.0f));
            AddGameObject("Obj3", new List<ComponentTypeEnum> { ComponentTypeEnum.kTransform2DComponent, ComponentTypeEnum.kSpriteComponent }, spriteSheet, new vec2(300.0f, 80.0f));

            var vertexBindings = SpriteInstanceVertex2D.GetBindingDescriptions();
            var vertexAttributes = SpriteInstanceVertex2D.GetAttributeDescriptions();
            GPUImport<Vertex2D> gpuImport = new GPUImport<Vertex2D>
            {
                MeshList = new List<Mesh<Vertex2D>>(GetMeshFromGameObjects()),
                TextureList = new List<Texture>(TextureList),
                MaterialList = new List<Material>(MaterialList)
            };

            // Create the pipeline first
            SpriteLayerList.Add(new SpriteBatchLayer(GameObjectList));
            CreateHardcodedPipeline(vertexBindings, vertexAttributes, gpuImport);
        }


        private void CreateHardcodedPipeline(ListPtr<VkVertexInputBindingDescription> vertexBindings, ListPtr<VkVertexInputAttributeDescription> vertexAttributes, GPUImport<Vertex2D> gpuImport)
        {
            // Descriptor Pool
            VkDescriptorPoolSize[] poolSizes = new[]
            {
                new VkDescriptorPoolSize { type = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER, descriptorCount = (uint)GameObjectList.Count },
                new VkDescriptorPoolSize { type = VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER, descriptorCount = (uint)TextureList.Count },
                new VkDescriptorPoolSize { type = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER, descriptorCount = (uint)MaterialList.Count }
            };

            GCHandle poolSizesHandle = GCHandle.Alloc(poolSizes, GCHandleType.Pinned);
            VkDescriptorPoolCreateInfo poolInfo = new VkDescriptorPoolCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO,
                maxSets = 3,
                poolSizeCount = (uint)poolSizes.Length,
                pPoolSizes = (VkDescriptorPoolSize*)poolSizesHandle.AddrOfPinnedObject()
            };
            VkFunc.vkCreateDescriptorPool(device, in poolInfo, null, out descriptorPool);
            poolSizesHandle.Free();

            // Descriptor Set Layout
            VkDescriptorSetLayoutBinding[] bindings = new[]
            {
                new VkDescriptorSetLayoutBinding
                {
                    binding = 0,
                    descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                    descriptorCount = (uint)GameObjectList.Count,
                    stageFlags = VkShaderStageFlagBits.VK_SHADER_STAGE_ALL
                },
                new VkDescriptorSetLayoutBinding
                {
                    binding = 1,
                    descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                    descriptorCount = (uint)TextureList.Count,
                    stageFlags = VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT
                },
                new VkDescriptorSetLayoutBinding
                {
                    binding = 2,
                    descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                    descriptorCount = (uint)MaterialList.Count,
                    stageFlags = VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT
                }
            };

            GCHandle bindingsHandle = GCHandle.Alloc(bindings, GCHandleType.Pinned);
            VkDescriptorSetLayoutCreateInfo layoutInfo = new VkDescriptorSetLayoutCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO,
                bindingCount = (uint)bindings.Length,
                pBindings = (VkDescriptorSetLayoutBinding*)bindingsHandle.AddrOfPinnedObject()
            };
            VkDescriptorSetLayout layout;
            VkFunc.vkCreateDescriptorSetLayout(device, &layoutInfo, null, out layout);
            descriptorSetLayoutList.Add(layout);
            bindingsHandle.Free();

            // Allocate Descriptor Sets
            GCHandle layoutsHandle = GCHandle.Alloc(descriptorSetLayoutList.ToArray(), GCHandleType.Pinned);
            VkDescriptorSetAllocateInfo allocInfo = new VkDescriptorSetAllocateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO,
                descriptorPool = descriptorPool,
                descriptorSetCount = descriptorSetLayoutList.UCount,
                pSetLayouts = (VkDescriptorSetLayout*)layoutsHandle.AddrOfPinnedObject()
            };
            VkDescriptorSet set;
            VkFunc.vkAllocateDescriptorSets(device, &allocInfo, out set);
            descriptorSetList.Add(set);
            layoutsHandle.Free();

            // Update Descriptor Sets
            var meshInfos = new List<Mesh<Vertex2D>>();
            foreach (SpriteBatchLayer spriteLayer in SpriteLayerList)
            {
                meshInfos.Add(spriteLayer.SpriteLayerMesh);
            }
            var meshInfo = GetMeshPropertiesBuffer(meshInfos);
            var textureInfos = GetTexturePropertiesBuffer(TextureList);
            var materialInfos = GetMaterialPropertiesBuffer(MaterialList);
            VkWriteDescriptorSet[] writes = new[]
            {
                new VkWriteDescriptorSet
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                    dstSet = descriptorSetList[0],
                    dstBinding = 0,
                    descriptorCount = meshInfo.UCount,
                    descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                    pBufferInfo = meshInfo.Ptr
                },
                new VkWriteDescriptorSet
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                    dstSet = descriptorSetList[0],
                    dstBinding = 1,
                    descriptorCount = textureInfos.UCount,
                    descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                    pImageInfo = textureInfos.Ptr
                },
                new VkWriteDescriptorSet
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                    dstSet = descriptorSetList[0],
                    dstBinding = 2,
                    descriptorCount = materialInfos.UCount,
                    descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                    pBufferInfo = materialInfos.Ptr
                }
            };

            GCHandle writesHandle = GCHandle.Alloc(writes, GCHandleType.Pinned);
            VkFunc.vkUpdateDescriptorSets(device, (uint)writes.Length, (VkWriteDescriptorSet*)writesHandle.AddrOfPinnedObject(), 0, null);
            writesHandle.Free();

            // Pipeline Layout
            VkPushConstantRange pushConstant = new VkPushConstantRange
            {
                stageFlags = VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT | VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT,
                offset = 0,
                size = (uint)sizeof(SceneDataBuffer)
            };

            GCHandle layoutsHandle2 = GCHandle.Alloc(descriptorSetLayoutList.ToArray(), GCHandleType.Pinned);
            VkPipelineLayoutCreateInfo pipelineLayoutInfo = new VkPipelineLayoutCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO,
                setLayoutCount = descriptorSetLayoutList.UCount,
                pSetLayouts = (VkDescriptorSetLayout*)layoutsHandle2.AddrOfPinnedObject(),
                pushConstantRangeCount = 1,
                pPushConstantRanges = &pushConstant
            };
            VkFunc.vkCreatePipelineLayout(device, &pipelineLayoutInfo, null, out pipelineLayout);
            layoutsHandle2.Free();

            // Graphics Pipeline
            VkShaderModule vertShader = CreateShaderModule("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Shaders\\Shader2DVert.spv");
            VkShaderModule fragShader = CreateShaderModule("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Shaders\\Shader2DFrag.spv");

            byte[] mainBytes = Encoding.UTF8.GetBytes("main\0");
            GCHandle vertMainHandle = GCHandle.Alloc(mainBytes, GCHandleType.Pinned);
            GCHandle fragMainHandle = GCHandle.Alloc(mainBytes, GCHandleType.Pinned);

            VkPipelineShaderStageCreateInfo[] stages = new[]
            {
                new VkPipelineShaderStageCreateInfo
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO,
                    stage = VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT,
                    module = vertShader,
                    pName = (char*)vertMainHandle.AddrOfPinnedObject()
                },
                new VkPipelineShaderStageCreateInfo
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO,
                    stage = VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT,
                    module = fragShader,
                    pName = (char*)fragMainHandle.AddrOfPinnedObject()
                }
            };

            VkPipelineVertexInputStateCreateInfo vertexInputInfo = new VkPipelineVertexInputStateCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO,
                vertexBindingDescriptionCount = vertexBindings.UCount,
                pVertexBindingDescriptions = vertexBindings.Ptr,
                vertexAttributeDescriptionCount = vertexAttributes.UCount,
                pVertexAttributeDescriptions = vertexAttributes.Ptr
            };

            VkPipelineInputAssemblyStateCreateInfo inputAssembly = new VkPipelineInputAssemblyStateCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO,
                topology = VkPrimitiveTopology.VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST,
                primitiveRestartEnable = 0
            };

            VkViewport viewport = new VkViewport { x = 0, y = 0, width = (float)RenderPassResolution.x, height = (float)RenderPassResolution.y, minDepth = 0, maxDepth = 1 };
            VkRect2D scissor = new VkRect2D { offset = new VkOffset2D(0, 0), extent = new VkExtent2D { width = (uint)RenderPassResolution.x, height = (uint)RenderPassResolution.y } };
            VkPipelineViewportStateCreateInfo viewportState = new VkPipelineViewportStateCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO,
                viewportCount = 1,
                pViewports = &viewport,
                scissorCount = 1,
                pScissors = &scissor
            };

            VkPipelineRasterizationStateCreateInfo rasterizer = new VkPipelineRasterizationStateCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO,
                depthClampEnable = 0,
                rasterizerDiscardEnable = 0,
                polygonMode = VkPolygonMode.VK_POLYGON_MODE_FILL,
                cullMode = VkCullModeFlagBits.VK_CULL_MODE_NONE,
                frontFace = VkFrontFace.VK_FRONT_FACE_CLOCKWISE,
                lineWidth = 1.0f
            };

            VkPipelineMultisampleStateCreateInfo multisampling = new VkPipelineMultisampleStateCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO,
                rasterizationSamples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                sampleShadingEnable = 0
            };

            VkPipelineDepthStencilStateCreateInfo depthStencil = new VkPipelineDepthStencilStateCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO,
                depthTestEnable = 1,
                depthWriteEnable = 1,
                depthCompareOp = VkCompareOp.VK_COMPARE_OP_LESS,
                depthBoundsTestEnable = 0,
                stencilTestEnable = 0
            };

            VkPipelineColorBlendAttachmentState colorBlendAttachment = new VkPipelineColorBlendAttachmentState
            {
                blendEnable = false,
                colorWriteMask = VkColorComponentFlagBits.VK_COLOR_COMPONENT_R_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_G_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_B_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_A_BIT
            };

            VkPipelineColorBlendStateCreateInfo colorBlending = new VkPipelineColorBlendStateCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO,
                logicOpEnable = 0,
                attachmentCount = 1,
                pAttachments = &colorBlendAttachment
            };

            GCHandle stagesHandle = GCHandle.Alloc(stages, GCHandleType.Pinned);
            VkGraphicsPipelineCreateInfo pipelineInfo = new VkGraphicsPipelineCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO,
                stageCount = 2,
                pStages = (VkPipelineShaderStageCreateInfo*)stagesHandle.AddrOfPinnedObject(),
                pVertexInputState = &vertexInputInfo,
                pInputAssemblyState = &inputAssembly,
                pViewportState = &viewportState,
                pRasterizationState = &rasterizer,
                pMultisampleState = &multisampling,
                pDepthStencilState = &depthStencil,
                pColorBlendState = &colorBlending,
                layout = pipelineLayout,
                renderPass = renderPass,
                subpass = 0
            };

            VkPipeline tempPipelinePtr;
            VkResult result = VkFunc.vkCreateGraphicsPipelines(device, pipelineCache, 1, &pipelineInfo, null, &tempPipelinePtr);
            if (result != VkResult.VK_SUCCESS)
            {
                throw new Exception($"vkCreateGraphicsPipelines failed: {result}");
            }
            pipeline = tempPipelinePtr;

            stagesHandle.Free();
            vertMainHandle.Free();
            fragMainHandle.Free();

            VkFunc.vkDestroyShaderModule(device, vertShader, null);
            VkFunc.vkDestroyShaderModule(device, fragShader, null);
        }

        public void Update(float deltaTime)
        {
            DestroyDeadGameObjects();
            VkCommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
            foreach (var obj in GameObjectList)
            {
                obj.Update(commandBuffer, deltaTime);
            }
            foreach (var spriteLayer in SpriteLayerList)
            {
                spriteLayer.Update(commandBuffer, deltaTime);
            }
            VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
        }

        private void DestroyDeadGameObjects()
        {
            if (!GameObjectList.Any())
            {
                return;
            }

            var deadGameObjectList = GameObjectList.Where(x => x.GameObjectAlive == false).ToList();
            if (deadGameObjectList.Any())
            {
                foreach (var gameObject in deadGameObjectList)
                {
                    var spriteComponent = gameObject.GetComponentByComponentType(ComponentTypeEnum.kSpriteComponent);
                    if (spriteComponent != null)
                    {
                        var sprite = (spriteComponent as SpriteComponent).SpriteObj;
                        gameObject.RemoveComponent(spriteComponent);
                    }
                    gameObject.Destroy();
                }
            }
            foreach (var gameObject in GameObjectList.Where(x => x.GameObjectAlive == false))
            {
                GameObjectList.Remove(gameObject);
            }
        }

        private VkShaderModule CreateShaderModule(string filePath)
        {
            byte[] code = System.IO.File.ReadAllBytes(filePath);
            GCHandle codeHandle = GCHandle.Alloc(code, GCHandleType.Pinned);
            VkShaderModuleCreateInfo createInfo = new VkShaderModuleCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO,
                codeSize = (nint)code.Length,
                pCode = (uint*)codeHandle.AddrOfPinnedObject()
            };

            VkFunc.vkCreateShaderModule(device, ref createInfo, IntPtr.Zero, out nint module);
            codeHandle.Free();
            return module;
        }

        private void TransitionTextureLayout(Texture texture)
        {
            VkCommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();

            VkImageMemoryBarrier barrier = new VkImageMemoryBarrier
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
                oldLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED,
                newLayout = VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL,
                srcQueueFamilyIndex = VulkanConst.VK_QUEUE_FAMILY_IGNORED,
                dstQueueFamilyIndex = VulkanConst.VK_QUEUE_FAMILY_IGNORED,
                image = texture.Image,
                subresourceRange = new VkImageSubresourceRange
                {
                    aspectMask = VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT,
                    baseMipLevel = 0,
                    levelCount = 1,
                    baseArrayLayer = 0,
                    layerCount = 1
                },
                srcAccessMask = 0,
                dstAccessMask = VkAccessFlagBits.VK_ACCESS_SHADER_READ_BIT
            };

            VkFunc.vkCmdPipelineBarrier(
                commandBuffer,
                VkPipelineStageFlagBits.TOP_OF_PIPE_BIT,
                VkPipelineStageFlagBits.FRAGMENT_SHADER_BIT,
                0,
                0, null,
                0, null,
                1, &barrier
            );

            VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
        }

        private ListPtr<VkDescriptorBufferInfo> GetMeshPropertiesBuffer(List<Mesh<Vertex2D>> meshList)
        {
            ListPtr<VkDescriptorBufferInfo> meshPropertiesBuffer = new ListPtr<VkDescriptorBufferInfo>();
            foreach (var mesh in meshList)
            {
                meshPropertiesBuffer.Add(mesh.GetMeshPropertiesBuffer());
            }
            return meshPropertiesBuffer;
        }

        private ListPtr<VkDescriptorImageInfo> GetTexturePropertiesBuffer(List<Texture> textureList)
        {
            ListPtr<VkDescriptorImageInfo> texturePropertiesBuffer = new ListPtr<VkDescriptorImageInfo>();
            foreach (var texture in textureList)
            {
                texturePropertiesBuffer.Add(texture.GetTexturePropertiesBuffer());
            }
            return texturePropertiesBuffer;
        }

        private ListPtr<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer(List<Material> materialList)
        {
            ListPtr<VkDescriptorBufferInfo> materialPropertiesBuffer = new ListPtr<VkDescriptorBufferInfo>();
            foreach (var material in materialList)
            {
                materialPropertiesBuffer.Add(material.GetMaterialPropertiesBuffer());
            }
            return materialPropertiesBuffer;
        }

        public VkCommandBuffer Draw(List<GameObject> meshList, SceneDataBuffer sceneDataBuffer)
        {
            var commandIndex = VulkanRenderer.CommandIndex % 3; // Cycle through 3 command buffers
            var imageIndex = VulkanRenderer.ImageIndex % 3; // Cycle through 3 images
            var commandBuffer = commandBufferList[(int)commandIndex];

            VkClearValue[] clearValues = new[]
            {
                new VkClearValue { Color = new VkClearColorValue(0, 0, 0, 1) },
                new VkClearValue { DepthStencil = new VkClearDepthStencilValue(1.0f, 0) }
            };

            GCHandle clearValuesHandle = GCHandle.Alloc(clearValues, GCHandleType.Pinned);
            VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
                renderPass = renderPass,
                framebuffer = FrameBufferList[(int)imageIndex],
                clearValueCount = (uint)clearValues.Length,
                pClearValues = (VkClearValue*)clearValuesHandle.AddrOfPinnedObject(),
                renderArea = new VkRect2D(new VkOffset2D(0, 0), VulkanRenderer.SwapChain.SwapChainResolution)
            };

            VkViewport viewport = new VkViewport
            {
                x = 0.0f,
                y = 0.0f,
                width = (float)RenderPassResolution.x,
                height = (float)RenderPassResolution.y,
                minDepth = 0.0f,
                maxDepth = 1.0f
            };

            VkRect2D scissor = new VkRect2D
            {
                offset = new VkOffset2D(0, 0),
                extent = VulkanRenderer.SwapChain.SwapChainResolution
            };

            VkCommandBufferBeginInfo commandInfo = new VkCommandBufferBeginInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
                flags = VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
            };

            VkFunc.vkResetCommandBuffer(commandBuffer, 0);
            VkFunc.vkBeginCommandBuffer(commandBuffer, &commandInfo);
            VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
            VkFunc.vkCmdSetViewport(commandBuffer, 0, 1, &viewport);
            VkFunc.vkCmdSetScissor(commandBuffer, 0, 1, &scissor);

            foreach (var spriteLayer in SpriteLayerList)
            {
                GCHandle vertexHandle = GCHandle.Alloc(spriteLayer.SpriteLayerMesh.MeshVertexBuffer.Buffer, GCHandleType.Pinned);
                GCHandle indexHandle = GCHandle.Alloc(spriteLayer.SpriteLayerMesh.MeshIndexBuffer.Buffer, GCHandleType.Pinned);
                GCHandle instanceHandle = GCHandle.Alloc(spriteLayer.SpriteBuffer.Buffer, GCHandleType.Pinned);

                ulong[] offsets = new ulong[] { 0, 0 };
                GCHandle offsetsHandle = GCHandle.Alloc(offsets, GCHandleType.Pinned);
                VkDescriptorSet descriptorSet = descriptorSetList[0];
                VkFunc.vkCmdPushConstants(commandBuffer, pipelineLayout, VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT | VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT, 0, (uint)sizeof(SceneDataBuffer), &sceneDataBuffer);
                VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline);
                VkFunc.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipelineLayout, 0, 1, &descriptorSet, 0, null);
                VkFunc.vkCmdBindVertexBuffers(commandBuffer, 0, 1, (nint*)vertexHandle.AddrOfPinnedObject(), (ulong*)offsetsHandle.AddrOfPinnedObject());
                VkFunc.vkCmdBindVertexBuffers(commandBuffer, 1, 1, (nint*)instanceHandle.AddrOfPinnedObject(), (ulong*)offsetsHandle.AddrOfPinnedObject() + 1);
                VkFunc.vkCmdBindIndexBuffer(commandBuffer, *(nint*)indexHandle.AddrOfPinnedObject(), 0, VkIndexType.VK_INDEX_TYPE_UINT32);
                VkFunc.vkCmdDrawIndexed(commandBuffer, spriteLayer.SpriteIndexList.UCount(), spriteLayer.SpriteInstanceList.UCount(), 0, 0, 0);

                vertexHandle.Free();
                indexHandle.Free();
                instanceHandle.Free();
                offsetsHandle.Free();
            }

            VkFunc.vkCmdEndRenderPass(commandBuffer);
            VkFunc.vkEndCommandBuffer(commandBuffer);

            clearValuesHandle.Free();
            return commandBuffer;
        }

        private void AddGameObject(string name, List<ComponentTypeEnum> gameObjectComponentTypeList, SpriteSheet spriteSheet, vec2 objectPosition)
        {
            GameObjectList.Add(new GameObject(name, new List<ComponentTypeEnum>
            {
                ComponentTypeEnum.kTransform2DComponent,
                ComponentTypeEnum.kSpriteComponent
            }, spriteSheet));
            var gameObject = GameObjectList.Last();

            foreach (var component in gameObjectComponentTypeList)
            {
                switch (component)
                {
                    case ComponentTypeEnum.kTransform2DComponent: gameObject.AddComponent(new Transform2DComponent(gameObject.GameObjectId, objectPosition, name)); break;
                    case ComponentTypeEnum.kSpriteComponent: gameObject.AddComponent(new SpriteComponent(gameObject, name, spriteSheet)); break;
                }
            }
        }

        private List<Mesh2D> GetMeshFromGameObjects()
        {
            var meshList = new List<Mesh2D>();
            foreach (SpriteBatchLayer spriteLayer in SpriteLayerList)
            {
                meshList.Add(spriteLayer.SpriteLayerMesh);
            }
            return meshList;
        }

        public void Destroy()
        {
            VkFunc.vkDestroyPipeline(device, pipeline, null);
            VkFunc.vkDestroyPipelineLayout(device, pipelineLayout, null);
            foreach (var layout in descriptorSetLayoutList)
            {
                VkFunc.vkDestroyDescriptorSetLayout(device, layout, null);
            }
            VkFunc.vkDestroyDescriptorPool(device, descriptorPool, null);
            foreach (var fb in FrameBufferList)
            {
                VkFunc.vkDestroyFramebuffer(device, fb, null);
            }
            VkFunc.vkDestroyRenderPass(device, renderPass, null);

            FrameBufferList.Dispose();
            commandBufferList.Dispose();
            descriptorSetLayoutList.Dispose();
            descriptorSetList.Dispose();
        }
    }
}