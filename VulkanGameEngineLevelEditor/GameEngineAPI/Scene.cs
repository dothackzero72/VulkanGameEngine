using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class Scene
    {
        private Texture texture;
        private Mesh2D mesh;
        private FrameBufferRenderPass renderPass;

        public Scene()
        {
            // Commented to avoid cross-thread issues
            // texture = new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\awesomeface.png", VkFormat.VK_FORMAT_R8G8B8A8_SRGB, TextureTypeEnum.kType_DiffuseTextureMap);
            // mesh = new Mesh2D();
            // renderPass = new FrameBufferRenderPass();
        }

        public void StartUp()
        {
            // Ensure this method is called on the UI thread
            texture = new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\awesomeface.png", VkFormat.VK_FORMAT_R8G8B8A8_SRGB, TextureTypeEnum.kType_DiffuseTextureMap);
            mesh = new Mesh2D();
            renderPass = new FrameBufferRenderPass();

            BuildRenderPasses();
        }

        // The rest remains unchanged
        public void Update(float deltaTime)
        {
            if (VulkanRenderer.RebuildRendererFlag)
            {
                UpdateRenderPasses();
            }
        }

        public void BuildRenderPasses()
        {
            renderPass.BuildRenderPass(texture);
        }
        public void UpdateRenderPasses()
        {
        }

        public void Draw()
        {
            List<VkCommandBuffer> CommandBufferSubmitList = new List<VkCommandBuffer>();
            VulkanRenderer.StartFrame();
            renderPass.Draw();
            VulkanRenderer.EndFrame(CommandBufferSubmitList);
        }

        public void Destroy()
        {

        }
    }
}