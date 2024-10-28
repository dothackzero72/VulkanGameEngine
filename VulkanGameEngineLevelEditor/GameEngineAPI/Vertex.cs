using Silk.NET.Vulkan;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VulkanGameEngineLevelEditor;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexVec2
    {
        public float X;
        public float Y;

        public VertexVec2(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VertexVec3
    {
        public float X;
        public float Y;
        public float Z;

        public VertexVec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VertexVec4
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public VertexVec4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex2D
    {
        public VertexVec2 Position;
        public VertexVec2 UV;
        public VertexVec4 Color;

        public Vertex2D(VertexVec2 position, VertexVec2 uv, VertexVec4 color)
        {
            Position = position;
            UV = uv;
            Color = color;
        }

        public static List<VertexInputBindingDescription> GetBindingDescriptions()
        {
            var bindingDescriptions = new List<VertexInputBindingDescription>
    {
        new VertexInputBindingDescription
        {
            Binding = 0,
            Stride = (uint)Marshal.SizeOf(typeof(Vertex2D)),
            InputRate = VertexInputRate.Vertex
        }
    };

            return bindingDescriptions;
        }

        public static List<VertexInputAttributeDescription> GetAttributeDescriptions()
        {
            var attributeDescriptions = new List<VertexInputAttributeDescription>();

            attributeDescriptions.Add(new VertexInputAttributeDescription
            {
                Binding = 0,
                Location = 0, // Matches your shader's inPosition
                Format = Format.R32G32Sfloat,
                Offset = (uint)Marshal.OffsetOf(typeof(Vertex2D), nameof(Position)).ToInt32()
            });

            attributeDescriptions.Add(new VertexInputAttributeDescription
            {
                Binding = 0,
                Location = 1, // Matches your shader's inUV
                Format = Format.R32G32Sfloat,
                Offset = (uint)Marshal.OffsetOf(typeof(Vertex2D), nameof(UV)).ToInt32()
            });

            attributeDescriptions.Add(new VertexInputAttributeDescription
            {
                Binding = 0,
                Location = 2, // Matches your shader's inColor
                Format = Format.R32G32B32A32Sfloat,
                Offset = (uint)Marshal.OffsetOf(typeof(Vertex2D), nameof(Color)).ToInt32()
            });

            return attributeDescriptions;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex3D
    {
        public VertexVec3 Position;
        public VertexVec3 Color;
        public VertexVec2 UV;

        public Vertex3D(VertexVec3 position, VertexVec3 color, VertexVec2 uv)
        {
            Position = position;
            Color = color;
            UV = uv;

        }

        public static List<VertexInputBindingDescription> GetBindingDescriptions()
        {
            var bindingDescriptions = new List<VertexInputBindingDescription>
    {
        new VertexInputBindingDescription
        {
            Binding = 0,
            Stride = (uint)Marshal.SizeOf(typeof(Vertex3D)),
            InputRate = VertexInputRate.Vertex
        }
    };

            return bindingDescriptions;
        }

        public static List<VertexInputAttributeDescription> GetAttributeDescriptions()
        {
            var attributeDescriptions = new List<VertexInputAttributeDescription>();

            attributeDescriptions.Add(new VertexInputAttributeDescription
            {
                Binding = 0,
                Location = 0,
                Format = Format.R32G32B32Sfloat,
                Offset = (uint)Marshal.OffsetOf(typeof(Vertex3D), nameof(Position)).ToInt32()
            });

            attributeDescriptions.Add(new VertexInputAttributeDescription
            {
                Binding = 0,
                Location = 1,
                Format = Format.R32G32B32Sfloat,
                Offset = (uint)Marshal.OffsetOf(typeof(Vertex3D), nameof(Color)).ToInt32()
            });

            attributeDescriptions.Add(new VertexInputAttributeDescription
            {
                Binding = 0,
                Location = 2,
                Format = Format.R32G32Sfloat,
                Offset = (uint)Marshal.OffsetOf(typeof(Vertex3D), nameof(UV)).ToInt32()
            });

            return attributeDescriptions;
        }
    }
}