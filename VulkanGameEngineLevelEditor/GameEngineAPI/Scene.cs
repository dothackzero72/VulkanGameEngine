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
            MeshTransform = mat4.Identity;
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
            Projection = mat4.Identity;
            View = mat4.Identity;
            CameraPosition = new vec3(0.0f);
        }
    };

    public unsafe class Scene
    {
        Vk vk = Vk.GetApi();
        SceneDataBuffer sceneProperties;
        OrthographicCamera orthographicCamera;
        public RenderPass3D rendererPass3D { get; set; } = new RenderPass3D();
        static readonly long startTime = DateTime.Now.Ticks;
        public List<Texture> textureList { get; set; } = new List<Texture>();
        public List<GameObject> GameObjectList { get; set; } = new List<GameObject>();

        public readonly Vertex3D[] vertices = new Vertex3D[]
{
            new Vertex3D(new (-0.5f, -0.5f, 0.0f), new (1.0f, 0.0f, 0.0f), new (1.0f, 0.0f)),
            new Vertex3D(new (0.5f, -0.5f, 0.0f), new (0.0f, 1.0f, 0.0f), new (0.0f, 0.0f)),
            new Vertex3D(new (0.5f, 0.5f, 0.0f), new (0.0f, 0.0f, 1.0f), new (0.0f, 1.0f)),
            new Vertex3D(new (-0.5f, 0.5f, 0.0f), new (1.0f, 1.0f, 1.0f), new (1.0f, 1.0f)),

            new Vertex3D(new (-0.5f, -0.5f, -0.5f), new (1.0f, 0.0f, 0.0f), new (1.0f, 0.0f)),
            new Vertex3D(new (0.5f, -0.5f, -0.5f), new (0.0f, 1.0f, 0.0f), new (0.0f, 0.0f)),
            new Vertex3D(new (0.5f, 0.5f, -0.5f), new (0.0f, 0.0f, 1.0f), new (0.0f, 1.0f)),
            new Vertex3D(new (-0.5f, 0.5f, -0.5f), new (1.0f, 1.0f, 1.0f), new (1.0f, 1.0f))
};

        public readonly uint[] indices = new uint[]
        {
            0, 1, 2, 2, 3, 0,
            4, 5, 6, 6, 7, 4
        };

        public Scene()
        {
        }

        public void StartUp()
        {
            MemoryManager.StartUp(30);

            textureList.Add(Texture.CreateTexture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\awesomeface.png", Format.R8G8B8A8Unorm, TextureTypeEnum.kType_DiffuseTextureMap));
            GameObjectList.Add(GameObject.CreateGameObject("gameObject"));
            var meshRenderer = MeshRenderer3DComponent.CreateRenderMesh3DComponent("asdfads", vertices, indices, 0);
            GameObjectList.First().AddComponent(meshRenderer);

            MemoryManager.ViewMemoryMap();

            var a = new vec2((float)VulkanRenderer.swapChain.swapchainExtent.Width, (float)VulkanRenderer.swapChain.swapchainExtent.Height);
            var b = new vec3(0.0f, 0.0f, 5.0f);
            orthographicCamera = new OrthographicCamera(a, b);
            rendererPass3D.Create3dRenderPass();
        }

        public void Update(float deltaTime)
        {
            if (VulkanRenderer.RebuildRendererFlag)
            {
                UpdateRenderPasses();
            }

            CommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
            foreach (var gameObject in GameObjectList)
            {
                gameObject.BufferUpdate(commandBuffer, deltaTime);
            }
            VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);

            sceneProperties = orthographicCamera.Update(sceneProperties);
        }

        public void UpdateRenderPasses()
        {
        }

        public void DrawFrame()
        {
            List<CommandBuffer> commandBufferList = new List<CommandBuffer>();
            VulkanRenderer.StartFrame();
            commandBufferList.Add(rendererPass3D.Draw(GameObjectList, sceneProperties));
            VulkanRenderer.EndFrame(commandBufferList);
        }
    }
}
