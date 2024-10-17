using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.RenderPassEditor
{
    public class RenderPassEditorConsts
    {
        public const string BasePath = $@"..\..\..\RenderPass\";
        public const string DefaultColorAttachmentDescriptionModel = $@"{BasePath}AttachmentDescription\DefaultColorAttachment.json";
        public const string DefaultDepthAttachmentDescriptionModel = $@"{BasePath}AttachmentDescription\DefaultDepthAttachment.json";
        public const string DefaultCreateColorImageInfo = $@"{BasePath}CreateImageInfo\DefaultColorTextureCreateInfo.json";
        public const string DefaultCreateDepthImageInfo = $@"{BasePath}CreateImageInfo\DefaultDepthTextureCreateInfo.json";
        public const string DefaultColorSamplerCreateInfo = $@"{BasePath}CreateSamplerInfo\DefaultColorSampleCreateInfo.json";
        public const string DefaultDepthSamplerCreateInfo = $@"{BasePath}CreateSamplerInfo\DefaultDepthSampleCreateInfo.json";
        public const string DefaultSubpassDependencyModel = $@"{BasePath}SubPassDependencies\DefaultSubpassDependency.json";

        public const string AttachmentDescriptionModelPath = $@"{BasePath}AttachmentDescription\";
        public const string CreateImageInfoPath = $@"{BasePath}CreateImageInfo\";
        public const string SamplerCreateInfoPath = $@"{BasePath}SamplerCreateInfo\";
        public const string SubpassDependencyModelPath = $@"{BasePath}SamplerCreateInfo\";
    }
}
