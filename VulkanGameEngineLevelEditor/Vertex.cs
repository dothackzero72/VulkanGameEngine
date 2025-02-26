using GlmSharp;
using Silk.NET.Vulkan;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VulkanGameEngineGameObjectScripts.Vulkan;
using VulkanGameEngineLevelEditor;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex2D
    {
        public vec2 Position;
        public vec2 UV;
        public vec4 Color;

        public Vertex2D(vec2 position, vec2 uv, vec4 color)
        {
            Position = position;
            UV = uv;
            Color = color;
        }

        public static List<VkVertexInputBindingDescription> GetBindingDescriptions()
        {
            var bindingDescriptions = new List<VkVertexInputBindingDescription>
            {
                new VkVertexInputBindingDescription
                {
                    binding = 0,
                    stride = (uint)Marshal.SizeOf(typeof(Vertex2D)),
                    inputRate = VkVertexInputRate.VK_VERTEX_INPUT_RATE_VERTEX
                }
            };

            return bindingDescriptions;
        }

        public static List<VkVertexInputAttributeDescription> GetAttributeDescriptions()
        {
            var attributeDescriptions = new List<VkVertexInputAttributeDescription>();

            attributeDescriptions.Add(new VkVertexInputAttributeDescription
            {
                binding = 0,
                location = 0,
                format = VkFormat.VK_FORMAT_R32G32_SFLOAT,
                offset = (uint)Marshal.OffsetOf(typeof(Vertex2D), nameof(Position)).ToInt32()
            });

            attributeDescriptions.Add(new VkVertexInputAttributeDescription
            {
                binding = 0,
                location = 1,
                format = VkFormat.VK_FORMAT_R32G32_SFLOAT,
                offset = (uint)Marshal.OffsetOf(typeof(Vertex2D), nameof(UV)).ToInt32()
            });

            attributeDescriptions.Add(new VkVertexInputAttributeDescription
            {
                binding = 0,
                location = 2,
                format = VkFormat.VK_FORMAT_R32G32B32A32_SFLOAT,
                offset = (uint)Marshal.OffsetOf(typeof(Vertex2D), nameof(Color)).ToInt32()
            });

            return attributeDescriptions;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex3D
    {
        public vec3 Position;
        public vec3 Color;
        public vec2 UV;

        public Vertex3D(vec3 position, vec3 color, vec2 uv)
        {
            Position = position;
            Color = color;
            UV = uv;

        }

        public static List<VkVertexInputBindingDescription> GetBindingDescriptions()
        {
            var bindingDescriptions = new List<VkVertexInputBindingDescription>
            {
                new VkVertexInputBindingDescription
                {
                    binding = 0,
                    stride = (uint)Marshal.SizeOf(typeof(Vertex3D)),
                    inputRate = VkVertexInputRate.VK_VERTEX_INPUT_RATE_VERTEX
                }
            };

            return bindingDescriptions;
        }

        public static List<VkVertexInputAttributeDescription> GetAttributeDescriptions()
        {
            var attributeDescriptions = new List<VkVertexInputAttributeDescription>();

            attributeDescriptions.Add(new VkVertexInputAttributeDescription
            {
                binding = 0,
                location = 0,
                format = VkFormat.VK_FORMAT_R32G32B32A32_SFLOAT,
                offset = (uint)Marshal.OffsetOf(typeof(Vertex3D), nameof(Position)).ToInt32()
            });

            attributeDescriptions.Add(new VkVertexInputAttributeDescription
            {
                binding = 0,
                location = 1,
                format = VkFormat.VK_FORMAT_R32G32B32_SFLOAT,
                offset = (uint)Marshal.OffsetOf(typeof(Vertex3D), nameof(Color)).ToInt32()
            });

            attributeDescriptions.Add(new VkVertexInputAttributeDescription
            {
                binding = 0,
                location = 2,
                format = VkFormat.VK_FORMAT_R32G32_SFLOAT,
                offset = (uint)Marshal.OffsetOf(typeof(Vertex3D), nameof(UV)).ToInt32()
            });

            return attributeDescriptions;
        }
    }
}