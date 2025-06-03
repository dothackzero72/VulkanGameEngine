#include "renderSystem.h"
#include "json.h"
#include "TextureSystem.h"
#include "ShaderSystem.h"
#include "VulkanBufferSystem.h"
#include "MeshSystem.h"
#include "GameObjectSystem.h"

RenderSystem renderSystem = RenderSystem();

RenderSystem::RenderSystem()
{

}

RenderSystem::~RenderSystem()
{

}

void RenderSystem::StartUp()
{
    renderer.RendererSetUp(vulkanWindow->WindowHandle);
    imGuiRenderer = ImGui_StartUp(cRenderer);
    shaderSystem.StartUp();
}

void RenderSystem::Update(const float& deltaTime)
{
    if (cRenderer.RebuildRendererFlag)
    {
        int width = cRenderer.SwapChainResolution.width;
        int height = cRenderer.SwapChainResolution.height;
        RecreateSwapchain();
        cRenderer.RebuildRendererFlag = false;
    }

    VkCommandBuffer commandBuffer = renderer.BeginSingleTimeCommands();
    for (auto& renderPass : RenderPassMap)
    {
        if (levelSystem.SpriteBatchLayerList.find(renderPass.second.RenderPassId) != levelSystem.SpriteBatchLayerList.end())
        {
            for (auto& spriteLayer : levelSystem.SpriteBatchLayerList[renderPass.second.RenderPassId])
            {
                spriteLayer.Update(commandBuffer, deltaTime);
            }
        }
    }
    renderer.EndSingleTimeCommands(commandBuffer);
}

void RenderSystem::CreateVulkanRenderPass(RenderPassBuildInfoModel& model, ivec2& renderPassResolution)
{
    Vector<Texture> renderedTextureList;
    Texture depthTexture = Texture();
    RenderPassMap[model.RenderPassId] = RenderPass_CreateVulkanRenderPass(cRenderer, model, renderPassResolution, sizeof(SceneDataBuffer), renderedTextureList, depthTexture);
    textureSystem.AddRenderedTexture(model.RenderPassId, renderedTextureList);
    if (depthTexture.textureView != VK_NULL_HANDLE)
    {
        textureSystem.AddDepthTexture(model.RenderPassId, depthTexture);
    }
}

void RenderSystem::CreateVulkanRenderPass(RenderPassBuildInfoModel& model, Texture& inputTexture, ivec2& renderPassResolution)
{
    Vector<Texture> renderedTextureList;
    Texture depthTexture = Texture();
    RenderPassMap[model.RenderPassId] = RenderPass_CreateVulkanRenderPass(cRenderer, model, renderPassResolution, sizeof(SceneDataBuffer), renderedTextureList, depthTexture);
    textureSystem.AddRenderedTexture(model.RenderPassId, renderedTextureList);
    if (depthTexture.textureView != VK_NULL_HANDLE)
    {
        textureSystem.AddDepthTexture(model.RenderPassId, depthTexture);
    }
}

void RenderSystem::RecreateSwapchain()
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

VkCommandBuffer RenderSystem::RenderFrameBuffer(VkGuid& renderPassId)
{
    const VulkanRenderPass renderPass = FindRenderPass(renderPassId);
    const VulkanPipeline& pipeline = FindRenderPipelineList(renderPassId)[0];
    const VkCommandBuffer& commandBuffer = renderPass.CommandBuffer;

    VkRenderPassBeginInfo renderPassBeginInfo = VkRenderPassBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
        .renderPass = renderPass.RenderPass,
        .framebuffer = renderPass.FrameBufferList[cRenderer.ImageIndex],
        .renderArea = renderPass.RenderArea,
        .clearValueCount = static_cast<uint32>(renderPass.ClearValueList.size()),
        .pClearValues = renderPass.ClearValueList.data()
    };

    VULKAN_RESULT(vkResetCommandBuffer(commandBuffer, 0));
    VULKAN_RESULT(vkBeginCommandBuffer(commandBuffer, &CommandBufferBeginInfo));
    vkCmdBeginRenderPass(commandBuffer, &renderPassBeginInfo, VK_SUBPASS_CONTENTS_INLINE);
    vkCmdBindPipeline(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline.Pipeline);
    vkCmdBindDescriptorSets(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline.PipelineLayout, 0, pipeline.DescriptorSetList.size(), pipeline.DescriptorSetList.data(), 0, nullptr);
    vkCmdDraw(commandBuffer, 6, 1, 0, 0);
    vkCmdEndRenderPass(commandBuffer);
    vkEndCommandBuffer(commandBuffer);
    return commandBuffer;
}

VkCommandBuffer RenderSystem::RenderLevel(VkGuid& renderPassId, VkGuid& levelId, const float deltaTime, SceneDataBuffer& sceneDataBuffer)
{
    const VulkanRenderPass& renderPass = FindRenderPass(renderPassId);
    const VulkanPipeline& spritePipeline = FindRenderPipelineList(renderPassId)[0];
    const VulkanPipeline& levelPipeline = FindRenderPipelineList(renderPassId)[1];
    const Vector<SpriteBatchLayer>& spriteLayerList = levelSystem.SpriteBatchLayerList[renderPassId];
    const Vector<Mesh>& levelLayerList = meshSystem.FindLevelLayerMeshList(levelId);
    const VkCommandBuffer& commandBuffer = renderPass.CommandBuffer;

    VkRenderPassBeginInfo renderPassBeginInfo = VkRenderPassBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
        .renderPass = renderPass.RenderPass,
        .framebuffer = renderPass.FrameBufferList[cRenderer.ImageIndex],
        .renderArea = renderPass.RenderArea,
        .clearValueCount = static_cast<uint32>(renderPass.ClearValueList.size()),
        .pClearValues = renderPass.ClearValueList.data()
    };

    VULKAN_RESULT(vkResetCommandBuffer(commandBuffer, 0));
    VULKAN_RESULT(vkBeginCommandBuffer(commandBuffer, &CommandBufferBeginInfo));
    vkCmdBeginRenderPass(commandBuffer, &renderPassBeginInfo, VK_SUBPASS_CONTENTS_INLINE);

    for (auto levelLayer : levelLayerList)
    {
        const VkBuffer& meshVertexBuffer = bufferSystem.VulkanBuffer[levelLayer.MeshVertexBufferId].Buffer;
        const VkBuffer& meshIndexBuffer = bufferSystem.VulkanBuffer[levelLayer.MeshIndexBufferId].Buffer;

        VkDeviceSize offsets[] = { 0 };
        vkCmdPushConstants(commandBuffer, levelPipeline.PipelineLayout, VK_SHADER_STAGE_VERTEX_BIT | VK_SHADER_STAGE_FRAGMENT_BIT, 0, sizeof(SceneDataBuffer), &sceneDataBuffer);
        vkCmdBindPipeline(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, levelPipeline.Pipeline);
        vkCmdBindDescriptorSets(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, levelPipeline.PipelineLayout, 0, levelPipeline.DescriptorSetList.size(), levelPipeline.DescriptorSetList.data(), 0, nullptr);
        vkCmdBindVertexBuffers(commandBuffer, 0, 1, &meshVertexBuffer, offsets);
        vkCmdBindIndexBuffer(commandBuffer, meshIndexBuffer, 0, VK_INDEX_TYPE_UINT32);
        vkCmdDrawIndexed(commandBuffer, levelLayer.IndexCount, 1, 0, 0, 0);
    }
    for (auto spriteLayer : spriteLayerList)
    {
        const Vector<SpriteInstanceStruct>& spriteInstanceList = levelSystem.SpriteInstanceList[spriteLayer.SpriteBatchLayerID];
        const Mesh& spriteMesh = meshSystem.FindSpriteMesh(spriteLayer.SpriteLayerMeshId);
        const VkBuffer& meshVertexBuffer = bufferSystem.VulkanBuffer[spriteMesh.MeshVertexBufferId].Buffer;
        const VkBuffer& meshIndexBuffer = bufferSystem.VulkanBuffer[spriteMesh.MeshIndexBufferId].Buffer;
        const VkBuffer& spriteInstanceBuffer = bufferSystem.VulkanBuffer[levelSystem.SpriteInstanceBufferList[spriteLayer.SpriteBatchLayerID]].Buffer;

        VkDeviceSize offsets[] = { 0 };
        vkCmdPushConstants(commandBuffer, spritePipeline.PipelineLayout, VK_SHADER_STAGE_VERTEX_BIT | VK_SHADER_STAGE_FRAGMENT_BIT, 0, sizeof(SceneDataBuffer), &sceneDataBuffer);
        vkCmdBindPipeline(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, spritePipeline.Pipeline);
        vkCmdBindDescriptorSets(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, spritePipeline.PipelineLayout, 0, spritePipeline.DescriptorSetList.size(), spritePipeline.DescriptorSetList.data(), 0, nullptr);
        vkCmdBindVertexBuffers(commandBuffer, 0, 1, &meshVertexBuffer, offsets);
        vkCmdBindVertexBuffers(commandBuffer, 1, 1, &spriteInstanceBuffer, offsets);
        vkCmdBindIndexBuffer(commandBuffer, meshIndexBuffer, 0, VK_INDEX_TYPE_UINT32);
        vkCmdDrawIndexed(commandBuffer, gameObjectSystem.SpriteIndexList.size(), spriteInstanceList.size(), 0, 0, 0);
    }
    vkCmdEndRenderPass(commandBuffer);
    vkEndCommandBuffer(commandBuffer);
    return commandBuffer;
}

VkGuid RenderSystem::LoadRenderPass(VkGuid& levelId, const String& jsonPath, ivec2 renderPassResolution)
{
    nlohmann::json json = Json::ReadJson(jsonPath);

    RenderPassBuildInfoModel model;
    model._name = json["_name"];
    model.RenderPassId = VkGuid(json["RenderPassId"].get<String>().c_str());
    model.IsRenderedToSwapchain = json["IsRenderedToSwapchain"].get<bool>();
    model.RenderArea = RenderAreaModel::from_json(json["RenderArea"], renderPassResolution);
    for (int x = 0; x < json["RenderPipelineList"].size(); x++)
    {
        model.RenderPipelineList.emplace_back(json["RenderPipelineList"][x]);
    }
    for (int x = 0; x < json["RenderedTextureInfoModelList"].size(); x++)
    {
        model.RenderedTextureInfoModelList.emplace_back(RenderedTextureInfoModel::from_json(json["RenderedTextureInfoModelList"][x], renderPassResolution));
    }
    for (int x = 0; x < json["SubpassDependencyList"].size(); x++)
    {
        model.SubpassDependencyModelList.emplace_back(Json::LoadSubpassDependency(json["SubpassDependencyList"][x]));
    }
    for (int x = 0; x < json["ClearValueList"].size(); x++)
    {
        model.ClearValueList.emplace_back(Json::LoadClearValue(json["ClearValueList"][x]));
    }

    Vector<Texture> renderedTextureList;
    Texture depthTexture = Texture();
    RenderPassMap[model.RenderPassId] = RenderPass_CreateVulkanRenderPass(cRenderer, model, renderPassResolution, sizeof(SceneDataBuffer), renderedTextureList, depthTexture);
    textureSystem.AddRenderedTexture(model.RenderPassId, renderedTextureList);
    if (depthTexture.textureView != VK_NULL_HANDLE)
    {
        textureSystem.AddDepthTexture(model.RenderPassId, depthTexture);
    }
    CreateVulkanRenderPass(model, renderPassResolution);

    for (int x = 0; x < model.RenderPipelineList.size(); x++)
    {
        uint pipeLineId = renderSystem.RenderPassMap.size();
        nlohmann::json json = Json::ReadJson(model.RenderPipelineList[x]);
        RenderPipelineModel renderPipelineModel = RenderPipelineModel::from_json(json);
        GPUIncludes include =
        {
            .vertexProperties = renderSystem.GetVertexPropertiesBuffer(),
            .indexProperties = renderSystem.GetIndexPropertiesBuffer(),
            .transformProperties = renderSystem.GetGameObjectTransformBuffer(),
            .meshProperties = renderSystem.GetMeshPropertiesBuffer(levelId),
            .texturePropertiesList = renderSystem.GetTexturePropertiesBuffer(model.RenderPassId, nullptr),
            .materialProperties = materialSystem.GetMaterialPropertiesBuffer()
        };

        Vector<VkPipelineShaderStageCreateInfo> pipelineShaderStageCreateInfoList = Vector<VkPipelineShaderStageCreateInfo>
        {
            shaderSystem.CreateShader(cRenderer.Device, renderPipelineModel.VertexShaderPath, VK_SHADER_STAGE_VERTEX_BIT),
            shaderSystem.CreateShader(cRenderer.Device, renderPipelineModel.FragmentShaderPath, VK_SHADER_STAGE_FRAGMENT_BIT)
        };

        RenderPipelineMap[model.RenderPassId].emplace_back(Pipeline_CreateRenderPipeline(cRenderer.Device, model.RenderPassId, pipeLineId, renderPipelineModel, RenderPassMap[model.RenderPassId].RenderPass, sizeof(SceneDataBuffer), renderPassResolution, include, pipelineShaderStageCreateInfoList));
    }

    return model.RenderPassId;
}

VkGuid RenderSystem::LoadRenderPass(VkGuid& levelId, const String& jsonPath, Texture& inputTexture, ivec2 renderPassResolution)
{
    nlohmann::json json = Json::ReadJson(jsonPath);

    RenderPassBuildInfoModel model;
    model._name = json["_name"];
    model.RenderPassId = VkGuid(json["RenderPassId"].get<String>().c_str());
    model.IsRenderedToSwapchain = json["IsRenderedToSwapchain"].get<bool>();
    model.RenderArea = RenderAreaModel::from_json(json["RenderArea"], renderPassResolution);
    for (int x = 0; x < json["RenderPipelineList"].size(); x++)
    {
        model.RenderPipelineList.emplace_back(json["RenderPipelineList"][x]);
    }
    for (int x = 0; x < json["RenderedTextureInfoModelList"].size(); x++)
    {
        model.RenderedTextureInfoModelList.emplace_back(RenderedTextureInfoModel::from_json(json["RenderedTextureInfoModelList"][x], renderPassResolution));
    }
    for (int x = 0; x < json["SubpassDependencyList"].size(); x++)
    {
        model.SubpassDependencyModelList.emplace_back(Json::LoadSubpassDependency(json["SubpassDependencyList"][x]));
    }
    for (int x = 0; x < json["ClearValueList"].size(); x++)
    {
        model.ClearValueList.emplace_back(Json::LoadClearValue(json["ClearValueList"][x]));
    }

    CreateVulkanRenderPass(model, inputTexture, renderPassResolution);
    for (int x = 0; x < model.RenderPipelineList.size(); x++)
    {
        uint pipeLineId = renderSystem.RenderPipelineMap.size();
        nlohmann::json json = Json::ReadJson(model.RenderPipelineList[x]);
        RenderPipelineModel renderPipelineModel = RenderPipelineModel::from_json(json);

        GPUIncludes include =
        {
            .vertexProperties = renderSystem.GetVertexPropertiesBuffer(),
            .indexProperties = renderSystem.GetIndexPropertiesBuffer(),
            .transformProperties = renderSystem.GetGameObjectTransformBuffer(),
            .meshProperties = renderSystem.GetMeshPropertiesBuffer(levelId),
            .texturePropertiesList = renderSystem.GetTexturePropertiesBuffer(model.RenderPassId, &inputTexture),
            .materialProperties = materialSystem.GetMaterialPropertiesBuffer()
        };

        Vector<VkPipelineShaderStageCreateInfo> pipelineShaderStageCreateInfoList = Vector<VkPipelineShaderStageCreateInfo>
        {
            shaderSystem.CreateShader(cRenderer.Device, renderPipelineModel.VertexShaderPath, VK_SHADER_STAGE_VERTEX_BIT),
            shaderSystem.CreateShader(cRenderer.Device, renderPipelineModel.FragmentShaderPath, VK_SHADER_STAGE_FRAGMENT_BIT)
        };
        RenderPipelineMap[model.RenderPassId].emplace_back(Pipeline_CreateRenderPipeline(cRenderer.Device, model.RenderPassId, pipeLineId, renderPipelineModel, RenderPassMap[model.RenderPassId].RenderPass, sizeof(SceneDataBuffer), renderPassResolution, include, pipelineShaderStageCreateInfoList));
    }

    return model.RenderPassId;
}

const Vector<VkDescriptorBufferInfo> RenderSystem::GetVertexPropertiesBuffer()
{
    //Vector<MeshStruct> meshList;
    //meshList.reserve(meshSystem.SpriteMeshList.size());
    //std::transform(meshSystem.SpriteMeshList.begin(), meshSystem.SpriteMeshList.end(),
    //    std::back_inserter(meshList),
    //    [](const auto& pair) { return pair.second; });


    Vector<VkDescriptorBufferInfo> vertexPropertiesBuffer;
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

const Vector<VkDescriptorBufferInfo> RenderSystem::GetIndexPropertiesBuffer()
{
    //Vector<MeshStruct> meshList;
    //meshList.reserve(meshSystem.SpriteMeshList.size());
    //std::transform(meshSystem.SpriteMeshList.begin(), meshSystem.SpriteMeshList.end(),
    //    std::back_inserter(meshList),
    //    [](const auto& pair) { return pair.second; });

    std::vector<VkDescriptorBufferInfo>	indexPropertiesBuffer;
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

const Vector<VkDescriptorBufferInfo> RenderSystem::GetGameObjectTransformBuffer()
{
    //Vector<MeshStruct> meshList;
    //meshList.reserve(meshSystem.SpriteMeshList.size());
    //std::transform(meshSystem.SpriteMeshList.begin(), meshSystem.SpriteMeshList.end(),
    //    std::back_inserter(meshList),
    //    [](const auto& pair) { return pair.second; });

    std::vector<VkDescriptorBufferInfo>	transformPropertiesBuffer;
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

const Vector<VkDescriptorBufferInfo> RenderSystem::GetMeshPropertiesBuffer(VkGuid& levelLayerId)
{
    Vector<Mesh> meshList;
    if (levelLayerId == VkGuid())
    {
        for (auto& sprite : meshSystem.SpriteMeshList())
        {
            meshList.emplace_back(sprite);

        }
    }
    else
    {
        for (auto& layer : meshSystem.FindLevelLayerMeshList(levelLayerId))
        {
            meshList.emplace_back(layer);
        }
    }

    Vector<VkDescriptorBufferInfo> meshPropertiesBuffer;
    if (meshList.empty())
    {
        meshPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
            {
                .buffer = VK_NULL_HANDLE,
                .offset = 0,
                .range = VK_WHOLE_SIZE
            });
    }
    else
    {
        for (auto& mesh : meshList)
        {
            const VulkanBufferStruct& meshProperties = bufferSystem.VulkanBuffer[mesh.PropertiesBufferId];
            meshPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
                {
                    .buffer = meshProperties.Buffer,
                    .offset = 0,
                    .range = VK_WHOLE_SIZE
                });
        }
    }

    return meshPropertiesBuffer;
}


const Vector<VkDescriptorImageInfo> RenderSystem::GetTexturePropertiesBuffer(VkGuid& renderPassId, const Texture* renderedTexture)
{
    Vector<Texture> textureList;
    if (renderedTexture != nullptr)
    {
        textureList.emplace_back(*renderedTexture);
    }
    else
    {
        for (auto& texture : textureSystem.TextureList())
        {
            textureList.emplace_back(texture);
        }
    }

    Vector<VkDescriptorImageInfo>	texturePropertiesBuffer;
    if (textureList.empty())
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
            textureSystem.GetTexturePropertiesBuffer(texture, texturePropertiesBuffer);
        }
    }

    return texturePropertiesBuffer;
}

const VulkanRenderPass& RenderSystem::FindRenderPass(const RenderPassGuid& guid)
{
    auto it = RenderPassMap.find(guid);
    if (it != RenderPassMap.end()) 
    {
        return it->second;
    }
    throw std::out_of_range("Render pass not found for given GUID");
}

const Vector<VulkanPipeline>& RenderSystem::FindRenderPipelineList(const RenderPassGuid& guid)
{
    auto it = RenderPipelineMap.find(guid);
    if (it != RenderPipelineMap.end())
    {
        return it->second;
    }
    throw std::out_of_range("Render Pipeline List not found for given GUID");
}

void RenderSystem::Destroy()
{
    ImGui_Destroy(cRenderer, imGuiRenderer);
    renderer.DestroyRenderer();
}