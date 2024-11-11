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

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    [TypeConverter(typeof(VkExtent3DConverter))]
    public unsafe class VkExtent3D : RenderPassEditorBaseModel
    {
        public uint _width { get; set; }
        public uint _height { get; set; }
        public uint _depth { get; set; }

        public VkExtent3D()
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

        public VkExtent3D(uint? width = null, uint? height = null, uint? depth = null)
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

        public VkExtent3D(Extent3D other)
        {
            _width = other.Width;
            _height = other.Height;
            _depth = other.Depth;
        }

        public override string ToString()
        {
            return $"{width}, {height}, {depth}";
        }

        public Extent3D Convert()
        {
            return new Extent3D
            {
                Width = _width,
                Height = _height,
                Depth = _depth
            };
        }

        public Extent3D* ConvertPtr()
        {
            Extent3D* extent = (Extent3D*)Marshal.AllocHGlobal(sizeof(Extent3D));
            extent->Width = _width;
            extent->Height = _height;
            extent->Depth = _depth;
            return extent;
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
