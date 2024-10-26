using Newtonsoft.Json.Linq;
using System;
using System.Buffers;
using System.Collections.Generic;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class MemoryManager
    {
        public static MemoryPool<GameObject> GameObjectMemoryPool = new MemoryPool<GameObject>();
        public static MemoryPool<RenderMesh2DComponent> RenderMesh2DComponentMemoryPool = new MemoryPool<RenderMesh2DComponent>();
        public static MemoryPool<Texture> TextureMemoryPool = new MemoryPool<Texture>();

        public static void StartUp(uint estObjectCount)
        {
            GameObjectMemoryPool.CreateMemoryPool(estObjectCount);
            RenderMesh2DComponentMemoryPool.CreateMemoryPool(estObjectCount);
            TextureMemoryPool.CreateMemoryPool(estObjectCount);
        }

        public static GameObject AllocateGameObject()
        {
            return GameObjectMemoryPool.AllocateMemoryLocation();
        }

        public static RenderMesh2DComponent AllocateGameRenderMesh2DComponent()
        {
            return RenderMesh2DComponentMemoryPool.AllocateMemoryLocation();
        }

        public static Texture AllocateTexture()
        {
            return TextureMemoryPool.AllocateMemoryLocation();
        }

        public static void ViewMemoryMap()
        {
            var gameObjectMemoryList = GameObjectMemoryPool.ViewMemoryPool();
            var renderMeshMemoryList = RenderMesh2DComponentMemoryPool.ViewMemoryPool();
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
                var render2DMemoryListRef = renderMeshMemoryList[x];
                RenderMesh2DComponent* render2DPtr = &render2DMemoryListRef;

                IntPtr address = (IntPtr)render2DPtr + (sizeof(RenderMesh2DComponent) * x);

                string value = renderMeshMemoryList[x]?.ToString() ?? "null";
                Console.WriteLine($"{x,10} : {address.ToString("X12")} : {value}");
            }

            Console.WriteLine();

            Console.WriteLine($"Memory Map of RenderMesh2DComponent:");
            Console.WriteLine("{0,20} {1,15}", "Index", "Value");
            for (int x = 0; x < RenderMesh2DComponentMemoryPool.ObjectCount; x++)
            {
                var render2DMemoryListRef = renderMeshMemoryList[x];
                RenderMesh2DComponent* render2DPtr = &render2DMemoryListRef;

                IntPtr address = (IntPtr)render2DPtr + (sizeof(RenderMesh2DComponent) * x);

                string value = renderMeshMemoryList[x]?.ToString() ?? "null";
                Console.WriteLine($"{x,10} : {address.ToString("X12")} : {value}");
            }

            Console.WriteLine();
        }
    }
}