#include "SpriteBatchLayer.h"
#include "TypeDef.h"
#include "JsonRenderPass.h"
#include "AssetManager.h"

class RenderSystem
{
private:
    UM_PipelineID CurrentGraphicsPipelineID = 0;
    UM_RenderPassID CurrentRenderPassID = 0;

    Vector<SpriteBatchLayer> SpriteLayerList;
    UnorderedMap<UM_PipelineID, JsonPipeline> GraphicsPipelineList;
    UnorderedMap<UM_RenderPassID, JsonRenderPass> RenderPassList;
    UnorderedMap<uint32, RenderedTexture> RenderedTextureList;
    UnorderedMap<uint32, Vector<SpriteInstanceStruct>> BatchLayerList;
    UnorderedMap<uint32, Vector<VkVertexInputBindingDescription>> VertexInputBindingList;
    UnorderedMap<uint32, Vector<VkVertexInputAttributeDescription>> VertexInputAttributeDescription;

    Vector<VkClearValue> clearValues;
    VkRenderPassBeginInfo renderPassInfo;
    VkCommandBufferBeginInfo CommandBufferBeginInfo;


    void LoadRenderPass(SharedPtr<JsonRenderPass> renderPass, SharedPtr<JsonPipeline> renderPipeline)
    {
        JsonRenderPassPtr = renderPass;
        JsonPipelinePtr = renderPipeline;

        clearValues =
        {
            VkClearValue{.color = { {0.0f, 0.0f, 0.0f, 1.0f} } },
            VkClearValue{.depthStencil = { 1.0f, 0 } }
        };

        renderPassInfo =
        {
            .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
            .renderPass = JsonRenderPassPtr->RenderPass,
            .framebuffer = JsonRenderPassPtr->FrameBufferList[cRenderer.ImageIndex],
            .renderArea
            {
                .offset = {0, 0},
                .extent =
                {
                    .width = static_cast<uint32>(JsonRenderPassPtr->RenderPassResolution.x),
                    .height = static_cast<uint32>(JsonRenderPassPtr->RenderPassResolution.y)
                }
            },
            .clearValueCount = static_cast<uint32>(clearValues.size()),
            .pClearValues = clearValues.data()
        };

        VkCommandBufferBeginInfo CommandBufferBeginInfo
        {
            .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
            .flags = VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
        };
    }

   /* void SortSpritesByLayer(Vector<SpriteBatchLayerECS>& sprites)
    {
        std::sort(sprites.begin(), sprites.end(), [](const SpriteBatchLayerECS& spriteA, const SpriteBatchLayerECS& spriteB)
            {
                return spriteA.SpriteLayerIndex > spriteB.SpriteLayerIndex;
            });
    }

    void InstanceUpdate(VkCommandBuffer& commandBuffer, const float& deltaTime)
    {
        for (auto& gameObj : assetManager.GameObjectList)
        {
            const Transform2DComponent transformComponent = assetManager.TransformComponentList[gameObj];
            const SpriteSheetECS spriteSheet = assetManager.SpriteSheetList[gameObj];
            SpriteECS spriteObj = assetManager.SpriteList[gameObj];
            Animation2D animation = spriteObj.AnimationList[spriteObj.CurrentAnimationID];
            Vector<SpriteInstanceStructECS> spriteInstanceList = BatchLayerList[gameObj];

            for (auto& sprite : spriteInstanceList)
            {
                mat4 spriteMatrix = mat4(1.0f);
                spriteMatrix = glm::translate(spriteMatrix, vec3(transformComponent.GameObjectPosition.x, transformComponent.GameObjectPosition.y, 0.0f));
                spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transformComponent.GameObjectRotation.x), vec3(1.0f, 0.0f, 0.0f));
                spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transformComponent.GameObjectRotation.y), vec3(0.0f, 1.0f, 0.0f));
                spriteMatrix = glm::rotate(spriteMatrix, glm::radians(0.0f), vec3(0.0f, 0.0f, 1.0f));
                spriteMatrix = glm::scale(spriteMatrix, vec3(transformComponent.GameObjectScale.x, transformComponent.GameObjectScale.y, 0.0f));

                sprite.SpritePosition = transformComponent.GameObjectPosition;
                sprite.SpriteSize = sprite.SpriteSize;
                sprite.UVOffset = vec4(spriteSheet.SpriteUVSize.x * animation.FrameList[spriteObj.CurrentFrame].x, spriteSheet.SpriteUVSize.y * animation.FrameList[spriteObj.CurrentFrame].y, spriteSheet.SpriteUVSize.x, spriteSheet.SpriteUVSize.y);
                sprite.Color = sprite.Color;
                sprite.MaterialID = sprite.MaterialID;
                sprite.InstanceTransform = spriteMatrix;

                animation.CurrentFrameTime += deltaTime;
                if (animation.CurrentFrameTime >= animation.FrameHoldTime)
                {
                    spriteObj.CurrentFrame += 1;
                    animation.CurrentFrameTime = 0.0f;
                    if (spriteObj.CurrentFrame > animation.FrameList.size() - 1)
                    {
                        spriteObj.CurrentFrame = 0;
                    }
                }
            }
        }
    }*/

public:

    RenderSystem()
    {
    }

    RenderSystem(Vector<String>& renderPassJsonList)
    {

       // auto asdf = JsonRenderPass("../RenderPass/FrameBufferRenderPass.json", levelRenderer->RenderedColorTextureList[0], ivec2(cRenderer.SwapChain.SwapChainResolution.width, cRenderer.SwapChain.SwapChainResolution.height);
        for (int x = 0; x < renderPassJsonList.size(); x++)
        {
            //RenderPassList[0] = JsonRenderPass("../RenderPass/FrameBufferRenderPass.json", levelRenderer->RenderedColorTextureList[0], ivec2(cRenderer.SwapChain.SwapChainResolution.width, cRenderer.SwapChain.SwapChainResolution.height);
        }
       // LoadRenderPass(renderPass, renderPipeline);
       // SpriteLayerList.emplace_back(SpriteBatchLayerECS());
    }

    ~RenderSystem()
    {

    }

    VkCommandBuffer RenderFrameBuffer(const float deltaTime, SceneDataBuffer& sceneDataBuffer, uint32 gameObjectID)
    {
        const JsonPipeline pipeline = GraphicsPipelineList[CurrentGraphicsPipelineID];
        const JsonRenderPass renderPass = RenderPassList[CurrentRenderPassID];
        const VkCommandBuffer commandBuffer = renderPass.CommandBuffer;

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

    VkCommandBuffer Render2D(const float deltaTime, SceneDataBuffer& sceneDataBuffer, uint32 gameObjectID)
    {
        const JsonPipeline pipeline = GraphicsPipelineList[CurrentGraphicsPipelineID];
        const JsonRenderPass renderPass = RenderPassList[CurrentRenderPassID];
        const VkCommandBuffer commandBuffer = renderPass.CommandBuffer;
        const Vector<SpriteInstanceStruct> batchLayerList = BatchLayerList[gameObjectID];

        //SortSpritesByLayer(SpriteLayerList);
      //  InstanceUpdate(commandBuffer, deltaTime);

        VULKAN_RESULT(vkResetCommandBuffer(commandBuffer, 0));
        VULKAN_RESULT(vkBeginCommandBuffer(commandBuffer, &CommandBufferBeginInfo));
        vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VK_SUBPASS_CONTENTS_INLINE);
        for (auto batchLayer : SpriteLayerList)
        {
            VkDeviceSize offsets[] = { 0 };
            vkCmdPushConstants(commandBuffer, pipeline.PipelineLayout, VK_SHADER_STAGE_VERTEX_BIT | VK_SHADER_STAGE_FRAGMENT_BIT, 0, sizeof(sceneDataBuffer), &sceneDataBuffer);
            vkCmdBindPipeline(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline.Pipeline);
            vkCmdBindDescriptorSets(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline.PipelineLayout, 0, pipeline.DescriptorSetList.size(), pipeline.DescriptorSetList.data(), 0, nullptr);
            vkCmdBindVertexBuffers(commandBuffer, 0, 1, batchLayer.SpriteLayerMesh->GetVertexBuffer().get(), offsets);
            vkCmdBindVertexBuffers(commandBuffer, 1, 1, &batchLayer.SpriteBuffer.Buffer, offsets);
            vkCmdBindIndexBuffer(commandBuffer, *batchLayer.SpriteLayerMesh->GetIndexBuffer().get(), 0, VK_INDEX_TYPE_UINT32);
            vkCmdDrawIndexed(commandBuffer, batchLayer.SpriteIndexList.size(), batchLayer.SpriteInstanceList.size(), 0, 0, 0);
        }
        vkCmdEndRenderPass(commandBuffer);
        VULKAN_RESULT(vkEndCommandBuffer(commandBuffer));
        return commandBuffer;
    }
};