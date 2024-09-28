using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using static VulkanGameEngineLevelEditor.VulkanAPI;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class Scene
    {
        public  Texture texture;
       // private Mesh2D mesh;
        private FrameBufferRenderPass renderPass;
       // public Texture texture;
        public TestRenderPass testRenderPass2D { get; set; }
        //public RenderPass2D renderPass2D { get; set; } = new RenderPass2D();
        private SceneDataBuffer sceneProperties;
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
            texture = new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\awesomeface.png", VkFormat.VK_FORMAT_R8G8B8A8_UNORM, TextureTypeEnum.kType_DiffuseTextureMap);
          //  mesh = new Mesh2D();
            renderPass = new FrameBufferRenderPass();
            testRenderPass2D = new TestRenderPass();
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
          //  renderPass2D.BuildRenderPass(mesh);
           testRenderPass2D.BuildRenderPass(texture);
            //renderPass.BuildRenderPass(texture);
        }
        public void UpdateRenderPasses()
        {
        }

        public void Draw()
        {
            List<VkCommandBuffer> CommandBufferSubmitList = new List<VkCommandBuffer>();
            VulkanRenderer.StartFrame();
           // renderPass2D.Draw(mesh);
            testRenderPass2D.Draw();
           // renderPass.Draw();
            VulkanRenderer.EndFrame(CommandBufferSubmitList);
            //BakeColorTexture("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/asdfa.bmp", texture);
        }

        [DllImport("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DLL_stbi_write_bmp(string filename, int w, int h, int comp, void* data);
        public unsafe void BakeColorTexture(string filename, Texture texture)
        {
            //std::shared_ptr<Texture2D> BakeTexture = std::make_shared<Texture2D>(Texture2D(Pixel(255, 0, 0), glm::vec2(1280,720), VkFormat::VK_FORMAT_R8G8B8A8_UNORM, TextureTypeEnum::kTextureAtlus));
            var pixel = new Pixel(0xFF, 0x00, 0x00, 0xFF);

            BakeTexture bakeTexture = new BakeTexture(pixel, new GlmSharp.ivec2(texture.Width, texture.Width), VkFormat.VK_FORMAT_R8G8B8A8_UNORM);

            VkCommandBuffer commandBuffer = VulkanRenderer.BeginCommandBuffer();

            bakeTexture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
            texture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL);

            VkImageCopy copyImage = new VkImageCopy();
            copyImage.srcSubresource.aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT;
            copyImage.srcSubresource.layerCount = 1;

            copyImage.dstSubresource.aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT;
            copyImage.dstSubresource.layerCount = 1;

            copyImage.dstOffset.X = 0;
            copyImage.dstOffset.Y = 0;
            copyImage.dstOffset.Z = 0;

            copyImage.extent.Width = (uint)texture.Width;
            copyImage.extent.Height = (uint)texture.Height;
            copyImage.extent.Depth = 1;

            vkCmdCopyImage(commandBuffer, texture.Image, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL, bakeTexture.Image, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &copyImage);

            bakeTexture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_GENERAL);
            texture.UpdateImageLayout(commandBuffer, VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR);
            VulkanRenderer.EndCommandBuffer(commandBuffer);

            VkImageSubresource subResource = new VkImageSubresource { aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT, mipLevel = 0, arrayLayer = 0 };
            VkSubresourceLayout subResourceLayout;
            vkGetImageSubresourceLayout(VulkanRenderer.Device, bakeTexture.Image, &subResource, &subResourceLayout);

            void* data;
            vkMapMemory(VulkanRenderer.Device, bakeTexture.Memory, 0, VulkanConsts.VK_WHOLE_SIZE, 0, (void**)&data);

            DLL_stbi_write_bmp(filename, bakeTexture.Width, bakeTexture.Height, 4, data);
        }


        public void Destroy()
        {

        }
    }
}