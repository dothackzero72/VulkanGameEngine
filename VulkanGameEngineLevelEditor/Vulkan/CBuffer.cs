using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public static unsafe class CBuffer
    {
        private static Vk vk = Vk.GetApi();
        public static VkResult AllocateMemory(VkBuffer buffer, ref VkDeviceMemory bufferMemory, VkMemoryPropertyFlagBits properties)
        {
            bufferMemory = new VkDeviceMemory();
            VkFunc.vkGetBufferMemoryRequirements(VulkanRenderer.device, buffer, out VkMemoryRequirements memRequirements);

            VkMemoryAllocateFlagsInfo extendedAllocFlagsInfo = new VkMemoryAllocateFlagsInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_FLAGS_INFO
            };

            VkMemoryAllocateInfo allocInfo = new VkMemoryAllocateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
                allocationSize = memRequirements.size,
                memoryTypeIndex = VulkanRenderer.GetMemoryType(memRequirements.memoryTypeBits, properties),
                pNext = &extendedAllocFlagsInfo
            };


            VkFunc.vkAllocateMemory(VulkanRenderer.device, &allocInfo, null, out bufferMemory);

            return VkResult.VK_SUCCESS;
        }

        public static unsafe void* MapBufferMemory(VkDeviceMemory bufferMemory, ulong bufferSize, bool* isMapped)
        {
            if (*isMapped)
            {
                throw new InvalidOperationException("Buffer already mapped!");
            }

            void* mappedData;
            VkResult result = VkFunc.vkMapMemory(VulkanRenderer.device, bufferMemory, 0, bufferSize, 0, &mappedData);
            if (result != VkResult.VK_SUCCESS)
            {
                return null;
            }

            *isMapped = true;
            return mappedData;
        }

        public static Result CUnmapBufferMemory(VkDeviceMemory bufferMemory, bool* isMapped)
        {
            if (*isMapped)
            {
                VkFunc.vkUnmapMemory(VulkanRenderer.device, bufferMemory);
                *isMapped = false;
            }
            return Result.Success;
        }

        public static VkResult CreateBuffer(out VkBuffer buffer, out VkDeviceMemory bufferMemory, void* bufferData, ulong bufferSize, VkBufferUsageFlagBits bufferUsage, VkMemoryPropertyFlagBits properties)
        {
            if (bufferData == null || bufferSize == 0)
            {
                throw new InvalidOperationException("Buffer Data and Size can't be NULL");
            }

            bufferMemory = new VkDeviceMemory();

            VkBufferCreateInfo bufferInfo = new VkBufferCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
                size = bufferSize,
                usage = bufferUsage,
                sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE
            };
            VkResult result = VkFunc.vkCreateBuffer(VulkanRenderer.device, &bufferInfo, null, out VkBuffer bufferPtr);
            buffer = bufferPtr;

            result = AllocateMemory(buffer, ref bufferMemory, properties);
            if (result != VkResult.VK_SUCCESS)
            {
                return result;
            }

            result = VkFunc.vkBindBufferMemory(VulkanRenderer.device, buffer, bufferMemory, 0);
            if (result != VkResult.VK_SUCCESS)
            {
                return result;
            }

            void* mappedData;
            result = VkFunc.vkMapMemory(VulkanRenderer.device, bufferMemory, 0, bufferSize, 0, &mappedData);
            if (result != VkResult.VK_SUCCESS)
            {
                return result;
            }

            int intCount = (int)bufferSize / 4;
            int[] mappedDataArray = new int[intCount];
            Marshal.Copy((IntPtr)bufferData, mappedDataArray, 0, intCount);
            VkFunc.vkUnmapMemory(VulkanRenderer.device, bufferMemory);

            return VkResult.VK_SUCCESS;
        }

        public static VkResult CreateStagingBuffer(out VkBuffer stagingBuffer, out VkDeviceMemory stagingBufferMemory, IntPtr bufferData, ulong bufferSize, VkBufferUsageFlagBits bufferUsage, VkMemoryPropertyFlagBits properties)
        {
            stagingBufferMemory = new VkDeviceMemory();
            var imageInfo = new VkBufferCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
                size = bufferSize,
                usage = VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
                sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE
            };
            VkFunc.vkCreateBuffer(VulkanRenderer.device, &imageInfo, null, out stagingBuffer);

            VkFunc.vkGetBufferMemoryRequirements(VulkanRenderer.device, stagingBuffer, out VkMemoryRequirements memRequirements);

            AllocateMemory(stagingBuffer, ref stagingBufferMemory, properties);
            return VkFunc.vkBindBufferMemory(VulkanRenderer.device, stagingBuffer, stagingBufferMemory, 0);
        }

        public static VkResult CopyBuffer(VkBuffer srcBuffer, VkBuffer dstBuffer, ulong size)
        {
            VkBufferCopy copyRegion = new VkBufferCopy
            {
                srcOffset = 0,
                dstOffset = 0,
                size = size
            };

            VkCommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
            VkFunc.vkCmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, 1, &copyRegion);
            return VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
        }

        public static Result CopyStagingBuffer(VkCommandBuffer commandBuffer, VkBuffer srcBuffer, VkBuffer dstBuffer, ulong size)
        {
            VkBufferCopy copyRegion = new VkBufferCopy
            {
                srcOffset = 0,
                dstOffset = 0,
                size = size
            };

            VkFunc.vkCmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, 1, &copyRegion);
            return Result.Success;
        }

        public static VkResult UpdateBufferSize(VkBuffer buffer, VkDeviceMemory bufferMemory, void* bufferData, ref ulong oldBufferSize, ulong newBufferSize, VkBufferUsageFlagBits bufferUsageFlags, VkMemoryPropertyFlagBits propertyFlags)
        {
            if (newBufferSize < oldBufferSize)
            {
                throw new InvalidOperationException("New buffer size can't be less than the old buffer size.");
            }

            oldBufferSize = newBufferSize;

            VkBufferCreateInfo bufferCreateInfo = new VkBufferCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
               size = newBufferSize,
                usage = bufferUsageFlags,
                sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE
            };

            VkResult result = VkFunc.vkCreateBuffer(VulkanRenderer.device, &bufferCreateInfo, null, out VkBuffer tempbuffer);
            buffer = tempbuffer;
            if (result != VkResult.VK_SUCCESS)
            {
                return result;
            }

            result = AllocateMemory(buffer, ref bufferMemory, propertyFlags);
            if (result != VkResult.VK_SUCCESS)
            {
                return result;
            }

            result = VkFunc.vkBindBufferMemory(VulkanRenderer.device, buffer, bufferMemory, 0);
            if (result != VkResult.VK_SUCCESS)
            {
                return result;
            }

            return VkFunc.vkMapMemory(VulkanRenderer.device, bufferMemory, 0, newBufferSize, 0, &bufferData);
        }

        public static VkResult UpdateBufferMemory(VkDeviceMemory bufferMemory, void* dataToCopy, ulong bufferSize)
        {
            if (dataToCopy == null || bufferSize == 0)
            {
                throw new InvalidOperationException("Buffer Data and Size can't be NULL");
            }

            void* mappedData;
            VkResult result = VkFunc.vkMapMemory(VulkanRenderer.device, bufferMemory, 0, bufferSize, 0, &mappedData);
            if (result != VkResult.VK_SUCCESS)
            {
                return result;
            }

            int intCount = (int)bufferSize / 4;
            int[] mappedDataArray = new int[intCount];
            Marshal.Copy((IntPtr)dataToCopy, mappedDataArray, 0, intCount);
            VkFunc.vkUnmapMemory(VulkanRenderer.device, bufferMemory);
            return VkResult.VK_SUCCESS;
        }


        public static VkResult BuffUpdateStagingBufferMemory(VkDeviceMemory bufferMemory, void* dataToCopy, ulong bufferSize)
        {
            if (dataToCopy == null || bufferSize == 0)
            {
                throw new InvalidOperationException("Buffer Data and Size can't be NULL");
            }

            void* mappedData;
            VkResult result = VkFunc.vkMapMemory(VulkanRenderer.device, bufferMemory, 0, bufferSize, 0, &mappedData);
            if (result != VkResult.VK_SUCCESS)
            {
                return result;
            }

            int intCount = (int)bufferSize / 4;
            int[] mappedDataArray = new int[intCount];
            Marshal.Copy((IntPtr)dataToCopy, mappedDataArray, 0, intCount);
            VkFunc.vkUnmapMemory(VulkanRenderer.device, bufferMemory);
            return VkResult.VK_SUCCESS;
        }
    }
}
