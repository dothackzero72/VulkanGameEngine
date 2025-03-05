#pragma once
#include "TextureCPP.h"

class DepthTexture : public Texture
{
private:
    VkResult CreateTextureView();

public:
    DepthTexture();
    DepthTexture(ivec2 TextureResolution);
    DepthTexture(VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo);
    virtual ~DepthTexture();
    void RecreateRendererTexture(vec2 textureResolution);
};

