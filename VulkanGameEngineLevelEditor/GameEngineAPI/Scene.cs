using GlmSharp;
using Microsoft.VisualBasic;
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
        JsonRenderPass renderPass3D { get; set; } = new JsonRenderPass();
        FrameBufferRenderPass frameBufferRenderPass { get; set; } = new FrameBufferRenderPass();
        public ListPtr<VkCommandBuffer> commandBufferList = new ListPtr<VkCommandBuffer>();
        public void StartUp()
        {
            MemoryManager.StartUp(30);

            var res = new vec2((float)VulkanRenderer.SwapChain.SwapChainResolution.width, (float)VulkanRenderer.SwapChain.SwapChainResolution.height);
            var pos = new vec3(0.0f, 0.0f, 5.0f);
            orthographicCamera = new OrthographicCamera(res, pos);

            textureList.Add(new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\awesomeface.png", VkFormat.VK_FORMAT_R8G8B8A8_UNORM, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_DiffuseTextureMap, false));
            textureList.Add(new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\container2.png", VkFormat.VK_FORMAT_R8G8B8A8_UNORM, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_DiffuseTextureMap, false));

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
            frameBufferRenderPass.BuildRenderPass("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\RenderPass\\FrameBufferRenderPass.json", textureList[0]);
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
            uint imageIndex = VulkanRenderer.ImageIndex;
            uint commandIndex = VulkanRenderer.CommandIndex;
            bool rebuildRendererFlag = VulkanRenderer.RebuildRendererFlag;

            VulkanRenderer.StartFrame();
           // GameEngineImport.DLL_Renderer_StartFrame(VulkanRenderer.device, VulkanRenderer.SwapChain.Swapchain, VulkanRenderer.InFlightFences.Ptr, VulkanRenderer.AcquireImageSemaphores.Ptr, &imageIndex, &commandIndex, &rebuildRendererFlag);
          //  commandBufferList.Add(renderPass3D.Draw(GameObjectList, sceneProperties));
            commandBufferList.Add(frameBufferRenderPass.Draw());
            //GameEngineImport.DLL_Renderer_EndFrame(VulkanRenderer.SwapChain.Swapchain, VulkanRenderer.AcquireImageSemaphores.Ptr, VulkanRenderer.PresentImageSemaphores.Ptr, VulkanRenderer.InFlightFences.Ptr, VulkanRenderer.graphicsQueue, VulkanRenderer.presentQueue, commandIndex, imageIndex, commandBufferList.Ptr, commandBufferList.UCount, &rebuildRendererFlag);
           VulkanRenderer.EndFrame(commandBufferList);
            //commandBufferList.Clear();
        }

        public void Destroy()
        {
            VulkanRenderer.Destroy();
        }
    }
}

