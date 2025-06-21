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
    public static class TextureSystem
    {
        public static Dictionary<Guid, Texture> TextureList { get; } = new Dictionary<Guid, Texture>();
        public static Dictionary<Guid, Texture> DepthTextureList { get; }
        public static Dictionary<Guid, Vector<Texture>> RenderedTextureList { get; }
        public static Dictionary<uint, Vector<Texture>> InputTextureList { get; }

        public static Guid LoadTexture(String texturePath)
        {
            if (texturePath.IsEmpty())
            {
                return new Guid();
            }

            string jsonContent = File.ReadAllText(texturePath);
            TextureJsonLoader textureJson = JsonConvert.DeserializeObject<TextureJsonLoader>(jsonContent);
            TextureList[textureJson.TextureId] = GameEngineImport.Texture_LoadTexture(RenderSystem.renderer, texturePath);

            return textureJson.TextureId;
        }
    }
}
