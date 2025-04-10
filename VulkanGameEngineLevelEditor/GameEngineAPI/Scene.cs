using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineGameObjectScripts.Input;
using VulkanGameEngineLevelEditor.Components;
using VulkanGameEngineLevelEditor.RenderPassEditor;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{


    public unsafe class Scene
    {
        Vk vk = Vk.GetApi();
        SceneDataBuffer sceneProperties;
        OrthographicCamera2D orthographicCamera;

        static readonly long startTime = DateTime.Now.Ticks;
        public List<Texture> textureList { get; set; } = new List<Texture>();
        public List<GameObject> GameObjectList { get; set; } = new List<GameObject>();
        JsonRenderPass<Vertex3D> renderPass3D { get; set; } = new JsonRenderPass<Vertex3D>();
        Level2D level2D { get; set; }
        FrameBufferRenderPass frameBufferRenderPass { get; set; } = new FrameBufferRenderPass();
        public ListPtr<VkCommandBuffer> commandBufferList = new ListPtr<VkCommandBuffer>();
        public void StartUp()
        {
            var res = new vec2((float)VulkanRenderer.SwapChain.SwapChainResolution.width, (float)VulkanRenderer.SwapChain.SwapChainResolution.height);
            var pos = new vec2(0.0f, 0.0f);
            orthographicCamera = new OrthographicCamera2D(res, pos);

            level2D = new Level2D("", new ivec2((int)VulkanRenderer.SwapChain.SwapChainResolution.width, (int)VulkanRenderer.SwapChain.SwapChainResolution.height));

            GPUImport<NullVertex> imports = new GPUImport<NullVertex>()
            {
                TextureList = new List<Texture>() { level2D.LevelRenderer.RenderedColorTextureList.First() },
                MaterialList = new List<Material>(),
                MeshList = new List<Mesh<NullVertex>>()
            };

            frameBufferRenderPass.BuildRenderPass($@"{ConstConfig.RenderPassBasePath}\\FrameBufferRenderPass.json", imports);
        }

        public void Input(KeyEventArgs e)
        {
            var inputComponents = GameObject.GetComponentFromGameObjects(GameObjectList, ComponentTypeEnum.kInputComponent);
            foreach (var obj in inputComponents)
            {
                (obj as InputComponent).Input((KeyBoardKeys)e.KeyCode, 0.0f);
            }
        }

        public void Update(float deltaTime)
        {
            VkCommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
            foreach (var gameObject in GameObjectList)
            {
                gameObject.Update(commandBuffer, startTime);
            }
            VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);

            orthographicCamera.Update(ref sceneProperties);
            level2D.Update(deltaTime);
        }

        public void DrawFrame()
        {
            uint imageIndex = VulkanRenderer.ImageIndex;
            uint commandIndex = VulkanRenderer.CommandIndex;
            bool rebuildRendererFlag = VulkanRenderer.RebuildRendererFlag;

            VulkanRenderer.StartFrame();
//            GameEngineImport.DLL_Renderer_StartFrame(VulkanRenderer.device, VulkanRenderer.SwapChain.Swapchain, VulkanRenderer.InFlightFences.Ptr, VulkanRenderer.AcquireImageSemaphores.Ptr, &imageIndex, &commandIndex, &rebuildRendererFlag);
            commandBufferList.Add(level2D.Draw(sceneProperties));
            commandBufferList.Add(frameBufferRenderPass.Draw());
  //          GameEngineImport.DLL_Renderer_EndFrame(VulkanRenderer.SwapChain.Swapchain, VulkanRenderer.AcquireImageSemaphores.Ptr, VulkanRenderer.PresentImageSemaphores.Ptr, VulkanRenderer.InFlightFences.Ptr, VulkanRenderer.graphicsQueue, VulkanRenderer.presentQueue, commandIndex, imageIndex, commandBufferList.Ptr, commandBufferList.UCount, &rebuildRendererFlag);
            VulkanRenderer.EndFrame(commandBufferList);
            commandBufferList.Clear();
        }

        public void SaveLevel()
        {
            level2D.SaveLevel();
        }

        public void Destroy()
        {
            VulkanRenderer.Destroy();
        }
    }
}
