using CSScripting;
using Newtonsoft.Json;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public struct TextureStruct
    {
        public Guid textureId { get; set; }
        public int width { get; set; } = 1;
        public int height { get; set; } = 1;
        public int depth { get; set; } = 1;
        public uint mipMapLevels { get; set; } = 1;
        public uint textureBufferIndex { get; set; } = 0;

        public VkImage textureImage { get; set; } = VulkanConst.VK_NULL_HANDLE;
        public VkDeviceMemory textureMemory { get; set; } = VulkanConst.VK_NULL_HANDLE;
        public VkImageView textureView { get; set; } = VulkanConst.VK_NULL_HANDLE;
        public VkSampler textureSampler { get; set; } = VulkanConst.VK_NULL_HANDLE;
        public VkDescriptorSet ImGuiDescriptorSet { get; set; } = VulkanConst.VK_NULL_HANDLE;

        public TextureUsageEnum textureUsage { get; set; } = TextureUsageEnum.kUse_Undefined;
        public TextureTypeEnum textureType { get; set; } = TextureTypeEnum.kType_UndefinedTexture;
        public VkFormat textureByteFormat { get; set; } = VkFormat.VK_FORMAT_UNDEFINED;
        public VkImageLayout textureImageLayout { get; set; } = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
        public VkSampleCountFlagBits sampleCount { get; set; } = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
        public ColorChannelUsed colorChannels { get; set; } = ColorChannelUsed.ChannelRGBA;

        public TextureStruct()
        {
        }
    };

    public static class TextureSystem
    {
        public static Dictionary<Guid, TextureStruct> TextureList { get; set; } = new Dictionary<Guid, TextureStruct>();
        public static Dictionary<Guid, TextureStruct> DepthTextureList { get; set; }
        public static Dictionary<Guid, Vector<TextureStruct>> RenderedTextureList { get; set; }
        public static Dictionary<uint, Vector<TextureStruct>> InputTextureList { get; set; }

        public static Guid LoadTexture(String texturePath)
        {
            if (texturePath.IsEmpty())
            {
                return new Guid();
            }

            string jsonContent = File.ReadAllText(texturePath);
            TextureJsonLoader textureJson = JsonConvert.DeserializeObject<TextureJsonLoader>(jsonContent);
            //if (TextureList.Where(x => x.Key == textureJson.textureId).Any())
            //{
            //    return textureJson.textureId;
            //}

           // TextureList[textureJson.TextureId] = GameEngineImport.Texture_LoadTexture(RenderSystem.ToStruct(), jsonContent);
            return textureJson.TextureId;
        }
    }
}
