#include "JsonPipeline.h"
#include "MemoryManager.h"
#include "ShaderCompiler.h"

JsonPipeline::JsonPipeline()
{
}

JsonPipeline::JsonPipeline(String jsonPath, VkRenderPass renderPass, GPUImport& gpuImport, uint constBufferSize)
{
  //  ParentRenderPass = parentRenderPass;
    nlohmann::json json = Json::ReadJson("../Pipelines/Default2DPipeline.json");

    RenderPipelineModel renderPipelineModel = RenderPipelineModel::from_json(json);
    LoadDescriptorSets(renderPipelineModel, gpuImport);
    LoadPipeline(renderPipelineModel, renderPass, constBufferSize);
}

JsonPipeline::~JsonPipeline()
{
}

void JsonPipeline::LoadDescriptorSets(RenderPipelineModel& model, GPUImport& gpuImport)
{
    Vector<VkDescriptorBufferInfo> meshProperties = GetMeshPropertiesBuffer(gpuImport.MeshList);
    Vector<VkDescriptorImageInfo> textureList = GetTexturePropertiesBuffer(gpuImport.TextureList);
    Vector<VkDescriptorBufferInfo> materialProperties = GetMaterialPropertiesBuffer(gpuImport.MaterialList);

    //CreateDescriptorPool
    {
        Vector<VkDescriptorPoolSize> descriptorPoolSizeList = Vector<VkDescriptorPoolSize>();
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
                            .descriptorCount = static_cast<uint32>(textureList.size())
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
        Vector<VkDescriptorSetLayoutBinding> descriptorSetLayoutBindingList = Vector<VkDescriptorSetLayoutBinding>();
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
                            .descriptorCount = static_cast<uint32>(textureList.size()),
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
            Vector<VkWriteDescriptorSet> writeDescriptorSet = Vector<VkWriteDescriptorSet>();
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
                                .descriptorCount = static_cast<uint32>(textureList.size()),
                                .descriptorType = VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                                .pImageInfo = textureList.data(),
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

void JsonPipeline::LoadPipeline(RenderPipelineModel& model, VkRenderPass renderPass, uint constBufferSize)
{
    //PipelineLayout
    {
        Vector<VkPushConstantRange> pushConstantRangeList = Vector<VkPushConstantRange>();
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
        Vector<VkVertexInputBindingDescription> vertexBinding = Vertex2D::GetBindingDescriptions();
        for (auto& instanceVar : SpriteInstanceVertex2D::GetBindingDescriptions())
        {
            vertexBinding.emplace_back(instanceVar);
        }

        Vector<VkVertexInputAttributeDescription> vertexAttribute = Vertex2D::GetAttributeDescriptions();
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

        Vector<VkViewport> viewPortList = model.ViewportList;
        Vector<VkRect2D> scissorList = model.ScissorList;
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

        Vector<VkDynamicState> dynamicStateList = Vector<VkDynamicState>
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

        Vector<VkPipelineShaderStageCreateInfo> pipelineShaderStageCreateInfoList = Vector<VkPipelineShaderStageCreateInfo>
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

const Vector<VkDescriptorImageInfo> JsonPipeline::GetTexturePropertiesBuffer(Vector<SharedPtr<Texture>>& textureList)
{
    Vector<VkDescriptorImageInfo>	texturePropertiesBuffer;
    if (textureList.size() == 0)
    {
        VkSamplerCreateInfo NullSamplerInfo = 
        {
            .sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
            .magFilter = VK_FILTER_NEAREST,
            .minFilter = VK_FILTER_NEAREST,
            .mipmapMode = VK_SAMPLER_MIPMAP_MODE_LINEAR,
            .addressModeU = VK_SAMPLER_ADDRESS_MODE_REPEAT,
            .addressModeV = VK_SAMPLER_ADDRESS_MODE_REPEAT,
            .addressModeW = VK_SAMPLER_ADDRESS_MODE_REPEAT,
            .mipLodBias = 0,
            .anisotropyEnable = VK_TRUE,
            .maxAnisotropy = 16.0f,
            .compareEnable = VK_FALSE,
            .compareOp = VK_COMPARE_OP_ALWAYS,
            .minLod = 0,
            .maxLod = 0,
            .borderColor = VK_BORDER_COLOR_INT_OPAQUE_BLACK,
            .unnormalizedCoordinates = VK_FALSE,
        };

        VkSampler nullSampler = VK_NULL_HANDLE;
        if (vkCreateSampler(cRenderer.Device, &NullSamplerInfo, nullptr, &nullSampler))
        {
            throw std::runtime_error("Failed to create Sampler.");
        }

        VkDescriptorImageInfo nullBuffer = 
        {
            .sampler = nullSampler,
            .imageView = VK_NULL_HANDLE,
            .imageLayout = VK_IMAGE_LAYOUT_UNDEFINED,
        };
        texturePropertiesBuffer.emplace_back(nullBuffer);
    }
    else
    {
        for (auto& texture : textureList)
        {
            texture->GetTexturePropertiesBuffer(texturePropertiesBuffer);
        }
    }

    return texturePropertiesBuffer;
}

const Vector<VkDescriptorBufferInfo> JsonPipeline::GetMaterialPropertiesBuffer(Vector<SharedPtr<Material>>& materialList)
{
    std::vector<VkDescriptorBufferInfo>	materialPropertiesBuffer;
    for (auto& material : materialList)
    {
        material->GetMaterialPropertiesBuffer(materialPropertiesBuffer);
    }
    return materialPropertiesBuffer;
}
