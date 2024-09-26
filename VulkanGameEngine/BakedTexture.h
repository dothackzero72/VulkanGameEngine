#pragma once
#include "Texture.h"
#include <fstream>

class BakedTexture : public Texture
{
private:
    void CreateTexture(const std::string& filePath);
    void CreateTexture(Pixel ClearPixel, VkFormat textureFormat);
    void CreateTexture(Pixel32 ClearPixel, VkFormat textureFormat);
    void CreateTextureImage();
    void CreateTextureView();
    void CreateTextureSampler();
public:
    BakedTexture();
    BakedTexture(const std::string& filePath, VkFormat textureByteFormat, TextureTypeEnum textureType);
    BakedTexture(const Pixel& ClearColor, const glm::ivec2& TextureResolution, VkFormat TextureFormat);
    BakedTexture(const Pixel32& ClearColor, const glm::ivec2& TextureResolution, VkFormat TextureFormat);
    ~BakedTexture();

    void RecreateRendererTexture(glm::vec2 TextureResolution);
};

