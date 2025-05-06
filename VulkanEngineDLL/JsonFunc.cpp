#include "JsonFunc.h"

 VkAttachmentDescription Json_LoadAttachmentDescription(nlohmann::json json)
{
	return VkAttachmentDescription
	{
		.flags = json["flags"],
		.format = json["format"],
		.samples = json["samples"],
		.loadOp = json["loadOp"],
		.storeOp = json["storeOp"],
		.stencilLoadOp = json["stencilLoadOp"],
		.stencilStoreOp = json["stencilStoreOp"],
		.initialLayout = json["initialLayout"],
		.finalLayout = json["finalLayout"]
	};
}

 VkImageCreateInfo Json_LoadImageCreateInfo(nlohmann::json json, ivec2 textureResolution)
{
	return VkImageCreateInfo
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
		.pNext = nullptr,
		.flags = 0,
		.imageType = json["imageType"],
		.format = json["format"],
		.extent = VkExtent3D{
			.width = static_cast<uint32>(textureResolution.x),
			.height = static_cast<uint32>(textureResolution.y),
			.depth = 1
		},
		.mipLevels = json["mipLevels"],
		.arrayLayers = json["arrayLayers"],
		.samples = json["samples"],
		.tiling = json["tiling"],
		.usage = json["usage"],
		.sharingMode = json["sharingMode"],
		.queueFamilyIndexCount = 0,
		.pQueueFamilyIndices = nullptr,
		.initialLayout = json["initialLayout"]
	};
}

 VkSamplerCreateInfo Json_LoadVulkanSamplerCreateInfo(nlohmann::json json)
{
	return VkSamplerCreateInfo
	{
		.sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
		.pNext = nullptr,
		.flags = 0,
		.magFilter = json["magFilter"],
		.minFilter = json["minFilter"],
		.mipmapMode = json["mipmapMode"],
		.addressModeU = json["addressModeU"],
		.addressModeV = json["addressModeV"],
		.addressModeW = json["addressModeW"],
		.mipLodBias = json["mipLodBias"],
		.anisotropyEnable = json["anisotropyEnable"],
		.maxAnisotropy = json["maxAnisotropy"],
		.compareEnable = json["compareEnable"],
		.compareOp = json["compareOp"],
		.minLod = json["minLod"],
		.maxLod = json["maxLod"],
		.borderColor = json["borderColor"],
		.unnormalizedCoordinates = json["unnormalizedCoordinates"]
	};
}

 VkClearValue Json_LoadClearValue(nlohmann::json json)
{
	VkClearValue clearValue = VkClearValue();

	clearValue.color.float32[0] = json["Color"]["Float32_0"];
	clearValue.color.float32[1] = json["Color"]["Float32_1"];
	clearValue.color.float32[2] = json["Color"]["Float32_2"];
	clearValue.color.float32[3] = json["Color"]["Float32_3"];

	clearValue.color.int32[0] = json["Color"]["Int32_0"];
	clearValue.color.int32[1] = json["Color"]["Int32_1"];
	clearValue.color.int32[2] = json["Color"]["Int32_2"];
	clearValue.color.int32[3] = json["Color"]["Int32_3"];

	clearValue.color.uint32[0] = json["Color"]["Uint32_0"];
	clearValue.color.uint32[1] = json["Color"]["Uint32_1"];
	clearValue.color.uint32[2] = json["Color"]["Uint32_2"];
	clearValue.color.uint32[3] = json["Color"]["Uint32_3"];

	clearValue.depthStencil.depth = json["DepthStencil"]["depth"];
	clearValue.depthStencil.stencil = json["DepthStencil"]["stencil"];

	return clearValue;
}

 VkSubpassDependency Json_LoadSubpassDependency(nlohmann::json json)
{
	return VkSubpassDependency
	{
		.srcSubpass = json["srcSubpass"],
		.dstSubpass = json["dstSubpass"],
		.srcStageMask = json["srcStageMask"],
		.dstStageMask = json["dstStageMask"],
		.srcAccessMask = json["srcAccessMask"],
		.dstAccessMask = json["dstAccessMask"],
		.dependencyFlags = json["dependencyFlags"]
	};
}

 VkViewport Json_LoadViewPort(nlohmann::json json)
{
	return VkViewport
	{
		.x = json["x"],
		.y = json["y"],
		.width = json["width"],
		.height = json["height"],
		.minDepth = json["minDepth"],
		.maxDepth = json["maxDepth"],
	};
}

 VkOffset2D Json_LoadOffset2D(nlohmann::json json)
{
	return VkOffset2D
	{
		.x = json["x"],
		.y = json["y"]
	};
}

 VkExtent2D Json_LoadExtent2D(nlohmann::json json)
{
	return VkExtent2D
	{
		.width = json["width"],
		.height = json["height"]
	};
}

 VkRect2D Json_LoadRect2D(nlohmann::json json)
{
	return VkRect2D
	{
		.offset = Json_LoadOffset2D(json["offset"]),
		.extent = Json_LoadExtent2D(json["extent"])
	};
}

 VkVertexInputBindingDescription Json_LoadVertexInputBindingDescription(nlohmann::json json)
{
	return VkVertexInputBindingDescription
	{
		.binding = json["binding"],
		.stride = json["stride"],
		.inputRate = json["inputRate"]
	};
}

 VkVertexInputAttributeDescription Json_LoadVertexInputAttributeDescription(nlohmann::json json)
{
	return VkVertexInputAttributeDescription
	{
		.location = json["location"],
		.binding = json["binding"],
		.format = json["format"],
		.offset = json["offset"]
	};
}

 VkPipelineColorBlendAttachmentState Json_LoadPipelineColorBlendAttachmentState(nlohmann::json json)
{
	return VkPipelineColorBlendAttachmentState
	{
		.blendEnable = json["blendEnable"],
		.srcColorBlendFactor = json["srcColorBlendFactor"],
		.dstColorBlendFactor = json["dstColorBlendFactor"],
		.colorBlendOp = json["colorBlendOp"],
		.srcAlphaBlendFactor = json["srcAlphaBlendFactor"],
		.dstAlphaBlendFactor = json["dstAlphaBlendFactor"],
		.alphaBlendOp = json["alphaBlendOp"],
		.colorWriteMask = json["colorWriteMask"]
	};
}

 VkPipelineColorBlendStateCreateInfo Json_LoadPipelineColorBlendStateCreateInfo(nlohmann::json json)
{
	VkPipelineColorBlendStateCreateInfo blendStateCreateInfo = VkPipelineColorBlendStateCreateInfo
	{
		.sType = VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO,
		.pNext = nullptr,
		.flags = 0,
		.logicOpEnable = VK_FALSE,
		.attachmentCount = 0,
		.pAttachments = nullptr,
	};

	//if (json["logicOpEnable"].get<bool>())
	//{
	//	blendStateCreateInfo.logicOpEnable = VK_TRUE;
	//	blendStateCreateInfo.logicOp = json["logicOp"].get<VkLogicOp>();
	//	/*blendStateCreateInfo.blendConstants =
	//	{
	//		json["blendConstants"]["R"].get<float>(),
	//		json["blendConstants"]["G"].get<float>(),
	//		json["blendConstants"]["B"].get<float>(),
	//		json["blendConstants"]["A"].get<float>(),
	//	}*/
	//}

	return blendStateCreateInfo;
}

 VkPipelineRasterizationStateCreateInfo Json_LoadPipelineRasterizationStateCreateInfo(nlohmann::json json)
{
	return VkPipelineRasterizationStateCreateInfo
	{
		.sType = VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO,
		.pNext = nullptr,
		.flags = 0,
		.depthClampEnable = json["depthClampEnable"],
		.rasterizerDiscardEnable = json["rasterizerDiscardEnable"],
		.polygonMode = json["polygonMode"],
		.cullMode = json["cullMode"],
		.frontFace = json["frontFace"],
		.depthBiasEnable = json["depthBiasEnable"],
		.depthBiasConstantFactor = json["depthBiasConstantFactor"],
		.depthBiasClamp = json["depthBiasClamp"],
		.depthBiasSlopeFactor = json["depthBiasSlopeFactor"],
		.lineWidth = json["lineWidth"],
	};
}

 VkPipelineMultisampleStateCreateInfo Json_LoadPipelineMultisampleStateCreateInfo(nlohmann::json json)
{
	return VkPipelineMultisampleStateCreateInfo
	{
		.sType = VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO,
		.pNext = nullptr,
		.flags = 0,
		.rasterizationSamples = json["rasterizationSamples"],
		.sampleShadingEnable = json["sampleShadingEnable"],
		.minSampleShading = json["minSampleShading"],
		.pSampleMask = nullptr,
		.alphaToCoverageEnable = json["alphaToCoverageEnable"],
		.alphaToOneEnable = json["alphaToOneEnable"]
	};
}

 VkPipelineDepthStencilStateCreateInfo Json_LoadPipelineDepthStencilStateCreateInfo(nlohmann::json json)
{
	VkPipelineDepthStencilStateCreateInfo depthStencilStateCreateInfo = VkPipelineDepthStencilStateCreateInfo
	{
		.sType = VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO,
		.pNext = nullptr,
		.flags = 0,
		.depthTestEnable = json["depthTestEnable"],
		.depthWriteEnable = json["depthWriteEnable"],
		.depthCompareOp = json["depthCompareOp"],
		.depthBoundsTestEnable = json["depthBoundsTestEnable"],
		.stencilTestEnable = json["stencilTestEnable"],
		.minDepthBounds = json["minDepthBounds"],
		.maxDepthBounds = json["maxDepthBounds"],
	};

	if (depthStencilStateCreateInfo.stencilTestEnable)
	{
		depthStencilStateCreateInfo.front = VkStencilOpState
		{
			.failOp = json["front"]["failOp"],
			.passOp = json["front"]["passOp"],
			.depthFailOp = json["front"]["depthFailOp"],
			.compareOp = json["front"]["compareOp"],
			.compareMask = json["front"]["compareMask"],
			.writeMask = json["front"]["writeMask"],
			.reference = json["front"]["reference"]
		};
		depthStencilStateCreateInfo.back = VkStencilOpState
		{
			.failOp = json["back"]["failOp"],
			.passOp = json["back"]["passOp"],
			.depthFailOp = json["back"]["depthFailOp"],
			.compareOp = json["back"]["compareOp"],
			.compareMask = json["back"]["compareMask"],
			.writeMask = json["back"]["writeMask"],
			.reference = json["back"]["reference"]
		};
	}

	return depthStencilStateCreateInfo;
}
 
 VkPipelineInputAssemblyStateCreateInfo Json_LoadPipelineInputAssemblyStateCreateInfo(nlohmann::json json)
 {
	 return VkPipelineInputAssemblyStateCreateInfo
	 {
		 .sType = VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO,
		 .pNext = nullptr,
		 .flags = 0,
		 .topology = json["topology"],
		 .primitiveRestartEnable = json["primitiveRestartEnable"]
	 };
 }

 VkDescriptorSetLayoutBinding Json_LoadLayoutBinding(nlohmann::json json)
 {
	 return VkDescriptorSetLayoutBinding
	 {
		 .binding = json["binding"],
		 .descriptorType = json["descriptorType"],
		 .descriptorCount = json["descriptorCount"],
		 .stageFlags = json["stageFlags"],
		 .pImmutableSamplers = nullptr
	 };
 }