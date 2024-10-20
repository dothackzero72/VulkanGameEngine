#include "RenderPass.h"
extern "C" 
{
#include "CVulkanRenderer.h"
#include <VulkanError.h>
#include "RenderMesh2DComponent.h"
}

Renderpass::Renderpass()
{
	RenderPassResolution = glm::ivec2((int)cRenderer.SwapChain.SwapChainResolution.width, (int)cRenderer.SwapChain.SwapChainResolution.height);
	SampleCount = VK_SAMPLE_COUNT_1_BIT;

	CommandBufferList.resize(cRenderer.SwapChain.SwapChainImageCount);
	FrameBufferList.resize(cRenderer.SwapChain.SwapChainImageCount);

	VULKAN_RESULT(renderer.CreateCommandBuffers(CommandBufferList));
}

Renderpass::~Renderpass()
{
}

VkWriteDescriptorSet Renderpass::CreateTextureDescriptorSet(std::shared_ptr<Texture> texture, uint32 bindingSlot)
{
    return CreateTextureDescriptorSet(texture, bindingSlot, 1);
}

VkWriteDescriptorSet Renderpass::CreateTextureDescriptorSet(std::shared_ptr<Texture> texture, uint32 bindingSlot, uint32 descriptorCount)
{
    return CreateTextureDescriptorSet(texture, bindingSlot, descriptorCount, 0);
}

VkWriteDescriptorSet Renderpass::CreateTextureDescriptorSet(std::shared_ptr<Texture> texture, uint32 bindingSlot, uint32 descriptorCount, uint32 arrayElement)
{
    VkWriteDescriptorSet textureBuffer
    {
        .sType = VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
        .dstSet = DescriptorSet,
        .dstBinding = 1,
        .dstArrayElement = 0,
        .descriptorCount = 1,
        .descriptorType = VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
        .pImageInfo = texture->GetTextureBuffer()
    };
    return textureBuffer;
}

VkWriteDescriptorSet Renderpass::CreateStorageDescriptorSet(std::shared_ptr<GameObject> mesh, uint32 bindingSlot)
{
   return CreateStorageDescriptorSet(mesh, bindingSlot, 0);
}

VkWriteDescriptorSet Renderpass::CreateStorageDescriptorSet(std::shared_ptr<GameObject> mesh, uint32 bindingSlot, uint32 arrayElement)
{
    VkWriteDescriptorSet buffer
    {
        .sType = VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
        .dstSet = DescriptorSet,
        .dstBinding = 0,
        .dstArrayElement = 0,
        .descriptorCount = 1,
        .descriptorType = VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
        .pBufferInfo = static_cast<RenderMesh2DComponent*>(mesh->GameObjectComponentList[0].get())->GetMeshPropertiesBuffer()->GetDescriptorbuffer(),
    };
    return buffer;
}
//
//VkWriteDescriptorSet RenderPass::CreateUnimformDescriptorSet()
//{
//    return VK_NULL_HANDLE;
//}

VkCommandBuffer Renderpass::Draw()
{
	return VK_NULL_HANDLE;
}

void Renderpass::Destroy()
{
    renderer.DestroyRenderPass(RenderPass);
    renderer.DestroyCommandBuffers(CommandBufferList);
    renderer.DestroyFrameBuffers(FrameBufferList);
}