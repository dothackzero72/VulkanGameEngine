#include "SpriteBatchLayer.h"
#include "TypeDef.h"
#include "JsonRenderPass.h"
#include "AssetManager.h"
#include "DepthTexture.h"
#include "RenderedTexture.h"
#include "ECGid.h"

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

    UM_PipelineID CurrentGraphicsPipelineID = 0;
    UM_RenderPassID CurrentRenderPassID = 0;

    UnorderedMap<VkGuid, Texture> TextureList;
    UnorderedMap<VkGuid, Material> MaterialList;
    UnorderedMap<VkGuid, SpriteVram> VramSpriteList;

    UnorderedMap<UM_RenderPassID, JsonRenderPass> RenderPassList;
    UnorderedMap<UM_RenderPassID, DepthTexture> DepthTextureList;
    UnorderedMap<UM_RenderPassID, VkRenderPassBeginInfo> RenderPassInfoList;
    UnorderedMap<UM_RenderPassID, ivec2> RenderPassResolutionList;
    UnorderedMap<UM_RenderPassID, Vector<JsonPipeline>> RenderPipelineList;
    UnorderedMap<UM_RenderPassID, Vector<SharedPtr<Texture>>> InputTextureList;
    UnorderedMap<UM_RenderPassID, Vector<RenderedTexture>> RenderedTextureList;
    UnorderedMap<UM_RenderPassID, Vector<SpriteBatchLayer>> SpriteBatchLayerList;
    UnorderedMap<UM_RenderPassID, Vector<VkClearValue>> ClearValueList;

    UnorderedMap<UM_SpriteBatchID, SpriteInstanceBuffer> SpriteInstanceBufferList;
    UnorderedMap<UM_SpriteBatchID, Vector<GameObjectID>> SpriteBatchLayerObjectList;
    UnorderedMap<UM_SpriteBatchID, Vector<SpriteInstanceStruct>> SpriteInstanceList;

    VkCommandBufferBeginInfo CommandBufferBeginInfo;

    RenderSystem();
    ~RenderSystem();

    void Update(const float& deltaTime);
    void UpdateBufferIndex();

    VkCommandBuffer RenderFrameBuffer(uint renderPassId);
    VkCommandBuffer RenderSprites(uint renderPassId, const float deltaTime, SceneDataBuffer& sceneDataBuffer);
 
    uint AddRenderPass(const String& jsonPath, ivec2 renderPassResolution);
    uint AddRenderPass(const String& jsonPath, Texture& inputTexture, ivec2 renderPassResolution);
    VkGuid AddSpriteVRAM(const String& spritePath);
    VkGuid LoadTexture(const String& texturePath);
    VkGuid LoadMaterial(const String& materialPath);

    void Destroy();
};
extern RenderSystem renderSystem;