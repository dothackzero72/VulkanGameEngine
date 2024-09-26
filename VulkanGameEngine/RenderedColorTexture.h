#pragma once
#include "Texture.h"

class RenderedColorTexture : public Texture
{
private:
protected:
	void CreateImageTexture(const std::string& FilePath);
	virtual void CreateTextureSampler() override;
public:
	RenderedColorTexture();
	RenderedColorTexture(const std::string& filePath, VkFormat textureByteFormat, TextureTypeEnum textureType);
	RenderedColorTexture(glm::ivec2& textureResolution, VkFormat format);
	virtual ~RenderedColorTexture() override;
};

