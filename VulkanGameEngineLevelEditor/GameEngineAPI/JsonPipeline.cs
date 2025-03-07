using GlmSharp;
using Newtonsoft.Json;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineGameObjectScripts.Vulkan;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public enum DescriptorBindingPropertiesEnum
    {
        kMeshPropertiesDescriptor,
        kTextureDescriptor,
        kMaterialDescriptor,
        kBRDFMapDescriptor,
        kIrradianceMapDescriptor,
        kPrefilterMapDescriptor,
        kCubeMapDescriptor,
        kEnvironmentDescriptor,
        kSunLightDescriptor,
        kDirectionalLightDescriptor,
        kPointLightDescriptor,
        kSpotLightDescriptor,
        kReflectionViewDescriptor,
        kDirectionalShadowDescriptor,
        kPointShadowDescriptor,
        kSpotShadowDescriptor,
        kViewTextureDescriptor,
        kViewDepthTextureDescriptor,
        kCubeMapSamplerDescriptor,
        kRotatingPaletteTextureDescriptor,
        kMathOpperation1Descriptor,
        kMathOpperation2Descriptor,
    };

    public unsafe class JsonPipeline
    {
        Vk vk = Vk.GetApi();
        VkDevice _device { get; set; }
        public VkDescriptorPool descriptorPool { get; protected set; }
        public List<VkDescriptorSetLayout> descriptorSetLayoutList { get; protected set; } = new List<VkDescriptorSetLayout>();
        public VkDescriptorSet descriptorSet { get; protected set; }
        public VkPipeline pipeline { get; protected set; }
        public VkPipelineLayout pipelineLayout { get; protected set; }
        public VkPipelineCache pipelineCache { get; protected set; }
        
        public JsonPipeline()
        {
          
        }

        public JsonPipeline(String jsonPipelineFilePath, VkRenderPass renderPass, uint ConstBufferSize)
        {
            _device = VulkanRenderer.device;

           // SavePipeline();

            string jsonContent = File.ReadAllText(jsonPipelineFilePath);
            RenderPipelineModel model = JsonConvert.DeserializeObject<RenderPipelineModel>(jsonContent);

            LoadDescriptorSets(model);
            LoadPipeline(model, renderPass, ConstBufferSize);
        }

        private void LoadDescriptorSets(RenderPipelineModel model)
        {
            var meshProperties = MemoryManager.GetGameObjectPropertiesBuffer();
            var textures = MemoryManager.GetTexturePropertiesBuffer();

            //CreateDescriptorPool
            {
                List<VkDescriptorPoolSize> descriptorPoolSizeList = new List<VkDescriptorPoolSize>();
                foreach (var binding in model.PipelineDescriptorModelsList)
                {
                    switch (binding.BindingPropertiesList)
                    {
                        case DescriptorBindingPropertiesEnum.kMeshPropertiesDescriptor:
                            {
                                descriptorPoolSizeList.Add(new VkDescriptorPoolSize()
                                {
                                    type = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                                    descriptorCount = meshProperties.UCount()
                                });
                                break;
                            }
                        case DescriptorBindingPropertiesEnum.kTextureDescriptor:
                            {
                                descriptorPoolSizeList.Add(new VkDescriptorPoolSize()
                                {
                                    type = VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                                    descriptorCount = textures.UCount()
                                });
                                break;
                            }
                        case DescriptorBindingPropertiesEnum.kMaterialDescriptor:
                            {
                                descriptorPoolSizeList.Add(new VkDescriptorPoolSize()
                                {
                                    type = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                                    descriptorCount = textures.UCount()
                                });
                                break;
                            }
                        default:
                            {
                                throw new Exception($"{binding} case hasn't been handled yet");
                            }
                    }
                }

                fixed (VkDescriptorPoolSize* descriptorPoolSize = descriptorPoolSizeList.ToArray())
                {
                    VkDescriptorPoolCreateInfo poolCreateInfo = new VkDescriptorPoolCreateInfo()
                    {
                        sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO,
                        maxSets = 500,
                        pPoolSizes = descriptorPoolSize,
                        poolSizeCount = descriptorPoolSizeList.UCount(),
                        flags = 0,
                        pNext = null
                    };
                    VkFunc.vkCreateDescriptorPool(_device, in poolCreateInfo, null, out VkDescriptorPool descriptorPoolPtr);
                    descriptorPool = descriptorPoolPtr;
                }
            }

            //CreateDescriptorSetLayout
            {
                List<VkDescriptorSetLayoutBinding> descriptorSetLayoutBindingList = new List<VkDescriptorSetLayoutBinding>();
                foreach (var binding in model.PipelineDescriptorModelsList)
                {
                    switch (binding.BindingPropertiesList)
                    {
                        case DescriptorBindingPropertiesEnum.kMeshPropertiesDescriptor:
                            {
                                descriptorSetLayoutBindingList.Add(new VkDescriptorSetLayoutBinding
                                {
                                    binding = binding.BindingNumber,
                                    descriptorCount = meshProperties.UCount(),
                                    descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                                    pImmutableSamplers = null,
                                    stageFlags = VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT | VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT
                                });
                                break;
                            }
                        case DescriptorBindingPropertiesEnum.kTextureDescriptor:
                            {
                                descriptorSetLayoutBindingList.Add(new VkDescriptorSetLayoutBinding
                                {
                                    binding = binding.BindingNumber,
                                    descriptorCount = textures.UCount(),
                                    descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                                    pImmutableSamplers = null,
                                    stageFlags = VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT | VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT
                                });
                                break;
                            }
                        case DescriptorBindingPropertiesEnum.kMaterialDescriptor:
                            {
                                descriptorSetLayoutBindingList.Add(new VkDescriptorSetLayoutBinding
                                {
                                    binding = binding.BindingNumber,
                                    descriptorCount = meshProperties.UCount(),
                                    descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                                    pImmutableSamplers = null,
                                    stageFlags = VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT | VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT
                                });
                                break;
                            }
                        default:
                            {
                                throw new Exception($"{binding} case hasn't been handled yet");
                            }
                    }
                }

                fixed (VkDescriptorSetLayoutBinding* descriptorSetLayouts = descriptorSetLayoutBindingList.ToArray())
                {
                    VkDescriptorSetLayoutCreateInfo descriptorSetLayoutCreateInfo = new VkDescriptorSetLayoutCreateInfo()
                    {
                        sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO,
                        bindingCount = descriptorSetLayoutBindingList.UCount(),
                        pBindings = descriptorSetLayouts,
                        flags = 0,
                        pNext = null
                    };
                    VkFunc.vkCreateDescriptorSetLayout(_device, &descriptorSetLayoutCreateInfo, null, out VkDescriptorSetLayout descriptorsetLayoutPtr);
                    descriptorSetLayoutList.Add(descriptorsetLayoutPtr);
                }
            }

            //AllocateDescriptorSets
            {
                VkDescriptorSetLayout* layouts = stackalloc VkDescriptorSetLayout[VulkanRenderer.MAX_FRAMES_IN_FLIGHT];

                for (int x = 0; x < VulkanRenderer.MAX_FRAMES_IN_FLIGHT; x++)
                {
                    layouts[x] = descriptorSetLayoutList[0];
                }

                VkDescriptorSetAllocateInfo allocInfo = new VkDescriptorSetAllocateInfo
                {
                    descriptorPool = descriptorPool,
                    descriptorSetCount = VulkanRenderer.MAX_FRAMES_IN_FLIGHT,
                    pSetLayouts = layouts
                };

                VkFunc.vkAllocateDescriptorSets(VulkanRenderer.device, &allocInfo, out VkDescriptorSet descriptorSetPtr);
                descriptorSet = descriptorSetPtr;
            }

            //UpdateDescriptorSets
            {
                List<VkWriteDescriptorSet> descriptorSetList = new List<VkWriteDescriptorSet>();
                for (uint x = 0; x < VulkanRenderer.SwapChain.ImageCount; x++)
                {
                    foreach (var binding in model.PipelineDescriptorModelsList)
                    {
                        switch (binding.BindingPropertiesList)
                        {
                            case DescriptorBindingPropertiesEnum.kMeshPropertiesDescriptor:
                                {
                                    fixed (VkDescriptorBufferInfo* meshInfo = meshProperties.ToArray())
                                    {
                                        descriptorSetList.Add(new VkWriteDescriptorSet()
                                        {
                                            sType = VkStructureType.VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                                            descriptorCount = meshProperties.UCount(),
                                            descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                                            dstBinding = binding.BindingNumber,
                                            dstArrayElement = 0,
                                            dstSet = descriptorSet,
                                            pBufferInfo = meshInfo,
                                        });
                                    }
                                    break;
                                }
                            case DescriptorBindingPropertiesEnum.kTextureDescriptor:
                                {
                                    fixed (VkDescriptorImageInfo* texturesPtr = textures.ToArray())
                                    {
                                        descriptorSetList.Add(new VkWriteDescriptorSet()
                                        {
                                            sType = VkStructureType.VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                                            descriptorCount = meshProperties.UCount(),
                                            descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                                            dstBinding = binding.BindingNumber,
                                            dstArrayElement = 0,
                                            dstSet = descriptorSet,
                                            pImageInfo = texturesPtr
                                        });
                                    }
                                    break;
                                }
                            case DescriptorBindingPropertiesEnum.kMaterialDescriptor:
                                {
                                    fixed (VkDescriptorBufferInfo* meshInfo = meshProperties.ToArray())
                                    {
                                        descriptorSetList.Add(new VkWriteDescriptorSet()
                                        {
                                            sType = VkStructureType.VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                                            descriptorCount = meshProperties.UCount(),
                                            descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                                            dstBinding = binding.BindingNumber,
                                            dstArrayElement = 0,
                                            dstSet = descriptorSet,
                                            pBufferInfo = meshInfo,
                                        });
                                    }
                                    break;
                                }
                            default:
                                {
                                    throw new Exception($"{binding} case hasn't been handled yet");
                                }
                        }
                    }
                    fixed (VkWriteDescriptorSet* writeDescriptorSet = descriptorSetList.ToArray())
                    {
                        VkFunc.vkUpdateDescriptorSets(_device, descriptorSetList.UCount(), writeDescriptorSet, 0, null);
                    }
                }
            }
        }

        private void LoadPipeline(RenderPipelineModel model, VkRenderPass renderPass, uint ConstBufferSize)
        {
            List<VkPushConstantRange> pushConstantRangeList = new List<VkPushConstantRange>();
            fixed (VkDescriptorSetLayout* descriptorSet = descriptorSetLayoutList.ToArray())
            {
                pushConstantRangeList = new List<VkPushConstantRange>()
                {
                    new VkPushConstantRange()
                    {
                        stageFlags = VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT | VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT,
                        offset = 0,
                        size = ConstBufferSize
                    }
                };

                fixed (VkPushConstantRange* pushConstantRange = pushConstantRangeList.ToArray())
                {
                    VkPipelineLayoutCreateInfo pipelineLayoutInfo = new VkPipelineLayoutCreateInfo
                    {
                        sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO,
                        setLayoutCount = descriptorSetLayoutList.UCount(),
                        pSetLayouts = descriptorSet,
                        pushConstantRangeCount = pushConstantRangeList.UCount(),
                        pPushConstantRanges = pushConstantRange,
                        flags = 0,
                        pNext = null,
                    };
                    VkFunc.vkCreatePipelineLayout(VulkanRenderer.device, &pipelineLayoutInfo, null, out VkPipelineLayout pipelinelayoutPtr);
                    pipelineLayout = pipelinelayoutPtr;
                }
            }

            List<VkVertexInputBindingDescription> vertexBindingList = new List<VkVertexInputBindingDescription>();
            for (int x = 0; x < Vertex2D.GetBindingDescriptions().Count(); x++)
            {
                vertexBindingList.Add(new VkVertexInputBindingDescription
                {
                    stride = Vertex2D.GetBindingDescriptions()[x].stride,
                    binding = Vertex2D.GetBindingDescriptions()[x].binding,
                    inputRate = Vertex2D.GetBindingDescriptions()[x].inputRate
                });
            }

            List<VkVertexInputAttributeDescription> attributeBindingList = new List<VkVertexInputAttributeDescription>();
            for (int x = 0; x < Vertex2D.GetAttributeDescriptions().Count(); x++)
            {
                attributeBindingList.Add(new VkVertexInputAttributeDescription()
                {
                    format = Vertex2D.GetAttributeDescriptions()[x].format,
                    location = Vertex2D.GetAttributeDescriptions()[x].location,
                    binding = Vertex2D.GetAttributeDescriptions()[x].binding,
                    offset = Vertex2D.GetAttributeDescriptions()[x].offset
                });
            }

            VkPipelineVertexInputStateCreateInfo vertexInputInfo = new VkPipelineVertexInputStateCreateInfo();
            fixed (VkVertexInputBindingDescription* vertexBindings = vertexBindingList.ToArray())
            fixed (VkVertexInputAttributeDescription* attributeBindings = attributeBindingList.ToArray())
            {
                vertexInputInfo = new VkPipelineVertexInputStateCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO,
                    pVertexAttributeDescriptions = attributeBindings,
                    pVertexBindingDescriptions = vertexBindings,
                    vertexAttributeDescriptionCount = Vertex2D.GetAttributeDescriptions().UCount(),
                    vertexBindingDescriptionCount = Vertex2D.GetBindingDescriptions().UCount(),
                    flags = 0,
                    pNext = null
                };
            }

            VkPipelineViewportStateCreateInfo pipelineViewportStateCreateInfo = new VkPipelineViewportStateCreateInfo();
            fixed (VkViewport* viewportPtr = model.ViewportList.ToArray())
            fixed (VkRect2D* scissorPtr = model.ScissorList.ToArray())
            {
                pipelineViewportStateCreateInfo = new VkPipelineViewportStateCreateInfo
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO,
                    viewportCount = model.ViewportList.UCount() + 1,
                    pViewports = viewportPtr,
                    scissorCount = model.ScissorList.UCount() + 1,
                    pScissors = scissorPtr,
                    flags = 0,
                    pNext = null
                };
            }

            VkPipelineColorBlendStateCreateInfo pipelineColorBlendStateCreateInfo = new VkPipelineColorBlendStateCreateInfo();
            fixed (VkPipelineColorBlendAttachmentState* attachments = model.PipelineColorBlendAttachmentStateList.ToArray())
            {
                pipelineColorBlendStateCreateInfo = model.PipelineColorBlendStateCreateInfoModel.Convert();
                pipelineColorBlendStateCreateInfo.attachmentCount = model.PipelineColorBlendAttachmentStateList.UCount();
                pipelineColorBlendStateCreateInfo.pAttachments = attachments;
            }

            List<VkDynamicState> dynamicStateList = new List<VkDynamicState>()
            {
                VkDynamicState.VK_DYNAMIC_STATE_VIEWPORT,
                VkDynamicState.VK_DYNAMIC_STATE_SCISSOR
            };

            VkPipelineDynamicStateCreateInfo pipelineDynamicStateCreateInfo = new VkPipelineDynamicStateCreateInfo();
            fixed (VkDynamicState* dynamicState = dynamicStateList.ToArray())
                pipelineDynamicStateCreateInfo = new VkPipelineDynamicStateCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_DYNAMIC_STATE_CREATE_INFO,
                    dynamicStateCount = dynamicStateList.UCount(),
                    pDynamicStates = dynamicState,
                    flags = 0,
                    pNext = null
                };

            List<VkPipelineShaderStageCreateInfo> pipelineShaderStageCreateInfoList = new List<VkPipelineShaderStageCreateInfo>()
            {
                VulkanRenderer.CreateShader(model.VertexShader,  VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT),
                VulkanRenderer.CreateShader(model.FragmentShader, VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT)
            };

            VkPipelineMultisampleStateCreateInfo pipelineMultisampleStateCreateInfo = model.PipelineMultisampleStateCreateInfo.ConvertPtr();
            pipelineMultisampleStateCreateInfo.pSampleMask = null;

            VkGraphicsPipelineCreateInfo graphicsPipelineCreateInfo = new VkGraphicsPipelineCreateInfo();
            fixed (VkPipelineShaderStageCreateInfo* pipelineShaderStageCreateInfo = pipelineShaderStageCreateInfoList.ToArray())
            {
                var pipelineInputAssemblyStateCreateInfov = model.PipelineInputAssemblyStateCreateInfo.ConvertPtr();
                var pipelineRasterizationStateCreateInfo = model.PipelineRasterizationStateCreateInfo.ConvertPtr();
                var pipelineDepthStencilStateCreateInfo = model.PipelineDepthStencilStateCreateInfo.ConvertPtr();
                graphicsPipelineCreateInfo = new VkGraphicsPipelineCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO,
                    pStages = pipelineShaderStageCreateInfo,
                    pVertexInputState = &vertexInputInfo,
                    pInputAssemblyState = pipelineInputAssemblyStateCreateInfov,
                    pViewportState = &pipelineViewportStateCreateInfo,
                    pRasterizationState = pipelineRasterizationStateCreateInfo,
                    pMultisampleState = &pipelineMultisampleStateCreateInfo,
                    pDepthStencilState = pipelineDepthStencilStateCreateInfo,
                    pColorBlendState = &pipelineColorBlendStateCreateInfo,
                    pDynamicState = &pipelineDynamicStateCreateInfo,
                    pTessellationState = null,
                    layout = pipelineLayout,
                    renderPass = renderPass,
                    stageCount = pipelineShaderStageCreateInfoList.UCount(),
                    subpass = 0,
                    // BasePipelineHandle = ,
                    basePipelineIndex = 0,
                    flags = 0,
                    pNext = null
                };
            }

            VkFunc.vkCreateGraphicsPipelines(VulkanRenderer.device, null, 1, &graphicsPipelineCreateInfo, null, out VkPipeline tempPipelinePtr);
            pipeline = tempPipelinePtr;
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
                    new VkPipelineColorBlendAttachmentState()
                    {
                        blendEnable = true,
                        srcColorBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_SRC_ALPHA,
                        dstColorBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ONE_MINUS_SRC_ALPHA,
                        colorBlendOp = VkBlendOp.VK_BLEND_OP_ADD,
                        srcAlphaBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ONE,
                        dstAlphaBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ONE_MINUS_SRC_ALPHA,
                        alphaBlendOp = VkBlendOp.VK_BLEND_OP_ADD,
                        colorWriteMask = VkColorComponentFlagBits.VK_COLOR_COMPONENT_R_BIT |
                                         VkColorComponentFlagBits.VK_COLOR_COMPONENT_G_BIT |
                                         VkColorComponentFlagBits.VK_COLOR_COMPONENT_B_BIT |
                                         VkColorComponentFlagBits.VK_COLOR_COMPONENT_A_BIT
                    }
                },
                PipelineDepthStencilStateCreateInfo = new VkPipelineDepthStencilStateCreateInfoModel()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO,
                    depthTestEnable = true,
                    depthWriteEnable = true,
                    depthCompareOp = VkCompareOp.VK_COMPARE_OP_LESS,
                    depthBoundsTestEnable = false,
                    stencilTestEnable = false
                },
                PipelineMultisampleStateCreateInfo = new VkPipelineMultisampleStateCreateInfoModel()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO,
                    rasterizationSamples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT
                },
                PipelineRasterizationStateCreateInfo = new VkPipelineRasterizationStateCreateInfoModel()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_LINE_STATE_CREATE_INFO,
                    depthClampEnable = false,
                    rasterizerDiscardEnable = false,
                    polygonMode = VkPolygonMode.VK_POLYGON_MODE_FILL,
                    cullMode = VkCullModeFlagBits.VK_CULL_MODE_NONE,
                    frontFace = VkFrontFace.VK_FRONT_FACE_COUNTER_CLOCKWISE,
                    depthBiasEnable = false,
                    lineWidth = 1.0f
                },
                ScissorList = new List<VkRect2D>(),
                ViewportList = new List<VkViewport>(),
                PipelineColorBlendStateCreateInfoModel = new VkPipelineColorBlendStateCreateInfoModel()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO,
                    attachmentCount = 0,
                    pAttachments = null,
                    logicOpEnable = false
                },
                PipelineInputAssemblyStateCreateInfo = new VkPipelineInputAssemblyStateCreateInfoModel()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO,
                    topology = VkPrimitiveTopology.VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST,
                    primitiveRestartEnable = false
                },
                PipelineDescriptorModelsList = new List<PipelineDescriptorModel>()
                {
                        new PipelineDescriptorModel
                        {
                            BindingNumber = 0,
                            BindingPropertiesList = DescriptorBindingPropertiesEnum.kMeshPropertiesDescriptor,
                            DescriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER
                        },
                        new PipelineDescriptorModel
                        {
                            BindingNumber = 1,
                            BindingPropertiesList = DescriptorBindingPropertiesEnum.kTextureDescriptor,
                            DescriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER
                        }
                },
                LayoutBindingList = new List<VkDescriptorSetLayoutBindingModel>()
                {
                        new VkDescriptorSetLayoutBindingModel()
                        {
                            binding = 0,
                            descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                            descriptorCount = 1,
                            stageFlags = VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT | VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT,
                            pImmutableSamplers = null
                        },
                        new VkDescriptorSetLayoutBindingModel()
                        {
                            binding = 1,
                            descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_SAMPLED_IMAGE,
                            descriptorCount = 1,
                            stageFlags = VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT,
                            pImmutableSamplers = null
                        }
                }
            };

            string finalfilePath = @"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\Pipelines\Default2DPipeline.json";
            string jsonString = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(finalfilePath, jsonString);
        }

        public VkCommandBuffer Draw(VkCommandBuffer[] commandBufferList, VkRenderPass renderPass, VkFramebuffer[] frameBufferList, ivec2 renderPassResolution, List<GameObject> gameObjectList, SceneDataBuffer sceneDataBuffer)
        {
            var commandIndex = (int)VulkanRenderer.CommandIndex;
            var imageIndex = (int)VulkanRenderer.ImageIndex;
            var commandBuffer = commandBufferList[commandIndex];

            List<VkClearValue> clearValueList = new List<VkClearValue>()
            {
                new VkClearValue
                {
                    color = new VkClearColorValue(0, 0, 0, 1),
                },
                new VkClearValue
                {
                    depthStencil = new VkClearDepthStencilValue(1.0f, 0)
                }
            };

            VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo();
            fixed (VkClearValue* clearValuePtr = clearValueList.ToArray())
            {
                renderPassInfo = new VkRenderPassBeginInfo()
                {
                    renderPass = renderPass,
                    framebuffer = frameBufferList[imageIndex],
                    clearValueCount = clearValueList.UCount(),
                    pClearValues = clearValuePtr,
                    renderArea = new VkRect2D
                    {
                        offset = new VkOffset2D(0, 0),
                        extent = new VkExtent2D()
                        {
                            width = (uint)renderPassResolution.x,
                            height = (uint)renderPassResolution.y
                        }
                    }
                };
            }

            var viewport = new VkViewport
            {
                x = 0.0f,
                y = 0.0f,
                width = (uint)renderPassResolution.x,
                height = (uint)renderPassResolution.y,
                minDepth = 0.0f,
                maxDepth = 1.0f
            };

            var scissor = new VkRect2D
            {
                offset = new VkOffset2D(0, 0),
                extent = new VkExtent2D()
                {
                    width = (uint)renderPassResolution.x,
                    height = (uint)renderPassResolution.y
                }
            };

            var descSet = descriptorSet;
            var commandInfo = new VkCommandBufferBeginInfo
            { 
                flags = 0
            };

            VkFunc.vkBeginCommandBuffer(commandBuffer, &commandInfo);
            VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
            VkFunc.vkCmdSetViewport(commandBuffer, 0, 1, &viewport);
            VkFunc.vkCmdSetScissor(commandBuffer, 0, 1, &scissor);
            VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline);
            foreach (var obj in gameObjectList)
            {
                obj.Draw(commandBuffer, pipeline, pipelineLayout, descSet, sceneDataBuffer);
            }
            VkFunc.vkCmdEndRenderPass(commandBuffer);
            VkFunc.vkEndCommandBuffer(commandBuffer);

            return commandBuffer;
        }
    }
}
