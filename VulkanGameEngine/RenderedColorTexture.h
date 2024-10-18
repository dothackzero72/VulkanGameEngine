#pragma once
#include "Texture.h"

class RenderedColorTexture : public Texture
{
private:
protected:
	void CreateImageTexture(const String& FilePath);
	virtual void CreateTextureSampler() override;
public:
	RenderedColorTexture();
	RenderedColorTexture(const String& filePath, VkFormat textureByteFormat, TextureTypeEnum textureType);
	RenderedColorTexture(glm::ivec2& textureResolution, VkFormat format);
	virtual ~RenderedColorTexture() override;
};

