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
using VulkanGameEngineLevelEditor.RenderPassEditor;
using VulkanGameEngineLevelEditor.Vulkan;
using ImageLayout = Silk.NET.Vulkan.ImageLayout;
namespace VulkanGameEngineLevelEditor.GameEngineAPI
{

    [StructLayout(LayoutKind.Sequential)]
    public struct SceneDataBuffer
    {
        public uint MeshBufferIndex;
        public ulong buffer;
        public mat4 Projection;
        public mat4 View;
        public vec3 CameraPosition;

        public SceneDataBuffer()
        {
            MeshBufferIndex = uint.MaxValue;
            Projection = new mat4();
            View = new mat4();
            CameraPosition = new vec3(0.0f);
            buffer = 0;
        }
    };

    public unsafe class Scene
    {
        Vk vk = Vk.GetApi();
        SceneDataBuffer sceneProperties;
        OrthographicCamera orthographicCamera;
       
        static readonly long startTime = DateTime.Now.Ticks;
        public List<Texture> textureList { get; set; } = new List<Texture>();
        public List<GameObject> GameObjectList { get; set; } = new List<GameObject>();
        JsonRenderPass renderPass3D { get; set; } = new JsonRenderPass();

        public void StartUp()
        {
            MemoryManager.StartUp(30);

            var res = new vec2((float)VulkanRenderer.swapChain.swapchainExtent.Width, (float)VulkanRenderer.swapChain.swapchainExtent.Height);
            var pos = new vec3(0.0f, 0.0f, 5.0f);
            orthographicCamera = new OrthographicCamera(res, pos);

            textureList.Add(Texture.CreateTexture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\awesomeface.png", Format.R8G8B8A8Unorm, TextureTypeEnum.kType_DiffuseTextureMap));
            textureList.Add(Texture.CreateTexture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\container2.png", Format.R8G8B8A8Unorm, TextureTypeEnum.kType_DiffuseTextureMap));

            GameObjectList.Add(GameObject.CreateGameObject("object1", new List<ComponentTypeEnum>() { ComponentTypeEnum.kRenderMesh2DComponent }));
            GameObjectList.Add(GameObject.CreateGameObject("object2", new List<ComponentTypeEnum>() { ComponentTypeEnum.kRenderMesh2DComponent }));

            MemoryManager.ViewMemoryMap();
            renderPass3D.JsonCreateRenderPass(RenderPassEditorConsts.Default2DRenderPass, new ivec2((int)VulkanRenderer.swapChain.swapchainExtent.Width, (int)VulkanRenderer.swapChain.swapchainExtent.Height));
        }

        public void Update()
        {
            CommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
            foreach (var gameObject in GameObjectList)
            {
                gameObject.BufferUpdate(commandBuffer, startTime);
            }
            VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);

            orthographicCamera.Update(ref sceneProperties);
        }

        public void DrawFrame()
        {
            List<CommandBuffer> commandBufferList = new List<CommandBuffer>();
            VulkanRenderer.StartFrame();

            commandBufferList.Add(renderPass3D.Draw(GameObjectList, sceneProperties));
            VulkanRenderer.EndFrame(commandBufferList);
        }
    }
}

