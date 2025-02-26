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
        public static Result AllocateMemory(Silk.NET.Vulkan.Buffer buffer, out DeviceMemory bufferMemory, MemoryPropertyFlags properties)
        {
            bufferMemory = new DeviceMemory();
            VkFunc.vkGetBufferMemoryRequirements(VulkanRenderer.device, buffer, out MemoryRequirements memRequirements);

            MemoryAllocateFlagsInfoKHR extendedAllocFlagsInfo = new MemoryAllocateFlagsInfoKHR
            {
                SType = StructureType.MemoryAllocateFlagsInfoKhr
            };

            MemoryAllocateInfo allocInfo = new MemoryAllocateInfo
            {
                SType = StructureType.MemoryAllocateInfo,
                AllocationSize = memRequirements.Size,
                MemoryTypeIndex = VulkanRenderer.GetMemoryType(memRequirements.MemoryTypeBits, properties),
                PNext = &extendedAllocFlagsInfo
            };


            VkFunc.vkAllocateMemory(VulkanRenderer.device, &allocInfo, null, out bufferMemory);

            return Result.Success;
        }

        public static unsafe void* MapBufferMemory(DeviceMemory bufferMemory, ulong bufferSize, bool* isMapped)
        {
            if (*isMapped)
            {
                throw new InvalidOperationException("Buffer already mapped!");
            }

            void* mappedData;
            Result result = VkFunc.vkMapMemory(VulkanRenderer.device, bufferMemory, 0, bufferSize, 0, &mappedData);
            if (result != Result.Success)
            {
                return null;
            }

            *isMapped = true;
            return mappedData;
        }

        public static Result CUnmapBufferMemory(DeviceMemory bufferMemory, bool* isMapped)
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

            bufferMemory = new DeviceMemory();

            VkBufferCreateInfo bufferInfo = new VkBufferCreateInfo
            {
                sType = VkStructureType.BufferCreateInfo,
                size = bufferSize,
                usage = bufferUsage,
                sharingMode = VkSharingMode.Exclusive
            };
            VkResult result = VkFunc.vkCreateBuffer(VulkanRenderer.device, &bufferInfo, null, out SVkBuffer bufferPtr);
            buffer = bufferPtr;

            result = VkAllocateMemory(buffer, out bufferMemory, properties);
            if (result != VkResult.Success)
            {
                return result;
            }

            result = VkFunc.vkBindBufferMemory(VulkanRenderer.device, buffer, bufferMemory, 0);
            if (result != VkResult.Success)
            {
                return result;
            }

            void* mappedData;
            result = VkFunc.vkMapMemory(VulkanRenderer.device, bufferMemory, 0, bufferSize, 0, &mappedData);
            if (result != VkResult.Success)
            {
                return result;
            }

            int intCount = (int)bufferSize / 4;
            int[] mappedDataArray = new int[intCount];
            Marshal.Copy((IntPtr)bufferData, mappedDataArray, 0, intCount);
            VkFunc.vkUnmapMemory(VulkanRenderer.device, bufferMemory);

            return VkResult.Success;
        }

        public static VkResult CreateStagingBuffer(out VkBuffer stagingBuffer, out VkDeviceMemory stagingBufferMemory, IntPtr bufferData, ulong bufferSize, VkBufferUsageFlagBits bufferUsage, VkMemoryPropertyFlagBits properties)
        {
            stagingBufferMemory = new VkDeviceMemory();
            var imageInfo = new BufferCreateInfo()
            {
                sType = VkStructureType.BufferCreateInfo,
                size = bufferSize,
                usage = VkBufferUsageFlagBits.TransferSrcBit,
                s = VkSharingMode.Exclusive
            };
            VkFunc.vkCreateBuffer(VulkanRenderer.device, &imageInfo, null, out stagingBuffer);

            VkFunc.vkGetBufferMemoryRequirements(VulkanRenderer.device, stagingBuffer, out MemoryRequirements memRequirements);

            AllocateMemory(stagingBuffer, out stagingBufferMemory, properties);
            return VkFunc.vkBindBufferMemory(VulkanRenderer.device, stagingBuffer, stagingBufferMemory, 0);
        }

        public static Result CopyBuffer(Silk.NET.Vulkan.Buffer srcBuffer, Silk.NET.Vulkan.Buffer dstBuffer, ulong size)
        {
            BufferCopy copyRegion = new BufferCopy
            {
                SrcOffset = 0,
                DstOffset = 0,
                Size = size
            };

            CommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
            VkFunc.vkCmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, 1, &copyRegion);
            return VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
        }

        public static Result CopyStagingBuffer(CommandBuffer commandBuffer, Silk.NET.Vulkan.Buffer srcBuffer, Silk.NET.Vulkan.Buffer dstBuffer, ulong size)
        {
            BufferCopy copyRegion = new BufferCopy
            {
                SrcOffset = 0,
                DstOffset = 0,
                Size = size
            };

            VkFunc.vkCmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, 1, &copyRegion);
            return Result.Success;
        }

        public static Result UpdateBufferSize(Silk.NET.Vulkan.Buffer buffer, DeviceMemory bufferMemory, void* bufferData, ref ulong oldBufferSize, ulong newBufferSize, BufferUsageFlags bufferUsageFlags, MemoryPropertyFlags propertyFlags)
        {
            if (newBufferSize < oldBufferSize)
            {
                throw new InvalidOperationException("New buffer size can't be less than the old buffer size.");
            }

            oldBufferSize = newBufferSize;

            BufferCreateInfo bufferCreateInfo = new BufferCreateInfo
            {
                SType = StructureType.BufferCreateInfo,
                Size = newBufferSize,
                Usage = bufferUsageFlags,
                SharingMode = SharingMode.Exclusive
            };

            var tempbuffer = buffer;
            Result result = VkFunc.vkCreateBuffer(VulkanRenderer.device, &bufferCreateInfo, null, &tempbuffer);
            buffer = tempbuffer;
            if (result != Result.Success)
            {
                return result;
            }

            result = AllocateMemory(buffer, out bufferMemory, propertyFlags);
            if (result != Result.Success)
            {
                return result;
            }

            result = VkFunc.vkBindBufferMemory(VulkanRenderer.device, buffer, bufferMemory, 0);
            if (result != Result.Success)
            {
                return result;
            }

            return VkFunc.vkMapMemory(VulkanRenderer.device, bufferMemory, 0, newBufferSize, 0, &bufferData);
        }

        public static Result UpdateBufferMemory(DeviceMemory bufferMemory, void* dataToCopy, ulong bufferSize)
        {
            if (dataToCopy == null || bufferSize == 0)
            {
                throw new InvalidOperationException("Buffer Data and Size can't be NULL");
            }

            void* mappedData;
            Result result = VkFunc.vkMapMemory(VulkanRenderer.device, bufferMemory, 0, bufferSize, 0, &mappedData);
            if (result != Result.Success)
            {
                return result;
            }

            int intCount = (int)bufferSize / 4;
            int[] mappedDataArray = new int[intCount];
            Marshal.Copy((IntPtr)dataToCopy, mappedDataArray, 0, intCount);
            VkFunc.vkUnmapMemory(VulkanRenderer.device, bufferMemory);
            return Result.Success;
        }


        public static Result BuffUpdateStagingBufferMemory(DeviceMemory bufferMemory, void* dataToCopy, ulong bufferSize)
        {
            if (dataToCopy == null || bufferSize == 0)
            {
                throw new InvalidOperationException("Buffer Data and Size can't be NULL");
            }

            void* mappedData;
            Result result = VkFunc.vkMapMemory(VulkanRenderer.device, bufferMemory, 0, bufferSize, 0, &mappedData);
            if (result != Result.Success)
            {
                return result;
            }

            int intCount = (int)bufferSize / 4;
            int[] mappedDataArray = new int[intCount];
            Marshal.Copy((IntPtr)dataToCopy, mappedDataArray, 0, intCount);
            VkFunc.vkUnmapMemory(VulkanRenderer.device, bufferMemory);
            return Result.Success;
        }
    }
}
