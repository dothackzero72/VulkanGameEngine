using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;
using VulkanGameEngineLevelEditor.Tests;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class VulkanBuffer<T> : IDisposable where T : unmanaged
    {
        public Silk.NET.Vulkan.Buffer Buffer { get; protected set; }
        public Silk.NET.Vulkan.Buffer StagingBuffer { get; protected set; }
        public DeviceMemory BufferMemory { get; protected set; }
        public DeviceMemory StagingBufferMemory { get; protected set; }
        public ulong BufferSize { get; protected set; }
        public BufferUsageFlags BufferUsage { get; protected set; }
        public MemoryPropertyFlags BufferProperties { get; protected set; }
        public void* BufferDeviceAddress { get; protected set; }
        public void* BufferHandle { get; protected set; }
        public void* BufferData { get; protected set; }
        public bool IsMapped { get; protected set; }
        public DescriptorBufferInfo DescriptorBufferInfo { get; protected set; }

        public VulkanBuffer() { }

        public VulkanBuffer(T data, BufferUsageFlags usage, MemoryPropertyFlags properties)
        {
            List<T> dataList = new List<T> { data };
            BufferSize = (uint)sizeof(T) * (uint)dataList.Count;
            BufferUsage = usage;
            BufferProperties = properties;
            CreateBuffer(dataList, usage, properties);
        }

        public VulkanBuffer(T[] data, BufferUsageFlags usage, MemoryPropertyFlags properties)
        {
            BufferSize = (uint)sizeof(T) * (uint)data.Length;
            BufferUsage = usage;
            BufferProperties = properties;
            CreateBuffer(data, usage, properties);
        }

        public VulkanBuffer(List<T> dataList, BufferUsageFlags usage, MemoryPropertyFlags properties)
        {
            BufferSize = (uint)sizeof(T) * (uint)dataList.Count;
            BufferUsage = usage;
            BufferProperties = properties;
            CreateBuffer(dataList, usage, properties);
        }

        public VulkanBuffer(void* data, ulong bufferSize, BufferUsageFlags usage, MemoryPropertyFlags properties)
        {
            BufferSize = bufferSize;
            BufferUsage = usage;
            BufferProperties = properties;
            CreateBuffer(data, bufferSize, usage, properties);
        }

        protected virtual Result CreateBuffer(T[] data, BufferUsageFlags bufferUsage, MemoryPropertyFlags properties)
        {
            GCHandle dataListHandle = GCHandle.Alloc(data.ToArray(), GCHandleType.Pinned);
            IntPtr dataListPtr = dataListHandle.AddrOfPinnedObject();

            Silk.NET.Vulkan.Buffer buffer = new Silk.NET.Vulkan.Buffer();
            DeviceMemory bufferMemory = new DeviceMemory();

            Result result = InternCreateBuffer(
                out buffer,
                (void*)dataListPtr,
                BufferSize,
                bufferUsage,
                properties
            );

            Buffer = buffer;
            BufferMemory = bufferMemory;

            dataListHandle.Free();
            return result;
        }

        protected virtual Result CreateBuffer(List<T> dataList, BufferUsageFlags bufferUsage, MemoryPropertyFlags properties)
        {
            GCHandle dataListHandle = GCHandle.Alloc(dataList.ToArray(), GCHandleType.Pinned);
            void* dataListPtr = (void*)dataListHandle.AddrOfPinnedObject();

            Silk.NET.Vulkan.Buffer buffer = new Silk.NET.Vulkan.Buffer();
            DeviceMemory bufferMemory = new DeviceMemory();

            Result result = InternCreateBuffer(
                out buffer,
                dataListPtr,
                BufferSize,
                bufferUsage,
                properties
            );

            Buffer = buffer;
            BufferMemory = bufferMemory;

            dataListHandle.Free();
            return result;
        }

        protected virtual Result CreateBuffer(void* bufferData, ulong bufferSize, BufferUsageFlags bufferUsage, MemoryPropertyFlags properties)
        {
            Silk.NET.Vulkan.Buffer buffer = new Silk.NET.Vulkan.Buffer();
            DeviceMemory bufferMemory = new DeviceMemory();

            Result result = InternCreateBuffer(
                out buffer,
                bufferData,
                bufferSize,
                bufferUsage,
                properties
            );

            Buffer = buffer;
            BufferMemory = bufferMemory;
            return result;
        }

        protected virtual Result CreateStagingBuffer()
        {
            Silk.NET.Vulkan.Buffer stagingBuffer = StagingBuffer;
            DeviceMemory stagingBufferMemory = StagingBufferMemory;
            var result = CreateStagingBuffer(out stagingBuffer, out stagingBufferMemory, (IntPtr)BufferData, BufferSize, BufferUsage, BufferProperties);
            return result;
        }

        //public void DestroyBuffer()
        //{
        //    var buffer = Buffer;
        //    var stagingBuffer = StagingBuffer;
        //    var bufferMemory = BufferMemory;
        //    var stagingBufferMemory = StagingBufferMemory;
        //    var bufferSize = BufferSize;
        //    var bufferUsage = BufferUsage;
        //    var bufferProperties = BufferProperties;
        //    DestroyBuffer(VulkanRenderer.Device, ref buffer, ref stagingBuffer, ref bufferMemory, ref stagingBufferMemory, BufferData, ref bufferSize, ref bufferUsage, ref bufferProperties);
        //}

        public DescriptorBufferInfo GetDescriptorBufferInfo()
        {
            return new DescriptorBufferInfo
            {
                Buffer = Buffer,
                Offset = 0,
                Range = Vk.WholeSize
            };
        }

        public List<T> CheckBufferContents()
        {
            List<T> dataList = new List<T>();
            ulong dataListSize = BufferSize / (ulong)sizeof(T);

            var isMapped = IsMapped;
            void* data = MapBufferMemory(BufferMemory, BufferSize, &isMapped);
            if (data == null)
            {
                return dataList;
            }

            for (int x = 0; x < (int)dataListSize; ++x)
            {
                IntPtr newPtr = IntPtr.Add((IntPtr)data, x * (int)sizeof(T));
                T item = Marshal.PtrToStructure<T>(newPtr);
                dataList.Add(item);
            }

            UnmapBufferMemory(BufferMemory, &isMapped);
            return dataList;
        }

        public void Dispose()
        {
        }

        private Result AllocateMemory(Silk.NET.Vulkan.Buffer buffer, MemoryPropertyFlags properties)
        {
            VKConst.vulkan.GetBufferMemoryRequirements(SilkVulkanRenderer.device, buffer, out MemoryRequirements memRequirements);

            MemoryAllocateFlagsInfoKHR extendedAllocFlagsInfo = new MemoryAllocateFlagsInfoKHR
            {
                SType = StructureType.MemoryAllocateFlagsInfoKhr
            };

            MemoryAllocateInfo allocInfo = new MemoryAllocateInfo
            {
                SType = StructureType.MemoryAllocateInfo,
                AllocationSize = memRequirements.Size,
                MemoryTypeIndex = SilkVulkanRenderer.GetMemoryType( memRequirements.MemoryTypeBits, properties),
                PNext = &extendedAllocFlagsInfo
            };

            VKConst.vulkan.AllocateMemory(SilkVulkanRenderer.device, &allocInfo, null, out DeviceMemory bufferMemory);
            BufferMemory = bufferMemory;

            return Result.Success;
        }

        private unsafe void* MapBufferMemory(DeviceMemory bufferMemory, ulong bufferSize, bool* isMapped)
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

        private Result UnmapBufferMemory(DeviceMemory bufferMemory, bool* isMapped)
        {
            if (*isMapped)
            {
                VKConst.vulkan.UnmapMemory(SilkVulkanRenderer.device, bufferMemory);
                *isMapped = false;
            }
            return Result.Success;
        }

        private Result InternCreateBuffer(out Silk.NET.Vulkan.Buffer buffer, void* bufferData, ulong bufferSize, BufferUsageFlags bufferUsage, MemoryPropertyFlags properties)
        {
            if (bufferData == null || bufferSize == 0)
            {
                throw new InvalidOperationException("Buffer Data and Size can't be NULL");
            }

            BufferCreateInfo bufferInfo = new BufferCreateInfo
            {
                SType = StructureType.BufferCreateInfo,
                Size = bufferSize,
                Usage = bufferUsage,
                SharingMode = SharingMode.Exclusive
            };
            Result result = VKConst.vulkan.CreateBuffer(SilkVulkanRenderer.device, &bufferInfo, null, out Silk.NET.Vulkan.Buffer bufferPtr);
            buffer = bufferPtr;

            result = AllocateMemory(buffer, properties);
            if (result != Result.Success)
            {
                return result;
            }

            result = VKConst.vulkan.BindBufferMemory(SilkVulkanRenderer.device, buffer, BufferMemory, 0);
            if (result != Result.Success)
            {
                return result;
            }

            void* mappedData;
            result = VKConst.vulkan.MapMemory(SilkVulkanRenderer.device, BufferMemory, 0, bufferSize, 0, &mappedData);
            if (result != Result.Success)
            {
                return result;
            }

            int intCount = (int)bufferSize / 4;
            int[] mappedDataArray = new int[intCount];
            Marshal.Copy((IntPtr)bufferData, mappedDataArray, 0, intCount);
            VKConst.vulkan.UnmapMemory(SilkVulkanRenderer.device, BufferMemory);

            return Result.Success;
        }

        private Result CreateStagingBuffer(out Silk.NET.Vulkan.Buffer stagingBuffer, out DeviceMemory stagingBufferMemory, IntPtr bufferData, ulong bufferSize, BufferUsageFlags bufferUsage, MemoryPropertyFlags properties)
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

            AllocateMemory(stagingBuffer, properties);
            return VKConst.vulkan.BindBufferMemory(SilkVulkanRenderer.device, stagingBuffer, stagingBufferMemory, 0);
        }

        private Result CopyBuffer(Silk.NET.Vulkan.Buffer srcBuffer, Silk.NET.Vulkan.Buffer dstBuffer, ulong size)
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

        private Result CopyStagingBuffer(CommandBuffer commandBuffer, Silk.NET.Vulkan.Buffer srcBuffer, Silk.NET.Vulkan.Buffer dstBuffer, ulong size)
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

        private Result UpdateBufferSize(Silk.NET.Vulkan.Buffer buffer, DeviceMemory bufferMemory, void* bufferData, ref ulong oldBufferSize, ulong newBufferSize, BufferUsageFlags bufferUsageFlags, MemoryPropertyFlags propertyFlags)
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

            result = AllocateMemory(buffer, propertyFlags);
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

        private Result UpdateBufferMemory(DeviceMemory bufferMemory, void* dataToCopy, ulong bufferSize)
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

        
        private Result UpdateStagingBufferMemory(DeviceMemory bufferMemory, void* dataToCopy, ulong bufferSize)
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

        //private void DestroyBuffer(Device device, ref Buffer buffer, ref Buffer stagingBuffer, ref DeviceMemory bufferMemory, ref DeviceMemory stagingBufferMemory, IntPtr bufferData, ref DeviceSize bufferSize, ref BufferUsageFlags bufferUsageFlags, ref MemoryPropertyFlags propertyFlags)
        //{
        //    bufferSize = 0;
        //    bufferUsageFlags = 0;
        //    propertyFlags = 0;
        //    bufferData = IntPtr.Zero;

        //    Renderer_DestroyBuffer(SilkVulkanRenderer.device, ref buffer);
        //    Renderer_DestroyBuffer(SilkVulkanRenderer.device, ref stagingBuffer);
        //    Renderer_FreeDeviceMemory(SilkVulkanRenderer.device, ref bufferMemory);
        //    Renderer_FreeDeviceMemory(SilkVulkanRenderer.device, ref stagingBufferMemory);
        //}
    }
}