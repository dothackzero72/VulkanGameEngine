#pragma once
#include "VulkanRenderer.h"
#include "SpriteBatchLayer.h"
#include "TypeDef.h"
#include "JsonPipeline.h"
#include "JsonRenderPass.h"
#include "AssetManager.h"
#include "DepthTexture.h"
#include "RenderedTexture.h"
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
class Sprite;
class JsonPipeline;
class JsonRenderPass;

class RenderSystem
{
    friend class JsonPipeline;
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

    LevelLayout                                                   levelLayout;

    Vector<VkImage>                                               SwapChainImages;
    Vector<VkImageView>                                           SwapChainImageViews;
    VkExtent2D                                                    SwapChainResolution;
    VkSwapchainKHR                                                Swapchain;
    VkPhysicalDeviceFeatures                                      PhysicalDeviceFeatures;

    UnorderedMap<VkGuid, Texture>                                 TextureList;
    UnorderedMap<VkGuid, Material>                                MaterialList;
    UnorderedMap<VkGuid, SpriteVram>                              VramSpriteList;
    UnorderedMap<VkGuid, LevelTileSet>                            LevelTileSetList;
    UnorderedMap<LevelGuid, Vector<LevelLayerMesh>>               LevelLayerMeshList;

    UnorderedMap<RenderPassID, Vector<VkGuid>>                    TextureRenderPassList;
    UnorderedMap<RenderPassID, Vector<VkGuid>>                    MaterialRenderPassList;
    UnorderedMap<RenderPassID, Vector<VkGuid>>                    VramSpriteRenderPassList;
    UnorderedMap<RenderPassID, Vector<VkGuid>>                    LevelTileSetRenderPassList;
    UnorderedMap<RenderPassID, Vector<VkGuid>>                    LevelLayerMeshRenderPassList;

    UnorderedMap<RenderPassID, JsonRenderPass>                    RenderPassList;
    UnorderedMap<RenderPassID, DepthTexture>                      DepthTextureList;
    UnorderedMap<RenderPassID, VkRenderPassBeginInfo>             RenderPassInfoList;
    UnorderedMap<RenderPassID, ivec2>                             RenderPassResolutionList;
    UnorderedMap<RenderPassID, Vector<JsonPipeline>>              RenderPipelineList;
    UnorderedMap<RenderPassID, Vector<RenderedTexture>>           RenderedTextureList;
    UnorderedMap<RenderPassID, Vector<SpriteBatchLayer>>          SpriteBatchLayerList;
    UnorderedMap<RenderPassID, Vector<VkClearValue>>              ClearValueList;
    UnorderedMap<RenderPassID, RenderPassBuildInfoModel>          renderPassBuildInfoList;

    UnorderedMap<RenderPassID, Vector<VkDescriptorBufferInfo>>    VertexDescriptorBufferList;
    UnorderedMap<RenderPassID, Vector<VkDescriptorBufferInfo>>    IndexDescriptorBufferList;
    UnorderedMap<RenderPassID, Vector<VkDescriptorBufferInfo>>    TransformDescriptorBufferList;
    UnorderedMap<RenderPassID, Vector<VkDescriptorBufferInfo>>    MeshDescriptorBufferList;
    UnorderedMap<RenderPassID, Vector<VkDescriptorImageInfo>>     TextureDescriptorBufferList;
    UnorderedMap<RenderPassID, Vector<VkDescriptorBufferInfo>>    MaterialDescriptorBufferList;

    UnorderedMap<UM_RenderPipelineID, RenderPipelineModel>        renderPipelineModelList;
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

    VkCommandBuffer RenderFrameBuffer(RenderPassID renderPassId);
    VkCommandBuffer RenderLevel(LevelGuid& levelId, RenderPassID renderPassId, const float deltaTime, SceneDataBuffer& sceneDataBuffer);
    VkCommandBuffer RenderSprites(RenderPassID renderPassId, const float deltaTime, SceneDataBuffer& sceneDataBuffer);
 
    RenderPassID AddRenderPass(const String& jsonPath, ivec2 renderPassResolution);
    RenderPassID AddRenderPass(const String& jsonPath, Texture& inputTexture, ivec2 renderPassResolution);
    VkGuid AddSpriteVRAM(const String& spritePath);
    VkGuid AddTileSetVRAM(const String& tileSetPath);
    VkGuid LoadTexture(const String& texturePath);
    VkGuid LoadMaterial(const String& materialPath);
    VkGuid LoadLevelLayout(const String& levelLayoutPath);

    void Destroy();
};
extern RenderSystem renderSystem;