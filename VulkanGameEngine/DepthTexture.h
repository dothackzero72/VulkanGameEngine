#pragma once
#include "Texture.h"

class DepthTexture : public Texture
{
private:
    VkResult CreateTextureView();

public:
    DepthTexture();
    DepthTexture(VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo);
    virtual ~DepthTexture();
    void RecreateRendererTexture(vec2 textureResolution);
};

