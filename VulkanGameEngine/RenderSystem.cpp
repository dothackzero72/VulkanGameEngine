#include "renderSystem.h"
RenderSystem renderSystem = RenderSystem();

VkResult RenderSystem::CreateCommandBuffer()
{
    return Renderer_CreateCommandBuffers(cRenderer.Device, cRenderer.CommandPool, &CommandBuffer, 1);
}

const Vector<VkDescriptorBufferInfo> RenderSystem::GetVertexPropertiesBuffer()
{
    Vector<SpriteMesh> meshList;
    meshList.reserve(assetManager.MeshList.size());
    std::transform(assetManager.MeshList.begin(), assetManager.MeshList.end(),
        std::back_inserter(meshList),
        [](const auto& pair) { return pair.second; });


    Vector<VkDescriptorBufferInfo> vertexPropertiesBuffer;
    if (meshList.empty())
    {
        vertexPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
            {
                .buffer = VK_NULL_HANDLE,
                .offset = 0,
                .range = VK_WHOLE_SIZE
            });
    }
    else
    {
        for (auto& mesh : meshList)
        {
            // mesh->GetVertexBuffer(vertexPropertiesBuffer);
        }
    }

    return vertexPropertiesBuffer;
}

const Vector<VkDescriptorBufferInfo> RenderSystem::GetIndexPropertiesBuffer()
{
    Vector<SpriteMesh> meshList;
    meshList.reserve(assetManager.MeshList.size());
    std::transform(assetManager.MeshList.begin(), assetManager.MeshList.end(),
        std::back_inserter(meshList),
        [](const auto& pair) { return pair.second; });

    std::vector<VkDescriptorBufferInfo>	indexPropertiesBuffer;
    if (meshList.empty())
    {
        indexPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
            {
                .buffer = VK_NULL_HANDLE,
                .offset = 0,
                .range = VK_WHOLE_SIZE
            });
    }
    else
    {
        for (auto& mesh : meshList)
        {
            //   mesh->GetIndexBuffer(indexPropertiesBuffer);
        }
    }
    return indexPropertiesBuffer;
}

const Vector<VkDescriptorBufferInfo> RenderSystem::GetGameObjectTransformBuffer()
{
    Vector<SpriteMesh> meshList;
    meshList.reserve(assetManager.MeshList.size());
    std::transform(assetManager.MeshList.begin(), assetManager.MeshList.end(),
        std::back_inserter(meshList),
        [](const auto& pair) { return pair.second; });

    std::vector<VkDescriptorBufferInfo>	transformPropertiesBuffer;
    if (meshList.empty())
    {
        transformPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
            {
                .buffer = VK_NULL_HANDLE,
                .offset = 0,
                .range = VK_WHOLE_SIZE
            });
    }
    else
    {
        for (auto& mesh : meshList)
        {
            mesh.GetTransformBuffer();
        }
    }

    return transformPropertiesBuffer;
}

const Vector<VkDescriptorBufferInfo> RenderSystem::GetMeshPropertiesBuffer()
{
    Vector<SpriteMesh> meshList;
    meshList.reserve(assetManager.MeshList.size());
    std::transform(assetManager.MeshList.begin(), assetManager.MeshList.end(),
        std::back_inserter(meshList),
        [](const auto& pair) { return pair.second; });

    Vector<VkDescriptorBufferInfo> meshPropertiesBuffer;
    if (meshList.empty())
    {
        meshPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
            {
                .buffer = VK_NULL_HANDLE,
                .offset = 0,
                .range = VK_WHOLE_SIZE
            });
    }
    else
    {
        for (auto& mesh : meshList)
        {
            meshPropertiesBuffer.emplace_back(mesh.GetMeshPropertiesBuffer());
        }
    }

    return meshPropertiesBuffer;
}

const Vector<VkDescriptorImageInfo> RenderSystem::GetTexturePropertiesBuffer(Vector<SharedPtr<Texture>>& renderedTextureList)
{
    Vector<Texture> textureList;
    if (renderedTextureList.empty())
    {
        textureList.reserve(TextureList.size());
        std::transform(TextureList.begin(), TextureList.end(),
            std::back_inserter(textureList),
            [](const auto& pair) { return pair.second; });
    }
    else
    {
        for (auto& texture : renderedTextureList)
        {
            textureList.emplace_back(*texture.get());
        }
    }

    Vector<VkDescriptorImageInfo>	texturePropertiesBuffer;
    if (textureList.empty())
    {
        VkSamplerCreateInfo NullSamplerInfo =
        {
            .sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
            .magFilter = VK_FILTER_NEAREST,
            .minFilter = VK_FILTER_NEAREST,
            .mipmapMode = VK_SAMPLER_MIPMAP_MODE_LINEAR,
            .addressModeU = VK_SAMPLER_ADDRESS_MODE_REPEAT,
            .addressModeV = VK_SAMPLER_ADDRESS_MODE_REPEAT,
            .addressModeW = VK_SAMPLER_ADDRESS_MODE_REPEAT,
            .mipLodBias = 0,
            .anisotropyEnable = VK_TRUE,
            .maxAnisotropy = 16.0f,
            .compareEnable = VK_FALSE,
            .compareOp = VK_COMPARE_OP_ALWAYS,
            .minLod = 0,
            .maxLod = 0,
            .borderColor = VK_BORDER_COLOR_INT_OPAQUE_BLACK,
            .unnormalizedCoordinates = VK_FALSE,
        };

        VkSampler nullSampler = VK_NULL_HANDLE;
        if (vkCreateSampler(cRenderer.Device, &NullSamplerInfo, nullptr, &nullSampler))
        {
            throw std::runtime_error("Failed to create Sampler.");
        }

        VkDescriptorImageInfo nullBuffer =
        {
            .sampler = nullSampler,
            .imageView = VK_NULL_HANDLE,
            .imageLayout = VK_IMAGE_LAYOUT_UNDEFINED,
        };
        texturePropertiesBuffer.emplace_back(nullBuffer);
    }
    else
    {
        for (auto& texture : textureList)
        {
            texture.GetTexturePropertiesBuffer(texturePropertiesBuffer);
        }
    }

    return texturePropertiesBuffer;
}

const Vector<VkDescriptorBufferInfo> RenderSystem::GetMaterialPropertiesBuffer()
{
    Vector<Material> materialList;
    materialList.reserve(MaterialList.size());
    std::transform(MaterialList.begin(), MaterialList.end(),
        std::back_inserter(materialList),
        [](const auto& pair) { return pair.second; });

    std::vector<VkDescriptorBufferInfo>	materialPropertiesBuffer;
    if (materialList.empty())
    {
        materialPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
            {
                .buffer = VK_NULL_HANDLE,
                .offset = 0,
                .range = VK_WHOLE_SIZE
            });
    }
    else
    {
        for (auto& material : materialList)
        {
            material.GetMaterialPropertiesBuffer(materialPropertiesBuffer);
        }
    }
    return materialPropertiesBuffer;
}

void RenderSystem::UpdateBufferIndex()
{
    int xy = 0;
    for (auto& [id, texture] : renderSystem.TextureList) {
        texture.UpdateTextureBufferIndex(xy);
        ++xy;
    }
    int xz = 0;
    for (auto& [id, material] : renderSystem.MaterialList) {
        material.UpdateMaterialBufferIndex(xz);
        ++xz;
    }
}

VkGuid RenderSystem::AddSpriteVRAM(const String& spritePath)
{
    nlohmann::json json = Json::ReadJson(spritePath);
    VkGuid vramId = VkGuid(json["VramSpriteId"].get<String>().c_str());
    VkGuid materialId = VkGuid(json["MaterialId"].get<String>().c_str());

    const Material& material = MaterialList.at(materialId);
    const Texture& texture = TextureList.at(material.AlbedoMapId);
    SpriteVram sprite = SpriteVram
    {
        .VramSpriteID = vramId,
        .SpriteMaterialID = materialId,
        .SpriteLayer = json["SpriteLayer"],
        .SpriteColor = vec4{ json["SpriteColor"][0], json["SpriteColor"][1], json["SpriteColor"][2], json["SpriteColor"][3] },
        .SpritePixelSize = ivec2{ json["SpritePixelSize"][0], json["SpritePixelSize"][1] },
        .SpriteScale = vec2(5.0f),
        .SpriteCells = ivec2(texture.Width / sprite.SpritePixelSize.x, texture.Height / sprite.SpritePixelSize.y),
        .SpriteUVSize = vec2(1.0f / (float)sprite.SpriteCells.x, 1.0f / (float)sprite.SpriteCells.y),
        .SpriteSize = vec2(sprite.SpritePixelSize.x * sprite.SpriteScale.x, sprite.SpritePixelSize.y * sprite.SpriteScale.y),
    };

    VramSpriteList[vramId] = sprite;
    return vramId;
}

VkGuid RenderSystem::LoadTexture(const String& texturePath)
{
    if (texturePath.empty() ||
        texturePath == "")
    {
        return VkGuid();
    }

    nlohmann::json json = Json::ReadJson(texturePath);
    VkGuid textureId = VkGuid(json["TextureId"].get<String>().c_str());
    String textureFilePath = json["TextureFilePath"];
    VkFormat textureByteFormat = json["TextureByteFormat"];
    VkImageAspectFlags imageType = json["ImageType"];
    TextureTypeEnum textureType = json["TextureType"];
    bool useMipMaps = json["UseMipMaps"];

    TextureList[textureId] = Texture(textureId, textureFilePath, textureByteFormat, imageType, textureType, useMipMaps);
    return textureId;
}

VkGuid RenderSystem::LoadMaterial(const String& materialPath)
{
    nlohmann::json json = Json::ReadJson(materialPath);


    String name = json["Name"];
    VkGuid materialId = VkGuid(json["MaterialId"].get<String>().c_str());

    MaterialList[materialId] = Material(name, materialId);
    MaterialList[materialId].Albedo = vec3(json["Albedo"][0], json["Albedo"][1], json["Albedo"][2]);
    MaterialList[materialId].Metallic = json["Metallic"];
    MaterialList[materialId].Roughness = json["Roughness"];
    MaterialList[materialId].AmbientOcclusion = json["AmbientOcclusion"];
    MaterialList[materialId].Emission = vec3(json["Emission"][0], json["Emission"][1], json["Emission"][2]);
    MaterialList[materialId].Alpha = json["Alpha"];

    MaterialList[materialId].AlbedoMapId = LoadTexture(json["AlbedoMapPath"]);
    MaterialList[materialId].MetallicRoughnessMapId = LoadTexture(json["MetallicRoughnessMapPath"]);
    MaterialList[materialId].MetallicMapId = LoadTexture(json["MetallicMapPath"]);
    MaterialList[materialId].RoughnessMapId = LoadTexture(json["RoughnessMapPath"]);
    MaterialList[materialId].AmbientOcclusionMapId = LoadTexture(json["AmbientOcclusionMapPath"]);
    MaterialList[materialId].NormalMapId = LoadTexture(json["NormalMapPath"]);
    MaterialList[materialId].DepthMapId = LoadTexture(json["DepthMapPath"]);
    MaterialList[materialId].AlphaMapId = LoadTexture(json["AlphaMapPath"]);
    MaterialList[materialId].EmissionMapId = LoadTexture(json["EmissionMapPath"]);
    MaterialList[materialId].HeightMapId = LoadTexture(json["HeightMapPath"]);

    return materialId;
}

void RenderSystem::Destroy()
{
}
