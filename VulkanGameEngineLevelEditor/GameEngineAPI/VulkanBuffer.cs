//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VulkanGameEngineLevelEditor.GameEngineAPI;
//using VulkanGameEngineLevelEditor;

//namespace VulkanGameEngineLevelEditor.GameEngineAPI
//{
//public unsafe class VulkanBuffer<T>
//    {
//        protected VkBuffer StagingBuffer;
//        protected VkDeviceMemory StagingBufferMemory;
//        protected VkDeviceMemory BufferMemory;
//        protected VkDeviceSize BufferSize;
//        protected VkBufferUsageFlags BufferUsage;
//        protected VkMemoryPropertyFlags BufferProperties;
//        protected ulong BufferDeviceAddress;
//        protected VkAccelerationStructureKHR BufferHandle;
//        protected void* BufferData;
//        protected bool IsMapped;
//        public VkBuffer Buffer;
//        public VkDescriptorBufferInfo DescriptorBufferInfo;

//        protected VulkanBufferInfo SendCBufferInfo()
//        {
//            VulkanBufferInfo bufferInfo = new VulkanBufferInfo
//            {
//                Buffer =  Buffer,
//                StagingBuffer = StagingBuffer,
//                BufferMemory = BufferMemory,
//                StagingBufferMemory = StagingBufferMemory,
//                BufferSize = BufferSize,
//                BufferUsage = BufferUsage,
//                BufferProperties = BufferProperties,
//                BufferDeviceAddress = BufferDeviceAddress,
//                BufferHandle = BufferHandle,
//                BufferData = BufferData,
//                IsMapped = IsMapped
//            };
//            return bufferInfo;
//        }

//        VkResult UpdateBufferSize(VkDeviceSize bufferSize)
//        {
//            BufferSize = bufferSize;
//            return DLL_Buffer_UpdateBufferSize(ref SendCBufferInfo(), bufferSize);
//        }

//        public VulkanBuffer()
//        {
//        }

//        public VulkanBuffer(void* bufferData, uint32 bufferSize, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties)
//        {
//            BufferData = bufferData;
//            CreateBuffer(bufferData, bufferSize, usage, properties);
//        }

//        public static VkResult CopyBuffer(VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
//        {
//            return Buffer_CopyBuffer(SendCBufferInfo().get(), srcBuffer, dstBuffer, size);
//        }

//        public VkResult CreateBuffer(void* bufferData, uint32 bufferSize, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties)
//        {
//            return Buffer_CreateBuffer(SendCBufferInfo().get(), bufferData, bufferSize, usage, properties);
//        }

//        public void UpdateBufferData(const T& bufferData)
//		{
//			if (BufferSize< sizeof(T)) 
//			{
//				RENDERER_ERROR("Buffer does not contain enough data for a single T object.");
//				return;
//			}
//            Buffer_UpdateBufferMemory(SendCBufferInfo().get(), const_cast<void*>(static_cast<const void*>(&bufferData)), sizeof(T));
//		}

//    public void UpdateBufferData(const List<T>& bufferData)
//    {
//        const VkDeviceSize newBufferSize = sizeof(T) * bufferData.size();
//        if (BufferSize != newBufferSize)
//        {
//            if (UpdateBufferSize(newBufferSize) != VK_SUCCESS)
//            {
//                RENDERER_ERROR("Failed to update buffer size.");
//                return;
//            }
//        }

//        if (!IsMapped)
//        {
//            RENDERER_ERROR("Buffer is not mapped! Cannot update data.");
//            return;
//        }

//        Buffer_UpdateBufferMemory(SendCBufferInfo().get(), const_cast<void*>(static_cast <const void*> (bufferData.data())), newBufferSize);
//    }

//    public void UpdateBufferData(void* bufferData)
//    {
//        if (BufferSize < sizeof(T))
//        {
//            RENDERER_ERROR("Buffer does not contain enough data for a single T object.");
//            return;
//        }
//        Buffer_UpdateBufferMemory(SendCBufferInfo().get(), bufferData, sizeof(T));
//    }

//     List<T> CheckBufferContents<T>()
//    {
//        List<T> DataList;
//        size_t dataListSize = BufferSize / sizeof(T);

//        void* data = Buffer_MapBufferMemory(SendCBufferInfo().get());
//        if (data == nullptr)
//        {
//            std::cerr << "Failed to map buffer memory\n";
//            return DataList;
//        }

//        char* newPtr = static_cast<char*>(data);
//        for (size_t x = 0; x < dataListSize; ++x)
//        {
//            DataList.emplace_back(*reinterpret_cast<T*>(newPtr));
//            newPtr += sizeof(T);
//        }
//        Buffer_UnmapBufferMemory(SendCBufferInfo().get());
//        return DataList;
//    }

//    public VkDescriptorBufferInfo GetDescriptorbuffer()
//    {
//        DescriptorBufferInfo = VkDescriptorBufferInfo

//                {
//				    .buffer = Buffer,
//				    .offset = 0,
//				    .range = VK_WHOLE_SIZE

//                };
//        return &DescriptorBufferInfo;
//    }

//    public void DestroyBuffer()
//    {
//        Buffer_DestroyBuffer(SendCBufferInfo().get());
//    }


//};



