#pragma once
#include "JsonRenderPass.h"
#include "SpriteBatchLayer.h"
#include "DepthTexture.h"
#include "RenderedTexture.h"

class Level2DRenderer
{
private:

		 void BuildRenderPass(const RenderPassBuildInfoModel& renderPassBuildInfo);
	 void BuildFrameBuffer(const RenderPassBuildInfoModel& renderPassBuildInfo);
	Vector<SpriteMesh> GetMeshFromGameObjects();

public:
	uint RenderPassId = 0;
	String Name;
	ivec2 RenderPassResolution;
	VkSampleCountFlagBits SampleCount;

	VkRenderPass RenderPass = VK_NULL_HANDLE;
	VkCommandBuffer CommandBuffer;
	std::vector<VkFramebuffer> FrameBufferList;
	Vector<VkClearValue> ClearValueList;
	VkRenderPassBeginInfo RenderPassInfo;

	Vector<JsonPipeline> JsonPipelineList;
	Vector<Texture> InputTextureList;

	Vector<RenderedTexture> RenderedColorTextureList = Vector<RenderedTexture>();
	SharedPtr<DepthTexture> depthTexture;

	Level2DRenderer();
	Level2DRenderer(const String& JsonPath, ivec2 RenderPassResolution);
	 ~Level2DRenderer();

	void StartLevelRenderer();
	 void Update(const float& deltaTime);
	VkCommandBuffer DrawSprites(Vector<SpriteBatchLayer> meshList, SceneDataBuffer& sceneDataBuffer);
	void UpdateBufferIndex();
	 void Destroy();
};