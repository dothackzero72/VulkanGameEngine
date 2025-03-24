using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineGameObjectScripts.Input;
using VulkanGameEngineLevelEditor.Components;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{


    public unsafe class Scene
    {
        Vk vk = Vk.GetApi();
        SceneDataBuffer sceneProperties;
        OrthographicCamera orthographicCamera;

        static readonly long startTime = DateTime.Now.Ticks;
        public List<Texture> textureList { get; set; } = new List<Texture>();
        public List<GameObject> GameObjectList { get; set; } = new List<GameObject>();
        JsonRenderPass<Vertex3D> renderPass3D { get; set; } = new JsonRenderPass<Vertex3D>();
        Level2DRenderer level2DRenderer { get; set; }
        FrameBufferRenderPass frameBufferRenderPass { get; set; } = new FrameBufferRenderPass();
        public ListPtr<VkCommandBuffer> commandBufferList = new ListPtr<VkCommandBuffer>();
        public void StartUp()
        {
            var res = new vec2((float)VulkanRenderer.SwapChain.SwapChainResolution.width, (float)VulkanRenderer.SwapChain.SwapChainResolution.height);
            var pos = new vec3(0.0f, 0.0f, 5.0f);
            orthographicCamera = new OrthographicCamera(res, pos);

            textureList.Add(new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\awesomeface.png", VkFormat.VK_FORMAT_R8G8B8A8_UNORM, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_DiffuseTextureMap, false));
            textureList.Add(new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\container2.png", VkFormat.VK_FORMAT_R8G8B8A8_UNORM, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_DiffuseTextureMap, false));

            //GameObjectList.Add(new GameObject("object1", new List<ComponentTypeEnum>()
            //{
            //    ComponentTypeEnum.kGameObjectTransform2DComponent,
            //    ComponentTypeEnum.kInputComponent,
            //    ComponentTypeEnum.kRenderMesh2DComponent
            //}));
            //GameObjectList.Add(new GameObject("object2", new List<ComponentTypeEnum>()
            //{
            //    ComponentTypeEnum.kGameObjectTransform2DComponent,
            //    ComponentTypeEnum.kInputComponent,
            //    ComponentTypeEnum.kRenderMesh2DComponent
            //}));

            // MemoryManager.ViewMemoryMap();
            // renderPass3D.CreateJsonRenderPass(ConstConfig.Default2DRenderPass, new ivec2((int)VulkanRenderer.SwapChain.SwapChainResolution.width, (int)VulkanRenderer.SwapChain.SwapChainResolution.height));
            level2DRenderer = new Level2DRenderer("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\RenderPass\\Default2DRenderPass.json", new ivec2((int)VulkanRenderer.SwapChain.SwapChainResolution.width, (int)VulkanRenderer.SwapChain.SwapChainResolution.height));
            level2DRenderer.StartLeveleRenderer();

            GPUImport<NullVertex> imports = new GPUImport<NullVertex>()
            {
                TextureList = new List<Texture>() { level2DRenderer.RenderedColorTextureList[0] },
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

        public void Update()
        {
            VkCommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
            foreach (var gameObject in GameObjectList)
            {
                gameObject.Update(commandBuffer, startTime);
            }
            VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);

            orthographicCamera.Update(ref sceneProperties);
        }

        public void DrawFrame()
        {
            uint imageIndex = VulkanRenderer.ImageIndex;
            uint commandIndex = VulkanRenderer.CommandIndex;
            bool rebuildRendererFlag = VulkanRenderer.RebuildRendererFlag;

            VulkanRenderer.StartFrame();
            //GameEngineImport.DLL_Renderer_StartFrame(VulkanRenderer.device, VulkanRenderer.SwapChain.Swapchain, VulkanRenderer.InFlightFences.Ptr, VulkanRenderer.AcquireImageSemaphores.Ptr, &imageIndex, &commandIndex, &rebuildRendererFlag);
            commandBufferList.Add(level2DRenderer.Draw(GameObjectList, sceneProperties));
            commandBufferList.Add(frameBufferRenderPass.Draw());
            //GameEngineImport.DLL_Renderer_EndFrame(VulkanRenderer.SwapChain.Swapchain, VulkanRenderer.AcquireImageSemaphores.Ptr, VulkanRenderer.PresentImageSemaphores.Ptr, VulkanRenderer.InFlightFences.Ptr, VulkanRenderer.graphicsQueue, VulkanRenderer.presentQueue, commandIndex, imageIndex, commandBufferList.Ptr, commandBufferList.UCount, &rebuildRendererFlag);
           VulkanRenderer.EndFrame(commandBufferList);
            commandBufferList.Clear();
        }

        public void Destroy()
        {
            VulkanRenderer.Destroy();
        }
    }
}

