using System;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Vulkan;

public struct Texture
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

    public Texture()
    {
    }
};