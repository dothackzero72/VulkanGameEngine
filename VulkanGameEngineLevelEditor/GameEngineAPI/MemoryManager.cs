using Coral.Managed.Interop;
using Newtonsoft.Json.Linq;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineGameObjectScripts.Component;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.RenderPassEditor;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class MemoryManager
    {
        public static Vk vk = Vk.GetApi();
       // public static Assembly GameObjectComponentDLL { get; set; }
        public static List<GameObject> GameObjectList { get; set; } = new List<GameObject>();
        public static List<Texture> TextureList { get; set; } = new List<Texture>();
        public static List<Material> MaterialList { get; set; } = new List<Material>();
        public static List<MeshRenderer2DComponent> RenderMesh2DComponentList { get; set; } = new List<MeshRenderer2DComponent>();
        //public static List<InputComponent> InputComponentList { get; set; } = new List<InputComponent>();
        // public static List<MeshRenderer3DComponent> RenderMesh3DComponentList = new List<MeshRenderer3DComponent>();

        public static MemoryPool<GameObject> GameObjectMemoryPool { get; set; } = new MemoryPool<GameObject>();
       // public static MemoryPool<InputComponent> InputComponentMemoryPool { get; set; } = new MemoryPool<InputComponent>();
        public static MemoryPool<MeshRenderer2DComponent> RenderMesh2DComponentMemoryPool { get; set; } = new MemoryPool<MeshRenderer2DComponent>();
        //public static MemoryPool<MeshRenderer3DComponent> RenderMesh3DComponentMemoryPool = new MemoryPool<MeshRenderer3DComponent>();
        public static MemoryPool<Texture> TextureMemoryPool { get; set; } = new MemoryPool<Texture>();

        public static void StartUp(uint estObjectCount)
        {
          //  GameObjectComponentDLL = Assembly.LoadFrom(ConstConfig.GameObjectComponentDLLPath);
            GameObjectMemoryPool.CreateMemoryPool(estObjectCount);
            RenderMesh2DComponentMemoryPool.CreateMemoryPool(estObjectCount);
          //  InputComponentMemoryPool.CreateMemoryPool(estObjectCount);
         //   RenderMesh3DComponentMemoryPool.CreateMemoryPool(estObjectCount);
            TextureMemoryPool.CreateMemoryPool(estObjectCount);
        }

        public static GameObject CreateGameObject(string name)
        {
            GameObject gameObject = MemoryManager.AllocateGameObject();
            gameObject.Initialize(name);
            return gameObject;
        }

        public static GameObject CreateGameObject(string name, List<ComponentTypeEnum> componentTypeList)
        {
            GameObject gameObject = MemoryManager.AllocateGameObject();
            gameObject.Initialize(name);

            GCHandle handle = GCHandle.Alloc(gameObject, GCHandleType.Normal);
            IntPtr parentGameObjectPtr = GCHandle.ToIntPtr(handle);

            foreach (var component in componentTypeList)
            {
                switch (component)
                {
                    case ComponentTypeEnum.kGameObjectTransform2DComponent: gameObject.AddComponent(new Transform2DComponent(IntPtr.Zero, IntPtr.Zero, parentGameObjectPtr, "Testing")); break;
                    case ComponentTypeEnum.kRenderMesh2DComponent: gameObject.AddComponent(MeshRenderer2DComponent.CreateRenderMesh2DComponent(parentGameObjectPtr, "Mesh Renderer", (uint)MemoryManager.RenderMesh2DComponentList.Count)); break;
                    case ComponentTypeEnum.kInputComponent: gameObject.AddComponent(new InputComponent(IntPtr.Zero, IntPtr.Zero, parentGameObjectPtr)); break;
                }
            }
            return gameObject;
        }

        public static GameObject AllocateGameObject()
        {
            GameObjectList.Add(GameObjectMemoryPool.AllocateMemoryLocation());
            return GameObjectList.Last();
        }

        public static MeshRenderer2DComponent AllocateGameRenderMesh2DComponent()
        {
            RenderMesh2DComponentList.Add(RenderMesh2DComponentMemoryPool.AllocateMemoryLocation());
            return RenderMesh2DComponentList.Last();
        }

        //public static InputComponent AllocateInputComponenComponent()
        //{
        //    InputComponentList.Add(InputComponentMemoryPool.AllocateMemoryLocation());
        //    return InputComponentList.Last();
        //}

        //public static MeshRenderer3DComponent AllocateGameRenderMesh3DComponent()
        //{
        //    RenderMesh3DComponentList.Add(RenderMesh3DComponentMemoryPool.AllocateMemoryLocation());
        //    return RenderMesh3DComponentList.Last();
        //}

        public static Texture AllocateTexture()
        {
            TextureList.Add(TextureMemoryPool.AllocateMemoryLocation());
            return TextureList.Last();
        }

        public static List<VkDescriptorBufferInfo> GetGameObjectPropertiesBuffer()
        {
            List<VkDescriptorBufferInfo> MeshPropertiesBuffer = new List<VkDescriptorBufferInfo>();
            if (RenderMesh2DComponentList.Count == 0)
            {
                VkDescriptorBufferInfo nullBuffer = new VkDescriptorBufferInfo();
                nullBuffer.buffer = new VkBuffer();
                nullBuffer.offset = 0;
                nullBuffer.range = Vk.WholeSize;
                MeshPropertiesBuffer.Add(nullBuffer);
            }
            else
            {
                foreach (var mesh in RenderMesh2DComponentList)
                {
                    if (mesh != null)
                    {
                        VkDescriptorBufferInfo MeshProperitesBufferInfo = new VkDescriptorBufferInfo();
                        MeshProperitesBufferInfo.buffer = mesh.GetMeshPropertiesBuffer().Buffer;
                        MeshProperitesBufferInfo.offset = 0;
                        MeshProperitesBufferInfo.range = Vk.WholeSize;
                        MeshPropertiesBuffer.Add(MeshProperitesBufferInfo);
                    }
                }
            }

            return MeshPropertiesBuffer;
        }

        public static List<VkDescriptorImageInfo> GetTexturePropertiesBuffer()
        {
            List<VkDescriptorImageInfo> TexturePropertiesBuffer = new List<VkDescriptorImageInfo>();
            if (TextureList.Count == 0)
            {
                VkSamplerCreateInfo NullSamplerInfo = new VkSamplerCreateInfo
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
                    magFilter = VkFilter.VK_FILTER_NEAREST,
                    minFilter = VkFilter.VK_FILTER_NEAREST,
                    addressModeU = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
                    addressModeV = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
                    addressModeW = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
                    anisotropyEnable = true,
                    maxAnisotropy = 16.0f,
                    borderColor = VkBorderColor.VK_BORDER_COLOR_FLOAT_OPAQUE_BLACK,
                    unnormalizedCoordinates = false,
                    compareEnable = false,
                    compareOp = VkCompareOp.VK_COMPARE_OP_ALWAYS,
                    mipmapMode = VkSamplerMipmapMode.VK_SAMPLER_MIPMAP_MODE_LINEAR,
                    minLod = 0,
                    maxLod = 0,
                    mipLodBias = 0
                };
                var result = VkFunc.vkCreateSampler(VulkanRenderer.device, &NullSamplerInfo, null, out VkSampler nullSampler);


                VkDescriptorImageInfo nullBuffer = new VkDescriptorImageInfo
                {
                    imageLayout = VkImageLayout.VK_IMAGE_LAYOUT_DEPTH_READ_ONLY_OPTIMAL,
                    imageView = new VkImageView(),
                    sampler = nullSampler
                };
                TexturePropertiesBuffer.Add(nullBuffer);
            }
            else
            {
                foreach (var texture in TextureList)
                {
                    if (texture != null)
                    {
                        VkDescriptorImageInfo textureDescriptor = new VkDescriptorImageInfo
                        {
                            imageLayout = VkImageLayout.VK_IMAGE_LAYOUT_DEPTH_READ_ONLY_OPTIMAL,
                            imageView = texture.View,
                            sampler = texture.Sampler
                        };
                        TexturePropertiesBuffer.Add(textureDescriptor);
                    }
                }
            }

            return TexturePropertiesBuffer;
        }

        public static List<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer()
        {
            List<VkDescriptorBufferInfo> MeshPropertiesBuffer = new List<VkDescriptorBufferInfo>();
            if (RenderMesh2DComponentList.Count == 0)
            {
                VkDescriptorBufferInfo nullBuffer = new VkDescriptorBufferInfo();
                nullBuffer.buffer = new VkBuffer();
                nullBuffer.offset = 0;
                nullBuffer.range = Vk.WholeSize;
                MeshPropertiesBuffer.Add(nullBuffer);
            }
            else
            {
                foreach (var mesh in MaterialList)
                {
                    mesh.GetMaterialPropertiesBuffer(ref MeshPropertiesBuffer);
                }
            }
            return MeshPropertiesBuffer;
        }

        public static void ViewMemoryMap()
        {
            var gameObjectMemoryList = GameObjectMemoryPool.ViewMemoryPool();
            var renderMesh2DMemoryList = RenderMesh2DComponentMemoryPool.ViewMemoryPool();
           // var inputMemoryList = InputComponentMemoryPool.ViewMemoryPool();
            // var renderMesh3DMemoryList = RenderMesh3DComponentMemoryPool.ViewMemoryPool();
            var textureMemoryList = TextureMemoryPool.ViewMemoryPool();

            Console.WriteLine($"Memory Map of Game Objects:");
            Console.WriteLine("{0,20} {1,15}", "Index", "Value");
            for (int x = 0; x < GameObjectMemoryPool.ObjectCount; x++)
            {
                var gameObjectMemoryListRef = gameObjectMemoryList[x];
                GameObject* gameObjectPtr = &gameObjectMemoryListRef;

                IntPtr address = (IntPtr)gameObjectPtr + (sizeof(GameObject) * x);

                string value = gameObjectMemoryList[x]?.ToString() ?? "null";
                Console.WriteLine($"{x,10} : {address.ToString("X12")} : {value}");
            }
            Console.WriteLine();

            Console.WriteLine($"Memory Map of RenderMesh2DComponent:");
            Console.WriteLine("{0,20} {1,15}", "Index", "Value");
            for (int x = 0; x < RenderMesh2DComponentMemoryPool.ObjectCount; x++)
            {
                var render2DMemoryListRef = renderMesh2DMemoryList[x];
                MeshRenderer2DComponent* render2DPtr = &render2DMemoryListRef;

                IntPtr address = (IntPtr)render2DPtr + (sizeof(MeshRenderer2DComponent) * x);

                string value = renderMesh2DMemoryList[x]?.ToString() ?? "null";
                Console.WriteLine($"{x,10} : {address.ToString("X12")} : {value}");
            }

            Console.WriteLine();

            //Console.WriteLine($"Memory Map of RenderMesh3DComponent:");
            //Console.WriteLine("{0,20} {1,15}", "Index", "Value");
            //for (int x = 0; x < RenderMesh3DComponentMemoryPool.ObjectCount; x++)
            //{
            //    var render3DMemoryListRef = renderMesh3DMemoryList[x];
            //    MeshRenderer3DComponent* render3DPtr = &render3DMemoryListRef;

            //    IntPtr address = (IntPtr)render3DPtr + (sizeof(MeshRenderer3DComponent) * x);

            //    string value = renderMesh3DMemoryList[x]?.ToString() ?? "null";
            //    Console.WriteLine($"{x,10} : {address.ToString("X12")} : {value}");
            //}

            //Console.WriteLine();

            //Console.WriteLine($"Memory Map of InputComponent:");
            //Console.WriteLine("{0,20} {1,15}", "Index", "Value");
            //for (int x = 0; x < InputComponentMemoryPool.ObjectCount; x++)
            //{
            //    var inputMemoryListRef = inputMemoryList[x];
            //    InputComponent* inputPtr = &inputMemoryListRef;

            //    IntPtr address = (IntPtr)inputPtr + (sizeof(InputComponent) * x);

            //    string value = inputMemoryList[x]?.ToString() ?? "null";
            //    Console.WriteLine($"{x,10} : {address.ToString("X12")} : {value}");
            //}
        }
    }
}