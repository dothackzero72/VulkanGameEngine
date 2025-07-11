using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vulkan;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct TextureJsonLoader
    {
        public string TextureFilePath { get; set; }
        public Guid TextureId { get; set; }
        public VkFormat TextureByteFormat { get; set; }
        public VkImageAspectFlagBits ImageType { get; set; }
        public TextureTypeEnum TextureType { get; set; }
        public bool UseMipMaps { get; set; }
    }
}
