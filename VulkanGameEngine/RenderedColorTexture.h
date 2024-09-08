#pragma once
#include "Texture.h"

class RenderedColorTexture : public Texture
{
private:
protected:
	virtual void CreateTextureSampler() override;
public:
	RenderedColorTexture();
	RenderedColorTexture(glm::ivec2& textureResolution, VkFormat format);
	virtual ~RenderedColorTexture() override;
};

