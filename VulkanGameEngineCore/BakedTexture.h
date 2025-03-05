#pragma once
#include "TextureCPP.h"
#include <fstream>

class BakedTexture : public Texture
{
private:
    void CreateTexture(const String& filePath);
    void CreateTexture(Pixel ClearPixel, VkFormat textureFormat);
    void CreateTexture(Pixel32 ClearPixel, VkFormat textureFormat);
    void CreateTextureImage();
    VkResult CreateTextureView();
    void CreateTextureSampler();
public:
    BakedTexture();
    BakedTexture(const String& filePath, VkFormat textureByteFormat, TextureTypeEnum textureType);
    BakedTexture(const Pixel& ClearColor, const glm::ivec2& TextureResolution, VkFormat TextureFormat);
    BakedTexture(const Pixel32& ClearColor, const glm::ivec2& TextureResolution, VkFormat TextureFormat);
    ~BakedTexture();

    void RecreateRendererTexture(glm::vec2 TextureResolution);
};

