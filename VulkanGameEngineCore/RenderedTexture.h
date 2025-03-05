#pragma once
#include "TextureCPP.h"
#include <fstream>
#include "BakedTexture.h"
#include "VulkanRenderer.h"

enum BakeTextureFormat
{
    Bake_BMP,
    Bake_JPG,
    Bake_PNG,
    Bake_TGA
};

class RenderedTexture : public Texture
{
private:
    virtual VkResult CreateTextureView() override;

public:
    RenderedTexture();
    RenderedTexture(glm::ivec2 TextureResolution);
    RenderedTexture(glm::ivec2 TextureResolution, VkSampleCountFlagBits sampleCount);
    RenderedTexture(glm::ivec2 TextureResolution, VkSampleCountFlagBits sampleCount, VkFormat format);
    RenderedTexture(VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo);
    ~RenderedTexture();
    void RecreateRendererTexture(glm::vec2 TextureResolution);

    SharedPtr<BakedTexture> BakeColorTexture(const char* filename, BakeTextureFormat textureFormat);
};

std::vector<byte> ExportColorTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, const char* filename, SharedPtr<Texture> texture, BakeTextureFormat textureFormat, uint32 channels);