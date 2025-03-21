#pragma once
#include "Texture.h"
#include <fstream>
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

public:
    RenderedTexture();
    RenderedTexture(VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo);
    ~RenderedTexture();

    void RecreateRendererTexture(glm::vec2 TextureResolution);
};
