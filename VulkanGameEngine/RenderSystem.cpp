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

const Vector<VkDescriptorImageInfo> RenderSystem::GetTexturePropertiesBuffer(Vector<Texture>& renderedTextureList)
{
    Vector<Texture> textureList;
    if (renderedTextureList.empty())
    {
        textureList.reserve(assetManager.TextureList.size());
        std::transform(assetManager.TextureList.begin(), assetManager.TextureList.end(),
            std::back_inserter(textureList),
            [](const auto& pair) { return pair.second; });
    }
    else
    {
        textureList = renderedTextureList;
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
    materialList.reserve(assetManager.MaterialList.size());
    std::transform(assetManager.MaterialList.begin(), assetManager.MaterialList.end(),
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