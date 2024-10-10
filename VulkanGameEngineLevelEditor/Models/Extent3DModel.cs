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
    public class Extent3DModel
    {
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("Image Size")]
        public uint Width { get; set; }
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("Image Size")]
        public uint Height { get; set; }
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("Image Size")]
        public uint Depth { get; set; }

        public Extent3DModel()
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
            return String.Format("{0}, {1}, {2}", Width, Height, Depth);
        }

        public Extent3D ConvertToVulkan()
        {
            return new Extent3D()
            {
                Width = Width,
                Height = Height,
                Depth = Depth
            };
        }
    }

    public class Extent3DConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            const string regex = @"^\s*[\d]+(\s*,\s*[\d]+)*\s*$";
            var match = Regex.Match(value.ToString(), regex);
            if (match.Success)
            {
                Extent3D extent3D = new Extent3D();
                extent3D.Width = uint.Parse(match.Groups["Width"].Value);
                extent3D.Width = uint.Parse(match.Groups["Height"].Value);
                extent3D.Width = uint.Parse(match.Groups["Depth"].Value);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

}
