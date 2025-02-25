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
                List<DescriptorSetLayoutBinding> descriptorSetLayoutBindingList = new List<DescriptorSetLayoutBinding>();
                foreach (var binding in model.PipelineDescriptorModelsList)
                {
                    switch (binding.BindingPropertiesList)
                    {
                        case DescriptorBindingPropertiesEnum.kMeshPropertiesDescriptor:
                            {
                                descriptorSetLayoutBindingList.Add(new DescriptorSetLayoutBinding()
                                {
                                    Binding = binding.BindingNumber,
                                    DescriptorCount = meshProperties.UCount(),
                                    DescriptorType = DescriptorType.StorageBuffer,
                                    PImmutableSamplers = null,
                                    StageFlags = ShaderStageFlags.FragmentBit | ShaderStageFlags.VertexBit
                                });
                                break;
                            }
                        case DescriptorBindingPropertiesEnum.kTextureDescriptor:
                            {
                                descriptorSetLayoutBindingList.Add(new DescriptorSetLayoutBinding()
                                {
                                    Binding = binding.BindingNumber,
                                    DescriptorCount = textures.UCount(),
                                    DescriptorType = DescriptorType.CombinedImageSampler,
                                    PImmutableSamplers = null,
                                    StageFlags = ShaderStageFlags.FragmentBit | ShaderStageFlags.VertexBit
                                });
                                break;
                            }
                        case DescriptorBindingPropertiesEnum.kMaterialDescriptor:
                            {
                                descriptorSetLayoutBindingList.Add(new DescriptorSetLayoutBinding()
                                {
                                    Binding = binding.BindingNumber,
                                    DescriptorCount = meshProperties.UCount(),
                                    DescriptorType = DescriptorType.StorageBuffer,
                                    PImmutableSamplers = null,
                                    StageFlags = ShaderStageFlags.FragmentBit | ShaderStageFlags.VertexBit
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

                VkFunc.vkAllocateDescriptorSets(VulkanRenderer.device.Handle, &allocInfo, out VkDescriptorSet descriptorSetPtr);
                descriptorSet = descriptorSetPtr;
            }

            //UpdateDescriptorSets
            {
                List<VkWriteDescriptorSet> descriptorSetList = new List<VkWriteDescriptorSet>();
                for (uint x = 0; x < VulkanRenderer.swapChain.ImageCount; x++)
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
                                            sType = StructureType.WriteDescriptorSet,
                                            descriptorCount = meshProperties.UCount(),
                                            descriptorType = DescriptorType.StorageBuffer,
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
                                            sType = StructureType.WriteDescriptorSet,
                                            descriptorCount = meshProperties.UCount(),
                                            descriptorType = DescriptorType.CombinedImageSampler,
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
                                            sType = StructureType.WriteDescriptorSet,
                                            descriptorCount = meshProperties.UCount(),
                                            descriptorType = DescriptorType.StorageBuffer,
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
                        stageFlags = VkShaderStageFlags.VertexBit | VkShaderStageFlags.FragmentBit,
                        offset = 0,
                        size = ConstBufferSize
                    }
                };

                fixed (VkPushConstantRange* pushConstantRange = pushConstantRangeList.ToArray())
                {
                    VkPipelineLayoutCreateInfo pipelineLayoutInfo = new VkPipelineLayoutCreateInfo
                    {
                        sType = VkStructureType.PipelineLayoutCreateInfo,
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

            List<VertexInputBindingDescription> vertexBindingList = new List<VertexInputBindingDescription>();
            for (int x = 0; x < Vertex2D.GetBindingDescriptions().Count(); x++)
            {
                vertexBindingList.Add(new VertexInputBindingDescription()
                {
                    Stride = Vertex2D.GetBindingDescriptions()[x].Stride,
                    Binding = Vertex2D.GetBindingDescriptions()[x].Binding,
                    InputRate = (VertexInputRate)Vertex2D.GetBindingDescriptions()[x].InputRate
                });
            }

            List<VertexInputAttributeDescription> attributeBindingList = new List<VertexInputAttributeDescription>();
            for (int x = 0; x < Vertex2D.GetAttributeDescriptions().Count(); x++)
            {
                attributeBindingList.Add(new VertexInputAttributeDescription()
                {
                    Format = (Format)Vertex2D.GetAttributeDescriptions()[x].Format,
                    Location = Vertex2D.GetAttributeDescriptions()[x].Location,
                    Binding = Vertex2D.GetAttributeDescriptions()[x].Binding,
                    Offset = Vertex2D.GetAttributeDescriptions()[x].Offset
                });
            }

            PipelineVertexInputStateCreateInfo vertexInputInfo = new PipelineVertexInputStateCreateInfo();
            fixed (VertexInputBindingDescription* vertexBindings = vertexBindingList.ToArray())
            fixed (VertexInputAttributeDescription* attributeBindings = attributeBindingList.ToArray())
            {
                vertexInputInfo = new PipelineVertexInputStateCreateInfo()
                {
                    SType = StructureType.PipelineVertexInputStateCreateInfo,
                    PVertexAttributeDescriptions = attributeBindings,
                    PVertexBindingDescriptions = vertexBindings,
                    VertexAttributeDescriptionCount = Vertex2D.GetAttributeDescriptions().UCount(),
                    VertexBindingDescriptionCount = Vertex2D.GetBindingDescriptions().UCount(),
                    Flags = 0,
                    PNext = null
                };
            }

            PipelineViewportStateCreateInfo pipelineViewportStateCreateInfo = new PipelineViewportStateCreateInfo();
            fixed (VkViewport* viewportPtr = model.ViewportList.ToArray())
            fixed (VkRect2D* scissorPtr = model.ScissorList.ToArray())
            {
                pipelineViewportStateCreateInfo = new PipelineViewportStateCreateInfo
                {
                    SType = StructureType.PipelineViewportStateCreateInfo,
                    ViewportCount = model.ViewportList.UCount() + 1,
                    PViewports = (Viewport*)viewportPtr,
                    ScissorCount = model.ScissorList.UCount() + 1,
                    PScissors = (Rect2D*)scissorPtr,
                    Flags = 0,
                    PNext = null
                };
            }

            VkPipelineColorBlendStateCreateInfo pipelineColorBlendStateCreateInfo = new VkPipelineColorBlendStateCreateInfo();
            fixed (VkPipelineColorBlendAttachmentState* attachments = VkPipelineColorBlendAttachmentState.ConvertPtrArray(model.PipelineColorBlendAttachmentStateList))
            {
                pipelineColorBlendStateCreateInfo = model.PipelineColorBlendStateCreateInfoModel;
                pipelineColorBlendStateCreateInfo.attachmentCount = model.PipelineColorBlendAttachmentStateList.UCount();
                pipelineColorBlendStateCreateInfo.pAttachments = attachments;
            }

            List<DynamicState> dynamicStateList = new List<DynamicState>()
            {
                DynamicState.Viewport,
                DynamicState.Scissor
            };

            VkPipelineDynamicStateCreateInfo pipelineDynamicStateCreateInfo = new VkPipelineDynamicStateCreateInfo();
            fixed (DynamicState* dynamicState = dynamicStateList.ToArray())
                pipelineDynamicStateCreateInfo = new VkPipelineDynamicStateCreateInfo()
                {
                    sType = VkStructureType.PipelineDynamicStateCreateInfo,
                    dynamicStateCount = dynamicStateList.UCount(),
                    pDynamicStates = dynamicState,
                    flags = 0,
                    pNext = null
                };

            List<VkPipelineShaderStageCreateInfo> pipelineShaderStageCreateInfoList = new List<VkPipelineShaderStageCreateInfo>()
            {
                VulkanRenderer.CreateShader(model.VertexShader,  ShaderStageFlags.VertexBit),
                VulkanRenderer.CreateShader(model.FragmentShader, ShaderStageFlags.FragmentBit)
            };

            VkPipelineMultisampleStateCreateInfo pipelineMultisampleStateCreateInfo = model.PipelineMultisampleStateCreateInfo;
            pipelineMultisampleStateCreateInfo.pSampleMask = null;

            VkGraphicsPipelineCreateInfo graphicsPipelineCreateInfo = new VkGraphicsPipelineCreateInfo();
            fixed (VkPipelineShaderStageCreateInfo* pipelineShaderStageCreateInfo = pipelineShaderStageCreateInfoList.ToArray())
            {
                graphicsPipelineCreateInfo = new VkGraphicsPipelineCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO,
                    pStages = pipelineShaderStageCreateInfo,
                    pVertexInputState = &vertexInputInfo,
                    pInputAssemblyState = model.PipelineInputAssemblyStateCreateInfo,
                    pViewportState = &pipelineViewportStateCreateInfo,
                    pRasterizationState = model.PipelineRasterizationStateCreateInfo,
                    pMultisampleState = pipelineMultisampleStateCreateInfo,
                    pDepthStencilState = model.PipelineDepthStencilStateCreateInfo,
                    pColorBlendState = pipelineColorBlendStateCreateInfo,
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

            VkFunc.vkCreateGraphicsPipelines(VulkanRenderer.device, pipelineCache, 1, &graphicsPipelineCreateInfo, null, out VkPipeline tempPipelinePtr);
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
                        srcColorBlendFactor = VkBlendFactor.SrcAlpha,
                        dstColorBlendFactor = VkBlendFactor.OneMinusSrcAlpha,
                        colorBlendOp = VkBlendOp.Add,
                        srcAlphaBlendFactor = VkBlendFactor.One,
                        dstAlphaBlendFactor = VkBlendFactor.OneMinusDstAlpha,
                        alphaBlendOp = VkBlendOp.Add,
                        colorWriteMask = VkColorComponentFlags.RBit |
                                         VkColorComponentFlags.GBit |
                                         VkColorComponentFlags.BBit |
                                         VkColorComponentFlags.ABit
                    }
                },
                PipelineDepthStencilStateCreateInfo = new VkPipelineDepthStencilStateCreateInfo()
                {
                    sType = StructureType.PipelineDepthStencilStateCreateInfo,
                    depthTestEnable = true,
                    depthWriteEnable = true,
                    depthCompareOp = CompareOp.Less,
                    depthBoundsTestEnable = false,
                    stencilTestEnable = false
                },
                PipelineMultisampleStateCreateInfo = new VkPipelineMultisampleStateCreateInfo()
                {
                    sType = VkStructureType.PipelineMultisampleStateCreateInfo,
                    rasterizationSamples = VkSampleCountFlags.SampleCount1Bit
                },
                PipelineRasterizationStateCreateInfo = new VkPipelineRasterizationStateCreateInfo()
                {
                    sType = StructureType.PipelineRasterizationStateCreateInfo,
                    depthClampEnable = false,
                    rasterizerDiscardEnable = false,
                    polygonMode = PolygonMode.Fill,
                    cullMode = CullModeFlags.None,
                    frontFace = FrontFace.CounterClockwise,
                    depthBiasEnable = false,
                    lineWidth = 1.0f
                },
                ScissorList = new List<VkRect2D>(),
                ViewportList = new List<VkViewport>(),
                PipelineColorBlendStateCreateInfoModel = new VkPipelineColorBlendStateCreateInfo()
                {
                    sType = VkStructureType.PipelineColorBlendStateCreateInfo,
                    attachmentCount = 0,
                    pAttachments = null,
                    logicOpEnable = false
                },
                PipelineInputAssemblyStateCreateInfo = new VkPipelineInputAssemblyStateCreateInfo()
                {
                    sType = StructureType.PipelineInputAssemblyStateCreateInfo,
                    topology = PrimitiveTopology.TriangleList,
                    primitiveRestartEnable = false
                },
                PipelineDescriptorModelsList = new List<PipelineDescriptorModel>()
                {
                        new PipelineDescriptorModel
                        {
                            BindingNumber = 0,
                            BindingPropertiesList = DescriptorBindingPropertiesEnum.kMeshPropertiesDescriptor,
                            DescriptorType = DescriptorType.StorageBuffer
                        },
                        new PipelineDescriptorModel
                        {
                            BindingNumber = 1,
                            BindingPropertiesList = DescriptorBindingPropertiesEnum.kTextureDescriptor,
                            DescriptorType = DescriptorType.CombinedImageSampler
                        }
                },
                LayoutBindingList = new List<VkDescriptorSetLayoutBinding>()
                {
                        new VkDescriptorSetLayoutBinding()
                        {
                            binding = 0,
                            descriptorType = DescriptorType.StorageBuffer,
                            descriptorCount = 1,
                            stageFlags = ShaderStageFlags.VertexBit | ShaderStageFlags.FragmentBit,
                            pImmutableSamplers = null
                        },
                        new VkDescriptorSetLayoutBinding()
                        {
                            binding = 1,
                            descriptorType = DescriptorType.CombinedImageSampler,
                            descriptorCount = 1,
                            stageFlags = ShaderStageFlags.FragmentBit,
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
                new VkClearValue(new VkClearColorValue(0, 0, 0, 1)),
                new VkClearValue(null, new VkClearDepthStencilValue(1.0f, 0))
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
