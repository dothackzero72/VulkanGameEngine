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
    public struct MeshProperitiesStruct
    {
        uint MeshIndex = 0;
        uint MaterialIndex = 0;
        mat4 MeshTransform;

        public MeshProperitiesStruct()
        {
            MeshTransform = new mat4();
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct SceneDataBuffer
    {
        public mat4 Projection;
        public mat4 View;
        public vec3 CameraPosition;

        public SceneDataBuffer()
        {
            Projection = new mat4();
            View = new mat4();
            CameraPosition = new vec3(0.0f);
        }
    };



    public unsafe class Scene
    {
        Vk vk = Vk.GetApi();
        public RendererPass3D RendererPass3D;
        static readonly long startTime = DateTime.Now.Ticks;
        public Texture texture;
    
        public Scene()
        {
        }

        public void StartUp()
        {
            texture = new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\awesomeface.png", Format.R8G8B8A8Unorm, TextureTypeEnum.kType_DiffuseTextureMap);

            RendererPass3D = new RendererPass3D();
            RendererPass3D.Create3dRenderPass();
        }

        public void Update()
        {
            RendererPass3D.UpdateUniformBuffer(startTime);

        }

        public void DrawFrame()
        {
            List<CommandBuffer> commandBufferList = new List<CommandBuffer>();
            SilkVulkanRenderer.StartFrame();
            commandBufferList.Add(RendererPass3D.Draw());
            SilkVulkanRenderer.EndFrame(commandBufferList);
        }
    }
}

