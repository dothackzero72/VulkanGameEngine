using GlmSharp;
using System.Runtime.InteropServices;
using Vulkan;


namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SpriteInstanceStruct
    {
        public vec2 SpritePosition;
        public vec4 UVOffset;
        public vec2 SpriteSize;
        public ivec2 FlipSprite;
        public vec4 Color;
        public mat4 InstanceTransform;
        public uint MaterialID;

        public SpriteInstanceStruct()
        {
            SpritePosition = new vec2(0.0f);
            UVOffset = new vec4(0.0f);
            SpriteSize = new vec2(0.0f);
            FlipSprite = new ivec2(0);
            Color = new vec4(0.0f);
            MaterialID = 0;
            InstanceTransform = mat4.Identity;
        }

        public SpriteInstanceStruct(vec2 spritePosition, vec4 uv, vec2 spriteSize, ivec2 flipSprite, vec4 color, uint materialID, mat4 instanceTransform, uint spriteLayer)
        {
            SpritePosition = spritePosition;
            UVOffset = uv;
            SpriteSize = spriteSize;
            FlipSprite = flipSprite;
            Color = color;
            MaterialID = materialID;
            InstanceTransform = instanceTransform;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct NullVertex
    {
        public NullVertex()
        {

        }

        public static ListPtr<VkVertexInputBindingDescription> GetBindingDescriptions()
        {
            return new ListPtr<VkVertexInputBindingDescription>();
        }

        public static ListPtr<VkVertexInputAttributeDescription> GetAttributeDescriptions()
        {
            return new ListPtr<VkVertexInputAttributeDescription>();
        }
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex2D
    {
        public vec2 Position;
        public vec2 UV;
        public vec4 Color;
        private vec2 vec21;
        private vec2 vec22;

        public Vertex2D(vec2 position, vec2 uv, vec4 color)
        {
            Position = position;
            UV = uv;
            Color = color;
        }

        public Vertex2D(vec2 vec21, vec2 vec22) : this()
        {
            this.vec21 = vec21;
            this.vec22 = vec22;
        }

        public static ListPtr<VkVertexInputBindingDescription> GetBindingDescriptions()
        {
            var bindingDescriptions = new ListPtr<VkVertexInputBindingDescription>
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

        public static ListPtr<VkVertexInputAttributeDescription> GetAttributeDescriptions()
        {
            var attributeDescriptions = new ListPtr<VkVertexInputAttributeDescription>();

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

        public static ListPtr<VkVertexInputBindingDescription> GetBindingDescriptions()
        {
            var bindingDescriptions = new ListPtr<VkVertexInputBindingDescription>
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

        public static ListPtr<VkVertexInputAttributeDescription> GetAttributeDescriptions()
        {
            var attributeDescriptions = new ListPtr<VkVertexInputAttributeDescription>();

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

    public unsafe struct SpriteInstanceVertex2D
    {
        public vec2 SpritePosition;
        public vec4 UVOffset;
        public vec2 SpriteSize;
        public ivec2 FlipSprite;
        public vec4 Color;
        public mat4 InstanceTransform;
        public uint MaterialID;

        public SpriteInstanceVertex2D()
        {
            SpritePosition = new vec2(0.0f);
            UVOffset = new vec4(0.0f);
            SpriteSize = new vec2(0.0f);
            FlipSprite = new ivec2(0);
            Color = new vec4(0.0f);
            MaterialID = 0;
            InstanceTransform = mat4.Identity;
        }

        public SpriteInstanceVertex2D(vec2 spritePosition, vec4 uv, vec2 spriteSize, ivec2 flipSprite, vec4 color, uint materialID, mat4 instanceTransform, uint spriteLayer)
        {
            SpritePosition = spritePosition;
            UVOffset = uv;
            SpriteSize = spriteSize;
            FlipSprite = flipSprite;
            Color = color;
            MaterialID = materialID;
            InstanceTransform = instanceTransform;
        }

        static public ListPtr<VkVertexInputBindingDescription> GetBindingDescriptions()
        {
            return new ListPtr<VkVertexInputBindingDescription>
            {
                   new VkVertexInputBindingDescription
                {
                    binding = 1,
                    stride = (uint)Marshal.SizeOf(typeof(SpriteInstanceVertex2D)),
                    inputRate = VkVertexInputRate.VK_VERTEX_INPUT_RATE_INSTANCE
                }
            };
        }

        static public ListPtr<VkVertexInputAttributeDescription> GetAttributeDescriptions()
        {
            uint offsetInstanceTransform = (uint)Marshal.OffsetOf(typeof(SpriteInstanceVertex2D), nameof(SpriteInstanceVertex2D.InstanceTransform));

            return new ListPtr<VkVertexInputAttributeDescription>
            {
                new VkVertexInputAttributeDescription
                {
                    location = 0,
                    binding = 1,
                    format = VkFormat.VK_FORMAT_R32G32_SFLOAT,
                    offset = (uint)Marshal.OffsetOf(typeof(SpriteInstanceVertex2D), nameof(SpritePosition)).ToInt32()
                },
                new VkVertexInputAttributeDescription
                {
                    location = 1,
                    binding = 1,
                    format = VkFormat.VK_FORMAT_R32G32B32A32_SFLOAT,
                    offset = (uint)Marshal.OffsetOf(typeof(SpriteInstanceVertex2D), nameof(UVOffset)).ToInt32()
                },
                new VkVertexInputAttributeDescription
                {
                    location = 2,
                    binding = 1,
                    format = VkFormat.VK_FORMAT_R32G32_SFLOAT,
                    offset = (uint)Marshal.OffsetOf(typeof(SpriteInstanceVertex2D), nameof(SpriteSize)).ToInt32()
                },
                new VkVertexInputAttributeDescription
                {
                    location = 3,
                    binding = 1,
                    format = VkFormat.VK_FORMAT_R32G32_SINT,
                    offset = (uint)Marshal.OffsetOf(typeof(SpriteInstanceVertex2D), nameof(FlipSprite)).ToInt32()
                },
                new VkVertexInputAttributeDescription
                {
                    location = 4,
                    binding = 1,
                    format = VkFormat.VK_FORMAT_R32G32B32A32_SFLOAT,
                    offset = (uint)Marshal.OffsetOf(typeof(SpriteInstanceVertex2D), nameof(Color)).ToInt32()
                },
                new VkVertexInputAttributeDescription
                {
                    location = 5,
                    binding = 1,
                    format = VkFormat.VK_FORMAT_R32G32B32A32_SFLOAT,
                    offset = (uint)Marshal.OffsetOf(typeof(SpriteInstanceVertex2D), nameof(InstanceTransform)).ToInt32()
                },
                new VkVertexInputAttributeDescription
                {
                    location = 6,
                    binding = 1,
                    format = VkFormat.VK_FORMAT_R32G32B32A32_SFLOAT,
                    offset = offsetInstanceTransform + (uint)sizeof(vec4)
                },
                new VkVertexInputAttributeDescription
                {
                    location = 7,
                    binding = 1,
                    format = VkFormat.VK_FORMAT_R32G32B32A32_SFLOAT,
                    offset = offsetInstanceTransform + (uint)sizeof(vec4) * 2
                },
                new VkVertexInputAttributeDescription
                {
                    location = 8,
                    binding = 1,
                    format = VkFormat.VK_FORMAT_R32G32B32A32_SFLOAT,
                    offset = offsetInstanceTransform + (uint)sizeof(vec4) * 3
                },
                new VkVertexInputAttributeDescription
                {
                    location = 9,
                    binding = 1,
                    format = VkFormat.VK_FORMAT_R32_UINT,
                    offset = (uint)Marshal.OffsetOf(typeof(SpriteInstanceVertex2D), nameof(MaterialID)).ToInt32()
                }
            };
        }
    }
}