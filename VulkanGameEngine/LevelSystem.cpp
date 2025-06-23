#include "LevelSystem.h"
#include "MaterialSystem.h"
#include "TextureSystem.h"
#include "GameObjectSystem.h"
#include "MeshSystem.h"
#include "VRAM.h"

LevelSystem levelSystem = LevelSystem();

LevelSystem::LevelSystem()
{
}

LevelSystem::~LevelSystem()
{
}

void LevelSystem::LoadLevel(const String& levelPath)
{
    OrthographicCamera = std::make_shared<OrthographicCamera2D>(OrthographicCamera2D(vec2((float)renderSystem.renderer.SwapChainResolution.width, (float)renderSystem.renderer.SwapChainResolution.height), vec3(0.0f, 0.0f, 0.0f)));

    VkGuid vramId;
    VkGuid tileSetId;

    nlohmann::json json = Json::ReadJson(levelPath);
    VkGuid LevelId = VkGuid(json["LevelID"].get<String>().c_str());
    for (int x = 0; x < json["LoadTextures"].size(); x++)
    {
        textureSystem.LoadTexture(json["LoadTextures"][x]);
    }
    for (int x = 0; x < json["LoadMaterials"].size(); x++)
    {
        materialSystem.LoadMaterial(json["LoadMaterials"][x]);
    }
    for (int x = 0; x < json["LoadSpriteVRAM"].size(); x++)
    {
        vramId = LoadSpriteVRAM(json["LoadSpriteVRAM"][x]);
    }
    for (int x = 0; x < json["LoadTileSetVRAM"].size(); x++)
    {
        tileSetId = LoadTileSetVRAM(json["LoadTileSetVRAM"][x]);
    }
    for (int x = 0; x < json["GameObjectList"].size(); x++)
    {
        String objectJson = json["GameObjectList"][x]["GameObjectPath"];
        vec2   positionOverride = vec2(json["GameObjectList"][x]["GameObjectPositionOverride"][0], json["GameObjectList"][x]["GameObjectPositionOverride"][1]);
        gameObjectSystem.CreateGameObject(objectJson, positionOverride);
    }
    {
        LoadLevelLayout(json["LoadLevelLayout"]);
    }
    {
        Level = Level2D(LevelId, tileSetId, levelLayout.LevelBounds, levelLayout.LevelMapList);
    }

    VkGuid dummyGuid = VkGuid();
    nlohmann::json json2 = Json::ReadJson("../RenderPass/LevelShader2DRenderPass.json");
    spriteRenderPass2DId = VkGuid(json2["RenderPassId"].get<String>().c_str());

    SpriteBatchLayerListMap[spriteRenderPass2DId].emplace_back(SpriteBatchLayer(spriteRenderPass2DId));
    spriteRenderPass2DId = renderSystem.LoadRenderPass(Level.LevelId, "../RenderPass/LevelShader2DRenderPass.json", ivec2(renderSystem.renderer.SwapChainResolution.width, renderSystem.renderer.SwapChainResolution.height));
    frameBufferId = renderSystem.LoadRenderPass(dummyGuid, "../RenderPass/FrameBufferRenderPass.json", textureSystem.FindRenderedTextureList(spriteRenderPass2DId)[0], ivec2(renderSystem.renderer.SwapChainResolution.width, renderSystem.renderer.SwapChainResolution.height));
}

void LevelSystem::DestoryLevel()
{
}

void LevelSystem::Update(const float& deltaTime)
{
    OrthographicCamera->Update(SceneProperties);
    Level.Update(deltaTime);
}

void LevelSystem::Draw(Vector<VkCommandBuffer>& commandBufferList, const float& deltaTime)
{
    commandBufferList.emplace_back(renderSystem.RenderLevel(spriteRenderPass2DId, Level.LevelId, deltaTime, SceneProperties));
    commandBufferList.emplace_back(renderSystem.RenderFrameBuffer(frameBufferId));
}

void LevelSystem::DestroyDeadGameObjects()
{
    //if (gameObjectSystem.GameObjectList.empty())
    //{
    //    return;
    //}

    //Vector<SharedPtr<GameObject>> deadGameObjectList;
    //for (auto it = GameObjectList.begin(); it != GameObjectList.end(); ++it) {
    //    if (!(*it)->GameObjectAlive) {
    //        deadGameObjectList.push_back(*it);
    //    }
    //}

    //if (!deadGameObjectList.empty())
    //{
    //    for (auto& gameObject : deadGameObjectList) {
    //        if (SharedPtr spriteComponent = gameObject->GetComponentByComponentType(kSpriteComponent)) {
    //            SharedPtr sprite = std::dynamic_pointer_cast<SpriteComponent>(spriteComponent);
    //            if (sprite) {
    //                SharedPtr spriteObject = sprite->GetSprite();
    //                SpriteLayerList[0]->RemoveSprite(spriteObject);
    //            }
    //        }
    //        gameObject->Destroy();
    //    }

    //    GameObjectList.erase(std::remove_if(GameObjectList.begin(), GameObjectList.end(),
    //        [&](const SharedPtr<GameObject>& gameObject) {
    //            return !gameObject->GameObjectAlive;
    //        }),
    //        GameObjectList.end());
    //}
}

VkGuid LevelSystem::LoadSpriteVRAM(const String& spritePath)
{
    nlohmann::json json = Json::ReadJson(spritePath);
    VkGuid vramId = VkGuid(json["VramSpriteId"].get<String>().c_str());
    VkGuid materialId = VkGuid(json["MaterialId"].get<String>().c_str());

    auto it = VramSpriteMap.find(vramId);
    if (it != VramSpriteMap.end())
    {
        return vramId;
    }

    const Material& material = materialSystem.FindMaterial(materialId);
    const Texture& texture = textureSystem.FindTexture(material.AlbedoMapId);

    Animation2D* animationListPtr = nullptr;
    vec2* animationFrameListPtr = nullptr;
    size_t animationListCount;
    size_t animationFrameCount;

    VramSpriteMap[vramId] = VRAM_LoadSpriteVRAM(spritePath.c_str(), material, texture);
    VRAM_LoadSpriteAnimation(spritePath.c_str(), animationListPtr, animationFrameListPtr, animationListCount, animationFrameCount);

    Vector<Animation2D> animation2DList = Vector<Animation2D>(animationListPtr, animationListPtr + animationListCount);
    Vector<vec2> animationFrameList = Vector<vec2>(animationFrameListPtr, animationFrameListPtr + animationFrameCount);

    for (size_t x = 0; x < animation2DList.size(); x++)
    {
        AnimationMap[animation2DList[x].AnimationId] = animation2DList[x];
    }
    AnimationFrameListMap[vramId].emplace_back(animationFrameList);

    VRAM_DeleteSpriteAnimation(animationListPtr, animationFrameListPtr);
    return vramId;
}

VkGuid LevelSystem::LoadTileSetVRAM(const String& tileSetPath)
{
    if (tileSetPath.empty() ||
        tileSetPath == "")
    {
        return VkGuid();
    }

    nlohmann::json json = Json::ReadJson(tileSetPath);
    VkGuid tileSetId = VkGuid(json["TileSetId"].get<String>().c_str());
    VkGuid materialId = VkGuid(json["MaterialId"].get<String>().c_str());

    auto it = LevelTileSetMap.find(tileSetId);
    if (it != LevelTileSetMap.end())
    {
        return tileSetId;
    }

    const Material& material = materialSystem.FindMaterial(materialId);
    const Texture& tileSetTexture = textureSystem.FindTexture(material.AlbedoMapId);

    LevelTileSet tileSet = LevelTileSet();
    tileSet.TileSetId = VkGuid(json["TileSetId"].get<String>().c_str());
    tileSet.MaterialId = materialId;
    tileSet.TilePixelSize = ivec2{ json["TilePixelSize"][0], json["TilePixelSize"][1] };
    tileSet.TileSetBounds = ivec2{ tileSetTexture.width / tileSet.TilePixelSize.x,  tileSetTexture.height / tileSet.TilePixelSize.y };
    tileSet.TileUVSize = vec2(1.0f / (float)tileSet.TileSetBounds.x, 1.0f / (float)tileSet.TileSetBounds.y);
    for (int x = 0; x < json["TileList"].size(); x++)
    {
        Tile tile;
        tile.TileId = json["TileList"][x]["TileId"];
        tile.TileUVCellOffset = ivec2(json["TileList"][x]["TileUVCellOffset"][0], json["TileList"][x]["TileUVCellOffset"][1]);
        tile.TileLayer = json["TileList"][x]["TileLayer"];
        tile.IsAnimatedTile = json["TileList"][x]["IsAnimatedTile"];
        tile.TileUVOffset = vec2(tile.TileUVCellOffset.x * tileSet.TileUVSize.x, tile.TileUVCellOffset.y * tileSet.TileUVSize.y);
        tileSet.LevelTileList.emplace_back(tile);
    }
    LevelTileSetMap[tileSetId] = tileSet;
    return tileSetId;
}

VkGuid LevelSystem::LoadLevelLayout(const String& levelLayoutPath)
{
    if (levelLayoutPath.empty() ||
        levelLayoutPath == "")
    {
        return VkGuid();
    }

    nlohmann::json json = Json::ReadJson(levelLayoutPath);
    VkGuid levelLayoutId = VkGuid(json["LevelLayoutId"].get<String>().c_str());

    levelLayout.LevelLayoutId = VkGuid(json["LevelLayoutId"].get<String>().c_str());
    levelLayout.LevelBounds = ivec2(json["LevelBounds"][0], json["LevelBounds"][1]);
    levelLayout.TileSizeinPixels = ivec2(json["TileSizeInPixels"][0], json["TileSizeInPixels"][1]);
    for (int x = 0; x < json["LevelLayouts"].size(); x++)
    {
        Vector<uint> levelLayer;
        for (int y = 0; y < json["LevelLayouts"][x].size(); y++)
        {
            for (int z = 0; z < json["LevelLayouts"][x][y].size(); z++)
            {
                levelLayer.emplace_back(json["LevelLayouts"][x][y][z]);
            }
        }
        levelLayout.LevelMapList.emplace_back(levelLayer);
    }
}