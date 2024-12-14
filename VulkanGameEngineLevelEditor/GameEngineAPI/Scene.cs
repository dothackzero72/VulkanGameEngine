using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public void StartUp()
        {
            MemoryManager.StartUp(30);

            var res = new vec2((float)VulkanRenderer.swapChain.swapchainExtent.Width, (float)VulkanRenderer.swapChain.swapchainExtent.Height);
            var pos = new vec3(0.0f, 0.0f, 5.0f);
            orthographicCamera = new OrthographicCamera(res, pos);

            textureList.Add(Texture.CreateTexture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\awesomeface.png", Format.R8G8B8A8Unorm, TextureTypeEnum.kType_DiffuseTextureMap));
            textureList.Add(Texture.CreateTexture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\container2.png", Format.R8G8B8A8Unorm, TextureTypeEnum.kType_DiffuseTextureMap));

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
            renderPass3D.CreateJsonRenderPass(ConstConfig.Default2DRenderPass, new ivec2((int)VulkanRenderer.swapChain.swapchainExtent.Width, (int)VulkanRenderer.swapChain.swapchainExtent.Height));
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
            CommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
            foreach (var gameObject in GameObjectList)
            {
                gameObject.Update(startTime);
                gameObject.BufferUpdate(commandBuffer.Handle, startTime);
            }
            VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);

            orthographicCamera.Update(ref sceneProperties);
        }

        public void DrawFrame()
        {
            List<CommandBuffer> commandBufferList = new List<CommandBuffer>();
            VulkanRenderer.StartFrame();
            commandBufferList.Add(renderPass3D.Draw(GameObjectList, sceneProperties));
            commandBufferList.Add(frameBufferRenderPass.Draw());
            VulkanRenderer.EndFrame(commandBufferList);
        }
    }
}

