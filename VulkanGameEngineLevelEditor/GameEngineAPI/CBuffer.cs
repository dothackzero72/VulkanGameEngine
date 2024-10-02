using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public static unsafe class CBuffer
    {

        public static Result AllocateMemory(Silk.NET.Vulkan.Buffer buffer, out DeviceMemory bufferMemory, MemoryPropertyFlags properties)
        {
            bufferMemory = new DeviceMemory();
            VKConst.vulkan.GetBufferMemoryRequirements(SilkVulkanRenderer.device, buffer, out MemoryRequirements memRequirements);

            MemoryAllocateFlagsInfoKHR extendedAllocFlagsInfo = new MemoryAllocateFlagsInfoKHR
            {
                SType = StructureType.MemoryAllocateFlagsInfoKhr
            };

            MemoryAllocateInfo allocInfo = new MemoryAllocateInfo
            {
                SType = StructureType.MemoryAllocateInfo,
                AllocationSize = memRequirements.Size,
                MemoryTypeIndex = SilkVulkanRenderer.GetMemoryType(memRequirements.MemoryTypeBits, properties),
                PNext = &extendedAllocFlagsInfo
            };


            VKConst.vulkan.AllocateMemory(SilkVulkanRenderer.device, &allocInfo, null, out bufferMemory);

            return Result.Success;
        }

        public static unsafe void* MapBufferMemory(DeviceMemory bufferMemory, ulong bufferSize, bool* isMapped)
        {
            if (*isMapped)
            {
                throw new InvalidOperationException("Buffer already mapped!");
            }

            void* mappedData;
            Result result = VKConst.vulkan.MapMemory(SilkVulkanRenderer.device, bufferMemory, 0, bufferSize, 0, &mappedData);
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
                VKConst.vulkan.UnmapMemory(SilkVulkanRenderer.device, bufferMemory);
                *isMapped = false;
            }
            return Result.Success;
        }

        public static Result CreateBuffer(out Silk.NET.Vulkan.Buffer buffer, out DeviceMemory bufferMemory, void* bufferData, ulong bufferSize, BufferUsageFlags bufferUsage, MemoryPropertyFlags properties)
        {
            if (bufferData == null || bufferSize == 0)
            {
                throw new InvalidOperationException("Buffer Data and Size can't be NULL");
            }

            bufferMemory = new DeviceMemory();

            BufferCreateInfo bufferInfo = new BufferCreateInfo
            {
                SType = StructureType.BufferCreateInfo,
                Size = bufferSize,
                Usage = bufferUsage,
                SharingMode = SharingMode.Exclusive
            };
            Result result = VKConst.vulkan.CreateBuffer(SilkVulkanRenderer.device, &bufferInfo, null, out Silk.NET.Vulkan.Buffer bufferPtr);
            buffer = bufferPtr;

            result = AllocateMemory(buffer, out bufferMemory, properties);
            if (result != Result.Success)
            {
                return result;
            }

            result = VKConst.vulkan.BindBufferMemory(SilkVulkanRenderer.device, buffer, bufferMemory, 0);
            if (result != Result.Success)
            {
                return result;
            }

            void* mappedData;
            result = VKConst.vulkan.MapMemory(SilkVulkanRenderer.device, bufferMemory, 0, bufferSize, 0, &mappedData);
            if (result != Result.Success)
            {
                return result;
            }

            int intCount = (int)bufferSize / 4;
            int[] mappedDataArray = new int[intCount];
            Marshal.Copy((IntPtr)bufferData, mappedDataArray, 0, intCount);
            VKConst.vulkan.UnmapMemory(SilkVulkanRenderer.device, bufferMemory);

            return Result.Success;
        }

        public static Result CreateStagingBuffer(out Silk.NET.Vulkan.Buffer stagingBuffer, out DeviceMemory stagingBufferMemory, IntPtr bufferData, ulong bufferSize, BufferUsageFlags bufferUsage, MemoryPropertyFlags properties)
        {
            stagingBufferMemory = new DeviceMemory();
            var imageInfo = new BufferCreateInfo()
            {
                SType = StructureType.BufferCreateInfo,
                Size = bufferSize,
                Usage = BufferUsageFlags.TransferSrcBit,
                SharingMode = SharingMode.Exclusive
            };
            VKConst.vulkan.CreateBuffer(SilkVulkanRenderer.device, &imageInfo, null, out stagingBuffer);

            VKConst.vulkan.GetBufferMemoryRequirements(SilkVulkanRenderer.device, stagingBuffer, out MemoryRequirements memRequirements);

            AllocateMemory(stagingBuffer, out stagingBufferMemory, properties);
            return VKConst.vulkan.BindBufferMemory(SilkVulkanRenderer.device, stagingBuffer, stagingBufferMemory, 0);
        }

        public static Result CopyBuffer(Silk.NET.Vulkan.Buffer srcBuffer, Silk.NET.Vulkan.Buffer dstBuffer, ulong size)
        {
            BufferCopy copyRegion = new BufferCopy
            {
                SrcOffset = 0,
                DstOffset = 0,
                Size = size
            };

            CommandBuffer commandBuffer = SilkVulkanRenderer.BeginSingleUseCommandBuffer();
            VKConst.vulkan.CmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, 1, &copyRegion);
            return SilkVulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
        }

        public static Result CopyStagingBuffer(CommandBuffer commandBuffer, Silk.NET.Vulkan.Buffer srcBuffer, Silk.NET.Vulkan.Buffer dstBuffer, ulong size)
        {
            BufferCopy copyRegion = new BufferCopy
            {
                SrcOffset = 0,
                DstOffset = 0,
                Size = size
            };

            VKConst.vulkan.CmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, 1, &copyRegion);
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
            Result result = VKConst.vulkan.CreateBuffer(SilkVulkanRenderer.device, &bufferCreateInfo, null, &tempbuffer);
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

            result = VKConst.vulkan.BindBufferMemory(SilkVulkanRenderer.device, buffer, bufferMemory, 0);
            if (result != Result.Success)
            {
                return result;
            }

            return VKConst.vulkan.MapMemory(SilkVulkanRenderer.device, bufferMemory, 0, newBufferSize, 0, &bufferData);
        }

        public static Result UpdateBufferMemory(DeviceMemory bufferMemory, void* dataToCopy, ulong bufferSize)
        {
            if (dataToCopy == null || bufferSize == 0)
            {
                throw new InvalidOperationException("Buffer Data and Size can't be NULL");
            }

            void* mappedData;
            Result result = VKConst.vulkan.MapMemory(SilkVulkanRenderer.device, bufferMemory, 0, bufferSize, 0, &mappedData);
            if (result != Result.Success)
            {
                return result;
            }

            int intCount = (int)bufferSize / 4;
            int[] mappedDataArray = new int[intCount];
            Marshal.Copy((IntPtr)dataToCopy, mappedDataArray, 0, intCount);
            VKConst.vulkan.UnmapMemory(SilkVulkanRenderer.device, bufferMemory);
            return Result.Success;
        }


        public static Result BuffUpdateStagingBufferMemory(DeviceMemory bufferMemory, void* dataToCopy, ulong bufferSize)
        {
            if (dataToCopy == null || bufferSize == 0)
            {
                throw new InvalidOperationException("Buffer Data and Size can't be NULL");
            }

            void* mappedData;
            Result result = VKConst.vulkan.MapMemory(SilkVulkanRenderer.device, bufferMemory, 0, bufferSize, 0, &mappedData);
            if (result != Result.Success)
            {
                return result;
            }

            int intCount = (int)bufferSize / 4;
            int[] mappedDataArray = new int[intCount];
            Marshal.Copy((IntPtr)dataToCopy, mappedDataArray, 0, intCount);
            VKConst.vulkan.UnmapMemory(SilkVulkanRenderer.device, bufferMemory);
            return Result.Success;
        }
    }
}
