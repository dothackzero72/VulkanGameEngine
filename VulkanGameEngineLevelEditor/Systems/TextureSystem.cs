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
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.Systems
{
    public unsafe static class TextureSystem
    {
        public static Dictionary<Guid, Texture> TextureList { get; set; } = new Dictionary<Guid, Texture>();
        public static Dictionary<Guid, Texture> DepthTextureList { get; set; } = new Dictionary<Guid, Texture>();
        public static Dictionary<Guid, ListPtr<Texture>> RenderedTextureList { get; set; } = new Dictionary<Guid, ListPtr<Texture>>();
        public static Dictionary<uint, ListPtr<Texture>> InputTextureList { get; set; } = new Dictionary<uint, ListPtr<Texture>>();

        public static Guid LoadTexture(string texturePath)
        {
            if (texturePath.IsEmpty())
            {
                return new Guid();
            }

            string jsonContent = File.ReadAllText(texturePath);
            TextureJsonLoader textureJson = JsonConvert.DeserializeObject<TextureJsonLoader>(jsonContent);
            if (TextureList.ContainsKey(textureJson.TextureId))
            {
                return textureJson.TextureId;
            }

            TextureList[textureJson.TextureId] = Texture_LoadTexture(RenderSystem.renderer, texturePath);
            return textureJson.TextureId;
        }

        public static void GetTexturePropertiesBuffer(Texture texture, ref ListPtr<VkDescriptorImageInfo> textureDescriptorList)
        {
            VkDescriptorImageInfo textureDescriptor = new VkDescriptorImageInfo
            {
		        sampler = texture.textureSampler,
		        imageView = texture.textureView,
		        imageLayout = VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL
            };
            textureDescriptorList.Add(textureDescriptor);
        }

        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern Texture Texture_LoadTexture(GraphicsRenderer renderer, [MarshalAs(UnmanagedType.LPStr)] string jsonString);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void Texture_GetTexturePropertiesBuffer(ref Texture texture, ref VkDescriptorImageInfo* textureDescriptorList, size_t textureDesciptorListCount);
    }
}
