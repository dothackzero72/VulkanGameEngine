#include "VulkanPipeline.h"
#include "MemoryLeakDetector.h"

 VulkanPipelineDLL* VulkanPipeline_ConvertToVulkanRenderPipelineDLL(VulkanPipeline& renderPipeline)
{
     VulkanPipelineDLL* vulkanRenderPipelineDLL = new VulkanPipelineDLL();
     vulkanRenderPipelineDLL->RenderPipelineId = renderPipeline.RenderPipelineId;
     vulkanRenderPipelineDLL->DescriptorSetLayoutCount = renderPipeline.DescriptorSetLayoutList.size();
     vulkanRenderPipelineDLL->DescriptorSetCount = renderPipeline.DescriptorSetList.size();
     vulkanRenderPipelineDLL->DescriptorPool = renderPipeline.DescriptorPool;
     vulkanRenderPipelineDLL->DescriptorSetLayoutList = memoryLeakSystem.AddPtrBuffer<VkDescriptorSetLayout>(renderPipeline.DescriptorSetLayoutList.size());
     vulkanRenderPipelineDLL->DescriptorSetList = memoryLeakSystem.AddPtrBuffer<VkDescriptorSet>(renderPipeline.DescriptorSetList.size());
     vulkanRenderPipelineDLL->Pipeline = renderPipeline.Pipeline;
     vulkanRenderPipelineDLL->PipelineLayout = renderPipeline.PipelineLayout;
     vulkanRenderPipelineDLL->PipelineCache = renderPipeline.PipelineCache;

     if (vulkanRenderPipelineDLL->DescriptorSetLayoutCount > 0)
     {
         vulkanRenderPipelineDLL->DescriptorSetLayoutList = memoryLeakSystem.AddPtrBuffer<VkDescriptorSetLayout>(renderPipeline.DescriptorSetLayoutList.size());
         std::memcpy(vulkanRenderPipelineDLL->DescriptorSetLayoutList, renderPipeline.DescriptorSetLayoutList.data(), vulkanRenderPipelineDLL->DescriptorSetLayoutCount * sizeof(VkFramebuffer));
     }
     else
     {
         vulkanRenderPipelineDLL->DescriptorSetLayoutList = nullptr;
     }

     if (vulkanRenderPipelineDLL->DescriptorSetCount > 0)
     {
         vulkanRenderPipelineDLL->DescriptorSetList = memoryLeakSystem.AddPtrBuffer<VkDescriptorSet>(renderPipeline.DescriptorSetList.size());
         std::memcpy(vulkanRenderPipelineDLL->DescriptorSetList, renderPipeline.DescriptorSetList.data(), vulkanRenderPipelineDLL->DescriptorSetCount * sizeof(VkClearValue));
     }
     else
     {
         vulkanRenderPipelineDLL->DescriptorSetList = nullptr;
     }

     return vulkanRenderPipelineDLL;
 }

 VulkanPipeline VulkanPipeline_ConvertToVulkanPipelinePass(VulkanPipelineDLL* renderPipelineDLL)
 {
     if (!renderPipelineDLL)
     {
         throw std::invalid_argument("Null RenderPipelineDLL pointer");
     }

     VulkanPipeline vulkanPipeline = VulkanPipeline
     {
        .RenderPipelineId = renderPipelineDLL->RenderPipelineId,
        .DescriptorPool = renderPipelineDLL->DescriptorPool,
        .DescriptorSetLayoutList = Vector<VkDescriptorSetLayout>(renderPipelineDLL->DescriptorSetLayoutList, renderPipelineDLL->DescriptorSetLayoutList + renderPipelineDLL->DescriptorSetLayoutCount),
        .DescriptorSetList = Vector<VkDescriptorSet>(renderPipelineDLL->DescriptorSetList, renderPipelineDLL->DescriptorSetList + renderPipelineDLL->DescriptorSetCount),
        .Pipeline = renderPipelineDLL->Pipeline,
        .PipelineLayout = renderPipelineDLL->PipelineLayout,
    .   PipelineCache = renderPipelineDLL->PipelineCache
     };
     VulkanPipeline_DeleteVulkanRenderPassDLLPtrs(renderPipelineDLL);
     return vulkanPipeline;
 }

 void VulkanPipeline_DeleteVulkanRenderPassDLLPtrs(VulkanPipelineDLL* renderPipelineDLL)
 {
     if (renderPipelineDLL)
     {
         memoryLeakSystem.RemovePtrBuffer(renderPipelineDLL->DescriptorSetLayoutList);
         memoryLeakSystem.RemovePtrBuffer(renderPipelineDLL->DescriptorSetList);
         memoryLeakSystem.RemovePtrBuffer(renderPipelineDLL);
     }
 }

 VulkanPipelineDLL* VulkanPipeline_CreateRenderPipeline(VkDevice device, VkGuid& renderPassId, uint renderPipelineId, RenderPipelineModel& model, VkRenderPass renderPass, size_t constBufferSize, ivec2& renderPassResolution, const GPUIncludes& includes, VkPipelineShaderStageCreateInfo& pipelineShaderList, size_t pipelineShaderCount)
{
    Vector<VkPipelineShaderStageCreateInfo> pipelineShaders = Vector<VkPipelineShaderStageCreateInfo>(&pipelineShaderList, &pipelineShaderList + pipelineShaderCount);

    VulkanPipeline vulkanPipeline = {};
    vulkanPipeline.RenderPipelineId = renderPipelineId;
    vulkanPipeline.DescriptorPool = Pipeline_CreatePipelineDescriptorPool(device, model, includes);
    vulkanPipeline.DescriptorSetLayoutList = Pipeline_CreatePipelineDescriptorSetLayout(device, model, includes);
    vulkanPipeline.DescriptorSetList = Pipeline_AllocatePipelineDescriptorSets(device, vulkanPipeline.DescriptorPool, model, vulkanPipeline.DescriptorSetLayoutList);
    Pipeline_UpdatePipelineDescriptorSets(device, vulkanPipeline.DescriptorSetList, model, includes);
    vulkanPipeline.PipelineLayout = Pipeline_CreatePipelineLayout(device, vulkanPipeline.DescriptorSetLayoutList, constBufferSize);
    vulkanPipeline.Pipeline = Pipeline_CreatePipeline(device, renderPass, vulkanPipeline.PipelineLayout, vulkanPipeline.PipelineCache, model, renderPassResolution, pipelineShaders);
    return VulkanPipeline_ConvertToVulkanRenderPipelineDLL(vulkanPipeline);
}

void VulkanPipeline_RecreateSwapchain(VkRenderPass renderPass, uint constBufferSize, int newWidth, int newHeight)
{
    //GPUIncludes include =
    //{
    //    .vertexProperties = renderSystem.GetVertexPropertiesBuffer(),
    //    .indexProperties = renderSystem.GetIndexPropertiesBuffer(),
    //    //        .transformProperties = renderSystem.GetTransformPropertiesBuffer(gpuImport.MeshList),
    //    .meshProperties = renderSystem.GetMeshPropertiesBuffer(),
    //    .texturePropertiesList = renderSystem.GetTexturePropertiesBuffer(renderSystem.InputTextureList[RenderPipelineId]),
    //    .materialProperties = renderSystem.GetMaterialPropertiesBuffer()
    //};

    //Destroy();

    //ivec2 renderPassResolution = ivec2(newWidth, newHeight);
    //DescriptorPool = Pipeline_CreateDescriptorPool(*renderSystem.Device.get(), renderSystem.renderPipelineModelList[RenderPipelineId], include);
    //DescriptorSetLayoutList = Pipeline_CreateDescriptorSetLayout(*renderSystem.Device.get(), renderSystem.renderPipelineModelList[RenderPipelineId], include);
    //DescriptorSetList = Pipeline_AllocateDescriptorSets(*renderSystem.Device.get(), DescriptorPool, renderSystem.renderPipelineModelList[RenderPipelineId], DescriptorSetLayoutList);
    //Pipeline_UpdateDescriptorSets(*renderSystem.Device.get(), DescriptorSetList, renderSystem.renderPipelineModelList[RenderPipelineId], include);
    //PipelineLayout = Pipeline_CreatePipelineLayout(*renderSystem.Device.get(), DescriptorSetLayoutList, constBufferSize);
    //Pipeline = Pipeline_CreatePipeline(*renderSystem.Device.get(), renderPass, PipelineLayout, PipelineCache, renderSystem.renderPipelineModelList[RenderPipelineId], renderPassResolution);
}

void VulkanPipeline_Destroy(VkDevice device, VulkanPipelineDLL& vulkanPipelineDLL)
{
    VulkanPipeline vulkanPipeline = VulkanPipeline_ConvertToVulkanPipelinePass(&vulkanPipelineDLL);

    vulkanPipeline.RenderPipelineId = 0;
    Renderer_DestroyPipeline(device, &vulkanPipeline.Pipeline);
    Renderer_DestroyPipelineLayout(device, &vulkanPipeline.PipelineLayout);
    Renderer_DestroyPipelineCache(device, &vulkanPipeline.PipelineCache);
    Renderer_DestroyDescriptorPool(device, &vulkanPipeline.DescriptorPool);
    for (auto& descriptorSet : vulkanPipeline.DescriptorSetLayoutList)
    {
        Renderer_DestroyDescriptorSetLayout(device, &descriptorSet);
    }
}


VkDescriptorPool Pipeline_CreatePipelineDescriptorPool(VkDevice device, const RenderPipelineModel& model, const GPUIncludes& includes)
{
    Vector<VkDescriptorPoolSize> descriptorPoolSizeList = Vector<VkDescriptorPoolSize>();
    for (auto binding : model.PipelineDescriptorModelsList)
    {
        switch (binding.BindingPropertiesList)
        {
        case kVertexDescsriptor:
        {
            descriptorPoolSizeList.emplace_back(VkDescriptorPoolSize
                {
                    .type = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                    .descriptorCount = static_cast<uint32>(includes.vertexProperties.size())
                });
            break;
        }
        case kIndexDescriptor:
        {
            descriptorPoolSizeList.emplace_back(VkDescriptorPoolSize
                {
                    .type = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                    .descriptorCount = static_cast<uint32>(includes.indexProperties.size())
                });
            break;
        }
        case kTransformDescriptor:
        {
            descriptorPoolSizeList.emplace_back(VkDescriptorPoolSize
                {
                    .type = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                    .descriptorCount = static_cast<uint32>(includes.transformProperties.size())
                });
            break;
        }
        case kMeshPropertiesDescriptor:
        {
            descriptorPoolSizeList.emplace_back(VkDescriptorPoolSize
                {
                    .type = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                    .descriptorCount = static_cast<uint32>(includes.meshProperties.size())
                });
            break;
        }
        case kTextureDescriptor:
        {
            descriptorPoolSizeList.emplace_back(VkDescriptorPoolSize
                {
                    .type = VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                    .descriptorCount = static_cast<uint32>(includes.texturePropertiesList.size())
                });
            break;
        }
        case kMaterialDescriptor:
        {
            descriptorPoolSizeList.emplace_back(VkDescriptorPoolSize
                {
                    .type = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                    .descriptorCount = static_cast<uint32>(includes.materialProperties.size())
                });
            break;
        }
        default:
        {
            throw std::runtime_error("Binding case hasn't been handled yet");
        }
        }
    }

    VkDescriptorPool descriptorPool = VK_NULL_HANDLE;
    VkDescriptorPoolCreateInfo poolCreateInfo = VkDescriptorPoolCreateInfo
    {
        .sType = VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO,
        .pNext = nullptr,
        .flags = 0,
        .maxSets = 500,
        .poolSizeCount = static_cast<uint32>(descriptorPoolSizeList.size()),
        .pPoolSizes = descriptorPoolSizeList.data()
    };
    VULKAN_RESULT(vkCreateDescriptorPool(device, &poolCreateInfo, nullptr, &descriptorPool));
    return descriptorPool;
}

Vector<VkDescriptorSetLayout> Pipeline_CreatePipelineDescriptorSetLayout(VkDevice device, const RenderPipelineModel& model, const GPUIncludes& includes)
{
    Vector<VkDescriptorSetLayoutBinding> descriptorSetLayoutBindingList = Vector<VkDescriptorSetLayoutBinding>();
    for (auto binding : model.PipelineDescriptorModelsList)
    {
        switch (binding.BindingPropertiesList)
        {
        case kVertexDescsriptor:
        {
            descriptorSetLayoutBindingList.emplace_back(VkDescriptorSetLayoutBinding
                {
                    .binding = binding.BindingNumber,
                    .descriptorType = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                    .descriptorCount = static_cast<uint32>(includes.meshProperties.size()),
                    .stageFlags = VK_SHADER_STAGE_ALL,
                    .pImmutableSamplers = nullptr
                });
            break;
        }
        case kIndexDescriptor:
        {
            descriptorSetLayoutBindingList.emplace_back(VkDescriptorSetLayoutBinding
                {
                    .binding = binding.BindingNumber,
                    .descriptorType = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                    .descriptorCount = static_cast<uint32>(includes.meshProperties.size()),
                    .stageFlags = VK_SHADER_STAGE_ALL,
                    .pImmutableSamplers = nullptr
                });
            break;
        }
        case kTransformDescriptor:
        {
            descriptorSetLayoutBindingList.emplace_back(VkDescriptorSetLayoutBinding
                {
                    .binding = binding.BindingNumber,
                    .descriptorType = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                    .descriptorCount = static_cast<uint32>(includes.transformProperties.size()),
                    .stageFlags = VK_SHADER_STAGE_ALL,
                    .pImmutableSamplers = nullptr
                });
            break;
        }
        case kMeshPropertiesDescriptor:
        {
            descriptorSetLayoutBindingList.emplace_back(VkDescriptorSetLayoutBinding
                {
                    .binding = binding.BindingNumber,
                    .descriptorType = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                    .descriptorCount = static_cast<uint32>(includes.meshProperties.size()),
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
                    .descriptorCount = static_cast<uint32>(includes.texturePropertiesList.size()),
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
                    .descriptorCount = static_cast<uint32>(includes.materialProperties.size()),
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

    VkDescriptorSetLayoutCreateInfo descriptorSetLayoutCreateInfo = VkDescriptorSetLayoutCreateInfo
    {
        .sType = VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO,
        .pNext = nullptr,
        .flags = 0,
        .bindingCount = static_cast<uint32>(descriptorSetLayoutBindingList.size()),
        .pBindings = descriptorSetLayoutBindingList.data()
    };

    Vector<VkDescriptorSetLayout> descriptorSetLayoutList = Vector<VkDescriptorSetLayout>(model.DescriptorSetLayoutCount);
    for (auto& descriptorSetLayout : descriptorSetLayoutList)
    {
        VULKAN_RESULT(vkCreateDescriptorSetLayout(device, &descriptorSetLayoutCreateInfo, nullptr, &descriptorSetLayout));
    }

    return descriptorSetLayoutList;
}

Vector<VkDescriptorSet> Pipeline_AllocatePipelineDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, const RenderPipelineModel& model, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList)
{
    VkDescriptorSetAllocateInfo allocInfo = {
        .sType = VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO,
        .pNext = nullptr,
        .descriptorPool = descriptorPool,
        .descriptorSetCount = static_cast<uint32>(descriptorSetLayoutList.size()),
        .pSetLayouts = descriptorSetLayoutList.data()
    };

    Vector<VkDescriptorSet> descriptorSetList = Vector<VkDescriptorSet>(model.DescriptorSetCount);
    for (auto& descriptorSet : descriptorSetList)
    {
        VULKAN_RESULT(vkAllocateDescriptorSets(device, &allocInfo, &descriptorSet));
    }
    return descriptorSetList;
}

void Pipeline_UpdatePipelineDescriptorSets(VkDevice device, const Vector<VkDescriptorSet>& descriptorSetList, const RenderPipelineModel& model, const GPUIncludes& includes)
{
    for (auto& descriptorSet : descriptorSetList)
    {
        Vector<VkWriteDescriptorSet> writeDescriptorSet = Vector<VkWriteDescriptorSet>();
        for (auto binding : model.PipelineDescriptorModelsList)
        {
            switch (binding.BindingPropertiesList)
            {
            case kVertexDescsriptor:
            {
                writeDescriptorSet.emplace_back(VkWriteDescriptorSet
                    {
                        .sType = VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                        .pNext = nullptr,
                        .dstSet = descriptorSet,
                        .dstBinding = binding.BindingNumber,
                        .dstArrayElement = 0,
                        .descriptorCount = static_cast<uint32>(includes.meshProperties.size()),
                        .descriptorType = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                        .pImageInfo = nullptr,
                        .pBufferInfo = includes.meshProperties.data(),
                        .pTexelBufferView = nullptr
                    });
                break;
            }
            case kIndexDescriptor:
            {
                writeDescriptorSet.emplace_back(VkWriteDescriptorSet
                    {
                        .sType = VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                        .pNext = nullptr,
                        .dstSet = descriptorSet,
                        .dstBinding = binding.BindingNumber,
                        .dstArrayElement = 0,
                        .descriptorCount = static_cast<uint32>(includes.meshProperties.size()),
                        .descriptorType = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                        .pImageInfo = nullptr,
                        .pBufferInfo = includes.meshProperties.data(),
                        .pTexelBufferView = nullptr
                    });
                break;
            }
            case kTransformDescriptor:
            {
                writeDescriptorSet.emplace_back(VkWriteDescriptorSet
                    {
                        .sType = VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                        .pNext = nullptr,
                        .dstSet = descriptorSet,
                        .dstBinding = binding.BindingNumber,
                        .dstArrayElement = 0,
                        .descriptorCount = static_cast<uint32>(includes.transformProperties.size()),
                        .descriptorType = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                        .pImageInfo = nullptr,
                        .pBufferInfo = includes.transformProperties.data(),
                        .pTexelBufferView = nullptr
                    });
                break;
            }
            case kMeshPropertiesDescriptor:
            {
                writeDescriptorSet.emplace_back(VkWriteDescriptorSet
                    {
                        .sType = VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                        .pNext = nullptr,
                        .dstSet = descriptorSet,
                        .dstBinding = binding.BindingNumber,
                        .dstArrayElement = 0,
                        .descriptorCount = static_cast<uint32>(includes.meshProperties.size()),
                        .descriptorType = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                        .pImageInfo = nullptr,
                        .pBufferInfo = includes.meshProperties.data(),
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
                        .descriptorCount = static_cast<uint32>(includes.texturePropertiesList.size()),
                        .descriptorType = VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                        .pImageInfo = includes.texturePropertiesList.data(),
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
                        .descriptorCount = static_cast<uint32>(includes.materialProperties.size()),
                        .descriptorType = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                        .pImageInfo = nullptr,
                        .pBufferInfo = includes.materialProperties.data(),
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
        vkUpdateDescriptorSets(device, static_cast<uint32>(writeDescriptorSet.size()), writeDescriptorSet.data(), 0, nullptr);
    }
}

VkPipelineLayout Pipeline_CreatePipelineLayout(VkDevice device, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList, uint constBufferSize)
{
    VkPipelineLayout pipelineLayout = VK_NULL_HANDLE;
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
        .setLayoutCount = static_cast<uint32>(descriptorSetLayoutList.size()),
        .pSetLayouts = descriptorSetLayoutList.data(),
        .pushConstantRangeCount = static_cast<uint32>(pushConstantRangeList.size()),
        .pPushConstantRanges = pushConstantRangeList.data()
    };
    VULKAN_RESULT(vkCreatePipelineLayout(device, &pipelineLayoutInfo, nullptr, &pipelineLayout));
    return pipelineLayout;
}

VkPipeline Pipeline_CreatePipeline(VkDevice device, VkRenderPass renderpass, VkPipelineLayout pipelineLayout, VkPipelineCache pipelineCache, const RenderPipelineModel& model, ivec2& extent, Vector<VkPipelineShaderStageCreateInfo>& pipelineShaders)
{
    VkPipeline pipeline = VK_NULL_HANDLE;
    VkPipelineVertexInputStateCreateInfo vertexInputInfo = VkPipelineVertexInputStateCreateInfo
    {
        .sType = VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO,
        .pNext = nullptr,
        .flags = 0,
        .vertexBindingDescriptionCount = static_cast<uint>(model.VertexInputBindingDescriptionList.size()),
        .pVertexBindingDescriptions = model.VertexInputBindingDescriptionList.data(),
        .vertexAttributeDescriptionCount = static_cast<uint>(model.VertexInputAttributeDescriptionList.size()),
        .pVertexAttributeDescriptions = model.VertexInputAttributeDescriptionList.data()
    };

    VkPipelineColorBlendStateCreateInfo pipelineColorBlendStateCreateInfoModel = model.PipelineColorBlendStateCreateInfoModel;
    pipelineColorBlendStateCreateInfoModel.attachmentCount = model.PipelineColorBlendAttachmentStateList.size();
    pipelineColorBlendStateCreateInfoModel.pAttachments = model.PipelineColorBlendAttachmentStateList.data();

    Vector<VkViewport> viewPortList;
    Vector<VkRect2D> scissorList;
    Vector<VkDynamicState> dynamicStateList;
    if (model.ViewportList.empty())
    {
        dynamicStateList = Vector<VkDynamicState>
        {
            VkDynamicState::VK_DYNAMIC_STATE_VIEWPORT,
            VkDynamicState::VK_DYNAMIC_STATE_SCISSOR
        };
    }
    else
    {
        for (auto& viewPort : model.ViewportList)
        {
            viewPortList.emplace_back(VkViewport
                {
                    .x = viewPort.x,
                    .y = viewPort.y,
                    .width = static_cast<float>(extent.x),
                    .height = static_cast<float>(extent.y),
                    .minDepth = viewPort.minDepth,
                    .maxDepth = viewPort.maxDepth
                });
        }
        for (auto& viewPort : model.ViewportList)
        {
            scissorList.emplace_back(VkRect2D
                {
                    .offset = VkOffset2D
                    {
                        .x = 0,
                        .y = 0
                    },
                    .extent = VkExtent2D
                    {
                        .width = static_cast<uint32>(extent.x),
                        .height = static_cast<uint32>(extent.y)
                    }
                });
        }
    }

    VkPipelineViewportStateCreateInfo pipelineViewportStateCreateInfo = VkPipelineViewportStateCreateInfo
    {
        .sType = VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO,
        .pNext = nullptr,
        .flags = 0,
        .viewportCount = static_cast<uint32>(viewPortList.size() + (model.ViewportList.empty() ? 1 : 0)),
        .pViewports = viewPortList.data(),
        .scissorCount = static_cast<uint32>(scissorList.size() + (model.ScissorList.empty() ? 1 : 0)),
        .pScissors = scissorList.data()
    };

    VkPipelineDynamicStateCreateInfo pipelineDynamicStateCreateInfo = VkPipelineDynamicStateCreateInfo
    {
        .sType = VK_STRUCTURE_TYPE_PIPELINE_DYNAMIC_STATE_CREATE_INFO,
        .pNext = nullptr,
        .flags = 0,
        .dynamicStateCount = static_cast<uint32>(dynamicStateList.size()),
        .pDynamicStates = dynamicStateList.data()
    };

    Vector<VkPipelineShaderStageCreateInfo> pipelineShaderStageCreateInfoList = pipelineShaders;

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
        .layout = pipelineLayout,
        .renderPass = renderpass,
        .subpass = 0,
        .basePipelineHandle = VK_NULL_HANDLE,
        .basePipelineIndex = 0,
    };

    VULKAN_RESULT(vkCreateGraphicsPipelines(device, pipelineCache, 1, &graphicsPipelineCreateInfo, nullptr, &pipeline));
    for (auto& shader : pipelineShaderStageCreateInfoList)
    {
        vkDestroyShaderModule(device, shader.module, nullptr);
    }

    return pipeline;
}
