#include "SpriteBatchLayer.h"
#include "TypeDef.h"
#include "JsonRenderPass.h"
#include "AssetManager.h"

typedef uint UM_SpriteID;
typedef uint UM_SpriteBatchID;

class Sprite;
class RenderSystem
{
private:
    UM_PipelineID CurrentGraphicsPipelineID = 0;
    UM_RenderPassID CurrentRenderPassID = 0;

    UnorderedMap<UM_PipelineID, JsonPipeline> GraphicsPipelineList;
    UnorderedMap<UM_RenderPassID, JsonRenderPass> RenderPassList;
    UnorderedMap<uint32, RenderedTexture> RenderedTextureList;
    UnorderedMap<uint32, Vector<VkVertexInputBindingDescription>> VertexInputBindingList;
    UnorderedMap<uint32, Vector<VkVertexInputAttributeDescription>> VertexInputAttributeDescription;

    Vector<VkClearValue> clearValues;
    VkRenderPassBeginInfo renderPassInfo;
    VkCommandBufferBeginInfo CommandBufferBeginInfo;

public:
    UnorderedMap<UM_SpriteBatchID, Vector<SpriteInstanceStruct>> SpriteInstanceList;
    UnorderedMap<UM_SpriteBatchID, SpriteInstanceBuffer> SpriteInstanceBufferList;

    RenderSystem()
    {
    }

    RenderSystem(Vector<String>& renderPassJsonList, SharedPtr<Texture>& texture, SceneDataBuffer sceneDataBuffer)
    {
        GPUImport gpuImport;
        gpuImport.TextureList.emplace_back(texture);

        for (int x = 0; x < renderPassJsonList.size(); x++)
        {
            RenderPassList[x] = JsonRenderPass(renderPassJsonList[x], gpuImport, ivec2(cRenderer.SwapChain.SwapChainResolution.width, cRenderer.SwapChain.SwapChainResolution.height), sceneDataBuffer);
          //  GraphicsPipelineList[x] = RenderPassList[x].JsonPipelineList[0];
        }
    }

    ~RenderSystem()
    {

    }

    void Update(const float& deltaTime)
    {

    }

    VkCommandBuffer RenderFrameBuffer(const float deltaTime, SceneDataBuffer& sceneDataBuffer, uint32 gameObjectID)
    {
        const JsonPipeline pipeline = GraphicsPipelineList[CurrentGraphicsPipelineID];
        const JsonRenderPass renderPass = RenderPassList[CurrentRenderPassID];
        const VkCommandBuffer commandBuffer = renderPass.CommandBuffer;
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
        return commandBuffer;
    }

    VkCommandBuffer RenderSprites(const float deltaTime, SceneDataBuffer& sceneDataBuffer, uint32 gameObjectID)
    {
        const JsonPipeline pipeline = GraphicsPipelineList[CurrentGraphicsPipelineID];
        const JsonRenderPass renderPass = RenderPassList[CurrentRenderPassID];
        VkCommandBuffer commandBuffer = renderPass.CommandBuffer;
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
        return commandBuffer;
    }

    void Destroy()
    {

    }
};
extern RenderSystem renderSystem;
RenderSystem renderSystem = RenderSystem();