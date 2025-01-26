#include "JsonPipeline.h"
#include "MemoryManager.h"
#include "ShaderCompiler.h"

JsonPipeline::JsonPipeline()
{
}

JsonPipeline::JsonPipeline(String jsonPath, VkRenderPass renderPass, uint constBufferSize)
{
  //  ParentRenderPass = parentRenderPass;
    nlohmann::json json = Json::ReadJson("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\\Pipelines\\Default2DPipeline.json");
    RenderPipelineModel renderPipelineModel = RenderPipelineModel::from_json(json);
    LoadDescriptorSets(renderPipelineModel);
    LoadPipeline(renderPipelineModel, renderPass, constBufferSize);
}

JsonPipeline::~JsonPipeline()
{
}

SharedPtr<JsonPipeline> JsonPipeline::CreateJsonRenderPass(String jsonPath, VkRenderPass renderPass, uint constBufferSize)
{
    SharedPtr<JsonPipeline> pipeline = MemoryManager::AllocateJsonPipeline();
    new (pipeline.get()) JsonPipeline(jsonPath, renderPass, constBufferSize);
    return pipeline;
}

void JsonPipeline::LoadDescriptorSets(RenderPipelineModel model)
{
    List<VkDescriptorBufferInfo> meshProperties = MemoryManager::GetMeshPropertiesBuffer();
    List<VkDescriptorImageInfo> TextureList = MemoryManager::GetTexturePropertiesBuffer();
    List<VkDescriptorBufferInfo> materialProperties = MemoryManager::GetMaterialPropertiesBuffer();

    //CreateDescriptorPool
    {
        List<VkDescriptorPoolSize> descriptorPoolSizeList = List<VkDescriptorPoolSize>();
        for (auto binding : model.PipelineDescriptorModelsList)
        {
            switch (binding.BindingPropertiesList)
            {
                case kMeshPropertiesDescriptor:
                {
                    descriptorPoolSizeList.emplace_back(VkDescriptorPoolSize
                        {
                            .type = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                            .descriptorCount = static_cast<uint32>(meshProperties.size())
                        });
                    break;
                }
                case kTextureDescriptor:
                {
                    descriptorPoolSizeList.emplace_back(VkDescriptorPoolSize
                        {
                            .type = VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                            .descriptorCount = static_cast<uint32>(TextureList.size())
                        });
                    break;
                }
                case kMaterialDescriptor:
                {
                    descriptorPoolSizeList.emplace_back(VkDescriptorPoolSize
                        {
                            .type = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                            .descriptorCount = static_cast<uint32>(materialProperties.size())
                        });
                    break;
                }
                default:
                {
                    throw std::runtime_error("Binding case hasn't been handled yet");
                }
            }
        }

        VkDescriptorPoolCreateInfo poolCreateInfo = VkDescriptorPoolCreateInfo
        {
            .sType = VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO,
            .pNext = nullptr,
            .flags = 0,
            .maxSets = 500,
            .poolSizeCount = static_cast<uint32>(descriptorPoolSizeList.size()),
            .pPoolSizes = descriptorPoolSizeList.data()
        };
        VULKAN_RESULT(vkCreateDescriptorPool(cRenderer.Device, &poolCreateInfo, nullptr, &DescriptorPool));
    }

    //CreateDescriptorSetLayout
    {
        List<VkDescriptorSetLayoutBinding> descriptorSetLayoutBindingList = List<VkDescriptorSetLayoutBinding>();
        for (auto binding : model.PipelineDescriptorModelsList)
        {
            switch (binding.BindingPropertiesList)
            {
                case kMeshPropertiesDescriptor:
                {
                    descriptorSetLayoutBindingList.emplace_back(VkDescriptorSetLayoutBinding
                        {
                            .binding = binding.BindingNumber,
                            .descriptorType = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                            .descriptorCount = static_cast<uint32>(meshProperties.size()),
                            .stageFlags = VK_SHADER_STAGE_ALL,
                            .pImmutableSamplers = nullptr
                        });
                    break;
                }
                case kTextureDescriptor:
                {
                    descriptorSetLayoutBindingList.emplace_back(VkDescriptorSetLayoutBinding
                        {
                            .binding = binding.BindingNumber,
                            .descriptorType = VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                            .descriptorCount = static_cast<uint32>(TextureList.size()),
                            .stageFlags = VK_SHADER_STAGE_ALL,
                            .pImmutableSamplers = nullptr
                        });
                    break;
                }
                case kMaterialDescriptor:
                {
                    descriptorSetLayoutBindingList.emplace_back(VkDescriptorSetLayoutBinding
                        {
                            .binding = binding.BindingNumber,
                            .descriptorType = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                            .descriptorCount = static_cast<uint32>(materialProperties.size()),
                            .stageFlags = VK_SHADER_STAGE_ALL,
                            .pImmutableSamplers = nullptr
                        });
                    break;
                }
                default:
                {
                    throw std::runtime_error("Binding case hasn't been handled yet");
                }
            }
        }

        DescriptorSetLayoutList.resize(1);
        for (auto& descriptorSetLayout : DescriptorSetLayoutList)
        {
            VkDescriptorSetLayoutCreateInfo descriptorSetLayoutCreateInfo = VkDescriptorSetLayoutCreateInfo
            {
                .sType = VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO,
                .pNext = nullptr,
                .flags = 0,
                .bindingCount = static_cast<uint32>(descriptorSetLayoutBindingList.size()),
                .pBindings = descriptorSetLayoutBindingList.data()
            };
            VULKAN_RESULT(vkCreateDescriptorSetLayout(cRenderer.Device, &descriptorSetLayoutCreateInfo, nullptr, &descriptorSetLayout));
        }
    }

    //AllocateDescriptorSets
    {
        DescriptorSetList.resize(DescriptorSetLayoutList.size());
        for (auto& descriptorSet : DescriptorSetList)
        {
            VkDescriptorSetAllocateInfo allocInfo =
            {
                .sType = VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO,
                .pNext = nullptr,
                .descriptorPool = DescriptorPool,
                .descriptorSetCount = static_cast<uint32>(DescriptorSetLayoutList.size()),
                .pSetLayouts = DescriptorSetLayoutList.data()
            };
            VULKAN_RESULT(vkAllocateDescriptorSets(cRenderer.Device, &allocInfo, &descriptorSet));
        }
    }

    //UpdateDescriptorSets
    {
        for (auto& descriptorSet : DescriptorSetList)
        {
            List<VkWriteDescriptorSet> writeDescriptorSet = List<VkWriteDescriptorSet>();
            for (auto binding : model.PipelineDescriptorModelsList)
            {
                switch (binding.BindingPropertiesList)
                {
                    case kMeshPropertiesDescriptor:
                    {
                        writeDescriptorSet.emplace_back(VkWriteDescriptorSet
                            {
                                .sType = VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                                .pNext = nullptr,
                                .dstSet = descriptorSet,
                                .dstBinding = binding.BindingNumber,
                                .dstArrayElement = 0,
                                .descriptorCount = static_cast<uint32>(meshProperties.size()),
                                .descriptorType = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                                .pImageInfo = nullptr,
                                .pBufferInfo = meshProperties.data(),
                                .pTexelBufferView = nullptr
                            });
                        break;
                    }
                    case kTextureDescriptor:
                    {
                        writeDescriptorSet.emplace_back(VkWriteDescriptorSet
                            {
                                .sType = VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                                .pNext = nullptr,
                                .dstSet = descriptorSet,
                                .dstBinding = binding.BindingNumber,
                                .dstArrayElement = 0,
                                .descriptorCount = static_cast<uint32>(TextureList.size()),
                                .descriptorType = VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                                .pImageInfo = TextureList.data(),
                                .pBufferInfo = nullptr,
                                .pTexelBufferView = nullptr
                            });

                        break;
                    }
                    case kMaterialDescriptor:
                    {
                        writeDescriptorSet.emplace_back(VkWriteDescriptorSet
                            {
                                .sType = VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                                .pNext = nullptr,
                                .dstSet = descriptorSet,
                                .dstBinding = binding.BindingNumber,
                                .dstArrayElement = 0,
                                .descriptorCount = static_cast<uint32>(materialProperties.size()),
                                .descriptorType = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                                .pImageInfo = nullptr,
                                .pBufferInfo = materialProperties.data(),
                                .pTexelBufferView = nullptr
                            });

                        break;
                    }
                    default:
                    {
                        throw std::runtime_error("Binding case hasn't been handled yet");
                    }
                }
            }
            vkUpdateDescriptorSets(cRenderer.Device, static_cast<uint32>(writeDescriptorSet.size()), writeDescriptorSet.data(), 0, nullptr);
        }
    }
}

void JsonPipeline::LoadPipeline(RenderPipelineModel model, VkRenderPass renderPass, uint constBufferSize)
{
    //PipelineLayout
    {
        List<VkPushConstantRange> pushConstantRangeList = List<VkPushConstantRange>();
        if (constBufferSize > 0)
        {
            pushConstantRangeList.emplace_back(VkPushConstantRange
                {
                    .stageFlags = VK_SHADER_STAGE_VERTEX_BIT | VK_SHADER_STAGE_FRAGMENT_BIT,
                    .offset = 0,
                    .size = constBufferSize
                });
        }

        VkPipelineLayoutCreateInfo pipelineLayoutInfo = VkPipelineLayoutCreateInfo
        {
            .sType = VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO,
            .pNext = nullptr,
            .flags = 0,
            .setLayoutCount = static_cast<uint32>(DescriptorSetLayoutList.size()),
            .pSetLayouts = DescriptorSetLayoutList.data(),
            .pushConstantRangeCount = static_cast<uint32>(pushConstantRangeList.size()),
            .pPushConstantRanges = pushConstantRangeList.data()
        };
        VULKAN_RESULT(vkCreatePipelineLayout(cRenderer.Device, &pipelineLayoutInfo, nullptr, &PipelineLayout));
    }

    //pipeline
    {
        List<VkVertexInputBindingDescription> vertexBinding = Vertex2D::GetBindingDescriptions();
        for (auto& instanceVar : SpriteInstanceVertex2D::GetBindingDescriptions())
        {
            vertexBinding.emplace_back(instanceVar);
        }

        List<VkVertexInputAttributeDescription> vertexAttribute = Vertex2D::GetAttributeDescriptions();
        for (auto& instanceVar : SpriteInstanceVertex2D::GetAttributeDescriptions())
        {
            vertexAttribute.emplace_back(instanceVar);
        }


        VkPipelineVertexInputStateCreateInfo vertexInputInfo = VkPipelineVertexInputStateCreateInfo
        {
            .sType = VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO,
            .pNext = nullptr,
            .flags = 0,
            .vertexBindingDescriptionCount = static_cast<uint>(vertexBinding.size()),
            .pVertexBindingDescriptions = vertexBinding.data(),
            .vertexAttributeDescriptionCount = static_cast<uint>(vertexAttribute.size()),
            .pVertexAttributeDescriptions = vertexAttribute.data()
        };

        List<VkViewport> viewPortList = model.ViewportList;
        List<VkRect2D> scissorList = model.ScissorList;
        VkPipelineViewportStateCreateInfo pipelineViewportStateCreateInfo = VkPipelineViewportStateCreateInfo
        {
                    .sType = VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO,
                    .pNext = nullptr,
                    .flags = 0,
                    .viewportCount = static_cast<uint32>(viewPortList.size() + 1),
                    .pViewports = viewPortList.data(),
                    .scissorCount = static_cast<uint32>(scissorList.size() + 1),
                    .pScissors = scissorList.data()
        };


        VkPipelineColorBlendStateCreateInfo pipelineColorBlendStateCreateInfoModel = model.PipelineColorBlendStateCreateInfoModel;
        pipelineColorBlendStateCreateInfoModel.attachmentCount = model.PipelineColorBlendAttachmentStateList.size();
        pipelineColorBlendStateCreateInfoModel.pAttachments = model.PipelineColorBlendAttachmentStateList.data();

        List<VkDynamicState> dynamicStateList = List<VkDynamicState>
        {
            VkDynamicState::VK_DYNAMIC_STATE_VIEWPORT,
            VkDynamicState::VK_DYNAMIC_STATE_SCISSOR
        };

        VkPipelineDynamicStateCreateInfo pipelineDynamicStateCreateInfo = VkPipelineDynamicStateCreateInfo
        {
            .sType = VK_STRUCTURE_TYPE_PIPELINE_DYNAMIC_STATE_CREATE_INFO,
            .pNext = nullptr,
            .flags = 0,
            .dynamicStateCount = static_cast<uint32>(dynamicStateList.size()),
            .pDynamicStates = dynamicStateList.data()
        };

        List<VkPipelineShaderStageCreateInfo> pipelineShaderStageCreateInfoList = List<VkPipelineShaderStageCreateInfo>
        {
            ShaderCompiler::CreateShader(model.VertexShaderPath, VK_SHADER_STAGE_VERTEX_BIT),
            ShaderCompiler::CreateShader(model.FragmentShaderPath, VK_SHADER_STAGE_FRAGMENT_BIT)
        };

        VkPipelineMultisampleStateCreateInfo pipelineMultisampleStateCreateInfo = model.PipelineMultisampleStateCreateInfo;
        pipelineMultisampleStateCreateInfo.pSampleMask = nullptr;

        VkGraphicsPipelineCreateInfo graphicsPipelineCreateInfo = VkGraphicsPipelineCreateInfo
        {
            .sType = VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO,
            .pNext = nullptr,
            .flags = 0,
            .stageCount = static_cast<uint32>(pipelineShaderStageCreateInfoList.size()),
            .pStages = pipelineShaderStageCreateInfoList.data(),
            .pVertexInputState = &vertexInputInfo,
            .pInputAssemblyState = &model.PipelineInputAssemblyStateCreateInfo,
            .pTessellationState = nullptr,
            .pViewportState = &pipelineViewportStateCreateInfo,
            .pRasterizationState = &model.PipelineRasterizationStateCreateInfo,
            .pMultisampleState = &pipelineMultisampleStateCreateInfo,
            .pDepthStencilState = &model.PipelineDepthStencilStateCreateInfo,
            .pColorBlendState = &pipelineColorBlendStateCreateInfoModel,
            .pDynamicState = &pipelineDynamicStateCreateInfo,
            .layout = PipelineLayout,
            .renderPass = renderPass,
            .subpass = 0,
            .basePipelineHandle = VK_NULL_HANDLE,
            .basePipelineIndex = 0,
        };

        VULKAN_RESULT(vkCreateGraphicsPipelines(cRenderer.Device, PipelineCache, 1, &graphicsPipelineCreateInfo, nullptr, &Pipeline));
        for (auto& shader : pipelineShaderStageCreateInfoList)
        {
            vkDestroyShaderModule(cRenderer.Device, shader.module, nullptr);
        }
    }
}

void JsonPipeline::Destroy()
{
    renderer.DestroyPipeline(Pipeline);
    renderer.DestroyPipelineLayout(PipelineLayout);
    renderer.DestroyPipelineCache(PipelineCache);
    renderer.DestroyDescriptorPool(DescriptorPool);
    for (auto& descriptorSet : DescriptorSetLayoutList)
    {
        renderer.DestroyDescriptorSetLayout(descriptorSet);
    }
}
