using Silk.NET.Core.Attributes;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    [TypeConverter(typeof(Extent3DConverter))]
    public class Extent3DModel : RenderPassEditorBaseModel
    {
        private uint _width;
        private uint _height;
        private uint _depth;

        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("Image Size")]
        public uint Width
        {
            get => _width;
            set
            {
                if (_width != value)
                {
                    _width = value;
                    OnPropertyChanged(nameof(Width));
                }
            }
        }

        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("Image Size")]
        public uint Height
        {
            get => _height;
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("Image Size")]
        public uint Depth
        {
            get => _depth;
            set
            {
                if (_depth != value)
                {
                    _depth = value;
                    OnPropertyChanged(nameof(Depth));
                }
            }
        }

        public Extent3DModel() : base()
        {
        }

        public Extent3DModel(string name) : base(name)
        {

        }

        public Extent3DModel(uint? width = null, uint? height = null, uint? depth = null)
        {
            if (width.HasValue)
            {
                Width = width.Value;
            }

            if (height.HasValue)
            {
                Height = height.Value;
            }

            if (depth.HasValue)
            {
                Depth = depth.Value;
            }
        }

        public override string ToString()
        {
            return $"{Width}, {Height}, {Depth}";
        }

        public Extent3D ConvertToVulkan()
        {
            return new Extent3D
            {
                Width = Width,
                Height = Height,
                Depth = Depth
            };
        }
    }

    public class Extent3DConverter : ExpandableObjectConverter
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
                    return new Extent3DModel
                    {
                        Width = uint.Parse(match.Groups[1].Value),
                        Height = uint.Parse(match.Groups[2].Value),
                        Depth = uint.Parse(match.Groups[3].Value)
                    };
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
