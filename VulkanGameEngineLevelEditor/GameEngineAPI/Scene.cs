using ClassLibrary1;
using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VulkanGameEngineGameObjectScripts;
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
        public void StartUp()
        {
            //List<string> scriptList = new List<string>();
            //scriptList.Add(@"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\VulkanGameEngineLevelEditor\GameEngineAPI\TestScriptComponent.cs");
            //ScriptCompiler.CompileScript(scriptList);

            MemoryManager.StartUp(30);

            var res = new vec2((float)VulkanRenderer.swapChain.swapchainExtent.Width, (float)VulkanRenderer.swapChain.swapchainExtent.Height);
            var pos = new vec3(0.0f, 0.0f, 5.0f);
            orthographicCamera = new OrthographicCamera(res, pos);

            textureList.Add(Texture.CreateTexture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\awesomeface.png", Format.R8G8B8A8Unorm, TextureTypeEnum.kType_DiffuseTextureMap));
            textureList.Add(Texture.CreateTexture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\container2.png", Format.R8G8B8A8Unorm, TextureTypeEnum.kType_DiffuseTextureMap));

            GameObjectList.Add(MemoryManager.CreateGameObject("object1", new List<ComponentTypeEnum>() { ComponentTypeEnum.kGameObjectTransform2DComponent, ComponentTypeEnum.kRenderMesh2DComponent }));
            GameObjectList.Add(MemoryManager.CreateGameObject("object2", new List<ComponentTypeEnum>() { ComponentTypeEnum.kGameObjectTransform2DComponent, ComponentTypeEnum.kRenderMesh2DComponent }));

            MemoryManager.ViewMemoryMap();
            renderPass3D.CreateJsonRenderPass(ConstConfig.Default2DRenderPass, new ivec2((int)VulkanRenderer.swapChain.swapchainExtent.Width, (int)VulkanRenderer.swapChain.swapchainExtent.Height));
        }

        public void Input(KeyEventArgs e)
        {
            //var inputObjects = MemoryManager.InputComponentMemoryPool.ViewMemoryPool();
            //foreach (var obj in inputObjects)
            //{

            //}
        }

        public void Update()
        {
            var a = GameObjectList[0].GameObjectComponentList[0] as Transform2DComponent;
            a.GameObjectPosition.x += 0.01f;

            CommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
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
            List<CommandBuffer> commandBufferList = new List<CommandBuffer>();
            VulkanRenderer.StartFrame();

            commandBufferList.Add(renderPass3D.Draw(GameObjectList, sceneProperties));
            VulkanRenderer.EndFrame(commandBufferList);
        }
    }
}

