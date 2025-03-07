#pragma once
#include <TextureCPP.h>
#include <ShaderCompiler.h>
#include "vertex.h"
#include "JsonPipeline.h"

class FrameBufferRenderPass : public JsonRenderPass
{
private:
public:
	FrameBufferRenderPass();
	FrameBufferRenderPass(const String& jsonPath, SharedPtr<Texture> texture);
	virtual ~FrameBufferRenderPass();

	VkCommandBuffer Draw();
};