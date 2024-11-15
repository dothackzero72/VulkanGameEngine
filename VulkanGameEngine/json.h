#pragma once
#include <vulkan/vulkan_core.h>
#include <nlohmann/json.hpp>
#include <glm/glm.hpp>
#include "Typedef.h"
#include "File.h"

class Json : public nlohmann::json
{
public:
	static nlohmann::json ReadJson(const String filePath)
	{
		return File::ReadJsonFile(filePath);
	}

	static void to_json(nlohmann::json& json, String& string)
	{
		json = string;
	}
	static void from_json(nlohmann::json& json, String& string)
	{
		json.get_to(string);
	}
	static void to_json(nlohmann::json& json, std::vector<String>& string)
	{
		json = string;
	}
	static void from_json(nlohmann::json& json, std::vector<String>& string)
	{
		/*	for (auto& vec2 : numberList)
			{
				json[0].get_to(vec2.x);
				json[1].get_to(vec2.y);
			}*/
	}

	static void to_json(nlohmann::json& json, int& number)
	{
		json = number;
	}
	static void from_json(nlohmann::json& json, int& number)
	{
		json.get_to(number);
	}
	static void to_json(nlohmann::json& json, std::vector<int>& numberList)
	{
		json = numberList;
	}
	static void from_json(nlohmann::json& json, std::vector<int>& numberList)
	{
		/*	for (auto& vec2 : numberList)
			{
				json[0].get_to(vec2.x);
				json[1].get_to(vec2.y);
			}*/
	}


	static void to_json(nlohmann::json& json, uint32_t& number)
	{
		json = number;
	}
	static void from_json(nlohmann::json& json, uint32_t& number)
	{
		json.get_to(number);
	}
	static void to_json(nlohmann::json& json, std::vector<uint32_t>& numberList)
	{
		json = numberList;
	}
	static void from_json(nlohmann::json& json, std::vector<uint32_t>& numberList)
	{
		json.get_to(numberList);
	}

	static void to_json(nlohmann::json& json, float number)
	{
		json = number;
	}
	static void from_json(nlohmann::json& json, float number)
	{
		json.get_to(number);
	}

	static void to_json(nlohmann::json& json, std::vector<float>& numberList)
	{
		json = numberList;
	}
	static void from_json(nlohmann::json& json, std::vector<float>& numberList)
	{
		json.get_to(numberList);
	}

	static void to_json(nlohmann::json& json, glm::vec2& vec2)
	{
		json = { vec2.x, vec2.y };
	}
	static void from_json(nlohmann::json& json, glm::vec2& vec2)
	{
		json[0].get_to(vec2.x);
		json[1].get_to(vec2.y);
	}

	static void to_json(nlohmann::json& json, std::vector<glm::vec2>& vec2List)
	{
		for (auto& vec2 : vec2List)
		{
			json = { vec2.x, vec2.y };
		}
	}
	static void from_json(nlohmann::json& json, std::vector<glm::vec2>& vec2List)
	{
		for (auto& vec2 : vec2List)
		{
			json[0].get_to(vec2.x);
			json[1].get_to(vec2.y);
		}
	}

	static void to_json(nlohmann::json& json, glm::vec3& vec3)
	{
		json = { vec3.x, vec3.y, vec3.z };
	}
	static void from_json(nlohmann::json& json, glm::vec3& vec3)
	{
		json[0].get_to(vec3.x);
		json[1].get_to(vec3.y);
		json[2].get_to(vec3.z);
	}

	static void to_json(nlohmann::json& json, std::vector<glm::vec3>& vec3List)
	{
		for (auto& vec3 : vec3List)
		{
			json = { vec3.x, vec3.y, vec3.z };
		}
	}
	static void from_json(nlohmann::json& json, std::vector<glm::vec3>& vec3List)
	{
		for (auto& vec3 : vec3List)
		{
			json[0].get_to(vec3.x);
			json[1].get_to(vec3.y);
			json[2].get_to(vec3.z);
		}
	}

	static void to_json(nlohmann::json& json, glm::vec4& vec4)
	{
		json = { vec4.x, vec4.y, vec4.z, vec4.w };
	}
	static void from_json(nlohmann::json& json, glm::vec4& vec4)
	{
		json[0].get_to(vec4.x);
		json[1].get_to(vec4.y);
		json[2].get_to(vec4.z);
		json[3].get_to(vec4.w);
	}

	static void to_json(nlohmann::json& json, std::vector<glm::vec4>& vec4List)
	{
		for (auto& vec4 : vec4List)
		{
			json = { vec4.x, vec4.y, vec4.z, vec4.w };
		}
	}
	static void from_json(nlohmann::json& json, std::vector<glm::vec4>& vec4List)
	{
		for (auto& vec4 : vec4List)
		{
			json[0].get_to(vec4.x);
			json[1].get_to(vec4.y);
			json[2].get_to(vec4.z);
			json[3].get_to(vec4.w);
		}
	}

	static void to_json(nlohmann::json& json, glm::ivec2& ivec2)
	{
		json = { ivec2.x, ivec2.y };
	}
	static void from_json(nlohmann::json& json, glm::ivec2& ivec2)
	{
		json[0].get_to(ivec2.x);
		json[1].get_to(ivec2.y);
	}

	static void to_json(nlohmann::json& json, std::vector<glm::ivec2>& ivec2List)
	{
		for (auto& ivec2 : ivec2List)
		{
			json = { ivec2.x, ivec2.y };
		}
	}
	static void from_json(nlohmann::json& json, std::vector<glm::ivec2>& ivec2List)
	{
		for (auto& ivec2 : ivec2List)
		{
			json[0].get_to(ivec2.x);
			json[1].get_to(ivec2.y);
		}
	}

	static void to_json(nlohmann::json& json, glm::ivec3& ivec3)
	{
		json = { ivec3.x, ivec3.y, ivec3.z };
	}
	static void from_json(nlohmann::json& json, glm::ivec3& ivec3)
	{
		json[0].get_to(ivec3.x);
		json[1].get_to(ivec3.y);
		json[2].get_to(ivec3.z);
	}

	static void to_json(nlohmann::json& json, std::vector<glm::ivec3>& ivec3List)
	{
		for (auto& ivec3 : ivec3List)
		{
			json = { ivec3.x, ivec3.y, ivec3.z };
		}
	}
	static void from_json(nlohmann::json& json, std::vector<glm::ivec3>& ivec3List)
	{
		for (auto& ivec3 : ivec3List)
		{
			json[0].get_to(ivec3.x);
			json[1].get_to(ivec3.y);
			json[2].get_to(ivec3.z);
		}
	}

	static void to_json(nlohmann::json& json, glm::ivec4& ivec4)
	{
		json = { ivec4.x, ivec4.y, ivec4.z, ivec4.w };
	}
	static void from_json(nlohmann::json& json, glm::ivec4& ivec4)
	{
		json[0].get_to(ivec4.x);
		json[1].get_to(ivec4.y);
		json[2].get_to(ivec4.z);
		json[3].get_to(ivec4.w);
	}

	static void to_json(nlohmann::json& json, std::vector<glm::ivec4>& ivec4List)
	{
		for (auto& ivec4 : ivec4List)
		{
			json = { ivec4.x, ivec4.y, ivec4.z, ivec4.w };
		}
	}
	static void from_json(nlohmann::json& json, std::vector<glm::ivec4>& ivec4List)
	{
		for (auto& ivec4 : ivec4List)
		{
			json[0].get_to(ivec4.x);
			json[1].get_to(ivec4.y);
			json[2].get_to(ivec4.z);
			json[3].get_to(ivec4.w);
		}
	}

	static void to_json(nlohmann::json& json, glm::mat2& mat2)
	{
		json = { mat2[0][0], mat2[0][1],
				 mat2[1][0], mat2[1][1] };
	}
	static void from_json(nlohmann::json& json, glm::mat4& mat2)
	{
		json[0].get_to(mat2[0][0]);
		json[1].get_to(mat2[0][1]);

		json[4].get_to(mat2[1][0]);
		json[5].get_to(mat2[1][1]);
	}

	static void to_json(nlohmann::json& json, std::vector<glm::mat2>& mat2List)
	{
		for (auto& mat2 : mat2List)
		{
			json = { mat2[0][0], mat2[0][1],
					 mat2[1][0], mat2[1][1] };
		}
	}
	static void from_json(nlohmann::json& json, std::vector<glm::mat3>& mat2List)
	{
		for (auto& mat2 : mat2List)
		{
			json[0].get_to(mat2[0][0]);
			json[1].get_to(mat2[0][1]);

			json[4].get_to(mat2[1][0]);
			json[5].get_to(mat2[1][1]);
		}
	}

	static void to_json(nlohmann::json& json, glm::mat3& mat3)
	{
		json = { mat3[0][0], mat3[0][1], mat3[0][2],
				 mat3[1][0], mat3[1][1], mat3[1][2],
				 mat3[2][0], mat3[2][1], mat3[2][2] };
	}
	static void from_json(nlohmann::json& json, glm::mat3& mat3)
	{
		json[0].get_to(mat3[0][0]);
		json[1].get_to(mat3[0][1]);
		json[2].get_to(mat3[0][2]);

		json[4].get_to(mat3[1][0]);
		json[5].get_to(mat3[1][1]);
		json[6].get_to(mat3[1][2]);

		json[8].get_to(mat3[2][0]);
		json[9].get_to(mat3[2][1]);
		json[10].get_to(mat3[2][2]);
	}

	static void to_json(nlohmann::json& json, std::vector<glm::mat3>& mat3List)
	{
		for (auto& mat3 : mat3List)
		{
			json = { mat3[0][0], mat3[0][1], mat3[0][2],
					 mat3[1][0], mat3[1][1], mat3[1][2],
					 mat3[2][0], mat3[2][1], mat3[2][2] };
		}
	}
	//static void from_json(nlohmann::json& json, std::vector<glm::mat3>& mat3List)
	//{
	//	for (auto& mat3 : mat3List)
	//	{
	//		json[0].get_to(mat3[0][0]);
	//		json[1].get_to(mat3[0][1]);
	//		json[2].get_to(mat3[0][2]);

	//		json[4].get_to(mat3[1][0]);
	//		json[5].get_to(mat3[1][1]);
	//		json[6].get_to(mat3[1][2]);

	//		json[8].get_to(mat3[2][0]);
	//		json[9].get_to(mat3[2][1]);
	//		json[10].get_to(mat3[2][2]);
	//	}
	//}

	static void to_json(nlohmann::json& json, glm::mat4& mat4)
	{
		json = { mat4[0][0], mat4[0][1], mat4[0][2], mat4[0][3],
				 mat4[1][0], mat4[1][1], mat4[1][2], mat4[1][3],
				 mat4[2][0], mat4[2][1], mat4[2][2], mat4[2][3],
				 mat4[3][0], mat4[3][1],mat4[3][2], mat4[3][3] };
	}
	//static void from_json(nlohmann::json& json, glm::mat4& mat4)
	//{
	//	json[0].get_to(mat4[0][0]);
	//	json[1].get_to(mat4[0][1]);
	//	json[2].get_to(mat4[0][2]);
	//	json[3].get_to(mat4[0][3]);

	//	json[4].get_to(mat4[1][0]);
	//	json[5].get_to(mat4[1][1]);
	//	json[6].get_to(mat4[1][2]);
	//	json[7].get_to(mat4[1][3]);

	//	json[8].get_to(mat4[2][0]);
	//	json[9].get_to(mat4[2][1]);
	//	json[10].get_to(mat4[2][2]);
	//	json[11].get_to(mat4[2][3]);

	//	json[12].get_to(mat4[3][0]);
	//	json[13].get_to(mat4[3][1]);
	//	json[14].get_to(mat4[3][2]);
	//	json[15].get_to(mat4[3][3]);
	//}

	static void to_json(nlohmann::json& json, std::vector<glm::mat4>& mat4List)
	{
		for (auto& mat4 : mat4List)
		{
			json = { mat4[0][0], mat4[0][1], mat4[0][2], mat4[0][3],
					 mat4[1][0], mat4[1][1], mat4[1][2], mat4[1][3],
					 mat4[2][0], mat4[2][1], mat4[2][2], mat4[2][3],
					 mat4[3][0], mat4[3][1],mat4[3][2], mat4[3][3] };
		}
	}
	static void from_json(nlohmann::json& json, std::vector<glm::mat4> mat4List)
	{
		for (auto& mat4 : mat4List)
		{
			json[0].get_to(mat4[0][0]);
			json[1].get_to(mat4[0][1]);
			json[2].get_to(mat4[0][2]);
			json[3].get_to(mat4[0][3]);

			json[4].get_to(mat4[1][0]);
			json[5].get_to(mat4[1][1]);
			json[6].get_to(mat4[1][2]);
			json[7].get_to(mat4[1][3]);

			json[8].get_to(mat4[2][0]);
			json[9].get_to(mat4[2][1]);
			json[10].get_to(mat4[2][2]);
			json[11].get_to(mat4[2][3]);

			json[12].get_to(mat4[3][0]);
			json[13].get_to(mat4[3][1]);
			json[14].get_to(mat4[3][2]);
			json[15].get_to(mat4[3][3]);
		}
	}

	static VkAttachmentDescription LoadAttachmentDescription(nlohmann::json json)
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

	static VkImageCreateInfo LoadImageCreateInfo(nlohmann::json json, ivec2 textureResolution)
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

	static VkSamplerCreateInfo LoadVulkanSamplerCreateInfo(nlohmann::json json)
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

	static VkSubpassDependency LoadSubpassDependency(nlohmann::json json)
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

	static VkViewport LoadViewPort(nlohmann::json json)
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

	static VkOffset2D LoadOffset2D(nlohmann::json json)
	{
		return VkOffset2D
		{
			.x = json["x"],
			.y = json["y"]
		};
	}

	static VkExtent2D LoadExtent2D(nlohmann::json json)
	{
		return VkExtent2D
		{
			.width = json["width"],
			.height = json["height"]
		};
	}

	static VkRect2D LoadRect2D(nlohmann::json json)
	{
		return VkRect2D
		{
			.offset = Json::LoadOffset2D(json["offset"]),
			.extent = Json::LoadExtent2D(json["extent"])
		};
	}

	static VkPipelineColorBlendAttachmentState LoadPipelineColorBlendAttachmentState(nlohmann::json json)
	{
		return VkPipelineColorBlendAttachmentState
		{
			.blendEnable = json["blendEnable"]["Value"],
			.srcColorBlendFactor = json["srcColorBlendFactor"],
			.dstColorBlendFactor = json["dstColorBlendFactor"],
		    .colorBlendOp = json["colorBlendOp"],
		    .srcAlphaBlendFactor = json["srcAlphaBlendFactor"],
		    .dstAlphaBlendFactor = json["dstAlphaBlendFactor"],
		    .alphaBlendOp = json["alphaBlendOp"],
		    .colorWriteMask = json["colorWriteMask"]
		};
	}

	static VkPipelineColorBlendStateCreateInfo LoadPipelineColorBlendStateCreateInfo(nlohmann::json json)
	{
		return VkPipelineColorBlendStateCreateInfo
		{
			.sType = VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO,
			.pNext = nullptr,
			.flags = 0,
			//.logicOpEnable = json["logicOpEnable"]["Value"],
			//.logicOp = json["logicOp"],
			.attachmentCount = 0,
			.pAttachments = nullptr,
		/*	.blendConstants =
			{
				json["blendConstants"]["R"],
				json["blendConstants"]["G"],
				json["blendConstants"]["B"],
				json["blendConstants"]["A"],
			}*/
		};
	}

	static VkPipelineRasterizationStateCreateInfo LoadPipelineRasterizationStateCreateInfo(nlohmann::json json)
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

	static VkPipelineMultisampleStateCreateInfo LoadPipelineMultisampleStateCreateInfo(nlohmann::json json)
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

	static VkPipelineDepthStencilStateCreateInfo LoadPipelineDepthStencilStateCreateInfo(nlohmann::json json)
	{
		return VkPipelineDepthStencilStateCreateInfo
		{
			.sType = VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO,
			.pNext = nullptr,
			.flags = 0,
			.depthTestEnable = json["depthTestEnable"],
			.depthWriteEnable = json["depthWriteEnable"],
			.depthCompareOp = json["depthCompareOp"],
			.depthBoundsTestEnable = json["depthBoundsTestEnable"],
			.stencilTestEnable = json["stencilTestEnable"],
			.front = VkStencilOpState
			{
				.failOp = json["front"]["failOp"],
				.passOp = json["front"]["passOp"],
				.depthFailOp = json["front"]["depthFailOp"],
				.compareOp = json["front"]["compareOp"],
				.compareMask = json["front"]["compareMask"],
				.writeMask = json["front"]["writeMask"],
				.reference = json["front"]["reference"]
			},
			.back = VkStencilOpState
			{
				.failOp = json["back"]["failOp"],
				.passOp = json["back"]["passOp"],
				.depthFailOp = json["back"]["depthFailOp"],
				.compareOp = json["back"]["compareOp"],
				.compareMask = json["back"]["compareMask"],
				.writeMask = json["back"]["writeMask"],
				.reference = json["back"]["reference"]
			},
			.minDepthBounds = json["minDepthBounds"],
			.maxDepthBounds = json["maxDepthBounds"],
		};
	}

	static VkPipelineInputAssemblyStateCreateInfo LoadPipelineInputAssemblyStateCreateInfo(nlohmann::json json)
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
	
	static VkDescriptorSetLayoutBinding LoadLayoutBinding(nlohmann::json json)
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
};