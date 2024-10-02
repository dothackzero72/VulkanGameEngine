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

            Result result = CBuffer.CreateBuffer(
                out buffer,
                out bufferMemory,
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

            Result result = CBuffer.CreateBuffer(
                out buffer,
                                out bufferMemory,
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

            Result result = CBuffer.CreateBuffer(
                out buffer,
                                out bufferMemory,
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
            var result = CBuffer.CreateStagingBuffer(out stagingBuffer, out stagingBufferMemory, (IntPtr)BufferData, BufferSize, BufferUsage, BufferProperties);
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
            void* data = CBuffer.MapBufferMemory(BufferMemory, BufferSize, &isMapped);
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

            CBuffer.CUnmapBufferMemory(BufferMemory, &isMapped);
            return dataList;
        }

        public void Dispose()
        {
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