#pragma once
#include <CoreVulkanRenderer.h>
#include <VulkanRenderPass.h>
#include <ImGuiFunc.h>
#include "SceneDataBuffer.h"


typedef uint UM_SpriteID;
typedef uint UM_SpriteBatchID;
typedef uint UM_RenderPassID;
typedef uint UM_RenderPipelineID;
typedef uint UM_LevelID;
typedef VkGuid RenderPassGuid;
typedef VkGuid LevelGuid;

class RenderSystem
{
    friend class JsonRenderPass;
private:
    void RecreateSwapchain();

    const Vector<VkDescriptorBufferInfo> GetVertexPropertiesBuffer();
    const Vector<VkDescriptorBufferInfo> GetIndexPropertiesBuffer();
    const Vector<VkDescriptorBufferInfo> GetGameObjectTransformBuffer();
    const Vector<VkDescriptorBufferInfo> GetMeshPropertiesBuffer(VkGuid& levelLayerId);
    const Vector<VkDescriptorImageInfo>  GetTexturePropertiesBuffer(VkGuid& renderPassId, Vector<SharedPtr<Texture>>& renderedTextureList);
    const Vector<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer();

    
public:

    ImGuiRenderer                                                 imGuiRenderer;

    Vector<VkImage>                                               SwapChainImages;
    Vector<VkImageView>                                           SwapChainImageViews;
    VkExtent2D                                                    SwapChainResolution;
    VkSwapchainKHR                                                Swapchain;
    VkPhysicalDeviceFeatures                                      PhysicalDeviceFeatures;

    UnorderedMap<RenderPassGuid, VulkanRenderPass>                RenderPassList;
    UnorderedMap<RenderPassGuid, Vector<VulkanPipeline>>          RenderPipelineList;
    UnorderedMap<RenderPassGuid, VkRenderPassBeginInfo>           RenderPassInfoList;
    UnorderedMap<RenderPassGuid, RenderPassBuildInfoModel>        renderPassBuildInfoList;
    UnorderedMap<RenderPassGuid, ivec2>                           RenderPassResolutionList;
    VkCommandBufferBeginInfo                                      CommandBufferBeginInfo;

    RenderSystem();
    ~RenderSystem();

    void StartUp();
    void Update(const float& deltaTime);
    void UpdateBufferIndex();

    VkCommandBuffer RenderFrameBuffer(VkGuid& renderPassId);
    VkCommandBuffer RenderLevel(VkGuid& renderPassId, VkGuid& levelId, const float deltaTime, SceneDataBuffer& sceneDataBuffer);
 
    VkGuid AddRenderPass(VkGuid& levelId, const String& jsonPath, ivec2 renderPassResolution);
    VkGuid AddRenderPass(VkGuid& levelId, const String& jsonPath, Texture& inputTexture, ivec2 renderPassResolution);

    void Destroy();
};
extern RenderSystem renderSystem;