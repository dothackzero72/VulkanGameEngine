using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.Components;
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
        Level2D level2DRenderer { get; set; }
        FrameBufferRenderPass frameBufferRenderPass { get; set; } = new FrameBufferRenderPass();
        public ListPtr<VkCommandBuffer> commandBufferList = new ListPtr<VkCommandBuffer>();
        public void StartUp()
        {
            var res = new vec2((float)RenderSystem.SwapChainResolution.width, (float)RenderSystem.SwapChainResolution.height);
            var pos = new vec2(0.0f, 0.0f);
            orthographicCamera = new OrthographicCamera2D(res, pos);

            level2DRenderer = new Level2D("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\RenderPass\\Default2DRenderPass.json", new ivec2((int)RenderSystem.SwapChainResolution.width, (int)RenderSystem.SwapChainResolution.height));

            GPUImport<NullVertex> imports = new GPUImport<NullVertex>()
            {
                TextureList = new List<Texture>() { level2DRenderer.RenderedLevelTexture },
                MaterialList = new List<Material>(),
                MeshList = new List<Mesh<NullVertex>>()
            };

            frameBufferRenderPass.BuildRenderPass("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\RenderPass\\FrameBufferRenderPass.json", imports);
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
            VkCommandBuffer commandBuffer = RenderSystem.BeginSingleUseCommandBuffer();
            foreach (var gameObject in GameObjectList)
            {
                gameObject.Update(commandBuffer, startTime);
            }
            RenderSystem.EndSingleUseCommandBuffer(commandBuffer);

            orthographicCamera.Update(ref sceneProperties);
            level2DRenderer.Update(deltaTime);
        }

        public void DrawFrame()
        {
            uint imageIndex = RenderSystem.ImageIndex;
            uint commandIndex = RenderSystem.CommandIndex;
            bool rebuildRendererFlag = RenderSystem.RebuildRendererFlag;

            RenderSystem.StartFrame();
            //            GameEngineImport.DLL_Renderer_StartFrame(VulkanRenderer.device, VulkanRenderer.SwapChain.Swapchain, VulkanRenderer.InFlightFences.Ptr, VulkanRenderer.AcquireImageSemaphores.Ptr, &imageIndex, &commandIndex, &rebuildRendererFlag);
            commandBufferList.Add(level2DRenderer.Draw(sceneProperties));
            commandBufferList.Add(frameBufferRenderPass.Draw());
            //          GameEngineImport.DLL_Renderer_EndFrame(VulkanRenderer.SwapChain.Swapchain, VulkanRenderer.AcquireImageSemaphores.Ptr, VulkanRenderer.PresentImageSemaphores.Ptr, VulkanRenderer.InFlightFences.Ptr, VulkanRenderer.graphicsQueue, VulkanRenderer.presentQueue, commandIndex, imageIndex, commandBufferList.Ptr, commandBufferList.UCount, &rebuildRendererFlag);
            RenderSystem.EndFrame(commandBufferList);
            commandBufferList.Clear();
        }

        public void SaveLevel()
        {
            level2DRenderer.SaveLevel();
        }

        public void Destroy()
        {
          //  RenderSystem.Destroy();
        }
    }
}