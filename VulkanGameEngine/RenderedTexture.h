#pragma once
#include "Texture.h"
#include <fstream>
#include "VulkanRenderer.h"

class RenderedTexture : public Texture
{
private:

public:
    RenderedTexture();
    RenderedTexture(VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo);
    ~RenderedTexture();

    void RecreateRendererTexture(VkImageAspectFlags imageType, vec2 TextureResolution);
};
