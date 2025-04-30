#include "SpriteBatchLayer.h"
#include "TypeDef.h"
#include "JsonRenderPass.h"
#include "AssetManager.h"
#include "FrameBufferRenderPass.h"

typedef uint UM_SpriteID;
typedef uint UM_SpriteBatchID;
typedef uint UM_RenderPassID;

class Sprite;
class JsonPipeline;
class JsonRenderPass;
class RenderSystem
{
    friend class JsonPipeline;
private:


    VkResult CreateCommandBuffer();

    const Vector<VkDescriptorBufferInfo> GetVertexPropertiesBuffer();
    const Vector<VkDescriptorBufferInfo> GetIndexPropertiesBuffer();
    const Vector<VkDescriptorBufferInfo> GetGameObjectTransformBuffer();
    const Vector<VkDescriptorBufferInfo> GetMeshPropertiesBuffer();
    const Vector<VkDescriptorImageInfo>  GetTexturePropertiesBuffer(Vector<SharedPtr<Texture>>& renderedTextureList);
    const Vector<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer();

public:
    Vector<Vertex2D> SpriteVertexList =
    {
        Vertex2D(vec2(0.0f, 1.0f), vec2(0.0f, 0.0f)),
        Vertex2D(vec2(1.0f, 0.0f), vec2(1.0f, 0.0f)),
        Vertex2D(vec2(1.0f, 0.0f), vec2(1.0f, 1.0f)),
        Vertex2D(vec2(0.0f, 0.0f), vec2(0.0f, 1.0f)),
    };

    Vector<uint32> SpriteIndexList =
    {
      0, 3, 1,
      1, 3, 2
    };

    VkCommandBuffer CommandBuffer = VK_NULL_HANDLE;
    UM_PipelineID CurrentGraphicsPipelineID = 0;
    UM_RenderPassID CurrentRenderPassID = 0;

    UnorderedMap<VkGuid, Texture> TextureList;
    UnorderedMap<VkGuid, Material> MaterialList;
    UnorderedMap<VkGuid, SpriteVram> VramSpriteList;

    UnorderedMap<UM_RenderPassID, JsonRenderPass> RenderPassList;
    UnorderedMap<UM_RenderPassID, DepthTexture> DepthTextureList;
    UnorderedMap<UM_RenderPassID, VkRenderPassBeginInfo> RenderPassInfoList;
    UnorderedMap<UM_RenderPassID, Vector<JsonPipeline>> RenderPipelineList;
    UnorderedMap<UM_RenderPassID, Vector<SharedPtr<Texture>>> InputTextureList;
    UnorderedMap<UM_RenderPassID, Vector<RenderedTexture>> RenderedTextureList;
    UnorderedMap<UM_RenderPassID, Vector<SpriteBatchLayer>> SpriteBatchLayerList;
    UnorderedMap<UM_RenderPassID, Vector<VkVertexInputBindingDescription>> VertexInputBindingList;
    UnorderedMap<UM_RenderPassID, Vector<VkVertexInputAttributeDescription>> VertexInputAttributeDescription;
    UnorderedMap<UM_RenderPassID, Vector<VkClearValue>> ClearValueList;

    UnorderedMap<UM_SpriteBatchID, SpriteInstanceBuffer> SpriteInstanceBufferList;
    UnorderedMap<UM_SpriteBatchID, Vector<UM_GameObjectID>> SpriteBatchLayerObjectList;
    UnorderedMap<UM_SpriteBatchID, Vector<SpriteInstanceStruct>> SpriteInstanceList;

    VkCommandBufferBeginInfo CommandBufferBeginInfo;

    RenderSystem()
    {
    }

    RenderSystem(Vector<String>& renderPassJsonList, Texture& texture, SceneDataBuffer sceneDataBuffer)
    {
        //GPUImport gpuImport;
        //gpuImport.TextureList.emplace_back(texture);

        //for (int x = 0; x < renderPassJsonList.size(); x++)
        //{
        //    RenderPassList[x] = JsonRenderPass(renderPassJsonList[x], gpuImport, ivec2(cRenderer.SwapChain.SwapChainResolution.width, cRenderer.SwapChain.SwapChainResolution.height), sceneDataBuffer);
        //  //  GraphicsPipelineList[x] = RenderPassList[x].JsonPipelineList[0];
        //}
    }

    ~RenderSystem()
    {

    }

    void RenderSystemStartUp()
    {
        CreateCommandBuffer();
    }

    void Update(const float& deltaTime)
    {
        // DestroyDeadGameObjects();
        VkCommandBuffer commandBuffer = renderer.BeginSingleTimeCommands();
        for (auto& spriteLayer : SpriteBatchLayerList[1])
        {
            spriteLayer.Update(commandBuffer, deltaTime);
        }
        renderer.EndSingleTimeCommands(commandBuffer);
    }

    void UpdateBufferIndex();

    VkCommandBuffer RenderFrameBuffer(uint renderPassId)
    {
         JsonRenderPass renderPass = RenderPassList[renderPassId];
        const JsonPipeline& pipeline = RenderPipelineList[renderPassId][0];
        const VkCommandBuffer commandBuffer = CommandBuffer;

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
        return CommandBuffer;
    }

    VkCommandBuffer RenderSprites(uint rendererId, const float deltaTime, SceneDataBuffer& sceneDataBuffer)
    {
        const JsonRenderPass& renderPass = RenderPassList[rendererId];
        const JsonPipeline& pipeline = RenderPipelineList[rendererId][0];
        const Vector<SpriteBatchLayer>& spriteLayerList = SpriteBatchLayerList[rendererId];
        const VkCommandBuffer commandBuffer = CommandBuffer;

        VkRenderPassBeginInfo renderPassBeginInfo = VkRenderPassBeginInfo
        {
            .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
            .renderPass = renderPass.RenderPass,
            .framebuffer = renderPass.FrameBufferList[cRenderer.ImageIndex],
            .renderArea = renderPass.renderArea,
            .clearValueCount = static_cast<uint32>(ClearValueList[rendererId].size()),
            .pClearValues = ClearValueList[rendererId].data()
        };

        VULKAN_RESULT(vkResetCommandBuffer(CommandBuffer, 0));
        VULKAN_RESULT(vkBeginCommandBuffer(CommandBuffer, &CommandBufferBeginInfo));
        vkCmdBeginRenderPass(CommandBuffer, &renderPassBeginInfo, VK_SUBPASS_CONTENTS_INLINE);
        for (auto spriteLayer : spriteLayerList)
        {
            const Vector<SpriteInstanceStruct>& spriteInstanceList = SpriteInstanceList[spriteLayer.SpriteBatchLayerID];
            const SpriteInstanceBuffer& spriteInstanceBuffer =SpriteInstanceBufferList[spriteLayer.SpriteBatchLayerID];

            VkDeviceSize offsets[] = { 0 };
            vkCmdPushConstants(CommandBuffer, pipeline.PipelineLayout, VK_SHADER_STAGE_VERTEX_BIT | VK_SHADER_STAGE_FRAGMENT_BIT, 0, sizeof(SceneDataBuffer), &sceneDataBuffer);
            vkCmdBindPipeline(CommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline.Pipeline);
            vkCmdBindDescriptorSets(CommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline.PipelineLayout, 0, pipeline.DescriptorSetList.size(), pipeline.DescriptorSetList.data(), 0, nullptr);
            vkCmdBindVertexBuffers(CommandBuffer, 0, 1, assetManager.MeshList[spriteLayer.SpriteLayerMeshId].GetVertexBuffer().get(), offsets);
            vkCmdBindVertexBuffers(CommandBuffer, 1, 1, &spriteInstanceBuffer.Buffer, offsets);
            vkCmdBindIndexBuffer(CommandBuffer, *assetManager.MeshList[spriteLayer.SpriteLayerMeshId].GetIndexBuffer().get(), 0, VK_INDEX_TYPE_UINT32);
            vkCmdDrawIndexed(CommandBuffer, SpriteIndexList.size(), spriteInstanceList.size(), 0, 0, 0);
        }
        vkCmdEndRenderPass(CommandBuffer);
        vkEndCommandBuffer(CommandBuffer);
        return CommandBuffer;
    }

    uint AddRenderPass(const String& jsonPath, ivec2 renderPassResolution)
    {
        uint id = RenderPassList.size() + 1;
        CurrentRenderPassID = id;
        RenderPassList[id] = JsonRenderPass(id, jsonPath, renderPassResolution);
        return id;
    }

    uint AddRenderPass(const String& jsonPath, Texture& inputTexture, ivec2 renderPassResolution)
    {
        uint id = RenderPassList.size() + 1;
        CurrentRenderPassID = id;
        RenderPassList[id] = JsonRenderPass(id, jsonPath, inputTexture, renderPassResolution);
        return id;
    }

    VkGuid AddSpriteVRAM(const String& spritePath);
    VkGuid LoadTexture(const String& texturePath);
    VkGuid LoadMaterial(const String& materialPath);

    void Destroy();
};
extern RenderSystem renderSystem;