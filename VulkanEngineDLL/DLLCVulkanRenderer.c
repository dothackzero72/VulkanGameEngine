#include "DLLCVulkanRenderer.h"

	VkResult DLL_CreateDebugUtilsMessengerEXT(VkInstance instance, const VkDebugUtilsMessengerCreateInfoEXT* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDebugUtilsMessengerEXT* pDebugMessenger)
	{
		return  DLL_CreateDebugUtilsMessengerEXT(instance, pCreateInfo, pAllocator, pDebugMessenger);
	}

	void DLL_DestroyDebugUtilsMessengerEXT(VkInstance instance, const VkAllocationCallbacks* pAllocator)
	{
		return  DLL_DestroyDebugUtilsMessengerEXT(instance, pAllocator);
	}

	VKAPI_ATTR VkBool32 VKAPI_CALL DLL_Vulkan_DebugCallBack(VkDebugUtilsMessageSeverityFlagBitsEXT MessageSeverity, VkDebugUtilsMessageTypeFlagsEXT MessageType, const VkDebugUtilsMessengerCallbackDataEXT* CallBackData, void* UserData)
	{
		return  DLL_Vulkan_DebugCallBack(MessageSeverity, MessageType, CallBackData, UserData);
	}

	VkResult DLL_Renderer_GetWin32Extensions(uint32_t* extensionCount, char*** enabledExtensions)
	{
		return DLL_Renderer_GetWin32Extensions(extensionCount, enabledExtensions);
	}

	VkResult DLL_Renderer_CreateCommandBuffers(VkDevice device, VkCommandPool commandPool, VkCommandBuffer* commandBufferList, uint32 commandBufferCount)
	{
		return  DLL_Renderer_CreateCommandBuffers(device, commandPool, commandBufferList, commandBufferCount);
	}

	VkResult DLL_Renderer_CreateFrameBuffer(VkDevice device, VkFramebuffer* pFrameBuffer, VkFramebufferCreateInfo* frameBufferCreateInfo)
	{
		return  DLL_Renderer_CreateFrameBuffer(device, pFrameBuffer, frameBufferCreateInfo);
	}

	VkResult DLL_Renderer_CreateRenderPass(VkDevice device, RenderPassCreateInfoStruct* renderPassCreateInfo)
	{
		return  DLL_Renderer_CreateRenderPass(device, renderPassCreateInfo);
	}

	VkResult DLL_Renderer_CreateDescriptorPool(VkDevice device, VkDescriptorPool* descriptorPool, VkDescriptorPoolCreateInfo* descriptorPoolCreateInfo)
	{
		return DLL_Renderer_CreateDescriptorPool(device, descriptorPool, descriptorPoolCreateInfo);
	}

	VkResult DLL_Renderer_CreateDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayout, VkDescriptorSetLayoutCreateInfo* descriptorSetLayoutCreateInfo)
	{
		return  DLL_Renderer_CreateDescriptorSetLayout(device, descriptorSetLayout, descriptorSetLayoutCreateInfo);
	}

	VkResult DLL_Renderer_CreatePipelineLayout(VkDevice device, VkPipelineLayout* pipelineLayout, VkPipelineLayoutCreateInfo* pipelineLayoutCreateInfo)
	{
		return  DLL_Renderer_CreatePipelineLayout(device, pipelineLayout, pipelineLayoutCreateInfo);
	}

	VkResult DLL_Renderer_AllocateDescriptorSets(VkDevice device, VkDescriptorSet* descriptorSet, VkDescriptorSetAllocateInfo* descriptorSetAllocateInfo)
	{
		return  DLL_Renderer_AllocateDescriptorSets(device, descriptorSet, descriptorSetAllocateInfo);
	}

	VkResult DLL_Renderer_AllocateCommandBuffers(VkDevice device, VkCommandBuffer* commandBuffer, VkCommandBufferAllocateInfo* commandBufferAllocateInfo)
	{
		return  DLL_Renderer_AllocateCommandBuffers(device, commandBuffer, commandBufferAllocateInfo);
	}

	VkResult DLL_Renderer_CreateGraphicsPipelines(VkDevice device, VkPipeline* graphicPipeline, VkGraphicsPipelineCreateInfo* createGraphicPipelines, uint32 createGraphicPipelinesCount)
	{
		return  DLL_Renderer_CreateGraphicsPipelines(device, graphicPipeline, createGraphicPipelines, createGraphicPipelinesCount);
	}

	VkResult DLL_Renderer_CreateCommandPool(VkDevice device, VkCommandPool* commandPool, VkCommandPoolCreateInfo* commandPoolInfo)
	{
		return DLL_Renderer_CreateCommandPool(device, commandPool, commandPoolInfo);
	}

	void DLL_Renderer_UpdateDescriptorSet(VkDevice device, VkWriteDescriptorSet* writeDescriptorSet, uint32 count)
	{
		return  DLL_Renderer_UpdateDescriptorSet(device, writeDescriptorSet, count);
	}

	VkResult DLL_Renderer_StartFrame(VkDevice device, VkSwapchainKHR swapChain, VkFence* fenceList, VkSemaphore* acquireImageSemaphoreList, uint32_t* pImageIndex, uint32_t* pCommandIndex, bool* pRebuildRendererFlag)
	{
		return  DLL_Renderer_StartFrame(device, swapChain, fenceList, acquireImageSemaphoreList, pImageIndex, pCommandIndex, pRebuildRendererFlag);
	}

	VkResult DLL_Renderer_EndFrame(VkSwapchainKHR swapChain, VkSemaphore* acquireImageSemaphoreList, VkSemaphore* presentImageSemaphoreList, VkFence* fenceList, VkQueue graphicsQueue, VkQueue presentQueue, uint32_t commandIndex, uint32_t imageIndex, VkCommandBuffer* pCommandBufferSubmitList, uint32_t commandBufferCount, bool* rebuildRendererFlag)
	{
		return  DLL_Renderer_EndFrame(swapChain, acquireImageSemaphoreList, presentImageSemaphoreList, fenceList, graphicsQueue, presentQueue, commandIndex, imageIndex, pCommandBufferSubmitList, commandBufferCount, rebuildRendererFlag);
	}

	uint32 DLL_Renderer_GetMemoryType(VkPhysicalDevice physicalDevice, uint32_t typeFilter, VkMemoryPropertyFlags properties)
	{
		return  DLL_Renderer_GetMemoryType(physicalDevice, typeFilter, properties);
	}

	VkResult DLL_Renderer_BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo)
	{
		return  DLL_Renderer_BeginCommandBuffer(pCommandBufferList, commandBufferBeginInfo);
	}

	VkResult DLL_Renderer_EndCommandBuffer(VkCommandBuffer* pCommandBufferList)
	{
		return  DLL_Renderer_EndCommandBuffer(pCommandBufferList);
	}

	VkCommandBuffer DLL_Renderer_BeginSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool)
	{
		return  DLL_Renderer_BeginSingleUseCommandBuffer(device, commandPool);
	}

	VkResult DLL_Renderer_EndSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkCommandBuffer commandBuffer)
	{
		return  DLL_Renderer_EndSingleUseCommandBuffer(device, commandPool, graphicsQueue, commandBuffer);
	}

	void DLL_Renderer_DestroyFences(VkDevice device, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, VkFence* fences, size_t semaphoreCount)
	{
		return  DLL_Renderer_DestroyFences(device, acquireImageSemaphores, presentImageSemaphores, fences, semaphoreCount);
	}

	void DLL_Renderer_DestroyCommandPool(VkDevice device, VkCommandPool* commandPool)
	{
		return  DLL_Renderer_DestroyCommandPool(device, commandPool);
	}

	void DLL_Renderer_DestroyDevice(VkDevice device)
	{
		return  DLL_Renderer_DestroyDevice(device);
	}

	void DLL_Renderer_DestroySurface(VkInstance instance, VkSurfaceKHR* surface)
	{
		return  DLL_Renderer_DestroySurface(instance, surface);
	}

	void DLL_Renderer_DestroyDebugger(VkInstance* instance, VkDebugUtilsMessengerEXT debugUtilsMessengerEXT)
	{
		return  DLL_Renderer_DestroyDebugger(instance, debugUtilsMessengerEXT);
	}

	void DLL_Renderer_DestroyInstance(VkInstance* instance)
	{
		return  DLL_Renderer_DestroyInstance(instance);
	}

	void DLL_Renderer_DestroyRenderPass(VkDevice device, VkRenderPass* renderPass)
	{
		return  DLL_Renderer_DestroyRenderPass(device, renderPass);
	}

	void DLL_Renderer_DestroyFrameBuffers(VkDevice device, VkFramebuffer* frameBufferList, uint32 count)
	{
		return  DLL_Renderer_DestroyFrameBuffers(device, frameBufferList, count);
	}

	void DLL_Renderer_DestroyDescriptorPool(VkDevice device, VkDescriptorPool* descriptorPool)
	{
		return DLL_Renderer_DestroyDescriptorPool(device, descriptorPool);
	}

	void DLL_Renderer_DestroyDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayout)
	{
		return  DLL_Renderer_DestroyDescriptorSetLayout(device, descriptorSetLayout);
	}

	void DLL_Renderer_DestroyCommandBuffers(VkDevice device, VkCommandPool* commandPool, VkCommandBuffer* commandBufferList, uint32 count)
	{
		return  DLL_Renderer_DestroyCommandBuffers(device, commandPool, commandBufferList, count);
	}

	void DLL_Renderer_DestroyBuffer(VkDevice device, VkBuffer* buffer)
	{
		return  DLL_Renderer_DestroyBuffer(device, buffer);
	}

	void DLL_Renderer_FreeDeviceMemory(VkDevice device, VkDeviceMemory* memory)
	{
		return  DLL_Renderer_FreeDeviceMemory(device, memory);
	}

	void DLL_Renderer_DestroySwapChainImageView(VkDevice device, VkSurfaceKHR surface, VkImageView* pSwapChainImageViewList, uint32 count)
	{
		return  DLL_Renderer_DestroySwapChainImageView(device, surface, pSwapChainImageViewList, count);
	}

	void DLL_Renderer_DestroySwapChain(VkDevice device, VkSwapchainKHR* swapChain)
	{
		return  DLL_Renderer_DestroySwapChain(device, swapChain);
	}

	void DLL_Renderer_DestroyImageView(VkDevice device, VkImageView* imageView)
	{
		return  DLL_Renderer_DestroyImageView(device, imageView);
	}

	void DLL_Renderer_DestroyImage(VkDevice device, VkImage* image)
	{
		return  DLL_Renderer_DestroyImage(device, image);
	}

	void DLL_Renderer_DestroySampler(VkDevice device, VkSampler* sampler)
	{
		return  DLL_Renderer_DestroySampler(device, sampler);
	}

	void DLL_Renderer_DestroyPipeline(VkDevice device, VkPipeline* pipeline)
	{
		return  DLL_Renderer_DestroyPipeline(device, pipeline);
	}

	void DLL_Renderer_DestroyPipelineLayout(VkDevice device, VkPipelineLayout* pipelineLayout)
	{
		return  DLL_Renderer_DestroyPipelineLayout(device, pipelineLayout);
	}

	void DLL_Renderer_DestroyPipelineCache(VkDevice device, VkPipelineCache* pipelineCache)
	{
		return  DLL_Renderer_DestroyPipelineCache(device, pipelineCache);
	}