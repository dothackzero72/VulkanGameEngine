#pragma once
#include "VulkanRenderer.h"
#include "SpriteBatchLayer.h"
#include "TypeDef.h"
#include <VulkanPipeline.h>
#include "JsonRenderPass.h"
#include "AssetManager.h"
#include "ECSid.h"
#include "InterfaceRenderPass.h"
#include "Mesh.h"
#include "LevelLayer.h"
#include "LevelTileSet.h"
#include "ShaderSystem.h"
#include "LevelLayout.h"
#include "Tile.h"

typedef uint UM_SpriteID;
typedef uint UM_SpriteBatchID;
typedef uint UM_RenderPassID;
typedef uint UM_RenderPipelineID;
typedef uint UM_LevelID;
typedef VkGuid LevelGuid;
typedef VkGuid RenderPassGuid;

class Sprite;
class JsonRenderPass;
class RenderSystem
{
    friend class JsonRenderPass;
private:

    //SharedPtr<uint32>                   ImageIndex;
    //SharedPtr<uint32>                   CommandIndex;
    //SharedPtr<uint32>			        SwapChainImageCount;
    //SharedPtr<uint32>		            GraphicsFamily;
    //SharedPtr<uint32>		            PresentFamily;

    //SharedPtr<VkInstance>               Instance;
    SharedPtr<VkDevice>                 Device;
    //SharedPtr<VkPhysicalDevice>         PhysicalDevice;
    //SharedPtr<VkSurfaceKHR>             Surface;
    //SharedPtr<VkCommandPool>            CommandPool;
    //SharedPtr<VkDebugUtilsMessengerEXT> DebugMessenger;
    //SharedPtr<VkQueue>	                GraphicsQueue;
    //SharedPtr<VkQueue>	                PresentQueue;
    //Vector<SharedPtr<VkFence>>          InFlightFences;
    //Vector<SharedPtr<VkSemaphore>>      AcquireImageSemaphores;
    //Vector<SharedPtr<VkSemaphore>>      PresentImageSemaphores;

    //VkFormat                 Format;
    //VkColorSpaceKHR          ColorSpace;
    //VkPresentModeKHR         PresentMode;

    VkResult CreateCommandBuffer();

    void RecreateSwapchain();

    const Vector<VkDescriptorBufferInfo> GetVertexPropertiesBuffer();
    const Vector<VkDescriptorBufferInfo> GetIndexPropertiesBuffer();
    const Vector<VkDescriptorBufferInfo> GetGameObjectTransformBuffer();
    const Vector<VkDescriptorBufferInfo> GetMeshPropertiesBuffer(VkGuid& levelLayerId);
    const Vector<VkDescriptorImageInfo>  GetTexturePropertiesBuffer(VkGuid& renderPassId, Vector<SharedPtr<Texture>>& renderedTextureList);
    const Vector<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer();

    void DestroyGraphicsPipeline(VkGuid& guid);

public:
    Vector<Vertex2D> SpriteVertexList =
    {
        Vertex2D(vec2(0.0f, 1.0f), vec2(0.0f, 0.0f)),
        Vertex2D(vec2(1.0f, 1.0f), vec2(1.0f, 0.0f)),
        Vertex2D(vec2(1.0f, 0.0f), vec2(1.0f, 1.0f)),
        Vertex2D(vec2(0.0f, 0.0f), vec2(0.0f, 1.0f)), 
    };

    Vector<uint32> SpriteIndexList =
    {
        0, 3, 1,
        1, 3, 2
    };

    LevelLayout                                                   levelLayout;

    Vector<VkImage>                                               SwapChainImages;
    Vector<VkImageView>                                           SwapChainImageViews;
    VkExtent2D                                                    SwapChainResolution;
    VkSwapchainKHR                                                Swapchain;
    VkPhysicalDeviceFeatures                                      PhysicalDeviceFeatures;

    UnorderedMap<RenderPassGuid, Texture>                         TextureList;
    UnorderedMap<RenderPassGuid, Material>                        MaterialList;
    UnorderedMap<RenderPassGuid, SpriteVram>                      VramSpriteList;
    UnorderedMap<RenderPassGuid, LevelTileSet>                    LevelTileSetList;
    UnorderedMap<LevelGuid, Vector<LevelLayerMesh>>               LevelLayerMeshList;

    UnorderedMap<RenderPassGuid, JsonRenderPass>                  RenderPassList;
    UnorderedMap<RenderPassGuid, Texture>                         DepthTextureList;
    UnorderedMap<RenderPassGuid, VkRenderPassBeginInfo>           RenderPassInfoList;
    UnorderedMap<RenderPassGuid, ivec2>                           RenderPassResolutionList;
    UnorderedMap<RenderPassGuid, Vector<VulkanPipeline>>          RenderPipelineList;
    UnorderedMap<RenderPassGuid, Vector<Texture>>                 RenderedTextureList;
    UnorderedMap<RenderPassGuid, Vector<SpriteBatchLayer>>        SpriteBatchLayerList;
    UnorderedMap<RenderPassGuid, Vector<VkClearValue>>            ClearValueList;
    UnorderedMap<RenderPassGuid, RenderPassBuildInfoModel>        renderPassBuildInfoList;

    UnorderedMap<UM_RenderPipelineID, Vector<SharedPtr<Texture>>> InputTextureList;

    UnorderedMap<UM_SpriteBatchID, SpriteInstanceBuffer>          SpriteInstanceBufferList;
    UnorderedMap<UM_SpriteBatchID, Vector<GameObjectID>>          SpriteBatchLayerObjectList;
    UnorderedMap<UM_SpriteBatchID, Vector<SpriteInstanceStruct>>  SpriteInstanceList;
    UnorderedMap<UM_SpriteBatchID, SpriteMesh>                    SpriteMeshList;

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
    VkGuid AddSpriteVRAM(const String& spritePath);
    VkGuid AddTileSetVRAM(const String& tileSetPath);
    VkGuid LoadTexture(const String& texturePath);
    VkGuid LoadMaterial(const String& materialPath);
    VkGuid LoadLevelLayout(const String& levelLayoutPath);

    void Destroy();
};
extern RenderSystem renderSystem;