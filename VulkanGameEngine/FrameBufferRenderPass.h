#pragma once
#include <Texture.h>
#include <ShaderCompiler.h>
#include "vertex.h"
#include "JsonPipeline.h"

class FrameBufferRenderPass : public JsonRenderPass
{
private:

public:
	FrameBufferRenderPass();
	FrameBufferRenderPass(const String& jsonPath, Texture& inputTexture, ivec2 renderPassResolution);
	virtual ~FrameBufferRenderPass();
};