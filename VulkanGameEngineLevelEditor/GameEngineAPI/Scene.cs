using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineGameObjectScripts.Component;
using VulkanGameEngineGameObjectScripts.Input;
using VulkanGameEngineLevelEditor.RenderPassEditor;
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
       // JsonRenderPass renderPass3D { get; set; } = new JsonRenderPass();
        FrameBufferRenderPass frameBufferRenderPass { get; set; } = new FrameBufferRenderPass();
        public void StartUp()
        {
            MemoryManager.StartUp(30);

            var res = new vec2((float)VulkanRenderer.SwapChain.SwapChainResolution.width, (float)VulkanRenderer.SwapChain.SwapChainResolution.height);
            var pos = new vec3(0.0f, 0.0f, 5.0f);
            orthographicCamera = new OrthographicCamera(res, pos);

            textureList.Add(Texture.CreateTexture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\awesomeface.png", VkFormat.VK_FORMAT_R8G8B8A8_UNORM, TextureTypeEnum.kType_DiffuseTextureMap));
            textureList.Add(Texture.CreateTexture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\container2.png", VkFormat.VK_FORMAT_R8G8B8A8_UNORM, TextureTypeEnum.kType_DiffuseTextureMap));

            GameObjectList.Add(MemoryManager.CreateGameObject("object1", new List<ComponentTypeEnum>()
            {
                ComponentTypeEnum.kGameObjectTransform2DComponent,
                ComponentTypeEnum.kInputComponent,
                ComponentTypeEnum.kRenderMesh2DComponent
            }));
            GameObjectList.Add(MemoryManager.CreateGameObject("object2", new List<ComponentTypeEnum>()
            {
                ComponentTypeEnum.kGameObjectTransform2DComponent,
                ComponentTypeEnum.kInputComponent,
                ComponentTypeEnum.kRenderMesh2DComponent
            }));

            // MemoryManager.ViewMemoryMap();
           // renderPass3D.CreateJsonRenderPass(ConstConfig.Default2DRenderPass, new ivec2((int)VulkanRenderer.SwapChain.SwapChainResolution.width, (int)VulkanRenderer.SwapChain.SwapChainResolution.height));
            frameBufferRenderPass.BuildRenderPass(textureList[0]);
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
                gameObject.Update(startTime);
                gameObject.BufferUpdate(commandBuffer, startTime);
            }
            VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);

            orthographicCamera.Update(ref sceneProperties);
        }

        public void DrawFrame()
        {
            // [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Renderer_StartFrame(VkDevice device, VkSwapchainKHR swapChain, VkFence* fenceList, VkSemaphore* acquireImageSemaphoreList, uint pImageIndex, uint pCommandIndex, ref bool pRebuildRendererFlag);
            // [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult DLL_Renderer_EndFrame(VkSwapchainKHR swapChain, VkSemaphore* acquireImageSemaphoreList, VkSemaphore* presentImageSemaphoreList, VkFence* fenceList, VkQueue graphicsQueue, VkQueue presentQueue, uint commandIndex, uint imageIndex, VkCommandBuffer* pCommandBufferSubmitList, uint commandBufferCount, ref bool rebuildRendererFlag);

            List<VkCommandBuffer> commandBufferList = new List<VkCommandBuffer>();
            uint imageIndex = VulkanRenderer.ImageIndex;
            uint commandIndex = VulkanRenderer.CommandIndex;
            bool rebuildRendererFlag = VulkanRenderer.RebuildRendererFlag;

            fixed (VkFence* fenceList = VulkanRenderer.InFlightFences.ToArray())
            fixed (VkSemaphore* acquireImageSemaphores = VulkanRenderer.AcquireImageSemaphores.ToArray())
            fixed (VkSemaphore* presentImageSemaphores = VulkanRenderer.PresentImageSemaphores.ToArray())
            fixed(VkCommandBuffer* commandBuffers = commandBufferList.ToArray())
            {
                GameEngineImport.DLL_Renderer_StartFrame(VulkanRenderer.device, VulkanRenderer.SwapChain.Swapchain, fenceList, acquireImageSemaphores, &imageIndex, &commandIndex,  &rebuildRendererFlag);
                // commandBufferList.Add(renderPass3D.Draw(GameObjectList, sceneProperties));
                commandBufferList.Add(frameBufferRenderPass.Draw());
                GameEngineImport.DLL_Renderer_EndFrame(VulkanRenderer.SwapChain.Swapchain, acquireImageSemaphores, presentImageSemaphores, fenceList, VulkanRenderer.graphicsQueue, VulkanRenderer.presentQueue, commandIndex, imageIndex, commandBuffers, commandBufferList.UCount(), &rebuildRendererFlag);
               VulkanRenderer.EndFrame(commandBufferList);
            }
        }
    }
}

