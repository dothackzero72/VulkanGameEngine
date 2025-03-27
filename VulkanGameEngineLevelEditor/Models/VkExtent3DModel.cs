using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    [TypeConverter(typeof(VkExtent3DConverter))]
    public unsafe class VkExtent3DModel : RenderPassEditorBaseModel
    {
        public uint _width { get; set; }
        public uint _height { get; set; }
        public uint _depth { get; set; }

        public VkExtent3DModel()
        {
        }

        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("Image Size")]
        public uint width
        {
            get => _width;
            set
            {
                if (_width != value)
                {
                    _width = value;
                    OnPropertyChanged(nameof(width));
                }
            }
        }

        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("Image Size")]
        public uint height
        {
            get => _height;
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged(nameof(height));
                }
            }
        }

        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("Image Size")]
        public uint depth
        {
            get => _depth;
            set
            {
                if (_depth != value)
                {
                    _depth = value;
                    OnPropertyChanged(nameof(depth));
                }
            }
        }

        public VkExtent3DModel(uint? width = null, uint? height = null, uint? depth = null)
        {
            if (width.HasValue)
            {
                width = width.Value;
            }

            if (height.HasValue)
            {
                height = height.Value;
            }

            if (depth.HasValue)
            {
                depth = depth.Value;
            }
        }

        public VkExtent3DModel(Extent3D other)
        {
            _width = other.Width;
            _height = other.Height;
            _depth = other.Depth;
        }

        public override string ToString()
        {
            return $"{width}, {height}, {depth}";
        }

        public VkExtent3D Convert()
        {
            return new VkExtent3D
            {
                width = _width,
                height = _height,
                depth = _depth
            };
        }

        public VkExtent3D* ConvertPtr()
        {
            VkExtent3D* extent = (VkExtent3D*)Marshal.AllocHGlobal(sizeof(VkExtent3D));
            extent->width = _width;
            extent->height = _height;
            extent->depth = _depth;
            return extent;
        }

        public VkExtent3DDLL ToDLL()
        {
            fixed (byte* namePtr = System.Text.Encoding.UTF8.GetBytes(_name + "\0"))
            {
                return new VkExtent3DDLL
                {
                    Name = (IntPtr)namePtr,
                    _depth = _depth,
                    _width = _width,
                    _height = _height,
                };
            }
        }

        public void Dispose()
        {
            // Implement disposal logic if necessary
        }

        public class VkExtent3DConverter : ExpandableObjectConverter
        {
            private static readonly Regex _regex = new Regex(@"^\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*$", RegexOptions.Compiled);

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string str)
                {
                    var match = _regex.Match(str);
                    if (match.Success)
                    {
                        return new VkExtent3D
                        {
                            width = uint.Parse(match.Groups[1].Value),
                            height = uint.Parse(match.Groups[2].Value),
                            depth = uint.Parse(match.Groups[3].Value)
                        };
                    }
                }

                return base.ConvertFrom(context, culture, value);
            }
        }
    }
}
