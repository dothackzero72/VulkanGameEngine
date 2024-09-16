using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor;
using System.Runtime.InteropServices;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class VulkanBuffer<T> where T : unmanaged
    {
        public VkBuffer Buffer { get; private set; }
        public VkBuffer StagingBuffer { get; private set; }
        public VkDeviceMemory BufferMemory { get; private set; }
        public VkDeviceMemory StagingBufferMemory { get; private set; }
        public VkDeviceSize BufferSize { get; private set; }
        public VkBufferUsageFlags BufferUsage { get; private set; }
        public VkMemoryPropertyFlagBits BufferProperties { get; private set; }
        public IntPtr BufferDeviceAddress { get; private set; }
        public IntPtr BufferHandle { get; private set; }
        public IntPtr BufferData { get; private set; }
        public bool IsMapped { get; private set; }
        public VkDescriptorBufferInfo DescriptorBufferInfo { get; private set; }

        protected VulkanBufferInfo SendCBufferInfo()
        {
            return new VulkanBufferInfo
            {
                Buffer = Buffer,
                StagingBuffer = StagingBuffer,
                BufferMemory = BufferMemory,
                StagingBufferMemory = StagingBufferMemory,
                BufferSize = BufferSize,
                BufferUsage = BufferUsage,
                BufferProperties = BufferProperties,
                BufferDeviceAddress = BufferDeviceAddress,
                BufferHandle = BufferHandle,
                BufferData = BufferData,
                IsMapped = IsMapped
            };
        }

        VkResult UpdateBufferSize(VkDeviceSize bufferSize)
        {
            BufferSize = bufferSize;
            VulkanBufferInfo vulkanBufferInfo = SendCBufferInfo();
            return GameEngineDLL.DLL_Buffer_UpdateBufferSize(ref vulkanBufferInfo, VulkanRenderer.Device, bufferSize);
        }

        public VulkanBuffer()
        {
        }

        public VulkanBuffer(IntPtr bufferData, UInt32 bufferSize, VkBufferUsageFlags usage, VkMemoryPropertyFlagBits properties)
        {
            VulkanBufferInfo vulkanBufferInfo = SendCBufferInfo();
            IntPtr vulkanBufferInfoPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VulkanBufferInfo)));
            CreateBuffer(vulkanBufferInfoPtr, bufferSize, usage, properties);
        }

        public virtual VkResult CopyBuffer(ref VkBuffer srcBuffer, ref VkBuffer dstBuffer, VkDeviceSize size)
        {
            return GameEngineDLL.DLL_Buffer_CopyBuffer( srcBuffer, dstBuffer, size);
        }

        public virtual VkResult CreateBuffer(VkBuffer bufferData, UInt32 bufferSize, VkBufferUsageFlags usage, VkMemoryPropertyFlagBits properties)
        {
            VulkanBufferInfo vulkanBufferInfo = SendCBufferInfo(); // Adjust if this needs to return details
            IntPtr vulkanBufferInfoPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VulkanBufferInfo)));

            // Initialize the allocated memory
            // Note: You need to set the Buffer and BufferMemory to IntPtr.Zero here
            Marshal.WriteIntPtr(vulkanBufferInfoPtr, IntPtr.Zero); // Buffer
            Marshal.WriteIntPtr(vulkanBufferInfoPtr + IntPtr.Size, IntPtr.Zero); // BufferMemory - offset by size of IntPtr for the next field
            Marshal.WriteInt64(vulkanBufferInfoPtr + IntPtr.Size * 2, 0); // BufferSize
            Marshal.WriteInt32(vulkanBufferInfoPtr + IntPtr.Size * 3, (int)usage); // BufferUsage
            Marshal.WriteInt32(vulkanBufferInfoPtr + IntPtr.Size * 4, (int)properties); // BufferProperties

            // Finally, make the PInvoke call
            var result = GameEngineDLL.DLL_Buffer_CreateBuffer(vulkanBufferInfoPtr, VulkanRenderer.Device, bufferData, bufferSize, usage, properties);

            // After using the pointer, be sure to free the allocated memory afterwards
            Marshal.FreeHGlobal(vulkanBufferInfoPtr);

            return result;
        }

        public virtual void UpdateBufferData(VkBuffer bufferData)
        {
            if (BufferSize < (ulong)sizeof(T))
            {
               // RENDERER_ERROR("Buffer does not contain enough data for a single T object.");
                return;
            }
            VulkanBufferInfo vulkanBufferInfo = SendCBufferInfo();
            GameEngineDLL.DLL_Buffer_UpdateBufferMemory(ref vulkanBufferInfo, VulkanRenderer.Device, bufferData, (uint)sizeof(T));
        }

        public List<T> CheckBufferContents()
        {
            List<T> DataList = new List<T>();
            ulong dataListSize = BufferSize / (ulong)sizeof(T);

            VulkanBufferInfo vulkanBufferInfo = SendCBufferInfo();
            IntPtr data = GameEngineDLL.DLL_Buffer_MapBufferMemory(ref vulkanBufferInfo, VulkanRenderer.Device);
            if (data == IntPtr.Zero)
            {
                return DataList;
            }

            for (int x = 0; x < (int)dataListSize; ++x)
            {
                IntPtr newPtr = IntPtr.Add(data, x * (int)sizeof(T));
                T item = Marshal.PtrToStructure<T>(newPtr);
                DataList.Add(item);
            }

            GameEngineDLL.DLL_Buffer_UnmapBufferMemory(ref vulkanBufferInfo, VulkanRenderer.Device);

            return DataList;
        }

        public VkDescriptorBufferInfo GetDescriptorbuffer()
        {
            return new VkDescriptorBufferInfo
            {
				buffer = Buffer,
				offset = 0,
				range = VulkanConsts.VK_WHOLE_SIZE
            };
        }

        public void DestroyBuffer()
        {
            VulkanBufferInfo vulkanBufferInfo = SendCBufferInfo();
            GameEngineDLL.DLL_Buffer_DestroyBuffer(ref vulkanBufferInfo);
        }
    }
}



