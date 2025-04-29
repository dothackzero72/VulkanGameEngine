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
    const Vector<VkDescriptorImageInfo>  GetTexturePropertiesBuffer(Vector<Texture>& renderedTextureList);
    const Vector<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer();

public:
    VkCommandBuffer CommandBuffer = VK_NULL_HANDLE;
    UM_PipelineID CurrentGraphicsPipelineID = 0;
    UM_RenderPassID CurrentRenderPassID = 0;

    UnorderedMap<UM_RenderPassID, JsonRenderPass> RenderPassList;
    UnorderedMap<UM_RenderPassID, Vector<JsonPipeline>> RenderPipelineList;
    UnorderedMap<UM_RenderPassID, Vector<Texture>> InputTextureList;
    UnorderedMap<UM_RenderPassID, Vector<RenderedTexture>> RenderedTextureList;
    UnorderedMap<UM_RenderPassID, DepthTexture> DepthTextureList;
    UnorderedMap<UM_RenderPassID, Vector<SpriteBatchLayer>> SpriteBatchLayerList;
    UnorderedMap<UM_RenderPassID, Vector<VkVertexInputBindingDescription>> VertexInputBindingList;
    UnorderedMap<UM_RenderPassID, Vector<VkVertexInputAttributeDescription>> VertexInputAttributeDescription;

    UnorderedMap<UM_SpriteBatchID, Vector<SpriteInstanceStruct>> SpriteInstanceList;
    UnorderedMap<UM_SpriteBatchID, SpriteInstanceBuffer> SpriteInstanceBufferList;

    Vector<VkClearValue> clearValues;
    VkRenderPassBeginInfo renderPassInfo;
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

    }

    VkCommandBuffer RenderFrameBuffer()
    {
        const JsonRenderPass renderPass = RenderPassList[CurrentRenderPassID];
        const JsonPipeline pipeline = RenderPipelineList[CurrentRenderPassID][0];
        const VkCommandBuffer commandBuffer = CommandBuffer;
        VkRenderPassBeginInfo renderPassInfo = renderPass.RenderPassInfo;

        renderPassInfo.clearValueCount = static_cast<uint32>(renderPass.ClearValueList.size());
        renderPassInfo.pClearValues = renderPass.ClearValueList.data();
        renderPassInfo.framebuffer = renderPass.FrameBufferList[cRenderer.ImageIndex];

        VULKAN_RESULT(vkResetCommandBuffer(commandBuffer, 0));
        VULKAN_RESULT(vkBeginCommandBuffer(commandBuffer, &CommandBufferBeginInfo));
        vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VK_SUBPASS_CONTENTS_INLINE);
        vkCmdBindPipeline(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline.Pipeline);
        vkCmdBindDescriptorSets(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline.PipelineLayout, 0, pipeline.DescriptorSetList.size(), pipeline.DescriptorSetList.data(), 0, nullptr);
        vkCmdDraw(commandBuffer, 6, 1, 0, 0);
        vkCmdEndRenderPass(commandBuffer);
        vkEndCommandBuffer(commandBuffer);
        return CommandBuffer;
    }

    VkCommandBuffer RenderSprites(const float deltaTime, SceneDataBuffer& sceneDataBuffer, uint32 gameObjectID)
    {
        //const JsonPipeline pipeline = GraphicsPipelineList[CurrentGraphicsPipelineID];
        //const JsonRenderPass renderPass = RenderPassList[CurrentRenderPassID];
        //VkCommandBuffer commandBuffer = renderPass.CommandBuffer;
        //VkRenderPassBeginInfo renderPassInfo = renderPass.RenderPassInfo;

        //renderPassInfo.clearValueCount = static_cast<uint32>(renderPass.ClearValueList.size());
        //renderPassInfo.pClearValues = renderPass.ClearValueList.data();
        //renderPassInfo.framebuffer = renderPass.FrameBufferList[cRenderer.ImageIndex];

        //VULKAN_RESULT(vkResetCommandBuffer(commandBuffer, 0));
        //VULKAN_RESULT(vkBeginCommandBuffer(commandBuffer, &CommandBufferBeginInfo));

        //for (auto& spriteLayer : SpriteLayerList)
        //{
        //    spriteLayer.Update(commandBuffer, deltaTime);
        //}

        //vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VK_SUBPASS_CONTENTS_INLINE);
        //for (auto batchLayer : SpriteLayerList)
        //{
        //    const Vector<SpriteInstanceStruct> spriteInstanceList = SpriteInstanceList[batchLayer.SpriteBatchLayerID];
        //    const SpriteInstanceBuffer spriteInstanceBuffer = SpriteInstanceBufferList[batchLayer.SpriteBatchLayerID];

        //    VkDeviceSize offsets[] = { 0 };
        //    vkCmdPushConstants(commandBuffer, pipeline.PipelineLayout, VK_SHADER_STAGE_VERTEX_BIT | VK_SHADER_STAGE_FRAGMENT_BIT, 0, sizeof(sceneDataBuffer), &sceneDataBuffer);
        //    vkCmdBindPipeline(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline.Pipeline);
        //    vkCmdBindDescriptorSets(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline.PipelineLayout, 0, pipeline.DescriptorSetList.size(), pipeline.DescriptorSetList.data(), 0, nullptr);
        //    vkCmdBindVertexBuffers(commandBuffer, 0, 1, batchLayer.SpriteLayerMesh.GetVertexBuffer().get(), offsets);
        //    vkCmdBindVertexBuffers(commandBuffer, 1, 1, &spriteInstanceBuffer.Buffer, offsets);
        //    vkCmdBindIndexBuffer(commandBuffer, *batchLayer.SpriteLayerMesh.GetIndexBuffer().get(), 0, VK_INDEX_TYPE_UINT32);
        //    vkCmdDrawIndexed(commandBuffer, batchLayer.SpriteIndexList.size(), spriteInstanceList.size(), 0, 0, 0);
        //}
        //vkCmdEndRenderPass(commandBuffer);
        //VULKAN_RESULT(vkEndCommandBuffer(commandBuffer));
        return CommandBuffer;
    }

    void AddRenderPass(const String& jsonPath, Texture& inputTexture, ivec2 renderPassResolution)
    {
        uint id = RenderPassList.size() + 1;
        CurrentRenderPassID = id;
        RenderPassList[id] = JsonRenderPass(id, jsonPath, inputTexture, renderPassResolution);
    }

    void Destroy()
    {

    }
};
extern RenderSystem renderSystem;