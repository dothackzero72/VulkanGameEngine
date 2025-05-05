#include "renderSystem.h"

RenderSystem renderSystem = RenderSystem();

RenderSystem::RenderSystem()
{

}

RenderSystem::~RenderSystem()
{

}

void RenderSystem::StartUp()
{
    renderer.RendererSetUp();
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
        int width = cRenderer.SwapChain.SwapChainResolution.width;
        int height = cRenderer.SwapChain.SwapChainResolution.height;
        RecreateSwapchain();
        cRenderer.RebuildRendererFlag = false;
    }

    VkCommandBuffer commandBuffer = renderer.BeginSingleTimeCommands();

    RenderPassID id = RenderPassID(1);
    for (auto& spriteLayer : SpriteBatchLayerList[id])
    {
        spriteLayer.Update(commandBuffer, deltaTime);
    }
    renderer.EndSingleTimeCommands(commandBuffer);
}

void RenderSystem::RecreateSwapchain()
{
    int width = 0;
    int height = 0;

    vkDeviceWaitIdle(*Device.get());

    vulkanWindow->GetFrameBufferSize(vulkanWindow, &width, &height);
    renderer.DestroySwapChainImageView();
    renderer.DestroySwapChain();
    renderer.SetUpSwapChain();

    RenderPassID id;
    id.id = 2;

    RenderPassList[id].RecreateSwapchain(width, height);
}

VkCommandBuffer RenderSystem::RenderFrameBuffer(RenderPassID renderPassId)
{
    JsonRenderPass renderPass = RenderPassList[renderPassId];
    const JsonPipeline& pipeline = RenderPipelineList[renderPassId][0];
    const VkCommandBuffer& commandBuffer = renderPass.CommandBuffer;

    VkRenderPassBeginInfo renderPassBeginInfo = VkRenderPassBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
        .renderPass = renderPass.RenderPass,
        .framebuffer = renderPass.FrameBufferList[cRenderer.ImageIndex],
        .renderArea = renderPass.renderArea,
        .clearValueCount = static_cast<uint32>(ClearValueList[renderPassId].size()),
        .pClearValues = ClearValueList[renderPassId].data()
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

VkCommandBuffer RenderSystem::RenderSprites(RenderPassID renderPassId, const float deltaTime, SceneDataBuffer& sceneDataBuffer)
{
    const JsonRenderPass& renderPass = RenderPassList[renderPassId];
    const JsonPipeline& pipeline = RenderPipelineList[renderPassId][0];
    const Vector<SpriteBatchLayer>& spriteLayerList = SpriteBatchLayerList[renderPassId];
    const VkCommandBuffer& commandBuffer = renderPass.CommandBuffer;

    VkRenderPassBeginInfo renderPassBeginInfo = VkRenderPassBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
        .renderPass = renderPass.RenderPass,
        .framebuffer = renderPass.FrameBufferList[cRenderer.ImageIndex],
        .renderArea = renderPass.renderArea,
        .clearValueCount = static_cast<uint32>(ClearValueList[renderPassId].size()),
        .pClearValues = ClearValueList[renderPassId].data()
    };

    VULKAN_RESULT(vkResetCommandBuffer(commandBuffer, 0));
    VULKAN_RESULT(vkBeginCommandBuffer(commandBuffer, &CommandBufferBeginInfo));
    vkCmdBeginRenderPass(commandBuffer, &renderPassBeginInfo, VK_SUBPASS_CONTENTS_INLINE);
    for (auto spriteLayer : spriteLayerList)
    {
        const Vector<SpriteInstanceStruct>& spriteInstanceList = SpriteInstanceList[spriteLayer.SpriteBatchLayerID];
        const SpriteInstanceBuffer& spriteInstanceBuffer = SpriteInstanceBufferList[spriteLayer.SpriteBatchLayerID];

        VkDeviceSize offsets[] = { 0 };
        vkCmdPushConstants(commandBuffer, pipeline.PipelineLayout, VK_SHADER_STAGE_VERTEX_BIT | VK_SHADER_STAGE_FRAGMENT_BIT, 0, sizeof(SceneDataBuffer), &sceneDataBuffer);
        vkCmdBindPipeline(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline.Pipeline);
        vkCmdBindDescriptorSets(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline.PipelineLayout, 0, pipeline.DescriptorSetList.size(), pipeline.DescriptorSetList.data(), 0, nullptr);
        vkCmdBindVertexBuffers(commandBuffer, 0, 1, renderSystem.SpriteMeshList[spriteLayer.SpriteLayerMeshId].GetVertexBuffer().get(), offsets);
        vkCmdBindVertexBuffers(commandBuffer, 1, 1, &spriteInstanceBuffer.Buffer, offsets);
        vkCmdBindIndexBuffer(commandBuffer, *renderSystem.SpriteMeshList[spriteLayer.SpriteLayerMeshId].GetIndexBuffer().get(), 0, VK_INDEX_TYPE_UINT32);
        vkCmdDrawIndexed(commandBuffer, SpriteIndexList.size(), spriteInstanceList.size(), 0, 0, 0);
    }
    vkCmdEndRenderPass(commandBuffer);
    vkEndCommandBuffer(commandBuffer);
    return commandBuffer;
}

RenderPassID RenderSystem::AddRenderPass(const String& jsonPath, ivec2 renderPassResolution)
{
    RenderPassID id;
    id.id = RenderPassList.size() + 1;
    RenderPassList[id] = JsonRenderPass(id, jsonPath, renderPassResolution);
    return id;
}

RenderPassID RenderSystem::AddRenderPass(const String& jsonPath, Texture& inputTexture, ivec2 renderPassResolution)
{
    RenderPassID id;
    id.id = RenderPassList.size() + 1;
    RenderPassList[id] = JsonRenderPass(id, jsonPath, inputTexture, renderPassResolution);
    return id;
}

const Vector<VkDescriptorBufferInfo> RenderSystem::GetVertexPropertiesBuffer()
{
    Vector<SpriteMesh> meshList;
    meshList.reserve(renderSystem.SpriteMeshList.size());
    std::transform(renderSystem.SpriteMeshList.begin(), renderSystem.SpriteMeshList.end(),
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
    meshList.reserve(renderSystem.SpriteMeshList.size());
    std::transform(renderSystem.SpriteMeshList.begin(), renderSystem.SpriteMeshList.end(),
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
    meshList.reserve(renderSystem.SpriteMeshList.size());
    std::transform(renderSystem.SpriteMeshList.begin(), renderSystem.SpriteMeshList.end(),
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

const Vector<VkDescriptorBufferInfo> RenderSystem::GetMeshPropertiesBuffer()
{
    Vector<SpriteMesh> meshList;
    meshList.reserve(renderSystem.SpriteMeshList.size());
    std::transform(renderSystem.SpriteMeshList.begin(), renderSystem.SpriteMeshList.end(),
        std::back_inserter(meshList),
        [](const auto& pair) { return pair.second; });

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

const Vector<VkDescriptorImageInfo> RenderSystem::GetTexturePropertiesBuffer(Vector<SharedPtr<Texture>>& renderedTextureList)
{
    Vector<Texture> textureList;
    if (renderedTextureList.empty())
    {
        textureList.reserve(TextureList.size());
        std::transform(TextureList.begin(), TextureList.end(),
            std::back_inserter(textureList),
            [](const auto& pair) { return pair.second; });
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
            texture.GetTexturePropertiesBuffer(texturePropertiesBuffer);
        }
    }

    return texturePropertiesBuffer;
}

const Vector<VkDescriptorBufferInfo> RenderSystem::GetMaterialPropertiesBuffer()
{
    Vector<Material> materialList;
    materialList.reserve(MaterialList.size());
    std::transform(MaterialList.begin(), MaterialList.end(),
        std::back_inserter(materialList),
        [](const auto& pair) { return pair.second; });

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
    for (auto& [id, texture] : renderSystem.TextureList) {
        texture.UpdateTextureBufferIndex(xy);
        ++xy;
    }
    int xz = 0;
    for (auto& [id, material] : renderSystem.MaterialList) {
        material.UpdateMaterialBufferIndex(xz);
        ++xz;
    }
}

VkGuid RenderSystem::AddSpriteVRAM(const String& spritePath)
{
    nlohmann::json json = Json::ReadJson(spritePath);
    VkGuid vramId = VkGuid(json["VramSpriteId"].get<String>().c_str());
    VkGuid materialId = VkGuid(json["MaterialId"].get<String>().c_str());

    const Material& material = MaterialList.at(materialId);
    const Texture& texture = TextureList.at(material.AlbedoMapId);
    SpriteVram sprite = SpriteVram
    {
        .VramSpriteID = vramId,
        .SpriteMaterialID = materialId,
        .SpriteLayer = json["SpriteLayer"],
        .SpriteColor = vec4{ json["SpriteColor"][0], json["SpriteColor"][1], json["SpriteColor"][2], json["SpriteColor"][3] },
        .SpritePixelSize = ivec2{ json["SpritePixelSize"][0], json["SpritePixelSize"][1] },
        .SpriteScale = vec2(5.0f),
        .SpriteCells = ivec2(texture.Width / sprite.SpritePixelSize.x, texture.Height / sprite.SpritePixelSize.y),
        .SpriteUVSize = vec2(1.0f / (float)sprite.SpriteCells.x, 1.0f / (float)sprite.SpriteCells.y),
        .SpriteSize = vec2(sprite.SpritePixelSize.x * sprite.SpriteScale.x, sprite.SpritePixelSize.y * sprite.SpriteScale.y),
    };

    VramSpriteList[vramId] = sprite;
    return vramId;
}

VkGuid RenderSystem::LoadTexture(const String& texturePath)
{
    if (texturePath.empty() ||
        texturePath == "")
    {
        return VkGuid();
    }

    nlohmann::json json = Json::ReadJson(texturePath);
    VkGuid textureId = VkGuid(json["TextureId"].get<String>().c_str());
    String textureFilePath = json["TextureFilePath"];
    VkFormat textureByteFormat = json["TextureByteFormat"];
    VkImageAspectFlags imageType = json["ImageType"];
    TextureTypeEnum textureType = json["TextureType"];
    bool useMipMaps = json["UseMipMaps"];

    TextureList[textureId] = Texture(textureId, textureFilePath, textureByteFormat, imageType, textureType, useMipMaps);
    return textureId;
}

VkGuid RenderSystem::LoadMaterial(const String& materialPath)
{
    nlohmann::json json = Json::ReadJson(materialPath);


    String name = json["Name"];
    VkGuid materialId = VkGuid(json["MaterialId"].get<String>().c_str());

    MaterialList[materialId] = Material(name, materialId);
    MaterialList[materialId].Albedo = vec3(json["Albedo"][0], json["Albedo"][1], json["Albedo"][2]);
    MaterialList[materialId].Metallic = json["Metallic"];
    MaterialList[materialId].Roughness = json["Roughness"];
    MaterialList[materialId].AmbientOcclusion = json["AmbientOcclusion"];
    MaterialList[materialId].Emission = vec3(json["Emission"][0], json["Emission"][1], json["Emission"][2]);
    MaterialList[materialId].Alpha = json["Alpha"];

    MaterialList[materialId].AlbedoMapId = LoadTexture(json["AlbedoMapPath"]);
    MaterialList[materialId].MetallicRoughnessMapId = LoadTexture(json["MetallicRoughnessMapPath"]);
    MaterialList[materialId].MetallicMapId = LoadTexture(json["MetallicMapPath"]);
    MaterialList[materialId].RoughnessMapId = LoadTexture(json["RoughnessMapPath"]);
    MaterialList[materialId].AmbientOcclusionMapId = LoadTexture(json["AmbientOcclusionMapPath"]);
    MaterialList[materialId].NormalMapId = LoadTexture(json["NormalMapPath"]);
    MaterialList[materialId].DepthMapId = LoadTexture(json["DepthMapPath"]);
    MaterialList[materialId].AlphaMapId = LoadTexture(json["AlphaMapPath"]);
    MaterialList[materialId].EmissionMapId = LoadTexture(json["EmissionMapPath"]);
    MaterialList[materialId].HeightMapId = LoadTexture(json["HeightMapPath"]);

    return materialId;
}

void RenderSystem::Destroy()
{
    InterfaceRenderPass::Destroy();
    renderer.DestroyRenderer();
}
