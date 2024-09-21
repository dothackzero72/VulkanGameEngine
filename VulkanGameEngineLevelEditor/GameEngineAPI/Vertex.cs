using System.Collections.Generic;
using System.Runtime.InteropServices;
using VulkanGameEngineLevelEditor;

[StructLayout(LayoutKind.Sequential)]
public struct vec2
{
    public float x;
    public float y;

    public vec2(float x = 0.0f, float y = 0.0f)
    {
        this.x = x;
        this.y = y;
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct vec4
{
    public float r;
    public float g;
    public float b;
    public float a;

    public vec4(float r = 1.0f, float g = 1.0f, float b = 1.0f, float a = 1.0f)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }
}

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