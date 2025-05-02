#pragma once
#include "VulkanRenderer.h"
#include "SpriteBatchLayer.h"
#include "TypeDef.h"
#include "JsonRenderPass.h"
#include "AssetManager.h"
#include "DepthTexture.h"
#include "RenderedTexture.h"
#include "ECGid.h"
#include "InterfaceRenderPass.h"

typedef uint UM_SpriteID;
typedef uint UM_SpriteBatchID;
typedef uint UM_RenderPassID;
typedef uint UM_RenderPipelineID;

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

    UM_PipelineID CurrentGraphicsPipelineID = 0;
    UM_RenderPassID CurrentRenderPassID = 0;

    UnorderedMap<VkGuid, Texture> TextureList;
    UnorderedMap<VkGuid, Material> MaterialList;
    UnorderedMap<VkGuid, SpriteVram> VramSpriteList;

    UnorderedMap<RenderPassID, JsonRenderPass> RenderPassList;
    UnorderedMap<RenderPassID, DepthTexture> DepthTextureList;
    UnorderedMap<RenderPassID, VkRenderPassBeginInfo> RenderPassInfoList;
    UnorderedMap<RenderPassID, VkRect2D> RenderPassResolutionList;
    UnorderedMap<RenderPassID, Vector<JsonPipeline>> RenderPipelineList;
    UnorderedMap<RenderPassID, Vector<RenderedTexture>> RenderedTextureList;
    UnorderedMap<RenderPassID, Vector<SpriteBatchLayer>> SpriteBatchLayerList;
    UnorderedMap<RenderPassID, Vector<VkClearValue>> ClearValueList;

    UnorderedMap<UM_RenderPipelineID, Vector<SharedPtr<Texture>>> InputTextureList;

    UnorderedMap<UM_SpriteBatchID, SpriteInstanceBuffer> SpriteInstanceBufferList;
    UnorderedMap<UM_SpriteBatchID, Vector<GameObjectID>> SpriteBatchLayerObjectList;
    UnorderedMap<UM_SpriteBatchID, Vector<SpriteInstanceStruct>> SpriteInstanceList;
    UnorderedMap<UM_SpriteBatchID, SpriteMesh> SpriteMeshList;

    VkCommandBufferBeginInfo CommandBufferBeginInfo;

    RenderSystem();
    ~RenderSystem();

    void StartUp();
    void Update(const float& deltaTime);
    void UpdateBufferIndex();

    VkCommandBuffer RenderFrameBuffer(RenderPassID renderPassId);
    VkCommandBuffer RenderSprites(RenderPassID renderPassId, const float deltaTime, SceneDataBuffer& sceneDataBuffer);
 
    RenderPassID AddRenderPass(const String& jsonPath, ivec2 renderPassResolution);
    RenderPassID AddRenderPass(const String& jsonPath, Texture& inputTexture, ivec2 renderPassResolution);
    VkGuid AddSpriteVRAM(const String& spritePath);
    VkGuid LoadTexture(const String& texturePath);
    VkGuid LoadMaterial(const String& materialPath);

    void Destroy();
};
extern RenderSystem renderSystem;