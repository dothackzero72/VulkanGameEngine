using GlmSharp;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class Renderpass
    {
        public ivec2 RenderPassResolution;
        public VkSampleCountFlagBits SampleCount;
        public VkRenderPass RenderPass { get; protected set; }
        public List<VkCommandBuffer> CommandBufferList { get; protected set; }
        public List<VkFramebuffer> FrameBufferList { get; protected set; }

        public VkDescriptorPool DescriptorPool { get; protected set; }
        public VkDescriptorSetLayout DescriptorSetLayout { get; protected set; }
        public VkDescriptorSet DescriptorSet { get; protected set; }
        public VkPipeline ShaderPipeline { get; protected set; }
        public VkPipelineLayout ShaderPipelineLayout { get; protected set; }
        public VkPipelineCache PipelineCache { get; protected set; }

        public Renderpass()
        {
            RenderPassResolution = new ivec2
            {
                x = (int)VulkanRenderer.SwapChainResolution.Width,
                y = (int)VulkanRenderer.SwapChainResolution.Height
            };
            SampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;

            CommandBufferList = new List<VkCommandBuffer>();
            for (int x = 0; x < VulkanRenderer.SwapChainImageCount; x++)
            {
                CommandBufferList.Add(new VkCommandBuffer());
            }

            FrameBufferList = new List<VkFramebuffer>();
            for (int x = 0; x < VulkanRenderer.SwapChainImageCount; x++)
            {
                FrameBufferList.Add(new VkFramebuffer());
            }

            VulkanRenderer.CreateCommandBuffers(CommandBufferList);
        }

        VkWriteDescriptorSet CreateTextureDescriptorSet(Texture texture, UInt32 bindingSlot)
        {
            return CreateTextureDescriptorSet(texture, bindingSlot, 1);
        }

        VkWriteDescriptorSet CreateTextureDescriptorSet(Texture texture, UInt32 bindingSlot, UInt32 descriptorCount)
        {
            return CreateTextureDescriptorSet(texture, bindingSlot, descriptorCount, 0);
        }

        VkWriteDescriptorSet CreateTextureDescriptorSet(Texture texture, UInt32 bindingSlot, UInt32 descriptorCount, UInt32 arrayElement)
        {
            //IntPtr textureBufferHandle = GCHandle.Alloc(texture.GetTextureBuffer(), GCHandleType.Pinned).AddrOfPinnedObject();
            //VkWriteDescriptorSet textureBuffer = new VkWriteDescriptorSet()
            //{
            //    sType = VkStructureType.VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
            //    pNext = null,
            //    dstSet = DescriptorSet,
            //    dstBinding = 0,
            //    dstArrayElement = 0,
            //    descriptorCount = 1,
            //    descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
            //    pImageInfo = textureBufferHandle
            //};
            return new VkWriteDescriptorSet();
        }

        VkWriteDescriptorSet CreateStorageDescriptorSet(Mesh mesh, UInt32 bindingSlot)
        {
            return CreateStorageDescriptorSet(mesh, bindingSlot, 0);
        }

        VkWriteDescriptorSet CreateStorageDescriptorSet(Mesh mesh, UInt32 bindingSlot, UInt32 arrayElement)
        {
            //IntPtr meshBufferPtr = GCHandle.Alloc(mesh.PropertiesBuffer.GetDescriptorbuffer(), GCHandleType.Pinned).AddrOfPinnedObject();
            //VkWriteDescriptorSet meshBufferBuffer = new VkWriteDescriptorSet()
            //{
            //    sType = VkStructureType.VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
            //    pNext = null,
            //    dstSet = DescriptorSet,
            //    dstBinding = 0,
            //    dstArrayElement = 0,
            //    descriptorCount = 1,
            //    descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
            //    pBufferInfo = meshBufferPtr
            //};
            return new VkWriteDescriptorSet();
        }

        public void Destroy()
        {
            //VulkanRenderer.DestroyRenderPass(RenderPassPtr);
            //VulkanRenderer.DestroyCommandBuffers(CommandBufferList);
            //VulkanRenderer.DestroyFrameBuffers(FrameBufferList);
        }
    }
}
