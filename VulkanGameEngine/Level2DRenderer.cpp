#include "Level2DRenderer.h"
#include "InputComponent.h"
#include "AssetManager.h"
#include "RenderSystem.h"
#include <VulkanRenderPass.h>

Level2DRenderer::Level2DRenderer() : JsonRenderPass2()
{
}

Level2DRenderer::Level2DRenderer(const String& jsonPath, ivec2 renderPassResolution)
{
    RenderPassId = 2;

    RenderPassResolution = renderPassResolution;
    SampleCount = VK_SAMPLE_COUNT_1_BIT;
    FrameBufferList.resize(cRenderer.SwapChain.SwapChainImageCount);

    VULKAN_RESULT(renderer.CreateCommandBuffer(CommandBuffer));

    nlohmann::json json = Json::ReadJson(jsonPath);
    RenderPassBuildInfoModel renderPassBuildInfo = RenderPassBuildInfoModel::from_json(json, renderPassResolution);
    BuildRenderPass(renderPassBuildInfo);
    BuildFrameBuffer(renderPassBuildInfo);
    renderSystem.ClearValueList[RenderPassId] = renderPassBuildInfo.ClearValueList;
    RenderPassInfo = VkRenderPassBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
        .renderPass = RenderPass,
        .framebuffer = FrameBufferList[cRenderer.ImageIndex],
        .renderArea = renderPassBuildInfo.RenderArea.RenderArea,
        .clearValueCount = static_cast<uint32>(renderSystem.ClearValueList[RenderPassId].size()),
        .pClearValues = renderSystem.ClearValueList[RenderPassId].data()
    };
}

Level2DRenderer::~Level2DRenderer()
{
}

void Level2DRenderer::StartLevelRenderer()
{
    auto textureId = renderSystem.LoadTexture("../Textures/TestTexture.json");
    auto materialId = renderSystem.LoadMaterial("../Materials/Material1.json");
    auto vramId = renderSystem.AddSpriteVRAM("../Sprites/TestSprite.json");

    assetManager.AnimationFrameList[0] = Vector<ivec2>
    {
        ivec2(0, 0),
        ivec2(1, 0)
    };

    assetManager.AnimationFrameList[1] = Vector<ivec2>
    {
        ivec2(3, 0),
        ivec2(4, 0),
        ivec2(5, 0),
        ivec2(4, 0)
    };

    assetManager.AnimationList[0] = Animation2D
    {
        .FrameHoldTime = 0.2f,
        .AnimationFrameId = 0
    };
    assetManager.AnimationList[1] = Animation2D
    {
        .FrameHoldTime = 0.2f,
        .AnimationFrameId = 1
    };

    assetManager.CreateGameObject(RenderPassId, "Obj1", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, vramId, vec2(300.0f, 40.0f));
    assetManager.CreateGameObject(RenderPassId, "Obj2", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, vramId, vec2(300.0f, 20.0f));
    for (int x = 0; x < 20000; x++)
    {
        assetManager.CreateGameObject(RenderPassId, "Obj3", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, vramId, vec2(300.0f, 80.0f));
    }
    JsonPipelineList.resize(1);
    renderSystem.SpriteBatchLayerList[RenderPassId].emplace_back(SpriteBatchLayer(RenderPassId));

    Vector<VkVertexInputBindingDescription> vertexBinding = NullVertex::GetBindingDescriptions();
    for (auto& instanceVar : SpriteInstanceVertex2D::GetBindingDescriptions())
    {
        vertexBinding.emplace_back(instanceVar);
    }

    Vector<VkVertexInputAttributeDescription> vertexAttribute = NullVertex::GetAttributeDescriptions();
    for (auto& instanceVar : SpriteInstanceVertex2D::GetAttributeDescriptions())
    {
        vertexAttribute.emplace_back(instanceVar);
    }

    renderSystem.RenderPipelineList[RenderPassId].emplace_back(JsonPipeline(2, "../Pipelines/Default2DPipeline.json", RenderPass, vertexBinding, vertexAttribute, sizeof(SceneDataBuffer), RenderPassResolution));
}

void Level2DRenderer::Update(const float& deltaTime)
{
   // DestroyDeadGameObjects();
    VkCommandBuffer commandBuffer = renderer.BeginSingleTimeCommands();
    for (auto& spriteLayer : renderSystem.SpriteBatchLayerList[RenderPassId])
    {
        spriteLayer.Update(commandBuffer, deltaTime);
    }
    renderer.EndSingleTimeCommands(commandBuffer);
}

void Level2DRenderer::UpdateBufferIndex()
{
    renderSystem.UpdateBufferIndex();
}

VkCommandBuffer Level2DRenderer::DrawSprites(Vector<SpriteBatchLayer> spriteLayerList, SceneDataBuffer& sceneDataBuffer)
{
    RenderPassInfo.clearValueCount = static_cast<uint32>(renderSystem.ClearValueList[RenderPassId].size());
    RenderPassInfo.pClearValues = renderSystem.ClearValueList[RenderPassId].data();
    RenderPassInfo.framebuffer = FrameBufferList[cRenderer.ImageIndex];

    VkCommandBufferBeginInfo CommandBufferBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
        .flags = VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
    };

    VULKAN_RESULT(vkResetCommandBuffer(CommandBuffer, 0));
    VULKAN_RESULT(vkBeginCommandBuffer(CommandBuffer, &CommandBufferBeginInfo));
    vkCmdBeginRenderPass(CommandBuffer, &RenderPassInfo, VK_SUBPASS_CONTENTS_INLINE);
    for (auto spriteLayer : spriteLayerList)
    {
        const Vector<SpriteInstanceStruct>& spriteInstanceList = renderSystem.SpriteInstanceList[spriteLayer.SpriteBatchLayerID];
        const SpriteInstanceBuffer& spriteInstanceBuffer = renderSystem.SpriteInstanceBufferList[spriteLayer.SpriteBatchLayerID];

        VkDeviceSize offsets[] = { 0 };
        vkCmdPushConstants(CommandBuffer, renderSystem.RenderPipelineList[RenderPassId][0].PipelineLayout, VK_SHADER_STAGE_VERTEX_BIT | VK_SHADER_STAGE_FRAGMENT_BIT, 0, sizeof(SceneDataBuffer), &sceneDataBuffer);
        vkCmdBindPipeline(CommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, renderSystem.RenderPipelineList[RenderPassId][0].Pipeline);
        vkCmdBindDescriptorSets(CommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, renderSystem.RenderPipelineList[RenderPassId][0].PipelineLayout, 0, renderSystem.RenderPipelineList[RenderPassId][0].DescriptorSetList.size(), renderSystem.RenderPipelineList[RenderPassId][0].DescriptorSetList.data(), 0, nullptr);
        vkCmdBindVertexBuffers(CommandBuffer, 0, 1, assetManager.MeshList[spriteLayer.SpriteLayerMeshId].GetVertexBuffer().get(), offsets);
        vkCmdBindVertexBuffers(CommandBuffer, 1, 1, &spriteInstanceBuffer.Buffer, offsets);
        vkCmdBindIndexBuffer(CommandBuffer, *assetManager.MeshList[spriteLayer.SpriteLayerMeshId].GetIndexBuffer().get(), 0, VK_INDEX_TYPE_UINT32);
        vkCmdDrawIndexed(CommandBuffer, renderSystem.SpriteIndexList.size(), spriteInstanceList.size(), 0, 0, 0);
    }
    vkCmdEndRenderPass(CommandBuffer);
    vkEndCommandBuffer(CommandBuffer);
    return CommandBuffer;
}

void Level2DRenderer::Destroy()
{

    for (auto& spriteLayer : renderSystem.SpriteBatchLayerList[RenderPassId])
    {
        spriteLayer.Destroy();
    }
    //for (auto& texture : TextureList)
    //{
    //    texture.Destroy();
    //}
  /*  for (auto& material : MaterialList)
    {
        material->Destroy();
    }*/
  //  JsonRenderPass::Destroy();
}

Vector<SpriteMesh> Level2DRenderer::GetMeshFromGameObjects()
{
    Vector<SpriteMesh> meshList;
    for (auto& spriteLayer : renderSystem.SpriteBatchLayerList[RenderPassId])
    {
        meshList.emplace_back(assetManager.MeshList[spriteLayer.SpriteLayerMeshId]);
    }

    return meshList;
}
