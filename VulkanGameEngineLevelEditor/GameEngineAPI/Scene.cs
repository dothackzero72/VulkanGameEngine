using GlmSharp;
using Silk.NET.Core.Native;
using Silk.NET.Maths;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Windowing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Vulkan;
using ImageLayout = Silk.NET.Vulkan.ImageLayout;
namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MeshProperitiesBuffer
    {
        public uint MaterialIndex = 0;
        public mat4 MeshTransform;

        public MeshProperitiesBuffer()
        {
            MeshTransform = new mat4();
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct SceneDataBuffer
    {
        public uint MeshIndex = 0;
        public mat4 Projection;
        public mat4 View;
        public vec3 CameraPosition;

        public SceneDataBuffer()
        {
            MeshIndex = 0;
            Projection = new mat4();
            View = new mat4();
            CameraPosition = new vec3(0.0f);
        }
    };

    public unsafe class Scene
    {
        Vk vk = Vk.GetApi();
        SceneDataBuffer sceneProperties;
        OrthographicCamera orthographicCamera;
        public RendererPass3D RendererPass3D { get; set; }
        static readonly long startTime = DateTime.Now.Ticks;
        public List<Texture> textureList { get; set; } = new List<Texture>();
        public List<GameObject> GameObjectList { get; set; } = new List<GameObject>();
    
        public Scene()
        {
        }

        public void StartUp()
        {
            MemoryManager.StartUp(30);

            textureList.Add(Texture.CreateTexture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\awesomeface.png", Format.R8G8B8A8Unorm, TextureTypeEnum.kType_DiffuseTextureMap));
            GameObjectList.Add(GameObject.CreateGameObject("gameObject"));
            var meshRenderer = RenderMesh2DComponent.CreateRenderMesh2DComponent("asdfads", 0);
            GameObjectList.First().AddComponent(meshRenderer);

            MemoryManager.ViewMemoryMap();

            RendererPass3D = new RendererPass3D();
            RendererPass3D.Create3dRenderPass();
        }

        public void Update(float deltaTime)
        {
            if(VulkanRenderer.RebuildRendererFlag)
            {
                UpdateRenderPasses();
            }

            CommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
            foreach(var gameObject in GameObjectList)
            {
                gameObject.BufferUpdate(commandBuffer, deltaTime);
            }
            RendererPass3D.UpdateUniformBuffer(startTime);

            orthographicCamera.Update(sceneProperties);
        }

        public void UpdateRenderPasses()
        {
        }

        public void DrawFrame()
        {
            List<CommandBuffer> commandBufferList = new List<CommandBuffer>();
            VulkanRenderer.StartFrame();
            commandBufferList.Add(RendererPass3D.Draw(GameObjectList, sceneProperties));
            VulkanRenderer.EndFrame(commandBufferList);
        }
    }
}

