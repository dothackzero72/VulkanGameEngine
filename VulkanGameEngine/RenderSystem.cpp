#include "renderSystem.h"
#include "json.h"
#include "TextureSystem.h"
#include "ShaderSystem.h"
#include "AssetManager.h"

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

   // InterfaceRenderPass::StartUp();

    //ImageIndex = std::make_shared<uint>(cRenderer.ImageIndex);
    //CommandIndex = std::make_shared<uint>(cRenderer.CommandIndex);
    //SwapChainImageCount = std::make_shared<uint>(cRenderer.SwapChain.SwapChainImageCount);
    //GraphicsFamily = std::make_shared<uint>(cRenderer.SwapChain.GraphicsFamily);
    //PresentFamily = std::make_shared<uint>(cRenderer.SwapChain.PresentFamily);

    //Instance = std::make_shared<VkInstance>(cRenderer.Instance);
    Device = std::make_shared<VkDevice>(cRenderer.Device);
    //PhysicalDevice = std::make_shared<VkPhysicalDevice>(cRenderer.PhysicalDevice);
    //Surface = std::make_shared<VkSurfaceKHR>(cRenderer.Surface);
    //CommandPool = std::make_shared<VkCommandPool>(cRenderer.CommandPool);
    //DebugMessenger = std::make_shared<VkDebugUtilsMessengerEXT>(cRenderer.DebugMessenger);
    //GraphicsQueue = std::make_shared<VkQueue>(cRenderer.SwapChain.GraphicsQueue);
    //PresentQueue = std::make_shared<VkQueue>(cRenderer.SwapChain.PresentQueue);
    /*for (int x = 0; x < *SwapChainImageCount.get(); x++)
    {
        InFlightFences.emplace_back(std::make_shared<VkFence>(cRenderer.InFlightFences));
        AcquireImageSemaphores.emplace_back(std::make_shared<uint>(cRenderer.AcquireImageSemaphores));
        PresentImageSemaphores.emplace_back(std::make_shared<VkSemaphore>(cRenderer.PresentImageSemaphores));
    }*/
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

    UpdateBufferIndex();
    VkCommandBuffer commandBuffer = renderer.BeginSingleTimeCommands();
    for (auto& renderPass : RenderPassList)
    {
        if (assetManager.SpriteBatchLayerList.find(renderPass.second.RenderPassId) != assetManager.SpriteBatchLayerList.end())
        {
            for (auto& spriteLayer : assetManager.SpriteBatchLayerList[renderPass.second.RenderPassId])
            {
                spriteLayer.Update(commandBuffer, deltaTime);
            }
        }
    }
    renderer.EndSingleTimeCommands(commandBuffer);
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
    VulkanRenderPass renderPass = RenderPassList[renderPassId];
    const VulkanPipeline& pipeline = RenderPipelineList[renderPassId][0];
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
    const VulkanRenderPass& renderPass = RenderPassList[renderPassId];
    const VulkanPipeline& spritePipeline = RenderPipelineList[renderPassId][0];
    const VulkanPipeline& levelPipeline = RenderPipelineList[renderPassId][1];
    const Vector<SpriteBatchLayer>& spriteLayerList = assetManager.SpriteBatchLayerList[renderPassId];
    const Vector<LevelLayerMesh>& levelLayerList = assetManager.LevelLayerMeshList[levelId];
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
        VkDeviceSize offsets[] = { 0 };
        vkCmdPushConstants(commandBuffer, levelPipeline.PipelineLayout, VK_SHADER_STAGE_VERTEX_BIT | VK_SHADER_STAGE_FRAGMENT_BIT, 0, sizeof(SceneDataBuffer), &sceneDataBuffer);
        vkCmdBindPipeline(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, levelPipeline.Pipeline);
        vkCmdBindDescriptorSets(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, levelPipeline.PipelineLayout, 0, levelPipeline.DescriptorSetList.size(), levelPipeline.DescriptorSetList.data(), 0, nullptr);
        vkCmdBindVertexBuffers(commandBuffer, 0, 1, &levelLayer.MeshVertexBuffer.Buffer, offsets);
        vkCmdBindIndexBuffer(commandBuffer, levelLayer.MeshIndexBuffer.Buffer, 0, VK_INDEX_TYPE_UINT32);
        vkCmdDrawIndexed(commandBuffer, levelLayer.IndexCount, 1, 0, 0, 0);
    }
    for (auto spriteLayer : spriteLayerList)
    {
        const Vector<SpriteInstanceStruct>& spriteInstanceList = assetManager.SpriteInstanceList[spriteLayer.SpriteBatchLayerID];
        const SpriteInstanceBuffer& spriteInstanceBuffer = assetManager.SpriteInstanceBufferList[spriteLayer.SpriteBatchLayerID];
        const SpriteMesh& spriteMesh = assetManager.SpriteMeshList[spriteLayer.SpriteLayerMeshId];

        VkDeviceSize offsets[] = { 0 };
        vkCmdPushConstants(commandBuffer, spritePipeline.PipelineLayout, VK_SHADER_STAGE_VERTEX_BIT | VK_SHADER_STAGE_FRAGMENT_BIT, 0, sizeof(SceneDataBuffer), &sceneDataBuffer);
        vkCmdBindPipeline(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, spritePipeline.Pipeline);
        vkCmdBindDescriptorSets(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, spritePipeline.PipelineLayout, 0, spritePipeline.DescriptorSetList.size(), spritePipeline.DescriptorSetList.data(), 0, nullptr);
        vkCmdBindVertexBuffers(commandBuffer, 0, 1, &spriteMesh.MeshVertexBuffer.Buffer, offsets);
        vkCmdBindVertexBuffers(commandBuffer, 1, 1, &spriteInstanceBuffer.Buffer, offsets);
        vkCmdBindIndexBuffer(commandBuffer, spriteMesh.MeshIndexBuffer.Buffer, 0, VK_INDEX_TYPE_UINT32);
        vkCmdDrawIndexed(commandBuffer, assetManager.SpriteIndexList.size(), spriteInstanceList.size(), 0, 0, 0);
    }
    vkCmdEndRenderPass(commandBuffer);
    vkEndCommandBuffer(commandBuffer);
    return commandBuffer;
}

VkGuid RenderSystem::AddRenderPass(VkGuid& levelId, const String& jsonPath, ivec2 renderPassResolution)
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

    RenderPassList[model.RenderPassId] = RenderPass_CreateVulkanRenderPass(cRenderer, model, renderPassResolution, sizeof(SceneDataBuffer), textureSystem.RenderedTextureList[model.RenderPassId], textureSystem.DepthTextureList[model.RenderPassId]);
    assetManager.SpriteBatchLayerList[model.RenderPassId].emplace_back(SpriteBatchLayer(model.RenderPassId));

    for (int x = 0; x < model.RenderPipelineList.size(); x++)
    {
        uint pipeLineId = renderSystem.RenderPipelineList.size();
        nlohmann::json json = Json::ReadJson(model.RenderPipelineList[x]);
        RenderPipelineModel renderPipelineModel = RenderPipelineModel::from_json(json);
        GPUIncludes include =
        {
            .vertexProperties = renderSystem.GetVertexPropertiesBuffer(),
            .indexProperties = renderSystem.GetIndexPropertiesBuffer(),
            //        .transformProperties = renderSystem.GetTransformPropertiesBuffer(gpuImport.MeshList),
            .meshProperties = renderSystem.GetMeshPropertiesBuffer(levelId),
            .texturePropertiesList = renderSystem.GetTexturePropertiesBuffer(model.RenderPassId, textureSystem.InputTextureList[pipeLineId]),
            .materialProperties = renderSystem.GetMaterialPropertiesBuffer()
        };

        Vector<VkPipelineShaderStageCreateInfo> pipelineShaderStageCreateInfoList = Vector<VkPipelineShaderStageCreateInfo>
        {
            shaderSystem.CreateShader(cRenderer.Device, renderPipelineModel.VertexShaderPath, VK_SHADER_STAGE_VERTEX_BIT),
            shaderSystem.CreateShader(cRenderer.Device, renderPipelineModel.FragmentShaderPath, VK_SHADER_STAGE_FRAGMENT_BIT)
        };

        renderSystem.RenderPipelineList[model.RenderPassId].emplace_back(Pipeline_CreateRenderPipeline(cRenderer.Device, model.RenderPassId, pipeLineId, renderPipelineModel, RenderPassList[model.RenderPassId].RenderPass, sizeof(SceneDataBuffer), renderPassResolution, include, pipelineShaderStageCreateInfoList));
    }

    return model.RenderPassId;
}

VkGuid RenderSystem::AddRenderPass(VkGuid& levelId, const String& jsonPath, Texture& inputTexture, ivec2 renderPassResolution)
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

    RenderPassList[model.RenderPassId] = RenderPass_CreateVulkanRenderPass(cRenderer, model, renderPassResolution, sizeof(SceneDataBuffer), textureSystem.RenderedTextureList[model.RenderPassId], textureSystem.DepthTextureList[model.RenderPassId]);
    assetManager.SpriteBatchLayerList[model.RenderPassId].emplace_back(SpriteBatchLayer(model.RenderPassId));

    for (int x = 0; x < model.RenderPipelineList.size(); x++)
    {
        uint pipeLineId = renderSystem.RenderPipelineList.size();
        nlohmann::json json = Json::ReadJson(model.RenderPipelineList[x]);
        RenderPipelineModel renderPipelineModel = RenderPipelineModel::from_json(json);

        textureSystem.InputTextureList[pipeLineId].emplace_back(std::make_shared<Texture>(inputTexture));
        GPUIncludes include =
        {
            .vertexProperties = renderSystem.GetVertexPropertiesBuffer(),
            .indexProperties = renderSystem.GetIndexPropertiesBuffer(),
            //        .transformProperties = renderSystem.GetTransformPropertiesBuffer(gpuImport.MeshList),
            .meshProperties = renderSystem.GetMeshPropertiesBuffer(levelId),
            .texturePropertiesList = renderSystem.GetTexturePropertiesBuffer(model.RenderPassId, textureSystem.InputTextureList[pipeLineId]),
            .materialProperties = renderSystem.GetMaterialPropertiesBuffer()
        };

        Vector<VkPipelineShaderStageCreateInfo> pipelineShaderStageCreateInfoList = Vector<VkPipelineShaderStageCreateInfo>
        {
            shaderSystem.CreateShader(cRenderer.Device, renderPipelineModel.VertexShaderPath, VK_SHADER_STAGE_VERTEX_BIT),
            shaderSystem.CreateShader(cRenderer.Device, renderPipelineModel.FragmentShaderPath, VK_SHADER_STAGE_FRAGMENT_BIT)
        };
        renderSystem.RenderPipelineList[model.RenderPassId].emplace_back(Pipeline_CreateRenderPipeline(cRenderer.Device, model.RenderPassId, pipeLineId, renderPipelineModel, RenderPassList[model.RenderPassId].RenderPass, sizeof(SceneDataBuffer), renderPassResolution, include, pipelineShaderStageCreateInfoList));
    }

    return model.RenderPassId;
}

const Vector<VkDescriptorBufferInfo> RenderSystem::GetVertexPropertiesBuffer()
{
    Vector<SpriteMesh> meshList;
    meshList.reserve(assetManager.SpriteMeshList.size());
    std::transform(assetManager.SpriteMeshList.begin(), assetManager.SpriteMeshList.end(),
        std::back_inserter(meshList),
        [](const auto& pair) { return pair.second; });


    Vector<VkDescriptorBufferInfo> vertexPropertiesBuffer;
    if (meshList.empty())
    {
        vertexPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
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
            // mesh->GetVertexBuffer(vertexPropertiesBuffer);
        }
    }

    return vertexPropertiesBuffer;
}

const Vector<VkDescriptorBufferInfo> RenderSystem::GetIndexPropertiesBuffer()
{
    Vector<SpriteMesh> meshList;
    meshList.reserve(assetManager.SpriteMeshList.size());
    std::transform(assetManager.SpriteMeshList.begin(), assetManager.SpriteMeshList.end(),
        std::back_inserter(meshList),
        [](const auto& pair) { return pair.second; });

    std::vector<VkDescriptorBufferInfo>	indexPropertiesBuffer;
    if (meshList.empty())
    {
        indexPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
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
            //   mesh->GetIndexBuffer(indexPropertiesBuffer);
        }
    }
    return indexPropertiesBuffer;
}

const Vector<VkDescriptorBufferInfo> RenderSystem::GetGameObjectTransformBuffer()
{
    Vector<SpriteMesh> meshList;
    meshList.reserve(assetManager.SpriteMeshList.size());
    std::transform(assetManager.SpriteMeshList.begin(), assetManager.SpriteMeshList.end(),
        std::back_inserter(meshList),
        [](const auto& pair) { return pair.second; });

    std::vector<VkDescriptorBufferInfo>	transformPropertiesBuffer;
    if (meshList.empty())
    {
        transformPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
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
            mesh.GetTransformBuffer();
        }
    }

    return transformPropertiesBuffer;
}

const Vector<VkDescriptorBufferInfo> RenderSystem::GetMeshPropertiesBuffer(VkGuid& levelLayerId)
{
    Vector<SpriteMesh> meshList;
    if (levelLayerId == VkGuid())
    {
        for (auto& sprite : assetManager.SpriteMeshList)
        {
            meshList.emplace_back(sprite.second);

        }
    }
    else
    {
        for (auto& layer : assetManager.LevelLayerMeshList[levelLayerId])
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
            meshPropertiesBuffer.emplace_back(mesh.GetMeshPropertiesBuffer());
        }
    }

    return meshPropertiesBuffer;
}


const Vector<VkDescriptorImageInfo> RenderSystem::GetTexturePropertiesBuffer(VkGuid& renderPassId, Vector<SharedPtr<Texture>>& renderedTextureList)
{
    Vector<Texture> textureList;
    if (renderedTextureList.empty())
    {
        for (auto& texture : textureSystem.TextureList)
        {
            textureList.emplace_back(texture.second);
        }
    }
    else
    {
        for (auto& texture : renderedTextureList)
        {
            textureList.emplace_back(*texture.get());
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

const Vector<VkDescriptorBufferInfo> RenderSystem::GetMaterialPropertiesBuffer()
{
    Vector<Material> materialList;
    for (auto& material : assetManager.MaterialList)
    {
        materialList.emplace_back(material.second);
    }

    std::vector<VkDescriptorBufferInfo>	materialPropertiesBuffer;
    if (materialList.empty())
    {
        materialPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
            {
                .buffer = VK_NULL_HANDLE,
                .offset = 0,
                .range = VK_WHOLE_SIZE
            });
    }
    else
    {
        for (auto& material : materialList)
        {
            material.GetMaterialPropertiesBuffer(materialPropertiesBuffer);
        }
    }
    return materialPropertiesBuffer;
}

void RenderSystem::UpdateBufferIndex()
{
    int xy = 0;
    for (auto& [id, texture] : textureSystem.TextureList) {
        textureSystem.UpdateTextureBufferIndex(texture, xy);
        ++xy;
    }
    int xz = 0;
    for (auto& [id, material] : assetManager.MaterialList) {
        material.UpdateMaterialBufferIndex(xz);
        material.UpdateBuffer();
        ++xz;
    }
}

void RenderSystem::Destroy()
{
    ImGui_Destroy(cRenderer, imGuiRenderer);
    renderer.DestroyRenderer();
}
